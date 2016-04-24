using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AnimationGroup : MonoBehaviour
{
    private enum TriggerOptions
    {
        ON_START,
        FROM_CODE
    }

    [SerializeField] private TriggerOptions CurrentTriggerOption = TriggerOptions.FROM_CODE;

    [SerializeField] private float DelayBetweenCalls;
    [SerializeField] private bool CallWaitAnimationEnd;
    [SerializeField] private bool PreserveExecutionOrder;

    [SerializeField] private List<AnimationCall> Group = new List<AnimationCall>();
    [SerializeField] private UnityEvent EndEvent;

    private Action callback;
    private AnimationGroupCaller AnimationCaller = new AnimationGroupCaller();

    private void Start()
    {
        //yield return new WaitForEndOfFrame();
        if (CurrentTriggerOption.Equals(TriggerOptions.ON_START))
            StartAnimations();
    }

    public void StartAnimations()
    {
        if (AnimationCaller.IsIdle())
        {
            callback = null;
            AnimationCaller.StartAnimation();
        }
    }

    public void StartAnimations(Action callback)
    {
        if (AnimationCaller.IsIdle())
        {
            this.callback = callback;
            AnimationCaller.StartAnimation();
        }
    }

    void Update()
    {
        AnimationCaller.FrameCall(Group, DelayBetweenCalls, CallWaitAnimationEnd, EndAnimationEvent);
    }

    private void EndAnimationEvent()
    {
        if (!PreserveExecutionOrder)
            ReverseAnimationOrder();
        
        EndEvent.Invoke();

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
        List<AnimationCall> newAnimList = new List<AnimationCall>();

        for (int index = 0; index < Group.Count; index++)
        {
            newAnimList.Add(Group[Group.Count - index - 1]);
        }

        Group = newAnimList;
        newAnimList = null;
    }

    public bool isAnimating()
    {
        return !AnimationCaller.IsIdle();
    }
}