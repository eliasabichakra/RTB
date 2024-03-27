# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7099

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RTB.csproj", "RTB/"]
RUN dotnet restore "RTB/RTB.csproj"
COPY . .
WORKDIR "/src/RTB"
RUN dotnet build "RTB.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "RTB.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RTB.dll"]
