using UCY.CodeGenerator.Console.Config;
using UCY.CodeGenerator.Console.Generator;
using UCY.CodeGenerator.Console.NewProject;

Console.WriteLine("Hello, World!");

CustomConfig.ConfigLoad();

while (true)
{
    Console.WriteLine("[1] => Create New Project");
    Console.WriteLine("[2] => Create New API");
    Console.WriteLine("[x] => Exit");

    string menuSelect = Console.ReadLine();

    if (menuSelect == "1")
    {
        var projectManager = new ProjectManager(CustomConfig.configFilePath);
        projectManager.Start();
    }
    else if (menuSelect == "2")
    {
        ModelToService modelToService = new ModelToService();
        modelToService.StartGenerator();
    }
    else if (menuSelect.ToLower() == "x")
    {
        Console.WriteLine("Exiting program...");
        break; 
    }
    else
    {
        Console.WriteLine("Invalid option, please try again.");
    }
}

Console.WriteLine("Program has ended.");
