# CosmosDB-Sample
#Sample SQL-API Azure Cosmos DB project with .Net Core 3.1 Console app in C#.

## Basic Understand of Azure Cosmos DB
1. Is Database in Azure portal. First have to create an Azure account and proceed to create the Azure Cosmos DB Account. (https://azure.microsoft.com/en-us/try/cosmosdb/) Thus, obtained EndpointUri and PrimaryKey for the Azure Cosmos DB Account.

2. Interact with the database via CosmosClient. With different function. Such as CreateDatabaseIfNotExistsAsync, ReadItemAsync, CreateItemAsync, and so on.

3. The container can be a collection, graph, or table. We may understand it as table.

4. An item can be a document, edge/vertex, or row, and is the content inside a container. We may understand it as table data.

5. keep item in json in the container. Can be totally different structure between each item. But the item must have an Id property serialized as id in JSON..

6. Beside the build in method of CosmosClient, also can use CosmosDB Query to interact with the CosmosDB. The Query are Case-sensitive when specify the column/field name. 

7. Create Database, Container, delete database, delete container, and CRUD for items also can be done through CosmosClient methods.

8. Partition Key. In terms of writing efficient queries, Cosmos DB allows you to group a set of items or data in your collection by a similar property determined by the partition key. It is essential to choose the partition key during the design phase of the applications as you cannot change the partition key once the container is created.

## To Test the app
1. Clone the project, and replace the EndpointUri and PrimaryKey with your Azure Cosmos DB Account given. ![image](https://drive.google.com/uc?export=view&id=1kWHI_oqsohQtOoSJvpycwNgPJmu9OrwP)
2. Run the console app project. It will Create database, container, add items, and query the table for the items. You may uncomment the rest of the function calls to explore the rest. ![image](https://drive.google.com/uc?export=view&id=1QS8Yi2FO8TdI_MlNGlP4Ywc6yLQ_LDxZ) ![image](https://drive.google.com/uc?export=view&id=1gTB4460ncCSGX9GvFalG_CA6SSMMo4vP)



3. The processes of this sample are Create Database, Collection with provided name if not exist. Then create items, query items, update item, delete item, and finally delete the database. But in order to see the items created in the Azure Cosmos DB, may temporary comment off the deletion function calls.

4. The items created will looks like this in the Azure Cosmos DB. ![image](https://drive.google.com/uc?export=view&id=1ZLw5L6HzGk6h5hRc2u2R1NAnYLva6Pkd)

 
