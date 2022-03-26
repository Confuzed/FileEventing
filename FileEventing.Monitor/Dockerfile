FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

COPY FileEventing.Monitor/FileEventing.Monitor.csproj /app/FileEventing.Monitor/
COPY FileEventing.Shared.Configuration/FileEventing.Shared.Configuration.csproj /app/FileEventing.Shared.Configuration/
COPY FileEventing.Contract/FileEventing.Contract.csproj /app/FileEventing.Contract/
WORKDIR /app/FileEventing.Monitor
RUN dotnet restore

COPY FileEventing.Monitor /app/FileEventing.Monitor/
COPY FileEventing.Shared.Configuration/ /app/FileEventing.Shared.Configuration/
COPY FileEventing.Contract/ /app/FileEventing.Contract/
RUN dotnet publish --configuration Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0

RUN apt update && apt install -y wait-for-it && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/FileEventing.Monitor/out .
COPY FileEventing.Monitor/fe-start-test.sh /app
RUN chmod +x /app/fe-start-test.sh
CMD ["dotnet", "FileEventing.Monitor.dll"]