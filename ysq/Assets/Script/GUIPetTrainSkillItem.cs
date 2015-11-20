using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainSkillItem : MonoBehaviour
{
	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mNum;

	private ItemInfo mItemInfo;

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

	private void OnIconClick(GameObject go)
	{
		if (this.mItemInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mItemInfo);
		}
	}
}
