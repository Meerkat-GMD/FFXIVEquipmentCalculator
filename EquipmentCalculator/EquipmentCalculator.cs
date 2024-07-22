namespace EquipmentCalculator;

public struct StatAvailableData
{
    public int WeaponDamage;
    public int MainStat;
    public int EquipmentCritical;
    public int EquipmentDirectHit;
    public int EquipmentDetermination;
    public int EquipmentSpeed;
    public int EquipmentTenacity;
    public int MaxCritical;
    public int MaxDetermination;
    public int MaxDirectHit;
    public int AllSlotValue;
    public int ExcludeCriticalValue;
}

public class DamageWithEquipmentAndStat
{
    public float ExpectedDamage;
    public Dictionary<ItemUICategory, EquipmentData> EquipmentList = new();
    public int Critical;
    public int DirectHit;
    public int Determination;
    public int Tenacity;
    public int Speed;
    public int SpeedMateria;
    public string FoodName;

    public DamageWithEquipmentAndStat Copy()
    {
        var temp = new DamageWithEquipmentAndStat();
        temp.ExpectedDamage = ExpectedDamage;
        temp.EquipmentList = EquipmentList.ToDictionary(x => x.Key, x => x.Value);
        temp.Critical = Critical;
        temp.DirectHit = DirectHit;
        temp.Determination = Determination;
        temp.Tenacity = Tenacity;
        temp.Speed = Speed;
        temp.SpeedMateria = SpeedMateria;
        temp.FoodName = FoodName;

        return temp;
    }
}

public class EquipmentCalculator
{
    private CategoryEquipmentDataGroup _categoryEquipmentDataGroup;
    private List<FoodData> _foodDataList;
    private float _bestExpectedDamage;
    private int _targetGCD;
    private ClassJobCategory _classJobCategory;
    private DamageWithEquipmentAndStat _bestEquipmentAndStat = new();
    private DamageWithEquipmentAndStat _secondEquipmentAndStat = new();
    public EquipmentCalculator(CategoryEquipmentDataGroup categoryEquipmentDataGroup, FoodData[] foodData)
    {
        _categoryEquipmentDataGroup = categoryEquipmentDataGroup;
        _foodDataList = foodData.ToList();
    }
    
    public (DamageWithEquipmentAndStat, DamageWithEquipmentAndStat) GetBestEquipmentWithMeld(ClassJobCategory currentClass, int targetGCD)
    {
        Dictionary<ItemUICategory, EquipmentData> currentEquipment = new();
        _bestEquipmentAndStat = new(); 
        _bestExpectedDamage = 0f;
        _targetGCD = targetGCD;
        _classJobCategory = currentClass;

        Recursive(ItemUICategory.Weapon, currentEquipment, 0);
        
        return (_bestEquipmentAndStat, _secondEquipmentAndStat);
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

                if (!GCDTier.GCDTierList(_classJobCategory).TryGetValue(_targetGCD - 1, out var nextGCDMinimum))
                {
                    Console.WriteLine("GCD 데이터를 초과했습니다.");
                    return;
                }

                if (nextGCDMinimum <= criticalAvailable.EquipmentSpeed + StatCalculator.BaseSpeed)
                {
                    return;
                }

                var targetGCDMinimumStat = GCDTier.GCDTierList(_classJobCategory)[_targetGCD];

                int needStat = targetGCDMinimumStat - (criticalAvailable.EquipmentSpeed + StatCalculator.BaseSpeed);
                int needMateria = 0;
                if (needStat > 0)
                {
                    needMateria = needStat / Const.SelectMateriaValue;
                    if (needStat % Const.SelectMateriaValue > 0)
                    {
                        needMateria++;
                    }    
                    criticalAvailable.ExcludeCriticalValue -= needMateria * Const.SelectMateriaValue;
                }

                if (criticalAvailable.ExcludeCriticalValue < 0)
                {
                    int getToCriticalMateria = (-criticalAvailable.ExcludeCriticalValue) / Const.SelectMateriaValue;
                    if ((-criticalAvailable.ExcludeCriticalValue) % Const.SelectMateriaValue > 0)
                    {
                        getToCriticalMateria++;
                    }

                    criticalAvailable.MaxCritical -= getToCriticalMateria * Const.SelectMateriaValue;
                    criticalAvailable.ExcludeCriticalValue += getToCriticalMateria * Const.SelectMateriaValue;
                }

                if (criticalAvailable.MaxCritical < 0)
                {
                    continue;
                }
                
                (var expectedDamage, Stat bestStat, string foodName) = CalculateExpectedDamage(criticalAvailable);
                if (_bestExpectedDamage < expectedDamage)
                {
                    _secondEquipmentAndStat = _bestEquipmentAndStat.Copy();
                    
                    _bestExpectedDamage = expectedDamage;
                    _bestEquipmentAndStat.ExpectedDamage = expectedDamage;
                    _bestEquipmentAndStat.EquipmentList = currentEquipment.ToDictionary(x => x.Key, x => x.Value);
                    _bestEquipmentAndStat.Critical = bestStat.CRT;
                    _bestEquipmentAndStat.Determination = bestStat.DET;
                    _bestEquipmentAndStat.DirectHit = bestStat.DIR;
                    _bestEquipmentAndStat.Tenacity = bestStat.TEN;
                    _bestEquipmentAndStat.Speed = bestStat.SPS;
                    _bestEquipmentAndStat.SpeedMateria = needMateria;
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

    private (float, Stat, string) CalculateExpectedDamage(StatAvailableData statAvailableData)
    {
        int availableMaxCrt = StatCalculator.BaseCRT + statAvailableData.EquipmentCritical + statAvailableData.MaxCritical; //이미 최대 크리티컬
        int baseCrt = StatCalculator.BaseCRT + statAvailableData.EquipmentCritical;
        int baseDet = StatCalculator.BaseDET + statAvailableData.EquipmentDetermination;
        int baseDir = StatCalculator.BaseDIR + statAvailableData.EquipmentDirectHit;
        int baseTen = StatCalculator.BaseTEN + statAvailableData.EquipmentTenacity;

        int maxValue = availableMaxCrt + baseDet + baseDir + statAvailableData.ExcludeCriticalValue;
        
        float bestOfBest = 0;
        Stat bestOfStat = new();
        string foodName ="";
        
        for (int adjustCal = availableMaxCrt; adjustCal >= availableMaxCrt - Const.SelectMateriaValue * 3; adjustCal -= Const.SelectMateriaValue)
        {
            int remainCritical = availableMaxCrt - adjustCal;
            foreach (var food in _foodDataList)
            {
                int tenMax = _classJobCategory.IsTank() ? Const.SelectMateriaValue * 5 : 0;
                for (int addTEN = 0; addTEN <= tenMax; addTEN += Const.SelectMateriaValue)
                {
                    int ten = baseTen + addTEN;
                    int foodCrt = Math.Clamp(adjustCal * food.CRT / 100, 0, food.CRTCap);

                    int maxDET = baseDet + statAvailableData.ExcludeCriticalValue + remainCritical - addTEN;
                    
                    if(maxDET > baseDet + statAvailableData.MaxDetermination)
                    {
                        continue;
                    }
                    
                    float best = -1;
                    Stat bestStat = new();
                    for (int det = maxDET; det >= baseDet; det--)
                    {
                        int dir = (maxDET - det) + baseDir;

                        if (dir < baseDir)
                        {
                            continue;
                        }

                        if (dir > baseDir + statAvailableData.ExcludeCriticalValue  + remainCritical)
                        {
                            continue;
                        }

                        if (maxValue != adjustCal + det + dir + addTEN)
                        {
                            Console.WriteLine("????");
                            continue;
                        }
                    
                        int foodDet = Math.Clamp(det * food.DET / 100, 0, food.DETCap);
                        int foodDir = Math.Clamp(dir * food.DIR / 100, 0, food.DIRCap);
                    
                        var stat = new Stat(adjustCal + foodCrt, det + foodDet, dir + foodDir, ten);
                        stat.MainStat = statAvailableData.MainStat;
                        stat.WeaponDamage = statAvailableData.WeaponDamage;
                        float damage = stat.CalculateExpectedDamage(_classJobCategory);
                        if (best < damage)
                        {
                            if (MathF.Abs(dir - baseDir) % Const.SelectMateriaValue == 0)
                            {
                                best = damage;
                                bestStat = stat;
                            }
                        }
                    }

                    if (best > bestOfBest)
                    {
                        bestOfBest = best;
                        bestOfStat = bestStat;
                        bestOfStat.SPS = StatCalculator.BaseSpeed + statAvailableData.EquipmentSpeed;
                        foodName = food.Name;
                    }
                }
            }
            
        }
        

        return (bestOfBest, bestOfStat, foodName);
    }

    //크리를 최우선으로 채우고 직격 의지 순으로 채운다.
    //여기서 계산하는 것은 크리 스탯의 총 양만 
    private StatAvailableData CalculateAvailableCritical(Dictionary<ItemUICategory, EquipmentData> equipmentDataDic)
    {
        var result = new StatAvailableData();
        foreach (var equipmentData in equipmentDataDic.Values)
        {
            var categoryData = new StatAvailableData();

            categoryData.WeaponDamage += equipmentData.WD;
            categoryData.MainStat += equipmentData.MainStat;
            
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
                case StatCategory.TEN:
                    categoryData.EquipmentTenacity += equipmentData.P_Value;
                    break;
                case StatCategory.SKS:
                case StatCategory.SPS:
                    categoryData.EquipmentSpeed += equipmentData.P_Value;
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
                case StatCategory.TEN:
                    categoryData.EquipmentTenacity += equipmentData.S_Value;
                    break;
                case StatCategory.SKS:
                case StatCategory.SPS:
                    categoryData.EquipmentSpeed += equipmentData.S_Value;
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
                        categoryData.MaxCritical += slotValue;
                    }
                    else
                    {
                        if (equipmentData.P_Value > equipmentData.S_Value + categoryData.MaxCritical + slotValue)
                        {
                            categoryData.MaxCritical += slotValue;
                        }
                        else if (equipmentData.P_Value + Const.StatOverValue >
                                 equipmentData.S_Value + categoryData.MaxCritical + slotValue)
                        {
                            categoryData.MaxCritical +=
                                equipmentData.P_Value - equipmentData.S_Value - categoryData.MaxCritical;
                        }
                        else
                        {
                            categoryData.ExcludeCriticalValue += slotValue;
                        }
                    }
                }
                else
                {
                    categoryData.ExcludeCriticalValue += slotValue;
                }
                
                if (equipmentData.P_Abbrv != StatCategory.DET)
                {
                    if (equipmentData.S_Abbrv != StatCategory.DET)
                    {
                        categoryData.MaxDetermination += slotValue;
                    }
                    else 
                    {
                        if (equipmentData.P_Value > equipmentData.S_Value + categoryData.MaxDetermination + slotValue)
                        {
                            categoryData.MaxDetermination += slotValue;    
                        }
                        else if (equipmentData.P_Value + Const.StatOverValue > equipmentData.S_Value + categoryData.MaxDetermination + slotValue)
                        {
                            categoryData.MaxDetermination += equipmentData.P_Value - equipmentData.S_Value - categoryData.MaxDetermination;
                        }
                    }
                }

                
                if (equipmentData.P_Abbrv != StatCategory.DIR)
                {
                    if (equipmentData.S_Abbrv != StatCategory.DIR)
                    {
                        categoryData.MaxDirectHit += slotValue;
                    }
                    else 
                    {
                        if (equipmentData.P_Value > equipmentData.S_Value + categoryData.MaxDirectHit + slotValue)
                        {
                            categoryData.MaxDirectHit += slotValue;    
                        }
                        else if (equipmentData.P_Value + Const.StatOverValue > equipmentData.S_Value + categoryData.MaxDirectHit + slotValue)
                        {
                            categoryData.MaxDirectHit += equipmentData.P_Value - equipmentData.S_Value - categoryData.MaxDirectHit;
                        }
                    }
                }

                categoryData.AllSlotValue += slotValue;
            }

            result.WeaponDamage += categoryData.WeaponDamage;
            result.MainStat += categoryData.MainStat;
            result.EquipmentCritical += categoryData.EquipmentCritical;
            result.EquipmentDetermination += categoryData.EquipmentDetermination;
            result.EquipmentDirectHit += categoryData.EquipmentDirectHit;
            result.EquipmentSpeed += categoryData.EquipmentSpeed;
            result.EquipmentTenacity += categoryData.EquipmentTenacity;
            result.MaxCritical += categoryData.MaxCritical;
            result.MaxDetermination += categoryData.MaxDetermination;
            result.MaxDirectHit += categoryData.MaxDirectHit;
            result.AllSlotValue += categoryData.AllSlotValue;
            result.ExcludeCriticalValue += categoryData.ExcludeCriticalValue;
        }

        return result;
    }
    
}