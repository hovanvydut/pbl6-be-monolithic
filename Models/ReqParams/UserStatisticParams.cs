using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams
{
    public class UserStatisticParams
    {
        public string Key { get; set; }

        public string UserIds { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class UserStatisticInDateParams : ReqParam
    {
        public string Key { get; set; }

        public DateTime Date { get; set; }
    }

    public class UserTopStatisticParams
    {
        public int Top { get; set; }

        public string Key { get; set; }

        public DateTime Date { get; set; }
    }
}