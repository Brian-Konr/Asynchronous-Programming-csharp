using CustomReportExtensions.Schemas;

namespace CustomReportExtensions
{
    public interface ICustomReportHelper
    {
        Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody);
    }
}
