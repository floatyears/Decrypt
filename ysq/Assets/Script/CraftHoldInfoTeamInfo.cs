using Proto;
using System;
using System.Text;
using UnityEngine;

public class CraftHoldInfoTeamInfo : MonoBehaviour
{
	private const int mPetNum = 3;

	private UILabel mLeftHp;

	private UILabel mZhanDouState;

	private UILabel mZhenYing;

	private UISprite mRankIcon;

	private UISprite mRankIconFrame;

	private UISlider mRankHp;

	private CraftHoldInfoPetIcon[] mPets = new CraftHoldInfoPetIcon[3];

	private GameObject mRecordBtn;

	private StringBuilder mSb = new StringBuilder(10);

	private void CreateObjects()
	{
		this.mRankIcon = base.transform.Find("rankIcon").GetComponent<UISprite>();
		this.mRankIconFrame = this.mRankIcon.transform.Find("Frame").GetComponent<UISprite>();
		this.mRankHp = this.mRankIcon.transform.Find("hp").GetComponent<UISlider>();
		this.mPets[0] = this.mRankIcon.transform.Find("Pet0").gameObject.AddComponent<CraftHoldInfoPetIcon>();
		this.mPets[1] = this.mRankIcon.transform.Find("Pet1").gameObject.AddComponent<CraftHoldInfoPetIcon>();
		this.mPets[2] = this.mRankIcon.transform.Find("Pet2").gameObject.AddComponent<CraftHoldInfoPetIcon>();
		this.mPets[0].Init();
		this.mPets[1].Init();
		this.mPets[2].Init();
		this.mRecordBtn = base.transform.Find("recordBtn").gameObject;
		UIEventListener expr_11D = UIEventListener.Get(this.mRecordBtn);
		expr_11D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_11D.onClick, new UIEventListener.VoidDelegate(this.OnRecordBtnClick));
		this.mZhanDouState = base.transform.Find("txt0/name").GetComponent<UILabel>();
		this.mZhanDouState.text = this.mSb.Remove(0, this.mSb.Length).Append("(").Append(Singleton<StringManager>.Instance.GetString("guildCraft34")).Append(")").ToString();
		this.mZhanDouState.gameObject.SetActive(false);
		this.mLeftHp = base.transform.Find("txt1/name").GetComponent<UILabel>();
		this.mZhenYing = base.transform.Find("txt3/name").GetComponent<UILabel>();
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	public void Refresh()
	{
		GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
		if (localClientMember == null)
		{
			return;
		}
		if (localClientMember.Member == null)
		{
			return;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		this.mRankIcon.spriteName = Tools.GetPlayerIcon(localClientMember.Data.FashionID);
		this.mRankIconFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(localClientMember.Data.ConstellationLevel));
		this.mRankHp.value = Mathf.Clamp01((float)localClientMember.Member.HealthPct / 10000f);
		int i;
		for (i = 0; i < localClientMember.Data.PetInfoID.Count; i++)
		{
			this.mPets[i].Refresh(localClientMember.Data.PetInfoID[i], (float)localClientMember.Member.HealthPct);
		}
		while (i < 3)
		{
			this.mPets[i].Refresh(0, 0f);
			i++;
		}
		this.mLeftHp.text = this.mSb.Remove(0, this.mSb.Length).Append(Mathf.RoundToInt((float)(localClientMember.Member.HealthPct / 100))).Append("%").ToString();
		bool flag = Globals.Instance.Player.GuildSystem.IsGuanZhanMember(mGWEnterData.WarID);
		if (flag)
		{
			this.mZhanDouState.gameObject.SetActive(false);
			this.mZhenYing.text = string.Empty;
			this.mRecordBtn.SetActive(false);
		}
		else
		{
			this.mZhanDouState.gameObject.SetActive(localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Fighting);
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			if (selfTeamFlag == EGuildWarTeamId.EGWTI_Red)
			{
				this.mZhenYing.text = this.mSb.Remove(0, this.mSb.Length).Append("[d42d2d]").Append(Singleton<StringManager>.Instance.GetString("guildCraft48")).Append("[-]").ToString();
			}
			else if (selfTeamFlag == EGuildWarTeamId.EGWTI_Blue)
			{
				this.mZhenYing.text = this.mSb.Remove(0, this.mSb.Length).Append("[4a85ff]").Append(Singleton<StringManager>.Instance.GetString("guildCraft49")).Append("[-]").ToString();
			}
			else
			{
				this.mZhenYing.text = string.Empty;
			}
			this.mRecordBtn.SetActive(true);
		}
	}

	private void OnRecordBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.IsCanZhanMember(mGWEnterData.WarID))
		{
			MC2S_GuildWarQueryCombatRecord mC2S_GuildWarQueryCombatRecord = new MC2S_GuildWarQueryCombatRecord();
			mC2S_GuildWarQueryCombatRecord.WarID = mGWEnterData.WarID;
			Globals.Instance.CliSession.Send(1001, mC2S_GuildWarQueryCombatRecord);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildCraft77", 0f, 0f);
		}
	}
}
