using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIGuildLootItem : UICustomGridItem
{
	private UISprite mItemIcon;

	private UISprite mItemQuality;

	private UILabel mItemName;

	private UILabel mItemNum;

	private GUIGuildLootItemData mLootData;

	private ItemInfo mItemInfo;

	private GameUIToolTip mToolTips;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mItemIcon = base.transform.Find("icon").GetComponent<UISprite>();
		UIEventListener.Get(this.mItemIcon.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnRewardPress);
		Transform transform = this.mItemIcon.transform;
		this.mItemQuality = transform.Find("QualityMark").GetComponent<UISprite>();
		this.mItemName = transform.Find("name").GetComponent<UILabel>();
		this.mItemNum = transform.Find("num").GetComponent<UILabel>();
		GameUITools.UpdateUIBoxCollider(base.transform, 4f, false);
	}

	public override void Refresh(object data)
	{
		if (this.mLootData == data)
		{
			return;
		}
		this.mLootData = (GUIGuildLootItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mLootData != null && this.mLootData.mRewardData != null)
		{
			if (this.mLootData.mRewardData.RewardType == 1)
			{
				this.mItemIcon.spriteName = "M101";
				this.mItemQuality.spriteName = Tools.GetItemQualityIcon(0);
				this.mItemName.text = Singleton<StringManager>.Instance.GetString("money");
				this.mItemName.color = Color.white;
				this.mItemNum.text = this.mLootData.mRewardData.RewardValue1.ToString();
				this.mItemInfo = null;
			}
			else if (this.mLootData.mRewardData.RewardType == 2)
			{
				this.mItemIcon.spriteName = "M102";
				this.mItemQuality.spriteName = Tools.GetItemQualityIcon(2);
				this.mItemName.text = Singleton<StringManager>.Instance.GetString("diamond");
				this.mItemName.color = Color.white;
				this.mItemNum.text = this.mLootData.mRewardData.RewardValue1.ToString();
				this.mItemInfo = null;
			}
			else if (this.mLootData.mRewardData.RewardType == 3)
			{
				this.mItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(this.mLootData.mRewardData.RewardValue1);
				if (this.mItemInfo != null)
				{
					this.mItemIcon.spriteName = this.mItemInfo.Icon;
					this.mItemQuality.gameObject.SetActive(true);
					this.mItemQuality.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
					this.mItemName.text = this.mItemInfo.Name;
					this.mItemName.color = Tools.GetItemQualityColor(this.mItemInfo.Quality);
					this.mItemNum.text = this.mLootData.mRewardData.RewardValue2.ToString();
				}
			}
		}
	}

	private void OnRewardPress(GameObject go, bool isPressed)
	{
		if (this.mLootData != null)
		{
			if (isPressed)
			{
				if (this.mToolTips == null)
				{
					this.mToolTips = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
				}
				if (this.mItemInfo != null)
				{
					this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mItemInfo.Name, this.mItemInfo.Desc, this.mItemInfo.Quality);
				}
				else if (this.mLootData.mRewardData.RewardType == 1)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(this.mLootData.mRewardData.RewardValue1).Append(Singleton<StringManager>.Instance.GetString("money"));
					this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mSb.ToString(), string.Format(Singleton<StringManager>.Instance.GetString("takeMoney"), this.mLootData.mRewardData.RewardValue1));
				}
				else if (this.mLootData.mRewardData.RewardType == 2)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(this.mLootData.mRewardData.RewardValue1).Append(Singleton<StringManager>.Instance.GetString("diamond"));
					this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mSb.ToString(), string.Format(Singleton<StringManager>.Instance.GetString("takeDiamond"), this.mLootData.mRewardData.RewardValue1));
				}
				this.mToolTips.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(0f, 100f, -5000f));
				this.mToolTips.EnableToolTip();
			}
			else if (this.mToolTips != null)
			{
				this.mToolTips.HideTipAnim();
			}
		}
	}
}
