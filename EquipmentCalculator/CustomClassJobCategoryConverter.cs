using Newtonsoft.Json;

namespace EquipmentCalculator;

public class CustomClassJobCategoryConverter : JsonConverter
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
            return ClassJobCategory.Error;
        }
        
        string valueStr = (string)reader.Value;
        if (string.IsNullOrWhiteSpace(valueStr))
        {
            return ClassJobCategory.Error;
        }
        
        valueStr = valueStr.Trim();

        if(Enum.TryParse<ClassJobCategory>(valueStr, false, out var itemCategory))
        {
            return itemCategory;
        }
        else
        {
            return ClassJobCategory.Error;
        }
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(ClassJobCategory);
}