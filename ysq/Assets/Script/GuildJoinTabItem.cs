using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GuildJoinTabItem : UICustomGridItem
{
	private GameObject mRefreshBtnGo;

	private GameObject mEnterItemGo;

	private UILabel mGuildName;

	private UILabel mPeopleNum;

	private UILabel mGuildAnnounce;

	private UILabel mJoinLvl;

	private UILabel mEnterBtnTxt;

	private GameObject mEnterBtn;

	private UIButton mEnterBtnUI;

	private GameObject mEntering;

	private StringBuilder mStringBuilder = new StringBuilder();

	public GuildJoinTabItemData mBriefGuildData
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
		this.mEnterItemGo = base.transform.Find("guildEnterItem").gameObject;
		Transform transform = this.mEnterItemGo.transform;
		this.mGuildName = transform.Find("tagBg/guildName").GetComponent<UILabel>();
		this.mPeopleNum = transform.Find("tagBg/peopleNum").GetComponent<UILabel>();
		this.mGuildAnnounce = transform.Find("guildAnnounce").GetComponent<UILabel>();
		this.mJoinLvl = transform.Find("joinLvl").GetComponent<UILabel>();
		this.mEnterBtn = transform.Find("enterBtn").gameObject;
		UIEventListener expr_A0 = UIEventListener.Get(this.mEnterBtn);
		expr_A0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A0.onClick, new UIEventListener.VoidDelegate(this.OnEnterClick));
		this.mEnterBtnUI = this.mEnterBtn.GetComponent<UIButton>();
		this.mEnterBtnTxt = this.mEnterBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mEntering = transform.Find("entering").gameObject;
		GameUITools.UpdateUIBoxCollider(transform, 8f, false);
		this.mRefreshBtnGo = base.transform.Find("guildEnterRefreshBtn").gameObject;
		UIEventListener expr_13A = UIEventListener.Get(this.mRefreshBtnGo);
		expr_13A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13A.onClick, new UIEventListener.VoidDelegate(this.OnRefreshBtnClick));
	}

	public override void Refresh(object data)
	{
		if (this.mBriefGuildData == data)
		{
			return;
		}
		this.mBriefGuildData = (GuildJoinTabItemData)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mBriefGuildData != null)
		{
			if (!this.mBriefGuildData.mIsRefreshBtn)
			{
				if (this.mBriefGuildData.mBriefGuildData == null)
				{
					return;
				}
				this.mRefreshBtnGo.SetActive(false);
				this.mEnterItemGo.SetActive(true);
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("Lv ").Append(this.mBriefGuildData.mBriefGuildData.Level).Append("    ").Append(this.mBriefGuildData.mBriefGuildData.Name);
				this.mGuildName.text = this.mStringBuilder.ToString();
				GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(this.mBriefGuildData.mBriefGuildData.Level);
				int num = (info == null) ? 0 : info.MaxMembers;
				int memberNum = this.mBriefGuildData.mBriefGuildData.MemberNum;
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append(Singleton<StringManager>.Instance.GetString("guild2"));
				if (memberNum >= num)
				{
					this.mStringBuilder.Append("[ff0000]").Append(memberNum).Append("/").Append(num).Append("[-]");
				}
				else
				{
					this.mStringBuilder.Append(memberNum).Append("/").Append(num);
				}
				this.mPeopleNum.text = this.mStringBuilder.ToString();
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append(Singleton<StringManager>.Instance.GetString("guild3")).Append(this.mBriefGuildData.mBriefGuildData.Manifesto);
				this.mGuildAnnounce.text = this.mStringBuilder.ToString();
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append(Singleton<StringManager>.Instance.GetString("guild4")).Append(this.mBriefGuildData.mBriefGuildData.ApplyLevel).Append("  ").Append(Singleton<StringManager>.Instance.GetString("guild39")).Append(this.mBriefGuildData.mBriefGuildData.CombatValue);
				this.mJoinLvl.text = this.mStringBuilder.ToString();
				if (this.mBriefGuildData.mBriefGuildData.Flag == 0)
				{
					this.mEnterBtn.gameObject.SetActive(true);
					this.mEntering.gameObject.SetActive(false);
					this.mEnterBtnTxt.text = Singleton<StringManager>.Instance.GetString("guild5");
				}
				else if (this.mBriefGuildData.mBriefGuildData.Flag == 1)
				{
					this.mEnterBtn.gameObject.SetActive(false);
					this.mEntering.gameObject.SetActive(true);
				}
				else
				{
					this.mEnterBtn.gameObject.SetActive(true);
					this.mEntering.gameObject.SetActive(false);
					this.mEnterBtnTxt.text = Singleton<StringManager>.Instance.GetString("guild6");
				}
				if (this.mEnterBtn.gameObject.activeInHierarchy)
				{
					this.mEnterBtnUI.isEnabled = (memberNum < num);
					UIButton[] components = this.mEnterBtn.GetComponents<UIButton>();
					for (int i = 0; i < components.Length; i++)
					{
						components[i].SetState((memberNum >= num) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
					}
				}
			}
			else
			{
				this.mRefreshBtnGo.SetActive(true);
				this.mEnterItemGo.SetActive(false);
			}
		}
	}

	private void OnEnterClick(GameObject go)
	{
		if (this.mBriefGuildData != null && !this.mBriefGuildData.mIsRefreshBtn && this.mBriefGuildData.mBriefGuildData != null)
		{
			if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)this.mBriefGuildData.mBriefGuildData.ApplyLevel))
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guild7", 0f, 0f);
				return;
			}
			if (Globals.Instance.Player.TeamSystem.GetCombatValue() < this.mBriefGuildData.mBriefGuildData.CombatValue)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guild41", 0f, 0f);
				return;
			}
			int joinGuildCD = Globals.Instance.Player.GuildSystem.GetJoinGuildCD();
			if (joinGuildCD != 0)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild24", new object[]
				{
					Tools.FormatTimeStr3(joinGuildCD, false, true)
				}), 0f, 0f);
			}
			else
			{
				MC2S_GuildApply mC2S_GuildApply = new MC2S_GuildApply();
				mC2S_GuildApply.ID = this.mBriefGuildData.mBriefGuildData.ID;
				Globals.Instance.CliSession.Send(911, mC2S_GuildApply);
			}
		}
	}

	private void OnRefreshBtnClick(GameObject go)
	{
		if (this.mBriefGuildData != null && this.mBriefGuildData.mIsRefreshBtn)
		{
			MC2S_GetGuildData ojb = new MC2S_GetGuildData();
			Globals.Instance.CliSession.Send(901, ojb);
		}
	}
}
