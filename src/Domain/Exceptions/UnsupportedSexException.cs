namespace Clean_Architecture.Domain.Exceptions;

public class UnsupportedSexException : Exception
{
    public UnsupportedSexException(string sex)
        : base($"Sex \"{sex}\" is unsupported.")
    {
    }
}
