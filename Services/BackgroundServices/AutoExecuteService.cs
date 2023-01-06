namespace Services.BackgroundServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class AutoExecuteService : BackgroundService
{
    readonly ILogger<AutoExecuteService> _logger;
    public AutoExecuteService(ILogger<AutoExecuteService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stopingToken)
    {
        while (!stopingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(1000), stopingToken);
        }
    }    
}
