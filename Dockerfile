ARG REGISTRY=plinfharbor.vpayusa.net
ARG DOTNET_VERSION=3.1
ARG DOTNET_SDK_VARIANT=bionic
ARG DOTNET_RUNTIME_VARIANT=bionic
ARG BASE_SDK_IMAGE=dotnet/core/sdk
ARG BASE_RUNTIME_IMAGE=dotnet/core/aspnet

# https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files/
FROM $REGISTRY/base-images/$BASE_SDK_IMAGE:$DOTNET_VERSION-$DOTNET_SDK_VARIANT AS build
ARG PROJECT_TAR=projectfiles.tar
ARG PROJECT_DIR
ARG PROJECT_NAME
WORKDIR /src
COPY $PROJECT_TAR .
RUN tar -xvf $PROJECT_TAR
RUN dotnet restore ${PROJECT_DIR}/${PROJECT_NAME}.csproj
COPY . .
WORKDIR "/src/$PROJECT_DIR"
RUN dotnet build "$PROJECT_NAME.csproj" -c Release -o /app
RUN dotnet publish "$PROJECT_NAME.csproj" -c Release -o /app

FROM $REGISTRY/base-images/$BASE_RUNTIME_IMAGE:$DOTNET_VERSION-$DOTNET_RUNTIME_VARIANT AS final
RUN apt-get update && apt-get install -y tzdata && ln -fs /usr/share/zoneinfo/America/Chicago /etc/localtime && dpkg-reconfigure -f noninteractive tzdata

ARG PROJECT_NAME
ENV PROJECT_NAME=$PROJECT_NAME
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
COPY --from=build /app .
ARG IMAGE_BUILD_TIME
ENV IMAGE_BUILD_TIME ${IMAGE_BUILD_TIME}
ENTRYPOINT "dotnet" "$PROJECT_NAME.dll"
