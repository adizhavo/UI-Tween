using UnityEngine;

[System.Serializable]
public class TimeProperty
{
    #region Exposed to Editor
    [SerializeField] private float duration;
    public float Duration
    {
        set
        {  if (value >= 0)
            duration = value; }
        private get
        { return duration; }
    }

    public bool UnscaledTime = true;
    #endregion

    public float GetPercentageDuration(float percentage)
    {
        return percentage / duration;
    }
}