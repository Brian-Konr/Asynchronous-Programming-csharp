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
        DoPhase3();
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
    private static void DoPhase3()
    {
        MockCustomReportHelper helper1 = new MockCustomReportHelper(avgResponse: 5, maxRequest: 5);
        CustomReportHelper helper2 = new CustomReportHelper(maxRequest: 5);
        MockCustomReportHelper helper3 = new MockCustomReportHelper(avgResponse: 3, maxRequest: 8);
        List<ReportHelper> reportHelperList = new List<ReportHelper> { helper1, helper2, helper3 };
        ReportHelperDistributer distributer = new ReportHelperDistributer(reportHelperList);
        for (int i = 0; i < 100; i++)
        {
            List<int> currentRequest = distributer.GetAllCurrentRequests();
            Console.WriteLine($"First Helper current count: {currentRequest[0]}");
            Console.WriteLine($"Second Helper current count: {currentRequest[1]}");
            Console.WriteLine($"Third Helper current count: {currentRequest[2]}");
            distributer.PostCustomReport(SampleRequest);
        }
        Console.ReadLine();
    }
}