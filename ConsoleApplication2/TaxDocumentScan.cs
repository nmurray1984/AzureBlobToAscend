using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class TaxDocumentScan
    {
        public byte[] Data { get; set; }

        public TaxDocumentScan(CloudBlockBlob blob) 
        {
            Console.WriteLine(blob.Name);
            Data = new byte[blob.Properties.Length];
            blob.DownloadToByteArray(Data, 0);
        }

        public TaxDocumentScan(byte[] data)
        {
            Data = data;
        }
    }
}
