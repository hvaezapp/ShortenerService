using DispatchR.Requests.Notification;
using ShortenerService.Infrastracture.IntegrationEvents;
using ShortenerService.Services;

namespace ShortenerService.Infrastracture.Consumers;

public sealed class UrlDetailsCreatedEventConsumer(RedisService redisService) : INotificationHandler<UrlDetailsChangedEvent>
{
    public async ValueTask Handle(UrlDetailsChangedEvent notification, CancellationToken cancellationToken)
    {
        // add to redis
        await redisService.SetUrl(notification.shortenCode, notification.longUrl);
    }
}
