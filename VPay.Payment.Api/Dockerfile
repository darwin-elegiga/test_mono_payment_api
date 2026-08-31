ARG REGISTRY=docker.repo1.uhc.com/vpay-docker

FROM ${REGISTRY}/base-images/dotnet/aspnet:10.0-noble-db2 AS base
# Single knob for the target framework used in DB2 clidriver paths below
ARG TFM=net10.0
EXPOSE 80
# .NET 8+ aspnet images default to port 8080; pin the historical port 80 explicitly
# (requires the base image to keep running as root - ports below 1024)
ENV ASPNETCORE_URLS=http://+:80

RUN apt-get update && apt-get -y upgrade && apt-get install -y libxml2 curl

ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/bin/Debug/${TFM}/clidriver/lib
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/bin/Release/${TFM}/clidriver/lib
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/app/clidriver/lib
ENV PATH=$PATH:/app/bin/Debug/${TFM}/clidriver/bin:/app/bin/Debug/${TFM}/clidriver/lib
ENV PATH=$PATH:/app/bin/Release/${TFM}/clidriver/bin:/app/bin/Release/${TFM}/clidriver/lib
ENV PATH=$PATH:/app/bin/Debug/${TFM}/clidriver/bin:/app/clidriver/lib


FROM ${REGISTRY}/base-images/dotnet/sdk:10.0-noble-db2 AS build
WORKDIR /src
COPY nuget.config nuget.config
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
