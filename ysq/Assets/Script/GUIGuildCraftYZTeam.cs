using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIGuildCraftYZTeam : MonoBehaviour
{
	private UILabel mPeiLvTxt;

	private UISprite mTeamBtnSp1;

	private UILabel mTeamName1;

	private UILabel mZhiChiNum1;

	private UISprite mTeamBtnSp2;

	private UILabel mTeamName2;

	private UILabel mZhiChiNum2;

	private GUICraftYZSliderNumber mGUICraftYZSliderNumber;

	private UIButton mXZBtn;

	private EGuildWarId mWarID;

	private EGuildWarTeamId mSupportTeamId = EGuildWarTeamId.EGWTI_None;

	private StringBuilder mSb = new StringBuilder(42);

	public EGuildWarTeamId SupportTeamId
	{
		get
		{
			return this.mSupportTeamId;
		}
		set
		{
			this.mSupportTeamId = value;
			if (this.mSupportTeamId == EGuildWarTeamId.EGWTI_Red)
			{
				this.mTeamBtnSp1.spriteName = "btn2";
				this.mTeamBtnSp2.spriteName = "btnBg3";
			}
			else if (this.mSupportTeamId == EGuildWarTeamId.EGWTI_Blue)
			{
				this.mTeamBtnSp1.spriteName = "btnBg3";
				this.mTeamBtnSp2.spriteName = "btn2";
			}
			else
			{
				this.mTeamBtnSp1.spriteName = "btnBg3";
				this.mTeamBtnSp2.spriteName = "btnBg3";
			}
		}
	}

	public void InitWithBaseScene(EGuildWarId warID)
	{
		this.mWarID = warID;
		this.CreateObjects();
		this.SupportTeamId = EGuildWarTeamId.EGWTI_None;
	}

	private void CreateObjects()
	{
		this.mPeiLvTxt = base.transform.Find("peiLv").GetComponent<UILabel>();
		this.mTeamBtnSp1 = base.transform.Find("teamBtn0").GetComponent<UISprite>();
		UIEventListener expr_46 = UIEventListener.Get(this.mTeamBtnSp1.gameObject);
		expr_46.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_46.onClick, new UIEventListener.VoidDelegate(this.OnTeamBtn1Click));
		this.mTeamName1 = this.mTeamBtnSp1.transform.Find("txt").GetComponent<UILabel>();
		this.mZhiChiNum1 = this.mTeamBtnSp1.transform.Find("zcNum").GetComponent<UILabel>();
		this.mTeamBtnSp2 = base.transform.Find("teamBtn1").GetComponent<UISprite>();
		UIEventListener expr_D2 = UIEventListener.Get(this.mTeamBtnSp2.gameObject);
		expr_D2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D2.onClick, new UIEventListener.VoidDelegate(this.OnTeamBtn2Click));
		this.mTeamName2 = this.mTeamBtnSp2.transform.Find("txt").GetComponent<UILabel>();
		this.mZhiChiNum2 = this.mTeamBtnSp2.transform.Find("zcNum").GetComponent<UILabel>();
		this.mGUICraftYZSliderNumber = base.transform.Find("Input").gameObject.AddComponent<GUICraftYZSliderNumber>();
		this.mGUICraftYZSliderNumber.InitWithBaseScene();
		this.mGUICraftYZSliderNumber.Minimum = 1;
		int @int = GameConst.GetInt32(222);
		this.mGUICraftYZSliderNumber.Maximum = @int;
		this.mGUICraftYZSliderNumber.CharLimit = @int.ToString().Length;
		this.mXZBtn = base.transform.Find("xiaZhuBtn").GetComponent<UIButton>();
		UIEventListener expr_1C3 = UIEventListener.Get(this.mXZBtn.gameObject);
		expr_1C3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C3.onClick, new UIEventListener.VoidDelegate(this.OnXZBtnClick));
	}

	private string FormatPerCent(float perCent)
	{
		int num;
		if (int.TryParse(perCent.ToString(), out num))
		{
			return num.ToString();
		}
		return perCent.ToString("f2");
	}

	public void Refresh()
	{
		List<GuildWarClientSupportInfo> battleSupportInfo = Globals.Instance.Player.GuildSystem.BattleSupportInfo;
		if (battleSupportInfo == null)
		{
			return;
		}
		for (int i = 0; i < battleSupportInfo.Count; i++)
		{
			GuildWarClientSupportInfo guildWarClientSupportInfo = battleSupportInfo[i];
			if (guildWarClientSupportInfo != null)
			{
				if (guildWarClientSupportInfo.WarID == this.mWarID)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("craftYZ1")).Append("  ").Append(this.FormatPerCent(guildWarClientSupportInfo.Red.LossPerCent)).Append(" : ").Append(this.FormatPerCent(guildWarClientSupportInfo.Blue.LossPerCent));
					this.mPeiLvTxt.text = this.mSb.ToString();
					ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
					this.mTeamName1.text = guildWarClientSupportInfo.Red.GuildName;
					this.mTeamName1.color = ((iD != guildWarClientSupportInfo.Red.GuildID) ? Color.white : Color.green);
					this.mTeamName2.text = guildWarClientSupportInfo.Blue.GuildName;
					this.mTeamName2.color = ((iD != guildWarClientSupportInfo.Blue.GuildID) ? Color.white : Color.green);
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("craftYZ2")).Append(guildWarClientSupportInfo.Red.Diamond);
					this.mZhiChiNum1.text = this.mSb.ToString();
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("craftYZ2")).Append(guildWarClientSupportInfo.Blue.Diamond);
					this.mZhiChiNum2.text = this.mSb.ToString();
					if (guildWarClientSupportInfo.SupportTeamID == EGuildWarTeamId.EGWTI_Red || guildWarClientSupportInfo.SupportTeamID == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mXZBtn.isEnabled = false;
						Tools.SetButtonState(this.mXZBtn.gameObject, false);
						this.mGUICraftYZSliderNumber.Editable = false;
						this.mGUICraftYZSliderNumber.Number = guildWarClientSupportInfo.SupportDiamond;
						this.SupportTeamId = guildWarClientSupportInfo.SupportTeamID;
					}
					else
					{
						this.mXZBtn.isEnabled = true;
						Tools.SetButtonState(this.mXZBtn.gameObject, true);
						this.mGUICraftYZSliderNumber.Editable = true;
						this.mGUICraftYZSliderNumber.Number = this.mGUICraftYZSliderNumber.Minimum;
					}
					break;
				}
			}
		}
	}

	private void OnXZBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.SupportTeamId == EGuildWarTeamId.EGWTI_None)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("craftYZ3", 0f, 0f);
			return;
		}
		List<GuildWarClientSupportInfo> battleSupportInfo = Globals.Instance.Player.GuildSystem.BattleSupportInfo;
		if (battleSupportInfo == null)
		{
			return;
		}
		for (int i = 0; i < battleSupportInfo.Count; i++)
		{
			GuildWarClientSupportInfo guildWarClientSupportInfo = battleSupportInfo[i];
			if (guildWarClientSupportInfo != null)
			{
				if (guildWarClientSupportInfo.WarID == this.mWarID)
				{
					if (guildWarClientSupportInfo.SupportTeamID == EGuildWarTeamId.EGWTI_Red || guildWarClientSupportInfo.SupportTeamID == EGuildWarTeamId.EGWTI_Blue)
					{
						GameUIManager.mInstance.ShowMessageTip("EGR", 124);
						return;
					}
					break;
				}
			}
		}
		MC2S_GuildWarSupport mC2S_GuildWarSupport = new MC2S_GuildWarSupport();
		mC2S_GuildWarSupport.WarID = this.mWarID;
		mC2S_GuildWarSupport.TeamID = this.SupportTeamId;
		mC2S_GuildWarSupport.Diamond = this.mGUICraftYZSliderNumber.Number;
		Globals.Instance.CliSession.Send(986, mC2S_GuildWarSupport);
	}

	private void DoSelectTeam(EGuildWarTeamId whichTeam)
	{
		List<GuildWarClientSupportInfo> battleSupportInfo = Globals.Instance.Player.GuildSystem.BattleSupportInfo;
		if (battleSupportInfo == null)
		{
			return;
		}
		for (int i = 0; i < battleSupportInfo.Count; i++)
		{
			GuildWarClientSupportInfo guildWarClientSupportInfo = battleSupportInfo[i];
			if (guildWarClientSupportInfo != null)
			{
				if (guildWarClientSupportInfo.WarID == this.mWarID)
				{
					if (guildWarClientSupportInfo.SupportTeamID == EGuildWarTeamId.EGWTI_Red || guildWarClientSupportInfo.SupportTeamID == EGuildWarTeamId.EGWTI_Blue)
					{
						GameUIManager.mInstance.ShowMessageTip("EGR", 124);
					}
					else
					{
						this.SupportTeamId = whichTeam;
					}
					break;
				}
			}
		}
	}

	private void OnTeamBtn1Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.DoSelectTeam(EGuildWarTeamId.EGWTI_Red);
	}

	private void OnTeamBtn2Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.DoSelectTeam(EGuildWarTeamId.EGWTI_Blue);
	}
}
