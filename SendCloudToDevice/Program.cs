using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace SendCloudToDevice
{
    public class Program
    {
        public static ServiceClient ServiceClient;
        public static string ConnectionString = "";

        public static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message...\n");
            ServiceClient = ServiceClient.CreateFromConnectionString(ConnectionString);

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();

            SendCloudToDeviceMessageAsync().Wait();

            Console.ReadLine();
        }

        private static async Task SendCloudToDeviceMessageAsync()
        {
            Message commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."));

            await ServiceClient.SendAsync("esDevice", commandMessage);
        }
    }
}
