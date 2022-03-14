FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

COPY FileEventing.Monitor/FileEventing.Monitor.csproj /app/FileEventing.Monitor/
COPY FileEventing.Shared.Configuration/FileEventing.Shared.Configuration.csproj /app/FileEventing.Shared.Configuration/
COPY FileEventing.Abstractions/FileEventing.Abstractions.csproj /app/FileEventing.Abstractions/
WORKDIR /app/FileEventing.Monitor
RUN dotnet restore

COPY FileEventing.Monitor /app/FileEventing.Monitor/
COPY FileEventing.Shared.Configuration/ /app/FileEventing.Shared.Configuration/
COPY FileEventing.Abstractions/ /app/FileEventing.Abstractions/
RUN dotnet publish --configuration Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app/FileEventing.Monitor/out .
ENTRYPOINT ["dotnet", "FileEventing.Monitor.dll"]