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
        static string Task { get; set; }
        static string ServerID { get; set; }
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    switch (args[i].Substring(2).ToLowerInvariant())
                    {
                        case "tasks": goto case "list-tasks";
                        case "list-tasks": Task = "list-tasks"; break;
                        case "servers": goto case "list-servers";
                        case "list-servers": Task = "list-servers"; break;
                        case "templates": goto case "list-templates";
                        case "list-templates": Task = "list-templates"; break;
                        case "on": goto case "power-on";
                        case "poweron": goto case "power-on";
                        case "power-on": Task = "power-on"; break;
                        case "off": goto case "power-off";
                        case "poweroff": goto case "power-off";
                        case "power-off": Task = "power-off"; break;
                        case "reset": Task = "reset"; break;
                        default:
                            Console.Error.WriteLine("Could not understand switch {0}", args[i].Substring(2));
                            return;
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
                        case "t": goto case "task";
                        case "task":
                            Task = value;
                            break;
                        case "id": goto case "sid";
                        case "sid": goto case "server";
                        case "server": goto case "server-id";
                        case "serverid": goto case "server-id";
                        case "server-id":
                            ServerID = value;
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
                        case "key": Key = line[1]; break;
                        case "login": Login = line[1]; break;
                        default: Console.Error.WriteLine("Don't understand key {0}", line[0]); return;
                    }
                }
            }
            var client = new CloudAtCostAPI.CloudClient();
            client.Key = Key;
            client.Login = Login;
            switch (Task)
            {
                case "list-servers":
                    var servers = client.ListServers().Result;
                    foreach(var server in servers)
                    {
                        Console.WriteLine("label: {0} ip: {1} serverid: {2}, status: {3}", server.lable, server.ip, server.id, server.status);
                    }
                    break;
                case "list-tasks":
                    var tasks = client.ListTasks().Result;
                    if (!tasks.Any())
                    {
                        Console.WriteLine("No currently running tasks");
                    }
                    foreach(var task in tasks)
                    {
                        Console.WriteLine("serverid: {0} action: {1} starttime: {2} status: {3} finishtime: {4}", task.serverid, task.action, task.starttime, task.status, task.finishtime);
                    }
                    break;
                case "power-on":
                    {
                        var response = client.PowerOn(ServerID).Result;
                        Console.WriteLine("Turned server {0} on, result {1}, taskid {2}", ServerID, response.result, response.taskid);
                        break;
                    }
                case "power-off":
                    {
                        var response = client.PowerOff(ServerID).Result;
                        Console.WriteLine("Turned server {0} off, result {1}, taskid {2}", ServerID, response.result, response.taskid);
                        break;
                    }
                case "reset":
                    {
                        var response = client.Reset(ServerID).Result;
                        Console.WriteLine("Reset server {0}, result {1}, taskid {2}", ServerID, response.result, response.taskid);
                        break;
                    }
                default:
                    Console.Error.WriteLine("Did not understand task {0}", Task);
                    return;
            }
        }
    }
}
