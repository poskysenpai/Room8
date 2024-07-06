namespace Room8.Domain.Entities
{
    public class ProductMetrics
    {
        public int ProductMetricsId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public int DailyActiveUsers { get; set; }
        public int MonthlyActiveUsers { get; set; }
        public Dictionary<string, int> FeatureUsage { get; set; } = new Dictionary<string, int>();
        public int NetPromoterScore { get; set; }
        public int CustomerSatisfaction { get; set; } // In-app star rating
    }
}
