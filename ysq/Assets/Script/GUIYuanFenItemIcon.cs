using Att;
using System;
using UnityEngine;

public class GUIYuanFenItemIcon : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private PetInfo mPetInfo;

	private GameUIToolTip mToolTips;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private GameObject mMask;

	private bool mIsActive;

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

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene)
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
		UIEventListener expr_94 = UIEventListener.Get(base.gameObject);
		expr_94.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_94.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
	}

	public void Refresh(PetInfo petInfo, bool isMask)
	{
		this.mPetInfo = petInfo;
		this.mIsActive = isMask;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mPetInfo != null)
		{
			this.IsVisible = true;
			this.mIcon.spriteName = this.mPetInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mPetInfo.Quality);
			if (this.mBaseScene.IsLocalPlayer)
			{
				this.mMask.SetActive(Globals.Instance.Player.PetSystem.GetPetCount(this.mPetInfo.ID) == 0);
			}
			else
			{
				this.mMask.SetActive(!this.mIsActive);
			}
		}
		else
		{
			this.IsVisible = false;
		}
	}

	private void OnItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.mPetInfo != null && !this.mPetInfo.ShowCollection && !this.mIsActive)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mPetInfo);
		}
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (this.mIsActive && this.mPetInfo != null && !this.mPetInfo.ShowCollection)
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
	}
}
