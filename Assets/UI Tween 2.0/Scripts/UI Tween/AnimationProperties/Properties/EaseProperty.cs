using UnityEngine;

public struct EaseProperty
{
    #region EasePropertyValues

    public Vector3 InitialPoint { get { return initialPoint; } }
    public Vector3 FinalPoint { get { return finalPoint; } }

    public AnimationCurve Ease
    {
        get
        {
            if (ease != null)
                return ease;
            else
                return new AnimationCurve();
        }
    }

    #endregion

    private Vector3 initialPoint;
    private Vector3 finalPoint;

    private AnimationCurve ease;

    public EaseProperty(Vector3 StartValues, Vector3 EndValues, AnimationCurve EaseIn, AnimationCurve EaseOut, CoreProperty animationCore)
    {
       
        bool isAnimationOpen = animationCore.IsOpened();

        initialPoint = !isAnimationOpen ? StartValues : EndValues;
        finalPoint = !isAnimationOpen ? EndValues : StartValues;
        ease = !isAnimationOpen ? EaseIn : EaseOut;
    }
}