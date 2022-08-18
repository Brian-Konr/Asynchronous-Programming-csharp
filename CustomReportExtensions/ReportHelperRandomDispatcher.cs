﻿using CustomReportExtensions.Schemas;

namespace CustomReportExtensions
{
    public class ReportHelperRandomDispatcher : ICustomReportHelper
    {
        private readonly Random RandomGenerator;
        private readonly List<ICustomReportHelper> HelperList;
        public ReportHelperRandomDispatcher(List<ICustomReportHelper> helperList)
        {
            HelperList = helperList;
            RandomGenerator = new Random();
        }

        public Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            int randomIndex = RandomGenerator.Next(0, HelperList.Count - 1);
            return HelperList[randomIndex].PostCustomReport(requestBody);
        }
    }
}
