### Add migration file for the project use this command

Install dotnet ef before using command: dotnet tool install --global dotnet-ef

1. Open CMD on root folder of the project.
2. Run the command:
	dotnet ef migrations add update-refreshtoken-and-product-table -c DataContext -o Migrations  --startup-project DataAccessLayer.csproj

	dotnet ef database update  --startup-project DataAccessLayer.csproj --context DataContext

3.Update dotnet runtime CLI
	Use command line, Cmd or PowerShell for specific version:

	dotnet tool update --global dotnet-ef --version 3.1.0

	or for latest version use (works also for reinstallation):

	dotnet tool update --global dotnet-ef
