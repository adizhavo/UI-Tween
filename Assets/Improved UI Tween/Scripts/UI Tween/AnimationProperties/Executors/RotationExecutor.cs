using UnityEngine;

[System.Serializable]
public class RotationExecutor : CurveExecutor
{
    public override void ExecuteTween(RectTransform animatedRect)
    {
        base.ExecuteTween(animatedRect);
        animatedRect.localEulerAngles = CalculatedPoint;
    }
}