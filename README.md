CloudAtCost API Wrapper
===

[![Build status](https://ci.appveyor.com/api/projects/status/uxb9tu13n57vxedm/branch/master?svg=true)](https://ci.appveyor.com/project/mirhagk/cloudatcostapi/branch/master)

This project provides a wrapper around the CloudAtCost API. It allows you to control your CloudAtCost servers from your .NET application.

Installation
---

The easiest way is to install it using nuget

~~~
Install-Package CloudAtCostAPI
~~~

Or download the latest version from the releases

Use
---

To use the client, simply construct it, and set the key and login

~~~
var client = new CloudAtCostAPI.CloudClient();
client.Key = Key;
client.Login = Login;
~~~

All of the methods are async, so call the method with await, for instance to list all the servers under your control, use:

~~~
var servers = await client.ListServers().Result;
foreach(var server in servers)
{
	Console.WriteLine("label: {0} ip: {1} serverid: {2}, status: {3}", server.lable, server.ip, server.id, server.status);
}
~~~

Troubleshooting
---

If you are getting an unauthorized exception, make sure you are providing your key and email. Also make sure that your current IP address is whitelisted on the CloudAtCost panel.

If you continue having issues, create an issue here.

Warning
---
The CloudAtCost API hasn't been stabilized yet, so this client may break when they update it. Report any issues that arise and make sure you get the latest version.