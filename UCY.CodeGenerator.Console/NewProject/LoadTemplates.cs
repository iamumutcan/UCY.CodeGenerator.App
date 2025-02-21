﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UCY.CodeGenerator.Console.Config;

namespace UCY.CodeGenerator.Console.NewProject
{
    public class LoadTemplates
    {
        private static LoadTemplates _instance;

        public static LoadTemplates Instance => _instance ??= new LoadTemplates();

        public string TemplatesPath { get; private set; }

        public List<LoadTemplateBaseClass> Templates { get; private set; } = new List<LoadTemplateBaseClass>();

        public LoadTemplates()
        {
            string _TemplatesPath = AppDomain.CurrentDomain.BaseDirectory;
            TemplatesPath = Path.Combine(_TemplatesPath, @"..\..\..\NewProject\Templates");
            DtoPathLoad();
            CorePathLoad();
            RepositoryPathLoad();
            SerivcePathLoad();
            APIPathLoad();
        }
        public void DtoPathLoad()
        {
            Templates.Clear();
            // File names and directories
            var dtoFiles = new[] { "BaseDto", "CustomResponseDto", "NoContentDto", "PaginationDto", "UserDto", "AuthLoginRequestDto", "RegisterRequestDto", "RegisterResponseDto", "UserWithRolesDto" };
            // Load all templates
            LoadFiles(dtoFiles, "Dto", "DTos\\Base");
            LoadFiles(dtoFiles, "Dto", "DTos\\User\\Request");
            LoadFiles(dtoFiles, "Dto", "DTos\\User\\Response");
            LoadFiles(dtoFiles, "Dto", "DTos\\Auth\\Request");
            LoadFiles(dtoFiles, "Dto", "DTos\\Auth\\Response");
            foreach (var selectedTemplate in Templates)
            {
                BaseClassGenerator(selectedTemplate.Value, selectedTemplate.FullPath());

            }
        }
        public void CorePathLoad()
        {
            Templates.Clear();
            // File names and directories
            var modelFiles = new[] { "BaseEntity", "IBaseEntity", "JwtSettings", "PaginationModel", "User", "UserRole","Role" };
            var repositoryFiles = new[] { "IGenericRepository", "IUserRepository" , "IUserRoleRepository" };
            var serviceFiles = new[] { "IService", "IUserService", "IUserRoleService" };
            var unitOfWorkFiles = new[] { "IUnitOfWork" };

            // Load all templates
            LoadFiles(modelFiles, "Core", "Model");
            LoadFiles(repositoryFiles, "Core", "Repositories");
            LoadFiles(serviceFiles, "Core", "Services");
            LoadFiles(unitOfWorkFiles, "Core", "UnitOfWorks");
            foreach (var selectedTemplate in Templates) {
                BaseClassGenerator(selectedTemplate.Value,selectedTemplate.FullPath());

            }
        }
        public void RepositoryPathLoad()
        {
            Templates.Clear();
            // File names and directories
            var repositoriesFiles = new[] { "GenericRepository", "UserRepository", "UserRoleRepository" };
            var unitOfWorksFiles = new[] { "UnitOfWork" };
            var baseRepositoryFiles = new[] { "AppDbContext" };

            // Load all templates
            LoadFiles(repositoriesFiles, "Repository", "Repositories");
            LoadFiles(unitOfWorksFiles, "Repository", "UnitOfWorks");
            LoadFiles(baseRepositoryFiles, "Repository", "");
            foreach (var selectedTemplate in Templates)
            {
                BaseClassGenerator(selectedTemplate.Value, selectedTemplate.FullPath());

            }
        }

        public void SerivcePathLoad()
        {
            Templates.Clear();
            // File names and directories
            var exceptionsFiles = new[] { "AuthorizationException", "ClientSideException" , "NotFoundExcepiton" };
            var mappingFiles = new[] { "MapProfile" };
            var serviceFiles = new[] { "Service", "UserService", "UserRoleService" };
            var validationsFiles = new[] { "UserDtoValidator" };

            // Load all templates
            LoadFiles(exceptionsFiles, "Service", "Exceptions");
            LoadFiles(mappingFiles, "Service", "Mapping");
            LoadFiles(serviceFiles, "Service", "Services");
            LoadFiles(validationsFiles, "Service", "Validations");

            foreach (var selectedTemplate in Templates) 
            {
                BaseClassGenerator(selectedTemplate.Value, selectedTemplate.FullPath());
            }
            Directory.CreateDirectory(Path.GetDirectoryName($"{CustomConfig.ProjectFilePath}\\{CustomConfig.ProjectName}{CustomConfig.Service}\\Validations"));

        }
        private void LoadFiles(IEnumerable<string> fileNames,string layer, string folder)
        {
            foreach (var fileName in fileNames)
            {
                string filePath;
                if (layer==null || layer=="") filePath = Path.Combine(TemplatesPath, folder, $"{fileName}.txt");
                else  filePath = Path.Combine(TemplatesPath, layer+"\\"+folder, $"{fileName}.txt");
                if (File.Exists(filePath))
                {
                    LoadTemplateBaseClass loadTemplateBaseClass = new LoadTemplateBaseClass();
                    loadTemplateBaseClass.Name = fileName;
                    loadTemplateBaseClass.Value= File.ReadAllText(filePath);
                    loadTemplateBaseClass.Layer = layer;
                    loadTemplateBaseClass.Path=folder;
                    Templates.Add(loadTemplateBaseClass);
                }
                else
                {
                    System.Console.WriteLine($"File not found: {filePath}");
                }
            }
        }

        public void APIPathLoad()
        {
            Templates.Clear();
            // File names and directories
            var controllersFiles = new[] { "CustomBaseController", "UserController", "AuthController" };
            var filtersFiles = new[] { "NotFoundFilter", "ValidateFilterAttribute" };
            var middlewaresFiles = new[] { "UseCustomExceptionHandler" };
            var modulesFiles = new[] { "RepoServiceModule" };
            var baseRepositoryFiles = new[] { "Program" };

            // Load all templates
            LoadFiles(controllersFiles, "API", "Controllers");
            LoadFiles(filtersFiles, "API", "Filters");
            LoadFiles(middlewaresFiles, "API", "Middlewares");
            LoadFiles(modulesFiles, "API", "Modules");
            LoadFiles(baseRepositoryFiles, "API", "");
            foreach (var selectedTemplate in Templates)
            {
                BaseClassGenerator(selectedTemplate.Value, selectedTemplate.FullPath());
            }
            string readappsettingfile = File.ReadAllText(Path.Combine(CustomConfig.projectDirectory, @"..\..\..\NewProject\Templates\API\appsettings.json"));
            string writeAppsettingfile = $"{CustomConfig.ProjectFilePath}\\{CustomConfig.ProjectName}{CustomConfig.API}\\appsettings.json";
            BaseClassGenerator(readappsettingfile, writeAppsettingfile);
        }
        public void BaseClassGenerator(string _template, string _path)
        {
            string generatorCode = _template
                .Replace("{{ProjectName}}", CustomConfig.ProjectName)
                .Replace("{{CoreLayer}}", CustomConfig.Core)
                .Replace("{{DtoLayer}}", CustomConfig.Dto)
                .Replace("{{APILayer}}", CustomConfig.API)
                .Replace("{{RepositoryLayer}}", CustomConfig.Repository)
                .Replace("{{ServiceLayer}}", CustomConfig.Service)
                .Replace("{{WebLayer}}", CustomConfig.Web)
                .Replace("{{CachingLayer}}", CustomConfig.Caching)
                .Replace("{{modelNameLower}}", CustomConfig.ModelNameLower)
                .Replace("{{ModelName}}", CustomConfig.ModelName);
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, generatorCode);
        }
    }

    public class LoadTemplateBaseClass
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Layer {  get; set; }
        public string Path { get; set; }
        public string FullPath()
        {
            string fullpath;
            if (Layer == null || Layer=="") fullpath = $"{CustomConfig.ProjectFilePath}\\{CustomConfig.ProjectName}.{this.Layer}\\{this.Name}.cs";
            else fullpath = $"{CustomConfig.ProjectFilePath}\\{CustomConfig.ProjectName}.{this.Layer}\\{this.Path}\\{this.Name}.cs";
            return fullpath;
        }
    }
}
