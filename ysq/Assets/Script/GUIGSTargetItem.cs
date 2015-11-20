using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIGSTargetItem : UICustomGridItem
{
	private GUIGuildSchoolTargetPopUp mBasePop;

	private UISprite mItemBgSp;

	private UISprite mMasterIcon;

	private UISprite mQualityMask;

	private UILabel mSchoolName;

	private UILabel mTipState;

	private GameObject mTipGo;

	private UILabel mSucRewardNum;

	private GameObject mMaskGo;

	private UILabel mMaskTip;

	private GUIGSTargetToggleBtn mToggleBtn;

	private GUIGSTargetItemData mGUIGSTargetItemData;

	private StringBuilder mSb = new StringBuilder(42);

	public bool IsToggleBtnChecked
	{
		get
		{
			return this.mToggleBtn.IsChecked;
		}
		set
		{
			this.mToggleBtn.IsChecked = value;
		}
	}

	public GUIGSTargetItemData TargetItemData
	{
		get
		{
			return this.mGUIGSTargetItemData;
		}
	}

	public void InitWithBaseScene(GUIGuildSchoolTargetPopUp basePop)
	{
		this.mBasePop = basePop;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mSchoolName = base.transform.Find("schoolName").GetComponent<UILabel>();
		this.mMaskGo = base.transform.Find("maskTip").gameObject;
		this.mMaskTip = this.mMaskGo.GetComponent<UILabel>();
		this.mTipGo = base.transform.Find("tipGo").gameObject;
		this.mSucRewardNum = this.mTipGo.transform.Find("c").GetComponent<UILabel>();
		this.mMasterIcon = base.transform.Find("itemIcon").GetComponent<UISprite>();
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mTipState = base.transform.Find("tipState").GetComponent<UILabel>();
		this.mToggleBtn = base.transform.Find("togBtnBg").gameObject.AddComponent<GUIGSTargetToggleBtn>();
		this.mToggleBtn.InitToggleBtn(false);
		GUIGSTargetToggleBtn expr_105 = this.mToggleBtn;
		expr_105.ToggleClickEvent = (GUIGSTargetToggleBtn.ToggleClickCallback)Delegate.Combine(expr_105.ToggleClickEvent, new GUIGSTargetToggleBtn.ToggleClickCallback(this.OnToggleBtnClick));
		this.mItemBgSp = base.transform.GetComponent<UISprite>();
		GameUITools.UpdateUIBoxCollider(base.transform, 4f, false);
	}

	public override void Refresh(object data)
	{
		if (this.mGUIGSTargetItemData != data)
		{
			this.mGUIGSTargetItemData = (GUIGSTargetItemData)data;
			this.Refresh();
		}
	}

	public void Refresh()
	{
		GuildData guild = Globals.Instance.Player.GuildSystem.Guild;
		if (guild != null && this.mGUIGSTargetItemData != null && this.mGUIGSTargetItemData.GuildInFo != null)
		{
			this.mSchoolName.text = Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
			{
				this.mGUIGSTargetItemData.SchoolId,
				this.mGUIGSTargetItemData.GuildInFo.Academy
			});
			GuildBossData guildBossData = Globals.Instance.Player.GuildSystem.GetGuildBossData(this.mGUIGSTargetItemData.SchoolId);
			if (guildBossData != null)
			{
				MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(guildBossData.InfoID);
				if (info != null)
				{
					this.mMasterIcon.spriteName = info.Icon;
					this.mQualityMask.spriteName = Tools.GetItemQualityIcon(info.Quality);
				}
				this.mMaskGo.SetActive(false);
				if (this.mGUIGSTargetItemData.SchoolId <= guild.MaxAcademyID + 1 || (this.mGUIGSTargetItemData.SchoolId == 1 && guild.MaxAcademyID == 0))
				{
					this.mTipGo.SetActive(true);
					this.mToggleBtn.gameObject.SetActive(true);
					this.mToggleBtn.IsChecked = this.mGUIGSTargetItemData.IsSelected;
					this.mTipState.gameObject.SetActive(false);
					this.mSucRewardNum.text = this.mGUIGSTargetItemData.GuildInFo.BossReputation.ToString();
					this.mItemBgSp.spriteName = ((!this.mGUIGSTargetItemData.IsSelected) ? "teamBagBg" : "gold_bg");
				}
				else
				{
					this.mTipGo.SetActive(false);
					this.mToggleBtn.gameObject.SetActive(false);
					GuildInfo info2 = Globals.Instance.AttDB.GuildDict.GetInfo(this.mGUIGSTargetItemData.SchoolId - 1);
					if (info2 != null)
					{
						this.mTipState.gameObject.SetActive(true);
						this.mTipState.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildSchool8")).Append(" ").Append(Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
						{
							this.mGUIGSTargetItemData.SchoolId - 1,
							info2.Academy
						})).ToString();
					}
					else
					{
						this.mTipState.gameObject.SetActive(false);
					}
					this.mItemBgSp.spriteName = "teamBagBg";
				}
			}
			else
			{
				if (this.mGUIGSTargetItemData.GuildInFo.BossID != 0)
				{
					MonsterInfo info3 = Globals.Instance.AttDB.MonsterDict.GetInfo(this.mGUIGSTargetItemData.GuildInFo.BossID);
					if (info3 != null)
					{
						this.mMasterIcon.spriteName = info3.Icon;
						this.mQualityMask.spriteName = Tools.GetItemQualityIcon(info3.Quality);
					}
				}
				this.mTipGo.SetActive(false);
				GuildInfo info4 = Globals.Instance.AttDB.GuildDict.GetInfo(this.mGUIGSTargetItemData.SchoolId - 1);
				if (info4 != null)
				{
					this.mTipState.gameObject.SetActive(true);
					this.mTipState.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildSchool8")).Append(" ").Append(Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
					{
						this.mGUIGSTargetItemData.SchoolId - 1,
						info4.Academy
					})).ToString();
				}
				else
				{
					this.mTipState.gameObject.SetActive(false);
				}
				this.mToggleBtn.gameObject.SetActive(false);
				this.mMaskGo.SetActive(true);
				this.mMaskTip.text = Singleton<StringManager>.Instance.GetString("guildSchool1", new object[]
				{
					this.mGUIGSTargetItemData.SchoolId
				});
				this.mItemBgSp.spriteName = "teamBagBg";
			}
		}
	}

	private void OnToggleBtnClick()
	{
		int selfGuildJob = Tools.GetSelfGuildJob();
		if ((selfGuildJob == 1 || selfGuildJob == 2) && this.mGUIGSTargetItemData != null && !this.IsToggleBtnChecked)
		{
			this.mBasePop.SelectSchool(this.mGUIGSTargetItemData.SchoolId);
		}
	}
}
