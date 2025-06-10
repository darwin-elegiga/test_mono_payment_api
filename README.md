[![Quality Gate Status](https://sonarqube.vpayusa.net/api/project_badges/measure?project=engineering_client-payments_payment-api_AX1J9PGB2Z-xByzhep0J&metric=alert_status)](https://sonarqube.vpayusa.net/dashboard?id=engineering_client-payments_payment-api_AX1J9PGB2Z-xByzhep0J)
[![Coverage](https://sonarqube.vpayusa.net/api/project_badges/measure?project=engineering_client-payments_payment-api_AX1J9PGB2Z-xByzhep0J&metric=coverage)](https://sonarqube.vpayusa.net/dashboard?id=engineering_client-payments_payment-api_AX1J9PGB2Z-xByzhep0J)

# VPay Payment API

This repo contains the logic and the API methods that are needed for warranty clients to handle their payments 

## What is in this project?

* Logic
* Web APIs

## Prerequisites
* Docker installed
  * Windows - [Docker CE For Windows](https://store.docker.com/editions/community/docker-ce-desktop-windows)
  * Mac - [Docker CE For Mac](https://store.docker.com/editions/community/docker-ce-desktop-mac)
* Visual Studio 2017
  * Docker Support needs to be enabled

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

## How to setup for Local development
1. Clone Repo
1. Open `VPay.Payment.sln` in Visual Studio 2017
1. Add file `appsettings.Local.json` to project `src/VPay.Payment.Api` and use the following template to setup your local settings
```json
{
  "Logging": {
    "GELF": {
      "AdditionalFields": {
        "environment": "Local"
      }
    }
  },
  "Db2": {
    "Username": "<Db2UserName>",
    "Password": "<Db2Password>"
  }
}
```
