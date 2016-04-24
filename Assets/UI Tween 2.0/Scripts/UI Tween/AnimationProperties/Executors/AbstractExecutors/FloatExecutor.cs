using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class FloatExecutor : StateExecutor, CorePropertyReader
{
    #region Exposed to Editor
    public float StartValue
    {
        set
        {
            if (value >= 0f && value <= 1f)
                startValue = value;
        }
        get { return startValue; }
    }

    public float EndValue
    {
        set
        {
            if (value >= 0f && value <= 1f)
                endValue = value;
        }
        get { return endValue; }
    }

    public bool OverrideChilds = false;
    #endregion

    protected CoreProperty CoreProperty { get { return coreProperty; } }
    protected FloatProperty FloatProperty { get { return floatProperty; } }
    
    [SerializeField] [HideInInspector] private CoreProperty coreProperty;
    [SerializeField] [HideInInspector] private FloatProperty floatProperty;

    [SerializeField] [HideInInspector] private float startValue = 0f;
    [SerializeField] [HideInInspector] private float endValue = 1f;

    #region CorePropertyComponent Implementation
    public virtual void Initialize(CoreProperty coreProperty)
    {
        // Initializer Guard
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Fade will not play");
            return;
        }

        this.coreProperty = coreProperty;
        floatProperty = new FloatProperty(startValue, endValue, coreProperty);
    }
    #endregion

    #region TweenExecutor Implementation
    public override void ExecuteTween(RectTransform animatedRect)
    {
        if (coreProperty == null)
            return;

        if (isExecutorEnabled())
            ApplyProperty(animatedRect.transform);
    }
    #endregion

    protected abstract void ApplyProperty(Transform alphaTr);
}