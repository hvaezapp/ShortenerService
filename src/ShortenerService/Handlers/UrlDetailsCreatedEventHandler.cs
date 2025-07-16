using DispatchR.Requests.Notification;
using ShortenerService.Events;
using ShortenerService.Services;

namespace ShortenerService.Handlers;

public sealed class UrlDetailsCreatedEventHandler(RedisService redisService) : INotificationHandler<UrlDetailsCreatedEvent>
{
    public async ValueTask Handle(UrlDetailsCreatedEvent notification, CancellationToken cancellationToken)
    {
        // add to redis
        await redisService.SetUrlDeatils(notification.shortenCode, notification.longUrl);
    }
}
