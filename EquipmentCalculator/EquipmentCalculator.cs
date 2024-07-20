namespace EquipmentCalculator;

public struct CalculateCriticalAvailable
{
    public int EquipmentCritical;
    public int EquipmentDirectHit;
    public int EquipmentDetermination;
    public int Critical;
    public int Remain;
}

public class DamageWithEquipmentAndStat
{
    public float ExpectedDamage;
    public Dictionary<ItemUICategory, EquipmentData> EquipmentList = new();
    public int Critical;
    public int DirectHit;
    public int Determination;
    public string FoodName;
}

public class EquipmentCalculator
{
    private CategoryEquipmentDataGroup _categoryEquipmentDataGroup;
    private List<FoodData> _foodDataList;
    private float _bestExpectedDamage;
    private DamageWithEquipmentAndStat _bestEquipmentAndStat = new();
    public EquipmentCalculator(CategoryEquipmentDataGroup categoryEquipmentDataGroup, FoodData[] foodData)
    {
        _categoryEquipmentDataGroup = categoryEquipmentDataGroup;
        _foodDataList = foodData.ToList();
    }
    
    public DamageWithEquipmentAndStat GetBestEquipmentWithMeld()
    {
        Dictionary<ItemUICategory, EquipmentData> currentEquipment = new();
        _bestEquipmentAndStat = new(); 
        _bestExpectedDamage = 0f;

        Recursive(ItemUICategory.Weapon, currentEquipment, 0);
        
        return _bestEquipmentAndStat;
    }

    private void Recursive(ItemUICategory itemCategory, Dictionary<ItemUICategory, EquipmentData> currentEquipment, int normalCount)
    {
        var categoryEquipment = new List<EquipmentData>();
        if (itemCategory == ItemUICategory.Ring1)
        {
            categoryEquipment = _categoryEquipmentDataGroup.CategoryDataGroup[ItemUICategory.Ring].ToList();
        }
        else
        {
            categoryEquipment = _categoryEquipmentDataGroup.CategoryDataGroup[itemCategory].ToList();
        }
        
        if (itemCategory == ItemUICategory.Ring1)
        {
            if (currentEquipment[ItemUICategory.Ring].IsUnique)
            {
                categoryEquipment.Remove(currentEquipment[ItemUICategory.Ring]);    
            }
                
            foreach (var item in categoryEquipment)
            {
                if (item.IsNormalEquipment())
                {
                    if (normalCount + item.ItemUICategory.NormalEquipmentExchangeTokenNum() > 8)
                    {
                        continue;
                    }
                }
                
                currentEquipment[ItemUICategory.Ring1] = item;
                var criticalAvailable = CalculateAvailableCritical(currentEquipment);
                (var expectedDamage, StatCalculator.Stat bestStat, string foodName) = CalculateExpectedDamage(criticalAvailable);
                if (_bestExpectedDamage < expectedDamage)
                {
                    _bestExpectedDamage = expectedDamage;
                    _bestEquipmentAndStat.ExpectedDamage = expectedDamage;
                    _bestEquipmentAndStat.EquipmentList = currentEquipment.ToDictionary(x => x.Key, x => x.Value);
                    _bestEquipmentAndStat.Critical = bestStat.CRT;
                    _bestEquipmentAndStat.Determination = bestStat.DET;
                    _bestEquipmentAndStat.DirectHit = bestStat.DIR;
                    _bestEquipmentAndStat.FoodName = foodName;
                }
            }
        }
        else
        {
            foreach (var item in categoryEquipment)
            {
                currentEquipment[itemCategory] = item;
                if (item.IsNormalEquipment())
                {
                    if (normalCount + item.ItemUICategory.NormalEquipmentExchangeTokenNum() <= 8)
                    {
                        Recursive((ItemUICategory)((int)itemCategory +1), currentEquipment, normalCount + item.ItemUICategory.NormalEquipmentExchangeTokenNum());    
                    }
                }
                else
                {
                    Recursive((ItemUICategory)((int)itemCategory +1), currentEquipment, normalCount); 
                }
                
            }
        }
    }

    private (float, StatCalculator.Stat, string) CalculateExpectedDamage(CalculateCriticalAvailable criticalAvailableData)
    {
        int calCrt = StatCalculator.BaseCRT + criticalAvailableData.EquipmentCritical + criticalAvailableData.Critical; //이미 최대 크리티컬
        int calDet = StatCalculator.BaseDET + criticalAvailableData.EquipmentDetermination + criticalAvailableData.Remain;
        int calDir = StatCalculator.BaseDIR + criticalAvailableData.EquipmentDirectHit;

        float bestOfBest = 0;
        StatCalculator.Stat bestOfStat = new();
        string foodName ="";
        
        for (int adjustCal = calCrt; adjustCal >= calCrt - Const.LowMateriaValue * 10; adjustCal -= Const.HighMateriaValue)
        {

            foreach (var food in _foodDataList)
            {
                int foodCrt = Math.Clamp(adjustCal * food.CRT / 100, 0, food.CRTCap);
                
                StatCalculator.Stat stat = new StatCalculator.Stat(adjustCal + foodCrt, calDet + calCrt - adjustCal, calDir);

                if (adjustCal + foodCrt == 2964)
                {
                    Console.WriteLine($"calCrt : {calCrt}");
                    Console.WriteLine($"adjustCal : {adjustCal}");
                    Console.WriteLine($"foodCrt : {foodCrt}");
                }
                
                
                int max = stat.DET + stat.DIR;
                float best = -1;
                StatCalculator.Stat bestStat = new();
                for (int i = criticalAvailableData.EquipmentDetermination + StatCalculator.BaseDET; i <= max - StatCalculator.BaseDIR + criticalAvailableData.EquipmentDirectHit; i++) 
                {
                    int foodDet = Math.Clamp(i * food.DET / 100, 0, food.DETCap);
                    int foodDir = Math.Clamp((max - i) * food.DIR / 100, 0, food.DIRCap);
                    
                    stat.DET = i + foodDet;
                    stat.DIR = max - i + foodDir;
                    float damage = stat.CalculateExpectedDamage();
                    if (best < damage)
                    {
        
                        if (MathF.Abs(stat.DIR - StatCalculator.BaseDIR - criticalAvailableData.EquipmentDirectHit) % Const.HighMateriaValue == 0)
                        {
                            best = damage;
                            bestStat.CRT = stat.CRT;
                            bestStat.DET = stat.DET;
                            bestStat.DIR = stat.DIR;
                        }
                    }
                }

                if (best > bestOfBest)
                {
                    bestOfBest = best;
                    bestOfStat = bestStat;
                    foodName = food.Name;
                }
            }
            
        }
        

        return (bestOfBest, bestOfStat, foodName);
    }

    //크리를 최우선으로 채우고 직격 의지 순으로 채운다.
    //여기서 계산하는 것은 크리 스탯의 총 양만 
    private CalculateCriticalAvailable CalculateAvailableCritical(Dictionary<ItemUICategory, EquipmentData> equipmentDataDic)
    {
        var result = new CalculateCriticalAvailable();
        foreach (var equipmentData in equipmentDataDic.Values)
        {
            var categoryData = new CalculateCriticalAvailable();

            switch (equipmentData.P_Abbrv)
            {
                case StatCategory.CRT:
                    categoryData.EquipmentCritical += equipmentData.P_Value;
                    break;
                case StatCategory.DET:
                    categoryData.EquipmentDetermination += equipmentData.P_Value;
                    break;
                case StatCategory.DIR:
                    categoryData.EquipmentDirectHit += equipmentData.P_Value;
                    break;
            }
            
            switch (equipmentData.S_Abbrv)
            {
                case StatCategory.CRT:
                    categoryData.EquipmentCritical += equipmentData.S_Value;
                    break;
                case StatCategory.DET:
                    categoryData.EquipmentDetermination += equipmentData.S_Value;
                    break;
                case StatCategory.DIR:
                    categoryData.EquipmentDirectHit += equipmentData.S_Value;
                    break;
            }
            
            for (int slot = 1; slot <= 5; slot++)
            {
                if (!equipmentData.OverMeld)
                {
                    if (equipmentData.Slots < slot)
                    {
                        break;
                    }
                }

                int slotValue;
                if ((equipmentData.ItemUICategory.IsAccessory() && slot >= 3) || slot >= 4)
                {
                    slotValue = Const.LowMateriaValue;
                }
                else
                {
                    slotValue = Const.HighMateriaValue;
                }

                if (equipmentData.P_Abbrv != StatCategory.CRT)
                {
                    if (equipmentData.S_Abbrv != StatCategory.CRT)
                    {
                        categoryData.Critical += slotValue;
                    }
                    else 
                    {
                        if (equipmentData.P_Value > equipmentData.S_Value + categoryData.Critical + slotValue)
                        {
                            categoryData.Critical += slotValue;    
                        }
                        else if (equipmentData.P_Value + Const.CriticalOverValue > equipmentData.S_Value + categoryData.Critical + slotValue)
                        {
                            categoryData.Critical += equipmentData.P_Value - equipmentData.S_Value - categoryData.Critical;
                        }
                        else
                        {
                            categoryData.Remain += slotValue;
                        }
                    }
                }
                else
                {
                    categoryData.Remain += slotValue;
                }
            }

            result.EquipmentCritical += categoryData.EquipmentCritical;
            result.EquipmentDetermination += categoryData.EquipmentDetermination;
            result.EquipmentDirectHit += categoryData.EquipmentDirectHit;
            result.Critical += categoryData.Critical;
            result.Remain += categoryData.Remain;
        }

        return result;
    }
    
}