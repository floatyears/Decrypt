using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUISettingMessagePopUp : MonoBehaviour
{
	private const int WORSHIP_PEOPLE = 3;

	private static GUISettingMessagePopUp mInstance;

	private UILabel mTitle;

	private GameObject mWindow;

	private GameObject mInputArea;

	private UIInput mUIInputMsg;

	private UILabel mInputTxt;

	private GUIWorship mGUIWorship;

	public static void Show()
	{
		if (GUISettingMessagePopUp.mInstance == null)
		{
			GUISettingMessagePopUp.CreateInstance();
		}
		GUISettingMessagePopUp.mInstance.Init();
	}

	private static void CreateInstance()
	{
		if (GUISettingMessagePopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUISettingMessagePopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUISettingMessagePopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUISettingMessagePopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUISettingMessagePopUp.mInstance = gameObject2.AddComponent<GUISettingMessagePopUp>();
		GUISettingMessagePopUp.mInstance.gameObject.SetActive(true);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		this.mTitle = this.mWindow.transform.Find("Title").GetComponent<UILabel>();
		this.mInputArea = this.mWindow.transform.Find("ChatInputArea").gameObject;
		this.mUIInputMsg = this.mInputArea.transform.Find("ChatInput").GetComponent<UIInput>();
		EventDelegate.Add(this.mUIInputMsg.onSubmit, new EventDelegate.Callback(this.OnSubmitMsg));
		this.mUIInputMsg.defaultText = Singleton<StringManager>.Instance.GetString("worship_pop", new object[]
		{
			30
		});
		if (this.mGUIWorship == null)
		{
			this.mGUIWorship = GameUIManager.mInstance.GetSession<GUIWorship>();
		}
		for (int i = 0; i < this.mGUIWorship.mFamePlayerInfo.Count; i++)
		{
			if (this.mGUIWorship.mFamePlayerInfo[i].GUID == Globals.Instance.Player.Data.ID)
			{
				if (!string.IsNullOrEmpty(this.mGUIWorship.MessageTxt[i].text))
				{
					this.mUIInputMsg.defaultText = this.mGUIWorship.MessageTxt[i].text;
				}
				else
				{
					this.mUIInputMsg.defaultText = Singleton<StringManager>.Instance.GetString("worship_pop", new object[]
					{
						30
					});
				}
			}
		}
		this.mUIInputMsg.characterLimit = 30;
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnFadeBGClick), base.gameObject);
		GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelClick), this.mWindow);
		GameUITools.RegisterClickEvent("Ok", new UIEventListener.VoidDelegate(this.OnOkClick), this.mWindow);
	}

	public void Init()
	{
		this.mTitle.text = Singleton<StringManager>.Instance.GetString("worshiptxt3");
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnOkClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.OnSubmitMsg();
	}

	private void OnSubmitMsg()
	{
		if (!string.IsNullOrEmpty(this.mUIInputMsg.value))
		{
			MC2S_SetFameMessage mC2S_SetFameMessage = new MC2S_SetFameMessage();
			mC2S_SetFameMessage.Message = this.mUIInputMsg.value;
			GameUIManager.mInstance.uiState.mShowMsg = this.mUIInputMsg.value;
			Globals.Instance.CliSession.Send(302, mC2S_SetFameMessage);
		}
		this.mUIInputMsg.value = string.Empty;
		this.CloseAll();
	}

	private void OnCancelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.CloseAll();
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.PlayCloseAnim();
	}

	private void CloseAll()
	{
		this.CloseImmediate();
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUISettingMessagePopUp.mInstance.gameObject);
	}

	private void PlayCloseAnim()
	{
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void OnFadeBGClick(GameObject go)
	{
		this.PlayCloseAnim();
	}
}
