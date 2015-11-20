using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class MailItem : UICustomGridItem
{
	private GUIMailScene mBaseScene;

	private UILabel mMailTitle;

	private UILabel mMailSender;

	private UILabel mMailTime;

	private Transform mReward;

	private UISprite mItemIcon;

	private UISprite mPetIcon;

	private UISprite mLopetIcon;

	private UISprite mQualityMark;

	private UISprite mFlag;

	private UISprite mMailFlag;

	private UISprite mFashionIcon;

	private UISprite mMailBg;

	private StringBuilder mStringBuilder = new StringBuilder();

	public MailItemData mMailData
	{
		get;
		private set;
	}

	public void InitItem(GUIMailScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameUITools.UpdateUIBoxCollider(base.transform, 4f, false);
		this.mMailTitle = base.transform.Find("mailTitle").GetComponent<UILabel>();
		this.mMailSender = base.transform.Find("mailSender").GetComponent<UILabel>();
		this.mMailTime = base.transform.Find("mailTime").GetComponent<UILabel>();
		this.mMailBg = base.transform.GetComponent<UISprite>();
		this.mReward = base.transform.Find("Reward");
		this.mItemIcon = this.mReward.Find("ItemIcon").GetComponent<UISprite>();
		this.mPetIcon = this.mReward.Find("PetIcon").GetComponent<UISprite>();
		this.mLopetIcon = this.mReward.Find("LopetIcon").GetComponent<UISprite>();
		this.mQualityMark = this.mReward.Find("QualityMark").GetComponent<UISprite>();
		this.mFlag = this.mReward.Find("Flag").GetComponent<UISprite>();
		this.mMailFlag = this.mReward.Find("MailFlag").GetComponent<UISprite>();
		this.mFashionIcon = this.mReward.Find("Fashion").GetComponent<UISprite>();
		UIEventListener expr_151 = UIEventListener.Get(base.gameObject);
		expr_151.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_151.onClick, new UIEventListener.VoidDelegate(this.OnMailItemClick));
	}

	private bool HasAffix(EAffixType affixType)
	{
		bool result = false;
		if (this.mMailData != null && this.mMailData.mMailData != null)
		{
			for (int i = 0; i < this.mMailData.mMailData.AffixType.Count; i++)
			{
				if (this.mMailData.mMailData.AffixType[i] == (int)affixType)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public override void Refresh(object data)
	{
		if (this.mMailData == data)
		{
			return;
		}
		this.mMailData = (MailItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mMailData != null && this.mMailData.mMailData != null)
		{
			base.gameObject.SetActive(true);
			this.mMailTitle.text = this.mMailData.mMailData.Title;
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
			this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("mailSender")).Append(this.mMailData.mMailData.Sender);
			this.mMailSender.text = this.mStringBuilder.ToString();
			this.mMailTime.text = Tools.ServerDateTimeFormat2(this.mMailData.mMailData.TimeStamp);
			bool flag = MailContentsUITable.IsReadMailFlag(this.mMailData.mMailData);
			this.mMailBg.spriteName = ((!flag) ? "Price_bg" : "mail_bg");
			if (this.mMailData.mMailData.AffixType.Count == 0)
			{
				this.mItemIcon.gameObject.SetActive(false);
				this.mPetIcon.gameObject.SetActive(false);
				this.mLopetIcon.gameObject.SetActive(false);
				this.mQualityMark.gameObject.SetActive(false);
				this.mFashionIcon.gameObject.SetActive(false);
				this.mFlag.gameObject.SetActive(false);
				this.mMailFlag.gameObject.SetActive(true);
				this.mMailFlag.spriteName = ((!flag) ? "email" : "mailReaded");
			}
			else
			{
				this.mMailFlag.gameObject.SetActive(false);
				ItemInfo itemInfo = null;
				PetInfo petInfo = null;
				LopetInfo lopetInfo = null;
				FashionInfo fashionInfo = null;
				int num = -1;
				for (int i = 0; i < this.mMailData.mMailData.AffixType.Count; i++)
				{
					if (this.mMailData.mMailData.AffixType[i] == 2)
					{
						ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(this.mMailData.mMailData.AffixValue1[i]);
						int num2 = info.Quality * 1000;
						if (num2 > num)
						{
							itemInfo = info;
							petInfo = null;
							lopetInfo = null;
							fashionInfo = null;
							num = num2;
						}
					}
					else if (this.mMailData.mMailData.AffixType[i] == 3)
					{
						PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(this.mMailData.mMailData.AffixValue1[i]);
						int num3 = info2.Quality * 10000;
						if (num3 > num)
						{
							itemInfo = null;
							fashionInfo = null;
							petInfo = info2;
							lopetInfo = null;
							num = num3;
						}
					}
					else if (this.mMailData.mMailData.AffixType[i] == 15)
					{
						LopetInfo info3 = Globals.Instance.AttDB.LopetDict.GetInfo(this.mMailData.mMailData.AffixValue1[i]);
						int num4 = info3.Quality * 10000;
						if (num4 > num)
						{
							itemInfo = null;
							fashionInfo = null;
							petInfo = null;
							lopetInfo = info3;
							num = num4;
						}
					}
					else if (this.mMailData.mMailData.AffixType[i] == 12)
					{
						FashionInfo info4 = Globals.Instance.AttDB.FashionDict.GetInfo(this.mMailData.mMailData.AffixValue1[i]);
						int num5 = info4.Quality * 50000;
						if (num5 > num)
						{
							itemInfo = null;
							petInfo = null;
							lopetInfo = null;
							fashionInfo = info4;
							break;
						}
					}
				}
				if (fashionInfo != null)
				{
					this.mItemIcon.gameObject.SetActive(false);
					this.mPetIcon.gameObject.SetActive(false);
					this.mLopetIcon.gameObject.SetActive(false);
					this.mQualityMark.gameObject.SetActive(true);
					this.mFashionIcon.gameObject.SetActive(true);
					this.mFlag.gameObject.SetActive(false);
					this.mFashionIcon.spriteName = fashionInfo.Icon;
					this.mQualityMark.spriteName = Tools.GetItemQualityIcon(fashionInfo.Quality);
				}
				else if (petInfo != null)
				{
					this.mItemIcon.gameObject.SetActive(false);
					this.mPetIcon.gameObject.SetActive(true);
					this.mLopetIcon.gameObject.SetActive(false);
					this.mQualityMark.gameObject.SetActive(true);
					this.mFashionIcon.gameObject.SetActive(false);
					this.mFlag.gameObject.SetActive(false);
					this.mPetIcon.spriteName = petInfo.Icon;
					this.mQualityMark.spriteName = Tools.GetItemQualityIcon(petInfo.Quality);
				}
				else if (lopetInfo != null)
				{
					this.mItemIcon.gameObject.SetActive(false);
					this.mPetIcon.gameObject.SetActive(false);
					this.mLopetIcon.gameObject.SetActive(true);
					this.mQualityMark.gameObject.SetActive(true);
					this.mFashionIcon.gameObject.SetActive(false);
					this.mFlag.gameObject.SetActive(false);
					this.mLopetIcon.spriteName = lopetInfo.Icon;
					this.mQualityMark.spriteName = Tools.GetItemQualityIcon(lopetInfo.Quality);
				}
				else if (itemInfo != null)
				{
					if (itemInfo.Type == 3 && itemInfo.SubType == 0)
					{
						PetInfo info5 = Globals.Instance.AttDB.PetDict.GetInfo(itemInfo.Value2);
						this.mItemIcon.gameObject.SetActive(false);
						this.mPetIcon.gameObject.SetActive(true);
						this.mLopetIcon.gameObject.SetActive(false);
						this.mQualityMark.gameObject.SetActive(true);
						this.mFashionIcon.gameObject.SetActive(false);
						this.mFlag.gameObject.SetActive(true);
						this.mPetIcon.spriteName = info5.Icon;
						this.mQualityMark.spriteName = Tools.GetItemQualityIcon(info5.Quality);
					}
					else
					{
						this.mItemIcon.gameObject.SetActive(true);
						this.mPetIcon.gameObject.SetActive(false);
						this.mLopetIcon.gameObject.SetActive(false);
						this.mFashionIcon.gameObject.SetActive(false);
						this.mQualityMark.gameObject.SetActive(true);
						this.mFlag.gameObject.SetActive(itemInfo.Type == 3);
						this.mItemIcon.spriteName = itemInfo.Icon;
						this.mQualityMark.spriteName = Tools.GetItemQualityIcon(itemInfo.Quality);
					}
				}
				else
				{
					this.mItemIcon.gameObject.SetActive(true);
					this.mPetIcon.gameObject.SetActive(false);
					this.mLopetIcon.gameObject.SetActive(false);
					this.mFashionIcon.gameObject.SetActive(false);
					this.mQualityMark.gameObject.SetActive(false);
					this.mFlag.gameObject.SetActive(false);
					switch (this.mMailData.mMailData.AffixType[0])
					{
					case 0:
						this.mItemIcon.spriteName = "M101";
						goto IL_921;
					case 1:
						this.mItemIcon.spriteName = "M102";
						goto IL_921;
					case 4:
						this.mItemIcon.spriteName = "M106";
						goto IL_921;
					case 5:
						this.mItemIcon.spriteName = "M103";
						goto IL_921;
					case 6:
						this.mItemIcon.spriteName = "I110";
						goto IL_921;
					case 7:
						this.mItemIcon.spriteName = "exp";
						goto IL_921;
					case 8:
						this.mItemIcon.spriteName = "M107";
						goto IL_921;
					case 9:
						this.mItemIcon.spriteName = "M105";
						goto IL_921;
					case 10:
						this.mItemIcon.spriteName = "M109";
						goto IL_921;
					case 11:
						this.mItemIcon.spriteName = "M108";
						goto IL_921;
					case 13:
						this.mItemIcon.spriteName = "I118";
						goto IL_921;
					case 14:
						this.mItemIcon.spriteName = "M110";
						goto IL_921;
					case 16:
						this.mItemIcon.spriteName = "M111";
						goto IL_921;
					case 17:
						this.mItemIcon.spriteName = "M112";
						goto IL_921;
					}
					global::Debug.LogErrorFormat("Error icon config {0}", new object[]
					{
						this.mMailData.mMailData.AffixType[0]
					});
				}
			}
			IL_921:;
		}
		else
		{
			this.mMailTitle.text = string.Empty;
			this.mMailSender.text = string.Empty;
			this.mMailTime.text = string.Empty;
			base.gameObject.SetActive(false);
		}
	}

	private void OnMailItemClick(GameObject go)
	{
		if (!this.mMailData.mMailData.Read)
		{
			Globals.Instance.Player.SetMailRead(this.mMailData.mMailData);
			GUIMainMenuScene session = GameUIManager.mInstance.GetSession<GUIMainMenuScene>();
			if (session != null)
			{
				session.UpdateUnreadMailFlag();
			}
		}
		this.mBaseScene.ReInitMailItems();
		this.mBaseScene.ShowMailDetailInfoLayer(this.mMailData.mMailData);
	}
}
