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
  "Db2": {
    "UserName": "<Db2UserName>",
    "Password": "<Db2Password>"
  }
}
```
