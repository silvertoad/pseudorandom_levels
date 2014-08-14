namespace core.task
{
    public abstract class BaseTask : ITask
    {
        public BaseTask ()
        {
            OnCancel = new strange.extensions.signal.impl.Signal<ITask> ();
            OnComplete = new strange.extensions.signal.impl.Signal<ITask> ();
        }

        #region ITask implementation

        public abstract void Start ();

        public abstract void Cancel ();

        public strange.extensions.signal.impl.Signal<ITask> OnComplete { get; private set; }

        public strange.extensions.signal.impl.Signal<ITask> OnCancel { get; private set; }

        #endregion

        #region IDisposable implementation

        public abstract void Dispose ();

        #endregion
    }
}