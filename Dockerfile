#######################################
## This Dockerfile requires BuildKit ##
#######################################

## General arguments
ARG REGISTRY_URL=centraluhg.jfrog.io
ARG REPO_PATH=glb-docker-mcr-docker-20200805-rem
ARG DOTNET_SDK_VERSION=10.0
ARG DOTNET_RUNTIME_VERSION=10.0

## ***Use for dotnet 5.0 and above***
ARG DOTNET_VARIANT=noble
ARG BASE_SDK_IMAGE=dotnet/sdk
ARG BASE_RUNTIME_IMAGE=dotnet/aspnet

## ***Use for dotnet core 3.1 and below***
# ARG DOTNET_SDK_VARIANT=bionic
# ARG DOTNET_RUNTIME_VARIANT=bionic-db2
# ARG BASE_SDK_IMAGE=dotnet/core/sdk
# ARG BASE_RUNTIME_IMAGE=dotnet/core/aspnet

## Build Stage
FROM ${REGISTRY_URL}/${REPO_PATH}/${BASE_SDK_IMAGE}:${DOTNET_SDK_VERSION}-${DOTNET_VARIANT} as build

## Build stage arguments
ARG CONFIG_PROFILE=Release
ARG PROJECT_DIR
ARG PROJECT_NAME

ENV PROJECT=${PROJECT_DIR}/${PROJECT_NAME}.csproj
WORKDIR /app

COPY nuget.config* ./
COPY *.sln ./
COPY Directory.Build.props global.json ./

## Copy .csproj files into the correct file structure
SHELL ["/bin/bash", "-O", "globstar", "-c"]
RUN --mount=target=docker_build_context \
cd docker_build_context;\
cp **/*.csproj ../ --parents;
RUN rm -rf docker_build_context
SHELL ["/bin/sh", "-c"]

## Restore project
RUN dotnet restore ${PROJECT}
## Copy all files if restore succeeds
COPY . ./
## Publish project without restoring
RUN dotnet publish --no-restore -c ${CONFIG_PROFILE} -o /app/out ${PROJECT}

## New stage used to reduce the size of the final image
FROM ${REGISTRY_URL}/${REPO_PATH}/${BASE_RUNTIME_IMAGE}:${DOTNET_RUNTIME_VERSION}-${DOTNET_VARIANT} AS final
## Final stage arguments
ARG PROJECT_NAME
ARG IBM_IACCESS_PACKAGE=ibm-iaccess-1.1.0.2-1.0.amd64.deb

COPY docker-packages/${IBM_IACCESS_PACKAGE} /tmp/${IBM_IACCESS_PACKAGE}

RUN apt-get update \
	&& apt-get install -y --no-install-recommends libxml2 odbcinst unixodbc "/tmp/${IBM_IACCESS_PACKAGE}" \
	&& rm -f "/tmp/${IBM_IACCESS_PACKAGE}" \
	&& rm -rf /var/lib/apt/lists/*

COPY odbc-setup/odbc.ini /etc/odbc.ini
COPY odbc-setup/odbcinst.ini /etc/odbcinst.ini

RUN ldd /opt/ibm/iSeriesAccess/lib64/libcwbodbc.so > /tmp/ibm-iaccess-ldd.txt \
	&& ! grep -q 'not found' /tmp/ibm-iaccess-ldd.txt \
	&& odbcinst -q -d -n 'iSeries Access ODBC Driver' \
	&& odbcinst -q -s -n AS400 \
	&& rm -f /tmp/ibm-iaccess-ldd.txt

WORKDIR /app

COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:80
ENV LD_LIBRARY_PATH=/app/clidriver/lib
ENV PATH=/app/clidriver/bin:/app/clidriver/lib:${PATH}

## Create a symlink so we can use exec form entrypoint
RUN ln -s ${PROJECT_NAME}.dll Entrypoint.dll

ENTRYPOINT [ "dotnet", "Entrypoint.dll" ]

## Optionally add image build time
ARG IMAGE_BUILD_TIME
ENV IMAGE_BUILD_TIME ${IMAGE_BUILD_TIME}

