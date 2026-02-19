# Restaurants API (Clean Architecture + CQRS + EF Core + Azure)

🎓 **Udemy Course:** https://www.udemy.com/course/aspnet-core-web-api-clean-architecture-azure/  
🧪 **Development (Swagger):** https://restaurants-api-dev-app-cqdpgmcgejf6cwhh.polandcentral-01.azurewebsites.net/swagger  
🚀 **Production (Swagger):** https://restaurants-api-prod-app-ane0fthtaue2daet.polandcentral-01.azurewebsites.net/swagger  
🎥 **Walkthrough Video:** *(add your video link here)*  
📜 **Certificate:** https://drive.google.com/file/d/11CF_e1657bjhhq0HsWZyEIH0lMH6ANH2/view?usp=drive_link  

---

## 🍽️ Overview

A layered **ASP.NET Core Web API** built with **Clean Architecture**, **CQRS (MediatR)**, **EF Core**, **FluentValidation**, **AutoMapper**, and a complete set of **unit/integration tests**.

Includes **Seeding**, **Pagination/Sorting/Filtering**, **Authorization** (roles/claims/policies/requirements/resource-based), **Serilog logging** (Console/File/Application Insights), **Azure SQL**, and **Azure Blob Storage**.

---

## ✨ Features

- ✅ Clean Architecture (Domain / Application / Infrastructure / IoC / API)
- ✅ CQRS with MediatR (Commands + Queries per entity)
- ✅ EF Core + Migrations + Configurations + Seeders
- ✅ FluentValidation + Validation Behavior pipeline
- ✅ AutoMapper Profiles + Mapping tests
- ✅ Unit of Work + Repository pattern (refactored for CQRS)
- ✅ Authentication + Authorization:
  - Roles & Claims
  - Policy-based authorization
  - Requirements + Resource-based authorization  
    *(e.g., `RestaurantAuthorizationService`)*
- ✅ Logging:
  - Serilog Console
  - Serilog File
  - Serilog Application Insights
- ✅ Azure:
  - App Service deployments (Dev/Prod)
  - Azure SQL
  - Application Insights telemetry
  - Azure Blob Storage + SAS URL generation
- ✅ Testing:
  - Domain / Application / Infrastructure unit tests
  - API integration tests (controllers + middleware + fake auth policy evaluator)

---

## 🔐 Roles & Access (Admin / Owner / User)

### 🛡️ Admin Role
- Can **assign/upgrade roles** (e.g., promote a User to Owner).
- The **Admin account is added directly to the database via SQL** (manual insert).

### 🧑‍🍳 Owner Role
- Can **create restaurants** (and manage them according to policies).
- To become an **Owner**:
  1) Register normally as a **User**
  2) The **Admin upgrades** your role to **Owner**
  3) Then you can **create your restaurant**

### 🔑 Demo Credentials
- You can get the **Owner** account and **Admin** account from me **after connecting with me**.

---

## 🧱 Architecture (5 Layers)

### 1) Domain
**Pure domain logic**:
- Entities
- Value Objects
- Constants (shared by Application & Infrastructure)
- Exceptions
- Interfaces (e.g., Blob Service & Authorization contracts)
- Repository interfaces

### 2) Application
Use-cases and business workflows:
- CQRS (MediatR): Commands & Queries per entity (DTOs + Handlers + Validators)
- AutoMapper profiles
- FluentValidation + `ValidationBehavior`
- Pagination (`PagedResult`)
- User/Identity:
  - `UserContext`
  - `CurrentUser`

### 3) Infrastructure
Implementation details:
- Persistence (`DbContext`)
- Repository implementations
- Migrations + EF configurations
- Seeding:
  - `IEntitySeeder`
  - `IDbInitializer`
- Storage:
  - Azure Blob implementation
- Authorization:
  - Policy names, requirements, claims principal factory
- Internal visibility + `RegisterInfrastructureServices`

### 4) IoC
Centralized service registration for all layers.

### 5) API
Delivery layer:
- Endpoints (Controllers / Minimal APIs)
- Middlewares (ErrorHandling, RequestTiming, etc.)
- Swagger + Identity endpoints
- Auth setup (Bearer tokens)
- Serilog config (Console/File/AppInsights)
- Authorization policies + requirements

---

## 📁 Solution Structure (Simplified)

```txt
src/
  Restaurants.Domain/
  Restaurants.Application/
  Restaurants.Infrastructure/
  Restaurants.IoC/
  Restaurants.API/

tests/
  Restaurants.Domain.UnitTests/
  Restaurants.Application.UnitTests/
  Restaurants.InfrastructureTests/
  Restaurants.APITests/  take this add details about the admin role that can assign roles and added by sql directly in the data base and there is owner role that can create the restaurants and to be an owner user you must register as user role then the admin user upgrade you to an  owner then you can create your restaurant add that the owner accoutn and the addmin accout you can get it form be after connect with me like comments in linked in post
