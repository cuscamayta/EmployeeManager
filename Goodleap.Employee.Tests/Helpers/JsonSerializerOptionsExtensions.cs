using System.Text.Json.Serialization;
using System.Text.Json;

namespace Goodleap.Employee.Tests
{
    public static class JsonSerializerOptionsExtensions
    {
        public static JsonSerializerOptions Setup(this JsonSerializerOptions options, bool addStringEnumConverter = true, bool addDateTimeOffsetConverter = true, bool addTimeSpanConverter = true, bool addDateTimeConverter = true)
        {
            options.PropertyNameCaseInsensitive = true;
            if (addStringEnumConverter)
            {
                options.Converters.Add(new JsonStringEnumConverter());
            }

            return options;
        }
    }
}
