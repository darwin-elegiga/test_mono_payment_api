FROM centos:7 AS base
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
	# Install .NET Core
	DOTNET_VERSION=2.1.1  \
	ASPNETCORE_VERSION=2.1.1

RUN curl -SL --output dotnethost.rpm https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnethost_sha512='6adeb9acb06893a6a682982fbf3d0eb1a02986cc2f3f35a913a8f1a3e21f4bdb7250c559e8da1ce1ce8ffbf7c7b2a82577f31998342d1af1776382d39267e1d9' \
    && echo "$dotnethost_sha512  dotnethost.rpm" | sha512sum -c - \
    && yum localinstall -y dotnethost.rpm \
    && rm dotnethost.rpm

RUN curl -SL --output dotnetfxr.rpm https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnetfxr_sha512='d1cab5b44c27f807227f2dc339d643bcaaefff6ee891e6c24fc10bb875cb6c561ef8afd07c411b769d43fbff60ad743033a2c5bccde339ff8cf9cbd0f9659fa7' \
    && echo "$dotnetfxr_sha512  dotnetfxr.rpm" | sha512sum -c - \
    && yum localinstall -y dotnetfxr.rpm \
    && rm dotnetfxr.rpm
	
RUN curl -SL --output dotnet-runtime-deps.rpm https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-deps-$DOTNET_VERSION-centos.7-x64.rpm \
    && dotnetruntimedeps_sha512='ab184ec303575339bb0a1cf35da1bf7fbf65ffe3166da9cb9ce28c27746e3897066e45e2602241b07e739741cffd2faf8ac6640778e38d379c769e0f64b336ff' \
    && echo "$dotnetruntimedeps_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && yum localinstall -y dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

RUN curl -SL --output dotnet.rpm https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5602e1898e66d18035cfe46b6a192c8f58ad8232a0e4c13b3a60672f295d5987e893d871b7a5bf95ebadb41e1d625fe0dbb20750cb51ed245dc817d841651d31' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    && yum localinstall -y dotnet.rpm \
    && rm dotnet.rpm

RUN curl -SL --output aspnetcore.rpm https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-x64.rpm \
    && aspnetcore_sha512='a0b788172a10566ff21afda78a265a0c4f4af3ce7b9a33b7b5acea04fd55bdc79cc519af8592372a82c9717535689822579773cc932978a9b13d53f10f993b4a' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && yum localinstall -y aspnetcore.rpm \
    && rm aspnetcore.rpm

RUN yum install -y unixODBC
COPY /docker-packages .
COPY /odbc-setup /etc
RUN yum localinstall -y ibm-iaccess-1.1.0.2-1.0.x86_64.rpm \
	&& rm ibm-iaccess-1.1.0.2-1.0.x86_64.rpm 
EXPOSE 80

FROM microsoft/dotnet:2.1.1-sdk AS build
WORKDIR /src
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
