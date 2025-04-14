using SimRacingTelemetryLogger.Logger.TelemetryPackets.TelemetryPacketTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.TelemetryPackets
{
    public abstract class TelemetryPacket : ITelemetryPacket
    {
        public int PacketId { get; set; }
        public virtual CarTelemetryPacket CarTelemetryPacket { get; set; }
        public virtual SessionTelemetryPacket SessionTelemetryPacket { get; set; }

        public abstract void TransformPacket(byte[] data);
    }
}
