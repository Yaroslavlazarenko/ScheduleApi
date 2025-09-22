FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ScheduleApi.sln", "."]
COPY ["ScheduleApi/ScheduleApi.csproj", "ScheduleApi/"]
COPY ["ScheduleApi.Application/ScheduleApi.Application.csproj", "ScheduleApi.Application/"]
COPY ["ScheduleApi.Infrastructure/ScheduleApi.Infrastructure.csproj", "ScheduleApi.Infrastructure/"]
COPY ["ScheduleApi.Core/ScheduleApi.Core.csproj", "ScheduleApi.Core/"]

RUN dotnet restore "ScheduleApi.sln"

COPY . .

WORKDIR "/src/ScheduleApi"
RUN dotnet publish "ScheduleApi.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ScheduleApi.dll"]