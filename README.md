# Packet-Sniffers
We're gonna sniff your packets bro.

## Generating Database
Make sure you're in the root directory for this project
when you run these commands.

dotnet add package Microsoft.EntityFrameworkCore.Sqlite

dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet add package Microsoft.EntityFrameworkCore.Design 

dotnet ef database update

dotnet build

dotnet watch

## Our Packages

## Packages / Dependencies

| Package | Version | Purpose |
|---|---|---|
| DotNetEnv | 3.2.0 | Loads environment variables from a .env file |
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 10.0.13 | Provides database error pages and diagnostics |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 10.0.13 | Integrates ASP.NET Core Identity with Entity Framework Core |
| Microsoft.AspNetCore.Identity.UI | 10.0.13 | Provides built-in login, registration, and account management UI |
| Microsoft.EntityFrameworkCore.Design | 10.0.14 | Provides design-time tools for Entity Framework Core |
| Microsoft.EntityFrameworkCore.Sqlite | 10.0.14 | Enables SQLite database support through Entity Framework Core |
| Microsoft.EntityFrameworkCore.Tools | 10.0.14 | Provides tools for database migrations and updates |
