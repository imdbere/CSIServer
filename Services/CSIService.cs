using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSIUserTool;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CSIServer.Services
{
    public class CSIService : IHostedService
    {
        private CSIReader _reader;
        private CSIMemoryService _memoryService;

        private readonly ILogger<CSIService> _logger;

        public CSIService(ILogger<CSIService> logger, CSIMemoryService memoryService)
        {
            _logger = logger;
            _memoryService = memoryService;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _reader = new CSIReader("/dev/CSI_dev", 4096);
            _reader.OnNewPacket += (sender, packet) =>
            {
                HandleNewPacket(packet);
            };

            _logger.LogInformation("Starting reading from CSI device");
            _reader.StartReading();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _reader.Stop();

            return Task.CompletedTask;
        }


        private void HandleNewPacket(CSIPacket packet)
        {
            _memoryService.IngestNewPacket(packet);
        }
    }
}