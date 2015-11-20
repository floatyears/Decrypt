using Proto;
using System;
using UnityEngine;

public class FriendCommonItem : UICustomGridItem
{
	private FriendDataEx mFriendData;

	private UISprite mParBg;

	private UISprite mParIcon;

	private GameObject mVIP;

	private UISprite mVIPSingle;

	private UISprite mVIPTens;

	private GameObject mGetKeyBtn;

	private GameObject mSendKeyBtn;

	private GameObject mAddFriendBtn;

	private GameObject mRemoveFriendBtn;

	private GameObject mAgreeBtn;

	private GameObject mRefuseBtn;

	private UIButton mSendKeyBtnBtn;

	private UIButton mGetKeyBtnBtn;

	public UIButton mAddFriendBtnBtn;

	public UIButton mAgreeBtnBtn;

	public UIButton mRefuseBtnBtn;

	private UILabel mFightingScore;

	private UILabel mGuild;

	private UILabel mGuildName;

	private UILabel mTime;

	private UILabel mName;

	public override void Refresh(object data)
	{
		this.mFriendData = (FriendDataEx)data;
		this.Refresh();
	}

	public void Init()
	{
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		Transform transform = base.transform.Find("infoBg");
		this.mFightingScore = transform.Find("fightingScore").GetComponent<UILabel>();
		this.mGuild = transform.Find("guild").GetComponent<UILabel>();
		this.mGuildName = this.mGuild.transform.Find("Label").GetComponent<UILabel>();
		this.mParBg = base.transform.Find("parBg").GetComponent<UISprite>();
		this.mParIcon = this.mParBg.transform.Find("par").GetComponent<UISprite>();
		this.mVIP = this.mParBg.transform.Find("VIP").gameObject;
		this.mVIPSingle = GameUITools.FindUISprite("Single", this.mVIP);
		this.mVIPTens = GameUITools.FindUISprite("Tens", this.mVIP);
		this.mTime = base.transform.Find("time").GetComponent<UILabel>();
		this.mName = base.transform.Find("name").GetComponent<UILabel>();
		this.mGetKeyBtn = base.transform.Find("getKeyBtn").gameObject;
		this.mSendKeyBtn = base.transform.Find("sendKeyBtn").gameObject;
		this.mAddFriendBtn = base.transform.Find("addFriendBtn").gameObject;
		this.mRemoveFriendBtn = base.transform.Find("removeFriendBtn").gameObject;
		this.mAgreeBtn = base.transform.Find("agreeBtn").gameObject;
		this.mRefuseBtn = base.transform.Find("refuseBtn").gameObject;
		this.mSendKeyBtnBtn = this.mSendKeyBtn.GetComponent<UIButton>();
		this.mGetKeyBtnBtn = this.mGetKeyBtn.GetComponent<UIButton>();
		this.mAddFriendBtnBtn = this.mAddFriendBtn.GetComponent<UIButton>();
		this.mAgreeBtnBtn = this.mAgreeBtn.GetComponent<UIButton>();
		this.mRefuseBtnBtn = this.mRefuseBtn.GetComponent<UIButton>();
		UIEventListener expr_221 = UIEventListener.Get(this.mParBg.gameObject);
		expr_221.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_221.onClick, new UIEventListener.VoidDelegate(this.OnItemIconBtnClick));
		UIEventListener expr_24D = UIEventListener.Get(this.mGetKeyBtn);
		expr_24D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_24D.onClick, new UIEventListener.VoidDelegate(this.OnGetHeartBtnClick));
		UIEventListener expr_279 = UIEventListener.Get(this.mSendKeyBtn);
		expr_279.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_279.onClick, new UIEventListener.VoidDelegate(this.OnSendHeartBtnClick));
		UIEventListener expr_2A5 = UIEventListener.Get(this.mAddFriendBtn);
		expr_2A5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2A5.onClick, new UIEventListener.VoidDelegate(this.OnAddFriendBtnClick));
		UIEventListener expr_2D1 = UIEventListener.Get(this.mRemoveFriendBtn);
		expr_2D1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2D1.onClick, new UIEventListener.VoidDelegate(this.OnRemoveBacklistBtnClick));
		UIEventListener expr_2FD = UIEventListener.Get(this.mAgreeBtn);
		expr_2FD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2FD.onClick, new UIEventListener.VoidDelegate(this.OnAgreeBtnClick));
		UIEventListener expr_329 = UIEventListener.Get(this.mRefuseBtn);
		expr_329.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_329.onClick, new UIEventListener.VoidDelegate(this.OnRefuseBtnClick));
	}

	private void OnAgreeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mFriendData.DoSendApplyRequest(true);
	}

	private void OnRefuseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mFriendData.DoSendApplyRequest(false);
	}

	private void OnRemoveBacklistBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_24", new object[]
		{
			this.mFriendData.FriendData.Name
		}), MessageBox.Type.OKCancel, this.mFriendData);
		GameMessageBox expr_4B = gameMessageBox;
		expr_4B.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_4B.OkClick, new MessageBox.MessageDelegate(this.OnRemoveBacklistOKClicked));
	}

	private void OnRemoveBacklistOKClicked(object obj)
	{
		MC2S_RemoveBlackList mC2S_RemoveBlackList = new MC2S_RemoveBlackList();
		mC2S_RemoveBlackList.GUID = this.mFriendData.FriendData.GUID;
		Globals.Instance.CliSession.Send(317, mC2S_RemoveBlackList);
	}

	private void OnAddFriendBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.FriendSystem.friends.Count < GameConst.GetInt32(194))
		{
			this.mFriendData.DoAddFriend();
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_106", 0f, 0f);
		}
	}

	private void OnSendHeartBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsCanSendEngery())
		{
			MC2S_GiveFriendEnergy mC2S_GiveFriendEnergy = new MC2S_GiveFriendEnergy();
			mC2S_GiveFriendEnergy.GUID = this.mFriendData.FriendData.GUID;
			Globals.Instance.CliSession.Send(319, mC2S_GiveFriendEnergy);
		}
		this.RefreshEngeryBtn();
	}

	private void OnGetHeartBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsCanGetEngery())
		{
			MC2S_TakeFriendEnergy mC2S_TakeFriendEnergy = new MC2S_TakeFriendEnergy();
			mC2S_TakeFriendEnergy.GUID = this.mFriendData.FriendData.GUID;
			Globals.Instance.CliSession.Send(321, mC2S_TakeFriendEnergy);
		}
		this.RefreshEngeryBtn();
	}

	private void OnItemIconBtnClick(GameObject go)
	{
		GUIFriendInfoPopUp.Show(this.mFriendData.FriendData);
		if (this.mFriendData.FriendType == EUITableLayers.ESL_Friend)
		{
			GameUIManager.mInstance.uiState.SelectFriendID = this.mFriendData.FriendData.GUID;
		}
		if (this.mFriendData.FriendType == EUITableLayers.ESL_BlackList)
		{
			GameUIManager.mInstance.uiState.SelectFriendID = this.mFriendData.FriendData.GUID;
		}
		if (this.mFriendData.FriendType == EUITableLayers.ESL_FriendRequest)
		{
			GameUIManager.mInstance.uiState.SelectFriendID = this.mFriendData.FriendData.GUID;
		}
		if (this.mFriendData.FriendType == EUITableLayers.ESL_FriendRecommend)
		{
			GameUIManager.mInstance.uiState.SelectFriendID = this.mFriendData.FriendData.GUID;
		}
	}

	private void RefreshEngeryBtn()
	{
		bool flag = this.IsCanSendEngery();
		bool flag2 = this.IsCanGetEngery();
		this.mSendKeyBtnBtn.isEnabled = flag;
		this.mGetKeyBtnBtn.isEnabled = flag2;
		Tools.SetButtonState(this.mSendKeyBtnBtn.gameObject, flag);
		Tools.SetButtonState(this.mGetKeyBtnBtn.gameObject, flag2);
	}

	public bool IsCanSendEngery()
	{
		return this.mFriendData.FriendData.GUID != Globals.Instance.Player.Data.ID && (this.mFriendData.FriendData.Flag & 1) == 0;
	}

	public bool IsCanGetEngery()
	{
		return this.IsFriend() && (this.mFriendData.FriendData.Flag & 2) != 0 && (this.mFriendData.FriendData.Flag & 4) == 0;
	}

	private bool IsOnLine()
	{
		return this.mFriendData.FriendData.Offline == 0;
	}

	public bool IsFriend()
	{
		bool result = false;
		for (int i = 0; i < Globals.Instance.Player.FriendSystem.friends.Count; i++)
		{
			FriendData friendData = Globals.Instance.Player.FriendSystem.friends[i];
			if (friendData.GUID != Globals.Instance.Player.Data.ID && this.mFriendData.FriendData.GUID == friendData.GUID)
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
			if (friendData.GUID != Globals.Instance.Player.Data.ID && this.mFriendData.FriendData.GUID == friendData.GUID)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void Refresh()
	{
		if (this.mFriendData == null)
		{
			return;
		}
		this.mName.text = this.mFriendData.FriendData.Name;
		this.mName.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(this.mFriendData.FriendData.ConLevel));
		this.mFightingScore.text = Singleton<StringManager>.Instance.GetString("friend_2", new object[]
		{
			this.mFriendData.FriendData.CombatValue.ToString()
		});
		this.mFightingScore.color = Color.green;
		if (!string.IsNullOrEmpty(this.mFriendData.FriendData.GuildName))
		{
			this.mGuild.text = Singleton<StringManager>.Instance.GetString("friend_29");
			this.mGuildName.text = Singleton<StringManager>.Instance.GetString("friend_15", new object[]
			{
				this.mFriendData.FriendData.GuildName
			});
		}
		else
		{
			this.mGuild.text = Singleton<StringManager>.Instance.GetString("friend_3");
			this.mGuildName.text = string.Empty;
		}
		this.mParBg.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mFriendData.FriendData.ConLevel));
		this.mParIcon.spriteName = Tools.GetPlayerIcon(this.mFriendData.FriendData.FashionID);
		switch (this.mFriendData.FriendType)
		{
		case EUITableLayers.ESL_Friend:
			this.mGetKeyBtn.SetActive(true);
			this.mSendKeyBtn.SetActive(true);
			this.mAddFriendBtn.SetActive(false);
			this.mRemoveFriendBtn.SetActive(false);
			this.mAgreeBtn.SetActive(false);
			this.mRefuseBtn.SetActive(false);
			this.mTime.gameObject.SetActive(true);
			this.RefreshEngeryBtn();
			break;
		case EUITableLayers.ESL_FriendRecommend:
			this.mGetKeyBtn.SetActive(false);
			this.mSendKeyBtn.SetActive(false);
			this.mAddFriendBtn.SetActive(true);
			this.mRemoveFriendBtn.SetActive(false);
			this.mAgreeBtn.SetActive(false);
			this.mRefuseBtn.SetActive(false);
			this.mTime.gameObject.SetActive(true);
			break;
		case EUITableLayers.ESL_BlackList:
			this.mGetKeyBtn.SetActive(false);
			this.mSendKeyBtn.SetActive(false);
			this.mAddFriendBtn.SetActive(false);
			this.mRemoveFriendBtn.SetActive(true);
			this.mAgreeBtn.SetActive(false);
			this.mRefuseBtn.SetActive(false);
			this.mTime.gameObject.SetActive(false);
			break;
		case EUITableLayers.ESL_FriendRequest:
			this.mGetKeyBtn.SetActive(false);
			this.mSendKeyBtn.SetActive(false);
			this.mAddFriendBtn.SetActive(false);
			this.mRemoveFriendBtn.SetActive(false);
			this.mAgreeBtn.SetActive(true);
			this.mRefuseBtn.SetActive(true);
			this.mTime.gameObject.SetActive(true);
			break;
		}
		NGUITools.SetActive(this.mTime.gameObject, true);
		if (this.IsOnLine())
		{
			this.mTime.text = Singleton<StringManager>.Instance.GetString("friend_13");
		}
		else
		{
			int offline = this.mFriendData.FriendData.Offline;
			this.mTime.text = GameUITools.FormatPvpRecordTime(Mathf.Max(Globals.Instance.Player.GetTimeStamp() - offline, 0));
		}
		int vipLevel = this.mFriendData.FriendData.VipLevel;
		if (this.mVIPSingle != null && this.mVIPTens != null)
		{
			if (vipLevel > 0)
			{
				this.mVIP.SetActive(true);
				this.mVIPSingle.enabled = true;
				if (vipLevel >= 10)
				{
					this.mVIPSingle.enabled = true;
					this.mVIPSingle.spriteName = (vipLevel % 10).ToString();
					this.mVIPTens.spriteName = (vipLevel / 10).ToString();
				}
				else
				{
					this.mVIPSingle.enabled = false;
					this.mVIPTens.spriteName = vipLevel.ToString();
				}
			}
			else
			{
				this.mVIP.SetActive(false);
			}
		}
	}
}
