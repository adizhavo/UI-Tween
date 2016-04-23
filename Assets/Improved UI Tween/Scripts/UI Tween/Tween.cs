using UnityEngine;

public class Tween : MonoBehaviour, IFrameTicker
{
    //-------------------------Exposed to Editor
    //----
    #region PropertiesInjector

    #region PublicGuardedSetters

    public RectTransform AnimatedRect
    {
        set
        {
            if (value != null)
                animatedRect = value;
        }
        get { return animatedRect; }
    }

    public CoreProperty CoreProperty
    {
        set
        {
            if (value != null)
                coreProperty = value;
        }
        get{ return coreProperty; }
    }

    public EventProperty EventProperty
    {
        set
        {
            if (value != null)
                eventProperty = value;
        }
        get{ return eventProperty; }
    }

    public TimeProperty TimeProperty
    {
        set
        {
            if (value != null)
                timeProperty = value;
        }
        get{ return timeProperty; }
    }

    private RectTransform animatedRect;
    private CoreProperty coreProperty;
    private EventProperty eventProperty;
    private TimeProperty timeProperty;

    #endregion

    #region PublicExecutors

    public FadeExecutor Fade;
    public CurveExecutor Scale;
    public CurveExecutor Position;
    public CurveExecutor Rotation;

    #endregion

    #endregion
    //----
    //-------------------------Exposed to Editor

    public bool IsUnscaledTicker()
    {
        return timeProperty.UnscaledTime;
    }

    public void Animate()
    {
        // Animation Guard
        if (coreProperty == null)
        {
            Debug.Log("You Inserted a null CoreProperty! Property not correctly initialized");
            return;
        }
        if (coreProperty.IsAnimating())
        {
            Debug.Log("Wait for the animation to complete.");
            return;
        }

        coreProperty.Start();
        BootComponents();
        TickObserver.Instance.Register(this);
    }

    protected virtual void BootComponents()
    {
        InitializeProp(Fade);
        InitializeProp(Scale);
        InitializeProp(Position);
        InitializeProp(Rotation);
        InitializeProp(eventProperty);
    }

    protected void InitializeProp(CorePropertyComponent prop)
    {
        if (coreProperty == null || prop != null)
            prop.Initialize(coreProperty);
    }

    public void Tick(float deltaTime)
    {
        if (coreProperty == null)
            return;

        if (coreProperty.HasReachedEnd())
            ClearTween();
        else
            ApplyTween(deltaTime);
    }

    private void ClearTween()
    {
        coreProperty.PostProcess();
        TickObserver.Instance.UnRegister(this);
    }

    private void ApplyTween(float deltaTime)
    {
        float deltaDuration = timeProperty.GetPercentageDuration(deltaTime);
        coreProperty.AddPercentage(deltaDuration);
        ApplyProperties();
        eventProperty.CheckTweenEvents();
    }

    protected virtual void ApplyProperties()
    {
        ApplyTransformations(Fade);
        ApplyTransformations(Scale);
        ApplyTransformations(Position);
        ApplyTransformations(Rotation);
    }

    protected void ApplyTransformations(TweenExecutor executor)
    {
        if (executor != null)
            executor.ExecuteTween(animatedRect);
    }

    public void RegisterCallback(Callback instance)
    {
        eventProperty.Register(instance);
    }
}