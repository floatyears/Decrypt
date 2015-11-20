using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUIAddFriendPopUp : MonoBehaviour
{
	private static GUIAddFriendPopUp mInstance;

	private UILabel mTitle;

	private GameObject mWindow;

	private GameObject mInputArea;

	private UIInput mUIInputMsg;

	private UILabel mInputTxt;

	public static void Show()
	{
		if (GUIAddFriendPopUp.mInstance == null)
		{
			GUIAddFriendPopUp.CreateInstance();
		}
		GUIAddFriendPopUp.mInstance.Init();
	}

	private static void CreateInstance()
	{
		if (GUIAddFriendPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIAddFriendPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIAddFriendPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIAddFriendPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIAddFriendPopUp.mInstance = gameObject2.AddComponent<GUIAddFriendPopUp>();
		GUIAddFriendPopUp.mInstance.gameObject.SetActive(true);
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
		this.mUIInputMsg.defaultText = Singleton<StringManager>.Instance.GetString("friend_16");
		this.mUIInputMsg.characterLimit = 12;
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnFadeBGClick), base.gameObject);
		GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelClick), this.mWindow);
		GameUITools.RegisterClickEvent("Ok", new UIEventListener.VoidDelegate(this.OnOkClick), this.mWindow);
	}

	public void Init()
	{
		this.mTitle.text = Singleton<StringManager>.Instance.GetString("friend_17");
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnOkClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.OnSubmitMsg();
	}

	public bool IsFriend(ulong id = 0uL, string name = null)
	{
		bool result = false;
		for (int i = 0; i < Globals.Instance.Player.FriendSystem.friends.Count; i++)
		{
			FriendData friendData = Globals.Instance.Player.FriendSystem.friends[i];
			if (name == friendData.Name || id == friendData.GUID)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void OnSubmitMsg()
	{
		if (string.IsNullOrEmpty(this.mUIInputMsg.value.Trim()))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_20", 0f, 0f);
			return;
		}
		if (Tools.GetLength(this.mUIInputMsg.value) > 12)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_21", 0f, 0f);
			return;
		}
		ulong num = 0uL;
		if (ulong.TryParse(this.mUIInputMsg.value, out num))
		{
			if (num == Globals.Instance.Player.Data.AccountID)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
			}
			else if (this.IsFriend(num, null))
			{
				GameUIManager.mInstance.ShowMessageTipByKey("friend_35", 0f, 0f);
			}
			else
			{
				MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
				mC2S_RequestFriend.AID = (int)num;
				Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
			}
		}
		else if (string.Compare(this.mUIInputMsg.value.Trim(), Globals.Instance.Player.Data.Name, true) == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
		}
		else if (this.IsFriend(0uL, this.mUIInputMsg.value.Trim()))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_35", 0f, 0f);
		}
		else
		{
			MC2S_RequestFriend mC2S_RequestFriend2 = new MC2S_RequestFriend();
			mC2S_RequestFriend2.GUID = 0uL;
			mC2S_RequestFriend2.Name = this.mUIInputMsg.value.Trim();
			Globals.Instance.CliSession.Send(309, mC2S_RequestFriend2);
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
		UnityEngine.Object.Destroy(GUIAddFriendPopUp.mInstance.gameObject);
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
