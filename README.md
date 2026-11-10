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
