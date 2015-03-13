using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Console
{
    /// <summary>
    /// The point of this project is to wrap the library in a command line application
    /// but also serves as a quick way to test that the application is functioning
    /// </summary>
    class Program
    {
        static string CredentialFile { get; set; }
        static void Main(string[] args)
        {
            for(int i=0;i<args.Length;i++)
            {
                if (args[i].StartsWith("--"))
                {

                }
            }
            var client = new CloudAtCostAPI.CloudClient();
        }
    }
}
