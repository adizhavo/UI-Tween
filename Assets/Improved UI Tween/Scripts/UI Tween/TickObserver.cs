using UnityEngine;
using System.Collections.Generic;

public class TickObserver : MonoBehaviour
{
    private static TickObserver instance;

    public static TickObserver Instance { get { return instance; } }

    private List<IFrameTicker> subscribers = new List<IFrameTicker>();

    public void Register(IFrameTicker subscriber)
    {
        if (subscriber != null)
            this.subscribers.Add(subscriber);
    }

    public void UnRegister(IFrameTicker subscriber)
    {
        if (subscriber != null)
            this.subscribers.Remove(subscriber);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (subscribers.Count == 0)
            return;
        
        for (int index = 0; index < subscribers.Count; index++)
        {
            float deltaTime = (subscribers[index].IsUnscaledTicker()) ? 
                    Time.unscaledDeltaTime : 
                    Time.deltaTime;
                
            subscribers[index].Tick(deltaTime);
        }
    }
}
