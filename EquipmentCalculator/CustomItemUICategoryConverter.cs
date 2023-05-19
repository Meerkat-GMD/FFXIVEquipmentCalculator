using Newtonsoft.Json;

namespace EquipmentCalculator;

public class CustomItemUICategoryConverter : JsonConverter
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
            return ItemUICategory.Weapon;
        }
        
        string valueStr = (string)reader.Value;
        if (string.IsNullOrWhiteSpace(valueStr))
        {
            return ItemUICategory.Weapon;
        }
        
        valueStr = valueStr.Trim();

        if(Enum.TryParse<ItemUICategory>(valueStr, false, out var itemCategory))
        {
            return itemCategory;
        }
        else
        {
            return ItemUICategory.Weapon;
        }
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(ItemUICategory);
}