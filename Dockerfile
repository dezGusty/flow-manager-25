# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and project files
COPY FlowManager/*.sln ./
COPY FlowManager/FlowManager.API/*.csproj ./FlowManager.API/
COPY FlowManager/FlowManager.Client/*.csproj ./FlowManager.Client/
COPY FlowManager/FlowManager.Application/*.csproj ./FlowManager.Application/
COPY FlowManager/FlowManager.Domain/*.csproj ./FlowManager.Domain/
COPY FlowManager/FlowManager.Infrastructure/*.csproj ./FlowManager.Infrastructure/
COPY FlowManager/FlowManager.Shared/*.csproj ./FlowManager.Shared/

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY FlowManager/ ./

# Build and publish
RUN dotnet publish FlowManager.API/FlowManager.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Create data directory for SQLite database
RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/FlowManager.db"

# Expose port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run migrations on startup and start app
ENTRYPOINT ["dotnet", "FlowManager.API.dll"]
