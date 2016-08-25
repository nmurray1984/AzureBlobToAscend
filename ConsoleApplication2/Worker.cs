using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using D = System.Data;
using SQL = System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Worker
    {
        //NM - At a high level, the primary steps should be broken out into their own methods, for instance
        // LoadConfiguration();
        // ProcessTaxDocument();
        // WriteToAzure();
        // WriteToDatabase();
        //
        // That way people can understand what is going on at a high level without diving deep into the code
        public Worker()
        {
            // NM - Modify this to read these values from a configuration file.  Google 'how to use appsettings'
            CloudStorageAccount csa = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=cminton;AccountKey=9/z9oCB6lMWHXjxBFEmxB79DUVx0bHF91iHMMjvpX1XXXBMwiJ5+ADpnAQhEhJgtBu8uw9qOtRAq/w2uQT723A==");
            CloudBlobClient blobclient = csa.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference("mycontainer");

            string connectionstr =
                "Server=tcp:cminton.database.windows.net,1433;Database=cmintondb;User ID=cminton;Password=Foopass42;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // create a blob object, then get the document blob from azure
            CloudBlockBlob docblob = container.GetBlockBlobReference("myblob");
            docblob.FetchAttributes();

            // extract the scan data from the blob
            TaxDocumentScan tdscan = new TaxDocumentScan(docblob);

            // create OCRClient and send scan data through OCR
            // get tax document object with completed fields
            OCRClient ocr = new OCRClient();
            TaxDocument taxdoc = ocr.ProcessTaxDocument(tdscan);

            // populate azure sql database with tax document fields
            using (var connection = new SQL.SqlConnection(connectionstr))
            {
                connection.Open();
                Console.WriteLine("Connection opened successfully.");
                UploadTaxDocument(connection, taxdoc);
            }
        }

        // NM - Refactor this into a Repository class (google 'repository pattern'), which uses LINQ and Entity Framework.
        // If you need help this, let me know
        
        // requires an open connection
        void UploadTaxDocument(SQL.SqlConnection connection, TaxDocument doc)
        {
            Console.WriteLine("Writing tax document to database...");
            SQL.SqlParameter parameter;

            using (var command = new SQL.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = D.CommandType.Text;
                command.CommandText = @"
insert into taxdocuments
    (importanttaxnumber,
    taxrate,
    propertyname,
    propertyvalue
    )
values
    (@importanttaxnumber,
    @taxrate,
    @propertyname,
    @propertyvalue
    ); ";

                parameter = new SQL.SqlParameter("@importanttaxnumber", D.SqlDbType.Int);
                parameter.Value = doc.ImportantTaxNumber;
                command.Parameters.Add(parameter);

                parameter = new SQL.SqlParameter("@taxrate", D.SqlDbType.Float);
                parameter.Value = doc.TaxRate;
                command.Parameters.Add(parameter);

                parameter = new SQL.SqlParameter("@propertyname", D.SqlDbType.NVarChar, 50);
                parameter.Value = doc.PropertyName;
                command.Parameters.Add(parameter);

                parameter = new SQL.SqlParameter("@propertyvalue", D.SqlDbType.Money);
                parameter.Value = doc.PropertyValue;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
                Console.WriteLine("Insert operation completed.");
            }
        }
    }
}
