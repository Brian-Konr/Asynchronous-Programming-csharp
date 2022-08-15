using CustomReportExtensions.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions
{
    public class MockCustomReportHelper
    {
        private readonly QueryDelegateResponse FakeResponse;

        /// <summary>
        /// 平均回應時間 (單位為秒)，必須要大於 0
        /// </summary>
        private readonly int AverageResponse;

        /// <summary>
        /// 最大同時請求總數，當接收超過最大請求總數會 throw exception 回去，此數值要大於 0
        /// </summary>
        private readonly int MaxConcurrentRequests;

        /// <summary>
        /// 提供外部存取目前此物件正在處理的 request 數量。
        /// 宣告成 field 是因為 Interlocked.Increment 不能放 property
        /// </summary>
        private int _CurrentRequestCount;


        public int CurrentRequestCount { get { return _CurrentRequestCount; } }

        public MockCustomReportHelper(int avgResponse, int maxRequest)
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
            MaxConcurrentRequests = maxRequest;
            _CurrentRequestCount = 0;
            FakeResponse = new QueryDelegateResponse(
                isCompleted: true,
                isFaulted: false,
                signature: "99.95",
                exception: string.Empty,
                result: "result"
            );
        }

        public async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            if (_CurrentRequestCount == MaxConcurrentRequests)
            {
                throw new InvalidOperationException("Current Requests have reached Maximum concurrent requests! Try call this method later.");
            }

            // 讓目前處理的數量 ++
            _CurrentRequestCount = Interlocked.Increment(ref _CurrentRequestCount);

            // 開始做隨機等待
            int randomWaitTime = new Random().Next(2 * AverageResponse);
            await Task.Delay(randomWaitTime * 1000);

            // 做完後將 CurrentRequestCount -1 並準備 return
            Interlocked.Decrement(ref _CurrentRequestCount);
            return FakeResponse;
        }
    }
}
