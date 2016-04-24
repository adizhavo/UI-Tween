using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[System.Serializable]
public class AnimationGroupCaller
{
    private enum TriggerState
    {
        IDLE,
        PAUSE,
        ANIMATING
    };

    private TriggerState CurrentTriggerState = TriggerState.IDLE;
    private EventSystem eventSystem;

    private int counter = 0;
    private float waitSeconds = 0f;

    public bool IsIdle()
    {
        return CurrentTriggerState.Equals(TriggerState.IDLE);
    }

    public void StartAnimation()
    {
        eventSystem = EventSystem.current;
        eventSystem.enabled = false;

        counter = 0;
        waitSeconds = Mathf.Infinity;
        CurrentTriggerState = TriggerState.ANIMATING;
    }

    public void PauseAnimation()
    {
        eventSystem.enabled = true;
        CurrentTriggerState = TriggerState.PAUSE;
    }

    public void ResumeAnimation()
    {
        eventSystem.enabled = false;
        CurrentTriggerState = TriggerState.ANIMATING;
    }

    public void JumpAtIndex(int i)
    {
        counter = i;
    }

    public void JumpAndResumeAtIndex(int i)
    {
        JumpAtIndex(i);
        ResumeAnimation();
    }

    public void FrameCall(List<AnimationCall> Animations, float DelayBetweenCalls, bool WaitAnimationTime, Action EndCallback)
    {
        if (CurrentTriggerState.Equals(TriggerState.ANIMATING))
        {
            AnimateThisFrame(Animations, DelayBetweenCalls, WaitAnimationTime, EndCallback);
        }
    }

    private void AnimateThisFrame(List<AnimationCall> Animations, float DelayBetweenCalls, bool WaitAnimationTime, Action EndCallback)
    {
        float timeWait = WaitAnimationTime && counter > 0 ? DelayBetweenCalls + Animations[counter - 1].GetLength() : DelayBetweenCalls;
        if (waitSeconds < timeWait)
        {
            waitSeconds += Time.unscaledDeltaTime;
            return;
        }
        else if (counter < Animations.Count)
        {
            CheckCallerState();
            Animate(Animations[counter]);
            return;
        }
        IterationOutro(EndCallback);
    }

    private void CheckCallerState()
    {
        if (CurrentTriggerState.Equals(TriggerState.PAUSE))
            CurrentTriggerState = TriggerState.IDLE;
    }

    private void Animate(AnimationCall Animations)
    {
        Animations.PlayAnimation();
        Animations.ExecuteAction(this);

        waitSeconds = 0f;
        counter++;
    }

    private void IterationOutro(Action EndCallback)
    {
        CurrentTriggerState = TriggerState.IDLE;
        eventSystem.enabled = true;
        if (EndCallback != null)
        {
            EndCallback();
        }
    }

}
