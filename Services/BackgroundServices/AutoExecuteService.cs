namespace Services.BackgroundServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.EmailAlert;

public class AutoExecuteService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    readonly ILogger<AutoExecuteService> _logger;
    public AutoExecuteService(ILogger<AutoExecuteService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stopingToken)
    {
        while (!stopingToken.IsCancellationRequested)
        {
            using (var service = serviceProvider.CreateScope())
            {
                var EmailAlertService = service.ServiceProvider.GetRequiredService<IEmailAlert>();
                await EmailAlertService.AddEmployeeAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stopingToken);
            }
        }
    }
}
