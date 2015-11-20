using Proto;
using System;
using UnityEngine;

public class GuildApplyMemberItem : UICustomGridItem
{
	private UILabel mApplyerName;

	private UILabel mApplyerLvl;

	public ulong mApplyerId;

	public GuildApplyMemberData mGuildApplication
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
		this.mApplyerName = base.transform.Find("playerName").GetComponent<UILabel>();
		this.mApplyerLvl = base.transform.Find("level").GetComponent<UILabel>();
		GameObject gameObject = base.transform.Find("yesBtn").gameObject;
		UIEventListener expr_52 = UIEventListener.Get(gameObject);
		expr_52.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_52.onClick, new UIEventListener.VoidDelegate(this.OnYesBtnClick));
		GameObject gameObject2 = base.transform.Find("noBtn").gameObject;
		UIEventListener expr_8F = UIEventListener.Get(gameObject2);
		expr_8F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8F.onClick, new UIEventListener.VoidDelegate(this.OnNoBtnClick));
	}

	public override void Refresh(object data)
	{
		if (this.mGuildApplication == data)
		{
			return;
		}
		this.mGuildApplication = (GuildApplyMemberData)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mGuildApplication != null && this.mGuildApplication.GuildApplicationData != null)
		{
			this.mApplyerName.text = this.mGuildApplication.GuildApplicationData.Name;
			this.mApplyerLvl.text = string.Format("Lv{0}", this.mGuildApplication.GuildApplicationData.Level);
		}
	}

	private void OnYesBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.DoSendApplyRequest(this.mGuildApplication.GuildApplicationData.ID, true);
	}

	private void OnNoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.DoSendApplyRequest(this.mGuildApplication.GuildApplicationData.ID, false);
	}

	private void DoSendApplyRequest(ulong id, bool agree)
	{
		MC2S_ProcessGuildApplication mC2S_ProcessGuildApplication = new MC2S_ProcessGuildApplication();
		mC2S_ProcessGuildApplication.ID = id;
		mC2S_ProcessGuildApplication.Agree = agree;
		Globals.Instance.CliSession.Send(915, mC2S_ProcessGuildApplication);
	}
}
