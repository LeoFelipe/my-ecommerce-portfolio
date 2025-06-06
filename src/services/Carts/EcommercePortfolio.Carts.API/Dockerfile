# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
RUN apt-get update && apt-get install -y curl
USER $APP_UID
EXPOSE 5050


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/Carts/EcommercePortfolio.Carts.API/EcommercePortfolio.Carts.API.csproj", "services/Carts/EcommercePortfolio.Carts.API/"]
COPY ["building-blocks/EcommercePortfolio.ApiGateways/EcommercePortfolio.ApiGateways.csproj", "building-blocks/EcommercePortfolio.ApiGateways/"]
COPY ["building-blocks/EcommercePortfolio.Core/EcommercePortfolio.Core.csproj", "building-blocks/EcommercePortfolio.Core/"]
COPY ["building-blocks/EcommercePortfolio.Services/EcommercePortfolio.Services.csproj", "building-blocks/EcommercePortfolio.Services/"]
COPY ["services/Orders/EcommercePortfolio.Orders.Domain/EcommercePortfolio.Orders.Domain.csproj", "services/Orders/EcommercePortfolio.Orders.Domain/"]
COPY ["services/Orders/EcommercePortfolio.Orders.Infra/EcommercePortfolio.Orders.Infra.csproj", "services/Orders/EcommercePortfolio.Orders.Infra/"]
COPY ["services/Carts/EcommercePortfolio.Carts.Domain/EcommercePortfolio.Carts.Domain.csproj", "services/Carts/EcommercePortfolio.Carts.Domain/"]
COPY ["services/Carts/EcommercePortfolio.Carts.Infra/EcommercePortfolio.Carts.Infra.csproj", "services/Carts/EcommercePortfolio.Carts.Infra/"]
RUN dotnet restore "./services/Carts/EcommercePortfolio.Carts.API/EcommercePortfolio.Carts.API.csproj"
COPY . .
WORKDIR "/src/services/Carts/EcommercePortfolio.Carts.API"
RUN dotnet build "./EcommercePortfolio.Carts.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EcommercePortfolio.Carts.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcommercePortfolio.Carts.API.dll"]