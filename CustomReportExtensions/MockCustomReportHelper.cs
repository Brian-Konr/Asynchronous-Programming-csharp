using CustomReportExtensions.Schemas;

namespace CustomReportExtensions
{
    public class MockCustomReportHelper : ICustomReportHelper
    {
        private readonly QueryDelegateResponse FakeResponse;

        /// <summary>
        /// 平均回應時間 (單位為秒)，必須要大於 0
        /// </summary>
        private readonly int AverageResponse;

        /// <summary>
        /// 最大同時請求總數，當接收超過最大請求總數會 throw exception 回去，此數值要大於 0
        /// </summary>
        protected readonly int MaxConcurrentRequests;

        /// <summary>
        /// 紀錄目前此物件正在處理的 request 數量。
        /// 宣告成 field 是因為 Interlocked.Increment 不能放 property
        /// </summary>
        protected int _CurrentRequestCount;

        /// <summary>
        /// 提供外界 access _CurrentRequestCount 的媒介
        /// </summary>
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
            _CurrentRequestCount = 0;
            MaxConcurrentRequests = maxRequest;
            AverageResponse = avgResponse;
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
            if (Interlocked.Increment(ref _CurrentRequestCount) > MaxConcurrentRequests)
            {
                Interlocked.Decrement(ref _CurrentRequestCount);
                throw new InvalidOperationException("Current Requests have reached Maximum concurrent requests! Try call this method later.");
            }

            // 開始做平均時間的等待
            await Task.Delay(AverageResponse * 1000);

            // 做完後將 CurrentRequestCount -1 並準備 return
            Interlocked.Decrement(ref _CurrentRequestCount);
            return FakeResponse;
        }
    }
}
