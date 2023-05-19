using System.Text;
using Newtonsoft.Json;

namespace EquipmentCalculator;

public class CustomStringListConverter : Newtonsoft.Json.JsonConverter
{
    private const char _splitChar = ' ';

    public override bool CanConvert(Type objectType) => objectType == typeof(List<string>);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            return new List<string>();
        }

        string valueStr = (string)reader.Value;
        if (string.IsNullOrWhiteSpace(valueStr))
        {
            return new List<string>();
        }

        valueStr = valueStr.Trim();
        List<string> list = new List<string>();

        foreach (string value in valueStr.Split(_splitChar))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            list.Add((string)Convert.ChangeType(value.Trim(), typeof(string)));
        }

        return list;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null || value.GetType() != typeof(List<string>))
        {
            writer.WriteNull();
            return;
        }

        List<string> list = (List<string>)value;
        if (!list.Any())
        {
            writer.WriteNull();
            return;
        }

        StringBuilder writeValue = new StringBuilder();
        foreach (string data in list)
        {
            writeValue.Append(data);
            writeValue.Append(_splitChar);
        }

        string returnValue = writeValue.ToString().Substring(0, writeValue.Length - 1);
        writer.WriteValue(returnValue);
    }
}