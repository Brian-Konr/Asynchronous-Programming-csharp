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
        MockCustomReportHelper helper1 = new MockCustomReportHelper(avgResponse: 5, maxRequest: 1);
        MockCustomReportHelper helper2 = new MockCustomReportHelper(avgResponse: 2, maxRequest: 3);
        MockCustomReportHelper helper3 = new MockCustomReportHelper(avgResponse: 1, maxRequest: 3);
        List<(ICustomReportHelper instance, int maxRequest, int helperID)> helperList = new()
        {
            (helper1, 1, 1),
            (helper2, 2, 2),
            (helper3, 3, 3)
        };
        ICustomReportHelper helper = new ServerBasedWaitDispatcher(helperList);
        Task<QueryDelegateResponse?>[] taskArr = new Task<QueryDelegateResponse?>[100];
        for (int i = 0; i < 100; i++)
        {
            taskArr[i] = helper.PostCustomReport(SampleRequest);
        }
        Task.WhenAll(taskArr).Wait();
    }
}