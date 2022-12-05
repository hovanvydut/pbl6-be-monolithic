using Newtonsoft.Json;

namespace Monolithic.Common;

public class LogContent
{
    public string Path { get; set; }

    public string Method { get; set; }

    public string Params { get; set; }

    public string Message { get; set; }

    public int StatusCode { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}