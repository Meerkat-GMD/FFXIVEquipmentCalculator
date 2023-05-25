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
    public int Level;
}

public class EquipmentDataGroup 
{
    public readonly List<EquipmentData> DataGroup = new();
}

public class EquipmentDataManager
{
    private EquipmentData[] _rawParsingData;

    readonly string parsingPath = "../../../FF14.json";

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

    public EquipmentDataGroup GetClassEquipmentData(ClassJobCategory className, int from, int to) 
    {
        if (!_classEquipmentDataDic.TryGetValue(className, out var equipmentDataGroup))
        {
            Console.WriteLine("장비 데이터 정보가 없습니다.");
            return new EquipmentDataGroup();
        }

        if (from > to)
        {
            (from, to) = (to, from);
        }

        var resultDataGroup = new EquipmentDataGroup();
        foreach (var equipmentData in equipmentDataGroup.DataGroup)
        {
            if (from <= equipmentData.Level && equipmentData.Level <= to)
            {
                resultDataGroup.DataGroup.Add(equipmentData);
            }
        }
        return resultDataGroup;
    }
}