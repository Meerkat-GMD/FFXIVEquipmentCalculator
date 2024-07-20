// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var ffxivDataManager = new FFXIVDataManager();

//Console.WriteLine("원하는 직업, 레벨");
//var className = Console.ReadLine();
var className = ClassJobCategory.AST;
var categoryEquipmentDataGroup = ffxivDataManager.GetClassEquipmentData(className, 700, 710);

EquipmentCalculator.EquipmentCalculator equipmentCalculator = new EquipmentCalculator.EquipmentCalculator(categoryEquipmentDataGroup, ffxivDataManager.FoodData);



var expectedDamage1 = StatCalculator.ExpectedDamage(2856, 582, 2129);
var expectedDamage2 = StatCalculator.ExpectedDamage(1675, 1893, 808);

Console.WriteLine(expectedDamage1);
Console.WriteLine(expectedDamage2);

var result = equipmentCalculator.GetBestEquipmentWithMeld();
if (Math.Abs(result.ExpectedDamage -
             StatCalculator.ExpectedDamage(result.Critical, result.DirectHit, result.Determination)) <
    float.Epsilon * 8)
{
    Console.WriteLine("Good");
}

Console.WriteLine(result.ExpectedDamage);
Console.WriteLine(result.FoodName);
Console.WriteLine($"bestCrt : {result.Critical} bestDET : {result.Determination} bestDIR {result.DirectHit}");

foreach (var kvp in result.EquipmentList)
{
    Console.WriteLine($"항목 : {kvp.Key} // 장비 이름 {kvp.Value.Name}");
}

Console.WriteLine("GoodBye, World!");