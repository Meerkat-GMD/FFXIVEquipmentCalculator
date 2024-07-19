// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var equipmentDataManager = new EquipmentDataManager();

//Console.WriteLine("원하는 직업, 레벨");
//var className = Console.ReadLine();
var className = ClassJobCategory.BLM;
var categoryEquipmentDataGroup = equipmentDataManager.GetClassEquipmentData(className, 640, 650);

foreach (ItemUICategory category in Enum.GetValues(typeof(ItemUICategory)))

{
}

var sumStat = new StatCalculator.Stat();
float bestExpectedDamage = -1;
StatCalculator.Stat bestStat;
Dictionary<ItemUICategory, EquipmentData> bestEquipment = new();



//StatRecursive(ItemUICategory.Weapon, new Dictionary<ItemUICategory, EquipmentData>(), 0);

void StatRecursive(ItemUICategory category, Dictionary<ItemUICategory, EquipmentData> equipGroup, int tomeStone,
    int ringIndex = 0)
{
    foreach (var data in categoryEquipmentDataGroup.CategoryDataGroup[category])
    {
        if (tomeStone + data.GetTomeStone() > 900)
        {
            continue;
        }

        for (int slot = 0; slot < 5; slot++)
        {
            if (!data.OverMeld && slot >= 2)
            {
                break;
            }

            int slotValue;
            if ((data.ItemUICategory.IsAccessory() && slot >= 2) || slot >= 3)
            {
                slotValue = 12;
            }
            else
            {
                slotValue = 36;
            }
            
        }

        sumStat += data;
        tomeStone += data.GetTomeStone();
        if (ringIndex == 1)
        {
            equipGroup[ItemUICategory.Ring1] = data;
        }
        else
        {
            equipGroup[category] = data;
        }


        if (category != ItemUICategory.Ring)
        {
            StatRecursive((ItemUICategory)((int)category + 1), equipGroup, tomeStone);
        }

        if (category == ItemUICategory.Ring && ringIndex == 0)
        {
            StatRecursive(category, equipGroup, tomeStone, 1);
        }


        float expectedDamage = sumStat.CalculateExpectedDamage();
        if (bestExpectedDamage < expectedDamage)
        {
            bestExpectedDamage = expectedDamage;
            bestStat = sumStat;
            foreach (ItemUICategory c in Enum.GetValues(typeof(ItemUICategory)))
            {
                bestEquipment[c] = equipGroup[c];
            }
        }

        if (category == ItemUICategory.Ring && ringIndex == 1)
        {
            equipGroup[ItemUICategory.Ring1] = null;
        }
        else
        {
            equipGroup[category] = null;
        }

        sumStat -= data;
    }
}

/*
Console.WriteLine(bestExpectedDamage);
foreach (var bestData in bestEquipment.Values)
{
    Console.WriteLine(bestData.Name);
}
1.395952
bsetCrt : 2409 bestDET : 1694 bestDIR 1763
bsetCrt : 2409 bestDET : 1910 bestDIR 1547
*///
int calCrt = 2313+62;
int calDet = 1680+103;
int calDir = 1121; 
StatCalculator.Stat stat = new StatCalculator.Stat(calCrt, calDet, calDir);

int max = stat.DET + stat.DIR;
float best = -1;
float bestDET = -1;
for (int i = 0; i <= max/2 ; i++) {
    stat.DET = i;
    stat.DIR = max - i;
    float damage = stat.CalculateExpectedDamage();
    if (best < damage)
    {
        
        if (MathF.Abs(stat.DET - calDet) % 12 == 0)
        {
            best = damage;
            bestDET = stat.DET;
        }
    }
}

Console.WriteLine(best);
Console.WriteLine($"bestCrt : {calCrt} bestDET : {bestDET} bestDIR {max - bestDET}");

Console.WriteLine("GoodBye, World!");