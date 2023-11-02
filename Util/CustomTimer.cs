namespace TidalWrapper.Util
{

    internal class CustomTimer
    {
        private readonly Timer timer;
        private int expirationInSeconds;
        private readonly int intervalInSeconds;
        private bool isStopped = false;

        public CustomTimer(int intervalInSeconds, int expirationInSeconds, Func<Task> operation)
        {
            this.intervalInSeconds = intervalInSeconds;
            this.expirationInSeconds = expirationInSeconds;
            timer = new Timer(async state => await ExecuteAsyncOperation(operation), null, 0, intervalInSeconds * 1000);
        }

        public bool IsStopped
        {
            get { return isStopped; }
        }

        public async Task WaitForCompletion()
        {
            while (!IsStopped)
            {
                await Task.Delay(100);
            }
        }

        private async Task ExecuteAsyncOperation(Func<Task> operation)
        {
            if (expirationInSeconds <= 0)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                Stop();
                Console.WriteLine("Timer expired.");
                return;
            }
            try
            {
                await operation.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while executing the timer operation: {ex.Message}");
            }

            expirationInSeconds -= intervalInSeconds;
        }

        public void Stop()
        {
            timer.Dispose();
            isStopped = true;
        }
    }
}