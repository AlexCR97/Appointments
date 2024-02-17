#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# api
COPY ["Appointments.Api/Appointments.Api.csproj", "Appointments.Api/"]
COPY ["Appointments.Api.Assets/Appointments.Api.Assets.csproj", "Appointments.Api.Assets/"]
COPY ["Appointments.Api.Client/Appointments.Api.Client.csproj", "Appointments.Api.Client/"]
COPY ["Appointments.Api.Connect/Appointments.Api.Connect.csproj", "Appointments.Api.Connect/"]
COPY ["Appointments.Api.Core/Appointments.Api.Core.csproj", "Appointments.Api.Core/"]
COPY ["Appointments.Api.Infrastructure/Appointments.Api.Infrastructure.csproj", "Appointments.Api.Infrastructure/"]
COPY ["Appointments.Api.Tenant/Appointments.Api.Tenant.csproj", "Appointments.Api.Tenant/"]

# common
COPY ["Appointments.Common.Application/Appointments.Common.Application.csproj", "Appointments.Common.Application/"]
COPY ["Appointments.Common.Domain/Appointments.Common.Domain.csproj", "Appointments.Common.Domain/"]
COPY ["Appointments.Common.MessageBroker/Appointments.Common.MessageBroker.csproj", "Appointments.Common.MessageBroker/"]
COPY ["Appointments.Common.MongoClient/Appointments.Common.MongoClient.csproj", "Appointments.Common.MongoClient/"]
COPY ["Appointments.Common.Secrets/Appointments.Common.Secrets.csproj", "Appointments.Common.Secrets/"]
COPY ["Appointments.Common.Utils/Appointments.Common.Utils.csproj", "Appointments.Common.Utils/"]

# modules / assets
COPY ["Appointments.Assets/Appointments.Assets.csproj", "Appointments.Assets/"]
COPY ["Appointments.Assets.Application/Appointments.Assets.Application.csproj", "Appointments.Assets.Application/"]
COPY ["Appointments.Assets.Domain/Appointments.Assets.Domain.csproj", "Appointments.Assets.Domain/"]
COPY ["Appointments.Assets.Infrastructure/Appointments.Assets.Infrastructure.csproj", "Appointments.Assets.Infrastructure/"]

# modules / core
COPY ["Appointments.Core/Appointments.Core.csproj", "Appointments.Core/"]
COPY ["Appointments.Core.Application/Appointments.Core.Application.csproj", "Appointments.Core.Application/"]
COPY ["Appointments.Core.Contracts/Appointments.Core.Contracts.csproj", "Appointments.Core.Contracts/"]
COPY ["Appointments.Core.Domain/Appointments.Core.Domain.csproj", "Appointments.Core.Domain/"]
COPY ["Appointments.Core.Infrastructure/Appointments.Core.Infrastructure.csproj", "Appointments.Core.Infrastructure/"]

# modules / jobs
COPY ["Appointments.Jobs/Appointments.Jobs.csproj", "Appointments.Jobs/"]
COPY ["Appointments.Jobs.Application/Appointments.Jobs.Application.csproj", "Appointments.Jobs.Application/"]
COPY ["Appointments.Jobs.Domain/Appointments.Jobs.Domain.csproj", "Appointments.Jobs.Domain/"]
COPY ["Appointments.Jobs.Infrastructure/Appointments.Jobs.Infrastructure.csproj", "Appointments.Jobs.Infrastructure/"]

# modules / notifications
COPY ["Appointments.Notifications/Appointments.Notifications.csproj", "Appointments.Notifications/"]
COPY ["Appointments.Notifications.Application/Appointments.Notifications.Application.csproj", "Appointments.Notifications.Application/"]
COPY ["Appointments.Notifications.Infrastructure/Appointments.Notifications.Infrastructure.csproj", "Appointments.Notifications.Infrastructure/"]

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
