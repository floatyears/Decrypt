using Att;
using Proto;
using System;
using UnityEngine;

public class CommonIconItem : MonoBehaviour
{
	private enum ECIIT
	{
		ECIIT_Item,
		ECIIT_Pet,
		ECIIT_Reward,
		ECIIT_Fashion,
		ECIIT_Lopet
	}

	public delegate void VoidCallBack(GameObject go);

	public delegate void ItemInfoCallBack(ItemInfo info);

	public CommonIconItem.VoidCallBack OnIconClickEvent;

	public CommonIconItem.VoidCallBack OnMinusClickEvent;

	public CommonIconItem.ItemInfoCallBack OnItemIconClickEvent;

	private ItemInfo mItemInfo;

	private PetInfo mPetInfo;

	private PetDataEx mPetData;

	private RewardData mRewardData;

	private FashionInfo mFashionInfo;

	private LopetDataEx mLopetData;

	private LopetInfo mLopetInfo;

	private UISprite mItemIcon;

	private UISprite mPetIcon;

	private UISprite mAvatarIcon;

	private UISprite mLopetIcon;

	private UISprite mQualityMark;

	private UISprite mGrayMask;

	private UILabel mName;

	private UILabel mNum;

	private UISprite mMinus;

	private UISprite mFragmentMask;

	private UISprite mLeftTopTag;

	private UILabel mLeftTopTagLb;

	private bool showName;

	private bool showNum;

	private bool showMinus;

	private bool showItemInfo;

	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
		set
		{
			if (base.gameObject.activeInHierarchy != value)
			{
				NGUITools.SetActive(base.gameObject, value);
			}
		}
	}

	public bool SetMask
	{
		set
		{
			if (this.mGrayMask != null)
			{
				this.mGrayMask.enabled = value;
			}
		}
	}

	public bool ShowItemInfo
	{
		get
		{
			return this.showItemInfo;
		}
		set
		{
			this.showItemInfo = value;
		}
	}

	public bool EnableLeftTopTag
	{
		get
		{
			return this.mLeftTopTag.gameObject.activeSelf;
		}
		set
		{
			this.mLeftTopTag.gameObject.SetActive(value);
		}
	}

	private void Init()
	{
		UIEventListener expr_0B = UIEventListener.Get(base.gameObject);
		expr_0B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_0B.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mItemIcon = GameUITools.FindUISprite("ItemIcon", base.gameObject);
		this.mPetIcon = GameUITools.FindUISprite("PetIcon", base.gameObject);
		this.mAvatarIcon = GameUITools.FindUISprite("AvatarIcon", base.gameObject);
		this.mLopetIcon = GameUITools.FindUISprite("LopetIcon", base.gameObject);
		this.mQualityMark = GameUITools.FindUISprite("QualityMark", base.gameObject);
		this.mGrayMask = GameUITools.FindUISprite("GrayMask", base.gameObject);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mNum = GameUITools.FindUILabel("Num", base.gameObject);
		this.mMinus = GameUITools.RegisterClickEvent("Minus", new UIEventListener.VoidDelegate(this.OnMinusClick), base.gameObject).GetComponent<UISprite>();
		this.mFragmentMask = GameUITools.FindUISprite("FragmentMask", base.gameObject);
		this.mFragmentMask.enabled = false;
		this.mLeftTopTag = GameUITools.FindUISprite("LeftTopTag", base.gameObject);
		this.mLeftTopTagLb = GameUITools.FindUILabel("Label", this.mLeftTopTag.gameObject);
		this.mLeftTopTag.gameObject.SetActive(false);
	}

	public static CommonIconItem Create(GameObject parent, Vector3 pos, CommonIconItem.VoidCallBack IconClick, bool enableTouch = true, float scale = 1f, CommonIconItem.VoidCallBack MinusClick = null)
	{
		GameObject gameObject = Res.LoadGUI("GUI/CommonIconItem");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/CommonIconItem error"
			});
			return null;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(parent, gameObject2);
		gameObject2.transform.localPosition = pos;
		gameObject2.transform.localScale = new Vector3(scale, scale, 1f);
		CommonIconItem commonIconItem = gameObject2.AddComponent<CommonIconItem>();
		commonIconItem.Init();
		if (enableTouch)
		{
			if (IconClick != null)
			{
				CommonIconItem expr_85 = commonIconItem;
				expr_85.OnIconClickEvent = (CommonIconItem.VoidCallBack)Delegate.Combine(expr_85.OnIconClickEvent, IconClick);
			}
		}
		else
		{
			commonIconItem.collider.enabled = false;
		}
		if (MinusClick != null)
		{
			CommonIconItem expr_B4 = commonIconItem;
			expr_B4.OnMinusClickEvent = (CommonIconItem.VoidCallBack)Delegate.Combine(expr_B4.OnMinusClickEvent, MinusClick);
		}
		return commonIconItem;
	}

	private void ClearData()
	{
		this.mItemInfo = null;
		this.mPetInfo = null;
		this.mPetData = null;
		this.mRewardData = null;
		this.mFashionInfo = null;
	}

	public void Refresh(ItemDataEx data, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mItemInfo == data.Info)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mItemInfo = data.Info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Item);
	}

	public void Refresh(ItemInfo info, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mItemInfo == info)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mItemInfo = info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Item);
	}

	public void Refresh(PetDataEx data, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mPetInfo == data.Info)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mPetData = data;
		this.mPetInfo = data.Info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Pet);
	}

	public void Refresh(PetInfo info, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mPetInfo == info)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mPetInfo = info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Pet);
	}

	public void Refresh(LopetDataEx data, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mLopetData == data)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mLopetData = data;
		this.mLopetInfo = data.Info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Lopet);
	}

	public void Refresh(LopetInfo info, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mLopetInfo == info)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mLopetInfo = info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Lopet);
	}

	public void Refresh(RewardData data, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mRewardData == data)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mRewardData = data;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Reward);
	}

	public void Refresh(FashionInfo info, bool showName = false, bool showNum = false, bool showMinus = false)
	{
		if (this.mFashionInfo == info)
		{
			return;
		}
		this.InitBool(showName, showNum, showMinus);
		this.mFashionInfo = info;
		this.SetType(CommonIconItem.ECIIT.ECIIT_Fashion);
	}

	private void InitBool(bool name, bool num, bool minus)
	{
		this.ClearData();
		this.showName = name;
		this.showNum = num;
		this.showMinus = minus;
	}

	private void SetType(CommonIconItem.ECIIT type)
	{
		this.mMinus.enabled = this.showMinus;
		this.mItemIcon.enabled = false;
		this.mPetIcon.enabled = false;
		this.mAvatarIcon.enabled = false;
		this.mLopetIcon.enabled = false;
		switch (type)
		{
		case CommonIconItem.ECIIT.ECIIT_Item:
			this.mItemIcon.enabled = true;
			this.RefreshItem();
			break;
		case CommonIconItem.ECIIT.ECIIT_Pet:
			this.mPetIcon.enabled = true;
			this.RefreshPet();
			break;
		case CommonIconItem.ECIIT.ECIIT_Reward:
			this.mItemIcon.enabled = true;
			this.RefreshReward();
			break;
		case CommonIconItem.ECIIT.ECIIT_Fashion:
			this.mAvatarIcon.enabled = true;
			this.RefreshFashion();
			break;
		case CommonIconItem.ECIIT.ECIIT_Lopet:
			this.mLopetIcon.enabled = true;
			this.RefreshLopet();
			break;
		}
	}

	private void RefreshReward()
	{
		if (this.mRewardData == null)
		{
			return;
		}
		if (this.showName)
		{
			this.mName.enabled = true;
			this.mName.color = Tools.GetItemQualityColor(0);
		}
		else
		{
			this.mName.enabled = false;
		}
		if (this.showNum)
		{
			this.mNum.enabled = true;
		}
		else
		{
			this.mNum.enabled = false;
		}
		ERewardType rewardType = (ERewardType)this.mRewardData.RewardType;
		switch (rewardType)
		{
		case ERewardType.EReward_Money:
		case ERewardType.EReward_Diamond:
		case ERewardType.EReward_Energy:
		case ERewardType.EReward_Exp:
		case ERewardType.EReward_GuildRepution:
		case ERewardType.EReward_MagicCrystal:
		case ERewardType.EReward_MagicSoul:
		case ERewardType.EReward_FireDragonScale:
		case ERewardType.EReward_KingMedal:
		case ERewardType.EReward_StarSoul:
		case ERewardType.EReward_Honor:
		case ERewardType.EReward_Emblem:
		case ERewardType.EReward_LopetSoul:
		case ERewardType.EReward_FestivalVoucher:
			this.mQualityMark.spriteName = Tools.GetRewardFrame(rewardType);
			this.mItemIcon.spriteName = Tools.GetRewardIcon(rewardType);
			this.mNum.text = Tools.FormatValue(this.mRewardData.RewardValue1);
			this.mName.text = Tools.GetRewardTypeName(rewardType, 0);
			this.mName.color = Tools.GetRewardNameColor(rewardType);
			break;
		case ERewardType.EReward_Item:
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(this.mRewardData.RewardValue1);
			if (info == null)
			{
				global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
				{
					this.mRewardData.RewardValue1
				});
				return;
			}
			this.mItemInfo = info;
			this.SetType(CommonIconItem.ECIIT.ECIIT_Item);
			this.SetNum((!this.showNum) ? 0 : this.mRewardData.RewardValue2);
			break;
		}
		case ERewardType.EReward_Pet:
		{
			PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(this.mRewardData.RewardValue1);
			if (info2 == null)
			{
				global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
				{
					this.mRewardData.RewardValue1
				});
				return;
			}
			this.mPetInfo = info2;
			this.SetType(CommonIconItem.ECIIT.ECIIT_Pet);
			this.SetNum((!this.showNum) ? 0 : this.mRewardData.RewardValue2);
			break;
		}
		case ERewardType.EReward_Fashion:
		{
			FashionInfo info3 = Globals.Instance.AttDB.FashionDict.GetInfo(this.mRewardData.RewardValue1);
			if (info3 == null)
			{
				global::Debug.LogErrorFormat("FashionDict.GetInfo error, id = {0}", new object[]
				{
					this.mRewardData.RewardValue1
				});
				return;
			}
			this.mFashionInfo = info3;
			this.SetType(CommonIconItem.ECIIT.ECIIT_Fashion);
			this.SetNum((!this.showNum) ? 0 : this.mRewardData.RewardValue2);
			break;
		}
		case ERewardType.EReward_Lopet:
		{
			LopetInfo info4 = Globals.Instance.AttDB.LopetDict.GetInfo(this.mRewardData.RewardValue1);
			if (info4 == null)
			{
				global::Debug.LogErrorFormat("LopetDict.GetInfo error, id = {0}", new object[]
				{
					this.mRewardData.RewardValue1
				});
				return;
			}
			this.mLopetInfo = info4;
			this.SetType(CommonIconItem.ECIIT.ECIIT_Lopet);
			this.SetNum((!this.showNum) ? 0 : this.mRewardData.RewardValue2);
			break;
		}
		}
	}

	private void RefreshFashion()
	{
		if (this.mFashionInfo == null)
		{
			return;
		}
		this.mAvatarIcon.spriteName = this.mFashionInfo.Icon;
		this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mFashionInfo.Quality);
		if (this.showName)
		{
			this.mName.enabled = true;
			this.mName.text = this.mFashionInfo.Name;
			this.mName.color = Tools.GetItemQualityColor(this.mFashionInfo.Quality);
		}
		else
		{
			this.mName.enabled = false;
		}
		if (this.showNum)
		{
			this.mNum.enabled = true;
			this.mNum.text = ((!Globals.Instance.Player.ItemSystem.HasFashion(this.mFashionInfo.ID)) ? "0" : "1");
		}
		else
		{
			this.mNum.enabled = false;
		}
	}

	private void RefreshPet()
	{
		if (this.mPetInfo == null)
		{
			return;
		}
		this.mPetIcon.spriteName = this.mPetInfo.Icon;
		this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mPetInfo.Quality);
		if (this.showName)
		{
			this.mName.enabled = true;
			this.mName.text = ((!string.IsNullOrEmpty(this.mPetInfo.FirstName)) ? this.mPetInfo.FirstName : this.mPetInfo.Name);
			this.mName.color = Tools.GetItemQualityColor(this.mPetInfo.Quality);
		}
		else
		{
			this.mName.enabled = false;
		}
		if (this.showNum && this.mPetData != null)
		{
			this.mNum.enabled = true;
			this.mNum.text = "1";
		}
		else
		{
			this.mNum.enabled = false;
		}
	}

	private void RefreshLopet()
	{
		if (this.mLopetInfo == null)
		{
			return;
		}
		this.mLopetIcon.spriteName = this.mLopetInfo.Icon;
		this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mLopetInfo.Quality);
		if (this.showName)
		{
			this.mName.enabled = true;
			this.mName.text = this.mLopetInfo.Name;
			this.mName.color = Tools.GetItemQualityColor(this.mLopetInfo.Quality);
		}
		else
		{
			this.mName.enabled = false;
		}
		if (this.showNum && this.mLopetData != null)
		{
			this.mNum.enabled = true;
			this.mNum.text = "1";
		}
		else
		{
			this.mNum.enabled = false;
		}
	}

	private void RefreshItem()
	{
		if (this.mItemInfo == null)
		{
			return;
		}
		this.mItemIcon.spriteName = this.mItemInfo.Icon;
		this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
		if (this.mItemInfo.Type == 3)
		{
			this.mFragmentMask.enabled = true;
			if (this.mItemInfo.SubType == 0)
			{
				this.mPetInfo = Globals.Instance.AttDB.PetDict.GetInfo(this.mItemInfo.Value2);
				if (this.mPetInfo == null)
				{
					global::Debug.LogErrorFormat("PetDict get info error , ID : {0}", new object[]
					{
						this.mItemInfo.Value2
					});
					return;
				}
				this.mItemIcon.enabled = false;
				this.mLopetIcon.enabled = false;
				this.mPetIcon.enabled = true;
				this.mPetIcon.spriteName = this.mPetInfo.Icon;
				this.mFragmentMask.spriteName = "frag";
			}
			else if (this.mItemInfo.SubType == 3)
			{
				this.mLopetInfo = Globals.Instance.AttDB.LopetDict.GetInfo(this.mItemInfo.Value2);
				if (this.mLopetInfo == null)
				{
					global::Debug.LogErrorFormat("LopetDict get info error , ID : {0}", new object[]
					{
						this.mItemInfo.Value2
					});
					return;
				}
				this.mItemIcon.enabled = false;
				this.mPetIcon.enabled = false;
				this.mLopetIcon.enabled = true;
				this.mLopetIcon.spriteName = this.mLopetInfo.Icon;
				this.mFragmentMask.spriteName = "frag";
			}
			else
			{
				this.mFragmentMask.spriteName = "frag2";
			}
		}
		else
		{
			this.mFragmentMask.enabled = false;
		}
		if (this.showName)
		{
			this.mName.enabled = true;
			this.mName.text = this.mItemInfo.Name;
			this.mName.color = Tools.GetItemQualityColor(this.mItemInfo.Quality);
		}
		else
		{
			this.mName.enabled = false;
		}
		int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID);
		if (this.showNum && itemCount > 0)
		{
			this.mNum.enabled = true;
			this.mNum.text = Tools.FormatValue(itemCount);
		}
		else
		{
			this.mNum.enabled = false;
		}
	}

	public void SetNum(int num)
	{
		if (num > 0)
		{
			this.mNum.enabled = true;
			this.mNum.text = Tools.FormatValue(num);
		}
		else
		{
			this.mNum.enabled = false;
		}
	}

	public void SetNeedNum(int curNum, int needNum)
	{
		if (needNum > 0)
		{
			this.mNum.enabled = true;
			this.mNum.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				Tools.FormatValue(curNum),
				needNum
			});
			if (curNum < needNum)
			{
				this.mNum.color = Color.red;
			}
			else
			{
				this.mNum.color = Color.white;
			}
		}
		else
		{
			this.mNum.enabled = false;
		}
	}

	public CommonIconItem SetNameStyle(int wordNum, UILabel.Overflow method)
	{
		this.mName.overflowMethod = method;
		this.mName.width = wordNum * this.mName.fontSize;
		return this;
	}

	public CommonIconItem SetNumStyle(int size)
	{
		this.mNum.fontSize = size;
		return this;
	}

	public void SetLeftTopTag(string str)
	{
		this.mLeftTopTagLb.text = str;
	}

	private void OnIconClick(GameObject go)
	{
		if (this.showItemInfo)
		{
			if (this.mItemInfo != null)
			{
				GameUIManager.mInstance.ShowItemInfo(this.mItemInfo);
			}
			else if (this.mPetInfo != null)
			{
				GameUIManager.mInstance.ShowPetInfo(this.mPetInfo);
			}
			else if (this.mLopetInfo != null)
			{
				GameUIManager.mInstance.ShowLopetInfo(this.mLopetInfo);
			}
		}
		else
		{
			if (this.OnIconClickEvent != null)
			{
				this.OnIconClickEvent(go);
			}
			if (this.OnItemIconClickEvent != null)
			{
				this.OnItemIconClickEvent(this.mItemInfo);
			}
		}
	}

	private void OnMinusClick(GameObject go)
	{
		if (this.OnMinusClickEvent != null)
		{
			this.OnMinusClickEvent(go);
		}
	}
}
