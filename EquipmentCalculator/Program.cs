// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var equipmentDataManager = new EquipmentDataManager();

//Console.WriteLine("원하는 직업, 레벨");
//var className = Console.ReadLine();
var className = ClassJobCategory.BLM;

equipmentDataManager.GetClassEquipmentData(className, 640, 650);


Console.WriteLine("GoodBye, World!");
