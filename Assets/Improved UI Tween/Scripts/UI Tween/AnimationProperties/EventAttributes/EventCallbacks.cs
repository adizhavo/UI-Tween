using System;
using UnityEngine;
using UnityEngine.Events;
// Should be an Interface, but we are making this easy for UnityEditor
[Serializable]
public abstract class Callback : MonoBehaviour
{
    #region Exposed to Editor
    public EventProperty.CallbackType Type;
    #endregion
    public abstract void Action();
}

[Serializable]
public class TweenCallbackEvent : Callback
{
    #region Exposed to Editor
    public UnityEvent callbackAction;
    #endregion

    public override void Action()
    {
        if (callbackAction != null)
            callbackAction.Invoke();
    }
}
