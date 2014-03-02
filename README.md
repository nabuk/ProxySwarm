ProxySwarm
==========

ProxySwarm is a WPF application that shows how to run many proxy connections efficiently.  
It has been written in .NET 4.5 and incorporates the [Task-based Asynchronous Pattern](http://msdn.microsoft.com/en-us/library/hh873175%28v=vs.110%29.aspx).

##Getting started

### 1. Proxies

You will need a file with proxies in the following format:

	IP:port
	IP:port
	IP:port
	...

If you don't have it already, you can get such a file here: *http://gatherproxy.com/subscribe*

### 2. Task factory

**TestProxyWorkerFactory** class, located under the following path:  
*\src\ProxySwarm.WpfApp\Concrete\TestProxyWorkerFactory.cs*  
is responsible for creating tasks.  

The factory class implements **IProxyWorkerFactory** interface, which obliges it to implement a method of the following signature:  
  
		Task<bool> CreateWorkerAsync(Proxy proxy);

In the mentioned class you can see an example, that downloads the content of Microsoft website and returns true, if the operation was successful.

### 3. Settings

**MaxConnectionCount** setting in **App.config** file tells how many connections can run simultaneously.

##License

ProxySwarm is available under the MIT license. See LICENSE.txt for more information.