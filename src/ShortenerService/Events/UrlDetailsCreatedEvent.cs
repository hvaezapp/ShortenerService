using DispatchR.Requests.Notification;

namespace ShortenerService.Events;
public sealed record UrlDetailsCreatedEvent(string shortenCode, string longUrl) : INotification;
