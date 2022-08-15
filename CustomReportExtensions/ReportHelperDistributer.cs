using CustomReportExtensions.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions
{
    public class ReportHelperDistributer
    {
        private readonly Random RandomGenerator;
        private readonly List<ReportHelper> HelperList;
        public ReportHelperDistributer(List<ReportHelper> helperList)
        {
            HelperList = helperList;
            RandomGenerator = new Random();
        }

        public Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            int randomIndex = RandomGenerator.Next(0, HelperList.Count - 1);
            return HelperList[randomIndex].PostCustomReport(requestBody);
        }

        public List<int> GetAllCurrentRequests()
        {
            return new List<int>
            {
                HelperList[0].CurrentRequestCount,
                HelperList[1].CurrentRequestCount,
                HelperList[2].CurrentRequestCount
            };
        }
    }
}
