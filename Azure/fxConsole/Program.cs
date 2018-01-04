using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace fxConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseBlob("1001_2.json");

            Console.WriteLine("\nAny key to exit...");
            Console.ReadKey();
        }

        static void ParseBlob(string name)
        {
            string prefix = name.Substring(0, 4);
            string path = string.Format(@"c:\blobs\{0}\{1}", prefix, name);

            Dictionary<string, PoloQuote> quotes = null;

            using (StreamReader stream = File.OpenText(path))
            {
                string json = stream.ReadToEnd();
                quotes = JsonConvert.DeserializeObject<Dictionary<string, PoloQuote>>(json);
            }

            if (quotes != null)
            {
                foreach (var key in quotes.Keys)
                {
                    Console.WriteLine("{0} {1}", key, quotes[key].ToString());
                }
            }
            else
            {
                Console.WriteLine("Found nothing.");
            }
        }

        static void DownloadBlobs(string prefix)
        {
            string connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("datacollection");

            List<string> names = new List<string>();
            int ct = 1;

            foreach (IListBlobItem item in container.ListBlobs(prefix, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    names.Add(blob.Name);
                }
                else
                {
                    Console.WriteLine("{0} | {1}", item.GetType().Name, item.Uri);
                }

                if (ct > 1000)
                {
                    Console.Write(".");
                    ct = 0;
                }

                ct += 1;
            }

            Console.WriteLine();
            Console.WriteLine("Found {0} names", names.Count);
            Console.WriteLine("\nAny key to continue...");
            Console.ReadKey();

            foreach (string name in names)
            {
                Console.WriteLine(name);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

                using (var fileStream = System.IO.File.OpenWrite(string.Format(@"d:\{0}\{1}", prefix, name)))
                {
                    blockBlob.DownloadToStream(fileStream);
                }
            }
        }
    }
}
