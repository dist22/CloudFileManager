FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cloud.API/Cloud.API.csproj", "Cloud.API/"]
COPY ["Cloud.Application/Cloud.Application.csproj", "Cloud.Application/"]
COPY ["Cloud.Domain/Cloud.Domain.csproj", "Cloud.Domain/"]
COPY ["Cloud.Infrastructure/Cloud.Infrastructure.csproj", "Cloud.Infrastructure/"]
RUN dotnet restore "Cloud.API/Cloud.API.csproj"
COPY . .
WORKDIR "/src/Cloud.API"
RUN dotnet build "./Cloud.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cloud.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cloud.API.dll"]
