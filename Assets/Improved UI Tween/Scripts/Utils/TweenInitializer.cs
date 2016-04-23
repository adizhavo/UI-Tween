using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class TweenInitializer : MonoBehaviour
{
    private enum TweenSetupStates
    {
        START_VALUES,
        END_VALUES
    };

    [SerializeField] private TweenSetupStates CurrentState;
    private Tween tweenScript;

    private void Awake()
    {
        if (!IsAnimationSetted() || !Application.isPlaying)
            return;        

        SetupAnimation();
    }

    private bool IsAnimationSetted()
    {
        if (tweenScript == null)
        {
            tweenScript = GetComponent<Tween>();
            if (tweenScript == null)
            {
                Debug.LogError("Easy Tween 2.0 not referenced");
                return false;
            }
        }
        return true;
    }

    private void SetupAnimation()
    {
        bool initialSet = CurrentState.Equals(TweenSetupStates.START_VALUES);
        if (initialSet)
        {
            SetInitialProperties();
        }
        else
        {
            SetFinalProperties();
        }
        tweenScript.AnimatedRect.gameObject.SetActive(!initialSet);
    }

    private void SetInitialProperties()
    {
        if (tweenScript.Scale.isExecutorEnabled())
            tweenScript.AnimatedRect.localScale = tweenScript.Scale.StartVector;

        if (tweenScript.Position.isExecutorEnabled())
            tweenScript.AnimatedRect.anchoredPosition = tweenScript.Position.StartVector;

        if (tweenScript.Rotation.isExecutorEnabled())
            tweenScript.AnimatedRect.localEulerAngles = tweenScript.Rotation.StartVector;

        if (tweenScript.Rotation.isExecutorEnabled())
            tweenScript.AnimatedRect.localEulerAngles = tweenScript.Rotation.StartVector;

        if (tweenScript.Alpha.isExecutorEnabled())
            SetAlphaValue(tweenScript.AnimatedRect.transform, tweenScript.Alpha.StartValue);
    }

    private void SetFinalProperties()
    {
        if (tweenScript.Scale.isExecutorEnabled())
            tweenScript.AnimatedRect.localScale = tweenScript.Scale.EndVector;

        if (tweenScript.Position.isExecutorEnabled())
            tweenScript.AnimatedRect.anchoredPosition = tweenScript.Position.EndVector;

        if (tweenScript.Rotation.isExecutorEnabled())
            tweenScript.AnimatedRect.localEulerAngles = tweenScript.Rotation.EndVector;

        if (tweenScript.Alpha.isExecutorEnabled())
            SetAlphaValue(tweenScript.AnimatedRect.transform, tweenScript.Alpha.EndValue);
    }

    private void SetAlphaValue(Transform alphaTr, float alphaValue)
    {
        SetAlphaToTr(alphaTr.GetComponent<MaskableGraphic>(), alphaValue);

        for (int i = 0; i < alphaTr.childCount; i++)
        {
            SetAlphaToChild(alphaTr.GetChild(i), alphaValue);
        }
    }

    private void SetAlphaToTr(MaskableGraphic GraphicElement, float alphaValue)
    {
        if (GraphicElement == null)
            return;

        Color objectColor = GraphicElement.color;
        objectColor.a = alphaValue;
        GraphicElement.color = objectColor;
    }

    private void SetAlphaToChild(Transform child, float alphaValue)
    {
        if ((!child.GetComponent<ReferencedFrom>() || tweenScript.Alpha.OverrideChilds) && child.gameObject.activeSelf)
        {
            SetAlphaValue(child, alphaValue);
        }
    }
}