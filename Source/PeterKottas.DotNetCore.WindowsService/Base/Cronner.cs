using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PeterKottas.DotNetCore.WindowsService.Base
{
    /// <summary>
    /// Cronner
    /// </summary>
    public class Cronner
    {
        private Thread thread;
        private AutoResetEvent stopRequest;
        private bool running = true;
        private bool paused = false;

        public Action OnTimer { get; set; }

        public Action<Exception> OnException { get; set; }

        public string Name { get; private set; }

        public string Cron { get; set; }

        public Cronner(string name, string cron, Action onTimer, Action<Exception> onException = null)
        {
            this.OnTimer = onTimer == null ? () => { } : onTimer;
            this.Name = name;
            this.Cron = cron;
            this.OnException = onException == null ? (e) => { } : onException;
        }

        private void InternalWork(object arg)
        {
            while (running)
            {
                try
                {
                    if (!paused && new CronExpression(this.Cron).IsSatisfiedBy(DateTimeOffset.UtcNow))
                    {
                        this.OnTimer();
                    }
                }
                catch (Exception exception)
                {
                    this.OnException(exception);
                }

                try
                {
                    if (stopRequest.WaitOne(1000))
                    {
                        return;
                    }
                }
                catch (Exception exception)
                {
                    this.OnException(exception);
                }

            }
        }

        public void Start()
        {
            stopRequest = new AutoResetEvent(false);
            running = true;
            thread = new Thread(InternalWork);
            thread.Start();
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
                stopRequest.Set();
                thread.Join();

                thread = null;
                stopRequest.Dispose();
                stopRequest = null;
            }
        }
    }
}
