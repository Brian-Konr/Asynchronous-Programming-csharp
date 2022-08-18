using CustomReportExtensions.Schemas;
using System.Collections.Concurrent;

namespace CustomReportExtensions
{
   public class SmartDispatcher : ICustomReportHelper
    {
        private SemaphoreSlim Locker;
        private ConcurrentQueue<ICustomReportHelper> AvailableHelperQueue;

        public SmartDispatcher(List<ICustomReportHelper> helperList, int[] connectionCountArr)
        {
            int connectionCountSum = 0;
            AvailableHelperQueue = new ConcurrentQueue<ICustomReportHelper>();
            for (int i = 0; i < helperList.Count; i++)
            {
                for (int connectionCount = 0; connectionCount < connectionCountArr[i]; connectionCount++)
                {
                    connectionCountSum++;
                    AvailableHelperQueue.Enqueue(helperList[i]);
                }
            }
            Locker = new SemaphoreSlim(initialCount: connectionCountSum, maxCount: connectionCountSum);
        }

        public async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            // 先 wait async 直到有資源釋放再走下去 dequeue 拿資源
            await Locker.WaitAsync();
            if (AvailableHelperQueue.TryDequeue(out ICustomReportHelper? availableHelper))
            {
                // 資源先去做事 做完後重新 enqueue 並 release locker
                QueryDelegateResponse? response = await availableHelper.PostCustomReport(requestBody);
                AvailableHelperQueue.Enqueue(availableHelper);
                Locker.Release();
                return response;
            }
            Console.WriteLine("Something went wrong! Queue should always have at least one helper after Locker.WaitAsync()");
            return null;
        }
    }
}
