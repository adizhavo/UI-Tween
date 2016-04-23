using UnityEngine;

[System.Serializable]
public abstract class StateExecutor : TweenExecutor
{
    protected enum ExecutorState
    {
        Enabled,
        Disabled
    }
    [SerializeField] [HideInInspector] private ExecutorState currentState = ExecutorState.Disabled;

    public abstract void ExecuteTween(RectTransform animatedRect);

    public void EnableExecutor()
    {
        currentState = ExecutorState.Enabled;
    }

    public void DisableExecutor()
    {
        currentState = ExecutorState.Disabled;
    }

    public bool isExecutorEnabled()
    {
        return currentState.Equals(ExecutorState.Enabled);
    }
}