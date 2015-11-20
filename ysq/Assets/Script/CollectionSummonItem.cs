using Att;
using System;
using UnityEngine;

public sealed class CollectionSummonItem : MonoBehaviour
{
	private GameObject mItemGo;

	private UISprite mSummonIcon;

	private UISprite mSummonQualityMask;

	private UISprite mOwnMask;

	private GameObject mNewMark;

	public PetInfo SummonItemInfo
	{
		get;
		private set;
	}

	public PetDataEx SummonItemData
	{
		get;
		private set;
	}

	public void InitItem(GUISummonCollectionScene baseScene, PetInfo petInfo, bool isPetSet)
	{
		this.SummonItemInfo = petInfo;
		if (petInfo != null)
		{
			this.SummonItemData = Globals.Instance.Player.PetSystem.GetPetByInfoID(this.SummonItemInfo.ID);
		}
		this.CreateObjects();
		this.Refresh();
	}

	public void SetItemVisible(bool isShow)
	{
		this.mItemGo.SetActive(isShow);
	}

	private void CreateObjects()
	{
		this.mItemGo = base.transform.Find("item").gameObject;
		this.mSummonIcon = this.mItemGo.transform.Find("PetIcon").GetComponent<UISprite>();
		this.mSummonQualityMask = this.mItemGo.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mOwnMask = this.mItemGo.transform.Find("PetMask").GetComponent<UISprite>();
		this.mNewMark = base.transform.Find("PetMark").gameObject;
		UIEventListener expr_A1 = UIEventListener.Get(base.gameObject);
		expr_A1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A1.onClick, new UIEventListener.VoidDelegate(this.OnSummonItemClick));
	}

	public void Refresh(PetInfo petInfo)
	{
		if (petInfo != null)
		{
			this.SummonItemInfo = petInfo;
			if (petInfo != null)
			{
				this.SummonItemData = Globals.Instance.Player.PetSystem.GetPetByInfoID(this.SummonItemInfo.ID);
			}
			this.Refresh();
		}
	}

	public void Refresh()
	{
		if (this.SummonItemData != null)
		{
			this.SummonItemInfo = this.SummonItemData.Info;
			this.mOwnMask.gameObject.SetActive(false);
			this.mNewMark.gameObject.SetActive(false);
		}
		else
		{
			this.mOwnMask.gameObject.SetActive(true);
			this.mNewMark.gameObject.SetActive(false);
		}
		if (this.SummonItemInfo != null)
		{
			this.mItemGo.SetActive(true);
			this.mSummonQualityMask.spriteName = Tools.GetItemQualityIcon(this.SummonItemInfo.Quality);
			this.mSummonIcon.spriteName = this.SummonItemInfo.Icon;
		}
		else
		{
			this.mItemGo.SetActive(false);
		}
	}

	private void OnSummonItemClick(GameObject go)
	{
		if (this.SummonItemData != null)
		{
			ItemInfo fragmentInfo = PetFragment.GetFragmentInfo(this.SummonItemData.Info.ID);
			if (fragmentInfo != null)
			{
				GameUIManager.mInstance.ShowPetCollectionInfo(fragmentInfo);
			}
		}
		else if (this.SummonItemInfo != null)
		{
			ItemInfo fragmentInfo2 = PetFragment.GetFragmentInfo(this.SummonItemInfo.ID);
			if (fragmentInfo2 != null)
			{
				GameUIManager.mInstance.ShowPetCollectionInfo(fragmentInfo2);
			}
		}
	}
}
