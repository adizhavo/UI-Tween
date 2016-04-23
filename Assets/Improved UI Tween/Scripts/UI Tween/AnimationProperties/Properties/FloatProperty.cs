using UnityEngine;

public struct FloatProperty
{
    #region EasePropertyValues

    public float Initial { get { return initial; } }
    public float Final { get { return final; } }

    #endregion

    private float initial;
    private float final;

    public FloatProperty(float initial, float final, CoreProperty animationCore)
    {
        if (animationCore == null)
        {
            Debug.LogWarning("CoreProperty is null, nothing will be apllied");
            return;
        }

        bool isOpen = !animationCore.IsOpened();
        this.initial = !isOpen ? initial : final;
        this.final = !isOpen ? initial : final;
    }
}
