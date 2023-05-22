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
    public bool OverMeld; // True면 5개까지 가능
    public int Slots; // 기본 슬롯 값 
    public int MainStat;
}