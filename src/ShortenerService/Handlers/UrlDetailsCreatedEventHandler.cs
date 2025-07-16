using DispatchR.Requests.Notification;
using ShortenerService.Events;
using ShortenerService.Services;

namespace ShortenerService.Handlers;

public sealed class UrlDetailsCreatedEventHandler(RedisService redisService) : INotificationHandler<UrlDetailsChangedEvent>
{
    public async ValueTask Handle(UrlDetailsChangedEvent notification, CancellationToken cancellationToken)
    {
        // add to redis
        await redisService.SetUrl(notification.shortenCode, notification.longUrl);
    }
}
