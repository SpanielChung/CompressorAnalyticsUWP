using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

namespace CompressorAnalyticsUWP
{
    static class CloudHelper
    {

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

    }
}
