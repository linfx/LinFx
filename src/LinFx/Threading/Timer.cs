using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Threading
{
    public class Timer
    {
        /// <summary>
        /// This event is raised periodically according to Period of Timer.
        /// </summary>
        public event EventHandler Elapsed;
        /// <summary>
        /// Task period of timer (as milliseconds).
        /// </summary>
        public int Period { get; set; }

        public ILogger<Timer> Logger { get; set; }

        private readonly System.Threading.Timer _timer;
        private volatile bool _performingTasks;
        private volatile bool _isRunning;

        public Timer()
        {
            Logger = NullLogger<Timer>.Instance;
            _timer = new System.Threading.Timer(TimerCallBack, null, Timeout.Infinite, Timeout.Infinite);
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (Period <= 0)
            {
                throw new LinFxException("Period should be set before starting the timer!");
            }

            lock (_timer)
            {
                _timer.Change(0, Period);
                _isRunning = true;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            lock (_timer)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                while (_performingTasks)
                {
                    Monitor.Wait(_timer);
                }

                _isRunning = false;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called by _taskTimer.
        /// </summary>
        /// <param name="state">Not used argument</param>
        private void TimerCallBack(object state)
        {
            lock (_timer)
            {
                if (!_isRunning || _performingTasks)
                {
                    return;
                }

                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _performingTasks = true;
            }

            try
            {
                Elapsed?.Invoke(this, new EventArgs());
            }
            catch
            {
            }
            finally
            {
                lock (_timer)
                {
                    _performingTasks = false;
                    if (_isRunning)
                    {
                        _timer.Change(Period, Timeout.Infinite);
                    }

                    Monitor.Pulse(_timer);
                }
            }
        }
    }
}
