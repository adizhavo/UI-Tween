using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class AnimationSetupInitializer : MonoBehaviour
{
    private enum TweenSetupStates
    {
        START_VALUES,
        END_VALUES
    };

    [SerializeField] private TweenSetupStates CurrentState;
    private EasyTween TweenToSetup;

    private void Awake()
    {
        if (!IsAnimationSetted() || !Application.isPlaying)
            return;        

        SetupAnimation();
    }

    private bool IsAnimationSetted()
    {
        if (TweenToSetup == null)
        {
            TweenToSetup = GetComponent<EasyTween>();
            if (TweenToSetup == null)
            {
                Debug.LogError("Easy Tween not referenced");
                return false;
            }
        }
        return true;
    }

    private void SetupAnimation()
    {
        bool setToStart = CurrentState.Equals(TweenSetupStates.START_VALUES);
        AnimationSetupApplier setup = new AnimationSetupApplier(TweenToSetup.rectTransform, TweenToSetup.animationParts, setToStart);
        setup.Apply();
        TweenToSetup.rectTransform.gameObject.SetActive(!setToStart);
    }
}

public class AnimationSetupApplier
{
    private RectTransform rectTr;
    private Vector3 AppliedScale;
    private Vector3 AppliedPosition;
    private Vector3 AppliedRotation;
    private UITween.AnimationParts animationPart;

    public AnimationSetupApplier(RectTransform rectTr, UITween.AnimationParts animationPart, bool setStartValues)
    {
        this.rectTr = rectTr;
        this.animationPart = animationPart;

        AppliedScale = (setStartValues) ? animationPart.ScalePropetiesAnim.StartScale : animationPart.ScalePropetiesAnim.EndScale;
        AppliedPosition = (setStartValues) ? animationPart.PositionPropetiesAnim.StartPos : animationPart.PositionPropetiesAnim.EndPos;
        AppliedRotation = (setStartValues) ? animationPart.RotationPropetiesAnim.StartRot : animationPart.RotationPropetiesAnim.EndRot;
    }

    public void Apply()
    {
        ApplyPosition();
        ApplyScale();
        ApplyRotaion();
        ApplyFade();
    }

    private void ApplyPosition()
    {
        if (animationPart.PositionPropetiesAnim.IsPositionEnabled())
        {
            rectTr.anchoredPosition = (Vector2)AppliedPosition;
        }
    }

    private void ApplyScale()
    {
        if (animationPart.ScalePropetiesAnim.IsScaleEnabled())
        {
            rectTr.localScale = AppliedScale;
        }
    }

    private void ApplyRotaion()
    {
        if (animationPart.RotationPropetiesAnim.IsRotationEnabled())
        {
            rectTr.localEulerAngles = AppliedRotation;
        }
    }

    private void ApplyFade()
    {
        if (animationPart.FadePropetiesAnim.IsFadeEnabled())
        {
            if (animationPart.IsObjectOpened())
                SetAlphaValue(rectTr.transform, animationPart.FadePropetiesAnim.GetStartFadeValue());
            else
                SetAlphaValue(rectTr.transform, animationPart.FadePropetiesAnim.GetEndFadeValue());
        }
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
        if ((!child.GetComponent<ReferencedFrom>() || animationPart.FadePropetiesAnim.IsFadeOverrideEnabled()) && child.gameObject.activeSelf)
        {
            SetAlphaValue(child, alphaValue);
        }
    }
}