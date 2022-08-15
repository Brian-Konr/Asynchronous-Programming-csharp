using CustomReportExtensions;
using CustomReportExtensions.Schemas;


class Program
{
    static void Main()
    {
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
}