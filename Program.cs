using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>().Build();
var cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosDb"));


var container = cosmosClient.GetContainer(databaseId: "ToDoList", "Searches");

// document about 100KB
var item = new Item(Guid.NewGuid().ToString(), new string('a', 102_400));
_ = await container.CreateItemAsync(item);


var pointReadResponse = await container.ReadItemAsync<Item>(item.id, new PartitionKey(item.id));
Console.WriteLine($"Point read: {pointReadResponse.RequestCharge} RUs");


var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
    .WithParameter("@id", item.id);

using var queryResults = container.GetItemQueryIterator<Item>(query);
while (queryResults.HasMoreResults)
{
    var response = await queryResults.ReadNextAsync();
    Console.WriteLine($"Query by partition key: {response.RequestCharge} RUs");
    Console.WriteLine($"Returned items: {response.Count}");
}


record struct Item(string id, string padding);
