ARG REGISTRY=plinfharbor.vpayusa.net

FROM alpine:latest AS code
COPY . /dotnet
RUN mkdir /csproj \
    && cd /dotnet \
    && find . -type f \
        \( -iname "*.csproj" -o -iname "*.sln" -o -iname "nuget.config" \) \
        -exec cp --parents {} /csproj \;

FROM ${REGISTRY}/base-images/dotnet-sdk-2-1:commit-master

# set user, healthcheck, workdir, and command
USER bogner:bogner
ENV ASPNETCORE_URLS=http://+:5000
HEALTHCHECK --interval=10s --timeout=3s --start-period=2m CMD nc -z localhost 5000 || exit 1
WORKDIR /dotnet/VPay.Payment.Api
CMD ["dotnet", "run", "-c", "Debug"]

# copy csproj files and restore
COPY --from=code --chown=bogner:bogner /csproj /dotnet
RUN dotnet restore

# copy all code
COPY --from=code --chown=bogner:bogner /dotnet /dotnet
RUN dotnet build
