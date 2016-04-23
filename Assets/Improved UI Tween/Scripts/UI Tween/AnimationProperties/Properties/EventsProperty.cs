using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventProperty : CorePropertyComponent
{
    public enum CallbackType
    {
        StartIntro,
        EndIntro,
        StartExit,
        EndExit
    }

    #region Exposed to Editor
    [SerializeField] private List<Callback> callbacks = new List<Callback>();

    public Vector2 IntroEventPercentage
    {
        set
        {
            introChecker.EventPercentage = value;
        }
        get{ return introChecker.EventPercentage; }
    }

    public Vector2 ExitEventPercentage
    {
        set
        {
            exitChecker.EventPercentage = value;
        }
        get{ return exitChecker.EventPercentage; }
    }
    #endregion

    [SerializeField]
    private EventTimeChecker introChecker = new EventTimeChecker(CallbackType.StartIntro, CallbackType.EndIntro);

    [SerializeField] 
    private EventTimeChecker exitChecker = new EventTimeChecker(CallbackType.StartExit, CallbackType.EndExit);

    [HideInInspector]
    private CoreProperty coreProperty;

    public void Initialize(CoreProperty coreProperty)
    {
        this.coreProperty = coreProperty;
    }

    public void Register(Callback callback)
    {
        if (callback != null)
            callbacks.Add(callback);
    }

    public void CheckTweenEvents()
    {
        EventTimeChecker currentChecker = (coreProperty.IsOpened()) ? introChecker : exitChecker;
        CallbackType? type = CheckEventPercentage(currentChecker);

        if (type.HasValue)
        {
            FireCallback(type.Value);
        }
    }

    public void ResetEventChecker()
    {
        EventTimeChecker currentChecker = (coreProperty.IsOpened()) ? introChecker : exitChecker;
        ResetEvent(currentChecker);
    }

    private void FireCallback(CallbackType callbackType)
    {
        foreach (Callback cllb in callbacks)
            if (cllb.Type.Equals(callbackType))
                cllb.Action();
    }

    private CallbackType? CheckEventPercentage(EventTimeChecker eventChecker)
    {
        if (eventChecker != null)
        {
            return eventChecker.CheckEvent(this, coreProperty.Percentage);
        }

        return null;
    }

    private void ResetEvent(EventTimeChecker eventChecker)
    {
        if (eventChecker != null)
        {
            eventChecker.Reset();
        }
    }
}