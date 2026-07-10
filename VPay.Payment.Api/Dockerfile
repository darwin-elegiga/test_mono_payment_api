ARG REGISTRY_URL=centraluhg.jfrog.io
ARG REPO_PATH=glb-docker-mcr-docker-20200805-rem
ARG DOTNET_SDK_VERSION=8.0.403
ARG DOTNET_RUNTIME_VERSION=8.0.21
ARG DOTNET_SDK_VARIANT=jammy
ARG DOTNET_RUNTIME_VARIANT=jammy
ARG BASE_SDK_IMAGE=dotnet/sdk
ARG BASE_RUNTIME_IMAGE=dotnet/aspnet

FROM ${REGISTRY_URL}/${REPO_PATH}/${BASE_RUNTIME_IMAGE}:${DOTNET_RUNTIME_VERSION}-${DOTNET_RUNTIME_VARIANT} AS base
EXPOSE 80

RUN apt-get update && apt-get -y upgrade && apt-get install -y libxml2 curl

ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/bin/Debug/net6.0/clidriver/lib
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/bin/Release/net6.0/clidriver/lib
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/clidriver/lib
ENV PATH=$PATH:/app/bin/Debug/net6.0/clidriver/bin:/app/bin/Debug/net6.0/clidriver/lib
ENV PATH=$PATH:/app/bin/Release/net6.0/clidriver/bin:/app/bin/Release/net6.0/clidriver/lib
ENV PATH=$PATH:/app/bin/Debug/net6.0/clidriver/bin:/app/clidriver/lib


FROM ${REGISTRY_URL}/${REPO_PATH}/${BASE_SDK_IMAGE}:${DOTNET_SDK_VERSION}-${DOTNET_SDK_VARIANT} AS build
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
