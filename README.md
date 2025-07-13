
# 🔧 UCY Code Generator – README

**“Create one model, get a full REST API in seconds.”**

---

## 📌 Overview

UCY Code Generator is a **.NET 8** CLI tool that scaffolds an entire **Layered Architecture** solution from a single C# model class.  
It automatically generates:

* DTOs (Single or Multiple)
* AutoMapper profiles
* Repository & Service layers (full CRUD)
* Entity Framework Core DbContext updates
* ASP.NET Core Web API Controllers
* JWT & Identity support

---

## 🚀 Quick Start

1. **Clone / unzip** the generator.
2. Edit `config.json` with your project names & paths.
3. Run the console app and follow the menu.

---

## 🛠️ Initial Setup

### 1. `config.json`

Place at the root of the generator folder:

```json
{
  "ProjectName": "MyApp",
  "ProjectFilePath": "C:\\ProjeDizini\\MyApp",
  "API": ".API",
  "Caching": ".Caching",
  "Core": ".Core",
  "Repository": ".Repository",
  "Service": ".Service",
  "Web": ".Web"
}
```

| Key            | Purpose |
|----------------|---------|
| `ProjectName`  | Root namespace & solution name |
| `ProjectFilePath` | Where the `.sln` and folders will be created |
| Remaining keys | Suffixes for each layer (e.g., `MyApp.API`) |

---

## 🧑‍💻 Developer Workflow

1. **Create a new project** (run option `1`).  
   A full Layered Architecture skeleton is generated:
   ```
   MyApp.sln
   ├─ MyApp.API
   ├─ MyApp.Caching
   ├─ MyApp.Core
   ├─ MyApp.Repository
   ├─ MyApp.Service
   └─ MyApp.Web
   ```

2. **Add your model class**  
   Inside `MyApp.Core\Model`, e.g.  
   `Product.cs`
   ```csharp
   public class Product
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public decimal Price { get; set; }
   }
   ```

3. **Run the generator again**, option `2`  
   * Pick the model from the list  
   * Choose:  
     **1** – Multi-DTO (Create, Update, List, …)  
     **2** – Single DTO

4. **Done!**  
   The following are auto-created and wired-up:
   * DTOs & AutoMapper profile
   * `IRepository`, `Repository`
   * `IService`, `Service`
   * `DbSet<Product>` added to `ApplicationDbContext`
   * ASP.NET Core controller (`ProductsController`) with full CRUD endpoints
   * JWT & Identity integration (already in `.API`)

---

## 🔁 Updating DTOs Later

Use menu option `3` to regenerate DTOs after model changes.  
Existing business & data layers remain untouched.

---

## 🧰 Project Structure After Generation

```
MyApp/
├── MyApp.Core/
│   ├── DTOs/
│   ├── Enums/
│   ├── Model/
│   ├── UnitOfWorks/
│   ├── Repositories/
│   └── Services/
├── MyApp.Repository/
│   ├── UnitOfWorks/
│   └── Repositories/
├── MyApp.Service/
│   ├── Exceptions/
│   ├── Mapping/
│   ├── Validations/
│   └── Services/
├── MyApp.API/
│   ├── Filters/
│   ├── Middlewares/
│   ├── Modules/
│   ├── Controllers/
│   └── Program.cs
└── ...
```

---

## 📦 Dependencies

The tool automatically injects the latest stable NuGet packages:

| Layer      | Key Packages |
|------------|--------------|
| .API       | Microsoft.AspNetCore.Authentication.JwtBearer, Swashbuckle, EF Core In-Memory |
| .Repository | EF Core SQL Server, Identity |
| .Service   | AutoMapper, FluentValidation |
| .Caching   | Depends on .Service |
| .Core      | ASP.NET Core Identity |

---

## 📋 Console Menu

```
[1] => Create New Project
[2] => Create New API (from model)
[3] => Update DTO
[x] => Exit
```

---

## 🧪 Example

1. **Model**  
   `Car.cs`
   ```csharp
   public class Car
   {
       public int Id { get; set; }
       public string Brand { get; set; }
       public int Year { get; set; }
   }
   ```

2. **Choose option 2 → Multi-DTO (1)**  
   Generated files (excerpt):

   ```
   MyApp.Service/DTOs/Car/CreateCarDto.cs
   MyApp.Service/DTOs/Car/CarDto.cs
   MyApp.Service/DTOs/Car/UpdateCarDto.cs
   MyApp.Service/Profiles/CarProfile.cs
   MyApp.Repository/Concrete/CarRepository.cs
   MyApp.Service/Concrete/CarService.cs
   MyApp.API/Controllers/CarsController.cs
   ```

3. **Run the API**  
   `dotnet run --project MyApp.API`  
   Swagger UI available at `https://localhost:5001/swagger`

---

## 📝 Notes

* Supports **.NET 8** (SDK must be installed).

---

## 🤝 Contributing

Issues & PRs welcome.  
Please include sample model & expected output when reporting bugs.

---

## 📄 License

MIT – use freely in personal & commercial projects.
```