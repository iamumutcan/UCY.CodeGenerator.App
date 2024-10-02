using UCY.CodeGenerator.Console.Config;
using UCY.CodeGenerator.Console.Generator;
using UCY.CodeGenerator.Console.NewProject;

Console.WriteLine("Hello, World!");

CustomConfig.ConfigLoad();

Console.WriteLine("[1] => Yeni Proje Oluştur");
Console.WriteLine("[2] => Yeni API Oluştur");
string menuSelect = System.Console.ReadLine();

if(menuSelect =="1")
{
    var projectManager = new ProjectManager(CustomConfig.configFilePath);
    projectManager.Start();
}

if (menuSelect == "2")
{
    ModelToService modelToService = new ModelToService();
    modelToService.StartGenerator();
}


