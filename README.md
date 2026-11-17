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
   Top-level Package                                           Requested   Resolved
   - DotNetEnv                                                 3.2.0       3.2.0
   - Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore      10.0.13     10.0.13
   - Microsoft.AspNetCore.Identity.EntityFrameworkCore         10.0.13     10.0.13
   - Microsoft.AspNetCore.Identity.UI                          10.0.13     10.0.13
   - Microsoft.EntityFrameworkCore.Design                      10.0.14     10.0.14
   - Microsoft.EntityFrameworkCore.Sqlite                      10.0.14     10.0.14
   - Microsoft.EntityFrameworkCore.Tools                       10.0.14     10.0.14