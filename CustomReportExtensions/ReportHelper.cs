using CustomReportExtensions.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions
{
    public abstract class ReportHelper
    {
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

        public abstract Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody);

        public ReportHelper(int macConcurrentRequests)
        {
            MaxConcurrentRequests = macConcurrentRequests;
            _CurrentRequestCount = 0;
        }
    }
}
