ARG REGISTRY=plinfharbor.vpayusa.net

FROM alpine:latest AS code
COPY . /dotnet
RUN mkdir /csproj \
    && cd /dotnet \
    && find . -type f \
        \( -iname "*.csproj" -o -iname "*.sln" -o -iname "*.dcproj" -o -iname "nuget.config" \) \
        -exec cp --parents {} /csproj \;
        
FROM ${REGISTRY}/base-images/dotnet/core/sdk:2.1-bionic AS build
WORKDIR /dotnet

# copy csproj files and resotre
COPY --from=code /csproj /dotnet
RUN dotnet restore VPay.Payment.sln

# copy all code
COPY --from=code /dotnet /dotnet