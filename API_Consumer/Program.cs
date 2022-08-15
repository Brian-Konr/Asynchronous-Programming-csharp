using CustomReportExtensions;
using CustomReportExtensions.Schemas;


class Program
{
    static void Main()
    {
        CustomReportHelper agent = new CustomReportHelper();
        CustomReportRequest request = new CustomReportRequest(
            dtno: 5493,
            ftno: 0,
            parameters: "AssignID=00878;AssignDate=20200101-99999999;DTOrder=2;MTPeriod=2;",
            keyMap: string.Empty,
            assignSpid: string.Empty
        );
        Task<QueryDelegateResponse?> pendingResponse = agent.PostCustomReport(request);
        QueryDelegateResponse? result = pendingResponse.Result;
        Console.WriteLine(result);
    }
}