using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AlphaExecutor : FloatExecutor
{
    protected override void ApplyProperty(Transform alphaTr)
    {
        if (CoreProperty == null)
            return;

        ApplyToChild(alphaTr);
        ApplyAlpha(alphaTr);
    }

    private void ApplyToChild(Transform alphaTr)
    {
        // better not using for each here, lots of ms lost if the tranform hierarchy is huge...
        for (int i = 0; i < alphaTr.childCount; i++)
        {
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
        objectColor.a = Mathf.Abs(FloatProperty.Initial + (FloatProperty.Final - FloatProperty.Initial) * CoreProperty.Percentage);
        GraphicElement.color = objectColor;
    }
}