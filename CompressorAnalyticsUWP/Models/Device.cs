using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorAnalyticsUWP
{
    /// <summary>
    /// Systems settings are sent to the cloud when they are changed.
    /// They are then retrieved when the sytem is restarted
    /// </summary>
    static class Device
    {
        internal static string refrigerantType {get;set;}
        internal static string compressorManufacturer { get; set; }

        internal static string compressorModel { get; set; }


    }

}
