using System;
using Microsoft.ServiceBus.Messaging;

namespace ProcessDeviceToCloudMessages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string iotHubConnectionString = "HostName=esThings.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=fOqVTq6qlMj5ox7w9AJTItDxNzxKTqL8QSjE78xdAlg=";
            const string iotHubD2CEndpoint = "messages/events";
            
            StoreEventProcessor.StorageConnectionString =
                "DefaultEndpointsProtocol=http;AccountName=esthings;AccountKey=XYNwkmqCx6NwT3dQHLtkGot0Xv6zcIeCwD3XDmcJ9NUdL9Yd8vUvNjDDZM8RE9PCy8cpAJC0IVWz2ecuroXFWg==";
            StoreEventProcessor.ServiceBusConnectionString =
                "Endpoint=sb://esthings.servicebus.windows.net/;SharedAccessKeyName=esThingsSend;SharedAccessKey=TM/CQ6LQEdYZ657z6tTUbLCqZUad3ofEOy7wRehLYRU=;EntityPath=esthingsqueue";

            string eventProcessorHostName = Guid.NewGuid().ToString();

            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, iotHubD2CEndpoint,
                EventHubConsumerGroup.DefaultGroupName, iotHubConnectionString,
                StoreEventProcessor.StorageConnectionString, "messages-events");

            Console.WriteLine("Registering EventProcessor...");

            eventProcessorHost.RegisterEventProcessorAsync<StoreEventProcessor>().Wait();
            Console.ReadLine();

            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
