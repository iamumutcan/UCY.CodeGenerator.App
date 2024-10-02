using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCY.CodeGenerator.Console.Config;

namespace UCY.CodeGenerator.Console.Generator;
public class Generator
{
    public string corePath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Core);
    public string cachingPath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Caching);
    public string APIPath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.API);
    public string repositoryPath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Repository);
    public string servicePath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Service);
    public string WebPath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Web);
    public void IRepositoryGenerator()
    {
        string generatorCode = CustomConfig.IRepositoryTemplate
            .Replace("{{ProjectName}}", CustomConfig.ProjectName)
            .Replace("{{CoreLayer}}", CustomConfig.Core)
            .Replace("{{APILayer}}", CustomConfig.API)
            .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
            .Replace("{{ServiceLayer}}", CustomConfig.Service)
            .Replace("{{WebLayer}}", CustomConfig.Web)
            .Replace("{{CachingLayer}}", CustomConfig.Caching)
            .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
            .Replace("{{ModelName}}", CustomConfig.ModelName);
        string generatorPath = Path.Combine(corePath + "\\" + "Repositories", $"I{CustomConfig.ModelName}Repository.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }

    public void IServiceGenerator()
    {
        string generatorCode = CustomConfig.IServiceTemplate
            .Replace("{{ProjectName}}", CustomConfig.ProjectName)
            .Replace("{{CoreLayer}}", CustomConfig.Core)
            .Replace("{{APILayer}}", CustomConfig.API)
            .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
            .Replace("{{ServiceLayer}}", CustomConfig.Service)
            .Replace("{{WebLayer}}", CustomConfig.Web)
            .Replace("{{CachingLayer}}", CustomConfig.Caching)
            .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
            .Replace("{{ModelName}}", CustomConfig.ModelName);
        string generatorPath = Path.Combine(corePath + "\\" + "Services", $"I{CustomConfig.ModelName}Service.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }
    public void RepositoryGenerator()
    {
        string generatorCode = CustomConfig.RepositoryTemplate
            .Replace("{{ProjectName}}", CustomConfig.ProjectName)
            .Replace("{{CoreLayer}}", CustomConfig.Core)
            .Replace("{{APILayer}}", CustomConfig.API)
            .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
            .Replace("{{ServiceLayer}}", CustomConfig.Service)
            .Replace("{{WebLayer}}", CustomConfig.Web)
            .Replace("{{CachingLayer}}", CustomConfig.Caching)
            .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
            .Replace("{{ModelName}}", CustomConfig.ModelName);
        string generatorPath = Path.Combine(repositoryPath + "\\" + "Repositories", $"{CustomConfig.ModelName}Repository.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }

    public void ServiceGenerator()
    {
        string generatorCode = CustomConfig.ServiceTemplate
            .Replace("{{ProjectName}}", CustomConfig.ProjectName)
            .Replace("{{CoreLayer}}", CustomConfig.Core)
            .Replace("{{APILayer}}", CustomConfig.API)
            .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
            .Replace("{{ServiceLayer}}", CustomConfig.Service)
            .Replace("{{WebLayer}}", CustomConfig.Web)
            .Replace("{{CachingLayer}}", CustomConfig.Caching)
            .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
            .Replace("{{ModelName}}", CustomConfig.ModelName);
        string generatorPath = Path.Combine(servicePath + "\\" + "Services", $"{CustomConfig.ModelName}Service.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }

    public void ApiControllerGenerator()
    {
        string generatorCode = CustomConfig.ApiControllerTemplate
            .Replace("{{ProjectName}}", CustomConfig.ProjectName)
            .Replace("{{CoreLayer}}", CustomConfig.Core)
            .Replace("{{APILayer}}", CustomConfig.API)
            .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
            .Replace("{{ServiceLayer}}", CustomConfig.Service)
            .Replace("{{WebLayer}}", CustomConfig.Web)
            .Replace("{{CachingLayer}}", CustomConfig.Caching)
            .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
            .Replace("{{ModelName}}", CustomConfig.ModelName);
        string generatorPath = Path.Combine(APIPath + "\\" + "Controllers", $"{CustomConfig.ModelName}Controller.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }
    public void DtoGenerator(List<ModelProperty> properties)
    {
        string DtoPropList = "";
        foreach (var prop in properties)
        {
            DtoPropList += $"public {prop.Type} {prop.Name} {{ get; set; }} \n         ";
        }
        string generatorCode = CustomConfig.DtoTemplate
            .Replace("{{DtoPropList}}", DtoPropList)
            .Replace("{{ProjectName}}", CustomConfig.ProjectName)
            .Replace("{{CoreLayer}}", CustomConfig.Core)
            .Replace("{{APILayer}}", CustomConfig.API)
            .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
            .Replace("{{ServiceLayer}}", CustomConfig.Service)
            .Replace("{{WebLayer}}", CustomConfig.Web)
            .Replace("{{CachingLayer}}", CustomConfig.Caching)
            .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
            .Replace("{{ModelName}}", CustomConfig.ModelName);
        string generatorPath = Path.Combine(corePath + "\\" + "DTOs", $"{CustomConfig.ModelName}Dto.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }
}
