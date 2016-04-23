using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FadeExecutor : TweenExecutor
{
    #region Exposed to Editor
    public float StartValue
    {
        set
        {
            if (value > 0f && value < 1f)
                startValue = value;
        }
    }

    public float EndValue
    {
        set
        {
            if (value > 0f && value < 1f)
                endValue = value;
        }
    }

    public bool OverrideChilds;
    #endregion

    private float startValue;
    private float endValue;

    [HideInInspector]
    private float initialPoint;
    [HideInInspector]
    private float finalPoint;
    [HideInInspector]
    private CoreProperty coreProperty;

    public virtual void Initialize(CoreProperty coreProperty)
    {
        // Initializer Guard
        if (coreProperty == null)
            return;
        else
            Debug.Log("You Inserted a null CoreProperty! Fade will not play");

        this.coreProperty = coreProperty;
        SetFadeValues(coreProperty.IsOpened());
    }

    private void SetFadeValues(bool isAnimationOpen)
    {
        initialPoint = (isAnimationOpen) ? startValue : endValue;
        finalPoint = (isAnimationOpen) ? endValue : startValue;
    }

    public virtual void ExecuteTween(RectTransform animatedRect)
    {
        if (coreProperty == null)
            return;
        
        ApplyProperty(animatedRect.transform);
    }

    private void ApplyProperty(Transform alphaTr)
    {
        if (coreProperty == null)
            return;

        ApplyToChild(alphaTr);
        ApplyAlpha(alphaTr);
    }

    private void ApplyToChild(Transform alphaTr)
    {
        // better not using for each here, lot of ms lost if the scene is huge...
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
        objectColor.a = Mathf.Abs(initialPoint + (finalPoint - initialPoint) * coreProperty.Percentage);
        GraphicElement.color = objectColor;
    }
}