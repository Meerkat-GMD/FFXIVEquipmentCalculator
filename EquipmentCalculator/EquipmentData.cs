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
    public string P_Abbrv;
    public int P_Value;
    public string S_Abbrv;
    public int S_Value;
    public bool OverMeld; // True면 5개까지 가능
    public int Slots; // 기본 슬롯 값 
    public int MainStat;
    public int iLvl;

    [JsonIgnore] public int SPS => P_Abbrv == "SPS" ? P_Value : S_Abbrv == "SPS" ? S_Value : 0;
    [JsonIgnore] public int SKS => P_Abbrv == "SKS" ? P_Value : S_Abbrv == "SKS" ? S_Value : 0;

    public int GetTomeStone()
    {
        if (Name.Contains("Credendum"))
        {
            switch (ItemUICategory)
            {
                case ItemUICategory.Weapon:
                    return 500;
                case ItemUICategory.Head:
                case ItemUICategory.Hands:
                case ItemUICategory.Feet:
                    return 495;
                case ItemUICategory.Body:
                case ItemUICategory.Legs:
                    return 825;
                default:
                    return 375;
            }
        }
        
        return 0;
    }
}

public class EquipmentDataGroup
{
    public readonly List<EquipmentData> DataGroup = new();
}

public class CategoryEquipmentDataGroup
{
    public readonly Dictionary<ItemUICategory, List<EquipmentData>> CategoryDataGroup = new();
}


public class EquipmentDataManager
{
    private EquipmentData[] _rawParsingData;

    readonly string parsingPath = "../../../FF14_1.json";

    private readonly Dictionary<ClassJobCategory, EquipmentDataGroup> _classEquipmentDataDic = new(); // only class not job

    public EquipmentDataManager()
    {
        Parse();
    }
    
    private void Parse()
    {
        using (StreamReader file = File.OpenText(parsingPath))
        {
            string jsonRawData = file.ReadToEnd();
            _rawParsingData = JsonConvert.DeserializeObject<EquipmentData[]>(jsonRawData);
        }

        foreach (var data in _rawParsingData)
        {
            foreach (var cj in data.ClassJobCategory)
            {
                if (cj.IsJob())
                {
                    continue;
                }
                
                if(!_classEquipmentDataDic.TryGetValue(cj, out var equipmentDataGroup))
                {
                    equipmentDataGroup = new EquipmentDataGroup();
                    _classEquipmentDataDic.Add(cj, equipmentDataGroup);
                }
                
                equipmentDataGroup.DataGroup.Add(data);
            }
        }
    }

    public CategoryEquipmentDataGroup GetClassEquipmentData(ClassJobCategory className, int from, int to) 
    {
        if (!_classEquipmentDataDic.TryGetValue(className, out var equipmentDataGroup))
        {
            Console.WriteLine("장비 데이터 정보가 없습니다.");
            return new CategoryEquipmentDataGroup();
        }

        if (from > to)
        {
            (from, to) = (to, from);
        }

        var resultDataGroup = new CategoryEquipmentDataGroup();
        foreach (var equipmentData in equipmentDataGroup.DataGroup)
        {
            if (from <= equipmentData.iLvl && equipmentData.iLvl <= to)
            {
                if (!resultDataGroup.CategoryDataGroup.TryGetValue(equipmentData.ItemUICategory, out var list))
                {
                    list = new List<EquipmentData>();
                    resultDataGroup.CategoryDataGroup.Add(equipmentData.ItemUICategory, list);
                }
                
                list.Add(equipmentData);
            }
        }
        
        return resultDataGroup;
    }
}