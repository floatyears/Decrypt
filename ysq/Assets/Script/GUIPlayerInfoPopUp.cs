using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUIPlayerInfoPopUp : MonoBehaviour
{
	private static GUIPlayerInfoPopUp mInstance;

	private RankData mRankData;

	private int friendType;

	private UILabel mName;

	private UILabel mCombatValue;

	private UILabel mGuildName;

	private UISprite mIcon;

	private UISprite mFrame;

	private GameObject mWindow;

	private GameObject mVip;

	private UISprite mVipSingle;

	private UISprite mVipTens;

	private UISprite mVipOne;

	private UILabel mBacklistLabel;

	private UILabel mFriendLabel;

	public static void Show(RankData data)
	{
		if (data == null)
		{
			global::Debug.LogError(new object[]
			{
				"RankData is null"
			});
			return;
		}
		if (GUIPlayerInfoPopUp.mInstance == null)
		{
			GUIPlayerInfoPopUp.CreateInstance();
		}
		GUIPlayerInfoPopUp.mInstance.Init(data);
	}

	private static void CreateInstance()
	{
		if (GUIPlayerInfoPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIPlayerInfoPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIPlayerInfoPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIPlayerInfoPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIPlayerInfoPopUp.mInstance = gameObject2.AddComponent<GUIPlayerInfoPopUp>();
	}

	public static bool TryClose()
	{
		if (GUIPlayerInfoPopUp.mInstance != null)
		{
			GUIPlayerInfoPopUp.mInstance.OnCloseClick(null);
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
		this.mName = GameUITools.FindUILabel("Name", this.mWindow);
		this.mCombatValue = GameUITools.FindUILabel("CombatValue/Value", this.mWindow);
		this.mGuildName = GameUITools.FindUILabel("Guild/Value", this.mWindow);
		this.mIcon = GameUITools.FindUISprite("Icon", this.mWindow);
		this.mFrame = GameUITools.FindUISprite("Frame", this.mIcon.gameObject);
		this.mVip = GameUITools.FindGameObject("VIP", this.mIcon.gameObject);
		this.mVipTens = GameUITools.FindUISprite("Tens", this.mVip);
		this.mVipSingle = GameUITools.FindUISprite("Single", this.mVip);
		this.mVipOne = GameUITools.FindUISprite("One", this.mVip);
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		GameUITools.RegisterClickEvent("Chat", new UIEventListener.VoidDelegate(this.OnChatClick), this.mWindow);
		GameUITools.RegisterClickEvent("View", new UIEventListener.VoidDelegate(this.OnViewClick), this.mWindow);
		GameObject gameObject = this.mWindow.transform.Find("friendBtn").gameObject;
		UIEventListener expr_17B = UIEventListener.Get(gameObject);
		expr_17B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_17B.onClick, new UIEventListener.VoidDelegate(this.OnFriendClick));
		this.mFriendLabel = gameObject.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject2 = this.mWindow.transform.Find("backlistBtn").gameObject;
		UIEventListener expr_1D8 = UIEventListener.Get(gameObject2);
		expr_1D8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1D8.onClick, new UIEventListener.VoidDelegate(this.OnBacklistClick));
		this.mBacklistLabel = gameObject2.transform.Find("Label").GetComponent<UILabel>();
	}

	public void Init(RankData data)
	{
		if (data == null)
		{
			return;
		}
		this.mRankData = data;
		this.mName.text = Singleton<StringManager>.Instance.GetString("equipImprove36", new object[]
		{
			this.mRankData.Data.Level
		}) + "  " + this.mRankData.Data.Name;
		this.mName.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(this.mRankData.Data.ConstellationLevel));
		this.mCombatValue.text = this.mRankData.Data.CombatValue.ToString();
		if (string.IsNullOrEmpty(this.mRankData.Data.GuildName))
		{
			this.mGuildName.text = Singleton<StringManager>.Instance.GetString("chatTxt10");
			this.mGuildName.color = Color.gray;
		}
		else
		{
			this.mGuildName.text = this.mRankData.Data.GuildName;
		}
		this.mIcon.spriteName = Tools.GetPlayerIcon(this.mRankData.Data.FashionID);
		this.mFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mRankData.Data.ConstellationLevel));
		int vipLevel = this.mRankData.Data.VipLevel;
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
		FriendData friendData = Globals.Instance.Player.FriendSystem.GetFriendData(data.Data.GUID);
		if (friendData != null)
		{
			this.friendType = friendData.FriendType;
		}
		bool flag = friendData != null && friendData.FriendType == 1;
		bool flag2 = friendData != null && friendData.FriendType == 2;
		this.mBacklistLabel.text = Singleton<StringManager>.Instance.GetString((!flag2) ? "friend_25" : "friend_26");
		this.mFriendLabel.text = Singleton<StringManager>.Instance.GetString((!flag) ? "friend_27" : "friend_28");
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.PlayCloseAnim();
	}

	private void OnFriendClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mRankData == null)
		{
			return;
		}
		if (this.mRankData.Data.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("PlayerR_119"), 0f, 0f);
		}
		else if (this.friendType == 1)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_22", new object[]
			{
				this.mRankData.Data.Name
			}), MessageBox.Type.OKCancel, this.mRankData.Data);
			GameMessageBox expr_B9 = gameMessageBox;
			expr_B9.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_B9.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				if (Globals.Instance.Player.FriendSystem.IsFriend(this.mRankData.Data.GUID))
				{
					MC2S_RemoveFriend mC2S_RemoveFriend = new MC2S_RemoveFriend();
					mC2S_RemoveFriend.GUID = this.mRankData.Data.GUID;
					Globals.Instance.CliSession.Send(313, mC2S_RemoveFriend);
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("friend_33", 0f, 0f);
				}
			}));
			this.friendType = 0;
		}
		else if (this.mRankData.Data.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
		}
		else
		{
			MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
			mC2S_RequestFriend.GUID = this.mRankData.Data.GUID;
			Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
		}
		this.CloseAll();
	}

	private void OnBacklistClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mRankData.Data == null)
		{
			return;
		}
		if (this.mRankData.Data.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("PlayerR_118"), 0f, 0f);
		}
		else if (this.friendType == 2)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_24", new object[]
			{
				this.mRankData.Data.Name
			}), MessageBox.Type.OKCancel, this.mRankData.Data);
			GameMessageBox expr_BE = gameMessageBox;
			expr_BE.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_BE.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_RemoveBlackList mC2S_RemoveBlackList = new MC2S_RemoveBlackList();
				mC2S_RemoveBlackList.GUID = this.mRankData.Data.GUID;
				Globals.Instance.CliSession.Send(317, mC2S_RemoveBlackList);
			}));
			this.friendType = 0;
		}
		else if (this.mRankData.Data.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_32", 0f, 0f);
		}
		else
		{
			GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_23", new object[]
			{
				this.mRankData.Data.Name
			}), MessageBox.Type.OKCancel, this.mRankData.Data);
			GameMessageBox expr_16D = gameMessageBox2;
			expr_16D.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_16D.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_AddBlackList mC2S_AddBlackList = new MC2S_AddBlackList();
				mC2S_AddBlackList.GUID = this.mRankData.Data.GUID;
				Globals.Instance.CliSession.Send(315, mC2S_AddBlackList);
			}));
		}
		this.CloseAll();
	}

	private void OnChatClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mRankData != null)
		{
			GameUIManager.mInstance.CreateSession<GUIChatWindowV2>(delegate(GUIChatWindowV2 sen)
			{
				sen.SwitchToPersonalLayer(this.mRankData.Data.GUID, this.mRankData.Data.Name);
			});
			this.CloseAll();
		}
	}

	private void CloseAll()
	{
		GUIGuildMinesResultPopUp.TryClose();
		this.CloseImmediate();
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
	}

	private void OnViewClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
		mC2S_QueryRemotePlayer.PlayerID = this.mRankData.Data.GUID;
		Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
		this.CloseAll();
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIPlayerInfoPopUp.mInstance.gameObject);
	}

	private void PlayCloseAnim()
	{
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}
}
