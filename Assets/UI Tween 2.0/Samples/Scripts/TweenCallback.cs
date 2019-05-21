using UnityEngine;
using System.Collections;

public class TweenCallback : Callback {

    [SerializeField] private UITween tweenListen = null;
    [SerializeField] private AnimationGroup groupCall = null;

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
