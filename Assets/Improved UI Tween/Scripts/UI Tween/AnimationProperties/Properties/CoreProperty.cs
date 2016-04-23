using UnityEngine;

[System.Serializable]
public class CoreProperty
{
    public enum CoreState
    {
        Close,
        Open
    }

    #region Exposed to Editor
    public bool LockState;
    // This is too exposed to other classes, because we want to expose to the editor for the setup
    public CoreState AnimationState;
    #endregion

    public float Percentage { get { return animationPercentage; } }

    [HideInInspector]
    private float animationPercentage;
    [HideInInspector]
    private bool isAnimating = false;

    public void Start()
    {
        ChangeState();
        animationPercentage = 0f;
    }

    public void AddPercentage(float addition)
    {
        animationPercentage += addition;

        if (animationPercentage > 1f)
            animationPercentage = 1f;
    }

    public bool IsOpened()
    {
        return AnimationState.Equals(CoreState.Open);
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }

    public bool HasReachedEnd()
    {
        return animationPercentage >= 1f;
    }

    public void PostProcess()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        isAnimating = !isAnimating;

        if (LockState)
            return;
        
        AnimationState = (AnimationState.Equals(CoreState.Open)) ? CoreState.Close : CoreState.Open;
    } 
}