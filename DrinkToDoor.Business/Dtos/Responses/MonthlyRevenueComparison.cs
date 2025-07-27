
namespace DrinkToDoor.Business.Dtos.Responses
{
    public class MonthlyRevenueComparison
    {
        public int Month { get; set; } 
        public decimal Revenue { get; set; }
        public decimal? PercentageChange { get; set; } 
    }
}