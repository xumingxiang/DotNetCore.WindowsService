using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeterKottas.DotNetCore.WindowsService.Base
{
    public class Cronners
    {
        List<Cronner> cronners = new List<Cronner>();

        public void Start(string timerName, string cron, Action onTimer, Action<Exception> onException = null)
        {
            var tmpTimer = cronners.Where(x => x.Name == timerName).FirstOrDefault();
            if (tmpTimer == null)
            {
                tmpTimer = new Cronner(timerName, cron, onTimer, onException);
                cronners.Add(tmpTimer);
                tmpTimer.Start();
            }
            else
            {
                tmpTimer.Stop();
                Update(timerName, cron, onTimer, onException);
                tmpTimer.Start();
            }
        }

        public void Update(string timerName, string cron, Action onTimer = null, Action<Exception> onException = null)
        {
            var tmpTimer = cronners.Where(x => x.Name == timerName).FirstOrDefault();
            if (tmpTimer != null)
            {
                if (onTimer != null)
                {
                    tmpTimer.OnTimer = onTimer;
                }
                if (onException != null)
                {
                    tmpTimer.OnException = onException;
                }

                if (CronExpression.IsValidExpression(cron) && cron != tmpTimer.Cron)
                {
                    tmpTimer.Cron = cron;
                }
            }
        }

        public void Resume()
        {
            foreach (var timer in cronners)
            {
                timer.Resume();
            }
        }

        public void Resume(string timerName)
        {
            var tmpTimer = cronners.Where(x => x.Name == timerName).FirstOrDefault();
            if (tmpTimer != null)
            {
                tmpTimer.Resume();
            }
        }

        public void Pause()
        {
            foreach (var timer in cronners)
            {
                timer.Pause();
            }
        }

        public void Pause(string timerName)
        {
            var tmpTimer = cronners.Where(x => x.Name == timerName).FirstOrDefault();
            if (tmpTimer != null)
            {
                tmpTimer.Pause();
            }
        }

        public void Stop()
        {
            foreach (var timer in cronners)
            {
                timer.Stop();
            }
        }

        public void Stop(string timerName)
        {
            var tmpTimer = cronners.Where(x => x.Name == timerName).FirstOrDefault();
            if (tmpTimer != null)
            {
                tmpTimer.Stop();
            }
        }
    }
}
