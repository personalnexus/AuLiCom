namespace AuLiComLib.Common
{
    public class SystemThread : IAsyncExecutor
    {
        public SystemThread(string name)
        {
            _name = name;
        }

        private readonly string _name;
        private Thread _thread;

        public void ExecuteAsync(Action action)
        {
            _thread = new Thread(new ThreadStart(action))
            {
                Name = _name
            };
            _thread.Start();
        }

        public void Join() => _thread.Join();
    }
}