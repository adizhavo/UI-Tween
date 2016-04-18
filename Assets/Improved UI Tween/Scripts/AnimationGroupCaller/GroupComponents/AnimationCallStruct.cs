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

    public EasyTween Animation;
    public Actions ActionOnEnd;

    public void PlayAnimation()
    {
        Animation.OpenCloseObjectAnimation();
    }

    public float GetAnimationLength()
    {
        return Animation.GetAnimationDuration();
    }

    public void ExecuteAction(AnimationGroupCaller caller)
    {
        if (ActionOnEnd.Equals(Actions.PAUSE))
        {
            PauseAction(caller);
        }
    }

    private void PauseAction(AnimationGroupCaller caller)
    {
        caller.PauseAnimation();
    }
}