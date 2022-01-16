using System;
using System.Collections.Generic;
using CSIUserTool;

namespace CSIServer.Services
{
    public class CSIMemoryService
    {
        private CSIPacket _lastCSIState;
        private object _locker = new object();

        private List<CSIPacket> _buffer = new List<CSIPacket>();
        public CSIPacket LastCSIState
        {
            get
            {
                lock (_locker)
                {
                    return _lastCSIState;
                }
            }
        }

        public CSIPacket[] GetLastN(int n)
        {
            lock (_locker)
            {
                n = Math.Min(n, _buffer.Count);
                var slice = _buffer.GetRange(_buffer.Count - n, n);
                return slice.ToArray();
            }
        }

        public void IngestNewPacket(CSIPacket packet)
        {
            lock (_locker)
            {
                _buffer.Add(packet);
                _lastCSIState = packet;
            }
        }
    }
}