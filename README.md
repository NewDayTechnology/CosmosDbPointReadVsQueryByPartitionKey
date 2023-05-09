# Cosmos DB point reads vs query by partition key RUs consumption for large documents (~100KB)

## Setup

Create a CosmosDB container with `id` as partition key.

1. Set the connection string:

    ```pwsh
    dotnet user-secrets set ConnectionStrings:CosmosDb "AccountEndpoint=foo;AccountKey=bar;"
    ```

1. Configure appsettings.json or set the environment variables `CosmosDb:DatabaseId` and `CosmosDb:ContainerId`

## Run

```pwsh
dotnet run
```
