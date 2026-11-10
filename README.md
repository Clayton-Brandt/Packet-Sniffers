# Packet-Sniffers
We're gonna sniff your packets bro.

## Generating Database
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet add package Microsoft.EntityFrameworkCore.Design 

dotnet ef database update

dotnet build

dotnet watch
