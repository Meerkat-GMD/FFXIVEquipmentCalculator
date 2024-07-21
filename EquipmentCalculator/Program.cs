// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var ffxivDataManager = new FFXIVDataManager();

//Console.WriteLine("원하는 직업, 레벨");
//var className = Console.ReadLine();
var className = ClassJobCategory.SCH;
var categoryEquipmentDataGroup = ffxivDataManager.GetClassEquipmentData(className, 700, 710);

EquipmentCalculator.EquipmentCalculator equipmentCalculator = new EquipmentCalculator.EquipmentCalculator(categoryEquipmentDataGroup, ffxivDataManager.FoodData);



var expectedDamage1 = StatCalculator.ExpectedDamage(2856, 582, 2129);
var expectedDamage2 = StatCalculator.ExpectedDamage(2436, 1950, 2133);

Console.WriteLine(expectedDamage1);
Console.WriteLine(expectedDamage2);

float bestDamageGCD = -1f;
float bestGCD = -1f;
float bestDamageInvideGCD = -1f;
float bestInvideGCD = -1f;


for (int targetGCD = 250; targetGCD >= 240; targetGCD -= 1)
{
    var result = equipmentCalculator.GetBestEquipmentWithMeld(className, targetGCD);
    if (Math.Abs(result.ExpectedDamage -
                 StatCalculator.ExpectedDamage(result.Critical, result.DirectHit, result.Determination)) <
        float.Epsilon * 8)
    {
        Console.WriteLine("Good");
    }

    if (result.ExpectedDamage > bestDamageGCD)
    {
        bestDamageGCD = result.ExpectedDamage;
        bestGCD = targetGCD;
    }

    if (result.ExpectedDamage / targetGCD > bestDamageInvideGCD)
    {
        bestDamageInvideGCD = result.ExpectedDamage / targetGCD;
        bestInvideGCD = targetGCD;
    }
    
    Console.WriteLine($"GCD :{targetGCD}");
    Console.WriteLine($"예상데미지 {result.ExpectedDamage}");
    Console.WriteLine($"예상데미지/GCD {result.ExpectedDamage/targetGCD}");
    Console.WriteLine(result.FoodName);
    Console.WriteLine($"bestCrt : {result.Critical} // bestDET : {result.Determination} // bestDIR {result.DirectHit}  // Base Speed {result.Speed} Speed Materia {result.SpeedMateria}");

    foreach (var kvp in result.EquipmentList)
    {
        Console.WriteLine($"항목 : {kvp.Key} // 장비 이름 {kvp.Value.Name}");
    }
}

Console.WriteLine($"BestGCD {bestGCD}");
Console.WriteLine($"bestInvideGCD {bestInvideGCD}");

Console.WriteLine("GoodBye, World!");
