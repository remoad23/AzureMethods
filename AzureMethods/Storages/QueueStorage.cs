using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AzureMethods.Storages.BlobStorage;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureMethods.Storages
{
    public class QueueStorage
    {
        public async Task<BlobBaseClient> GetBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            
            // Create a QueueServiceClient to interact with the Queue service
            QueueServiceClient queueServiceClient = new QueueServiceClient(connectionString);

            // List all the queues
            await foreach (QueueItem queueItem in queueServiceClient.GetQueuesAsync())
            {
                Console.WriteLine($"Queue name: {queueItem.Name}");
            }

            return blobClient;
        }


        public async Task<BlobClient> CreateBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            // Create a QueueClient
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            // Ensure the queue exists
            await queueClient.CreateIfNotExistsAsync();

            // Add a message to the queue
            string message = "Hello, Queue!";
            await queueClient.SendMessageAsync(message);
        }

        public async Task ModifyBlob(BinaryData binaryData, string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            // Create a QueueClient
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            QueueMessage message = new();

            string updatedMessage = "Updated content!";

            // Update the message
            await queueClient.UpdateMessageAsync(
                message.MessageId,
                message.PopReceipt,
                updatedMessage,
                visibilityTimeout: TimeSpan.FromSeconds(0)
            );
        }

        public async Task DeleteBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            // Create a QueueClient
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            QueueMessage message = new();

            string updatedMessage = "Updated content!";

            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }
    }
}
