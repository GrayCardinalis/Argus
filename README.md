## Project launch
	dotnet user-secrets init
	dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=ArgusDb;Username=postgres;Password=5105"
	dotnet user-secrets set "Jwt:Key" ""