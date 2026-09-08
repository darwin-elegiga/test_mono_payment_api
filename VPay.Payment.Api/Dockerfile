ARG REGISTRY=centraluhg.jfrog.io/glb-docker-mcr-docker-20200805-rem

FROM ${REGISTRY}/dotnet/aspnet:10.0-noble AS base
ARG IBM_IACCESS_PACKAGE=ibm-iaccess-1.1.0.2-1.0.amd64.deb

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

COPY docker-packages/${IBM_IACCESS_PACKAGE} /tmp/${IBM_IACCESS_PACKAGE}

RUN apt-get update \
	&& apt-get install -y --no-install-recommends libxml2 curl odbcinst unixodbc "/tmp/${IBM_IACCESS_PACKAGE}" \
	&& rm -f "/tmp/${IBM_IACCESS_PACKAGE}" \
	&& rm -rf /var/lib/apt/lists/*

COPY odbc-setup/odbc.ini /etc/odbc.ini
COPY odbc-setup/odbcinst.ini /etc/odbcinst.ini

RUN ldd /opt/ibm/iSeriesAccess/lib64/libcwbodbc.so > /tmp/ibm-iaccess-ldd.txt \
	&& ! grep -q 'not found' /tmp/ibm-iaccess-ldd.txt \
	&& odbcinst -q -d -n 'iSeries Access ODBC Driver' \
	&& odbcinst -q -s -n AS400 \
	&& rm -f /tmp/ibm-iaccess-ldd.txt

ENV LD_LIBRARY_PATH=/app/clidriver/lib
ENV PATH=/app/clidriver/bin:/app/clidriver/lib:${PATH}


FROM ${REGISTRY}/dotnet/sdk:10.0-noble AS build
WORKDIR /src
COPY NuGet.config NuGet.config
COPY Directory.Build.props global.json ./
COPY VPay.Payment.Api/VPay.Payment.Api.csproj VPay.Payment.Api/
COPY VPay.Payment/VPay.Payment.csproj VPay.Payment/
COPY VPay.Payment.Common/VPay.Payment.Common.csproj VPay.Payment.Common/
COPY database/VPay.Payment.Data.Db2/VPay.Payment.Data.Db2.csproj database/VPay.Payment.Data.Db2/
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
