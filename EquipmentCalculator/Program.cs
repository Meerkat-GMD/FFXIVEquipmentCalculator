// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using EquipmentCalculator;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

string path = "../../../FF14.json";
EquipmentData[] data;

using (StreamReader file = File.OpenText(path))
{
    string jsonRawData = file.ReadToEnd();
    data = JsonConvert.DeserializeObject<EquipmentData[]>(jsonRawData);
}


foreach (var d in data)
{
    Console.WriteLine($"{d.Name} : {d.ClassJobCategory[0]}");
}

Console.ReadLine();
Console.WriteLine("GoodBye, World!");
