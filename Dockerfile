#######################################
## This Dockerfile requires BuildKit ##
#######################################

## General arguments
ARG REGISTRY_URL=centraluhg.jfrog.io
ARG REPO_PATH=glb-docker-mcr-docker-20200805-rem
ARG DOTNET_SDK_VERSION=8.0.404
ARG DOTNET_RUNTIME_VERSION=8.0.22
ARG DOTNET_SDK_VARIANT=jammy
ARG DOTNET_RUNTIME_VARIANT=jammy
ARG BASE_SDK_IMAGE=dotnet/sdk
ARG BASE_RUNTIME_IMAGE=dotnet/aspnet

## Build Stage
FROM ${REGISTRY_URL}/${REPO_PATH}/${BASE_SDK_IMAGE}:${DOTNET_SDK_VERSION}-${DOTNET_SDK_VARIANT} AS build

## Build stage arguments
ARG CONFIG_PROFILE=Release
ARG PROJECT_DIR=VPay.Payment.Api
ARG PROJECT_NAME=VPay.Payment.Api

ENV PROJECT=${PROJECT_DIR}/${PROJECT_NAME}.csproj
WORKDIR /app

COPY nuget.config* ./
COPY *.sln ./

## Copy .csproj files into the correct file structure
SHELL ["/bin/bash", "-O", "globstar", "-c"]
RUN --mount=target=docker_build_context \
cd docker_build_context;\
cp **/*.csproj ../ --parents;
RUN rm -rf docker_build_context
SHELL ["/bin/sh", "-c"]

## Restore project
#RUN dotnet restore ${PROJECT}
## Restore project using BuildKit secrets for NuGet authentication
RUN --mount=type=secret,id=jf-token,env=JF_TOKEN \
    --mount=type=secret,id=jf-user,env=JF_USER \
    dotnet restore ${PROJECT}

## Copy all files if restore succeeds
COPY . ./
## Publish project without restoring
RUN dotnet publish --no-restore -c ${CONFIG_PROFILE} -o /app/out ${PROJECT}
RUN ls -la /app/out

## New stage used to reduce the size of the final image
FROM ${REGISTRY_URL}/${REPO_PATH}/${BASE_RUNTIME_IMAGE}:${DOTNET_RUNTIME_VERSION}-${DOTNET_RUNTIME_VARIANT} AS final
## Final stage arguments
ARG PROJECT_NAME=VPay.Payment.Api

WORKDIR /app

# Create non-root user 'vpay' and set ownership
RUN groupadd -r vpay && useradd -r -g vpay vpay && \
    chown -R vpay:vpay /app
    
COPY --from=build --chown=vpay:vpay /app/out .
ENV ASPNETCORE_URLS=http://+:80

### Create a symlink so we can use exec form entrypoint
#RUN ln -s ${PROJECT_NAME}.dll Entrypoint.dll

# Switch to non-root user
USER vpay

ENTRYPOINT [ "dotnet", "VPay.Payment.Api.dll" ]

## Optionally add image build time
ARG IMAGE_BUILD_TIME
ENV IMAGE_BUILD_TIME=${IMAGE_BUILD_TIME}

