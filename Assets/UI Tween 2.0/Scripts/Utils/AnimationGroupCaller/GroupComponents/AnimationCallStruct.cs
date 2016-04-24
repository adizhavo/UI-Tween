using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AnimationCall
{
    public enum Actions
    {
        NOTHING,
        PAUSE
    };

    public UITween Animation;
    public Actions ActionOnEnd;

    public void PlayAnimation()
    {
        Animation.Animate();
    }

    public float GetLength()
    {
        return Animation.Length();
    }

    public void ExecuteAction(AnimationGroupCaller caller)
    {
        if (ActionOnEnd.Equals(Actions.PAUSE))
        {
            caller.PauseAnimation();
        }
    }
}