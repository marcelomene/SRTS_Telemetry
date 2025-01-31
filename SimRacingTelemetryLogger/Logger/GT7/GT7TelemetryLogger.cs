using SimRacingTelemetryLogger.Logger;
using SimRacingTelemetryLogger.Logger.TelemetryPackets;
using SimRacingTelemetryLogger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.GT7
{
    public class GT7TelemetryLogger : TelemetryLogger
    {
        private static readonly byte KeepAliveByte = 0x41; // 'A'
        private readonly string CryptoKey = "Simulator Interface Packet GT7 ver 0.0";

        public GT7TelemetryLogger(int listeningPort) : base(listeningPort)
        {
            throw new NotSupportedException("GT7 Logger needs to inform remote IP address and keep alive information.");
        }

        public GT7TelemetryLogger(int listeningPort, int remotePort, string remoteIp, Action<TelemetryPacket> packetReadyCallback)
            : base(listeningPort, remotePort, true, true, [KeepAliveByte], [KeepAliveByte], remoteIp, packetReadyCallback)
        {
        }

        private byte[] DecryptFrame(byte[] data)
        {
            byte[] key = Encoding.ASCII.GetBytes(CryptoKey);

            // Get IV
            uint iv1 = BitConverter.ToUInt32(data, 0x40);
            uint iv2 = iv1 ^ 0xDEADBEAF;

            byte[] IV = BitConverter.GetBytes(iv2)
                .Concat(BitConverter.GetBytes(iv1))
                .ToArray(); // 8 bytes final IV           

            // Salsa20Decrypt
            Salsa20 salsa20alg = new();
            ICryptoTransform cryptoTransform = salsa20alg.CreateDecryptor(key.Take(32).ToArray(), IV);
            byte[] decryptedData = cryptoTransform.TransformFinalBlock(data, 0, data.Length);

            // Check magic number
            uint magic = BitConverter.ToUInt32(decryptedData, 0);

            if (magic != 0x47375330) // 0x47375330 is 'GT7S0'
                return Array.Empty<byte>();
            else
                return decryptedData;
        }

        public override GT7TelemetryPacket ProcessTelemetryPacket(byte[] data)
        {
            GT7TelemetryPacket packet = null;
            try
            {
                packet = GT7TelemetryPacket.CreatePacket(DecryptFrame(data));
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
            }
            return packet;
        }
    }
}
