using Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ConsoleApp {
    internal sealed class Worker : BackgroundService {

        private readonly IHostApplicationLifetime _lifetime;

        private readonly IOptions<ConsoleAppOptions> _options;

        private readonly Processor _processor;

        private readonly Saver _saver;

        private readonly ILogger<Worker> _logger;

        public Worker(IHostApplicationLifetime lifetime, IOptions<ConsoleAppOptions> options, Processor processor, Saver saver, ILogger<Worker> logger) {
            _lifetime = lifetime;
            _options = options;
            _processor = processor;
            _saver = saver;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            await Task.Delay(1);
            try {
                if (!File.Exists(_options.Value.Input)) {
                    _logger.LogError("File not found: {File}", _options.Value.Input);
                    return;
                }
                if (string.IsNullOrEmpty(_options.Value.Output)) {
                    _logger.LogError("Output file not specified");
                    return;
                }
                var image = Image.Load<Argb32>(_options.Value.Input);
                var pointCloud = _processor.Run(image);
                using var stream = File.OpenWrite(_options.Value.Output);
                _saver.Save(pointCloud, stream);
            } finally {
                _lifetime.StopApplication();
            }
        }
    }
}
