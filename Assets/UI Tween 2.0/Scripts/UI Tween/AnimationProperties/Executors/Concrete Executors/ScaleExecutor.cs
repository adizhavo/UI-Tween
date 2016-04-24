using UnityEngine;

[System.Serializable]
public class ScaleExecutor : CurveExecutor
{
    protected override void ApplyCalculation(RectTransform animatedRect, Vector3 calculatedPoint)
    {
        animatedRect.localScale = calculatedPoint;
    }
}