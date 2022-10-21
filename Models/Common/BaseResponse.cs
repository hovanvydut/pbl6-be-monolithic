using Newtonsoft.Json.Serialization;
using Monolithic.Constants;
using Newtonsoft.Json;

namespace Monolithic.Models.Common;

public class BaseResponse<T>
{
    public T Data { get; set; }

    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; }

    public BaseResponse() { }
    public BaseResponse(T Data,
                        int StatusCode = HttpCode.OK,
                        string Message = "",
                        bool Success = true)
    {
        this.Success = Success;
        this.StatusCode = StatusCode;
        this.Message = Message;
        this.Data = Data;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(
            this,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        );
    }
}