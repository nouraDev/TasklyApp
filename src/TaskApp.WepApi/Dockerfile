# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Environment variables to speed up build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Copy csproj and restore dependencies
COPY ["src/TaskApp.WepApi/TaskApp.WepApi.csproj", "TaskApp.WepApi/"]
COPY ["src/TaskApp.Infrastructure/TaskApp.Infrastructure.csproj", "TaskApp.Infrastructure/"]
COPY ["src/TaskApp.Application/TaskApp.Application.csproj", "TaskApp.Application/"]
COPY ["src/TaskApp.Domain/TaskApp.Domain.csproj", "TaskApp.Domain/"]

RUN dotnet restore "TaskApp.WepApi/TaskApp.WepApi.csproj"

# Copy the entire source code
COPY src/ .

# Build and publish
RUN dotnet publish "TaskApp.WepApi/TaskApp.WepApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskApp.WepApi.dll"]
