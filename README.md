# run
dotnet run --project ./FileTransfer.API/FileTransfer.API.csproj --environment "Development"

# run (watch)
dotnet watch run --project ./FileTransfer.API/FileTransfer.API.csproj --environment "Development"

# create migration
dotnet ef migrations add MIGRATION_NAME --project ./FileTransfer.Persistence/FileTransfer.Persistence.csproj -s ./FileTransfer.API/FileTransfer.API.csproj

# apply migration
dotnet ef database update -s ./FileTransfer.API/FileTransfer.API.csproj

# remove migration
dotnet ef migrations remove --project ./FileTransfer.Persistence/FileTransfer.Persistence.csproj -s ./FileTransfer.API/FileTransfer.API.csproj

# list migration
dotnet ef migrations list --project ./FileTransfer.Persistence/FileTransfer.Persistence.csproj -s ./FileTransfer.API/FileTransfer.API.csproj
