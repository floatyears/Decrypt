using System;
using UnityEngine;

public class UISizeScale : MonoBehaviour
{
	private float oldScale = 1f;

	private void Awake()
	{
		this.AdjustSize();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	private void ScreenSizeChanged()
	{
		this.AdjustSize();
	}

	private void AdjustSize()
	{
		float d = (!(GameUIManager.mInstance != null)) ? 1f : ((float)GameUIManager.mInstance.uiRoot.activeHeight / 640f);
		base.transform.localScale /= this.oldScale;
		base.transform.localScale *= d;
		this.oldScale = d;
	}
}
