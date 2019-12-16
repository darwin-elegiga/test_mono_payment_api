ARG REGISTRY=plinfharbor.vpayusa.net
ARG GIT_COMMIT_SHORT_HASH=docker

FROM payment-api/build-sdk:${GIT_COMMIT_SHORT_HASH} AS build
WORKDIR "/dotnet/VPay.Payment.Api"
RUN dotnet build "VPay.Payment.Api.csproj" -c Release -o /app

FROM ${REGISTRY}/base-images/dotnet/core/aspnet:2.1-bionic AS base
RUN apt-get update && apt-get install -y tzdata && ln -fs /usr/share/zoneinfo/America/Chicago /etc/localtime && dpkg-reconfigure -f noninteractive tzdata
RUN apt-get update && apt-get install -y unixodbc unixodbc-dev
COPY --from=build /dotnet/docker-packages .
COPY --from=build /dotnet/odbc-setup /etc
RUN apt-get install -y ./ibm-iaccess-1.1.0.2-1.0.amd64.deb \
	&& rm -r ibm-iaccess-1.1.0.2-1.0.amd64.deb
WORKDIR /app
EXPOSE 80

FROM build AS publish
RUN dotnet publish "VPay.Payment.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VPay.Payment.Api.dll"]