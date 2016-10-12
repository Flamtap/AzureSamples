using System;
using System.Linq;
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
            ReceiveFeedbackAsync();

            Console.WriteLine("Press any key to send a C2D message.");
            while (true)
            {
                Console.ReadLine();

                SendCloudToDeviceMessageAsync().Wait();

                Console.ReadLine();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static async Task SendCloudToDeviceMessageAsync()
        {
            Message commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."));

            commandMessage.Ack = DeliveryAcknowledgement.Full;

            await ServiceClient.SendAsync("esDevice", commandMessage);
        }

        private static async void ReceiveFeedbackAsync()
        {
            FeedbackReceiver<FeedbackBatch> feedbackReceiver = ServiceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");

            while (true)
            {
                FeedbackBatch feedbackBatch = await feedbackReceiver.ReceiveAsync();

                if (feedbackBatch == null)
                    continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }
    }
}
