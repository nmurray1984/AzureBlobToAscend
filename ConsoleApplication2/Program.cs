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
    // NM - make sure this has a public modifier.  It won't cause issues now, but will in bigger projects when
    // you try to call it from another assembly (the default is protected, which is only accessible in this assembly)
    class Program
    {
        static void Main(string[] args)
        {
            // NM - so that you can test the worker well, make this two parts.  1) to instanciate the object, and
            // 2) to start the worker.  Something like var worker = new Worker(); worker.Start();
            // This makes it more readable and people who have never seen your code understand what is happening.
            // In the future also, this make it easier to implement methods for Stop() or Pause(), if that's ever necessary
            new Worker();
        }
    }
}
