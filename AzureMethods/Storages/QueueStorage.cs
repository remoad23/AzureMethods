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
        public async Task<QueueMessage> GetMessage(string connectionString,string queueName, bool authRequired = false)
        {
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            QueueMessage[] messages = queueClient.ReceiveMessages(maxMessages: 1);

            var message = messages[0];
            return message;
        }

        public async Task CreateMessage(string connectionString, string queueName,string message, bool authRequired = false)
        {
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }

        public async Task ModifyMessage(string connectionString, string queueName,string messageId, string popReceipt,string updatedMessage, bool authRequired = false)
        {
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            queueClient.ReceiveMessage();

            await queueClient.UpdateMessageAsync(
                messageId,
                popReceipt,
                updatedMessage,
                visibilityTimeout: TimeSpan.FromMinutes(5)
            );
        }

        public async Task DeleteMessage(string connectionString, string queueName,string messageId, string popReceipt, bool authRequired = false)
        {
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            await queueClient.DeleteMessageAsync(messageId,popReceipt);
        }
    }
}
