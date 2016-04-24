using UnityEngine;

[System.Serializable]
public class CoreProperty
{
    public enum CoreState
    {
        Close,
        Open
    }

    public enum FinalExit
    {
        Disable,
        Destroy,
        Nothing
    }

    #region Exposed to Editor
    public bool LockState;
    // This is too exposed to other classes, because we want to expose to the editor for the setup
    public CoreState AnimationState;
    public FinalExit FinalAction;
    #endregion
    public float Percentage { get { return Mathf.Clamp01(animationPercentage); } }

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

        if (FinalAction.Equals(FinalExit.Destroy))
        {
            GameObject.Destroy( animGame );
        }
        else if (FinalAction.Equals(FinalExit.Disable))
        {
            animGame.SetActive(false);
        }
    }

    private void ChangeState()
    {
        if (LockState)
            return;
        
        AnimationState = AnimationState.Equals(CoreState.Open) ? CoreState.Close : CoreState.Open;
    } 
}