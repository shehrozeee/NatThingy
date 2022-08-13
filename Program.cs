using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mono.Nat;

namespace NatThingy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true; 
                /* run your code here */ 
                Console.WriteLine("Hello, world"); 
                new UPNP();
            }).Start();
            while(true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Hello, 1000"); 

            }

        }
    }



    public class UPNP
        {
            public UPNP()
            {
                NatUtility.DeviceFound += DeviceFound;
                NatUtility.StartDiscovery();
            }
    
            private void DeviceFound(object sender, DeviceEventArgs args)
            {
                INatDevice device = device = args.Device;
                device.CreatePortMap(new Mapping(Protocol.Tcp, 27016, 27017));
    
                foreach (Mapping portMap in device.GetAllMappings())
                {
                    Console.WriteLine(portMap.ToString());
                }
    
                Console.WriteLine(device.GetExternalIP().ToString());
                Console.WriteLine(device.GetSpecificMapping(Protocol.Tcp, 27016).PublicPort);
            }
    
            private void DeviceLost(object sender, DeviceEventArgs args)
            {
                INatDevice device = args.Device;
                device.DeletePortMap(new Mapping(Protocol.Tcp, 80, 80));
                // on device disconnect code
            }
        }
    
}