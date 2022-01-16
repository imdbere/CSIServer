using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CSIServer.Models;
using CSIServer.Services;
using CSIUserTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CSIServer.Controllers
{
    [ApiController]
    [Route("/csi")]
    public class CSIController : ControllerBase
    {
        private readonly ILogger<CSIController> _logger;
        private readonly CSIMemoryService _csiMemoryService;

        public CSIController(ILogger<CSIController> logger, CSIMemoryService csiMemoryService)
        {
            _logger = logger;
            _csiMemoryService = csiMemoryService;
        }

        [HttpGet]
        public async Task<CSIPacketDTO> Get(int tx, int rx, int subcarrier, int numSamples)
        {
            var packets = _csiMemoryService.GetLastN(numSamples);
            var values = new List<CSIValueDTO>();
        
            foreach (var packet in packets)
            {
                values.Add(new CSIValueDTO
                {
                    // This is wrong
                    Timestamp = Tools.UnixTimeStampToDateTime(packet.StatusPacket.TimeStamp - 1000000000),
                    Value = packet.CSIMatrix[tx, rx, subcarrier]
                });
            }

            var firstPacket = packets[0];
            var dto = new CSIPacketDTO
            {
                NumReceivingAntennas = firstPacket.StatusPacket.NumReceivingAntennas,
                NumTransmittingAntennas = firstPacket.StatusPacket.NumTransmittingAntennas,
                NumSubcarriers = firstPacket.StatusPacket.NumSubcarriers,

                Values = values.ToArray()
            };

            return dto;
        }

        [HttpGet("latest")]
        public async Task<CSIPacketDTO> GetLatest()
        {
            var packets = _csiMemoryService.LastCSIState;

            var values = new List<CSIValueDTO>();
            for (int r = 0; r < packets.StatusPacket.NumReceivingAntennas; r++)
            {
                for (int t = 0; t < packets.StatusPacket.NumTransmittingAntennas; t++)
                {
                    for (int s = 0; s < packets.StatusPacket.NumSubcarriers; s++)
                    {
                        var value = packets.CSIMatrix[r, t, s];
                        values.Add(new CSIValueDTO
                        {
                            Value = value,
                            Rx = r,
                            Tx = t,
                            Subcarrier = s
                        });
                    }
                }
            }


            var dto = new CSIPacketDTO
            {
                NumReceivingAntennas = packets.StatusPacket.NumReceivingAntennas,
                NumTransmittingAntennas = packets.StatusPacket.NumTransmittingAntennas,
                NumSubcarriers = packets.StatusPacket.NumSubcarriers,

                Values = values.ToArray()
            };

            return dto;
        }
    }
}
