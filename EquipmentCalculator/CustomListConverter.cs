using System.Text;
using Newtonsoft.Json;

namespace EquipmentCalculator;

public class CustomListConverter<T> : Newtonsoft.Json.JsonConverter where T : struct
{
    private const char _splitChar = ' ';

    public override bool CanConvert(Type objectType) => objectType == typeof(List<T>);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            return new List<T>();
        }

        string valueStr = (string)reader.Value;
        if (string.IsNullOrWhiteSpace(valueStr))
        {
            return new List<T>();
        }

        valueStr = valueStr.Trim();
        List<T> list = new List<T>();

        foreach (string value in valueStr.Split(_splitChar))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            list.Add((T)Convert.ChangeType(value.Trim(), typeof(T)));
        }

        return list;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null || value.GetType() != typeof(List<T>))
        {
            writer.WriteNull();
            return;
        }

        List<T> list = (List<T>)value;
        if (!list.Any())
        {
            writer.WriteNull();
            return;
        }

        StringBuilder writeValue = new StringBuilder();
        foreach (T data in list)
        {
            writeValue.Append(data);
            writeValue.Append(_splitChar);
        }

        string returnValue = writeValue.ToString().Substring(0, writeValue.Length - 1);
        writer.WriteValue(returnValue);
    }
}