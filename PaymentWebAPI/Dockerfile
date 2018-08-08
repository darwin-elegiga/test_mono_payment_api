FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 64158
EXPOSE 44319

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY PaymentWebAPI/PaymentWebAPI.csproj PaymentWebAPI/
RUN dotnet restore PaymentWebAPI/PaymentWebAPI.csproj
COPY . .
WORKDIR /src/PaymentWebAPI
RUN dotnet build PaymentWebAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PaymentWebAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentWebAPI.dll"]
