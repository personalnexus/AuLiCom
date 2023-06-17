namespace AuLiComLib.Common
{
    public class SystemThread : IAsyncExecutor
    {
        private readonly string _name;

        public SystemThread(string name)
        {
            _name = name;
        }

        public void ExecuteAsync(Action action)
        {
            var thread = new Thread(new ThreadStart(action))
            {
                Name = _name
            };
            thread.Start();
        }
    }
}