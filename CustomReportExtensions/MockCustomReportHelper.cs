using CustomReportExtensions.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions
{
    public class MockCustomReportHelper : ReportHelper
    {
        private readonly QueryDelegateResponse FakeResponse;

        /// <summary>
        /// 平均回應時間 (單位為秒)，必須要大於 0
        /// </summary>
        private readonly int AverageResponse;

        public MockCustomReportHelper(int avgResponse, int maxRequest) : base(maxRequest)
        {
            if (avgResponse < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(avgResponse));
            }
            if (maxRequest < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRequest)); 
            }

            AverageResponse = avgResponse;
            FakeResponse = new QueryDelegateResponse(
                isCompleted: true,
                isFaulted: false,
                signature: "99.95",
                exception: string.Empty,
                result: "result"
            );
        }

        public override async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            if (_CurrentRequestCount == MaxConcurrentRequests)
            {
                throw new InvalidOperationException("Current Requests have reached Maximum concurrent requests! Try call this method later.");
            }

            /* 讓目前處理的數量 ++
             * 要使用 Interlocked 是因為在面對 multi-thread 修改 shared variables 時要 block 住
             */
            Interlocked.Increment(ref _CurrentRequestCount);

            // 開始做隨機等待
            int randomWaitTime = new Random().Next(-AverageResponse, AverageResponse);
            Console.WriteLine($"random wait time: {AverageResponse * 1000 + 300 * randomWaitTime}");
            await Task.Delay(AverageResponse * 1000 + 300 * randomWaitTime);

            // 做完後將 CurrentRequestCount -1 並準備 return
            Interlocked.Decrement(ref _CurrentRequestCount);
            return FakeResponse;
        }
    }
}
