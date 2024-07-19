// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var ffxivDataManager = new FFXIVDataManager();

//Console.WriteLine("원하는 직업, 레벨");
//var className = Console.ReadLine();
var className = ClassJobCategory.BLM;
var categoryEquipmentDataGroup = ffxivDataManager.GetClassEquipmentData(className, 700, 710);

EquipmentCalculator.EquipmentCalculator equipmentCalculator = new EquipmentCalculator.EquipmentCalculator(categoryEquipmentDataGroup, ffxivDataManager.FoodData);

var result = equipmentCalculator.GetBestEquipmentWithMeld();



Console.WriteLine(result.ExpectedDamage);
Console.WriteLine(result.FoodName);
Console.WriteLine($"bestCrt : {result.Critical} bestDET : {result.Determination} bestDIR {result.DirectHit}");

foreach (var kvp in result.EquipmentList)
{
    Console.WriteLine($"항목 : {kvp.Key} // 장비 이름 {kvp.Value.Name}");
}

Console.WriteLine("GoodBye, World!");