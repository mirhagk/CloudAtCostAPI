using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAtCostAPI
{
    public class CloudClient
    {
        public string Key;
        public string Login;
        public class Response<T>
        {
            public string status { get; set; }
            public int time { get; set; }
            public string api { get; set; }
            public string action { get; set; }
            public List<T> data { get; set; }
        }
        public class Server
        {
            public int id { get; set; }
            public int packageid { get; set; }
            public string servername { get; set; }
            public string lable { get; set; }
            public string vmname { get; set; }
            public string ip { get; set; }
            public string netmask { get; set; }
            public string portgroup { get; set; }
            public string hostname { get; set; }
            public string rootpass { get; set; }
            public string vncport { get; set; }
            public string vncpass { get; set; }
            public string servertype { get; set; }
            public string template { get; set; }
            public string cpu { get; set; }
            public string cpuusage { get; set; }
            public string ram { get; set; }
            public string ramusage { get; set; }
            public string storage { get; set; }
            public string hdusage { get; set; }
            public string sdate { get; set; }
            public string status { get; set; }
            public string panel_note { get; set; }
            public string mode { get; set; }
            public string uid { get; set; }
            public string sid { get; set; }
        }
        public class Template
        {
            public int id { get; set; }
            public string detail { get; set; }
        }
        public class Task
        {
            public string cid { get; set; }
            public string idf { get; set; }
            public string serverid { get; set; }
            public string action { get; set; }
            public string status { get; set; }
            public string starttime { get; set; }
            public string finishtime { get; set; }
        }
        private async Task<T> Execute<T>(string url, string method)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            return JsonConvert.DeserializeObject<T>(await client.DownloadStringTaskAsync(new Uri(url)));
        }
        public async Task<IEnumerable<Server>> ListServers()
        {
            return (await Execute<Response<Server>>(String.Format("https://panel.cloudatcost.com/api/v1/listservers.php?key={0}&login={1}", Key, Login), "get")).data;
        }
        public async Task<IEnumerable<Template>> ListTemplates()
        {
            return (await Execute<Response<Template>>(String.Format("https://panel.cloudatcost.com/api/v1/listtemplates.php?key={0}&login={1}", Key, Login), "get")).data;
        }
        public async Task<IEnumerable<Task>> ListTasks()
        {
            return (await Execute<Response<Task>>(String.Format("https://panel.cloudatcost.com/api/v1/listtasks.php?key={0}&login={1}", Key, Login), "get")).data;
        }
    }
}
