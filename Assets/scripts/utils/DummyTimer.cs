using System;
using UnityEngine;

public class DummyTimer : MonoBehaviour
{
    public Action OnComplete = delegate {
    };
    bool started = false;
    float delay;
    float initDelay;

    public float TimeLeft { get; private set; }

    void Update ()
    {
        if (started) {
            TimeLeft += Time.deltaTime;
            delay -= Time.deltaTime;
            if (delay <= 0) {
                OnComplete ();
                delay = initDelay;
            }
        }
    }

    public void Dispose ()
    {
        Destroy (gameObject);
    }

    void OnDestroy ()
    {
        OnComplete = null;
    }

    public void StartTimer (float _delay)
    {
        TimeLeft = 0;
        initDelay = delay = _delay / 1000;
        started = true;
    }

    public static DummyTimer Create ()
    {
        return GameUtils.AddToPath<DummyTimer> ("system.timers");
    }

    public static DummyTimer Create (GameObject _parent)
    {
        return _parent.AddComponent<DummyTimer> ();
    }
}