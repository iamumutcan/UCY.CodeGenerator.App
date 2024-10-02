using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCY.CodeGenerator.Console.NewProject;

public class ProjectConfig
{
    public string ProjectName { get; set; }
    public string ProjectFilePath { get; set; }
    public string API { get; set; }
    public string Caching { get; set; }
    public string Core { get; set; }
    public string Repository { get; set; }
    public string Service { get; set; }
    public string Web { get; set; }
}
