using System.Text.Json;

namespace Monolithic.Common;

public class LogUtil
{
    public static void logJson(Object o)
    {
        string jsonString = JsonSerializer.Serialize(o);
        Console.WriteLine(jsonString);
    }
}