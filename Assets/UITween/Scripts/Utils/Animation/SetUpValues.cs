using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetUpValues : MonoBehaviour {

    [SerializeField] private EasyTween TweenToSetUp;

    private enum TweenSetupStates
    {
        START_VALUES, 
        END_VALUES
    };
    [SerializeField] private TweenSetupStates CurrentState;

	void Awake () 
    {
        if (TweenToSetUp == null)
        {
            TweenToSetUp = GetComponent<EasyTween>();
        }

        if (TweenToSetUp == null)
        {
            Debug.LogError("Easy Tween not referenced");
            return;
        }

        if (!Application.isPlaying)
        {
            return;
        }

        if (CurrentState.Equals(TweenSetupStates.START_VALUES))
        {            
            SetStartValues();
            TweenToSetUp.rectTransform.gameObject.SetActive(false);
        }
        else
        {
            SetEndValues();
        }
	}

    void SetStartValues ()
    {
        RectTransform selectedTransform = TweenToSetUp.rectTransform;

        if (TweenToSetUp.animationParts.PositionPropetiesAnim.IsPositionEnabled ())
            selectedTransform.anchoredPosition = (Vector2)TweenToSetUp.animationParts.PositionPropetiesAnim.StartPos;

        if (TweenToSetUp.animationParts.ScalePropetiesAnim.IsScaleEnabled ())
            selectedTransform.localScale = TweenToSetUp.animationParts.ScalePropetiesAnim.StartScale; 

        if (TweenToSetUp.animationParts.RotationPropetiesAnim.IsRotationEnabled ())
            selectedTransform.localEulerAngles = TweenToSetUp.animationParts.RotationPropetiesAnim.StartRot;

        if (TweenToSetUp.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
            if (TweenToSetUp.animationParts.IsObjectOpened ())
                SetAlphaValue (selectedTransform.transform, TweenToSetUp.animationParts.FadePropetiesAnim.GetEndFadeValue());
            else
                SetAlphaValue (selectedTransform.transform, TweenToSetUp.animationParts.FadePropetiesAnim.GetStartFadeValue());
        }
    }

    void SetEndValues ()
    {
        RectTransform selectedTransform = TweenToSetUp.rectTransform;

        if (TweenToSetUp.animationParts.PositionPropetiesAnim.IsPositionEnabled ())
            selectedTransform.anchoredPosition = (Vector2)TweenToSetUp.animationParts.PositionPropetiesAnim.EndPos;

        if (TweenToSetUp.animationParts.ScalePropetiesAnim.IsScaleEnabled ())
            selectedTransform.localScale = TweenToSetUp.animationParts.ScalePropetiesAnim.EndScale; 

        if (TweenToSetUp.animationParts.RotationPropetiesAnim.IsRotationEnabled ())
            selectedTransform.localEulerAngles = TweenToSetUp.animationParts.RotationPropetiesAnim.EndRot;

        if (TweenToSetUp.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
            if (TweenToSetUp.IsObjectOpened ())
                SetAlphaValue (selectedTransform.transform, TweenToSetUp.animationParts.FadePropetiesAnim.GetStartFadeValue());
            else
                SetAlphaValue (selectedTransform.transform, TweenToSetUp.animationParts.FadePropetiesAnim.GetEndFadeValue());
        }
    }

    void SetAlphaValue (Transform _objectToSetAlpha, float alphaValue)
    {
        if (_objectToSetAlpha.GetComponent<MaskableGraphic> ()) {
            MaskableGraphic GraphicElement = _objectToSetAlpha.GetComponent<MaskableGraphic> ();

            Color objectColor = GraphicElement.color;

            objectColor.a = alphaValue;
            GraphicElement.color = objectColor;
        }

        if (_objectToSetAlpha.childCount > 0) {
            for (int i = 0; i < _objectToSetAlpha.childCount; i++) {
                if ((!_objectToSetAlpha.GetChild (i).GetComponent<ReferencedFrom> () || TweenToSetUp.animationParts.FadePropetiesAnim.IsFadeOverrideEnabled ()) && _objectToSetAlpha.gameObject.activeSelf) 
                {
                    SetAlphaValue (_objectToSetAlpha.GetChild (i), alphaValue);
                }
            }
        }
    }
}
