using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingTelemetryLogger.Logger.TelemetryPackets.TelemetryPacketTypes
{
    public class CarTelemetryPacket
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int CurrentGear { get; set; }
        public int SuggestedGear { get; set; }
        public float FuelCapacity { get; set; }
        public float CurrentFuel { get; set; }
        public float Boost { get; set; }
        public float TyreDiameterFL { get; set; }
        public float TyreDiameterFR { get; set; }
        public float TyreDiameterRL { get; set; }
        public float TyreDiameterRR { get; set; }
        public float CarSpeed { get; set; }
        public float TyreSlipRatioFL { get; set; }
        public float TyreSlipRatioFR { get; set; }
        public float TyreSlipRatioRL { get; set; }
        public float TyreSlipRatioRR { get; set; }
        public float Throttle { get; set; }
        public float Rpm { get; set; }
        public ushort RpmRevWarning { get; set; }
        public float Brake { get; set; }
        public ushort RpmRevLimiter { get; set; }
        public short EstimatedTopSpeed { get; set; }
        public float Clutch { get; set; }
        public float ClutchEngaged { get; set; }
        public float RpmAfterClutch { get; set; }
        public float OilTemp { get; set; }
        public float WaterTemp { get; set; }
        public float OilPressure { get; set; }
        public float RideHeight { get; set; }
        public float TyreTempFL { get; set; }
        public float TyreTempFR { get; set; }
        public float SuspensionFL { get; set; }
        public float SuspensionFR { get; set; }
        public float TyreTempRL { get; set; }
        public float TyreTempRR { get; set; }
        public float SuspensionRL { get; set; }
        public float SuspensionRR { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float VelocityZ { get; set; }
        public float RotationPitch { get; set; }
        public float RotationYaw { get; set; }
        public float RotationRoll { get; set; }
        public float AngularVelocityX { get; set; }
        public float AngularVelocityY { get; set; }
        public float AngularVelocityZ { get; set; }
    }
}
