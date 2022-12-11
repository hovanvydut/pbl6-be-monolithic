using Savorboard.CAP.InMemoryMessageQueue;

namespace Monolithic.Extensions;

public static class MessageQueueExtension
{
    public static void ConfigureCapQueue(this IServiceCollection services)
    {
        services.AddCap(capOptions =>
        {
            capOptions.UseInMemoryStorage();
            capOptions.UseInMemoryMessageQueue();
        });
    }
}