using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.TelemetryPackets
{
    public interface ITelemetryLogger : IDisposable
    {
        Task StartLoggingAsync();
        void StopLogging();
        TelemetryPacket ProcessTelemetryPacket(byte[] data);
    }
}
