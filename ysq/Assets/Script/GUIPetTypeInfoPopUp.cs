using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIPetTypeInfoPopUp : GameUIBasePopup
{
	private static GUIPetTypeInfoPopUp mInstance;

	private Transform mWinBG;

	public static void ShowMe()
	{
		if (GUIPetTypeInfoPopUp.mInstance == null)
		{
			GUIPetTypeInfoPopUp.CreateInstance();
		}
		GUIPetTypeInfoPopUp.mInstance.Init();
	}

	private static void CreateInstance()
	{
		if (GUIPetTypeInfoPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIPetTypeInfoPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIPetTypeInfoPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIPetTypeInfoPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIPetTypeInfoPopUp.mInstance = gameObject2.AddComponent<GUIPetTypeInfoPopUp>();
	}

	private void Awake()
	{
		this.mWinBG = GameUITools.FindGameObject("winBG", base.gameObject).transform;
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
	}

	public void Init()
	{
		GameUITools.PlayOpenWindowAnim(this.mWinBG.transform, null, true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWinBG.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIPetTypeInfoPopUp.mInstance.gameObject);
		GUIPetTypeInfoPopUp.mInstance = null;
	}
}
