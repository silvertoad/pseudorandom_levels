using strange.extensions.signal.impl;
using System;

public interface ITask : IDisposable
{
    Signal<ITask> OnComplete { get; }

    Signal<ITask> OnCancel { get; }

    void Start ();

    void Cancel ();
}