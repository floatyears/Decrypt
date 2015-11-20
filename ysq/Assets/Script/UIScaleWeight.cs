using System;
using UnityEngine;

[AddComponentMenu("Game/GUI/UIScaleWeight")]
public class UIScaleWeight : MonoBehaviour
{
	private float oldScale = 1f;

	private UIRoot uiRoot;

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
		if (this.uiRoot == null)
		{
			if (GameUIManager.mInstance != null && GameUIManager.mInstance.uiRoot != null)
			{
				this.uiRoot = GameUIManager.mInstance.uiRoot;
			}
			else
			{
				GameObject root = NGUITools.GetRoot(base.gameObject);
				if (root != null)
				{
					this.uiRoot = root.GetComponent<UIRoot>();
				}
			}
		}
		if (this.uiRoot == null)
		{
			return;
		}
		base.transform.localScale /= this.oldScale;
		this.oldScale = 1f;
		int num = this.uiRoot.activeHeight * Screen.width / Screen.height;
		if (num < 1024)
		{
			float d = (float)num / 1024f;
			base.transform.localScale *= d;
			this.oldScale = d;
		}
	}
}
