using UnityEngine;

[System.Serializable]
public class RotationExecutor : CurveExecutor
{
    protected override void ApplyCalculation(RectTransform animatedRect, Vector3 calculatedPoint)
    {
        animatedRect.localEulerAngles = calculatedPoint;
    }
}