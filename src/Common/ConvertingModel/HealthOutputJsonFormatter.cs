using System.Text.Json;
using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Json;

namespace Common.ConvertingModel;

public class HealthOutputJsonFormatter : IHealthOutputFormatter
{
    
    private readonly JsonSerializerOptions _serializerSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealthOutputJsonFormatter"/> class.
    /// </summary>
    public HealthOutputJsonFormatter()
    {
        this._serializerSettings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true,
            MaxDepth = 32,
            WriteIndented = true,
        };

    }

    /// <summary>
    /// Gets the type of the media.
    /// </summary>
    public HealthMediaTypeValue MediaType
    {
        get { return new HealthMediaTypeValue("application", "vnd.appmetrics.health", "v1", "json"); }
    }

    /// <summary>
    /// Writes the asynchronous.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="healthStatus">The health status.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="ArgumentNullException">output</exception>
    public async Task WriteAsync(
        Stream output,
        HealthStatus healthStatus,
        CancellationToken cancellationToken = default)
    {
        if (output == null) throw new ArgumentNullException(nameof(output));
        if (healthStatus.Results == null)
            throw new ArgumentNullException($"{nameof(healthStatus)}.{nameof(healthStatus.Results)}");

        var healthyChecks = healthStatus.Results.Where(r => r.Check.Status.IsHealthy())
            .Select(c => new KeyValuePair<string, string>(c.Name, CheckMessage(c)))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        var unhealthyChecks = healthStatus.Results.Where(r => r.Check.Status.IsUnhealthy())
            .Select(c => new KeyValuePair<string, string>(c.Name, CheckMessage(c)))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        var degradedChecks = healthStatus.Results.Where(r => r.Check.Status.IsDegraded())
            .Select(c => new KeyValuePair<string, string>(c.Name, CheckMessage(c)))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        var ignoredChecks = healthStatus.Results.Where(r => r.Check.Status.IsIgnored())
            .Select(c => new KeyValuePair<string, string>(c.Name, CheckMessage(c)))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        var healthStatusData = new HealthStatusData
        {
            Status = HealthConstants.HealthStatusDisplay[healthStatus.Status],
            Healthy = healthyChecks.Any() ? healthyChecks : null,
            Unhealthy = unhealthyChecks.Any() ? unhealthyChecks : null,
            Degraded = degradedChecks.Any() ? degradedChecks : null,
            Ignored = ignoredChecks.Any() ? ignoredChecks : null,
        };

        await JsonSerializer.SerializeAsync(output, healthStatusData, this._serializerSettings, CancellationToken.None)
            .ConfigureAwait(false);
    }

    private static string CheckMessage(HealthCheck.Result c)
    {
        return !c.IsFromCache ? c.Check.Message : "[Cached] " + c.Check.Message;
    }
}