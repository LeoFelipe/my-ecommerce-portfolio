# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
RUN apt-get update && apt-get install -y curl
USER $APP_UID
EXPOSE 5060
EXPOSE 5061


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/Deliveries/EcommercePortfolio.Delivery.API/EcommercePortfolio.Deliveries.API.csproj", "services/Deliveries/EcommercePortfolio.Delivery.API/"]
COPY ["building-blocks/EcommercePortfolio.Core.ExternalServices/EcommercePortfolio.Core.ExternalServices.csproj", "building-blocks/EcommercePortfolio.Core.ExternalServices/"]
COPY ["building-blocks/EcommercePortfolio.Core/EcommercePortfolio.Core.csproj", "building-blocks/EcommercePortfolio.Core/"]
COPY ["building-blocks/EcommercePortfolio.Services/EcommercePortfolio.Services.csproj", "building-blocks/EcommercePortfolio.Services/"]
COPY ["services/Deliveries/EcommercePortfolio.Delivery.Domain/EcommercePortfolio.Deliveries.Domain.csproj", "services/Deliveries/EcommercePortfolio.Delivery.Domain/"]
COPY ["services/Deliveries/EcommercePortfolio.Delivery.Infra/EcommercePortfolio.Deliveries.Infra.csproj", "services/Deliveries/EcommercePortfolio.Delivery.Infra/"]
RUN dotnet restore "./services/Deliveries/EcommercePortfolio.Delivery.API/EcommercePortfolio.Deliveries.API.csproj"
COPY . .
WORKDIR "/src/services/Deliveries/EcommercePortfolio.Delivery.API"
RUN dotnet build "./EcommercePortfolio.Deliveries.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EcommercePortfolio.Deliveries.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcommercePortfolio.Deliveries.API.dll"]