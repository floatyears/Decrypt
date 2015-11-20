using Att;
using Proto;
using System;
using UnityEngine;

public abstract class CommonRankItemBase : UICustomGridItem
{
	private class RankPetIcon : MonoBehaviour
	{
		private UISprite mIcon;

		private UISprite mFrame;

		private GameObject mStars;

		private UISprite[] mStar = new UISprite[5];

		public void Init()
		{
			this.mIcon = base.gameObject.GetComponent<UISprite>();
			this.mFrame = GameUITools.FindUISprite("Frame", base.gameObject);
			this.mStars = GameUITools.FindGameObject("Stars", this.mIcon.gameObject);
			int num = 0;
			while (num < this.mStars.transform.childCount && num < this.mStar.Length)
			{
				this.mStar[num] = this.mStars.transform.GetChild(num).GetComponent<UISprite>();
				num++;
			}
		}

		public void Refresh(int id, int awakeLevel)
		{
			if (id == 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(id);
			if (info == null)
			{
				global::Debug.LogErrorFormat("Get Pet Info Error , ID : {0} ", new object[]
				{
					id
				});
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			this.mIcon.spriteName = info.Icon;
			this.mFrame.spriteName = Tools.GetItemQualityIcon(info.Quality);
			int num = awakeLevel / 10;
			if (num > 0)
			{
				this.mStars.gameObject.SetActive(true);
				int i = 0;
				while (i < num && i < this.mStar.Length)
				{
					this.mStar[i].spriteName = "star";
					i++;
				}
				while (i < this.mStar.Length)
				{
					this.mStar[i].spriteName = "star0";
					i++;
				}
			}
			else
			{
				this.mStars.gameObject.SetActive(false);
			}
		}
	}

	protected GameUICommonBillboardPopUp mBaseScene;

	protected BillboardCommonLayer mBaseLayer;

	protected GUIGuildMinesResultPopUp mBasePop;

	public BillboardInfoData mUserData;

	protected UISprite mOutlineSprite;

	protected UISprite mBg;

	protected UISprite mRankSprite;

	protected UISprite mRankIcon;

	protected UISprite mRankIconFrame;

	private GameObject mVip;

	private UISprite mVipSingle;

	private UISprite mVipTens;

	private UISprite mVipOne;

	private GameObject mStars;

	private UISprite[] mStar = new UISprite[5];

	private CommonRankItemBase.RankPetIcon[] mPets = new CommonRankItemBase.RankPetIcon[3];

	protected UISprite mGoldIcon;

	protected UISprite mDiamondIcon;

	protected CommonIconItem mGoldItemIcon;

	protected CommonIconItem mDiamondItemIcon;

	protected UILabel mRankTxt;

	protected UILabel mDiamondNum;

	protected UILabel mGoldNum;

	protected UILabel mLvlName;

	protected UILabel mScore;

	protected int nameXValue = 5;

	public virtual int GetDiamond()
	{
		return 0;
	}

	public virtual int GetMoney()
	{
		return 0;
	}

	public abstract int GetRank();

	public abstract string GetLvlName();

	public abstract string GetScore();

	public virtual int GetSpecialNo()
	{
		return this.GetRank();
	}

	public virtual void InitWithBaseScene(object baseScene)
	{
		if (baseScene is GameUICommonBillboardPopUp)
		{
			this.mBaseScene = (GameUICommonBillboardPopUp)baseScene;
		}
		else if (baseScene is BillboardCommonLayer)
		{
			this.mBaseLayer = (BillboardCommonLayer)baseScene;
		}
		else
		{
			if (!(baseScene is GUIGuildMinesResultPopUp))
			{
				return;
			}
			this.mBasePop = (GUIGuildMinesResultPopUp)baseScene;
		}
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBg = base.transform.GetComponent<UISprite>();
		this.mOutlineSprite = base.transform.Find("outLine").GetComponent<UISprite>();
		this.mRankTxt = base.transform.Find("rankTxt").GetComponent<UILabel>();
		this.mRankSprite = base.transform.Find("rankSprite").GetComponent<UISprite>();
		this.mRankIcon = base.transform.Find("rankIcon").GetComponent<UISprite>();
		this.mRankIconFrame = GameUITools.FindUISprite("Frame", this.mRankIcon.gameObject);
		this.mVip = GameUITools.FindGameObject("VIP", this.mRankIcon.gameObject);
		this.mVipTens = GameUITools.FindUISprite("Tens", this.mVip);
		this.mVipSingle = GameUITools.FindUISprite("Single", this.mVip);
		this.mVipOne = GameUITools.FindUISprite("One", this.mVip);
		this.mStars = GameUITools.FindGameObject("Stars", this.mRankIcon.gameObject);
		int num = 0;
		while (num < this.mStars.transform.childCount && num < this.mStar.Length)
		{
			this.mStar[num] = this.mStars.transform.GetChild(num).GetComponent<UISprite>();
			num++;
		}
		this.mPets[0] = GameUITools.FindGameObject("Pet0", this.mRankIcon.gameObject).AddComponent<CommonRankItemBase.RankPetIcon>();
		this.mPets[1] = GameUITools.FindGameObject("Pet1", this.mRankIcon.gameObject).AddComponent<CommonRankItemBase.RankPetIcon>();
		this.mPets[2] = GameUITools.FindGameObject("Pet2", this.mRankIcon.gameObject).AddComponent<CommonRankItemBase.RankPetIcon>();
		this.mPets[0].Init();
		this.mPets[1].Init();
		this.mPets[2].Init();
		Transform transform = base.transform.Find("trailInfo");
		this.mDiamondNum = transform.Find("diamondNum").GetComponent<UILabel>();
		this.mDiamondIcon = this.mDiamondNum.transform.Find("diamond").GetComponent<UISprite>();
		this.mGoldNum = transform.Find("goldNum").GetComponent<UILabel>();
		this.mGoldIcon = this.mGoldNum.transform.Find("gold").GetComponent<UISprite>();
		this.mLvlName = transform.Find("LvName").GetComponent<UILabel>();
		this.mScore = transform.Find("score").GetComponent<UILabel>();
		UIDragScrollView uIDragScrollView = base.gameObject.AddComponent<UIDragScrollView>();
		uIDragScrollView.scrollView = base.transform.parent.parent.GetComponent<UIScrollView>();
		GameUITools.UpdateUIBoxCollider(base.transform, 4f, false);
	}

	protected virtual string GetRankSprite()
	{
		switch (this.GetSpecialNo())
		{
		case 1:
			return "First";
		case 2:
			return "Second";
		case 3:
			return "Third";
		default:
			return string.Empty;
		}
	}

	public override void Refresh(object data)
	{
		if (this.mUserData == data)
		{
			return;
		}
		this.mUserData = (BillboardInfoData)data;
		this.Refresh();
	}

	public virtual void Refresh()
	{
		if (this.mUserData != null && this.mUserData.userData != null)
		{
			UIEventListener.Get(base.gameObject).onClick = null;
			UIEventListener expr_37 = UIEventListener.Get(base.gameObject);
			expr_37.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_37.onClick, new UIEventListener.VoidDelegate(this.OnRankItemClick));
			if (this.mUserData.userData is RankData)
			{
				RankData rankData = (RankData)this.mUserData.userData;
				if (rankData != null)
				{
					this.mRankIcon.spriteName = Tools.GetPlayerIcon(rankData.Data.FashionID);
					this.mRankIconFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(rankData.Data.ConstellationLevel));
					int vipLevel = rankData.Data.VipLevel;
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
					int num = rankData.Data.AwakeLevel / 10;
					if (num > 0)
					{
						this.mStars.gameObject.SetActive(true);
						int i = 0;
						while (i < num && i < this.mStar.Length)
						{
							this.mStar[i].spriteName = "star";
							i++;
						}
						while (i < this.mStar.Length)
						{
							this.mStar[i].spriteName = "star0";
							i++;
						}
					}
					else
					{
						this.mStars.gameObject.SetActive(false);
					}
					int j;
					for (j = 0; j < rankData.Data.PetInfoID.Count; j++)
					{
						this.mPets[j].Refresh(rankData.Data.PetInfoID[j], (j >= rankData.Data.PetAwakeLevel.Count) ? 0 : rankData.Data.PetAwakeLevel[j]);
					}
					while (j < 3)
					{
						this.mPets[j].Refresh(0, 0);
						j++;
					}
				}
			}
			else
			{
				for (int k = 0; k < 3; k++)
				{
					this.mPets[k].Refresh(0, 0);
				}
			}
			this.mRankIcon.gameObject.SetActive(true);
			int specialNo = this.GetSpecialNo();
			if ((this.mBaseScene != null && specialNo <= this.mBaseScene.specialNum) || (this.mBaseLayer != null && specialNo <= this.mBaseLayer.specialNum) || (this.mBasePop != null && specialNo <= this.mBasePop.specialNum))
			{
				this.mRankTxt.gameObject.SetActive(false);
				this.mRankSprite.gameObject.SetActive(true);
				this.mRankSprite.spriteName = this.GetRankSprite();
				this.mOutlineSprite.spriteName = string.Format("{0}_bg", this.mRankSprite.spriteName);
				this.mOutlineSprite.gameObject.SetActive(true);
			}
			else
			{
				this.mRankTxt.gameObject.SetActive(true);
				this.mRankSprite.gameObject.SetActive(false);
				this.mRankTxt.text = this.GetRank().ToString();
				this.mOutlineSprite.gameObject.SetActive(false);
			}
			this.mLvlName.text = this.GetLvlName();
			this.mScore.text = this.GetScore();
			if (!string.IsNullOrEmpty(this.mScore.text))
			{
				this.mLvlName.transform.localPosition = new Vector3((float)this.nameXValue, 16f, 0f);
			}
			else
			{
				this.mLvlName.transform.localPosition = new Vector3((float)this.nameXValue, 0f, 0f);
			}
			this.mScore.transform.localPosition = new Vector3((float)this.nameXValue, -19f, 0f);
			int diamond = this.GetDiamond();
			if (diamond > 0)
			{
				this.mDiamondNum.text = diamond.ToString();
				this.mDiamondNum.gameObject.SetActive(true);
			}
			else
			{
				this.mDiamondNum.gameObject.SetActive(false);
			}
		}
	}

	public void OnRankItemClick(GameObject go)
	{
		if (this.mUserData.userData is RankData)
		{
			RankData rankData = (RankData)this.mUserData.userData;
			if (rankData != null)
			{
				GUIPlayerInfoPopUp.Show(rankData);
			}
		}
	}

	protected void CreateItemIcon(bool gold)
	{
		if (gold)
		{
			if (this.mGoldItemIcon == null)
			{
				this.mGoldItemIcon = CommonIconItem.Create(this.mGoldNum.gameObject, new Vector3(3f, 5f, 0f), null, false, 0.35f, null);
			}
		}
		else if (this.mDiamondItemIcon == null)
		{
			this.mDiamondItemIcon = CommonIconItem.Create(this.mDiamondNum.gameObject, new Vector3(3f, 5f, 0f), null, false, 0.35f, null);
		}
	}
}
