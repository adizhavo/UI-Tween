using UnityEngine;

[System.Serializable]
public class CoreProperty
{
    public enum CoreState
    {
        Close,
        Open
    }

    public enum EndAction
    {
        Disable,
        Destroy,
        Nothing
    }

    #region Exposed to Editor
    public bool LockState;
    // This is too exposed to other classes, because we want to expose to the editor for the setup
    public CoreState AnimationState;
    public EndAction FinalAction;
    #endregion
    // we add 10% for the callback, can be adjusted on the editor
    public float CallbackPercentage { get { return animationPercentage + 0.1f; } }
    public float ExactPercentage { get { return animationPercentage; } }

    [HideInInspector] private float animationPercentage = 0f;
    [HideInInspector] private bool isAnimating = false;

    public void Start(GameObject animGame)
    {
        if (isAnimating)
            return;

        isAnimating = true;
        animationPercentage = 0f;
        animGame.SetActive(true);
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

    public void PostProcess(GameObject animGame)
    {
        if (isAnimating && !HasReachedEnd())
            return;

        isAnimating = false;
        ProcessFinalAction(animGame);
        ChangeState();
    }

    protected virtual void ProcessFinalAction(GameObject animGame)
    {
        if (!IsOpened())
            return;

        if (FinalAction.Equals(EndAction.Destroy))
        {
            GameObject.Destroy( animGame );
        }
        else if (FinalAction.Equals(EndAction.Disable))
        {
            animGame.SetActive(false);
        }
    }

    private void ChangeState()
    {
        if (LockState)
            return;
        
        AnimationState = (AnimationState.Equals(CoreState.Open)) ? CoreState.Close : CoreState.Open;
    } 
}