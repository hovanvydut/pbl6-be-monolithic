using Newtonsoft.Json;

namespace Monolithic.Models.Common;

public class BaseResponse<T>
{
    public T Data { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; }

    public BaseResponse() { }
    public BaseResponse(T Data, int StatusCode, string Message)
    {
        this.Data = Data;
        this.StatusCode = StatusCode;
        this.Message = Message;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}