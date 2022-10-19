namespace Monolithic.Models.Common;

public class BaseException : Exception
{
    public int StatusCode { get; set; }

    public BaseException() { }

    public BaseException(int StatusCode,
                         string Message = "") : base(Message)
    {
        this.StatusCode = StatusCode;
    }
}