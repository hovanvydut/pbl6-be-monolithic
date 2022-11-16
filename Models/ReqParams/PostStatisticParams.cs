using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams
{
    public class PostStatisticParams
    {
        public string Key { get; set; }

        public string PostIds { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IncludeDeletedPost { get; set; }
    }

    public class PostStatisticInDateParams : ReqParam
    {
        public string Key { get; set; }

        public DateTime Date { get; set; }

        public bool IncludeDeletedPost { get; set; }
    }

    public class PostTopStatisticParams
    {
        public int Top { get; set; }

        public string Key { get; set; }

        public DateTime Date { get; set; }

        public bool IncludeDeletedPost { get; set; }
    }
}