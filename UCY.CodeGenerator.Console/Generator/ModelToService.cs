﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UCY.CodeGenerator.Console.Config;

namespace UCY.CodeGenerator.Console.Generator
{
    public class ModelToService
    {
        List<ModelProperty> properties = null;

        public string selectedModelName { get; set; }

        // Klasördeki tüm dosyaların yollarını al

        public void StartGenerator()
        {
            SelectModel();
            Generator generator = new Generator();
            System.Console.WriteLine("[1] => Multi DTO");
            System.Console.WriteLine("[2] => Single DTO");
            System.Console.Write("Please make your selection (1 or 2): ");

            // Read the selection from the user
            int select = Convert.ToInt32(System.Console.ReadLine());

            if (select == 1)
            {
                generator.DtoGeneratorMulti(properties, CustomConfig.ModelName);
                generator.AddModelToMapProfileMulti(CustomConfig.ModelName);
            }
            else if (select == 2)
            {
                generator.DtoGeneratorSingle(properties, CustomConfig.ModelName);
                generator.AddModelToMapProfileSingle(CustomConfig.ModelName);
            }
            else
            {
                System.Console.WriteLine("Invalid selection! Please choose 1 or 2.");
                return;
            }
            generator.IRepositoryGenerator();
            generator.IServiceGenerator();
            generator.RepositoryGenerator();
            generator.ServiceGenerator();
            generator.AddModelToDbContext(CustomConfig.ModelName);
            generator.ConfigurationGenerator();
            generator.ApiControllerGenerator(select);


        }
        public void UpdateDtoGenerator()
        {
            SelectModel();
            Generator generator = new Generator();

            // Prompt the user for selection
            System.Console.WriteLine("[1] => Multi DTO");
            System.Console.WriteLine("[2] => Single DTO");
            System.Console.Write("Please make your selection (1 or 2): ");

            // Read the selection from the user
            int select = Convert.ToInt32(System.Console.ReadLine());

            if (select == 1)
            {
                generator.DtoGeneratorMulti(properties, CustomConfig.ModelName);
                generator.AddModelToMapProfileMulti(CustomConfig.ModelName);
            }
            else if (select == 2)
            {
                generator.DtoGeneratorSingle(properties, CustomConfig.ModelName);
                generator.AddModelToMapProfileSingle(CustomConfig.ModelName);
            }

        }
        public void SelectModel()
        {
            // Create the path to the Models folder
            string modelsPath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Core, "Model");

            // List the model files
            string[] modelFiles = Directory.GetFiles(modelsPath, "*.cs");

            System.Console.WriteLine("Found Model Files:");
            for (int i = 0; i < modelFiles.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}: {Path.GetFileName(modelFiles[i])}");
            }

            // Ask the user to select a model
            System.Console.Write("Select a model (number): ");
            if (int.TryParse(System.Console.ReadLine(), out int selectedIndex) && selectedIndex > 0 && selectedIndex <= modelFiles.Length)
            {
                string selectedModelFile = modelFiles[selectedIndex - 1];

                // List the properties of the selected model and store them in the properties array
                properties = GetModelProperties(selectedModelFile);
                CustomConfig.ModelName = Path.ChangeExtension(Path.GetFileName(modelFiles[selectedIndex - 1]), null);
                CustomConfig.ModelNameLower = CustomConfig.ModelName.ToLower();
                System.Console.WriteLine("Model Properties:");
                foreach (var prop in properties)
                {
                    System.Console.WriteLine($"{prop.Type} {prop.Name}");
                }

                // Use the properties to create a new model (if necessary)
            }
            else
            {
                System.Console.WriteLine("Invalid selection.");
            }
        }

        // Method that returns the properties of a model file
        public static List<ModelProperty> GetModelProperties(string modelFilePath)
        {
            try
            {
                // Read the model file
                string code = File.ReadAllText(modelFilePath);

                // Parse the code using Roslyn
                SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

                // List to hold the class and property information
                List<ModelProperty> properties = new List<ModelProperty>();

                // Analyze the classes and their properties
                foreach (var @class in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    foreach (var property in @class.Members.OfType<PropertyDeclarationSyntax>())
                    {
                        var type = property.Type.ToString();
                        var name = property.Identifier.Text;
                        properties.Add(new ModelProperty { Name = name, Type = type });
                    }
                }

                return properties;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Unable to read the model file: {ex.Message}");
                return new List<ModelProperty>();
            }
        }

    }
}
