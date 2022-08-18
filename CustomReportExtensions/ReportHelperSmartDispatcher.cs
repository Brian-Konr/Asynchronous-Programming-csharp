using CustomReportExtensions.Schemas;
using System.Collections.Concurrent;

namespace CustomReportExtensions
{
   public class ReportHelperSmartDispatcher : ICustomReportHelper
    {
        private int[] ConnectionCountArr;
        // queue 要記 helper instance 以及他的編號 (讓 ConnectionCountArr 可以找到對應 helper 做資源加減)
        private ConcurrentQueue<(ICustomReportHelper, int)> AvailableHelperQueue;
        private readonly SemaphoreSlim Locker;


        public ReportHelperSmartDispatcher(List<ICustomReportHelper> helperList, int[] connectionCountArr)
        {
            AvailableHelperQueue = new ConcurrentQueue<(ICustomReportHelper, int)>();
            ConnectionCountArr = connectionCountArr;
            int helperCount = helperList.Count;
            for (int i = 0; i < helperCount; i++)
            {
                AvailableHelperQueue.Enqueue((helperList[i], i));
            }
            Locker = new SemaphoreSlim(initialCount: helperCount, maxCount: helperCount);
        }

        public async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            // 先 WaitAsync 確認 SemaphoreSlim 有資源可以拿
            await Locker.WaitAsync();
            AvailableHelperQueue.TryDequeue(out (ICustomReportHelper helper, int helperID) resource);
            if (Interlocked.Decrement(ref ConnectionCountArr[resource.helperID]) == 0)
            {
                QueryDelegateResponse? response = await resource.helper.PostCustomReport(requestBody);
                AvailableHelperQueue.Enqueue(resource);
                Interlocked.Increment(ref ConnectionCountArr[resource.helperID]);
                Locker.Release();
                return response;
            }
            else 
            {
                AvailableHelperQueue.Enqueue(resource);
                Locker.Release();
                QueryDelegateResponse? response = await resource.helper.PostCustomReport(requestBody);
                Interlocked.Increment(ref ConnectionCountArr[resource.helperID]);
                return response;
            }
        }
    }
}
