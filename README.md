# Cosmos DB Demo

This repository contains a demo using Microsoft's [Azure CosmosDB](https://azure.microsoft.com/en-us/services/cosmos-db/) to build a simple ASPNET Core API for updating and creating user profiles.

## Getting Started

You need to install the following on your computer:

- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2017](https://www.visualstudio.com/)
- [.NET Core](https://www.microsoft.com/net/core#windowscmd) >= 2 (this packaged with Visual Studio 2017)
- [DocumentDB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)

If you environment is set up correctly you should be able to run the following command on the command line and get the output:

``` bash
dotnet --version
# 2.0.0
```

## Running the API

> *NOTE:* You need to start the CosmosDB emulator before running the API

To run the server in a few ways:

- Open the folder in Visual Studio Code and press F5
- Open the solution in Visual Studio and press F5
- Open the root folder on the command line and run the following:

  ```bash
  cd server
  dotnet run
  ```

  > *NOTE:* you will also need to set the environment variable `ASPNETCORE_ENVIRONMENT` to `Development` if you want to run the server from the command line.

## Deploy to Azure

You can deploy this directly to azure

>*NOTE*: This will create a App Service Plan and a CosmosDB account

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)