using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CloudAtCostAPIConsole
{
    /// <summary>
    /// The point of this project is to wrap the library in a command line application
    /// but also serves as a quick way to test that the application is functioning
    /// </summary>
    class Program
    {
        static string CredentialFile { get; set; }
        static string Key { get; set; }
        static string Login { get; set; }
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    switch (args[i].Substring(2).ToLowerInvariant())
                    {
                    }
                }
                else if (args[i].StartsWith("-"))
                {
                    var param = args[i].Substring(1).ToLowerInvariant();
                    if (i + 1 == args.Length)
                    {
                        Console.Error.WriteLine("Expected value for parameter {0}", param);
                        return;
                    }
                    var value = args[++i].ToLowerInvariant();
                    switch (param)
                    {
                        case "c": goto case "cred";
                        case "cred": goto case "credentials";
                        case "credentials":
                            CredentialFile = value;
                            break;
                        case "k": goto case "key";
                        case "key":
                            Key = value;
                            break;
                        case "l": goto case "login";
                        case "login":
                            Login = value;
                            break;
                        default:
                            Console.Error.WriteLine("Could not understand parameter {0}", param);
                            return;
                    }
                }
            }
            if (CredentialFile != null)
            {
                if (Key != null || Login != null)
                {
                    Console.Error.WriteLine("Can't specify both the credentials and the file");
                    return;
                }
                foreach (var line in File.ReadAllLines(CredentialFile).Select(x => x.Split('=').Select(y => y.Trim().ToLowerInvariant()).ToArray()))
                {
                    switch (line[0])
                    {
                        case "key":Key = line[1]; break;
                        case "login":Login = line[1]; break;
                        default: Console.Error.WriteLine("Don't understand key {0}", line[0]); break;
                    }
                }
            }
            var client = new CloudAtCostAPI.CloudClient();
        }
    }
}
