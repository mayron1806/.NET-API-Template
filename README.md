# run
dotnet run --project ./API/API.csproj --environment "Development"

# run (watch)
dotnet watch run --project ./API/API.csproj --environment "Development"

# create migration
dotnet ef migrations add MIGRATION_NAME --project ./Infrastructure/Infrastructure.csproj -s ./API/API.csproj

# apply migration
dotnet ef database update -s ./API/API.csproj

# remove migration
dotnet ef migrations remove --project ./Infrastructure/Infrastructure.csproj -s ./API/API.csproj

# list migration
dotnet ef migrations list --project ./Infrastructure/Infrastructure.csproj -s ./API/API.csproj
