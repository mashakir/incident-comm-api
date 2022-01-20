# Database Migration

### Migration for `Incident.Comm.Integration.Data`

#### Using Package Manager Console
- Open > NuGet Package Manager Console
- Select `Incident.Comm.Integration.Data` in `Default Project` dropdown
- Run following commands:
  > `Add-Migration -Context IncidentCommDbContext -StartupProject Incident.Comm.Integration.Api <migration-name>`
  > `Update-Database`
#### Using `dotnet` CLI from command prompt:
- Install `dotnet-ef` globally
  > dotnet tool install --global dotnet-ef
- Open terminal
- Change directory to `Incident.Comm.Updates.Integration.Data`
- Run following command:
  > dotnet ef migrations add <migration-name> --context IncidentCommDbContext --startup-project ../Incident.Comm.Integration.Api
  > `Update-Database`