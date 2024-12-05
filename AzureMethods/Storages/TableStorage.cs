using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AzureMethods.Storages.BlobStorage;
using Azure.Data.Tables;
using Azure;
using static AzureMethods.Storages.TableStorage;
using System.Collections.Concurrent;

namespace AzureMethods.Storages
{
    public class TableStorage
    {
        public async Task<BlobBaseClient> GetBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {

            // Query entities with a filter
            await foreach (var entity in tableClient.QueryAsync<MyEntity>(e => e.Age > 25))
            {
                Console.WriteLine($"Name: {entity.Name}, Age: {entity.Age}");
            }
        }


        public async Task<BlobClient> CreateBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {

            // Create a TableServiceClient
            TableServiceClient serviceClient = new TableServiceClient(connectionString);

            // Create a table (if it doesn't already exist)
            TableClient tableClient = serviceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
        }

        public async Task<BlobClient> CreateBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {

            // Define an entity class
            public class MyEntity : ITableEntity
            {
                public string PartitionKey { get; set; }
                public string RowKey { get; set; }
                public string Name { get; set; }
                public int Age { get; set; }

                // Required for Table Storage, but you can leave it unimplemented if you don't use it
                public DateTimeOffset? Timestamp { get; set; }
                public ETag ETag { get; set; }
            }

            string partitionKey = "Partition1";
            string rowKey = "Row1";

            // Create an instance of the entity
            MyEntity entity = new MyEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Name = "John Doe",
                Age = 30
            };

            // Insert the entity into the table
            await tableClient.AddEntityAsync(entity);
        }

        public async Task ModifyBlob(BinaryData binaryData, string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            MyEntity entityToUpdate = await tableClient.GetEntityAsync<MyEntity>(partitionKey, rowKey);

            // Modify the entity
            entityToUpdate.Age = 31; // Update the age

            // Upsert (update or insert)
            await tableClient.UpsertEntityAsync(entityToUpdate);
        }

        public async Task DeleteBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task DeleteBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            await serviceClient.DeleteTableAsync(tableName);
        }
    }
}
