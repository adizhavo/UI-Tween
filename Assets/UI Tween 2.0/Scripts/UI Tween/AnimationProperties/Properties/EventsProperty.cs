using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventProperty : CorePropertyReader
{
    public enum CallbackType
    {
        StartIntro,
        EndIntro,
        StartExit,
        EndExit
    }

    #region Exposed to Editor

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
    
    [SerializeField] private PercentageChecker introChecker = new PercentageChecker(CallbackType.StartIntro, CallbackType.EndIntro);
    [SerializeField] private PercentageChecker exitChecker = new PercentageChecker(CallbackType.StartExit, CallbackType.EndExit);

    [SerializeField] private List<Callback> callbacks = new List<Callback>();

    [HideInInspector] private CoreProperty coreProperty;

    public void Initialize(CoreProperty coreProperty)
    {
        // Initializer Guard
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Property not correctly initialized");
            return;
        }

        this.coreProperty = coreProperty;
    }

    public void Register(Callback callback)
    {
        if (callback != null)
            callbacks.Add(callback);
    }

    public void CheckTweenEvents()
    {
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Callbacks will not be executed.");
            return;
        }

        PercentageChecker currentChecker = !coreProperty.IsOpened() ? introChecker : exitChecker;
        CallbackType? type = CheckEventPercentage(currentChecker);

        if (type.HasValue)
        {
            FireCallback(type.Value);
        }
    }

    public void ResetEventChecker()
    {
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Callbacks will not be resetted.");
            return;
        }

        PercentageChecker currentChecker = !coreProperty.IsOpened() ? introChecker : exitChecker;
        ResetEvent(currentChecker);
    }

    private void FireCallback(CallbackType callbackType)
    {
        foreach (Callback cllb in callbacks)
            if (cllb.Type.Equals(callbackType))
                cllb.Action();
    }

    private CallbackType? CheckEventPercentage(PercentageChecker eventChecker)
    {
        if (eventChecker != null)
        {
            return eventChecker.CheckEvent(this, coreProperty.Percentage);
        }

        return null;
    }

    private void ResetEvent(PercentageChecker eventChecker)
    {
        if (eventChecker != null)
        {
            eventChecker.Reset();
        }
    }
}