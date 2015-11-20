using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GuildBaseInfoTabLayer : MonoBehaviour
{
	private UILabel mGuildProsperity;

	private UILabel mGuildLvl;

	private UILabel mGuildName;

	private UILabel mMasterName;

	private UILabel mID;

	private UILabel mMemberNum;

	private UILabel mExpTxt;

	private UILabel mMoneyNum;

	private UILabel mAnnounceTxt;

	private GuildAnnounceInput mAnnounceInput;

	private UISlider mExp;

	private GuildActivityTable mGuildActiveGrid;

	private GameObject mActiveLeftBtn;

	private GameObject mActiveRightBtn;

	private GameObject mImpeach;

	private GameObject mImpeaching;

	private GameObject mLeaveGuild;

	private GameObject mGuildSetBtn;

	private GameObject mChangeAnnounceBtn;

	private GameObject mProsperityTip;

	public GuildActivityItemData mGuildMagicItem;

	public GuildActivityItemData mGuildSchoolItem;

	public GuildActivityItemData mGuildKuangShiItem;

	public GuildActivityItemData mGuildCraftItem;

	private StringBuilder mStringBuilder = new StringBuilder();

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.Refresh();
		this.InitActivities();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("ProsperityBg").gameObject;
		UIEventListener expr_1C = UIEventListener.Get(gameObject);
		expr_1C.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_1C.onPress, new UIEventListener.BoolDelegate(this.OnProsperityBgPressed));
		this.mGuildProsperity = gameObject.transform.Find("num").GetComponent<UILabel>();
		this.mProsperityTip = gameObject.transform.Find("tipBg").gameObject;
		this.mProsperityTip.SetActive(false);
		Transform transform = base.transform.Find("infoBg");
		this.mGuildLvl = transform.Find("titleBg/guildLvl").GetComponent<UILabel>();
		this.mGuildName = transform.Find("titleBg/guildName").GetComponent<UILabel>();
		this.mMasterName = transform.Find("txt0/manager").GetComponent<UILabel>();
		this.mID = transform.Find("txt1/ID").GetComponent<UILabel>();
		this.mMemberNum = transform.Find("txt2/peopleNum").GetComponent<UILabel>();
		this.mExp = transform.Find("txt3/Exp").GetComponent<UISlider>();
		this.mExpTxt = this.mExp.transform.Find("ExpText").GetComponent<UILabel>();
		transform.Find("txt4").gameObject.SetActive(false);
		this.mMoneyNum = transform.Find("txt4/money").GetComponent<UILabel>();
		this.mImpeach = transform.Find("impeach").gameObject;
		UIEventListener expr_181 = UIEventListener.Get(this.mImpeach);
		expr_181.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_181.onClick, new UIEventListener.VoidDelegate(this.OnImpeachClick));
		this.mImpeaching = transform.Find("impeaching").gameObject;
		UIEventListener expr_1C3 = UIEventListener.Get(this.mImpeaching);
		expr_1C3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C3.onClick, new UIEventListener.VoidDelegate(this.OnImpeachingClick));
		Transform transform2 = base.transform.Find("guildActivity");
		this.mGuildActiveGrid = transform2.Find("contentsPanel/recordContents").gameObject.AddComponent<GuildActivityTable>();
		this.mGuildActiveGrid.maxPerLine = 20;
		this.mGuildActiveGrid.arrangement = UICustomGrid.Arrangement.Horizontal;
		this.mGuildActiveGrid.cellWidth = 116f;
		this.mGuildActiveGrid.cellHeight = 126f;
		this.mActiveLeftBtn = transform2.Find("leftBtn").gameObject;
		UIEventListener expr_26A = UIEventListener.Get(this.mActiveLeftBtn);
		expr_26A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26A.onClick, new UIEventListener.VoidDelegate(this.OnActiveLeftBtnClick));
		this.mActiveLeftBtn.SetActive(false);
		this.mActiveRightBtn = transform2.Find("rightBtn").gameObject;
		UIEventListener expr_2B8 = UIEventListener.Get(this.mActiveRightBtn);
		expr_2B8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2B8.onClick, new UIEventListener.VoidDelegate(this.OnActiveRightBtnClick));
		this.mActiveRightBtn.SetActive(false);
		this.mChangeAnnounceBtn = base.transform.Find("announceBg/changeBtn").gameObject;
		UIEventListener expr_30B = UIEventListener.Get(this.mChangeAnnounceBtn);
		expr_30B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_30B.onClick, new UIEventListener.VoidDelegate(this.OnChangeAnnounceClick));
		this.mAnnounceTxt = base.transform.Find("announceBg/announceTxt").GetComponent<UILabel>();
		UIEventListener expr_357 = UIEventListener.Get(this.mAnnounceTxt.gameObject);
		expr_357.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_357.onClick, new UIEventListener.VoidDelegate(this.OnAnnounceTxtClick));
		this.mAnnounceInput = base.transform.Find("announceBg/announceInput").GetComponent<GuildAnnounceInput>();
		EventDelegate.Add(this.mAnnounceInput.onSubmit, new EventDelegate.Callback(this.CommitAnnounceInput));
		GuildAnnounceInput expr_3B6 = this.mAnnounceInput;
		expr_3B6.mInputLoseFocus = (GuildAnnounceInput.VoidCallback)Delegate.Combine(expr_3B6.mInputLoseFocus, new GuildAnnounceInput.VoidCallback(this.OnAnnounceInputLoseFocus));
		this.mLeaveGuild = base.transform.Find("leaveGuild").gameObject;
		UIEventListener expr_3FD = UIEventListener.Get(this.mLeaveGuild);
		expr_3FD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3FD.onClick, new UIEventListener.VoidDelegate(this.OnLeaveGuildClick));
		GameObject gameObject2 = base.transform.Find("rankGuild").gameObject;
		UIEventListener expr_43A = UIEventListener.Get(gameObject2);
		expr_43A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_43A.onClick, new UIEventListener.VoidDelegate(this.OnRankGuildClick));
		GameObject gameObject3 = base.transform.Find("logGuild").gameObject;
		UIEventListener expr_479 = UIEventListener.Get(gameObject3);
		expr_479.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_479.onClick, new UIEventListener.VoidDelegate(this.OnLogGuildClick));
		this.mGuildSetBtn = base.transform.Find("setGuild").gameObject;
		UIEventListener expr_4C0 = UIEventListener.Get(this.mGuildSetBtn);
		expr_4C0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4C0.onClick, new UIEventListener.VoidDelegate(this.OnGuildSetBtnClick));
	}

	private void OnProsperityBgPressed(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.mProsperityTip.SetActive(true);
		}
		else if (this.mProsperityTip != null)
		{
			this.mProsperityTip.SetActive(false);
		}
	}

	public void RefreshAnnounce()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			this.mAnnounceTxt.text = Globals.Instance.Player.GuildSystem.Guild.Manifesto;
			this.mAnnounceTxt.gameObject.SetActive(true);
			this.mAnnounceInput.gameObject.SetActive(false);
		}
	}

	private void OnAnnounceInputLoseFocus()
	{
		this.RefreshAnnounce();
	}

	public void RefreshGuildName()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			this.mGuildName.text = Globals.Instance.Player.GuildSystem.Guild.Name;
		}
	}

	public void RefreshImpectBtn()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			if (Globals.Instance.Player.GuildSystem.Guild.ImpeachID != 0uL)
			{
				this.mImpeaching.SetActive(true);
				this.mImpeach.SetActive(false);
			}
			else
			{
				this.mImpeaching.SetActive(false);
				bool flag = false;
				for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
				{
					GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
					if (guildMember.Rank == 1 && Globals.Instance.Player.GetTimeStamp() - guildMember.LastOnlineTime > 604800)
					{
						flag = true;
					}
				}
				this.mImpeach.SetActive(Tools.GetSelfGuildJob() != 1 && flag);
			}
		}
	}

	public void RefreshMagicItem()
	{
		for (int i = 0; i < this.mGuildActiveGrid.mDatas.Count; i++)
		{
			GuildActivityItemData guildActivityItemData = (GuildActivityItemData)this.mGuildActiveGrid.mDatas[i];
			if (guildActivityItemData != null)
			{
				guildActivityItemData.RefreshRedMark();
			}
		}
		for (int j = 0; j < this.mGuildActiveGrid.transform.childCount; j++)
		{
			GuildActivityItem component = this.mGuildActiveGrid.transform.GetChild(j).GetComponent<GuildActivityItem>();
			if (component != null)
			{
				component.Refresh();
			}
		}
	}

	private void InitActivities()
	{
		this.mGuildActiveGrid.ClearData();
		this.mGuildMagicItem = new GuildActivityItemData(GuildActivityItemData.EGAIType.EGAIGuildMagic);
		this.mGuildActiveGrid.AddData(this.mGuildMagicItem);
		this.mGuildActiveGrid.AddData(new GuildActivityItemData(GuildActivityItemData.EGAIType.EGAIGuildShop));
		this.mGuildSchoolItem = new GuildActivityItemData(GuildActivityItemData.EGAIType.EGAIGuildSchool);
		this.mGuildActiveGrid.AddData(this.mGuildSchoolItem);
		this.mGuildKuangShiItem = new GuildActivityItemData(GuildActivityItemData.EGAIType.EGAIGuildKuangShi);
		this.mGuildActiveGrid.AddData(this.mGuildKuangShiItem);
		this.mGuildCraftItem = new GuildActivityItemData(GuildActivityItemData.EGAIType.EGAIGuildCraft);
		this.mGuildActiveGrid.AddData(this.mGuildCraftItem);
	}

	public void Refresh()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GuildData guild = Globals.Instance.Player.GuildSystem.Guild;
			int selfGuildJob = Tools.GetSelfGuildJob();
			if (selfGuildJob == 1 || selfGuildJob == 2)
			{
				this.mLeaveGuild.transform.localPosition = new Vector3(74f, this.mLeaveGuild.transform.localPosition.y, this.mLeaveGuild.transform.localPosition.z);
				this.mGuildSetBtn.SetActive(true);
			}
			else
			{
				this.mLeaveGuild.transform.localPosition = new Vector3(180f, this.mLeaveGuild.transform.localPosition.y, this.mLeaveGuild.transform.localPosition.z);
				this.mGuildSetBtn.SetActive(false);
			}
			this.mChangeAnnounceBtn.SetActive(selfGuildJob == 1 || selfGuildJob == 2);
			this.RefreshImpectBtn();
			this.mGuildProsperity.text = guild.Prosperity.ToString();
			this.mGuildLvl.text = string.Format("Lv{0}", guild.Level);
			this.RefreshGuildName();
			this.mMasterName.text = Tools.GetMasterName();
			this.mID.text = guild.ID.ToString();
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(guild.Level);
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append(Globals.Instance.Player.GuildSystem.Members.Count).Append("/").Append((info == null) ? 0 : info.MaxMembers);
			this.mMemberNum.text = this.mStringBuilder.ToString();
			this.mExp.value = (float)guild.Exp / ((info == null) ? 1f : ((float)info.Exp));
			this.mExpTxt.text = this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append(guild.Exp).Append("/").Append((info == null) ? 1 : info.Exp).ToString();
			this.mMoneyNum.text = guild.Money.ToString();
			this.RefreshAnnounce();
			this.RefreshMagicItem();
		}
	}

	private void OnGuildSetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildSetPopUp, false, null, null);
	}

	private void OnChangeAnnounceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int selfGuildJob = Tools.GetSelfGuildJob();
		if (selfGuildJob == 1 || selfGuildJob == 2)
		{
			this.mAnnounceTxt.gameObject.SetActive(false);
			this.mAnnounceInput.gameObject.SetActive(true);
			this.mAnnounceInput.value = this.mAnnounceTxt.text;
			this.mAnnounceInput.isSelected = true;
		}
	}

	private void OnAnnounceTxtClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int selfGuildJob = Tools.GetSelfGuildJob();
		if (selfGuildJob == 1 || selfGuildJob == 2)
		{
			this.mAnnounceTxt.gameObject.SetActive(false);
			this.mAnnounceInput.gameObject.SetActive(true);
			this.mAnnounceInput.value = this.mAnnounceTxt.text;
			this.mAnnounceInput.isSelected = true;
		}
	}

	private void CommitAnnounceInput()
	{
		if (!string.IsNullOrEmpty(this.mAnnounceInput.value))
		{
			MC2S_SetGuildManifesto mC2S_SetGuildManifesto = new MC2S_SetGuildManifesto();
			mC2S_SetGuildManifesto.Manifesto = this.mAnnounceInput.value;
			Globals.Instance.CliSession.Send(919, mC2S_SetGuildManifesto);
		}
	}

	private void OnImpeachClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildImpeachPopUp, false, null, null);
	}

	private void GetImpeachAboutName(out string name1, out string name2)
	{
		name1 = string.Empty;
		name2 = string.Empty;
		for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
		{
			GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
			if (guildMember.Rank == 1)
			{
				name2 = guildMember.Name;
			}
			if (guildMember.ID == Globals.Instance.Player.GuildSystem.Guild.ImpeachID)
			{
				name1 = guildMember.Name;
			}
		}
	}

	private void OnImpeachingClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		string empty = string.Empty;
		string empty2 = string.Empty;
		this.GetImpeachAboutName(out empty, out empty2);
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guild12", new object[]
		{
			empty,
			empty2
		}), MessageBox.Type.OKCancel, null);
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("guild13");
		gameMessageBox.TextCancel = Singleton<StringManager>.Instance.GetString("guild14");
		GameMessageBox expr_7B = gameMessageBox;
		expr_7B.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_7B.OkClick, new MessageBox.MessageDelegate(this.OnSureImpeachClick));
	}

	private void OnSureImpeachClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_SupportImpeach ojb = new MC2S_SupportImpeach();
		Globals.Instance.CliSession.Send(925, ojb);
	}

	private void OnActiveLeftBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnActiveRightBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnLeaveGuildOkClick(object go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_LeaveGuild ojb = new MC2S_LeaveGuild();
		Globals.Instance.CliSession.Send(905, ojb);
	}

	private void OnLeaveGuildClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guild16"), MessageBox.Type.OKCancel, null);
		GameMessageBox expr_2D = gameMessageBox;
		expr_2D.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_2D.OkClick, new MessageBox.MessageDelegate(this.OnLeaveGuildOkClick));
	}

	private void OnRankGuildClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_GuildRankData ojb = new MC2S_GuildRankData();
		Globals.Instance.CliSession.Send(958, ojb);
	}

	private void OnLogGuildClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.Player.GuildSystem.SendQueryGuildEvent();
	}
}
