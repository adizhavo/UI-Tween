using UnityEngine;

public class DisableGameObjectCallback : Callback 
{
    [SerializeField] private UITween tweenListen = null;
    [SerializeField] private GameObject DisableGameObject = null;

    private void Awake()
    {
        tweenListen.RegisterCallback(this);
    }

    public override void Action()
    {
        DisableGameObject.SetActive(false);
    }
}