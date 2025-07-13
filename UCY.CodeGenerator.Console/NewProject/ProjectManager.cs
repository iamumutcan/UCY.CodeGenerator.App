using Newtonsoft.Json;
using System.Diagnostics;

namespace UCY.CodeGenerator.Console.NewProject;

public class ProjectManager
{
    private readonly string _configPath;
    private ProjectConfig _config;

    public ProjectManager(string configPath)
    {
        _configPath = configPath;
        LoadConfig();
    }

    private void LoadConfig()
    {
        string jsonString = File.ReadAllText(_configPath);
        _config = JsonConvert.DeserializeObject<ProjectConfig>(jsonString);
    }

    public void Start()
    {
        System.Console.WriteLine("Do you want to create a new project? (y/n):");
        string cevap = System.Console.ReadLine();

        if (cevap?.ToLower() == "y")
        {
            CreateNewProject();
        }
        else
        {
            Log("The new project creation process has been canceled.");
        }
    }

    private void CreateNewProject()
    {
        string projectDirectory = _config.ProjectFilePath;

        if (string.IsNullOrEmpty(projectDirectory))
        {
            Log("You entered an invalid directory, the operation has been canceled.");
            return;
        }

        if (!Directory.Exists(projectDirectory))
        {
            Directory.CreateDirectory(projectDirectory);
            Log($"Main project directory created: {projectDirectory}");
        }

        CreateSolution(projectDirectory, _config.ProjectName);
        CreateProject(_config.ProjectName, projectDirectory, _config.API, "webapi", true, GetAPIReferences());
        CreateProject(_config.ProjectName, projectDirectory, _config.Caching, "classlib", false, GetCachingReferences());
        CreateProject(_config.ProjectName, projectDirectory, _config.Core, "classlib", false, GetCoreReferences());
        CreateProject(_config.ProjectName, projectDirectory, _config.Repository, "classlib", false, GetRepositoryReferences());
        CreateProject(_config.ProjectName, projectDirectory, _config.Service, "classlib", false, GetServiceReferences());
        CreateProject(_config.ProjectName, projectDirectory, _config.Web, "mvc", true, GetWebReferences());


        LoadTemplates loadTemplates = new LoadTemplates();

        Log("The project structure has been successfully created.");
    }

    private void CreateSolution(string basePath, string solutionName)
    {
        string solutionPath = Path.Combine(basePath, $"{solutionName}.sln");

        if (!File.Exists(solutionPath))
        {
            string dotnetNewSlnCmd = $"dotnet new sln -n {solutionName} -o \"{basePath}\"";
            RunCommand(dotnetNewSlnCmd);
            Log($".sln file created: {solutionPath}");
        }
    }

    private void CreateProject(string projectName, string basePath, string layerName, string projectType, bool isApiOrMvc, string references)
    {
        string layerPath = Path.Combine(basePath, $"{projectName}{layerName}");

        if (!Directory.Exists(layerPath))
        {
            Directory.CreateDirectory(layerPath);
            Log($"{projectName}{layerName} layer created: {layerPath}");

            string dotnetNewCmd = $"dotnet new {projectType} -n {projectName}{layerName} -o \"{layerPath}\" -f net8.0"; // .NET 8 sürümü eklendi
            RunCommand(dotnetNewCmd);

            string dotnetSlnCmd = $"dotnet sln \"{basePath}\\{projectName}.sln\" add \"{layerPath}\\{projectName}{layerName}.csproj\"";
            RunCommand(dotnetSlnCmd);

            UpdateCsproj(layerPath, projectName, layerName, references);
        }
        else
        {
            Log($"{projectName}{layerName} layer already exists.");
        }
    }

    private void UpdateCsproj(string layerPath, string projectName, string layerName, string references)
    {
        string csprojFilePath = Path.Combine(layerPath, $"{projectName}{layerName}.csproj");
        if (File.Exists(csprojFilePath))
        {
            string updatedContent = File.ReadAllText(csprojFilePath);
            if (references != null)
            {
                updatedContent = updatedContent.Replace("</Project>", references + "\n</Project>");
                File.WriteAllText(csprojFilePath, updatedContent);
                Log($"{projectName}{layerName}.csproj updated.");
            }
        }
    }

    private string GetAPIReferences()
    {
        return $@"
  <ItemGroup>
     <PackageReference Include=""Autofac.Extensions.DependencyInjection"" Version=""9.0.0"" />
    <PackageReference Include=""Microsoft.AspNetCore.Authentication.JwtBearer"" Version=""8.0.11"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Design"" Version=""8.0.11"">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include=""Microsoft.EntityFrameworkCore.InMemory"" Version=""8.0.11"" />
    <PackageReference Include=""Microsoft.IdentityModel.Tokens"" Version=""8.0.2"" />
    <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""6.7.1"" />
    <PackageReference Include=""System.IdentityModel.Tokens.Jwt"" Version=""8.0.2"" />
    <ProjectReference Include=""..\{_config.ProjectName}{_config.Caching}\{_config.ProjectName}{_config.Caching}.csproj"" />
  </ItemGroup>";
    }

    private string GetCachingReferences()
    {
        return $@"
  <ItemGroup>
    <ProjectReference Include=""..\{_config.ProjectName}{_config.Service}\{_config.ProjectName}{_config.Service}.csproj"" />
  </ItemGroup>";
    }
    private string GetCoreReferences()
    {
        return $@"
  <ItemGroup>
        <PackageReference Include=""Microsoft.AspNetCore.Identity.EntityFrameworkCore"" Version=""8.0.11"" />
  </ItemGroup>";
    }
    private string GetRepositoryReferences()
    {
        return $@"
  <ItemGroup>
    <PackageReference Include=""Microsoft.AspNetCore.Identity.EntityFrameworkCore"" Version=""8.0.11"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.SqlServer"" Version=""8.0.11"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Tools"" Version=""8.0.11"" />
    <ProjectReference Include=""..\{_config.ProjectName}{_config.Core}\{_config.ProjectName}{_config.Core}.csproj"" />
  </ItemGroup>";
    }

    private string GetServiceReferences()
    {
        return $@"
  <ItemGroup>
    <PackageReference Include=""AutoMapper.Extensions.Microsoft.DependencyInjection"" Version=""12.0.1"" />
    <PackageReference Include=""FluentValidation.AspNetCore"" Version=""11.3.0"" />
    <ProjectReference Include=""..\{_config.ProjectName}{_config.Core}\{_config.ProjectName}{_config.Core}.csproj"" />
    <ProjectReference Include=""..\{_config.ProjectName}{_config.Repository}\{_config.ProjectName}{_config.Repository}.csproj"" />
  </ItemGroup>";
    }

    private string GetWebReferences()
    {
        return null; // Web için özel bir referans eklenmeyecekse null dönebilirsiniz
    }

    private void RunCommand(string command)
    {
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", $"/c {command}");
        psi.RedirectStandardOutput = true;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        Process proc = new Process { StartInfo = psi };
        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();
        Log(output);
    }

    private void Log(string message)
    {
        System.Console.WriteLine(message);
    }
}
