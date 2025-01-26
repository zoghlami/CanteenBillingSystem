# Base Image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["src/CanteenBillingSystem.API/CanteenBillingSystem.API.csproj", "CanteenBillingSystem.API/"]
RUN dotnet restore "CanteenBillingSystem.API/CanteenBillingSystem.API.csproj"

# Copy all source files
COPY src/ .
WORKDIR "/src/CanteenBillingSystem.API"

# Build the application
RUN dotnet build "CanteenBillingSystem.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CanteenBillingSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Entry point
ENTRYPOINT ["dotnet", "CanteenBillingSystem.API.dll"]
