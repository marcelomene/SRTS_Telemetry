using SimRacingTelemetryLogger.Logger.GT7;
using SimRacingTelemetryLogger.Logger.TelemetryPackets;
using SRTS_Telemetry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SRTS_Telemetry.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public string RemoteIPAddress { get; set; }
        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }

        private string logMessages;
        public string LogMessages
        {
            get => logMessages;
            set
            {
                logMessages = value;
                OnPropertyChanged(nameof(LogMessages));
            }
        }

        private GT7TelemetryPacket currentPacket;
        public GT7TelemetryPacket CurrentPacket
        {
            get => currentPacket;
            set
            {
                currentPacket = value;
                OnPropertyChanged(nameof(CurrentPacket));
            }
        }

        #region HudControls
        private double speed;
        public double Speed 
        { 
            get => speed;
            set
            {
                speed = value;
                OnPropertyChanged(nameof(Speed));
            }
        }

        private int rpm;
        public int RPM
        {
            get => rpm;
            set
            {
                rpm = value;
                OnPropertyChanged(nameof(RPM));
            }
        }

        private double throttle;
        public double Throttle
        {
            get => throttle;
            set
            {
                throttle = value;
                OnPropertyChanged(nameof(Throttle));
            }
        }

        private double brake;
        public double Brake
        {
            get => brake;
            set
            {
                brake = value;
                OnPropertyChanged(nameof(Brake));
            }
        }

        private string bestLap;
        public string BestLap
        {
            get => bestLap;
            set
            {
                bestLap = value;
                OnPropertyChanged(nameof(BestLap));
            }
        }

        private string lastLap;
        public string LastLap
        {
            get => lastLap;
            set
            {
                lastLap = value;
                OnPropertyChanged(nameof(LastLap));
            }
        }

        private int gear;
        public int Gear
        {
            get => gear;
            set
            {
                gear = value;
                OnPropertyChanged(nameof(Gear));
            }
        }

        private int position;
        public int Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private int currentLap;
        public int CurrentLap
        {
            get => currentLap;
            set
            {
                currentLap = value;
                OnPropertyChanged(nameof(CurrentLap));
            }
        }

        private int totalLaps;
        public int TotalLaps
        {
            get => totalLaps;
            set
            {
                totalLaps = value;
                OnPropertyChanged(nameof(TotalLaps));
            }
        }

        private double frontLeftTyreTemp;
        public double FrontLeftTyreTemp
        {
            get => frontLeftTyreTemp;
            set
            {
                frontLeftTyreTemp = value;
                OnPropertyChanged(nameof(FrontLeftTyreTemp));
            }
        }

        private double frontRightTyreTemp;
        public double FrontRightTyreTemp
        {
            get => frontRightTyreTemp;
            set
            {
                frontRightTyreTemp = value;
                OnPropertyChanged(nameof(FrontRightTyreTemp));
            }
        }

        private double rearLeftTyreTemp;
        public double RearLeftTyreTemp
        {
            get => rearLeftTyreTemp;
            set
            {
                rearLeftTyreTemp = value;
                OnPropertyChanged(nameof(RearLeftTyreTemp));
            }
        }

        private double rearRightTyreTemp;
        public double RearRightTyreTemp
        {
            get => rearRightTyreTemp;
            set
            {
                rearRightTyreTemp = value;
                OnPropertyChanged(nameof(RearRightTyreTemp));
            }
        }
        #endregion



        private GT7TelemetryLogger _logger;
        
        public MainViewModel()
        {
            ConnectCommand = new Command(async () => await ConnectAsync());
            DisconnectCommand = new Command(() => Disconnect());
            LocalPort = 33740;
            RemotePort = 33739;
            RemoteIPAddress = "10.0.10.128";
        }

        public void Disconnect()
        {
            _logger?.StopLogging();
            _logger?.Dispose();
        }

        public async Task ConnectAsync()
        {
            try
            {
                IPAddress address;
                if (IPAddress.TryParse(RemoteIPAddress, out address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    _logger = new GT7TelemetryLogger(LocalPort, RemotePort, RemoteIPAddress, HandlePacket);
                    await _logger.StartLoggingAsync();
                }
                else
                {
                    LogMessages += "Endereço de IP inválido.";
                }
            }
            catch (Exception ex)
            {
                LogMessages += ex.Message;
                LogMessages += ex.StackTrace;
            }
        }

        private void MapValues()
        {
            RPM                = (int)CurrentPacket.CarTelemetryPacket.Rpm;
            Throttle           = Math.Round(CurrentPacket.CarTelemetryPacket.Throttle, 2);
            Brake              = Math.Round(CurrentPacket.CarTelemetryPacket.Brake, 2);
            BestLap            = CurrentPacket.SessionTelemetryPacket.BestLap.ToString(@"mm\:ss\:fff");
            LastLap            = CurrentPacket.SessionTelemetryPacket.LastLap.ToString(@"mm\:ss\:fff");
            Speed              = Math.Round(CurrentPacket.CarTelemetryPacket.CarSpeed, 2);
            Gear               = CurrentPacket.CarTelemetryPacket.CurrentGear;
            Position           = CurrentPacket.SessionTelemetryPacket.CurrentPosition;
            CurrentLap         = CurrentPacket.SessionTelemetryPacket.CurrentLap;
            TotalLaps          = CurrentPacket.SessionTelemetryPacket.TotalLaps;
            FrontLeftTyreTemp  = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempFL, 2);
            FrontRightTyreTemp = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempFR, 2);
            RearLeftTyreTemp   = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempRL, 2);
            RearRightTyreTemp  = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempRR, 2);
        }

        public void HandlePacket(TelemetryPacket packet)
        {
            try
            {
                if (packet is GT7TelemetryPacket && packet != null)
                {
                    LogMessages = $"Received packet id {packet.PacketId}!\n";
                    CurrentPacket = packet as GT7TelemetryPacket;

                    MapValues();

                    if (_throttleControl != null && _brakeControl != null)
                    {
                        Application.Current.Dispatcher.Dispatch(() =>
                        {
                            _throttleControl.WidthRequest = 3 * Throttle;
                            _brakeControl.WidthRequest = 3 * Brake;
                        });
                    }
                }
                else
                {
                    throw new InvalidDataException("Unsupported packet type");
                }
            }
            catch (Exception ex)
            {
                LogMessages += ex.Message;
                LogMessages += ex.StackTrace;
            }
        }

        private BoxView _throttleControl;
        private BoxView _brakeControl;

        public void SetControls(BoxView throttleControl, BoxView brakeControl)
        {
            _throttleControl = throttleControl;
            _brakeControl = brakeControl;
        }
    }
}
