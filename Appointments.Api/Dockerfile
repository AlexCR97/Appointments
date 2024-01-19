#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Appointments.Api/Appointments.Api.csproj", "Appointments.Api/"]
COPY ["Appointments.Application/Appointments.Application.csproj", "Appointments.Application/"]
COPY ["Appointments.Domain/Appointments.Domain.csproj", "Appointments.Domain/"]
COPY ["Appointments.Infrastructure/Appointments.Infrastructure.csproj", "Appointments.Infrastructure/"]
COPY ["Appointments.Common.MessageBroker/Appointments.Common.MessageBroker.csproj", "Appointments.Common.MessageBroker/"]
COPY ["Appointments.Common.MongoClient/Appointments.Common.MongoClient.csproj", "Appointments.Common.MongoClient/"]
COPY ["Appointments.Common.Secrets/Appointments.Common.Secrets.csproj", "Appointments.Common.Secrets/"]
RUN dotnet restore "Appointments.Api/Appointments.Api.csproj"
COPY . .
WORKDIR "/src/Appointments.Api"
RUN dotnet build "Appointments.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Appointments.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointments.Api.dll"]
