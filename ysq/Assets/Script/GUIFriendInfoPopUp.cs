using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUIFriendInfoPopUp : MonoBehaviour
{
	private static GUIFriendInfoPopUp mInstance;

	private FriendData mFriendData;

	private int friendType;

	private UILabel mName;

	private UILabel mCombatValue;

	private UILabel mGuildName;

	private UISprite mIcon;

	private UISprite mFrame;

	private GameObject mWindow;

	private GameObject mInfoBg;

	private GameObject mVip;

	private UISprite mVipSingle;

	private UISprite mVipTens;

	private UISprite mVipOne;

	private UILabel mBacklistLabel;

	private UILabel mFriendLabel;

	private GUIFriendScene mGUIFriendScene;

	public static void Show(FriendData data)
	{
		if (GUIFriendInfoPopUp.mInstance == null)
		{
			GUIFriendInfoPopUp.CreateInstance();
		}
		GUIFriendInfoPopUp.mInstance.Init(data);
	}

	private static void CreateInstance()
	{
		if (GUIFriendInfoPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIFriendInfoPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIFriendInfoPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIFriendInfoPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIFriendInfoPopUp.mInstance = gameObject2.AddComponent<GUIFriendInfoPopUp>();
	}

	public static bool TryClose()
	{
		if (GUIFriendInfoPopUp.mInstance != null)
		{
			GUIFriendInfoPopUp.mInstance.OnCloseClick(null);
			return true;
		}
		return false;
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		this.mInfoBg = GameUITools.FindGameObject("BG", this.mWindow);
		this.mName = GameUITools.FindUILabel("Name", this.mInfoBg);
		this.mCombatValue = GameUITools.FindUILabel("CombatValue/Value", this.mInfoBg);
		this.mGuildName = GameUITools.FindUILabel("Guild", this.mInfoBg);
		this.mIcon = GameUITools.FindUISprite("Icon", this.mInfoBg);
		this.mFrame = GameUITools.FindUISprite("Frame", this.mIcon.gameObject);
		this.mVip = GameUITools.FindGameObject("VIP", this.mIcon.gameObject);
		this.mVipTens = GameUITools.FindUISprite("Tens", this.mVip);
		this.mVipSingle = GameUITools.FindUISprite("Single", this.mVip);
		this.mVipOne = GameUITools.FindUISprite("One", this.mVip);
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnFadeBGClick), base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		GameUITools.RegisterClickEvent("Chat", new UIEventListener.VoidDelegate(this.OnChatClick), this.mWindow);
		GameUITools.RegisterClickEvent("View", new UIEventListener.VoidDelegate(this.OnViewClick), this.mWindow);
		GameObject gameObject = this.mWindow.transform.Find("friendBtn").gameObject;
		UIEventListener expr_191 = UIEventListener.Get(gameObject);
		expr_191.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_191.onClick, new UIEventListener.VoidDelegate(this.OnFriendClick));
		this.mFriendLabel = gameObject.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject2 = this.mWindow.transform.Find("backlistBtn").gameObject;
		UIEventListener expr_1EE = UIEventListener.Get(gameObject2);
		expr_1EE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1EE.onClick, new UIEventListener.VoidDelegate(this.OnBacklistClick));
		this.mBacklistLabel = gameObject2.transform.Find("Label").GetComponent<UILabel>();
	}

	public void Init(FriendData data)
	{
		if (data == null)
		{
			return;
		}
		this.mFriendData = data;
		this.friendType = data.FriendType;
		this.mName.text = Singleton<StringManager>.Instance.GetString("friend_4", new object[]
		{
			data.Level
		}) + "  " + data.Name;
		this.mName.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(data.ConLevel));
		if (data.CombatValue > 0)
		{
			this.mCombatValue.enabled = true;
			this.mCombatValue.text = data.CombatValue.ToString();
		}
		else
		{
			this.mCombatValue.enabled = false;
		}
		if (!string.IsNullOrEmpty(data.GuildName))
		{
			this.mGuildName.enabled = true;
			this.mGuildName.text = Singleton<StringManager>.Instance.GetString("friend_1", new object[]
			{
				data.GuildName
			});
		}
		else
		{
			this.mGuildName.enabled = true;
			this.mGuildName.text = Singleton<StringManager>.Instance.GetString("friend_3");
			this.mGuildName.color = Color.gray;
		}
		this.mIcon.spriteName = Tools.GetPlayerIcon(data.FashionID);
		this.mFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(data.ConLevel));
		int vipLevel = this.mFriendData.VipLevel;
		if (vipLevel > 0)
		{
			this.mVip.gameObject.SetActive(true);
			if (vipLevel >= 10)
			{
				this.mVipSingle.enabled = true;
				this.mVipTens.enabled = true;
				this.mVipOne.enabled = false;
				this.mVipSingle.spriteName = (vipLevel % 10).ToString();
				this.mVipTens.spriteName = (vipLevel / 10).ToString();
			}
			else
			{
				this.mVipSingle.enabled = false;
				this.mVipTens.enabled = false;
				this.mVipOne.enabled = true;
				this.mVipOne.spriteName = vipLevel.ToString();
			}
		}
		else
		{
			this.mVip.gameObject.SetActive(false);
		}
		this.mBacklistLabel.text = Singleton<StringManager>.Instance.GetString((data.FriendType != 2) ? "friend_25" : "friend_26");
		this.mFriendLabel.text = Singleton<StringManager>.Instance.GetString((data.FriendType != 1) ? "friend_27" : "friend_28");
		if (data != null && data.FriendType == 0)
		{
			this.mBacklistLabel.text = Singleton<StringManager>.Instance.GetString("friend_25");
			this.mFriendLabel.text = Singleton<StringManager>.Instance.GetString("friend_27");
		}
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.PlayCloseAnim();
	}

	private void OnChatClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mFriendData != null)
		{
			GameUIManager.mInstance.CreateSession<GUIChatWindowV2>(delegate(GUIChatWindowV2 sen)
			{
				sen.SwitchToPersonalLayer(this.mFriendData.GUID, this.mFriendData.Name);
			});
			this.CloseAll();
		}
	}

	private void CloseAll()
	{
		this.CloseImmediate();
	}

	private void OnViewClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
		mC2S_QueryRemotePlayer.PlayerID = this.mFriendData.GUID;
		Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
		this.CloseAll();
	}

	private void OnBacklistClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mFriendData == null)
		{
			return;
		}
		if (this.friendType == 2)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_24", new object[]
			{
				this.mFriendData.Name
			}), MessageBox.Type.OKCancel, this.mFriendData);
			GameMessageBox expr_5E = gameMessageBox;
			expr_5E.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_5E.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_RemoveBlackList mC2S_RemoveBlackList = new MC2S_RemoveBlackList();
				mC2S_RemoveBlackList.GUID = this.mFriendData.GUID;
				Globals.Instance.CliSession.Send(317, mC2S_RemoveBlackList);
			}));
			this.friendType = 0;
		}
		else if (this.mFriendData.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_32", 0f, 0f);
		}
		else
		{
			GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_23", new object[]
			{
				this.mFriendData.Name
			}), MessageBox.Type.OKCancel, this.mFriendData);
			GameMessageBox expr_FE = gameMessageBox2;
			expr_FE.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_FE.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_AddBlackList mC2S_AddBlackList = new MC2S_AddBlackList();
				mC2S_AddBlackList.GUID = this.mFriendData.GUID;
				Globals.Instance.CliSession.Send(315, mC2S_AddBlackList);
			}));
		}
		this.CloseAll();
	}

	public bool IsFriend()
	{
		bool result = false;
		for (int i = 0; i < Globals.Instance.Player.FriendSystem.friends.Count; i++)
		{
			FriendData friendData = Globals.Instance.Player.FriendSystem.friends[i];
			if (friendData.GUID != Globals.Instance.Player.Data.ID && this.mFriendData.GUID == friendData.GUID)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool IsBlackList()
	{
		bool result = false;
		for (int i = 0; i < Globals.Instance.Player.FriendSystem.blackList.Count; i++)
		{
			FriendData friendData = Globals.Instance.Player.FriendSystem.blackList[i];
			if (friendData.GUID != Globals.Instance.Player.Data.ID && this.mFriendData.GUID == friendData.GUID)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void OnFriendClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mFriendData == null)
		{
			return;
		}
		if (this.friendType == 1)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_22", new object[]
			{
				this.mFriendData.Name
			}), MessageBox.Type.OKCancel, this.mFriendData);
			GameMessageBox expr_5E = gameMessageBox;
			expr_5E.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_5E.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				if (Globals.Instance.Player.FriendSystem.IsFriend(this.mFriendData.GUID))
				{
					MC2S_RemoveFriend mC2S_RemoveFriend = new MC2S_RemoveFriend();
					mC2S_RemoveFriend.GUID = this.mFriendData.GUID;
					Globals.Instance.CliSession.Send(313, mC2S_RemoveFriend);
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("friend_33", 0f, 0f);
				}
			}));
			this.friendType = 0;
		}
		else if (this.mFriendData.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
		}
		else
		{
			MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
			mC2S_RequestFriend.GUID = this.mFriendData.GUID;
			Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
		}
		this.CloseAll();
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIFriendInfoPopUp.mInstance.gameObject);
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
