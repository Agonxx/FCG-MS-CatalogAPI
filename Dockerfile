FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CatalogAPI.Api/CatalogAPI.Api.csproj", "CatalogAPI.Api/"]
COPY ["CatalogAPI.Application/CatalogAPI.Application.csproj", "CatalogAPI.Application/"]
COPY ["CatalogAPI.Domain/CatalogAPI.Domain.csproj", "CatalogAPI.Domain/"]
COPY ["CatalogAPI.Infrastructure/CatalogAPI.Infrastructure.csproj", "CatalogAPI.Infrastructure/"]
RUN dotnet restore "CatalogAPI.Api/CatalogAPI.Api.csproj"
COPY . .
WORKDIR "/src/CatalogAPI.Api"
RUN dotnet build "CatalogAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatalogAPI.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogAPI.Api.dll"]
