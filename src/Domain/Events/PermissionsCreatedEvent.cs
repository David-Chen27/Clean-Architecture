namespace Clean_Architecture.Domain.Events;

public class PermissionsCreatedEvent : BaseEvent
{
    public PermissionsCreatedEvent(List<Permission> items)
    {
        Items = items;
    }

    public List<Permission> Items { get; }
}
