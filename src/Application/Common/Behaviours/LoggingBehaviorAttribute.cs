namespace Clean_Architecture.Application.Common.Behaviours;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class LoggingBehaviorAttribute: Attribute
{
    public LoggingBehaviorAttribute(){ }
    
    public string[] mask { get; set; } = Array.Empty<string>();
}
