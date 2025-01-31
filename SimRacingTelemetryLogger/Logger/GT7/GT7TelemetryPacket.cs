using SimRacingTelemetryLogger.Logger.TelemetryPackets;
using SimRacingTelemetryLogger.Logger.TelemetryPackets.TelemetryPacketTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.GT7
{
    public class GT7TelemetryPacket : TelemetryPacket
    {
        public CarTelemetryPacket CarTelemetryPacket { get; set; }
        public SessionTelemetryPacket SessionTelemetryPacket { get; set; }

        public GT7TelemetryPacket() { }
        public GT7TelemetryPacket(byte[] data)
            => TransformPacket(data);

        public override void TransformPacket(byte[] data)
        {
            PacketId = BitConverter.ToInt32(data, 0x70);

            SessionTelemetryPacket = new()
            {
                BestLap         = TimeSpan.FromMilliseconds(BitConverter.ToInt32(data, 0x78)),
                LastLap         = TimeSpan.FromMilliseconds(BitConverter.ToInt32(data, 0x7C)),
                CurrentLap      = BitConverter.ToInt16(data, 0x74),
                CurrentPosition = BitConverter.ToInt16(data, 0x84),
                TotalPositions  = BitConverter.ToInt16(data, 0x86),
                TimeOnTrack     = TimeSpan.FromMilliseconds(BitConverter.ToInt32(data, 0x80)),
                TotalLaps       = BitConverter.ToInt16(data, 0x76),
                InRace          = (data[0x8e] & 0b10) != 0,
                IsPaused        = (data[0x8e] & 0b01) != 0
            };

            CarTelemetryPacket = new()
            {
                CarId             = BitConverter.ToInt32(data, 0x124),
                CurrentGear       = (byte)(data[0x90] & 0xf),
                SuggestedGear     = (byte)(data[0x90] >> 4),
                FuelCapacity      = BitConverter.ToSingle(data, 0x48),
                CurrentFuel       = BitConverter.ToSingle(data, 0x44),
                Boost             = BitConverter.ToSingle(data, 0x50) - 1,
                TyreDiameterFL    = BitConverter.ToSingle(data, 0xB4),
                TyreDiameterFR    = BitConverter.ToSingle(data, 0xB8),
                TyreDiameterRL    = BitConverter.ToSingle(data, 0xBC),
                TyreDiameterRR    = BitConverter.ToSingle(data, 0xC0),
                CarSpeed          = 3.6f * BitConverter.ToSingle(data, 0x4C),
                Throttle          = data[0x91] / 2.55f,
                Rpm               = BitConverter.ToSingle(data, 0x3C),
                RpmRevWarning     = BitConverter.ToUInt16(data, 0x88),
                Brake             = data[0x92] / 2.55f,
                RpmRevLimiter     = BitConverter.ToUInt16(data, 0x8A),
                EstimatedTopSpeed = BitConverter.ToInt16(data, 0x8C),
                Clutch            = BitConverter.ToSingle(data, 0xF4),
                ClutchEngaged     = BitConverter.ToSingle(data, 0xF8),
                RpmAfterClutch    = BitConverter.ToSingle(data, 0xFC),
                OilTemp           = BitConverter.ToSingle(data, 0x5C),
                WaterTemp         = BitConverter.ToSingle(data, 0x58),
                OilPressure       = BitConverter.ToSingle(data, 0x54),
                RideHeight        = 1000 * BitConverter.ToSingle(data, 0x38),
                TyreTempFL        = BitConverter.ToSingle(data, 0x60),
                TyreTempFR        = BitConverter.ToSingle(data, 0x64),
                SuspensionFL      = BitConverter.ToSingle(data, 0xC4),
                SuspensionFR      = BitConverter.ToSingle(data, 0xC8),
                TyreTempRL        = BitConverter.ToSingle(data, 0x68),
                TyreTempRR        = BitConverter.ToSingle(data, 0x6C),
                SuspensionRL      = BitConverter.ToSingle(data, 0xCC),
                SuspensionRR      = BitConverter.ToSingle(data, 0xD0),
                PositionX         = BitConverter.ToSingle(data, 0x04),
                PositionY         = BitConverter.ToSingle(data, 0x08),
                PositionZ         = BitConverter.ToSingle(data, 0x0C),
                VelocityX         = BitConverter.ToSingle(data, 0x10),
                VelocityY         = BitConverter.ToSingle(data, 0x14),
                VelocityZ         = BitConverter.ToSingle(data, 0x18),
                RotationPitch     = BitConverter.ToSingle(data, 0x1C),
                RotationYaw       = BitConverter.ToSingle(data, 0x20),
                RotationRoll      = BitConverter.ToSingle(data, 0x24),
                AngularVelocityX  = BitConverter.ToSingle(data, 0x2C),
                AngularVelocityY  = BitConverter.ToSingle(data, 0x30),
                AngularVelocityZ  = BitConverter.ToSingle(data, 0x34),
            };

            // Calculates Tyre Slip Ratio if the car is moving
            if (CarTelemetryPacket.CarSpeed > 0)
            {
                CarTelemetryPacket.TyreSlipRatioFL = Math.Abs(3.6f * CarTelemetryPacket.TyreDiameterFL * BitConverter.ToSingle(data, 0xA4) / CarTelemetryPacket.CarSpeed);
                CarTelemetryPacket.TyreSlipRatioFR = Math.Abs(3.6f * CarTelemetryPacket.TyreDiameterFR * BitConverter.ToSingle(data, 0xA8) / CarTelemetryPacket.CarSpeed);
                CarTelemetryPacket.TyreSlipRatioRL = Math.Abs(3.6f * CarTelemetryPacket.TyreDiameterRL * BitConverter.ToSingle(data, 0xAC) / CarTelemetryPacket.CarSpeed);
                CarTelemetryPacket.TyreSlipRatioRR = Math.Abs(3.6f * CarTelemetryPacket.TyreDiameterRR * BitConverter.ToSingle(data, 0xB0) / CarTelemetryPacket.CarSpeed);
            }

            // Get car name
           // CarTelemetryPacket.CarName = GT7Utils.GetCarPrettyName(CarTelemetryPacket.CarId);
        }

        public static GT7TelemetryPacket CreatePacket(byte[] data)
            => new GT7TelemetryPacket(data);
    }
}
