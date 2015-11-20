using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainJueXingCostItem : MonoBehaviour
{
	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mNum;

	private ItemInfo mItemInfo;

	private PetInfo mPetInfo;

	private StringBuilder mSb = new StringBuilder(42);

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
		this.mIcon.spriteName = string.Empty;
		UIEventListener expr_3B = UIEventListener.Get(this.mIcon.gameObject);
		expr_3B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3B.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mQualityMask.spriteName = string.Empty;
		this.mNum = base.transform.Find("num").GetComponent<UILabel>();
		this.mNum.text = string.Empty;
	}

	public void Refresh(ItemInfo itemInfo, int curItemCount, int needItemCount)
	{
		this.mItemInfo = itemInfo;
		this.mPetInfo = null;
		this.IsEnough = (curItemCount >= needItemCount);
		if (itemInfo != null)
		{
			this.mIcon.gameObject.SetActive(true);
			this.mQualityMask.gameObject.SetActive(true);
			this.mIcon.spriteName = itemInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(itemInfo.Quality);
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
		else
		{
			this.mIcon.gameObject.SetActive(false);
			this.mQualityMask.gameObject.SetActive(false);
			this.mNum.text = string.Empty;
		}
	}

	public void Refresh(PetInfo petInfo, int curPetCount, int needPetCount)
	{
		this.mItemInfo = null;
		this.mPetInfo = petInfo;
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
	}
}
