using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIGuildSchoolItem : UICustomGridItem
{
	private GUIGuildSchoolPopUp mBasePop;

	private UISprite mItemBgSp;

	private UISprite mMasterIcon;

	private UISprite mQualityMask;

	private UILabel mSchoolName;

	private UILabel mTipState;

	private UILabel mSucRewardNum;

	private GameObject mTipGo;

	private GameObject mTipEnd;

	private UILabel mProgressTxt;

	private UILabel mMaskTip;

	private UILabel mTipRewardStatus;

	private GameObject mGoBtn;

	private GameObject mNewMark;

	private GameObject mMaskGo;

	private UISlider mProgressBar;

	private GUIGuildSchoolItemData mGUIGuildSchoolItemData;

	private StringBuilder mSb = new StringBuilder(42);

	public GUIGuildSchoolItemData SchoolItemData
	{
		get
		{
			return this.mGUIGuildSchoolItemData;
		}
	}

	public void InitWithBaseScene(GUIGuildSchoolPopUp basePop)
	{
		this.mBasePop = basePop;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mSchoolName = base.transform.Find("schoolName").GetComponent<UILabel>();
		this.mProgressBar = base.transform.Find("progressBar").GetComponent<UISlider>();
		this.mProgressTxt = this.mProgressBar.transform.Find("ExpText").GetComponent<UILabel>();
		this.mMaskGo = base.transform.Find("maskTip").gameObject;
		this.mMaskTip = this.mMaskGo.GetComponent<UILabel>();
		GameObject gameObject = this.mMaskGo.transform.Find("mask").gameObject;
		UIEventListener expr_A3 = UIEventListener.Get(gameObject);
		expr_A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A3.onClick, new UIEventListener.VoidDelegate(this.OnSchoolItemClick));
		this.mTipGo = base.transform.Find("tipGo").gameObject;
		this.mSucRewardNum = this.mTipGo.transform.Find("c").GetComponent<UILabel>();
		this.mTipEnd = base.transform.Find("tipEnd").gameObject;
		this.mMasterIcon = base.transform.Find("itemIcon").GetComponent<UISprite>();
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mTipState = base.transform.Find("tipState").GetComponent<UILabel>();
		this.mTipRewardStatus = base.transform.Find("tipReward").GetComponent<UILabel>();
		this.mGoBtn = base.transform.Find("goBtn").gameObject;
		UIEventListener expr_1AC = UIEventListener.Get(this.mGoBtn);
		expr_1AC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1AC.onClick, new UIEventListener.VoidDelegate(this.OnGoBtnClick));
		this.mNewMark = this.mGoBtn.transform.Find("newMark").gameObject;
		UIEventListener expr_1F8 = UIEventListener.Get(base.gameObject);
		expr_1F8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1F8.onClick, new UIEventListener.VoidDelegate(this.OnSchoolItemClick));
		this.mItemBgSp = base.transform.GetComponent<UISprite>();
		GameUITools.UpdateUIBoxCollider(base.transform, 4f, false);
	}

	public override void Refresh(object data)
	{
		if (this.mGUIGuildSchoolItemData != data)
		{
			this.mGUIGuildSchoolItemData = (GUIGuildSchoolItemData)data;
			this.Refresh();
		}
	}

	private void Refresh()
	{
		GuildData guild = Globals.Instance.Player.GuildSystem.Guild;
		if (guild != null && this.mGUIGuildSchoolItemData != null && this.mGUIGuildSchoolItemData.GuildInFo != null)
		{
			this.mSchoolName.text = Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
			{
				this.mGUIGuildSchoolItemData.SchoolId,
				this.mGUIGuildSchoolItemData.GuildInFo.Academy
			});
			GuildBossData guildBossData = Globals.Instance.Player.GuildSystem.GetGuildBossData(this.mGUIGuildSchoolItemData.SchoolId);
			if (guildBossData != null)
			{
				MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(guildBossData.InfoID);
				if (info != null)
				{
					this.mMasterIcon.spriteName = info.Icon;
					this.mQualityMask.spriteName = Tools.GetItemQualityIcon(info.Quality);
				}
				this.mMaskGo.SetActive(false);
				if (this.mGUIGuildSchoolItemData.SchoolId == 1 && guild.MaxAcademyID == 0)
				{
					this.mTipGo.SetActive(true);
					this.mGoBtn.SetActive(true);
					this.mProgressBar.gameObject.SetActive(true);
					this.mTipState.gameObject.SetActive(false);
					this.mSucRewardNum.text = this.mGUIGuildSchoolItemData.GuildInFo.BossReputation.ToString();
					this.mProgressBar.value = Mathf.Clamp01(guildBossData.HealthPct);
					this.mProgressTxt.text = string.Format("{0}%", Mathf.RoundToInt(Mathf.Clamp01(guildBossData.HealthPct) * 100f));
					this.mTipEnd.SetActive(guildBossData.HealthPct == 0f);
					if (guildBossData.HealthPct == 0f)
					{
						this.mTipRewardStatus.gameObject.SetActive(true);
						if ((Globals.Instance.Player.Data.DataFlag & 16) != 0)
						{
							this.mTipRewardStatus.text = Singleton<StringManager>.Instance.GetString("guildSchool6");
							this.mNewMark.SetActive(false);
						}
						else
						{
							this.mTipRewardStatus.text = Singleton<StringManager>.Instance.GetString("guildSchool9");
							this.mNewMark.SetActive(true);
						}
					}
					else
					{
						this.mTipRewardStatus.gameObject.SetActive(false);
						this.mNewMark.SetActive(Tools.IsInGuildBossTime() && Tools.IsGuildBossHasNum());
					}
					this.mItemBgSp.spriteName = "gold_bg";
				}
				else if (this.mGUIGuildSchoolItemData.SchoolId > guild.MaxAcademyID + 1)
				{
					this.mTipGo.SetActive(false);
					this.mGoBtn.SetActive(false);
					this.mProgressBar.gameObject.SetActive(false);
					this.mTipRewardStatus.gameObject.SetActive(false);
					this.mNewMark.SetActive(false);
					this.mTipEnd.SetActive(false);
					GuildInfo info2 = Globals.Instance.AttDB.GuildDict.GetInfo(this.mGUIGuildSchoolItemData.SchoolId - 1);
					if (info2 != null)
					{
						this.mTipState.gameObject.SetActive(true);
						this.mTipState.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildSchool8")).Append(" ").Append(Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
						{
							this.mGUIGuildSchoolItemData.SchoolId - 1,
							info2.Academy
						})).ToString();
					}
					else
					{
						this.mTipState.gameObject.SetActive(false);
					}
					this.mItemBgSp.spriteName = "teamBagBg";
				}
				else if (this.mGUIGuildSchoolItemData.SchoolId == guild.AttackAcademyID1)
				{
					this.mTipGo.SetActive(true);
					this.mGoBtn.SetActive(true);
					this.mProgressBar.gameObject.SetActive(true);
					this.mTipState.gameObject.SetActive(false);
					this.mSucRewardNum.text = this.mGUIGuildSchoolItemData.GuildInFo.BossReputation.ToString();
					this.mProgressBar.value = Mathf.Clamp01(guildBossData.HealthPct);
					this.mProgressTxt.text = string.Format("{0}%", Mathf.RoundToInt(Mathf.Clamp01(guildBossData.HealthPct) * 100f));
					this.mTipEnd.SetActive(guildBossData.HealthPct == 0f);
					if (guildBossData.HealthPct == 0f)
					{
						this.mTipRewardStatus.gameObject.SetActive(true);
						if ((Globals.Instance.Player.Data.DataFlag & 16) != 0)
						{
							this.mTipRewardStatus.text = Singleton<StringManager>.Instance.GetString("guildSchool6");
							this.mNewMark.SetActive(false);
						}
						else
						{
							this.mTipRewardStatus.text = Singleton<StringManager>.Instance.GetString("guildSchool9");
							this.mNewMark.SetActive(true);
						}
					}
					else
					{
						this.mTipRewardStatus.gameObject.SetActive(false);
						this.mNewMark.SetActive(Tools.IsInGuildBossTime() && Tools.IsGuildBossHasNum());
					}
					this.mItemBgSp.spriteName = "gold_bg";
				}
				else
				{
					this.mTipGo.SetActive(false);
					this.mGoBtn.SetActive(false);
					this.mProgressBar.gameObject.SetActive(false);
					this.mTipRewardStatus.gameObject.SetActive(false);
					this.mNewMark.SetActive(false);
					this.mTipState.gameObject.SetActive(true);
					this.mTipEnd.SetActive(false);
					this.mTipState.text = Singleton<StringManager>.Instance.GetString("guildSchool4");
					this.mItemBgSp.spriteName = "teamBagBg";
				}
			}
			else
			{
				if (this.mGUIGuildSchoolItemData.GuildInFo.BossID != 0)
				{
					MonsterInfo info3 = Globals.Instance.AttDB.MonsterDict.GetInfo(this.mGUIGuildSchoolItemData.GuildInFo.BossID);
					if (info3 != null)
					{
						this.mMasterIcon.spriteName = info3.Icon;
						this.mQualityMask.spriteName = Tools.GetItemQualityIcon(info3.Quality);
					}
				}
				this.mTipGo.SetActive(false);
				GuildInfo info4 = Globals.Instance.AttDB.GuildDict.GetInfo(this.mGUIGuildSchoolItemData.SchoolId - 1);
				if (info4 != null)
				{
					this.mTipState.gameObject.SetActive(true);
					this.mTipState.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildSchool8")).Append(" ").Append(Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
					{
						this.mGUIGuildSchoolItemData.SchoolId - 1,
						info4.Academy
					})).ToString();
				}
				else
				{
					this.mTipState.gameObject.SetActive(false);
				}
				this.mGoBtn.SetActive(false);
				this.mProgressBar.gameObject.SetActive(false);
				this.mTipRewardStatus.gameObject.SetActive(false);
				this.mNewMark.SetActive(false);
				this.mMaskGo.SetActive(true);
				this.mMaskTip.text = Singleton<StringManager>.Instance.GetString("guildSchool1", new object[]
				{
					this.mGUIGuildSchoolItemData.SchoolId
				});
				this.mTipEnd.SetActive(false);
				this.mItemBgSp.spriteName = "teamBagBg";
			}
		}
	}

	private void OnGoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mGUIGuildSchoolItemData != null)
		{
			GuildBossData guildBossData = Globals.Instance.Player.GuildSystem.GetGuildBossData(this.mGUIGuildSchoolItemData.SchoolId);
			if (guildBossData != null)
			{
				GameUIManager.mInstance.ChangeSession<GUIGuildSchoolScene>(null, false, true);
				GameUIPopupManager.GetInstance().PopState(true, null);
			}
		}
	}

	private void OnSchoolItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mGUIGuildSchoolItemData != null)
		{
			this.mBasePop.ShowSchoolItemTip(this.mGUIGuildSchoolItemData.SchoolId);
		}
	}
}
