using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIPropertyTypeInfoPopUp : MonoBehaviour
{
	private static GUIPropertyTypeInfoPopUp mInstance;

	private Transform mWinBG;

	public static void ShowMe()
	{
		if (GUIPropertyTypeInfoPopUp.mInstance == null)
		{
			GUIPropertyTypeInfoPopUp.CreateInstance();
		}
		GUIPropertyTypeInfoPopUp.mInstance.Init();
	}

	private static void CreateInstance()
	{
		if (GUIPropertyTypeInfoPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIPropertyTypeInfoPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIPropertyTypeInfoPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIPropertyTypeInfoPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIPropertyTypeInfoPopUp.mInstance = gameObject2.AddComponent<GUIPropertyTypeInfoPopUp>();
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
		UnityEngine.Object.Destroy(GUIPropertyTypeInfoPopUp.mInstance.gameObject);
		GUIPropertyTypeInfoPopUp.mInstance = null;
	}
}
