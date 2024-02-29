namespace WebApplication1.Models
{
    [Serializable]
    public class DashboardData
    {
        public int TotalCustomerCount { get; set; }
        public int TotalRevenue { get; set; }
        public string TopSellingProductname { get; set; }
        public string TopSellingCountryName { get; set; }
        
    }
}
