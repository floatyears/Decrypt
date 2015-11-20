using System;
using System.Text;
using UnityEngine;

public class GuildCraftKillLogItem : UICustomGridItem
{
	private UISprite mBg;

	private GameObject mMVPSp;

	private UISprite mRankSp;

	private UILabel mRankTxt;

	private UISprite mOutLine;

	private UILabel mLvName;

	private UILabel mJiShaNum;

	private UILabel mJiFenNum;

	private UISprite mHeadIcon;

	private UISprite mQuiltyMask;

	private GameObject mVip;

	private UISprite mVipSingle;

	private UISprite mVipTens;

	private UISprite mVipOne;

	private StringBuilder mSb = new StringBuilder(42);

	public GuildCraftKillLogData mRecordData
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
		this.mBg = base.transform.GetComponent<UISprite>();
		this.mRankSp = base.transform.Find("rankSprite").GetComponent<UISprite>();
		this.mRankSp.gameObject.SetActive(false);
		this.mMVPSp = base.transform.Find("mvp").gameObject;
		this.mMVPSp.SetActive(false);
		this.mRankTxt = base.transform.Find("rankTxt").GetComponent<UILabel>();
		this.mRankTxt.gameObject.SetActive(false);
		this.mOutLine = base.transform.Find("outLine").GetComponent<UISprite>();
		this.mOutLine.gameObject.SetActive(false);
		this.mLvName = base.transform.Find("lvName").GetComponent<UILabel>();
		this.mJiShaNum = base.transform.Find("jiShaNum").GetComponent<UILabel>();
		this.mJiFenNum = base.transform.Find("jiFenNum").GetComponent<UILabel>();
		this.mHeadIcon = base.transform.Find("rankIcon").GetComponent<UISprite>();
		this.mQuiltyMask = this.mHeadIcon.transform.Find("Frame").GetComponent<UISprite>();
		this.mVip = this.mHeadIcon.transform.Find("VIP").gameObject;
		this.mVipSingle = this.mVip.transform.Find("Single").GetComponent<UISprite>();
		this.mVipTens = this.mVip.transform.Find("Tens").GetComponent<UISprite>();
		this.mVipOne = this.mVip.transform.Find("One").GetComponent<UISprite>();
		this.mVip.SetActive(false);
	}

	public override void Refresh(object data)
	{
		if (this.mRecordData == data)
		{
			return;
		}
		this.mRecordData = (GuildCraftKillLogData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mRecordData == null)
		{
			return;
		}
		if (Globals.Instance.Player.Data.ID == this.mRecordData.mMemberData.Data.GUID)
		{
			this.mBg.spriteName = "Retroactive_bg";
		}
		else
		{
			this.mBg.spriteName = "teamBagBg";
		}
		if (this.mRecordData.mRankNum <= 3)
		{
			this.mRankTxt.gameObject.SetActive(false);
			if (this.mRecordData.mRankNum == 1)
			{
				if (this.mRecordData.IsMVP)
				{
					this.mRankSp.gameObject.SetActive(false);
					this.mMVPSp.SetActive(true);
				}
				else
				{
					this.mRankSp.gameObject.SetActive(true);
					this.mMVPSp.SetActive(false);
					this.mRankSp.spriteName = "First";
				}
			}
			else if (this.mRecordData.mRankNum == 2)
			{
				this.mRankSp.gameObject.SetActive(true);
				this.mMVPSp.SetActive(false);
				this.mRankSp.spriteName = "Second";
			}
			else if (this.mRecordData.mRankNum == 3)
			{
				this.mRankSp.gameObject.SetActive(true);
				this.mMVPSp.SetActive(false);
				this.mRankSp.spriteName = "Third";
			}
			this.mOutLine.gameObject.SetActive(true);
			this.mOutLine.spriteName = string.Format("{0}_bg", this.mRankSp.spriteName);
		}
		else
		{
			this.mRankTxt.gameObject.SetActive(true);
			this.mRankSp.gameObject.SetActive(false);
			this.mMVPSp.SetActive(false);
			this.mOutLine.gameObject.SetActive(false);
			this.mRankTxt.text = this.mRecordData.mRankNum.ToString();
		}
		this.mHeadIcon.spriteName = Tools.GetPlayerIcon(this.mRecordData.mMemberData.Data.FashionID);
		this.mQuiltyMask.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mRecordData.mMemberData.Data.ConstellationLevel));
		int vipLevel = this.mRecordData.mMemberData.Data.VipLevel;
		if (vipLevel > 0)
		{
			this.mVip.gameObject.SetActive(true);
			if (vipLevel >= 10)
			{
				this.mVipSingle.enabled = true;
				this.mVipTens.enabled = true;
				this.mVipOne.enabled = false;
				this.mVipSingle.spriteName = (vipLevel % 10).ToString();
				this.mVipTens.spriteName = (vipLevel / 10).ToString();
			}
			else
			{
				this.mVipSingle.enabled = false;
				this.mVipTens.enabled = false;
				this.mVipOne.enabled = true;
				this.mVipOne.spriteName = vipLevel.ToString();
			}
		}
		else
		{
			this.mVip.gameObject.SetActive(false);
		}
		this.mLvName.text = string.Format("Lv{0} {1}", this.mRecordData.mMemberData.Data.Level, this.mRecordData.mMemberData.Data.Name);
		this.mJiShaNum.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildCraft8")).Append(this.mRecordData.mMemberData.Member.KillNum).ToString();
		this.mJiFenNum.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildCraft75")).Append(this.mRecordData.mMemberData.Member.Score).ToString();
	}
}
