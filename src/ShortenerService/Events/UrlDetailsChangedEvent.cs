using DispatchR.Requests.Notification;

namespace ShortenerService.Events;
public sealed record UrlDetailsChangedEvent(string shortenCode, string longUrl) : INotification;
