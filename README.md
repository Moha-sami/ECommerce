# ECommerce.API

A .NET Web API for an E-Commerce store, built using **Onion Architecture** with a rich domain model approach.

## 🏗️ Architecture

This project follows Onion Architecture, with strict inward-only dependencies:

```
ECommerce.API            → Presentation (Controllers, Program.cs, Middleware)
ECommerce.Infrastructure → Data access, EF Core, external services
ECommerce.Application    → Use cases, DTOs, service interfaces, business orchestration
ECommerce.Domain         → Core layer — entities, interfaces, no external dependencies
```

**Dependency rule:** Domain depends on nothing. Application depends on Domain. Infrastructure depends on Application (and transitively Domain). API depends on Application + Infrastructure.

## 📦 Tech Stack

- .NET / ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- AutoMapper
- Fluent API (`IEntityTypeConfiguration<T>`) for entity configuration

## 🧱 Domain Model

Rich domain model approach — entities protect their own invariants via private setters and constructor guard clauses (e.g. a `Product` cannot be created with a negative price or empty name).

### Entities

- **Product** — Name, Description, Price, PictureUrl (optional), belongs to one `Brand` and one `ProductType`
- **Brand** — Name, has many `Products`
- **ProductType** — Name, has many `Products`

All entities inherit from `BaseEntity<T>`, which provides:
- `Id`
- `CreatedAt` / `UpdatedAt` (`DateTimeOffset`)
- `IsDeleted` (soft delete flag)

### Relationships

- `Product` → `Brand` (many-to-one, `DeleteBehavior.Restrict` — a Brand with existing Products cannot be deleted)
- `Product` → `ProductType` (many-to-one, `DeleteBehavior.Restrict` — same rule)

### Soft Delete

Implemented at the repository level (explicit flag flip on delete, not a hard `DELETE`), combined with EF Core global query filters (`HasQueryFilter`) so deleted rows are automatically excluded from every query.

## 🗄️ Data Access

- **Generic Repository** (`IGenericRepository<T>`) — Add, Update, Delete, GetById, GetAll — interface in Domain, implementation in Infrastructure
- **Unit of Work** — coordinates repositories and `SaveChangesAsync` (dictionary-based repository resolution)
- **Data Seeding** — JSON-based seed files for `Product`, `Brand`, `ProductType`. Applies pending migrations automatically, checks for existing data before seeding, deserializes via entity constructors (`[JsonConstructor]`) so domain validation still runs during seeding.

## 🧩 Application Layer

- **Result Pattern** — service methods return a `Result` / `Result<T>` wrapper instead of throwing for expected failure cases (e.g. not found), supporting single and multiple error scenarios
- **DTOs** — decouple API contracts from domain entities
- **AutoMapper** — mapping profiles between entities and DTOs
- **Service interfaces** implemented per entity, consumed by controllers

## 🌐 API

- Controllers implemented for Products, Brands, and ProductTypes
- Currently working through: translating `Result<T>` failures into correct HTTP status codes (404, 400, etc.) via a shared base controller pattern, instead of leaking raw failed results to serialization

## 🚧 Roadmap / In Progress

- [ ] Base Controller with shared `Result` → `IActionResult` translation
- [ ] Specification pattern (for flexible filtering / includes without repository bloat)
- [ ] Audit column automation via EF Core `SaveChanges` interceptor
- [ ] Additional entities (Orders, Categories, etc.)

## 📚 Learning Project

This project is being built as a mentorship-style learning exercise, focused on deliberate architectural decisions (soft delete strategy, encapsulation vs. serialization tradeoffs, delete behaviors, DI scoping) rather than just "make it work" code.
