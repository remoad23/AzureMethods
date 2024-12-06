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
        public async Task<ShareFileItem> GetFile(string connectionString, string fileShareName, string fileName, bool authRequired = false)
        {
            // Create the ShareClient
            ShareClient shareClient = new ShareClient(connectionString, fileShareName);

            // Get a reference to the file within the share
            ShareFileClient fileClient = shareClient.GetRootDirectoryClient().GetFileClient(fileName);

            await foreach (ShareFileItem fileItem in shareClient.GetRootDirectoryClient().GetFilesAndDirectoriesAsync())
            {
                if (fileName == fileItem.Name)
                    return fileItem;
            }

            return null;
        }

        public async Task CreateFile(string connectionString, string fileShareName, string fileName, FileStream file, bool authRequired = false)
        {
            // Create the ShareClient
            ShareClient shareClient = new ShareClient(connectionString, fileShareName);

            // Get a reference to the file within the share
            ShareFileClient fileClient = shareClient.GetRootDirectoryClient().GetFileClient(fileName);

            await fileClient.UploadAsync(file);
        }

        public async Task ModifyFile(BinaryData binaryData, string connectionString, string fileShareName, string fileName, FileStream file, bool authRequired = false)
        {
            // Create the ShareClient
            ShareClient shareClient = new ShareClient(connectionString, fileShareName);

            // Get a reference to the file to delete
            ShareFileClient fileClient = shareClient.GetRootDirectoryClient().GetFileClient(fileName);
            
            //automatic overwrite
            await fileClient.UploadAsync(file);
        }

        public async Task DeleteFile(string connectionString,string fileShareName, string fileName, bool authRequired = false)
        {
            // Create the ShareClient
            ShareClient shareClient = new ShareClient(connectionString, fileShareName);

            // Get a reference to the file to delete
            ShareFileClient fileClient = shareClient.GetRootDirectoryClient().GetFileClient(fileName);

            // Delete the file
            await fileClient.DeleteIfExistsAsync();
        }
    }
}
