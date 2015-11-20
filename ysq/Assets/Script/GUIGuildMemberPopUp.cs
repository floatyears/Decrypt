using Att;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIGuildMemberPopUp : GameUIBasePopup
{
	private UISprite mMemberIcon;

	private UISprite mQualityMask;

	private UILabel mLvName;

	private GameObject mJobForMaster;

	private GameObject mJobForOfficial;

	private UILabel mJobForMasterBtnTxt0;

	private UILabel mJobForMasterBtnTxt1;

	private UILabel mJobForMasterBtnTxt2;

	private UILabel mJobForOfficialBtnTxt;

	private GameObject mChangeBtn;

	private GameObject mKickOutBtn;

	private GameObject mChatBtn;

	private GameObject mViewTeamBtn;

	private GameObject mFriendsBtn;

	private GameObject mBlackBtn;

	private UILabel mFriendLb;

	private UILabel mBlackLb;

	private GuildMember mGuildMember;

	private int friendType;

	private StringBuilder mStringBuilder = new StringBuilder();

	public override void InitPopUp(GuildMember guildMember)
	{
		this.mGuildMember = guildMember;
		this.Refresh();
		if (this.mGuildMember != null)
		{
			FriendData friendData = Globals.Instance.Player.FriendSystem.GetFriendData(this.mGuildMember.ID);
			if (friendData != null)
			{
				this.friendType = friendData.FriendType;
			}
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mLvName = transform.Find("lvName").GetComponent<UILabel>();
		this.mJobForMaster = transform.Find("jobMasterBg").gameObject;
		GameObject gameObject2 = this.mJobForMaster.transform.Find("Btn0").gameObject;
		UIEventListener expr_96 = UIEventListener.Get(gameObject2);
		expr_96.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_96.onClick, new UIEventListener.VoidDelegate(this.OnJobForMasterBtn0));
		this.mJobForMasterBtnTxt0 = gameObject2.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject3 = this.mJobForMaster.transform.Find("Btn1").gameObject;
		UIEventListener expr_F3 = UIEventListener.Get(gameObject3);
		expr_F3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F3.onClick, new UIEventListener.VoidDelegate(this.OnJobForMasterBtn1));
		this.mJobForMasterBtnTxt1 = gameObject3.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject4 = this.mJobForMaster.transform.Find("Btn2").gameObject;
		UIEventListener expr_152 = UIEventListener.Get(gameObject4);
		expr_152.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_152.onClick, new UIEventListener.VoidDelegate(this.OnJobForMasterBtn2));
		this.mJobForMasterBtnTxt2 = gameObject4.transform.Find("Label").GetComponent<UILabel>();
		this.mJobForOfficial = transform.Find("jobOfficialBg").gameObject;
		GameObject gameObject5 = this.mJobForOfficial.transform.Find("Btn0").gameObject;
		UIEventListener expr_1C8 = UIEventListener.Get(gameObject5);
		expr_1C8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C8.onClick, new UIEventListener.VoidDelegate(this.OnJobForOfficialBtn0));
		this.mJobForOfficialBtnTxt = gameObject5.transform.Find("Label").GetComponent<UILabel>();
		this.mChangeBtn = transform.Find("changeJobBtn").gameObject;
		UIEventListener expr_226 = UIEventListener.Get(this.mChangeBtn);
		expr_226.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_226.onClick, new UIEventListener.VoidDelegate(this.OnChangeJobBtnClick));
		this.mKickOutBtn = transform.Find("kickBtn").gameObject;
		UIEventListener expr_268 = UIEventListener.Get(this.mKickOutBtn);
		expr_268.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_268.onClick, new UIEventListener.VoidDelegate(this.OnKickOutBtnClick));
		this.mChatBtn = transform.Find("chatBtn").gameObject;
		UIEventListener expr_2AA = UIEventListener.Get(this.mChatBtn);
		expr_2AA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2AA.onClick, new UIEventListener.VoidDelegate(this.OnChatBtnClick));
		this.mViewTeamBtn = transform.Find("viewTeamBtn").gameObject;
		UIEventListener expr_2EC = UIEventListener.Get(this.mViewTeamBtn);
		expr_2EC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2EC.onClick, new UIEventListener.VoidDelegate(this.OnViewTeamBtnClick));
		this.mFriendsBtn = transform.Find("friendBtn").gameObject;
		this.mFriendLb = this.mFriendsBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mFriendLb.text = string.Empty;
		UIEventListener expr_35E = UIEventListener.Get(this.mFriendsBtn);
		expr_35E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_35E.onClick, new UIEventListener.VoidDelegate(this.OnFriendBtnClick));
		this.mBlackBtn = transform.Find("blackBtn").gameObject;
		this.mBlackLb = this.mBlackBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mBlackLb.text = string.Empty;
		UIEventListener expr_3D0 = UIEventListener.Get(this.mBlackBtn);
		expr_3D0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3D0.onClick, new UIEventListener.VoidDelegate(this.OnBlackBtnClick));
		this.mMemberIcon = transform.Find("headIcon").GetComponent<UISprite>();
		this.mQualityMask = this.mMemberIcon.transform.Find("qualityMask").GetComponent<UISprite>();
		GuildSubSystem expr_436 = Globals.Instance.Player.GuildSystem;
		expr_436.RemoveMemberEvent = (GuildSubSystem.RemoveMemberCallback)Delegate.Combine(expr_436.RemoveMemberEvent, new GuildSubSystem.RemoveMemberCallback(this.OnRemoveMemberEvent));
		Globals.Instance.CliSession.Register(918, new ClientSession.MsgHandler(this.OnMsgGuildAppoint));
	}

	private void OnDestroy()
	{
		GuildSubSystem expr_0F = Globals.Instance.Player.GuildSystem;
		expr_0F.RemoveMemberEvent = (GuildSubSystem.RemoveMemberCallback)Delegate.Remove(expr_0F.RemoveMemberEvent, new GuildSubSystem.RemoveMemberCallback(this.OnRemoveMemberEvent));
		Globals.Instance.CliSession.Unregister(918, new ClientSession.MsgHandler(this.OnMsgGuildAppoint));
	}

	private void Refresh()
	{
		if (this.mGuildMember == null)
		{
			return;
		}
		this.mLvName.text = this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("Lv").Append(this.mGuildMember.Level).Append("    ").Append(this.mGuildMember.Name).ToString();
		this.mMemberIcon.spriteName = Tools.GetPlayerIcon(this.mGuildMember.FashionID);
		this.mQualityMask.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mGuildMember.ConstellationLevel));
		FriendData friendData = Globals.Instance.Player.FriendSystem.GetFriendData(this.mGuildMember.ID);
		bool flag = friendData != null && friendData.FriendType == 1;
		this.mFriendLb.text = Singleton<StringManager>.Instance.GetString((!flag) ? "friend_27" : "friend_28");
		bool flag2 = friendData != null && friendData.FriendType == 2;
		this.mBlackLb.text = Singleton<StringManager>.Instance.GetString((!flag2) ? "friend_25" : "friend_26");
		int selfGuildJob = Tools.GetSelfGuildJob();
		if (selfGuildJob == 1 || (selfGuildJob == 2 && this.mGuildMember.Rank > selfGuildJob))
		{
			this.mChangeBtn.gameObject.SetActive(true);
			this.mKickOutBtn.gameObject.SetActive(true);
		}
		else
		{
			this.mChangeBtn.gameObject.SetActive(false);
			this.mKickOutBtn.gameObject.SetActive(false);
		}
		if (selfGuildJob == 1)
		{
			this.mJobForMasterBtnTxt0.text = Tools.GetGuildMemberJobDesc(1);
			this.mJobForMasterBtnTxt1.text = ((this.mGuildMember.Rank != 2) ? Tools.GetGuildMemberJobDesc(2) : Tools.GetGuildMemberJobDesc(3));
			this.mJobForMasterBtnTxt2.text = ((this.mGuildMember.Rank != 4) ? Tools.GetGuildMemberJobDesc(4) : Tools.GetGuildMemberJobDesc(3));
		}
		if (selfGuildJob == 2 && this.mGuildMember.Rank > selfGuildJob)
		{
			this.mJobForOfficialBtnTxt.text = ((this.mGuildMember.Rank != 3) ? Tools.GetGuildMemberJobDesc(3) : Tools.GetGuildMemberJobDesc(4));
		}
		this.mJobForMaster.SetActive(false);
		this.mJobForOfficial.SetActive(false);
	}

	private void OnCloseBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void DoSendRequest(int rank)
	{
		MC2S_GuildAppoint mC2S_GuildAppoint = new MC2S_GuildAppoint();
		mC2S_GuildAppoint.ID = this.mGuildMember.ID;
		mC2S_GuildAppoint.Rank = rank;
		Globals.Instance.CliSession.Send(917, mC2S_GuildAppoint);
	}

	private void OnJobForMasterBtn0(GameObject go)
	{
		int @int = GameConst.GetInt32(3);
		if (this.mGuildMember.Level < @int)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild37", new object[]
			{
				@int
			}), 0f, 0f);
			return;
		}
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guild36"), MessageBox.Type.OKCancel, null);
		GameMessageBox expr_63 = gameMessageBox;
		expr_63.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_63.OkClick, new MessageBox.MessageDelegate(this.OnSureRankMaster));
	}

	private void OnSureRankMaster(object ob)
	{
		this.DoSendRequest(1);
	}

	private void OnJobForMasterBtn1(GameObject go)
	{
		if (this.mGuildMember.Rank == 2)
		{
			this.DoSendRequest(3);
		}
		else
		{
			this.DoSendRequest(2);
		}
	}

	private void OnJobForMasterBtn2(GameObject go)
	{
		if (this.mGuildMember.Rank == 4)
		{
			this.DoSendRequest(3);
		}
		else
		{
			this.DoSendRequest(4);
		}
	}

	private void OnJobForOfficialBtn0(GameObject go)
	{
		if (this.mGuildMember.Rank == 3)
		{
			this.DoSendRequest(4);
		}
		else
		{
			this.DoSendRequest(3);
		}
	}

	private void OnChangeJobBtnClick(GameObject go)
	{
		int selfGuildJob = Tools.GetSelfGuildJob();
		if (selfGuildJob == 1)
		{
			this.mJobForMaster.SetActive(!this.mJobForMaster.activeInHierarchy);
		}
		else
		{
			this.mJobForOfficial.SetActive(!this.mJobForOfficial.activeInHierarchy);
		}
	}

	private bool CanKickOutMember()
	{
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
		return info == null || Globals.Instance.Player.GuildSystem.Guild.KickCount < info.MaxMembers / 4;
	}

	private bool IsNewer()
	{
		bool result = false;
		if (this.mGuildMember != null)
		{
			int num = Globals.Instance.Player.GetTimeStamp() - this.mGuildMember.JoinGuildTime;
			if (0 < num && num <= 86400)
			{
				result = true;
			}
		}
		return result;
	}

	private void OnKickOutBtnClick(GameObject go)
	{
		if (this.mGuildMember == null)
		{
			return;
		}
		if (!this.CanKickOutMember())
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", 72);
			return;
		}
		if (this.IsNewer())
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", 107);
			return;
		}
		MC2S_RemoveGuildMember mC2S_RemoveGuildMember = new MC2S_RemoveGuildMember();
		mC2S_RemoveGuildMember.ID = this.mGuildMember.ID;
		Globals.Instance.CliSession.Send(907, mC2S_RemoveGuildMember);
	}

	private void OnRemoveMemberEvent(ulong playerId)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnChatBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
		GameUIManager.mInstance.CreateSession<GUIChatWindowV2>(delegate(GUIChatWindowV2 sen)
		{
			sen.SwitchToPersonalLayer(this.mGuildMember.ID, this.mGuildMember.Name);
		});
	}

	private void OnMsgGuildAppoint(MemoryStream stream)
	{
		MS2C_GuildAppoint mS2C_GuildAppoint = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildAppoint), stream) as MS2C_GuildAppoint;
		if (mS2C_GuildAppoint.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildAppoint.Result);
			return;
		}
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnViewTeamBtnClick(GameObject go)
	{
		if (this.mGuildMember != null)
		{
			MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
			mC2S_QueryRemotePlayer.PlayerID = this.mGuildMember.ID;
			Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
	}

	private void OnFriendBtnClick(GameObject go)
	{
		if (this.mGuildMember != null)
		{
			if (this.friendType == 1)
			{
				if (this.mGuildMember.ID != Globals.Instance.Player.Data.ID)
				{
					GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_22", new object[]
					{
						this.mGuildMember.Name
					}), MessageBox.Type.OKCancel, null);
					GameMessageBox expr_67 = gameMessageBox;
					expr_67.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_67.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
					{
						MC2S_RemoveFriend mC2S_RemoveFriend = new MC2S_RemoveFriend();
						mC2S_RemoveFriend.GUID = this.mGuildMember.ID;
						Globals.Instance.CliSession.Send(313, mC2S_RemoveFriend);
					}));
				}
				this.friendType = 0;
			}
			else if (this.mGuildMember.ID == Globals.Instance.Player.Data.ID)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
			}
			else
			{
				MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
				mC2S_RequestFriend.GUID = this.mGuildMember.ID;
				Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
			}
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
	}

	private void OnBlackBtnClick(GameObject go)
	{
		if (this.mGuildMember == null)
		{
			return;
		}
		if (this.friendType == 2)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_24", new object[]
			{
				this.mGuildMember.Name
			}), MessageBox.Type.OKCancel, null);
			GameMessageBox expr_44 = gameMessageBox;
			expr_44.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_44.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_RemoveBlackList mC2S_RemoveBlackList = new MC2S_RemoveBlackList();
				mC2S_RemoveBlackList.GUID = this.mGuildMember.ID;
				Globals.Instance.CliSession.Send(317, mC2S_RemoveBlackList);
			}));
			this.friendType = 0;
		}
		else if (this.mGuildMember.ID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_32", 0f, 0f);
		}
		else
		{
			GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_23", new object[]
			{
				this.mGuildMember.Name
			}), MessageBox.Type.OKCancel, null);
			GameMessageBox expr_DF = gameMessageBox2;
			expr_DF.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_DF.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_AddBlackList mC2S_AddBlackList = new MC2S_AddBlackList();
				mC2S_AddBlackList.GUID = this.mGuildMember.ID;
				Globals.Instance.CliSession.Send(315, mC2S_AddBlackList);
			}));
		}
		GameUIPopupManager.GetInstance().PopState(true, null);
	}
}
