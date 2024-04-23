# Utilizar la imagen base de ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# Utilizar la imagen base de SDK de .NET Core para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Permissions.API/Permissions.API.csproj", "Permissions.API/"]
COPY ["Permissions.Infrastructure/Permissions.Infrastructure.csproj", "Permissions.Infrastructure/"]
RUN dotnet restore "Permissions.API/Permissions.API.csproj"
COPY . .
WORKDIR "/src/Permissions.API"
RUN dotnet build "Permissions.API.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "Permissions.API.csproj" -c Release -o /app/publish

# Crear la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Permissions.API.dll"]
