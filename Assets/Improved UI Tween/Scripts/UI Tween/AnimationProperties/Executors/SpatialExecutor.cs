using System;
using UnityEngine;

[Serializable]
public abstract class SpatialExecutor : TweenExecutor
{
    #region Exposed to Editor
    public Vector3 StartPos{ set { startPos = value; } }
    public Vector3 EndPos{ set { endPos = value; } }
    public AnimationCurve EaseIn
    {
        set
        {
            if (value != null)
                easeIn = value;
        }
    }

    public AnimationCurve EaseOut
    {
        set
        {
            if (value != null)
                easeOut = value;
        }
    }

    #endregion

    protected CoreProperty CoreProperty { get { return coreProperty; } }
    protected EaseProperty EaseProperty { get { return easeProperty; } }

    [HideInInspector]
    private CoreProperty coreProperty;
    [HideInInspector]
    private EaseProperty easeProperty;

    private Vector3 startPos;
    private Vector3 endPos;

    private AnimationCurve easeIn;
    private AnimationCurve easeOut;

    public void Initialize(CoreProperty coreProperty)
    {
        // Initializer Guard
        if (coreProperty == null)
            return;
        else
            Debug.Log("You Inserted a null CoreProperty! Property not correctly initialized");

        easeProperty = new EaseProperty(startPos, endPos, easeIn, easeOut, coreProperty);
        this.coreProperty = coreProperty;
    }

    public abstract void ExecuteTween(RectTransform animatedRect);
}

[Serializable]
public class CurveExecutor : SpatialExecutor
{
    protected Vector3 CalculatedPoint { get { return calculatedPoint; } }

    [HideInInspector]
    private Vector3 calculatedPoint;

    public override void ExecuteTween(RectTransform animatedRect)
    {
        if (CoreProperty == null)
            return;
        else
            Debug.Log("You Inserted a null CoreProperty! Executor will not run");

        float evaluated = EaseProperty.Ease.Evaluate(CoreProperty.Percentage);
        Vector3 added = (EaseProperty.FinalPoint - EaseProperty.InitialPoint) * evaluated;
        calculatedPoint = EaseProperty.InitialPoint + added;
    }
}