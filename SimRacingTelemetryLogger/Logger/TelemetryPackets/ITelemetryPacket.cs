using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.TelemetryPackets
{
    public interface ITelemetryPacket
    {
        void TransformPacket(byte[] data);
    }
}
