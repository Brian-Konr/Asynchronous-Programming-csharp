using CustomReportExtensions;
using CustomReportExtensions.Schemas;


class Program
{
    private static CustomReportRequest SampleRequest;

    static Program()
    {
        SampleRequest = new CustomReportRequest(
            dtno: 5493,
            ftno: 0,
            parameters: "AssignID=00878;AssignDate=20200101-99999999;DTOrder=2;MTPeriod=2;",
            keyMap: string.Empty,
            assignSpid: string.Empty
        );
    }
    static void Main()
    {
        DoPhase4();
        return;
        //CustomReportHelper agent = new CustomReportHelper();
        MockCustomReportHelper agent = new MockCustomReportHelper(avgResponse: 3, maxRequest: 5);
        CustomReportRequest request = new CustomReportRequest(
            dtno: 5493,
            ftno: 0,
            parameters: "AssignID=00878;AssignDate=20200101-99999999;DTOrder=2;MTPeriod=2;",
            keyMap: string.Empty,
            assignSpid: string.Empty
        );

        List<Task<QueryDelegateResponse?>> pendingResponseArr = new List<Task<QueryDelegateResponse?>>();
        for (int i = 0; i < 100; i++)
        {
            pendingResponseArr.Add(agent.PostCustomReport(request));
        }
        int successCount = 0;
        int failureCount = 0;
        foreach (Task<QueryDelegateResponse?> task in pendingResponseArr)
        {
            try
            {
                QueryDelegateResponse? response = task.Result;
                successCount++;
            }
            catch (Exception)
            {
                failureCount++;
            }
        }
        Console.WriteLine($"success: {successCount}, failure: {failureCount}");
        //Task<QueryDelegateResponse?> pendingResponse = agent.PostCustomReport(request);
        //QueryDelegateResponse? result = pendingResponse.Result;
    }
    private static void DoPhase4()
    {
        MockCustomReportHelper helper1 = new MockCustomReportHelper(avgResponse: 1, maxRequest: 100);
        MockCustomReportHelper helper2 = new MockCustomReportHelper(avgResponse: 1, maxRequest: 80);
        MockCustomReportHelper helper3 = new MockCustomReportHelper(avgResponse: 1, maxRequest: 8);
        List<ICustomReportHelper> reportHelperList = new List<ICustomReportHelper> { helper1, helper2, helper3 };
        //ReportHelperRandomDispatcher distributer = new ReportHelperRandomDispatcher(reportHelperList);
        ICustomReportHelper helper = new ReportHelperSmartDispatcher(reportHelperList, new int[] { 100, 80, 8 });
        Task<QueryDelegateResponse?>[] taskArr = new Task<QueryDelegateResponse?>[1000];
        for (int i = 0; i < 1000; i++)
        {
            taskArr[i] = helper.PostCustomReport(SampleRequest);
        }
        Task.WhenAll(taskArr).Wait();
    }
}