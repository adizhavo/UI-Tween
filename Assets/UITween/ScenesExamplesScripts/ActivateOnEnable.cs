using UnityEngine;
using System.Collections;

public class ActivateOnEnable : MonoBehaviour {

	public EasyTween EasyTweenStart;

    private void OnEnable () 
	{
		EasyTweenStart.OpenCloseObjectAnimation();
	}
}
