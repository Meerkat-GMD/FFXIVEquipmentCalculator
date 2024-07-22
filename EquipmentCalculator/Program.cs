// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var ffxivDataManager = new FFXIVDataManager();

//Console.WriteLine("원하는 직업, 레벨");
//var className = Console.ReadLine();
var className = ClassJobCategory.WAR;
var categoryEquipmentDataGroup = ffxivDataManager.GetClassEquipmentData(className, 700, 710);

EquipmentCalculator.EquipmentCalculator equipmentCalculator = new EquipmentCalculator.EquipmentCalculator(categoryEquipmentDataGroup, ffxivDataManager.FoodData);


int apMod = 190;
var value1 = StatCalculator.CalculateMainStat(ClassJobCategory.PLD, 442);
var value2 = StatCalculator.CalculateMainStat(ClassJobCategory.PLD, 443);

Console.WriteLine(value1);
Console.WriteLine(value2);

float bestDamageGCD = -1f;
float bestGCD = -1f;
float bestDamageInvideGCD = -1f;
float bestInvideGCD = -1f;


for (int targetGCD = 250; targetGCD >= 240; targetGCD -= 1)
{
    (var result, var second) = equipmentCalculator.GetBestEquipmentWithMeld(className, targetGCD);
    if (Math.Abs(result.ExpectedDamage - StatCalculator.ExpectedDamage(new Stat(result.Critical, result.Determination, result.DirectHit, result.Tenacity), className)) < float.Epsilon * 8)
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
    Console.WriteLine($"bestCrt : {result.Critical} // bestDET : {result.Determination} // bestDIR {result.DirectHit} // bestTEN {result.Tenacity} " +
                      $"// Base Speed {result.Speed} Speed Materia {result.SpeedMateria}");

    foreach (var kvp in result.EquipmentList)
    {
        Console.WriteLine($"항목 : {kvp.Key} // 장비 이름 {kvp.Value.Name}");
    }
    /*
    Console.WriteLine("//////////////////////////////////////////////////////");
    
    if (second.ExpectedDamage > bestDamageGCD)
    {
        bestDamageGCD = second.ExpectedDamage;
        bestGCD = targetGCD;
    }

    if (second.ExpectedDamage / targetGCD > bestDamageInvideGCD)
    {
        bestDamageInvideGCD = second.ExpectedDamage / targetGCD;
        bestInvideGCD = targetGCD;
    }
    
    Console.WriteLine($"GCD :{targetGCD}");
    Console.WriteLine($"예상데미지 {second.ExpectedDamage}");
    Console.WriteLine($"예상데미지/GCD {second.ExpectedDamage/targetGCD}");
    Console.WriteLine(second.FoodName);
    Console.WriteLine($"bestCrt : {second.Critical} // bestDET : {second.Determination} // bestDIR {second.DirectHit} // bestTEN {second.Tenacity} " +
                      $"// Base Speed {second.Speed} Speed Materia {second.SpeedMateria}");

    foreach (var kvp in second.EquipmentList)
    {
        Console.WriteLine($"항목 : {kvp.Key} // 장비 이름 {kvp.Value.Name}");
    }
    */
}

Console.WriteLine($"BestGCD {bestGCD}");
Console.WriteLine($"bestInvideGCD {bestInvideGCD}");

Console.WriteLine("GoodBye, World!");
