using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AnimationTrigger : MonoBehaviour
{
    
    private enum TriggerOptions
    {
        ON_START,
        FROM_CODE
    };

    [SerializeField] private TriggerOptions CurrentTriggerOption;

    [SerializeField] private float TimeDelayBetweenAnimations;
    [SerializeField] private bool MustWaitAnimationLength;
    [SerializeField] private List<EasyTween> Animations = new List<EasyTween>();
    [SerializeField] private UnityEvent endEvent;

    private Action callback;
    private AnimationCaller AnimationCaller = new AnimationCaller();

    void Start()
    {
        if (CurrentTriggerOption.Equals(TriggerOptions.ON_START))
            TriggerAnimations();
    }

    public void TriggerAnimations()
    {
        if (AnimationCaller.IsInIdle())
        {
            callback = null;
            AnimationCaller.StartAnimation();
        }
    }

    public void TriggerAnimations(Action callback)
    {
        if (AnimationCaller.IsInIdle())
        {
            AnimationCaller.StartAnimation();
            this.callback = callback;
        }
    }

    public float GetTotalTimeAnimation()
    {
        return Animations.Count * TimeDelayBetweenAnimations;
    }

    void Update()
    {
        AnimationCaller.FrameCall(Animations, TimeDelayBetweenAnimations, MustWaitAnimationLength, EndAnimationEvent);
    }

    private void EndAnimationEvent()
    {
        ReverseAnimationOrder();
        endEvent.Invoke();

        if (callback != null)
        {
            callback();
        }
    }

    public void PauseAnimationTrigger()
    {
        AnimationCaller.PauseAnimation();
    }

    public void ResumeAnimation()
    {
        AnimationCaller.ResumeAnimation();
    }

    public void JumpAtIndex(int i)
    {
        AnimationCaller.JumpAtIndex(i);
    }

    public void JumpAndResumeAtIndex(int i)
    {
        AnimationCaller.JumpAndResumeAtIndex(i);
    }

    public void ReverseAnimationOrder()
    {
        List<EasyTween> newAnimList = new List<EasyTween>();

        for (int index = 0; index < Animations.Count; index++)
        {
            newAnimList.Add(Animations[Animations.Count - index - 1]);
        }

        Animations = newAnimList;
        newAnimList = null;
    }
}

[System.Serializable]
public class AnimationCaller
{
    private enum TriggerState
    {
        IDLE,
        PAUSE,
        ANIMATING}

    ;

    private TriggerState CurrentTriggerState = TriggerState.IDLE;
    private EventSystem eventSystem;

    private int counter = 0;
    private float waitSeconds = 0f;

    public bool IsInIdle()
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

    public void FrameCall(List<EasyTween> Animations, float TimeDelayBetweenAnimations, bool WaitAnimationTime, Action EndCallback)
    {
        if (CurrentTriggerState.Equals(TriggerState.ANIMATING))
        {
            float timeWait = (WaitAnimationTime && counter > 0) ? TimeDelayBetweenAnimations + Animations[counter - 1].GetAnimationDuration() : TimeDelayBetweenAnimations;

            if (waitSeconds < timeWait)
            {
                waitSeconds += Time.unscaledDeltaTime;
                return;
            }
            else if (counter < Animations.Count)
            {
                if (CurrentTriggerState.Equals(TriggerState.PAUSE))
                {
                    CurrentTriggerState = TriggerState.IDLE;
                }

                Animations[counter].OpenCloseObjectAnimation();
                waitSeconds = 0;
                counter++;
                return;
            }

            CurrentTriggerState = TriggerState.IDLE;
            eventSystem.enabled = true;

            if (EndCallback != null)
            {
                EndCallback();
            }
        }
    }
}
