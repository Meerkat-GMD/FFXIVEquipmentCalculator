using Newtonsoft.Json;

namespace EquipmentCalculator;

public class CustomStatCategoryConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(value);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            return StatCategory.CRT;
        }
        
        string valueStr = (string)reader.Value;
        if (string.IsNullOrWhiteSpace(valueStr))
        {
            return StatCategory.CRT;
        }
        
        valueStr = valueStr.Trim();

        if(Enum.TryParse<StatCategory>(valueStr, false, out var itemCategory))
        {
            return itemCategory;
        }
        else
        {
            return StatCategory.CRT;
        }
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(StatCategory);
}