using System;
using System.Numerics;

namespace CSIServer.Models
{
    public class CSIPacketDTO
    {
        public int NumSubcarriers { get; set; }
        public int NumReceivingAntennas { get; set; }
        public int NumTransmittingAntennas { get; set; }

        public CSIValueDTO[] Values { get; set; }
    }

    public class CSIValueDTO
    {
        public DateTime Timestamp { get; set; }
        public int Rx { get; set; }
        public int Tx { get; set; }
        public int Subcarrier { get; set; }
        public Complex Value { get; set; }
    }
}