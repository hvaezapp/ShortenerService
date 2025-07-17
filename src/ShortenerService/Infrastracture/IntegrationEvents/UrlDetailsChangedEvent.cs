using DispatchR.Requests.Notification;

namespace ShortenerService.Infrastracture.IntegrationEvents;
public sealed record UrlDetailsChangedEvent(string shortenCode, string longUrl) : INotification;
