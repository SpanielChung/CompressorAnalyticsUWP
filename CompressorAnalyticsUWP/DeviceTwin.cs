using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorAnalyticsUWP
{
    internal class DeviceTwin
    {
        string deviceId { get; set; }
        bool pushData { get; set; }
        string dataTransferRate { get; set; }

        //  Location
        decimal latitude { get; set; }  //  decimal degrees
        decimal longitude { get; set; } //  decimal degrees
        decimal altitude { get; set; }  //  meters

    }
}
