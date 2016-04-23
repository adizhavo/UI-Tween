using System;
using UnityEngine;

[Serializable]
public abstract class SpatialExecutor : StateExecutor, CorePropertyReader
{
    #region Exposed to Editor
    public Vector3 StartVector;
    public Vector3 EndVector;

    public AnimationCurve EaseIn
    {
        set
        {
            if (value != null)
                easeIn = value;
        }
        get { return easeIn; }
    }

    public AnimationCurve EaseOut
    {
        set
        {
            if (value != null)
                easeOut = value;
        }
        get { return easeOut; }
    }

    #endregion

    protected CoreProperty CoreProperty { get { return coreProperty; } }
    protected EaseProperty EaseProperty { get { return easeProperty; } }

    [SerializeField] [HideInInspector] private CoreProperty coreProperty;
    [SerializeField] [HideInInspector] private EaseProperty easeProperty;

    [SerializeField] private AnimationCurve easeIn = new AnimationCurve();
    [SerializeField] private AnimationCurve easeOut = new AnimationCurve();


    #region CorePropertyComponent Implementation
    public void Initialize(CoreProperty coreProperty)
    {
        // Initializer Guard
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Property not correctly initialized");
            return;
        }

        easeProperty = new EaseProperty(StartVector, EndVector, easeIn, easeOut, coreProperty);
        this.coreProperty = coreProperty;
    }
    #endregion

    #region StateExecutor Implementation
    public override abstract void ExecuteTween(RectTransform animatedRect);
    #endregion
}

[Serializable]
public abstract class CurveExecutor : SpatialExecutor
{
    public override void ExecuteTween(RectTransform animatedRect)
    {
        if (CoreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Executor will not run");
            return;
        }

        if (isExecutorEnabled())
        {
            float evaluated = EaseProperty.Ease.Evaluate(CoreProperty.ExactPercentage);
            Vector3 added = (EaseProperty.FinalPoint - EaseProperty.InitialPoint) * evaluated;
            Vector3 calculatedPoint = EaseProperty.InitialPoint + added;
            ApplyCalculation(animatedRect, calculatedPoint);
        }
    }

    protected abstract void ApplyCalculation(RectTransform animatedRect, Vector3 calculatedPoint);
}