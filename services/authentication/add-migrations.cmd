REM dotnet ef migrations add %1 --project ..\..\libs\entities\Fanda.Entities.csproj --startup-project Fanda.Auth.csproj --context AuthContext
dotnet ef migrations add %1 -o Migrations/%2