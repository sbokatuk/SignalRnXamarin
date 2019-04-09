using System;
using System.Timers;

namespace SignalR
{
    public class PingMessageTracker : IDisposable, IPingTracker
    {
        private static int _counter = 0;
        private double _delay;
        private Timer _pingTimer;
        private DateTime _lastPingTime;
        private Action _onFail { get; set; }

        public PingMessageTracker(double pingDelay, Action onFail)
        {
            _counter++;
            _delay = pingDelay;
            _pingTimer = new Timer(pingDelay);
            _pingTimer.Elapsed += _pingTimer_Elapsed;
            _onFail = onFail;
        }

        private void _pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var delta = DateTime.Now.Subtract(_lastPingTime);
            if (delta > TimeSpan.FromMilliseconds(_delay))
            {
                _pingTimer.Stop();
                if (_onFail != null)
                    _onFail();
            }
        }

        public void Ping()
        {
            _lastPingTime = DateTime.Now;
            if (!_pingTimer.Enabled)
                _pingTimer.Start();
        }

        ~PingMessageTracker()
        {
            if (_pingTimer != null)
                Dispose();
        }

        public void Dispose()
        {
            _pingTimer.Stop();
            _pingTimer.Dispose();
            _pingTimer = null;
            _onFail = null;
            _lastPingTime = DateTime.MinValue;
        }
    }
}
