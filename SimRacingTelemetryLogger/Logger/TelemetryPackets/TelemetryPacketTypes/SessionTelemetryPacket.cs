using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.TelemetryPackets.TelemetryPacketTypes
{
    public class SessionTelemetryPacket
    {
        public TimeSpan BestLap { get; set; }
        public TimeSpan LastLap { get; set; }
        public int CurrentLap { get; set; }
        public TimeSpan TimeOnTrack { get; set; }
        public short TotalLaps { get; set; }
        public short CurrentPosition { get; set; }
        public bool IsPaused { get; set; }
        public bool InRace { get; set; }
        public short TotalPositions { get; set; }
    }
}
