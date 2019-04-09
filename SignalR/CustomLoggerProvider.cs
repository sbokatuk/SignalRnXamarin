using System;
using Microsoft.Extensions.Logging;

namespace SignalR
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private IPingTracker _tracker;

        public CustomLoggerProvider(IPingTracker tracker)
        {
            _tracker = tracker;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(_tracker);
        }

        public void Dispose()
        {
            
        }
    }
}
