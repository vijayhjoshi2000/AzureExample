//copy form azure portal
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=storageaccountvj2020;AccountKey=DHF7L4RY5lRjmE+mB2UcNiRs22JGIIeoD/P+vZz6GbA6p3MybCPQ8UuU7Gd1YzUNqB668Dy9wEtgm+5fq8L7RQ==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = DateTime.Today.ToString("yyyyMMdd");

            BlobContainerClient containerClient = null;
            try
            { 
                // create blob container if not exists else throw error if exists
                containerClient = blobServiceClient.CreateBlobContainer(containerName);
            }
            catch (Azure.RequestFailedException rfe)
            {
                // get ref if exists
                containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch { throw; }

            // create create a handle to create new blob
            BlobClient blobClient = containerClient.GetBlobClient("dummy.txt");
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);            
            // Open the file and upload its data it will override if file exists
            blobClient.Upload("dummy.txt", overwrite: true);

            Console.WriteLine("List of files available:");
            foreach(var info in containerClient.GetBlobs())
            {
                Console.WriteLine(info.Name);
            }

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = blobClient.Download();
            FileStream fs = File.OpenWrite("dummy_Downloaded.txt");
            download.Content.CopyTo(fs);
            fs.Close();

            // delete resources if required
            blobClient.Delete();
            containerClient.Delete();
