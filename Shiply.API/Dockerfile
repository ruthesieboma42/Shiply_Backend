# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy csproj files
COPY ["Shiply.API/Shiply.API.csproj", "Shiply.API/"]
COPY ["Shiply.Application/Shiply.Application.csproj", "Shiply.Application/"]
COPY ["Shiply.Domain/Shiply.Domain.csproj", "Shiply.Domain/"]
COPY ["Shiply.Infrastructure/Shiply.Infrastructure.csproj", "Shiply.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Shiply.API/Shiply.API.csproj"

# Copy everything else
COPY . .

# Move into API project
WORKDIR "/src/Shiply.API"

# Publish
RUN dotnet publish "Shiply.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Shiply.API.dll"]
