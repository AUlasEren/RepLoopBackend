using MassTransit;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Infrastructure.Messaging;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
        => _publishEndpoint.Publish(@event, cancellationToken);
}
