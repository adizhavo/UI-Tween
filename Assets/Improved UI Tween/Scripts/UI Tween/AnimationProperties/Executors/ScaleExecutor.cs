using UnityEngine;

[System.Serializable]
public class ScaleExecutor : CurveExecutor
{
    public override void ExecuteTween(RectTransform animatedRect)
    {
        base.ExecuteTween(animatedRect);
        animatedRect.localScale = CalculatedPoint;
    }
}