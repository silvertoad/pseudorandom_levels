using strange.extensions.mediation.impl;
using UnityEngine;
using core.task;

public class DialogLoadingMediator : Mediator
{

    [Inject ("INIT_QUEUE")]
    public TaskQueue initQueue { get; set; }

    [Inject]
    public appsignals.StartupBunleLoadingSignal onStartLoading { get; set; }

    [Inject] 
    public appsignals.InitCompleteSignal onInitComplete { get; set; }

    [Inject]
    public DialogLoadingView view { get; set; }

    int tasksTotalCount;

    public override void OnRegister ()
    {
        onStartLoading.AddListener (StartLoadingHandler);
        onInitComplete.AddListener (InitCompleteHandler);
    }

    public override void OnRemove ()
    {
        Debug.Log ("DialogLoadingMediator OnRemove");
    
        onInitComplete.RemoveListener (InitCompleteHandler);
        onStartLoading.RemoveListener (StartLoadingHandler);
        initQueue.OnTaskComplete.RemoveListener (LoadingProgressHandler);
    }

    void InitCompleteHandler ()
    {
        Destroy (gameObject);
    }

    void StartLoadingHandler ()
    {
        view.ShowProgress ();
        tasksTotalCount = initQueue.Count;

        onStartLoading.RemoveListener (StartLoadingHandler);
        initQueue.OnTaskComplete.AddListener (LoadingProgressHandler);
    }

    void LoadingProgressHandler (ITask _task)
    {
        Debug.Log (tasksTotalCount + ": " + initQueue.Count);
        view.SetProgress ((tasksTotalCount - initQueue.Count) / (tasksTotalCount * 1f));
    }
}