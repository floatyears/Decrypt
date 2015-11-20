using Proto;
using System;
using UnityEngine;

public class GuildMemberItem : UICustomGridItem
{
	private UILabel mMemberName;

	private UILabel mMemberLvl;

	private UILabel mMemberJob;

	private UILabel mReputation;

	private UILabel mLastOnTime;

	private GameObject mSendBtnGo;

	private GameObject mReceiveBtnGo;

	private UIButton mSendBtn;

	private UIButton mReceiveBtn;

	public GuildMemberItemData mGuildMember
	{
		get;
		private set;
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mMemberName = base.transform.Find("name").GetComponent<UILabel>();
		this.mMemberLvl = base.transform.Find("level").GetComponent<UILabel>();
		this.mMemberJob = base.transform.Find("job").GetComponent<UILabel>();
		this.mReputation = base.transform.Find("reputation").GetComponent<UILabel>();
		this.mLastOnTime = base.transform.Find("lastTime").GetComponent<UILabel>();
		this.mSendBtnGo = base.transform.Find("sendBtn").gameObject;
		this.mSendBtn = this.mSendBtnGo.transform.Find("sendBtn").GetComponent<UIButton>();
		UIEventListener expr_D2 = UIEventListener.Get(this.mSendBtn.gameObject);
		expr_D2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D2.onClick, new UIEventListener.VoidDelegate(this.OnSendClick));
		this.mReceiveBtnGo = base.transform.Find("receiveBtn").gameObject;
		this.mReceiveBtn = this.mReceiveBtnGo.transform.Find("receiveBtn").GetComponent<UIButton>();
		UIEventListener expr_13E = UIEventListener.Get(this.mReceiveBtn.gameObject);
		expr_13E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13E.onClick, new UIEventListener.VoidDelegate(this.OnReceiveClick));
		UIEventListener expr_16A = UIEventListener.Get(base.gameObject);
		expr_16A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_16A.onClick, new UIEventListener.VoidDelegate(this.OnMemberItemClick));
		GameUITools.UpdateUIBoxCollider(base.transform, 6f, false);
	}

	public bool IsCanSendGift()
	{
		return this.mGuildMember.MemberData.ID != Globals.Instance.Player.Data.ID && (this.mGuildMember.MemberData.Flag & 2) == 0;
	}

	public bool IsCanReceiveGift()
	{
		return this.mGuildMember.MemberData.ID != Globals.Instance.Player.Data.ID && (this.mGuildMember.MemberData.Flag & 1) == 0;
	}

	public override void Refresh(object data)
	{
		if (this.mGuildMember == data)
		{
			return;
		}
		this.mGuildMember = (GuildMemberItemData)data;
		this.Refresh();
	}

	private bool IsOnLine()
	{
		return (this.mGuildMember.MemberData.Flag & 8) != 0;
	}

	public void Refresh()
	{
		if (this.mGuildMember != null && this.mGuildMember.MemberData != null)
		{
			this.mMemberName.text = this.mGuildMember.MemberData.Name;
			this.mMemberLvl.text = this.mGuildMember.MemberData.Level.ToString();
			this.mMemberJob.text = Tools.GetGuildMemberJobDesc(this.mGuildMember.MemberData.Rank);
			this.mReputation.text = this.mGuildMember.MemberData.TotalReputation.ToString();
			if (this.IsOnLine())
			{
				this.mLastOnTime.text = Singleton<StringManager>.Instance.GetString("guild25");
			}
			else
			{
				this.mLastOnTime.text = GameUITools.FormatPvpRecordTime(Mathf.Max(Globals.Instance.Player.GetTimeStamp() - this.mGuildMember.MemberData.LastOnlineTime, 0));
			}
			if (this.mGuildMember.MemberData.ID != Globals.Instance.Player.Data.ID)
			{
				this.mSendBtnGo.gameObject.SetActive(true);
				this.mReceiveBtnGo.gameObject.SetActive(true);
				this.mSendBtn.isEnabled = this.IsCanSendGift();
				this.mReceiveBtn.isEnabled = this.IsCanReceiveGift();
				UIButton[] components = this.mSendBtn.GetComponents<UIButton>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].SetState((!this.mSendBtn.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
				}
				UIButton[] components2 = this.mReceiveBtn.GetComponents<UIButton>();
				for (int j = 0; j < components2.Length; j++)
				{
					components2[j].SetState((!this.mReceiveBtn.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
				}
			}
			else
			{
				this.mSendBtnGo.gameObject.SetActive(false);
				this.mReceiveBtnGo.gameObject.SetActive(false);
			}
		}
	}

	private void OnSendClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsCanSendGift())
		{
			MC2S_GiveGift mC2S_GiveGift = new MC2S_GiveGift();
			mC2S_GiveGift.PlayerID = this.mGuildMember.MemberData.ID;
			Globals.Instance.CliSession.Send(950, mC2S_GiveGift);
		}
	}

	private void OnReceiveClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.TakeGuildGift == GameConst.GetInt32(166))
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", 96);
			return;
		}
		if (this.IsCanReceiveGift())
		{
			MC2S_TakeGift mC2S_TakeGift = new MC2S_TakeGift();
			mC2S_TakeGift.PlayerID = this.mGuildMember.MemberData.ID;
			Globals.Instance.CliSession.Send(948, mC2S_TakeGift);
		}
	}

	private void OnMemberItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.ID != this.mGuildMember.MemberData.ID)
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildMemberPopUp, false, null, null);
			GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(this.mGuildMember.MemberData);
		}
	}
}
