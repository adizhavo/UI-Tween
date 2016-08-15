using UnityEngine;

public class DisableGameObjectCallback : Callback 
{
    [SerializeField] private UITween tweenListen;
    [SerializeField] private GameObject DisableGameObject;

    private void Awake()
    {
        tweenListen.RegisterCallback(this);
    }

    public override void Action()
    {
        DisableGameObject.SetActive(false);
    }
}