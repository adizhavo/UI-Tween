using System;
using UnityEngine;

[Serializable]
public class PercentageChecker
{
    private CallBackTypeState startTypeState;
    private CallBackTypeState endTypeState;

    public Vector2 EventPercentage = new Vector2(0f, 1f);

    public PercentageChecker(EventProperty.CallbackType StartType, EventProperty.CallbackType EndType)
    {
        this.startTypeState = new CallBackTypeState(StartType);
        this.endTypeState = new CallBackTypeState(EndType);
    }

    public EventProperty.CallbackType? CheckEvent(EventProperty eventProperty, float percentage)
    {
        if (eventProperty == null)
        {
            Debug.LogWarning("EventProperty is null, maybe something went wrong at the initialization? Event will not be fired");
            return null;
        }

        ClampValues(ref EventPercentage.x);
        ClampValues(ref EventPercentage.y);

        float minVal = EventPercentage.y < EventPercentage.x ? EventPercentage.y : EventPercentage.x;
        float maxVal = EventPercentage.x < EventPercentage.y ? EventPercentage.y : EventPercentage.x;

        return EventToFire(percentage, minVal, maxVal);
    }

    private EventProperty.CallbackType? EventToFire(float percentage, float minVal, float maxVal)
    {
        if (percentage >= minVal && !startTypeState.isFired)
        {
            startTypeState.isFired = true;
            return startTypeState.type;
        }
        if (percentage >= maxVal && !endTypeState.isFired)
        {
            endTypeState.isFired = true;
            return endTypeState.type;
        }

        return null;
    }

    public void Reset()
    {
        startTypeState.isFired = false;
        endTypeState.isFired = false;
    }

    private void ClampValues(ref float val)
    {
        if (val > 1f)
            val = 1f;
        else if (val < 0)
            val = 0;
    }
}

[Serializable]
public class CallBackTypeState
{
    public EventProperty.CallbackType type;
    public bool isFired;

    public CallBackTypeState(EventProperty.CallbackType type)
    {
        this.type = type;
        isFired = false;
    }
}