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
        public async Task<BaseEntity> GetEntity(string partitionKey,string rowKey, string connectionString,string tableName, bool authRequired = false)
        {
            var tableClient = new TableClient(connectionString, tableName);
            var entity = await tableClient.GetEntityAsync<BaseEntity>(partitionKey,rowKey);
            return entity;
        }

        public async Task CreateEntity(BaseEntity entity,string partitionKey, string rowKey,string connectionString, string tableName, bool authRequired = false)
        {
            var tableClient = new TableClient(connectionString, tableName);
            await tableClient.AddEntityAsync<BaseEntity>(entity);
        }

        public async Task ModifyEntity(string partitionKey, string rowKey, string connectionString, string tableName, bool authRequired = false)
        {
            var tableClient = new TableClient(connectionString, tableName);
            BaseEntity entityToUpdate = await tableClient.GetEntityAsync<BaseEntity>(partitionKey, rowKey);
            await tableClient.UpsertEntityAsync(entityToUpdate);
        }

        public async Task DeleteEntity(string partitionKey, string rowKey, string connectionString, string tableName, bool authRequired = false)
        {
            var tableClient = new TableClient(connectionString, tableName);
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

    }
}
