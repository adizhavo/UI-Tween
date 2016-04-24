using UnityEngine;

[System.Serializable]
public class PositionExecutor : CurveExecutor
{
    protected override void ApplyCalculation(RectTransform animatedRect, Vector3 calculatedPoint)
    {
        animatedRect.anchoredPosition = calculatedPoint;
    }
}