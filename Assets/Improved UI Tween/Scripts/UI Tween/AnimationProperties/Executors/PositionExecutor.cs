using UnityEngine;

[System.Serializable]
public class PositionExecutor : CurveExecutor
{
    public override void ExecuteTween(RectTransform animatedRect)
    {
        base.ExecuteTween(animatedRect);
        animatedRect.anchoredPosition = CalculatedPoint;
    }
}