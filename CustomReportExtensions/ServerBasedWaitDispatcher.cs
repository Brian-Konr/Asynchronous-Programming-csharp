using CustomReportExtensions.Schemas;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions
{
    public class ServerBasedWaitDispatcher : ICustomReportHelper
    {
        private ConcurrentQueue<(CustomReportRequest, TaskCompletionSource<QueryDelegateResponse?>)> Requests;
        private SemaphoreSlim RequestCounter;

        public ServerBasedWaitDispatcher(List<(ICustomReportHelper instance, int maxConcurrentRequest)> helperList)
        {
            Requests = new ConcurrentQueue<(CustomReportRequest, TaskCompletionSource<QueryDelegateResponse?>)>();
            RequestCounter = new SemaphoreSlim(0);

            // 開始讓 server 去試著 dequeue resource
            foreach ((ICustomReportHelper instance, int maxConcurrentRequest) in helperList)
            {
                // 每個 helper 都有一個自己的 max request counter
                SemaphoreSlim availableCounter = new (initialCount: maxConcurrentRequest, maxCount: maxConcurrentRequest);
                Task.Run(async () =>
                {
                    while (true)
                    {
                        // 先確認自己還有空間可以處理新的 request
                        await availableCounter.WaitAsync();
                        // 再確認還有 requests 等著被處理
                        await RequestCounter.WaitAsync();
                        // dequeue request 拿資源來做
                        if (Requests.TryDequeue(out (CustomReportRequest body, TaskCompletionSource<QueryDelegateResponse?> pendingResponse) todoTask))
                        {
                            todoTask.pendingResponse.SetResult(await instance.PostCustomReport(todoTask.body));
                        }
                        else
                        {
                            todoTask.pendingResponse.SetCanceled();
                        }
                        availableCounter.Release();
                    }
                });
            }
        }

        public async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            // enqueue 時要連同 taskcompletionsource 一起放進去，才能在 function 內被 setResult
            TaskCompletionSource<QueryDelegateResponse?> pendingResponse = new TaskCompletionSource<QueryDelegateResponse?>();
            Requests.Enqueue((requestBody, pendingResponse));
            RequestCounter.Release();
            return await pendingResponse.Task;
        }
    }
}
