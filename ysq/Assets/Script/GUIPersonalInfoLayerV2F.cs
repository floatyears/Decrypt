using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIPersonalInfoLayerV2F : MonoBehaviour
{
	private GUIChatWindowV2F mBaseLayer;

	private UILabel mNameLabel;

	private UILabel mGuildNameLabel;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mBacklistLabel;

	private UILabel mFriendLabel;

	private ChatMessage mChatInfo;

	private StringBuilder mSb = new StringBuilder(42);

	private int friendType;

	public void InitWithBaseScene(GUIChatWindowV2F baseLayer)
	{
		this.mBaseLayer = baseLayer;
		this.CreateObjects();
	}

	public void SetPersonalInfo(ChatMessage chatInfo)
	{
		this.mChatInfo = chatInfo;
		if (this.mChatInfo != null)
		{
			this.mSb.Remove(0, this.mSb.Length).Append("Lv").Append(chatInfo.Level).Append(" ").Append(chatInfo.Name);
			this.mNameLabel.text = this.mSb.ToString();
			if (string.IsNullOrEmpty(chatInfo.GuildName))
			{
				this.mGuildNameLabel.text = Singleton<StringManager>.Instance.GetString("chatTxt10");
			}
			else
			{
				this.mGuildNameLabel.text = chatInfo.GuildName;
			}
			this.mIcon.spriteName = Tools.GetPlayerIcon(this.mChatInfo.FashionID);
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mChatInfo.ConstellationLevel));
			FriendData friendData = Globals.Instance.Player.FriendSystem.GetFriendData(this.mChatInfo.PlayerID);
			if (friendData != null)
			{
				this.friendType = friendData.FriendType;
			}
			bool flag = friendData != null && friendData.FriendType == 1;
			bool flag2 = friendData != null && friendData.FriendType == 2;
			this.mBacklistLabel.text = Singleton<StringManager>.Instance.GetString((!flag2) ? "friend_25" : "friend_26");
			this.mFriendLabel.text = Singleton<StringManager>.Instance.GetString((!flag) ? "friend_27" : "friend_28");
		}
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("personInfoLayer");
		this.mNameLabel = transform.Find("name").GetComponent<UILabel>();
		this.mGuildNameLabel = transform.Find("guildName").GetComponent<UILabel>();
		Transform transform2 = transform.Find("bg");
		this.mIcon = transform2.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = transform2.Find("Frame").GetComponent<UISprite>();
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_8C = UIEventListener.Get(gameObject);
		expr_8C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8C.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject2 = transform.Find("chatBtn").gameObject;
		UIEventListener expr_C4 = UIEventListener.Get(gameObject2);
		expr_C4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C4.onClick, new UIEventListener.VoidDelegate(this.OnChatClick));
		GameObject gameObject3 = transform.Find("viewBtn").gameObject;
		UIEventListener expr_FE = UIEventListener.Get(gameObject3);
		expr_FE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_FE.onClick, new UIEventListener.VoidDelegate(this.OnViewClick));
		GameObject gameObject4 = transform.Find("friendBtn").gameObject;
		UIEventListener expr_138 = UIEventListener.Get(gameObject4);
		expr_138.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_138.onClick, new UIEventListener.VoidDelegate(this.OnFriendClick));
		this.mFriendLabel = gameObject4.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject5 = transform.Find("backlistBtn").gameObject;
		UIEventListener expr_18E = UIEventListener.Get(gameObject5);
		expr_18E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18E.onClick, new UIEventListener.VoidDelegate(this.OnBacklistClick));
		this.mBacklistLabel = gameObject5.transform.Find("Label").GetComponent<UILabel>();
	}

	private void OnFriendClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mChatInfo == null)
		{
			return;
		}
		if (this.friendType == 1)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_22", new object[]
			{
				this.mChatInfo.Name
			}), MessageBox.Type.OKCancel, this.mChatInfo);
			GameMessageBox expr_5E = gameMessageBox;
			expr_5E.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_5E.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				if (Globals.Instance.Player.FriendSystem.IsFriend(this.mChatInfo.PlayerID))
				{
					MC2S_RemoveFriend mC2S_RemoveFriend = new MC2S_RemoveFriend();
					mC2S_RemoveFriend.GUID = this.mChatInfo.PlayerID;
					Globals.Instance.CliSession.Send(313, mC2S_RemoveFriend);
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("friend_33", 0f, 0f);
				}
			}));
			this.friendType = 0;
		}
		else if (this.mChatInfo.PlayerID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
		}
		else
		{
			MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
			mC2S_RequestFriend.GUID = this.mChatInfo.PlayerID;
			Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
		}
		this.EnablePersonalInfoLayer(false);
	}

	private void OnBacklistClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mChatInfo == null)
		{
			return;
		}
		if (this.friendType == 2)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_24", new object[]
			{
				this.mChatInfo.Name
			}), MessageBox.Type.OKCancel, this.mChatInfo);
			GameMessageBox expr_5E = gameMessageBox;
			expr_5E.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_5E.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_RemoveBlackList mC2S_RemoveBlackList = new MC2S_RemoveBlackList();
				mC2S_RemoveBlackList.GUID = this.mChatInfo.PlayerID;
				Globals.Instance.CliSession.Send(317, mC2S_RemoveBlackList);
			}));
			this.friendType = 0;
		}
		else if (this.mChatInfo.PlayerID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_32", 0f, 0f);
		}
		else
		{
			GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_23", new object[]
			{
				this.mChatInfo.Name
			}), MessageBox.Type.OKCancel, this.mChatInfo);
			GameMessageBox expr_FE = gameMessageBox2;
			expr_FE.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_FE.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_AddBlackList mC2S_AddBlackList = new MC2S_AddBlackList();
				mC2S_AddBlackList.GUID = this.mChatInfo.PlayerID;
				Globals.Instance.CliSession.Send(315, mC2S_AddBlackList);
			}));
		}
		this.EnablePersonalInfoLayer(false);
	}

	private void OnChatClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mChatInfo != null)
		{
			this.mBaseLayer.SwitchToPersonalLayer(this.mChatInfo.PlayerID, this.mChatInfo.Name);
			this.EnablePersonalInfoLayer(false);
		}
	}

	private void OnViewClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.SenceMgr.sceneInfo != null)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt26", 0f, 0f);
			return;
		}
		if (this.mChatInfo != null)
		{
			MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
			mC2S_QueryRemotePlayer.PlayerID = this.mChatInfo.PlayerID;
			Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
			this.mBaseLayer.Close();
		}
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.EnablePersonalInfoLayer(false);
	}

	public void EnablePersonalInfoLayer(bool isEnable)
	{
		base.gameObject.SetActive(isEnable);
	}
}
