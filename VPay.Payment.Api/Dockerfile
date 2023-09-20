ARG REGISTRY=docker.repo1.uhc.com/vpay-docker

FROM ${REGISTRY}/base-images/dotnet/aspnet:6.0-focal-db2 AS base
EXPOSE 80

RUN apt-get update && apt-get -y upgrade && apt-get install -y libxml2 curl

ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/bin/Debug/net6.0/clidriver/lib
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/bin/Release/net6.0/clidriver/lib
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/clidriver/lib
ENV PATH=$PATH:/app/bin/Debug/net6.0/clidriver/bin:/app/bin/Debug/net6.0/clidriver/lib
ENV PATH=$PATH:/app/bin/Release/net6.0/clidriver/bin:/app/bin/Release/net6.0/clidriver/lib
ENV PATH=$PATH:/app/bin/Debug/net6.0/clidriver/bin:/app/clidriver/lib


FROM ${REGISTRY}/base-images/dotnet/sdk:6.0-focal-db2 AS build
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
