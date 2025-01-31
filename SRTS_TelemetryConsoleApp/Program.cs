// See https://aka.ms/new-console-template for more information
using SimRacingTelemetryLogger.Logger.GT7;
using SimRacingTelemetryLogger.Logger.TelemetryPackets;

try
{
    Console.WriteLine("SRTS Telemetry Console Tester 1.0");

    GT7TelemetryLogger logger = new(33740, 33739, "10.0.10.128", WriteInformation);
    await logger.StartLoggingAsync();

    Console.WriteLine("Press any key to finish.");
    Console.ReadKey();
}
catch(Exception e)
{
    Console.WriteLine($"Error: {e}");
}

void WriteInformation(TelemetryPacket packet)
{
    try
    {
        GT7TelemetryPacket telemetryPacket = packet as GT7TelemetryPacket;

        if (telemetryPacket != null)
        {
            // Escrever título apenas uma vez
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("=== CAR INFORMATION ======================================");

            // Atualizar as informações da velocidade, marcha e outros dados
            Console.SetCursorPosition(0, 1); // Posição da linha para velocidade
            Console.WriteLine($"Speed.........: {telemetryPacket.CarTelemetryPacket.CarSpeed.ToString("000.00")}km/h");

            Console.SetCursorPosition(0, 2); // Posição da linha para marcha atual
            Console.WriteLine($"Current gear..: {telemetryPacket.CarTelemetryPacket.CurrentGear}");

            Console.SetCursorPosition(0, 3); // Posição da linha para marcha sugerida
            Console.WriteLine($"Suggested gear: {(telemetryPacket.CarTelemetryPacket.SuggestedGear != 15 ?
                telemetryPacket.CarTelemetryPacket.SuggestedGear : "-")}");

            Console.SetCursorPosition(0, 4); // Posição da linha para throttle
            Console.WriteLine($"Throttle......: {telemetryPacket.CarTelemetryPacket.Throttle.ToString("00.0")}%");

            Console.SetCursorPosition(0, 5); // Posição da linha para brake
            Console.WriteLine($"Brake.........: {telemetryPacket.CarTelemetryPacket.Brake.ToString("00.0")}%");

            Console.SetCursorPosition(0, 6); // Posição da linha para RPM
            Console.WriteLine($"RPM...........: {telemetryPacket.CarTelemetryPacket.Rpm}");

            // Separador para as informações de posição
            Console.SetCursorPosition(0, 8);
            Console.WriteLine("=== POSITION =============================================");

            Console.SetCursorPosition(0, 9); // Posição X
            Console.WriteLine($"X: {telemetryPacket.CarTelemetryPacket.PositionX.ToString("00.0")}");

            Console.SetCursorPosition(0, 10); // Posição Y
            Console.WriteLine($"Y: {telemetryPacket.CarTelemetryPacket.PositionY.ToString("00.0")}");

            Console.SetCursorPosition(0, 11); // Posição Z
            Console.WriteLine($"Z: {telemetryPacket.CarTelemetryPacket.PositionZ.ToString("00.0")}");

            // Separador para as informações da sessão
            Console.SetCursorPosition(0, 13);
            Console.WriteLine("=== SESSION INFORMATION ==================================");

            // Atualizar as informações de tempo de volta e outras métricas
            Console.SetCursorPosition(0, 14); // Atual volta
            Console.WriteLine($"Current lap.....: {telemetryPacket.SessionTelemetryPacket.CurrentLap}");

            Console.SetCursorPosition(0, 15); // Última volta
            Console.WriteLine($"Last lap........: {telemetryPacket.SessionTelemetryPacket.LastLap.ToString(@"mm\:ss\:fff")}");

            Console.SetCursorPosition(0, 16); // Melhor volta
            Console.WriteLine($"Best lap........: {telemetryPacket.SessionTelemetryPacket.BestLap.ToString(@"mm\:ss\:fff")}");

            Console.SetCursorPosition(0, 17); // Total de voltas
            Console.WriteLine($"Total laps......: {telemetryPacket.SessionTelemetryPacket.TotalLaps}");

            Console.SetCursorPosition(0, 18); // Posição atual
            Console.WriteLine($"Position........: {telemetryPacket.SessionTelemetryPacket.CurrentPosition} of {telemetryPacket.SessionTelemetryPacket.TotalPositions}");

            Console.SetCursorPosition(0, 19); // Tempo na pista
            Console.WriteLine($"Time of day track: {telemetryPacket.SessionTelemetryPacket.TimeOnTrack.ToString(@"hh\:mm\:ss")}");
        }
    }
    catch (Exception e)
    {
        // Tratar exceções
        Console.SetCursorPosition(0, 20);
        Console.WriteLine($"Error: {e.Message}");
    }
}