using Clean_Architecture.Domain.Events;
using Microsoft.Extensions.Logging;
using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Application.Authorization.Permissions.EventHandlers;

public class PermissionsCreatedEventHandler : INotificationHandler<PermissionsCreatedEvent>
{
    private readonly ILogger<PermissionsCreatedEventHandler> _logger;
    
    private readonly IApplicationDbContext _context;

    public PermissionsCreatedEventHandler(ILogger<PermissionsCreatedEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(PermissionsCreatedEvent notification, CancellationToken cancellationToken)
    {
        
        _logger.LogInformation("Clean_Architecture Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
