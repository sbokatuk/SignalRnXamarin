using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace SignalR
{
    public class CustomLogger : ILogger
    {
        private IPingTracker _pingMessageTracker;

        public CustomLogger(IPingTracker tracker)
        {
            _pingMessageTracker = tracker;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new LogScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(eventId.Name == "ReceivedPing")
            {
                _pingMessageTracker.Ping();
            }
            Console.WriteLine($"{eventId}, {state}, {exception}, {formatter(state, exception)}");
        }
    }
}
