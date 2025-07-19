## [1.0.0] – 2025-07-17
### Added
- Layered Architecture template generator (Core, Repository, Service, API, Web layers)  
- AutoMapper + FluentValidation scaffolding  
- ASP.NET Core Identity & JWT authentication  
- Multi-DTO / Single-DTO choice  
- CLI menu (create project, create API, update DTO)

### Fixed
- Register endpoint error handling edge-case

## [1.1.0] – 2025-07-18
### Added
- Generic filter endpoint (`POST /api/{entity}/filter`)  
- `FilterDto` + `PaginationDto<T>` support  
- Auto-generated service & repository methods: `FilterAsync(...)`

## [1.2.0] – 2025-07-19
### Added
- Bulk insert support (`POST /api/{entity}/bulk`) via `AddRange`