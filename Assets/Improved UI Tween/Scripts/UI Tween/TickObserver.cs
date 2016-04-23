using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TickObserver : MonoBehaviour
{
    public static TickObserver Instance 
    { 
        get 
        { 
            if (instance == null)
            {
                GameObject tickOb = new GameObject("Tick Observer");
                instance = tickOb.AddComponent<TickObserver>();
            }

            return instance; 
        } 
    }
    private static TickObserver instance;

    private List<IFrameTicker> subscribers = new List<IFrameTicker>();

    public void Register(IFrameTicker subscriber)
    {
        if (subscriber != null)
            subscribers.Add(subscriber);
    }

    public void UnRegister(IFrameTicker subscriber)
    {
        if (subscriber != null)
            subscribers.Remove(subscriber);
    }

    private void Awake()
    {
        instance = this;
    }

    private bool sleep = true;

    private IEnumerator Start()
    {
        // wait two frames, doesnt messup the callbacks
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        sleep = false;
    }

    private void Update()
    {
        if (subscribers.Count == 0 || sleep)
            return;
        
        for (int index = 0; index < subscribers.Count; index++)
        {
            float preferedDeltaTime = subscribers[index].IsUnscaledTicker() ? 
                    Time.unscaledDeltaTime : 
                    Time.deltaTime;
                
            subscribers[index].Tick(preferedDeltaTime);
        }
    }
}
