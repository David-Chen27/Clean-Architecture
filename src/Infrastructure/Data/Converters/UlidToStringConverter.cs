using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUlid;

namespace Clean_Architecture.Infrastructure.Data.Converters
{
    public class UlidToStringConverter : ValueConverter<Ulid, string>
    {
        public UlidToStringConverter() 
            : base(
                ulid => ulid.ToString(), 
                str => Ulid.Parse(str))
        {
        }
    }
}
