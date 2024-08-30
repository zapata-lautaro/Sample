#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sample.Console/Sample.Console.csproj", "Sample.Console/"]
COPY ["Sample.Application/Sample.Application.csproj", "Sample.Application/"]
COPY ["Sample.Domain/Sample.Domain.csproj", "Sample.Domain/"]
COPY ["Sample.Infrastructure/Sample.Infrastructure.csproj", "Sample.Infrastructure/"]
RUN dotnet restore "./Sample.Console/Sample.Console.csproj"
COPY . .
WORKDIR "/src/Sample.Console"
RUN dotnet build "./Sample.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sample.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Console.dll"]