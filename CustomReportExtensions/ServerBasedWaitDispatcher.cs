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

        public ServerBasedWaitDispatcher(List<(ICustomReportHelper instance, int maxConcurrentRequest, int helperID)> helperList)
        {
            Requests = new ConcurrentQueue<(CustomReportRequest, TaskCompletionSource<QueryDelegateResponse?>)>();
            RequestCounter = new SemaphoreSlim(0);

            // 開始讓 server 去試著 dequeue resource
            foreach ((ICustomReportHelper instance, int maxConcurrentRequest, int helperID) in helperList)
            {
                /* maxConcurrentRequest 代表一個 server 可以一次接的量，所以這邊就 for loop 出這麼多的量
                 * 然後每個都去等 request queue 的東西
                 */
                for (int i = 0; i < maxConcurrentRequest; i++)
                {
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            // 再確認還有 requests 等著被處理
                            await RequestCounter.WaitAsync();
                            // dequeue request 拿資源來做
                            if (Requests.TryDequeue(out (CustomReportRequest body, TaskCompletionSource<QueryDelegateResponse?> pendingResponse) todoTask))
                            {
                                Console.WriteLine($"request consumed by helper {helperID}");
                                try
                                {
                                    todoTask.pendingResponse.SetResult(await instance.PostCustomReport(todoTask.body));
                                }
                                catch (Exception ex)
                                {
                                    todoTask.pendingResponse.SetException(ex);
                                }
                            }
                        }
                    });
                }
            }
        }

        public Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            // enqueue 時要連同 taskcompletionsource 一起放進去，才能在 function 內被 setResult
            TaskCompletionSource<QueryDelegateResponse?> pendingResponse = new TaskCompletionSource<QueryDelegateResponse?>();
            Requests.Enqueue((requestBody, pendingResponse));
            RequestCounter.Release();
            return pendingResponse.Task;
        }
    }
}
