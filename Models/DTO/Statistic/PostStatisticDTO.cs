namespace Monolithic.Models.DTO
{
    public class PostStatisticDTO
    {
        public IEnumerable<PostStatisticDetail> Posts { get; set; }

        public string StatisticDate { get; set; }
    }

    public class PostStatisticDetail
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public double StatisticValue { get; set; }
    }
}