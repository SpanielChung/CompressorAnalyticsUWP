using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace CompressorAnalyticsUWP
{
    static class CloudHelper
    {
        static DeviceClient Client = null;
        static string DeviceConnectionString = "HostName=BTSIoTExample.azure-devices.net;DeviceId=danspi;SharedAccessKey=DU16pNDxrTsRmhC7/sIgDQO3j6n+HvZamxWqkToPogg=";

        public static async void SendDeviceToCloudMessagesAsync(string msg)
        {

            string iotHubUri = "BTSIoTExample.azure-devices.net"; // ! put in value !
            string deviceId = "danspi"; // ! put in value !
            string deviceKey = "DU16pNDxrTsRmhC7/sIgDQO3j6n+HvZamxWqkToPogg="; // ! put in value !

            var deviceClient = DeviceClient.Create(iotHubUri,
            AuthenticationMethodFactory.
            CreateAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
            TransportType.Http1);

            var message = new Message(Encoding.ASCII.GetBytes(msg));

            await deviceClient.SendEventAsync(message);
        }

        public static async Task InitClientAsync(MainPage p)
        {

                try
                {
                    p.addLineToConsole("Connecting to Client");
                    Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
                    p.addLineToConsole("Retrieving Device Twin");
                    Twin twin = await Client.GetTwinAsync();

                    TwinProperties prop = twin.Properties;

                p.addLineToConsole("desired properties...");
                p.addLineToConsole( prop.Desired.ToJson().ToString());
                p.addLineToConsole("reported properties...");
                p.addLineToConsole(prop.Reported.ToJson().ToString());

            }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }

            return;
        }

    }
}
