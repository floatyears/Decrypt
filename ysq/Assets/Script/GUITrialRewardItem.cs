using Att;
using System;
using UnityEngine;

public class GUITrialRewardItem : UICustomGridItem
{
	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mNum;

	private UISprite mDaiBiIcon;

	private GUITrialRewardItemData mItemRewardData;

	private GameUIToolTip mToolTips;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mIcon.gameObject.SetActive(false);
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mNum = base.transform.Find("num").GetComponent<UILabel>();
		this.mDaiBiIcon = base.transform.Find("daiBiIcon").GetComponent<UISprite>();
		this.mDaiBiIcon.gameObject.SetActive(false);
		UIEventListener expr_99 = UIEventListener.Get(base.gameObject);
		expr_99.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_99.onPress, new UIEventListener.BoolDelegate(this.OnRewardPress));
	}

	public override void Refresh(object data)
	{
		if (this.mItemRewardData != data)
		{
			this.mItemRewardData = (GUITrialRewardItemData)data;
			this.Refresh();
		}
	}

	private void Refresh()
	{
		if (this.mItemRewardData != null)
		{
			if (this.mItemRewardData.mItemInfo != null)
			{
				this.mIcon.gameObject.SetActive(true);
				this.mDaiBiIcon.gameObject.SetActive(false);
				this.mIcon.spriteName = this.mItemRewardData.mItemInfo.Icon;
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemRewardData.mItemInfo.Quality);
				this.mNum.text = this.mItemRewardData.mItemNum.ToString();
			}
			else
			{
				this.mIcon.gameObject.SetActive(false);
				this.mDaiBiIcon.gameObject.SetActive(true);
				if (this.mItemRewardData.mERewardType == ERewardType.EReward_Emblem)
				{
					this.mDaiBiIcon.spriteName = "emblem";
				}
				else if (this.mItemRewardData.mERewardType == ERewardType.EReward_LopetSoul)
				{
					this.mDaiBiIcon.spriteName = "lopetSoul";
				}
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(3);
				this.mNum.text = this.mItemRewardData.mDaiBiNum.ToString();
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnRewardPress(GameObject go, bool isPressed)
	{
		if (this.mItemRewardData != null && this.mItemRewardData.mItemInfo != null)
		{
			if (isPressed)
			{
				if (this.mToolTips == null)
				{
					this.mToolTips = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
				}
				this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mItemRewardData.mItemInfo.Name, this.mItemRewardData.mItemInfo.Desc, this.mItemRewardData.mItemInfo.Quality);
				this.mToolTips.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(0f, 80f, -6000f));
				this.mToolTips.EnableToolTip();
			}
			else if (this.mToolTips != null)
			{
				this.mToolTips.HideTipAnim();
			}
		}
	}
}
