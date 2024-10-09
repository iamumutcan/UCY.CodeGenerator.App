using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCY.CodeGenerator.Console.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            generator.IRepositoryGenerator();
            generator.IServiceGenerator();
            generator.RepositoryGenerator();
            generator.ServiceGenerator();
            generator.DtoGenerator(properties);
            generator.ApiControllerGenerator();
            generator.AddModelToDbContext(CustomConfig.ModelName);
            generator.AddModelToMapProfile(CustomConfig.ModelName);

        }
        public void SelectModel()
        {
            // Model klasörünün yolunu oluştur
            string modelsPath = Path.Combine(CustomConfig.ProjectFilePath, CustomConfig.ProjectName + CustomConfig.Core, "Model");

            // Model dosyalarını listele
            string[] modelFiles = Directory.GetFiles(modelsPath, "*.cs");

            System.Console.WriteLine("Bulunan Model Dosyaları:");
            for (int i = 0; i < modelFiles.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}: {Path.GetFileName(modelFiles[i])}");
            }

            // Kullanıcıdan bir model seçmesini iste
            System.Console.Write("Bir model seçin (numara): ");
            if (int.TryParse(System.Console.ReadLine(), out int selectedIndex) && selectedIndex > 0 && selectedIndex <= modelFiles.Length)
            {
                string selectedModelFile = modelFiles[selectedIndex - 1];

                // Seçilen modelin özelliklerini listele ve dizide tut
                properties = GetModelProperties(selectedModelFile);
                CustomConfig.ModelName = Path.ChangeExtension(Path.GetFileName(modelFiles[selectedIndex - 1]), null);
                CustomConfig.ModelNameLower = CustomConfig.ModelName.ToLower();
                System.Console.WriteLine("Model Özellikleri:");
                foreach (var prop in properties)
                {
                     System.Console.WriteLine($"{prop.Type} {prop.Name}");
                }

                // Yeni model oluşturmak için özelllikleri kullan


            }
            else
            {
                System.Console.WriteLine("Geçersiz seçim.");
            }
        }
        // Model dosyasının özelliklerini döndüren metod
        public static List<ModelProperty> GetModelProperties(string modelFilePath)
        {
            try
            {
                // Model dosyasını oku
                string code = File.ReadAllText(modelFilePath);

                // Roslyn ile kodu parse et
                SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

                // Sınıf ve özellikleri tutacak liste
                List<ModelProperty> properties = new List<ModelProperty>();

                // Sınıfları ve özellikleri analiz et
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
                System.Console.WriteLine($"Model dosyası okunamadı: {ex.Message}");
                return new List<ModelProperty>();
            }
        }
    }
}
