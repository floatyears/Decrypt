using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIGuildLootItemBase : MonoBehaviour
{
	private UISprite mItemIcon;

	private UISprite mItemQuality;

	private UILabel mItemNum;

	private RewardData mLootData;

	private ItemInfo mItemInfo;

	private GameUIToolTip mToolTips;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene(RewardData lootData)
	{
		this.mLootData = lootData;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mItemIcon = base.transform.GetComponent<UISprite>();
		UIEventListener.Get(this.mItemIcon.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnRewardPress);
		this.mItemQuality = this.mItemIcon.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mItemNum = this.mItemIcon.transform.Find("num").GetComponent<UILabel>();
	}

	private void Refresh()
	{
		if (this.mLootData != null)
		{
			if (this.mLootData.RewardType == 1)
			{
				this.mItemIcon.spriteName = "M101";
				this.mItemQuality.spriteName = Tools.GetItemQualityIcon(0);
				this.mItemNum.text = this.mLootData.RewardValue1.ToString();
				this.mItemInfo = null;
			}
			else if (this.mLootData.RewardType == 2)
			{
				this.mItemIcon.spriteName = "M102";
				this.mItemQuality.spriteName = Tools.GetItemQualityIcon(2);
				this.mItemNum.text = this.mLootData.RewardValue1.ToString();
				this.mItemInfo = null;
			}
			else if (this.mLootData.RewardType == 3)
			{
				this.mItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(this.mLootData.RewardValue1);
				if (this.mItemInfo != null)
				{
					this.mItemIcon.spriteName = this.mItemInfo.Icon;
					this.mItemQuality.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
					this.mItemNum.text = this.mLootData.RewardValue2.ToString();
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
				else if (this.mLootData.RewardType == 1)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(this.mLootData.RewardValue1).Append(Singleton<StringManager>.Instance.GetString("money"));
					this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mSb.ToString(), string.Format(Singleton<StringManager>.Instance.GetString("takeMoney"), this.mLootData.RewardValue1));
				}
				else if (this.mLootData.RewardType == 2)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(this.mLootData.RewardValue1).Append(Singleton<StringManager>.Instance.GetString("diamond"));
					this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mSb.ToString(), string.Format(Singleton<StringManager>.Instance.GetString("takeDiamond"), this.mLootData.RewardValue1));
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
