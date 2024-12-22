#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TripAssistant.Api/TripAssistant.Api.csproj", "TripAssistant.Api/"]
RUN dotnet restore "TripAssistant.Api/TripAssistant.Api.csproj"
COPY . .
WORKDIR "/src/TripAssistant.Api"
RUN dotnet build "TripAssistant.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripAssistant.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripAssistant.Api.dll"]