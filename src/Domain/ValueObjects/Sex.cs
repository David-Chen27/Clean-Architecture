namespace Clean_Architecture.Domain.ValueObjects;

public class Sex(string code) : ValueObject
{
    public static Sex From(string code)
    {
        var sex = new Sex(code);

        if (!SupportedSexes.Contains(sex))
        {
            throw new UnsupportedSexException(code);
        }

        return sex;
    }

    public static IReadOnlyCollection<Sex> GetSupportedStatuses()
    {
        return SupportedSexes.ToList().AsReadOnly();
    }
    
    public static Sex Null => new("Null");
    public static Sex Male => new("Male"); 
    public static Sex Female => new("Female");

    public string Code { get; private set; } = string.IsNullOrWhiteSpace(code) ? throw new UnsupportedSexException(code) : code;

    public static implicit operator string(Sex sex)
    {
        return sex.ToString();
    }

    public static explicit operator Sex(string pharmacyStatus)
    {
        return From(pharmacyStatus);
    }

    public override string ToString()
    {
        return Code;
    }

    protected static IEnumerable<Sex> SupportedSexes
    {
        get
        {
            yield return Null;
            yield return Male;
            yield return Female;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
