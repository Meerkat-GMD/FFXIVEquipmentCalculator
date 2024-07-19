using Newtonsoft.Json;

namespace EquipmentCalculator;

public class FoodData
{
    public string Name;
    [JsonConverter(typeof(CustomIntConverter))]
    public int VIT = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int VITCap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int DIR = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int DIRCap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int CRT = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int CRTCap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int DET = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int DETCap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int SKS = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int SKSCap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int SPS = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int SPSCap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int PIE = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int PIECap = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int TEN = 0;
    [JsonConverter(typeof(CustomIntConverter))]
    public int TENCap = 0;
}