using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AlphaExecutor : StateExecutor, CorePropertyComponent
{
    #region Exposed to Editor
    public float StartValue
    {
        set
        {
            if (value >= 0f && value < 1f)
                startValue = value;
        }
        get { return startValue; }
    }

    public float EndValue
    {
        set
        {
            if (value > 0f && value <= 1f)
                endValue = value;
        }
        get { return endValue; }
    }

    public bool OverrideChilds = false;
    #endregion
    [SerializeField] [HideInInspector] private float startValue = 0f;
    [SerializeField] [HideInInspector] private float endValue = 1f;

    [HideInInspector]
    private float initialPoint;
    [HideInInspector]
    private float finalPoint;
    [HideInInspector]
    private CoreProperty coreProperty;

    #region CorePropertyComponent Implementation
    public virtual void Initialize(CoreProperty coreProperty)
    {
        // Initializer Guard
        if (coreProperty == null)
        {
            Debug.LogWarning("You Inserted a null CoreProperty! Fade will not play");
            return;
        }

        this.coreProperty = coreProperty;
        SetFadeValues(coreProperty.IsOpened());
    }
    #endregion

    private void SetFadeValues(bool isAnimationOpen)
    {
        initialPoint = (!isAnimationOpen) ? startValue : endValue;
        finalPoint = (!isAnimationOpen) ? endValue : startValue;
    }

    #region TweenExecutor Implementation
    public override void ExecuteTween(RectTransform animatedRect)
    {
        if (coreProperty == null)
            return;

        if (isExecutorEnabled())
            ApplyProperty(animatedRect.transform);
    }
    #endregion

    private void ApplyProperty(Transform alphaTr)
    {
        if (coreProperty == null)
            return;

        ApplyToChild(alphaTr);
        ApplyAlpha(alphaTr);
    }

    private void ApplyToChild(Transform alphaTr)
    {
        // better not using for each here, lot of ms lost if the tranform hierarchy is huge...
        for (int i = 0; i < alphaTr.childCount; i++)
        {
            // lets hope Unity uses some array indexing, hope to save some ms
            CheckChild(alphaTr.GetChild(i));
        }
    }

    private void CheckChild(Transform child)
    {
        if (child.gameObject.activeSelf && 
           (!child.GetComponent<ReferencedFrom>() || OverrideChilds))
        {
            ApplyAlpha(child);
        }
    }

    private void ApplyAlpha(Transform alphaTr)
    {
        if (!alphaTr.GetComponent<MaskableGraphic>())
            return;

        MaskableGraphic GraphicElement = alphaTr.GetComponent<MaskableGraphic>();
        Color objectColor = GraphicElement.color;
        objectColor.a = Mathf.Abs(initialPoint + (finalPoint - initialPoint) * coreProperty.ExactPercentage);
        GraphicElement.color = objectColor;
    }
}