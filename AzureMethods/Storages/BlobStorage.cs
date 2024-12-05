using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMethods.Storages
{
    public class BlobStorage
    {
        public enum BlockType
        {
            BlockBlob,
            AppendBlob,
            PageBlob
        }

        #region calls
        public async Task<BlobBaseClient> GetBlob(string connectionString, BlockType blockType,string containerName,string blobName, bool authRequired = false)
        {
            // mit credentials

            BlobServiceClient blobServiceClient = GetBlobServiceClient(connectionString,authRequired);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobBaseClient blobClient = DetermineBlockTypeClient(containerClient, blockType, blobName);

            return blobClient;
        }

        // mehere Blobs holen
        public async Task<IAsyncEnumerable<Page<BlobItem>>> GetBlobPages<BlockType>(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        where BlockType : class
        {
            BlobServiceClient blobServiceClient = GetBlobServiceClient(connectionString, authRequired);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            return containerClient.GetBlobsAsync().AsPages();
        }

        public async Task<BlobClient> CreateBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            BlobServiceClient blobServiceClient = GetBlobServiceClient(connectionString, authRequired);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Create a new blob in the container
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload data to the blob
            string blobContent = "This is a blob content";
            using MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
            await blobClient.UploadAsync(memoryStream);

            return blobClient;
        }

        public async Task ModifyBlob(BinaryData binaryData,string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            BlobServiceClient blobServiceClient = GetBlobServiceClient(connectionString, authRequired);

            // Get the blob container client
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get the blob client
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload and overwrite
            await blobClient.UploadAsync(binaryData, overwrite: true);
        }

        public async Task DeleteBlob(string connectionString, BlockType blockType, string containerName, string blobName, bool authRequired = false)
        {
            BlobServiceClient blobServiceClient = GetBlobServiceClient(connectionString, authRequired);

            // Get the blob container client
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get the blob client
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Get blob properties
            await blobClient.DeleteAsync();
        }

        #endregion calls


        internal BlobServiceClient GetBlobServiceClient(string connectionString,bool authRequired)
        {
            BlobServiceClient blobServiceClient = null;

            if (authRequired)
                blobServiceClient = new BlobServiceClient(connectionString, new DefaultAzureCredential());   
            else
                blobServiceClient = new BlobServiceClient(connectionString);

            return blobServiceClient;
        }

        internal BlobBaseClient DetermineBlockTypeClient(BlobContainerClient containerClient, BlockType type, string blockName)
        {
            BlobBaseClient blockBaseClientToReturn = null;

            if (type == BlockType.BlockBlob)
                blockBaseClientToReturn = containerClient.GetBlockBlobClient(blockName);

            else if (type == BlockType.PageBlob)
                blockBaseClientToReturn = containerClient.GetPageBlobClient(blockName);

            else if (type == BlockType.AppendBlob)
                blockBaseClientToReturn = containerClient.GetAppendBlobClient(blockName);

            return blockBaseClientToReturn;
        }

    }
}
