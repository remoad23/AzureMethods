using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Data.Tables;
using Azure.Storage.Files.Shares;


namespace AzureMethods.Storages
{
    public class StorageCaller
    {

        public async Task GetQueueStorage()
        {
            // Queue service client
            QueueServiceClient queueServiceClient = new QueueServiceClient(connectionString);

            // List all queues
            await foreach (var queue in queueServiceClient.GetQueuesAsync())
            {
                Console.WriteLine($"Queue Name: {queue.Name}");
            }
        }

        public async Task GetFileStorage()
        {
            // File share service client
            ShareServiceClient shareServiceClient = new ShareServiceClient(connectionString);

            // List all file shares
            await foreach (var share in shareServiceClient.GetSharesAsync())
            {
                Console.WriteLine($"File Share Name: {share.Name}");
            }
        }

        public async Task GetTableStorage()
        {
            // Table service client
            TableServiceClient tableServiceClient = new TableServiceClient(connectionString);

            // List all tables
            await foreach (var table in tableServiceClient.GetTablesAsync())
            {
                Console.WriteLine($"Table Name: {table.Name}");
            }
        }
    }
}
