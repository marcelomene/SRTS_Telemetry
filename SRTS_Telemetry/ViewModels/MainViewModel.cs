using CommunityToolkit.Maui.Storage;
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
        public ICommand ClearLogCommand { get; set; }
        public bool SaveSession { get; set; }

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

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private TelemetryPacket currentPacket;
        public TelemetryPacket CurrentPacket
        {
            get => currentPacket;
            set
            {
                currentPacket = value;
                OnPropertyChanged(nameof(CurrentPacket));
            }
        }

        #region HudControls
        private bool hasRevWarning;
        public bool HasRevWarning
        {
            get => hasRevWarning;
            set
            {
                hasRevWarning = value;
                OnPropertyChanged(nameof(HasRevWarning));
            }
        }

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

        private string gear;
        public string Gear
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

        private double oilTemp;
        public double OilTemp
        {
            get => oilTemp;
            set
            {
                oilTemp = value;
                OnPropertyChanged(nameof(OilTemp));
            }
        }

        private double waterTemp;
        public double WaterTemp
        {
            get => waterTemp;
            set
            {
                waterTemp = value;
                OnPropertyChanged(nameof(WaterTemp));
            }
        }

        private double oilPressure;
        public double OilPressure
        {
            get => oilPressure;
            set
            {
                oilPressure = value;
                OnPropertyChanged(nameof(OilPressure));
            }
        }

        private double currentFuel;
        public double CurrentFuel
        {
            get => currentFuel;
            set
            {
                currentFuel = value;
                OnPropertyChanged(nameof(CurrentFuel));
            }
        }

        private string timeOnTrack;
        public string TimeOnTrack
        {
            get => timeOnTrack;
            set
            {
                timeOnTrack = value;
                OnPropertyChanged(nameof(TimeOnTrack));
            }
        }

        private bool pushToPassAvailable;
        public bool PushToPassAvailable
        {
            get => pushToPassAvailable;
            set
            {
                pushToPassAvailable = value;
                OnPropertyChanged(nameof(PushToPassAvailable));
            }
        }

        private bool pushToPassState;
        public bool PushToPassState
        {
            get => pushToPassState;
            set
            {
                pushToPassState = value;
                OnPropertyChanged(nameof(PushToPassState));
            }
        }
        #endregion

        private GT7TelemetryLogger _logger;

        public MainViewModel()
        {
            ConnectCommand = new Command(async () => await ConnectAsync().ConfigureAwait(false));
            DisconnectCommand = new Command(() => Disconnect());
            ClearLogCommand = new Command(() => ClearLog());
            LocalPort = 33740;
            RemotePort = 33739;
            RemoteIPAddress = "10.0.10.128";
        }

        private void ClearLog()
            => LogMessages = "";

        public void Disconnect()
        {
            _logger?.StopLogging();
            _logger?.Dispose();
            IsConnected = false;
        }

        public async Task ConnectAsync()
        {
            try
            {
                IPAddress address;
                if (IPAddress.TryParse(RemoteIPAddress, out address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    string filePath = "";
                    if(SaveSession)
                    {
                       var result = await FolderPicker.Default.PickAsync();
                        filePath = result.Folder.Path + $"session_{DateTime.Now.ToString("f")}.srts";
                    }
                    _logger = new GT7TelemetryLogger(LocalPort, RemotePort, RemoteIPAddress, HandlePacket);
                    IsConnected = true;
                    await _logger.StartLoggingAsync();
                }
                else
                    LogMessages += "Endereço de IP inválido.";
            }
            catch (Exception ex)
            {
                LogMessages += ex.Message;
                Disconnect();
                //LogMessages += ex.StackTrace;
            }
        }

        private void MapValues()
        {
            RPM                 = (int)CurrentPacket.CarTelemetryPacket.Rpm;
            Throttle            = Math.Round(CurrentPacket.CarTelemetryPacket.Throttle, 2);
            Brake               = Math.Round(CurrentPacket.CarTelemetryPacket.Brake, 2);
            BestLap             = CurrentPacket.SessionTelemetryPacket.BestLap.ToString(@"mm\:ss\:fff");
            LastLap             = CurrentPacket.SessionTelemetryPacket.LastLap.ToString(@"mm\:ss\:fff");
            Speed               = Math.Round(CurrentPacket.CarTelemetryPacket.CarSpeed, 2);
            Gear                = CurrentPacket.CarTelemetryPacket.CurrentGear == 0 && CurrentPacket is GT7TelemetryPacket
                                    ? "R" : CurrentPacket.CarTelemetryPacket.CurrentGear.ToString();
            Position            = CurrentPacket.SessionTelemetryPacket.CurrentPosition;
            CurrentLap          = CurrentPacket.SessionTelemetryPacket.CurrentLap;
            TotalLaps           = CurrentPacket.SessionTelemetryPacket.TotalLaps;
            TimeOnTrack         = CurrentPacket.SessionTelemetryPacket.TimeOnTrack.ToString(@"mm\:ss\:fff");
            FrontLeftTyreTemp   = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempFL, 2);
            FrontRightTyreTemp  = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempFR, 2);
            RearLeftTyreTemp    = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempRL, 2);
            RearRightTyreTemp   = Math.Round(CurrentPacket.CarTelemetryPacket.TyreTempRR, 2);
            OilPressure         = Math.Round(CurrentPacket.CarTelemetryPacket.OilPressure, 2);
            OilTemp             = Math.Round(CurrentPacket.CarTelemetryPacket.OilTemp, 2);
            WaterTemp           = Math.Round(CurrentPacket.CarTelemetryPacket.WaterTemp, 2);
            CurrentFuel         = Math.Round(CurrentPacket.CarTelemetryPacket.CurrentFuel, 2);
            PushToPassAvailable = CurrentPacket.CarTelemetryPacket.PushToPassAvailable > 0 ? true : false;
            PushToPassState     = CurrentPacket.CarTelemetryPacket.PushToPass > 0 ? true : false;
        }

        private void UpdateThrottleAndBrakeControls()
        {
            if (_throttleControl != null && _brakeControl != null)
            {
                double availableHeight = _throttleControl.Parent is Grid parentGrid ? parentGrid.Height : 0;

                Application.Current.Dispatcher.Dispatch(() =>
                {
                    _throttleControl.HeightRequest = 3 * Throttle;
                    _brakeControl.HeightRequest = 3 * Brake;
                });
            }
        }

        private void UpdateRevWarningControl()
            => HasRevWarning = CurrentPacket?.CarTelemetryPacket.Rpm > CurrentPacket?.CarTelemetryPacket.RpmRevWarning ?
            true : false;

        private void UpdateRpmIndicatorsControl(bool hadGearChange)
        {
            if (_rpmBoxViews != null && CurrentPacket != null)
            {
                float rpm = CurrentPacket.CarTelemetryPacket.Rpm;
                float revLimit = CurrentPacket.CarTelemetryPacket.RpmRevLimiter;
                float currentRpmPerc = (rpm * 100) / revLimit;

                Application.Current.Dispatcher.Dispatch(() =>
                {
                    foreach (var boxView in _rpmBoxViews)
                        boxView.Opacity = 0.1;
                });

                if(hadGearChange) Thread.Sleep(500); // Wait for 0.5 second to show the change

                int count = (int)Math.Round((currentRpmPerc * _rpmBoxViews.Count) / 100);

                Application.Current.Dispatcher.Dispatch(() =>
                {
                    for (int i = 0; i < count; i++)
                        _rpmBoxViews[i].Opacity = 1;
                });
            }
        }

        public void HandlePacket(TelemetryPacket packet)
        {
            try
            {
                if (packet is GT7TelemetryPacket && packet != null)
                {
                    LogMessages = $"Received packet id {packet.PacketId}!\n";
                    CurrentPacket = packet as GT7TelemetryPacket;

                    var previousGear = Gear;
                    MapValues();

                    // Updates visible controls
                    Task.Run(() => 
                    {
                        UpdateThrottleAndBrakeControls();
                        UpdateRevWarningControl();
                        UpdateRpmIndicatorsControl(Gear != previousGear);
                    });
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
        private List<BoxView> _rpmBoxViews;
        public void SetControls(BoxView throttleControl, BoxView brakeControl, List<BoxView> rpmBoxViews)
        {
            _throttleControl = throttleControl;
            _brakeControl = brakeControl;
            _rpmBoxViews = rpmBoxViews;
        }
    }
}
