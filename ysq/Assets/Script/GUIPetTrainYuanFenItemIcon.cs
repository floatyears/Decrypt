using Att;
using System;
using UnityEngine;

public class GUIPetTrainYuanFenItemIcon : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private PetInfo mPetInfo;

	private ItemInfo mItemInfo;

	private GameUIToolTip mToolTips;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private GameObject mMask;

	private bool mIsVisible;

	public bool IsVisible
	{
		get
		{
			return this.mIsVisible;
		}
		set
		{
			this.mIsVisible = value;
			base.gameObject.SetActive(this.mIsVisible);
		}
	}

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mMask = base.transform.Find("mask").gameObject;
		base.gameObject.AddComponent<UIDragScrollView>();
		UIEventListener expr_68 = UIEventListener.Get(base.gameObject);
		expr_68.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_68.onPress, new UIEventListener.BoolDelegate(this.OnItemPress));
	}

	public void Refresh(PetInfo petInfo, bool isMask)
	{
		this.mPetInfo = petInfo;
		this.mItemInfo = null;
		this.Refresh();
	}

	public void Refresh(ItemInfo itemInfo, bool isMask)
	{
		this.mPetInfo = null;
		this.mItemInfo = itemInfo;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mPetInfo != null)
		{
			this.IsVisible = true;
			this.mIcon.atlas = this.mBaseScene.mPetsIconAtlas;
			this.mIcon.spriteName = this.mPetInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mPetInfo.Quality);
			this.mMask.SetActive(Globals.Instance.Player.PetSystem.GetPetCount(this.mPetInfo.ID) == 0);
		}
		else if (this.mItemInfo != null)
		{
			this.IsVisible = true;
			this.mIcon.atlas = this.mBaseScene.mItemsIconAtlas;
			this.mIcon.spriteName = this.mItemInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
			this.mMask.SetActive(Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID) == 0);
		}
		else
		{
			this.IsVisible = false;
		}
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (this.mPetInfo != null && !this.mPetInfo.ShowCollection)
		{
			if (isPressed)
			{
				if (this.mToolTips == null)
				{
					this.mToolTips = GameUIToolTipManager.GetInstance().CreatePetTooltip(go.transform, this.mPetInfo);
				}
				this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), Tools.GetPetName(this.mPetInfo), this.mPetInfo.Desc, string.Empty);
				this.mToolTips.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(-50f, 80f, -7000f));
				this.mToolTips.EnableToolTip();
			}
			else if (this.mToolTips != null)
			{
				this.mToolTips.HideTipAnim();
			}
		}
		else if (this.mItemInfo != null)
		{
			if (isPressed)
			{
				if (this.mToolTips == null)
				{
					this.mToolTips = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
				}
				this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mItemInfo.Name, this.mItemInfo.Desc, this.mItemInfo.Quality);
				this.mToolTips.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(-50f, 80f, -7000f));
				this.mToolTips.EnableToolTip();
			}
			else if (this.mToolTips != null)
			{
				this.mToolTips.HideTipAnim();
			}
		}
	}
}
