using Newtonsoft.Json;

namespace EquipmentCalculator;

public class CustomIntConverter : JsonConverter<int>
{
    public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String && string.IsNullOrEmpty((string)reader.Value))
        {
            return 0;
        }
        else if (reader.TokenType == JsonToken.Null)
        {
            return 0;
        }
        return Convert.ToInt32(reader.Value);
    }

    public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}