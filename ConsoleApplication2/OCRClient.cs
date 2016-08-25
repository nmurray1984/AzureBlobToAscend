using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class OCRClient
    {
        public TaxDocument ProcessTaxDocument(TaxDocumentScan document)
        {
            TaxDocument tdout = new TaxDocument();
            tdout.ImportantTaxNumber = 42;
            tdout.PropertyName = "Six Flags over Atlanta";
            tdout.PropertyValue = 988342.37;
            tdout.TaxRate = 0.51;
            Console.WriteLine("ProcessTaxDocument called");
            Console.WriteLine("blob data: {0}", Encoding.Default.GetString(document.Data));
            Console.WriteLine("Returning TaxDocument with params:");
            Console.WriteLine("ImportantTaxNumber={0}", tdout.ImportantTaxNumber);
            Console.WriteLine("PropertyName={0}", tdout.PropertyName);
            Console.WriteLine("PropertyValue={0}", tdout.PropertyValue);
            Console.WriteLine("TaxRate={0}", tdout.TaxRate);

            return tdout;
        }
    }
}
