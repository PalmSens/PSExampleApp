using System;
using Xamarin.Forms;

namespace PSExampleApp.Common.Models
{
    public class Countdown
    {
        private const int _interval = 100;

        public event Action Completed;

        public event Action Ticked;

        public double ElapsedTime { get; set; }
        public double RemainingTimeInMilliSeconds { get; set; }
        public double TotalTimeInMilliSeconds { get; set; }

        public void Start(int DurationInMilliseconds)
        {
            TotalTimeInMilliSeconds = DurationInMilliseconds;
            RemainingTimeInMilliSeconds = DurationInMilliseconds;
            ElapsedTime = 0;

            Device.StartTimer(TimeSpan.FromMilliseconds(_interval), () =>
            {
                ElapsedTime = ElapsedTime + _interval;
                RemainingTimeInMilliSeconds = RemainingTimeInMilliSeconds - _interval;

                var ticked = RemainingTimeInMilliSeconds > 0;

                if (ticked)
                {
                    Ticked?.Invoke();
                }
                else
                {
                    Completed?.Invoke();
                }

                return ticked;
            });
        }
    }
}