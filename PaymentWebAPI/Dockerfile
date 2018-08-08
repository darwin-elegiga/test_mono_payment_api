FROM microsoft/dotnet:2.1.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1.1-sdk AS build
WORKDIR /src
COPY PaymentWebApi/PaymentWebApi.csproj PaymentWebApi/
RUN dotnet restore PaymentWebApi/PaymentWebApi.csproj
COPY . .
WORKDIR /src/PaymentWebApi
RUN dotnet build PaymentWebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PaymentWebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentWebApi.dll"]
