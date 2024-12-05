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
using Azure.Storage.Files.Shares;
using System.Security.Cryptography.X509Certificates;
using Azure.Storage.Files.Shares.Models;

namespace AzureMethods.Storages
{
    public class FileStorage
    {
        public async Task<BlobBaseClient> GetBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {

            // Get a reference to the file within the share
            ShareFileClient fileClient = shareClient.GetFileClient(fileName);

            await foreach (ShareFileItem fileItem in shareClient.GetFilesAndDirectoriesAsync())
            {
                Console.WriteLine(fileItem.Name);
            }
        }


        public async Task<BlobClient> CreateBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {

            // Get a reference to the file within the share
            ShareFileClient fileClient = shareClient.GetFileClient(fileName);

            // Upload the file (overwrite if it exists)
            using FileStream fs = File.OpenRead(filePath);
            await fileClient.UploadAsync(fs, overwrite: true);
        }

        public async Task<BlobClient> CreateBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {

            // Create a ShareClient
            ShareClient shareClient = new ShareClient(connectionString, shareName);

            // Create the file share if it doesn't exist
            await shareClient.CreateIfNotExistsAsync();

            Console.WriteLine("File share created (if not already exists).");
        }

        public async Task ModifyBlob(BinaryData binaryData, string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            // Get a reference to the file to delete
            ShareFileClient fileClient = shareClient.GetFileClient(fileName);

            // Delete the file
            await fileClient.DeleteIfExistsAsync();
        }

        public async Task DeleteBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            // Create a QueueClient
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            QueueMessage message = new();

            string updatedMessage = "Updated content!";

            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        public async Task DeleteBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            await shareClient.DeleteIfExistsAsync();
            Console.WriteLine($"File share '{shareName}' deleted.");

        }
    }
}
