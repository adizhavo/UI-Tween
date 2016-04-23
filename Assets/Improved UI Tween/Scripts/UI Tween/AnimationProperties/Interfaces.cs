using UnityEngine;

public interface IFrameTicker
{
    void Tick(float deltaTime);
    bool IsUnscaledTicker();
}

public interface CorePropertyComponent
{
    void Initialize(CoreProperty coreProperty);
}

public interface TweenExecutor : CorePropertyComponent
{
    void ExecuteTween(RectTransform animatedRect);
}
