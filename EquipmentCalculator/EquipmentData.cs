using Newtonsoft.Json;

namespace EquipmentCalculator;


public class EquipmentData
{
    public string Name;
    [JsonConverter(typeof(CustomItemUICategoryConverter))]
    public ItemUICategory ItemUICategory;

    [JsonConverter(typeof(CustomEnumListConverter<ClassJobCategory>))]
    public List<ClassJobCategory> ClassJobCategory;
    public int CRT;
    public int DET;
    public int DIR;
    public int TEN;
    public int PIE;
    public bool OverMeld;
}