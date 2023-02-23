ARG REGISTRY=jfrog.vpayusa.net/plinfharbor

FROM ${REGISTRY}/base-images/dotnet/core/aspnet:2.1-bionic-db2 AS base
EXPOSE 80

FROM ${REGISTRY}/base-images/dotnet/core/sdk:2.1-bionic-db2 AS build
WORKDIR /src
COPY NuGet.config NuGet.config
COPY VPay.Payment.Api/VPay.Payment.Api.csproj VPay.Payment.Api/
RUN dotnet restore VPay.Payment.Api/VPay.Payment.Api.csproj
COPY . .
WORKDIR /src/VPay.Payment.Api
RUN dotnet build VPay.Payment.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VPay.Payment.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VPay.Payment.Api.dll"]
