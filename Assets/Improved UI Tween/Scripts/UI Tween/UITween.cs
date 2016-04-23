using UnityEngine;

[System.Serializable]
public class UITween : MonoBehaviour, IFrameTicker
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

    public TimeProperty TimeProperty
    {
        set
        {
            if (value != null)
                timeProperty = value;
        }
        get{ return timeProperty; }
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

    [SerializeField] [HideInInspector] private RectTransform animatedRect;
    [SerializeField] [HideInInspector] private CoreProperty coreProperty = new CoreProperty();
    [SerializeField] [HideInInspector] private TimeProperty timeProperty = new TimeProperty();
    [SerializeField] [HideInInspector] private EventProperty eventProperty = new EventProperty();

    #endregion

    #region PublicExecutors

    public AlphaExecutor Alpha = new AlphaExecutor();
    public ScaleExecutor Scale = new ScaleExecutor();
    public PositionExecutor Position = new PositionExecutor();
    public RotationExecutor Rotation = new RotationExecutor();

    #endregion

    #endregion

    //----
    //-------------------------Exposed to Editor

    public bool IsUnscaledTicker()
    {
        return timeProperty.UnscaledTime;
    }

    public bool IsOpened()
    {
        return coreProperty.IsOpened();
    }

    public float Length()
    {
        return timeProperty.Duration;
    }

    public void Animate()
    {
        // Animation Guard
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Property not correctly initialized");
            return;
        }
        if (coreProperty.IsAnimating())
        {
            Debug.Log("Wait for the animation to complete.");
            return;
        }

        TickObserver.Instance.Register(this);
        coreProperty.Start(animatedRect.gameObject);
        BootComponents();
    }

    protected virtual void BootComponents()
    {
        InitializeProp(Alpha);
        InitializeProp(Scale);
        InitializeProp(Position);
        InitializeProp(Rotation);
        InitializeProp(eventProperty);
    }

    protected void InitializeProp(CorePropertyReader prop)
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
        coreProperty.PostProcess(animatedRect.gameObject);
        eventProperty.ResetEventChecker();
        TickObserver.Instance.UnRegister(this);
    }

    private void ApplyTween(float deltaTime)
    {
        float deltaDuration = timeProperty.GetPercentageDuration(deltaTime);
        coreProperty.AddPercentage(deltaDuration);
        ApplyProperties();
    }

    protected virtual void ApplyProperties()
    {
        ApplyTransformations(Alpha);
        ApplyTransformations(Scale);
        ApplyTransformations(Position);
        ApplyTransformations(Rotation);
        eventProperty.CheckTweenEvents();
    }

    protected void ApplyTransformations(TweenExecutor executor)
    {
        if (executor.isExecutorEnabled())
            executor.ExecuteTween(animatedRect);
    }

    public void RegisterCallback(Callback instance)
    {
        eventProperty.Register(instance);
    }
}