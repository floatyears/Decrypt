using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainJinjieItem : MonoBehaviour
{
	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mNum;

	private ItemInfo mItemInfo;

	private PetInfo mPetInfo;

	private LopetInfo mLopetInfo;

	private StringBuilder mSb = new StringBuilder();

	public bool IsEnough
	{
		get;
		set;
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_2B = UIEventListener.Get(this.mIcon.gameObject);
		expr_2B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2B.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mNum = base.transform.Find("num").GetComponent<UILabel>();
	}

	public void Refresh(ItemInfo itemInfo, int curItemCount, int needItemCount)
	{
		this.mItemInfo = itemInfo;
		this.mPetInfo = null;
		this.mLopetInfo = null;
		this.IsEnough = (curItemCount >= needItemCount);
		if (itemInfo != null)
		{
			this.mIcon.gameObject.SetActive(true);
			this.mQualityMask.gameObject.SetActive(true);
			this.mIcon.spriteName = itemInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(itemInfo.Quality);
		}
		else
		{
			this.mIcon.gameObject.SetActive(false);
			this.mQualityMask.gameObject.SetActive(false);
		}
		this.mSb.Remove(0, this.mSb.Length);
		if (curItemCount < needItemCount)
		{
			this.mSb.Append("[ff0000]");
		}
		else
		{
			this.mSb.Append("[ffffff]");
		}
		this.mSb.Append(curItemCount).Append("[-]/").Append(needItemCount);
		this.mNum.text = this.mSb.ToString();
	}

	public void Refresh(PetInfo petInfo, int curPetCount, int needPetCount)
	{
		this.mItemInfo = null;
		this.mPetInfo = petInfo;
		this.mLopetInfo = null;
		this.IsEnough = (curPetCount >= needPetCount);
		if (petInfo != null)
		{
			this.mIcon.gameObject.SetActive(true);
			this.mQualityMask.gameObject.SetActive(true);
			this.mIcon.spriteName = petInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(petInfo.Quality);
		}
		else
		{
			this.mIcon.gameObject.SetActive(false);
			this.mQualityMask.gameObject.SetActive(false);
		}
		this.mSb.Remove(0, this.mSb.Length);
		if (curPetCount < needPetCount)
		{
			this.mSb.Append("[ff0000]");
		}
		else
		{
			this.mSb.Append("[ffffff]");
		}
		this.mSb.Append(curPetCount).Append("[-]/").Append(needPetCount);
		this.mNum.text = this.mSb.ToString();
	}

	public void Refresh(LopetInfo lopetInfo, int curLopetCount, int needLopetCount)
	{
		this.mItemInfo = null;
		this.mPetInfo = null;
		this.mLopetInfo = lopetInfo;
		this.IsEnough = (curLopetCount >= needLopetCount);
		if (lopetInfo != null)
		{
			this.mIcon.gameObject.SetActive(true);
			this.mQualityMask.gameObject.SetActive(true);
			this.mIcon.spriteName = lopetInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(lopetInfo.Quality);
		}
		else
		{
			this.mIcon.gameObject.SetActive(false);
			this.mQualityMask.gameObject.SetActive(false);
		}
		this.mSb.Remove(0, this.mSb.Length);
		if (curLopetCount < needLopetCount)
		{
			this.mSb.Append("[ff0000]");
		}
		else
		{
			this.mSb.Append("[ffffff]");
		}
		this.mSb.Append(curLopetCount).Append("[-]/").Append(needLopetCount);
		this.mNum.text = this.mSb.ToString();
	}

	private void OnIconClick(GameObject go)
	{
		if (this.mItemInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mItemInfo);
		}
		else if (this.mPetInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mPetInfo);
		}
		else if (this.mLopetInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mLopetInfo);
		}
	}
}
