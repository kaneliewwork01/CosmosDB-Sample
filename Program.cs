using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;
using CoreSQL_Sample.Models;

namespace CoreSQL_Sample
{
    public class Program
    {
        //*This example is to demo aboue SQL(Core) API in Azure Cosmos DB*//
        // Note: Container = Table,  item/document = row (JSON documents if SQL API)

        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://01af7c3a-0ee0-4-231-b9ee.documents.azure.com:443/";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "fI7cVKmxWJ6rfnMG7QY18hGMxdeIOZhb2a4J6WfGyvuUhd1ZyQVVEoT0d8VmDeV3umYRtLSb5AKAMHQzbAuO2g==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        // A container can be a collection, graph, or table
        // An item can be a document, edge/vertex, or row, and is the content inside a container
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "FamilyDatabase";
        private string containerId = "FamilyContainer";

        //public static void Main(string[] args)
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                await p.ConnectCosmosDBAccountAsync();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Connect to an Azure Cosmos DB account
        /// </summary>
        /// <returns></returns>
        public async Task ConnectCosmosDBAccountAsync()
        {
            // Connect to Azure Cosmos DB Account
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            // Create Database
            await this.CreateDatabaseAsync();

            // Create Container
            await this.CreateContainerAsync();

            // Create Item
            await this.AddItemsToContainerAsync();

            // Read Items
            await this.QueryItemsAsync();

            // Update Item
            //await this.ReplaceFamilyItemAsync();

            // Delete Item
            //await this.DeleteFamilyItemAsync();

            // Delete Database
            //await this.DeleteDatabaseAndCleanupAsync();
        }

        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        /// <summary>
        /// Create the container if it does not exist. 
        /// Specifiy "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
        /// </summary>
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        /// <summary>
        /// Add Item to Container
        /// </summary>
        /// <returns></returns>
        private async Task AddItemsToContainerAsync()
        {
            #region Create a family object for the Andersen family if not exist
            FamilyModel andersenFamily = new FamilyModel
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new ParentModel[]
                {
                    new ParentModel { FirstName = "Thomas" },
                    new ParentModel { FirstName = "Mary Kay" }
                },
                Children = new ChildModel[]
                {
                    new ChildModel
                    {
                        FirstName = "Henriette Thaulow",
                        Gender = "female",
                        Grade = 5,
                        Pets = new PetModel[]
                        {
                            new PetModel { GivenName = "Fluffy" }
                        }
                    }
                },
                Address = new AddressModel { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = false
            };

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<FamilyModel> andersenFamilyResponse = await this.container.ReadItemAsync<FamilyModel>(andersenFamily.Id, new PartitionKey(andersenFamily.LastName));
                Console.WriteLine("Item in database with id: {0} already exists\n", andersenFamilyResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<FamilyModel> andersenFamilyResponse = await this.container.CreateItemAsync<FamilyModel>(andersenFamily, new PartitionKey(andersenFamily.LastName));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", andersenFamilyResponse.Resource.Id, andersenFamilyResponse.RequestCharge);
            }
            #endregion

            #region Create a family object for the Wakefield Family if not exist
            FamilyModel wakefieldFamily = new FamilyModel
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new ParentModel[]
                {
                    new ParentModel { FamilyName = "Wakefield", FirstName = "Robin" },
                    new ParentModel { FamilyName = "Miller", FirstName = "Ben" }
                },
                Children = new ChildModel[]
                {
                    new ChildModel
                    {
                        FamilyName = "Merriam",
                        FirstName = "Jesse",
                        Gender = "female",
                        Grade = 8,
                        Pets = new PetModel[]
                        {
                            new PetModel { GivenName = "Goofy" },
                            new PetModel { GivenName = "Shadow" }
                        }
                    },
                    new ChildModel
                    {
                        FamilyName = "Miller",
                        FirstName = "Lisa",
                        Gender = "female",
                        Grade = 1
                    }
                },
                Address = new AddressModel { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = true
            };

            try
            {
                // Read the item to see if it exists
                ItemResponse<FamilyModel> wakefieldFamilyResponse = await this.container.ReadItemAsync<FamilyModel>(wakefieldFamily.Id, new PartitionKey(wakefieldFamily.LastName));
                Console.WriteLine("Item in database with id: {0} already exists\n", wakefieldFamilyResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
                ItemResponse<FamilyModel> wakefieldFamilyResponse = await this.container.CreateItemAsync<FamilyModel>(wakefieldFamily, new PartitionKey(wakefieldFamily.LastName));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", wakefieldFamilyResponse.Resource.Id, wakefieldFamilyResponse.RequestCharge);
            }
            #endregion

            #region Create different model Object
            MonsterModel monster01 = new MonsterModel();
            monster01.Id = "fierce-trex-001";
            monster01.IsDanger = true;
            monster01.AverageWeight = 5234.42m;

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<MonsterModel> monster01Response = await this.container.ReadItemAsync<MonsterModel>(monster01.Id, new PartitionKey(monster01.LastName));
                Console.WriteLine("Item in database with id: {0} already exists\n", monster01Response.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<MonsterModel> monster01Response = await this.container.CreateItemAsync<MonsterModel>(monster01, new PartitionKey(monster01.LastName));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", monster01Response.Resource.Id, monster01Response.RequestCharge);
            }
            #endregion
        }

        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// </summary>
        private async Task QueryItemsAsync()
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<FamilyModel> queryResultSetIterator = this.container.GetItemQueryIterator<FamilyModel>(queryDefinition);

            List<FamilyModel> families = new List<FamilyModel>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<FamilyModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (FamilyModel family in currentResultSet)
                {
                    families.Add(family);
                    Console.WriteLine("\tRead {0}\n", family);
                }
            }
        }

        /// <summary>
        /// Replace an item in the container
        /// </summary>
        private async Task ReplaceFamilyItemAsync()
        {
            // Get the item
            ItemResponse<FamilyModel> wakefieldFamilyResponse = await this.container.ReadItemAsync<FamilyModel>("Wakefield.7", new PartitionKey("Wakefield"));
            var itemBody = wakefieldFamilyResponse.Resource;

            // update registration status from false to true
            itemBody.IsRegistered = true;
            // update grade of child
            itemBody.Children[0].Grade = 6;

            // replace the item with the updated content
            wakefieldFamilyResponse = await this.container.ReplaceItemAsync<FamilyModel>(itemBody, itemBody.Id, new PartitionKey(itemBody.LastName));
            Console.WriteLine("Updated Family [{0},{1}].\n \tBody is now: {2}\n", itemBody.LastName, itemBody.Id, wakefieldFamilyResponse.Resource);
        }


        /// <summary>
        /// Delete an item in the container
        /// </summary>
        private async Task DeleteFamilyItemAsync()
        {
            var partitionKeyValue = "Wakefield";
            var familyId = "Wakefield.7";

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<FamilyModel> wakefieldFamilyResponse = await this.container.DeleteItemAsync<FamilyModel>(familyId, new PartitionKey(partitionKeyValue));
            Console.WriteLine("Deleted Family [{0},{1}]\n", partitionKeyValue, familyId);
        }


        /// <summary>
        /// Delete the database and dispose of the Cosmos Client instance
        /// </summary>
        private async Task DeleteDatabaseAndCleanupAsync()
        {
            DatabaseResponse databaseResourceResponse = await this.database.DeleteAsync();
            // Also valid: await this.cosmosClient.Databases["FamilyDatabase"].DeleteAsync();

            Console.WriteLine("Deleted Database: {0}\n", this.databaseId);

            //Dispose of CosmosClient
            this.cosmosClient.Dispose();
        }
    }
}
