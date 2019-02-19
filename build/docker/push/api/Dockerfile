ARG REGISTRY=plinfharbor.vpayusa.net
ARG GIT_COMMIT_SHORT_HASH=docker

FROM payment-api/payment-web-api-sdk:${GIT_COMMIT_SHORT_HASH} AS build
RUN mkdir /dotnet/VPay.Payment.Api/publish
RUN dotnet publish "/dotnet/VPay.Payment.Api/VPay.Payment.Api.csproj" --no-restore -c Release -o "/dotnet/VPay.Payment.Api/publish"

FROM ${REGISTRY}/base-images/dotnet-aspnetcore-runtime-2-1:commit-master

# set user, healthcheck, workdir, and command
USER bogner:bogner
ENV ASPNETCORE_URLS=http://+:5000
HEALTHCHECK --interval=10s --timeout=3s --start-period=2m CMD nc -z localhost 5000 || exit 1
WORKDIR /dotnet
CMD ["dotnet", "VPay.Payment.Api.dll"]

# copy application files
COPY --from=build --chown=bogner:bogner \
    /dotnet/VPay.Payment.Api/publish \
    /dotnet
