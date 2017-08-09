using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressorAnalyticsUWP
{
    class CloudData
    {

        public decimal compressorPower;             //pC
        public decimal fanPower;                    //pF
        public decimal returnAirTemp;               //tR
        public decimal dischargeAirTemp;            //tD
        public decimal returnAirHumidity;           //rhR
        public decimal dischargeAirHumidity;        //rhD
        public decimal SuctionTemp;                 //  temp at inlet to compressor
        public decimal compressionTemp;             //  temp at outlet of compressor

        public CloudData()
        {

        }


    }
}
