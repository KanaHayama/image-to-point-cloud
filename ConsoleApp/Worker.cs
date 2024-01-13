using Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleApp {
    internal sealed class Worker : BackgroundService {

        private readonly IHostApplicationLifetime _lifetime;

        private readonly IOptions<ConsoleAppOptions> _options;

        private readonly Processor _processor;

        public Worker(IHostApplicationLifetime lifetime, IOptions<ConsoleAppOptions> options, Processor processor) {
            _lifetime = lifetime;
            _options = options;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            await Task.Delay(1);
            try {
                Console.WriteLine(_options.Value.File);
            } finally {
                _lifetime.StopApplication();
            }
        }
    }
}
