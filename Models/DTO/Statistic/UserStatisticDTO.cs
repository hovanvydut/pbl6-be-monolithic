namespace Monolithic.Models.DTO
{
    public class UserStatisticDTO
    {
        public IEnumerable<UserStatisticDetail> Users { get; set; }

        public double StatisticValue { get; set; }

        public string StatisticDate { get; set; }
    }

    public class UserStatisticDetail
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public double StatisticValue { get; set; }
    }
}