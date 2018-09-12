FROM microsoft/dotnet:2.1.1-aspnetcore-runtime AS base
RUN apt-get update && apt-get install -y unixodbc unixodbc-dev
COPY /docker-packages .
COPY /odbc-setup /etc
RUN apt-get install -y ./ibm-iaccess-1.1.0.2-1.0.amd64.deb \
	&& rm ibm-iaccess-1.1.0.2-1.0.amd64.deb 
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
