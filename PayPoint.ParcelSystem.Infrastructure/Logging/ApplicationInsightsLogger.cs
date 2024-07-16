using Microsoft.ApplicationInsights;
using PayPoint.ParcelSystem.Domain.Interfaces;

namespace PayPoint.ParcelSystem.Infrastructure.Logging;

public class ApplicationInsightsLogger : ILogger
{
    private readonly TelemetryClient _telemetryClient;

    public ApplicationInsightsLogger(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    public void Log(string message)
    {
        _telemetryClient.TrackTrace(message);
    }
}