using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCY.CodeGenerator.Console.Config;

public static class CustomConfig
{
    public static string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
    public static string configFilePath = Path.Combine(projectDirectory, @"..\..\..\config.json");
    public static string ModelName { get; set; }
    public static string ModelNameLower { get; set; }

    public static string ProjectName { get; private set; }
    public static string ProjectFilePath { get; private set; }
    public static string API { get; private set; }
    public static string Caching { get; private set; }
    public static string Core { get; private set; }
    public static string Repository { get; private set; }
    public static string Service { get; private set; }
    public static string Web { get; private set; }

    // Templates
    public static string IRepositoryTemplate { get; private set; }
    public static string RepositoryTemplate { get; private set; }

    public static string IServiceTemplate { get; private set; }
    public static string ServiceTemplate { get; private set; }

    public static string DtoTemplate { get; private set; }
    public static string ConfigurationTemplate { get; private set; }
    public static string ApiControllerTemplate { get; private set; }
    public static string ApiControllerWithSingleDtoTemplate { get; private set; }


    public static void ConfigLoad()
    {
        // JSON dosyasını okuyun
        string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string configFilePath = Path.Combine(projectDirectory, @"..\..\..\config.json");

        try
        {
            string json = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<dynamic>(json);

            // Yüklenen verileri mevcut nesneye kopyala
            ProjectName = config.ProjectName;
            ProjectFilePath = config.ProjectFilePath;
            API = config.API;
            Caching = config.Caching;
            Core = config.Core;
            Repository = config.Repository;
            Service = config.Service;
            Web = config.Web;

            // templateler
            string IRepositoryFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\IRepositoryTemplate.txt");
            IRepositoryTemplate = File.ReadAllText(IRepositoryFilePath);

            string RepositoryFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\RepositoryTemplate.txt");
            RepositoryTemplate = File.ReadAllText(RepositoryFilePath);

            string IServiceFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\IServiceTemplate.txt");
            IServiceTemplate = File.ReadAllText(IServiceFilePath);

            string ServiceFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\ServiceTemplate.txt");
            ServiceTemplate = File.ReadAllText(ServiceFilePath);

            string DtoFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\DtoTemplate.txt");
            DtoTemplate = File.ReadAllText(DtoFilePath);

            string ConfigurationFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\ConfigurationTemplate.txt");
            ConfigurationTemplate = File.ReadAllText(ConfigurationFilePath);

            string ControllerFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\ApiControllerTemplate.txt");
            ApiControllerTemplate = File.ReadAllText(ControllerFilePath);

            string ControllerWithSingleFilePath = Path.Combine(projectDirectory, @"..\..\..\Templates\ApiControllerTemplateWithSingleDto.txt");
            ApiControllerWithSingleDtoTemplate= File.ReadAllText(ControllerWithSingleFilePath);

        }
        catch (Exception ex)
        {
        }
    }
}
