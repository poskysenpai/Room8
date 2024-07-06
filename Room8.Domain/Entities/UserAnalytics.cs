namespace Room8.Domain.Entities
{
    public class UserAnalytics
    {
        public int AnalyticsId { get; set; }
        public int VisitingCustomers { get; set; }
        public int AverageUsageTime { get; set; }
        public int AgeDemographic { get; set; }
        public int GenderDemographic { get; set; }
        public int LocationDemographic { get; set; } 

    }
}
