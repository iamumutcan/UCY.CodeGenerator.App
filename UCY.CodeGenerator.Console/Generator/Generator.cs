﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public void DtoGeneratorMulti(List<ModelProperty> properties)
    {
        DtoGenerator(properties,"User");
    }

    public void DtoGenerator(List<ModelProperty> properties,string dtoFilename)
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
        string generatorPath = Path.Combine(corePath + "\\" + "DTOs", $"{CustomConfig.ModelName}{dtoFilename}Dto.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(generatorPath));
        File.WriteAllText(generatorPath, generatorCode);
    }
    public void AddModelToDbContext(string modelName)
    {
        string ModelNamePluralize = Helper.Pluralize(modelName);

        string appDbContextPath = Path.Combine(repositoryPath, "AppDbContext.cs");

        // Read the AppDbContext file
        string[] lines = File.ReadAllLines(appDbContextPath).ToArray();

        // Check if the model is already defined
        bool modelExists = lines.Any(line => line.Contains($"public DbSet<{modelName}>"));

        if (!modelExists)
        {
            int insertIndex = -1;

            // Find the last occurrence of "public DbSet<" in the file
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i].Trim().StartsWith("public DbSet<"))
                {
                    insertIndex = i + 1; // Set insertIndex to the line after the last DbSet
                    break;
                }
            }

            // Add the new DbSet definition
            using (StreamWriter sw = new StreamWriter(appDbContextPath, false))
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    sw.WriteLine(lines[i]);
                    if (i == insertIndex - 1)
                    {
                        sw.WriteLine($"    public DbSet<{modelName}> {ModelNamePluralize} {{ get; set; }}");
                    }
                }
            }

            // Print a message indicating the model was successfully added
            System.Console.WriteLine($"{modelName} model successfully added.");
        }
        else
        {
            // Notify if the model already exists
            System.Console.WriteLine($"{modelName} model already exists.");
        }
    }
    public void AddModelToMapProfile(string modelName)
    {
        string modelNamePluralized = Helper.Pluralize(modelName);

        string mapProfilePath = Path.Combine(servicePath, "Mapping\\MapProfile.cs");

        // Read the MapProfile file
        string[] lines = File.ReadAllLines(mapProfilePath).ToArray();

        // Check if the mapping is already defined
        bool mappingExists = lines.Any(line => line.Contains($"CreateMap<{modelName}>"));

        if (!mappingExists)
        {
            int insertIndex = -1;

            // Find the last occurrence of "CreateMap<" in the file
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i].Trim().StartsWith("CreateMap<"))
                {
                    insertIndex = i + 1; // Set insertIndex to the line after the last CreateMap
                    break;
                }
            }

            // Add the new CreateMap definition
            using (StreamWriter sw = new StreamWriter(mapProfilePath, false))
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    sw.WriteLine(lines[i]);
                    if (i == insertIndex - 1)
                    {
                        sw.WriteLine($"        CreateMap<{modelName}, {modelName}Dto>().ReverseMap();");
                    }
                }
            }

            // Print a message indicating the model mapping was successfully added
            System.Console.WriteLine($"{modelName} mapping successfully added.");
        }
        else
        {
            // Notify if the mapping already exists
            System.Console.WriteLine($"{modelName} mapping already exists.");
        }
    }



}
