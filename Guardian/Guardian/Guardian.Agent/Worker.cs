using Guardian.Common.Interfaces;

namespace Guardian.Agent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISoftwareScanner _softwareScanner;

        public Worker(ILogger<Worker> logger, ISoftwareScanner softwareScanner)
        {
            _logger = logger;
            _softwareScanner = softwareScanner;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Guardian Agent Started");

            var applications = await _softwareScanner.ScanAsync();

            foreach (var app in applications.Take(20))
            {
                _logger.LogInformation("{Name} | {Version}", app.Name, app.Version);
            }

            _logger.LogInformation("Found {Count} applications", applications.Count);

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
