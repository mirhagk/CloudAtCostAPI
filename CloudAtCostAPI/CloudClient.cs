using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public class PowerOperationResponse : Response<object>
        {
            public string serverid { get; set; }
            public string result { get; set; }
            public string taskid { get; set; }
        }
        public class ConsoleResponse : Response<object>
        {
            public string serverid { get; set; }
            public string console { get; set; }
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
        public class Resources
        {
            public class Used
            {
                public string cpu_used { get; set; }
                public string ram_used { get; set; }
                public string storage_used { get; set; }
            }
            public class Total
            {
                public string cpu_total { get; set; }
                public string ram_total { get; set; }
                public string storage_total { get; set; }
            }
            public Used used { get; set; }
            public List<Total> total { get; set; }
        }
        private async Task<T> Execute<T>(string url, object data = null)
        {
            System.Net.WebClient client = new System.Net.WebClient();

            string response;
            if (data != null)
            {
                var dataCollection = new NameValueCollection();
                foreach(var field in data.GetType().GetProperties())
                    dataCollection.Add(field.Name, field.GetValue(data).ToString());
                response = new string(Encoding.UTF8.GetChars(await client.UploadValuesTaskAsync(url, dataCollection)));
            }
            else
                response = await client.DownloadStringTaskAsync(new Uri(url));

            return JsonConvert.DeserializeObject<T>(response);
        }
        public async Task<PowerOperationResponse> BuildServer(int cpu, int ram, int storage, int os)
        {
            return (await Execute<PowerOperationResponse>($"https://panel.cloudatcost.com/api/v1/cloudpro/build.php?key={Key}&login={Login}&cpu={cpu}&ram={ram}&storage={storage}&os={os}"));
        }
        public async Task<PowerOperationResponse> DeleteServer(int serverID)
        {
            return await Execute<PowerOperationResponse>("https://panel.cloudatcost.com/api/v1/cloudpro/delete.php", new { key = Key, login = Login, sid = serverID });
        }
        public async Task<Resources> GetResourceUsage()
        {
            return (await Execute<Response<Resources>>("https://panel.cloudatcost.com/api/v1/cloudpro/resources.php", new { key = Key, login = Login})).data[0];
        }
        public async Task<IEnumerable<Server>> ListServers()
        {
            return (await Execute<Response<Server>>(String.Format("https://panel.cloudatcost.com/api/v1/listservers.php?key={0}&login={1}", Key, Login))).data;
        }
        public async Task<IEnumerable<Template>> ListTemplates()
        {
            return (await Execute<Response<Template>>(String.Format("https://panel.cloudatcost.com/api/v1/listtemplates.php?key={0}&login={1}", Key, Login))).data;
        }
        public async Task<IEnumerable<Task>> ListTasks()
        {
            return (await Execute<Response<Task>>(String.Format("https://panel.cloudatcost.com/api/v1/listtasks.php?key={0}&login={1}", Key, Login))).data;
        }
        public async Task<PowerOperationResponse> PowerOn(string serverID)
        {
            return await Execute<PowerOperationResponse>("https://panel.cloudatcost.com/api/v1/powerop.php", new { key=Key, login=Login, sid = serverID, action="poweron" });
        }
        public async Task<PowerOperationResponse> PowerOff(string serverID)
        {
            return await Execute<PowerOperationResponse>("https://panel.cloudatcost.com/api/v1/powerop.php", new { key = Key, login = Login, sid = serverID, action = "poweroff" });
        }
        public async Task<PowerOperationResponse> Reset(string serverID)
        {
            return await Execute<PowerOperationResponse>("https://panel.cloudatcost.com/api/v1/powerop.php", new { key = Key, login = Login, sid = serverID, action = "reset" });
        }
        public async Task<ConsoleResponse> GetServerConsole(string serverID)
        {
            return await Execute<ConsoleResponse>("https://panel.cloudatcost.com/api/v1/console.php", new { key = Key, login = Login, sid = serverID });
        }
    }
}
