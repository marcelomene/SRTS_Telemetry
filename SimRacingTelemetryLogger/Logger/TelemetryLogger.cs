
using log4net;
using SimRacingTelemetryLogger.Logger.TelemetryPackets;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;

namespace SimRacingTelemetryLogger.Logger
{
    public abstract class TelemetryLogger : ITelemetryLogger
    {
        public int ListeningPort { get; set; }
        public int RemotePort { get; set; }
        public bool HasKeepAlive { get; set; }
        public bool SendStartSignal { get; set; }
        public byte[] KeepAliveFrame { get; set; }
        public byte[] StartSignalFrame { get; set; }
        public string RemoteIP { get; set; }

        protected readonly ConcurrentQueue<byte[]> _packetBuffer = new ConcurrentQueue<byte[]>();
        protected static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly UdpClient _udpClient;
        protected CancellationTokenSource _loggingCancellationToken;
        protected CancellationTokenSource _processingCancellationToken;
        protected Action<TelemetryPacket> PacketReadyCallback;

        public TelemetryLogger(int listeningPort)
        {
            ListeningPort = listeningPort;
            _udpClient = new UdpClient(ListeningPort);
            _udpClient.Client.ReceiveTimeout = 5000;
            _udpClient.Client.SendTimeout = 5000;
            _loggingCancellationToken = new CancellationTokenSource();
            _processingCancellationToken = new CancellationTokenSource();
            HasKeepAlive = false;
            KeepAliveFrame = Array.Empty<byte>();
            StartSignalFrame = Array.Empty<byte>();
        }

        public TelemetryLogger(int listeningPort, int remotePort, bool hasKeepAlive, 
            bool sendStartSignal, byte[] keepAliveFrame, byte[] sendStartSignalFrame, string remoteIp, Action<TelemetryPacket> packetReadyCallback)
        {
            ListeningPort = listeningPort;
            _udpClient = new UdpClient(ListeningPort);
            _udpClient.Client.ReceiveTimeout = 5000;
            _udpClient.Client.SendTimeout = 5000;
            _loggingCancellationToken = new CancellationTokenSource();
            _processingCancellationToken = new CancellationTokenSource();
            HasKeepAlive = hasKeepAlive;
            KeepAliveFrame = keepAliveFrame;
            SendStartSignal = sendStartSignal;
            StartSignalFrame = sendStartSignalFrame;
            RemoteIP = remoteIp;
            RemotePort = remotePort;
            PacketReadyCallback = packetReadyCallback;
        }

        public abstract TelemetryPacket ProcessTelemetryPacket(byte[] data);

        protected void StartProcessingPackets()
            => Task.Run(ProcessPacketsAsync, _processingCancellationToken.Token);

        protected virtual async Task ProcessPacketsAsync()
        {
            try
            {
                while (!_processingCancellationToken.IsCancellationRequested)
                {
                    if (_packetBuffer.TryDequeue(out byte[] data))
                    {
                        var packet = ProcessTelemetryPacket(data);
                        await Task.Run(() => PacketReadyCallback.Invoke(packet));
                    }
                    else
                        Thread.Sleep(10);
                }
            }
            catch (Exception e) 
            { 
                _log.Error(e.Message, e); 
            }
        }

        public virtual async Task StartLoggingAsync()
        {
            int packetCount = 0;
            try
            {
                if(SendStartSignal)
                    _udpClient.Send(StartSignalFrame, StartSignalFrame.Length, RemoteIP, RemotePort);

                StartProcessingPackets();

                while (true)
                {
                    _loggingCancellationToken.Token.ThrowIfCancellationRequested();

                    UdpReceiveResult result = await _udpClient.ReceiveAsync(_loggingCancellationToken.Token);
                    _log.Debug($"Telemetry packet received from host {result.RemoteEndPoint.Address}:{result.RemoteEndPoint.Port}");

                    _loggingCancellationToken.Token.ThrowIfCancellationRequested();

                    _packetBuffer.Enqueue(result.Buffer);
                    packetCount++;

                    if (HasKeepAlive && packetCount >= 100) // Sends keep alive message at every 100 packets received.
                    {
                        _udpClient.Send(KeepAliveFrame, KeepAliveFrame.Length, RemoteIP, RemotePort);
                        packetCount = 0;
                    }
                }
            }
            catch (OperationCanceledException) 
            { 
                _log.Debug("Operation cancelled by user.");
                throw;
            }
            catch (Exception e) 
            { 
                _log.Error(e.Message, e);
                throw;
            }
            finally
            {
                _udpClient?.Close();
                _processingCancellationToken?.Cancel();
            }
        }

        public void StopLogging()
            => _loggingCancellationToken?.Cancel();

        public void Dispose()
        {
            _udpClient?.Close();
            _udpClient?.Dispose();
        }
    }
}
