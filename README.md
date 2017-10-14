# Cosmos DB Demo

This repository contains a demo of the various ways to use Microsoft's [Azure CosmosDB](https://azure.microsoft.com/en-us/services/cosmos-db/) and includes a simple React front end.

## Getting Started

You need to install the following on your computer:

- [Node.js](https://nodejs.org) >= 8
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2017](https://www.visualstudio.com/)
- [.NET Core](https://www.microsoft.com/net/core#windowscmd) >= 2 (this packaged with Visual Studio 2017)
- [DocumentDB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)

If you environment is set up correctly you should be able to run the following commands on the command line and get the output:

``` bash
node -v
# v8.2.0
dotnet --version
# 2.0.0
```

## Running the app

You will need to run the server and the front end together on your machine.

### Server

To run the server in a few ways:

- Open the folder in Visual Studio Code and press F5
- Open the solution in Visual Studio and press F5
- Open the root folder on the command line and run the following:

  ```bash
  cd server
  dotnet run
  ```

  > *NOTE:* you will also need to set the environment variable `ASPNETCORE_ENVIRONMENT` to `Development` if you want to run the server from the command line.

### Front end

Open the root folder on the command line and rung the following:

```bash
cd client
npm start
```