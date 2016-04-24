using UnityEngine;
using System.Collections;

public class TweenCallback : Callback {

    [SerializeField] private UITween tweenListen;
    [SerializeField] private AnimationGroup groupCall;

    private void Awake()
    {
        Type = EventProperty.CallbackType.EndIntro;
        tweenListen.RegisterCallback(this);
    }

    public override void Action()
    {
        StopAllCoroutines();
        StartCoroutine( WaitForGroupToFinish() );
    }

    private IEnumerator WaitForGroupToFinish()
    {
        while (groupCall.isAnimating())
            yield return null;

        groupCall.StartAnimations();
    }
}
