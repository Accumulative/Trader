//using Microsoft.Azure;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace TraderData
//{
//    public class BlobHandler
//    {
//        // Retrieve storage account from connection string.
//        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
//        CloudConfigurationManager.GetSetting("StorageConnectionString"));

//        private string imageDirecoryUrl;

//        /// <summary>
//        /// Receives the users Id for where the pictures are and creates 
//        /// a blob storage with that name if it does not exist.
//        /// </summary>
//        /// <param name="imageDirecoryUrl"></param>
//        public BlobHandler(string imageDirecoryUrl)
//        {
//            this.imageDirecoryUrl = imageDirecoryUrl;
//            // Create the blob client.
//            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

//            // Retrieve a reference to a container. 
//            CloudBlobContainer container = blobClient.GetContainerReference(imageDirecoryUrl);

//            // Create the container if it doesn't already exist.
//            container.CreateIfNotExists();

//            //Make available to everyone
//            container.SetPermissions(
//                new BlobContainerPermissions
//                {
//                    PublicAccess = BlobContainerPublicAccessType.Blob
//                });
//        }

//        public void Upload(HttpPostedFileBase file)
//        {
//            // Create the blob client.
//            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

//            // Retrieve a reference to a container. 
//            CloudBlobContainer container = blobClient.GetContainerReference(imageDirecoryUrl);

//            if (file != null)
//            {
//                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
//                blockBlob.UploadFromStream(file.InputStream);
//            }
//        }

//        public List<string> GetBlobs()
//        {
//            // Create the blob client. 
//            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

//            // Retrieve reference to a previously created container.
//            CloudBlobContainer container = blobClient.GetContainerReference(imageDirecoryUrl);

//            List<string> blobs = new List<string>();

//            // Loop over blobs within the container and output the URI to each of them
//            foreach (var blobItem in container.ListBlobs())
//                blobs.Add(blobItem.Uri.ToString());

//            return blobs;
//        }
//    }
//}