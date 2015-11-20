using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIPassCombatPopUp : MonoBehaviour
{
	public delegate void VoidCallback();

	private static GUIPassCombatPopUp mInstance;

	private GameObject mWindow;

	private GUIPassCombatPopUp.VoidCallback OKEvent;

	private GUIPassCombatPopUp.VoidCallback CancelEvent;

	public static void Show(GUIPassCombatPopUp.VoidCallback OKEvent, GUIPassCombatPopUp.VoidCallback CancelEvent)
	{
		if (GUIPassCombatPopUp.mInstance == null)
		{
			GUIPassCombatPopUp.CreateInstance();
		}
		GUIPassCombatPopUp.mInstance.Init(OKEvent, CancelEvent);
	}

	private static void CreateInstance()
	{
		if (GUIPassCombatPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIPassCombatPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIPassCombatPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIPassCombatPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 2500f);
		GUIPassCombatPopUp.mInstance = gameObject2.AddComponent<GUIPassCombatPopUp>();
	}

	public static bool TryClose()
	{
		if (GUIPassCombatPopUp.mInstance != null)
		{
			GUIPassCombatPopUp.mInstance.OnCloseClick(null);
			return true;
		}
		return false;
	}

	public void Init(GUIPassCombatPopUp.VoidCallback cb, GUIPassCombatPopUp.VoidCallback cb2)
	{
		this.OKEvent = cb;
		this.CancelEvent = cb2;
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOKClick), this.mWindow);
		GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelClick), this.mWindow);
		base.gameObject.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnCancelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.CancelEvent != null)
		{
			this.CancelEvent();
		}
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.OKEvent != null)
		{
			this.OKEvent();
		}
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIPassCombatPopUp.mInstance.gameObject);
		GUIPassCombatPopUp.mInstance = null;
	}
}
