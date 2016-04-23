using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ReferencedFrom))] 
public class ReferencedProxy : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();
        EditorGUILayout.HelpBox ("This object is referenced from a Tween", MessageType.Info);
    }
}

[CustomEditor(typeof(Tween))]
public class TweenEditor : Editor
{
    private Tween tweenScript;

    private void OnEnable()
    {
        tweenScript = (Tween)target;
    }

    public override void OnInspectorGUI()
    {
        if (tweenScript == null)
        {
            EditorGUILayout.HelpBox("Your script is null, internal crash...", MessageType.Error);
            return;
        }

        if (tweenScript.AnimatedRect == null)
        {
            EditorGUILayout.HelpBox("Please Drag and Drop your UI GameObject in the area below.", MessageType.Info);
            RenderDropArea("Drop UI Object here to assign");
        }
        else
            PopulateEditor();
    }

    private void PopulateEditor()
    {
        if (tweenScript.CoreProperty == null)
            return;

        if (!tweenScript.AnimatedRect.gameObject.GetComponent<ReferencedFrom> ())
        {
            tweenScript.AnimatedRect.gameObject.AddComponent<ReferencedFrom> ();
        }
        
        RenderAssignmentField();
        RenderAnimationProperty();
        RenderAnimationEndState("GameObject end animation action");
        RenderAlphaExecutor(tweenScript.Alpha, "Start alpha value", "End alpha value", "Override child alpha? ", "Enable Alpha executor");
        RenderExecutorEditor(tweenScript.Position, "Start Position", "End Position", "Enable Position Executor");
        RenderExecutorEditor(tweenScript.Rotation, "Start Rotation", "End Rotation", "Enable Rotation Executor");
        RenderExecutorEditor(tweenScript.Scale, "Start Scale", "End Scale", "Enable Scale Executor");
        RenderAnimationButtons();
        RenderCallbackEventProperty();
    }

    private void RenderAssignmentField()
    {
        string labelMessage = string.Format("UI Tranform path: {0}", GetGameObjectPath(tweenScript.AnimatedRect.transform));
        string buttonMessage = string.Format("Go to object in the scene");
        string areaMessage = string.Format("Drop UI Object here to re-assign");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(labelMessage);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        Rect buttonArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonArea, buttonMessage))
        {
            Selection.activeTransform = tweenScript.AnimatedRect.transform;
        }
        RenderDropArea(areaMessage);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void RenderAnimationProperty()
    {
        RenderSeparator();
        string stateMesssage = string.Format("Current State of the animation: ");
        string lockMessage = string.Format("Lock animation to current state? ");
        string unscaledMessage = string.Format("Time scale indipendent animation? ");
        string lenghtMessage = string.Format("Animation Lenght (sec): ");

        RenderStateLabel(stateMesssage);
        RenderToggle(lockMessage, ref tweenScript.CoreProperty.LockState);
        RenderToggle(unscaledMessage, ref tweenScript.TimeProperty.UnscaledTime);
        tweenScript.TimeProperty.Duration = RenderFloatLabel(lenghtMessage, tweenScript.TimeProperty.Duration, 0f, 10f);
    }

    private void RenderAlphaExecutor(AlphaExecutor fadeExecutor, string initialValMessage, string finalValMessage, string overrideChild, string enableMessage)
    {
        RenderSeparator();

        if (fadeExecutor.isExecutorEnabled())
        {
            fadeExecutor.StartValue = EditorGUILayout.FloatField(initialValMessage, fadeExecutor.StartValue);
            fadeExecutor.EndValue = EditorGUILayout.FloatField(finalValMessage, fadeExecutor.EndValue);
            fadeExecutor.OverrideChilds = EditorGUILayout.Toggle(overrideChild, fadeExecutor.OverrideChilds);

            if (GUI.Button(GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true)), "Disable Executor"))
            {
                fadeExecutor.DisableExecutor();
            }
        }
        else if (GUI.Button(GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true)), enableMessage))
        {
            fadeExecutor.EnableExecutor();
        }
    }

    private void RenderExecutorEditor(SpatialExecutor executor, string startMessage, string endMessage, string enableMessage)
    {
        RenderSeparator();

        if (executor.isExecutorEnabled())
        {
            string easeinMessage = "EaseIn";
            string easeoutMessage = "EaseOut";
            
            RenderVector3(startMessage, ref executor.StartVector);
            RenderVector3(endMessage, ref executor.EndVector);
            executor.EaseIn = RenderAnimationCurve(easeinMessage, executor.EaseIn);
            executor.EaseOut = RenderAnimationCurve(easeoutMessage, executor.EaseOut);

            if (GUI.Button(GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true)), "Disable Executor"))
            {
                executor.DisableExecutor();
            }
        }
        else if (GUI.Button(GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true)), enableMessage))
        {
            executor.EnableExecutor();
        }
    }

    private void RenderAnimationButtons()
    {
        RenderSeparator();
        EditorGUILayout.BeginHorizontal();
        Rect buttonArea = GUILayoutUtility.GetRect(90.0f, 60.0f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonArea, "Start Animation"))
        {
            tweenScript.Animate();
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        buttonArea = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonArea, "Get initial properties"))
        {
            GetInitialProperties();
        }
        buttonArea = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonArea, "Get final properties"))
        {
            GetFinalProperties();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        buttonArea = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonArea, "Set to initial properties"))
        {
            SetInitialProperties();
        }
        buttonArea = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
        if (GUI.Button(buttonArea, "Set to final properties"))
        {
            SetFinalProperties();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void RenderCallbackEventProperty()
    {
        RenderSeparator();
        EditorGUILayout.LabelField("Percentage (0f-1f) when callback is fired during the animation");
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Left point (" + tweenScript.EventProperty.IntroEventPercentage.x + ") = \"StartIntro\" --- Right point (" + tweenScript.EventProperty.IntroEventPercentage.y + ") = \"EndIntro\"");
        Vector2 IntroEventPercentage = tweenScript.EventProperty.IntroEventPercentage;
        EditorGUILayout.MinMaxSlider(ref IntroEventPercentage.x, ref IntroEventPercentage.y, 0f, 1f);
        tweenScript.EventProperty.IntroEventPercentage = IntroEventPercentage;

        EditorGUILayout.LabelField("Left point (" + tweenScript.EventProperty.ExitEventPercentage.x + ") = \"StartExit\" --- Right point (" + tweenScript.EventProperty.ExitEventPercentage.y + ") = \"EndExit\"");
        Vector2 ExitEventPercentage = tweenScript.EventProperty.ExitEventPercentage;
        EditorGUILayout.MinMaxSlider(ref ExitEventPercentage.x, ref ExitEventPercentage.y, 0f, 1f);
        tweenScript.EventProperty.ExitEventPercentage = ExitEventPercentage;
    }

    private void RenderStateLabel(string stateMesssage)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(stateMesssage);
        Rect enumArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        tweenScript.CoreProperty.AnimationState = (CoreProperty.CoreState)EditorGUI.EnumPopup(enumArea, string.Empty, tweenScript.CoreProperty.AnimationState);
        EditorGUILayout.EndHorizontal();
    }

    private void RenderAnimationEndState(string stateMesssage)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(stateMesssage);
        Rect enumArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        tweenScript.CoreProperty.FinalAction = (CoreProperty.EndAction)EditorGUI.EnumPopup(enumArea, string.Empty, tweenScript.CoreProperty.FinalAction);
        EditorGUILayout.EndHorizontal();
    }

    private void RenderToggle(string lockMessage, ref bool value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lockMessage);
        Rect lockArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        value = EditorGUI.Toggle(lockArea, value);
        EditorGUILayout.EndHorizontal();
    }

    private float RenderFloatLabel(string lenghtMessage, float value, float minClamp, float maxClamp)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lenghtMessage);
        Rect lenghArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        float tempDuration = EditorGUI.FloatField(lenghArea, string.Empty, value);
        tempDuration = Mathf.Clamp(tempDuration, minClamp, maxClamp);
        EditorGUILayout.EndHorizontal();
        return tempDuration;
    }

    private void RenderVector3(string vectorMessage, ref Vector3 vector)
    {
        Rect vectorArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        vector = EditorGUI.Vector3Field(vectorArea, vectorMessage, vector);
    }

    private AnimationCurve RenderAnimationCurve(string curveMessage, AnimationCurve curve)
    {
        return EditorGUILayout.CurveField(curveMessage, curve);
    }

    private void RenderSeparator()
    {
        EditorGUILayout.Space();
        Rect separator = GUILayoutUtility.GetRect(0.0f, 5.0f, GUILayout.ExpandWidth(true));
        GUI.Box(separator, string.Empty);
        EditorGUILayout.Space();
    }

    private void RenderDropArea(string message)
    {
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, message);

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is GameObject)
                            tweenScript.AnimatedRect = (draggedObject as GameObject).GetComponent<RectTransform>();
                    }
                }
                break;
        }
    }

    private string GetGameObjectPath(Transform tr)
    {
        string path = "/" + tr.name;
        while (tr.parent != null)
        {
            tr = tr.parent;
            path = "/" + tr.name + path;
        }
        return path;
    }

    private void GetInitialProperties()
    {
        if (tweenScript.Scale.isExecutorEnabled())
            tweenScript.Scale.StartVector = tweenScript.AnimatedRect.localScale;
        
        if (tweenScript.Position.isExecutorEnabled())
            tweenScript.Position.StartVector = tweenScript.AnimatedRect.anchoredPosition;
        
        if (tweenScript.Rotation.isExecutorEnabled())
            tweenScript.Rotation.StartVector = tweenScript.AnimatedRect.localEulerAngles;
    }

    private void GetFinalProperties()
    {
        if (tweenScript.Scale.isExecutorEnabled())
            tweenScript.Scale.EndVector = tweenScript.AnimatedRect.localScale;
        
        if (tweenScript.Position.isExecutorEnabled())
            tweenScript.Position.EndVector = tweenScript.AnimatedRect.anchoredPosition;
        
        if (tweenScript.Rotation.isExecutorEnabled())
            tweenScript.Rotation.EndVector = tweenScript.AnimatedRect.localEulerAngles;
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
        if (!child.GetComponent<ReferencedFrom>() && child.gameObject.activeSelf)
        {
            SetAlphaValue(child, alphaValue);
        }
    }
}