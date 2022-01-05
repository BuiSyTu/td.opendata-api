	
## Migrations
This command is to be executed from the Host Directory of the project.
```powershell
dotnet ef migrations add <CommitMessage> --project .././Migrators/Migrators.<Provider>/ --context ApplicationDbContext -o Migrations/Application
```
CommitMessage : Enter a commit message here.
Provider : Enter the available DB Provider name. MSSQL , MySQL , PostgreSQL , Oracle
While adding migrations for a particular provider, ensure that you have configured a valid connection string to the provider's database at both `src/Host/Configurations/database.json` and `src/Host/Configurations/hangfire.json`.




dotnet ef migrations add Initial --project .././Migrators/Migrators.MSSQL/ --context ApplicationDbContext -o Migrations/Application
dotnet ef migrations add Initial --project .././Migrators/Migrators.MSSQL/ --context TenantManagementDbContext -o Migrations/Root



dotnet ef database update --context ApplicationDbContext

