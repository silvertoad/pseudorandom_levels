using System.Collections.Generic;
using strange.extensions.signal.impl;
using UnityEngine;

namespace core.task
{
    public class TaskQueue
    {
        List<ITask> tasks = new List<ITask> ();
        public Signal<ITask> OnTaskComplete = new Signal<ITask> ();
        public Signal OnComplte = new Signal ();

        public bool IsRunning { get; private set; }

        public void Add (ITask _task)
        {
            tasks.Add (_task);
        }

        public void Start ()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            CheckNext ();
        }

        public void Cancel ()
        {
            if (!IsRunning)
                return;

            var currentTask = tasks [0];
            currentTask.OnComplete.RemoveListener (TaskCompleteHandler);
            currentTask.Cancel ();
            tasks = new List<ITask> ();
            IsRunning = false;
        }

        void CheckNext ()
        {
            if (tasks.Count == 0) {
                IsRunning = false;
                OnComplte.Dispatch ();
            } else {
                StartTask (tasks [0]);
            }
        }

        void StartTask (ITask _task)
        {
            _task.OnComplete.AddListener (TaskCompleteHandler);
            _task.Start ();
        }

        void TaskCompleteHandler (ITask _task)
        {
            tasks.Remove (_task);

            _task.OnComplete.RemoveListener (TaskCompleteHandler);
            OnTaskComplete.Dispatch (_task);

            CheckNext ();
        }

        public int Count {
            get { return tasks.Count; }
        }
    }
}