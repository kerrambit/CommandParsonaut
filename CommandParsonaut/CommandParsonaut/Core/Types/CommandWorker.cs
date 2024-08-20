namespace CommandParsonaut.Core.Types
{
    public class CommandWorker
    {
        private Action<IList<ParameterResult>> _worker { get; } = null;
        private Func<IList<ParameterResult>, Task> _asyncWorker { get; } = null;

        public bool IsAsync { get; }

        public CommandWorker(Action<IList<ParameterResult>> worker)
        {
            _worker = worker;
            IsAsync = false;
        }

        public CommandWorker(Func<IList<ParameterResult>, Task> asyncWorker)
        {
            _asyncWorker = asyncWorker;
            IsAsync = true;
        }

        public Action<IList<ParameterResult>> GetWorker()
        {
            if (_worker == null)
            {
                throw new InvalidOperationException("Worker is set to null!");
            }

            return _worker;
        }

        public Func<IList<ParameterResult>, Task> GetAsyncWorker()
        {
            if (_asyncWorker == null)
            {
                throw new InvalidOperationException("AsyncWorker is set to null!");
            }

            return _asyncWorker;
        }
    }
}
