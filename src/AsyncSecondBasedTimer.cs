namespace SimpleTimer
{
    internal sealed class AsyncSecondDrivenTimer
    {
        private readonly int _intervalSeconds;
        private int _elapsed;
        private bool _stop;
        public event Action<AsyncSecondDrivenTimer> OnSecondElapsed;
        public event Action OnIntervalElapsed;

        public int IntervalSeconds => _intervalSeconds;
        public int Elapsed => _elapsed;


        public AsyncSecondDrivenTimer(int intervalSeconds)
        {
            _intervalSeconds = intervalSeconds;
        }

        public async Task Start()
        {
            while (_elapsed < _intervalSeconds)
            {
                await Task.Delay(1000);
                _elapsed++;
                if (_stop)
                {
                    return;
                }
                OnSecondElapsed?.Invoke(this);
            }
            OnIntervalElapsed?.Invoke();
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}
