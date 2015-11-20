using Att;
using System;
using UnityEngine;

public class ShopItemIcon : MonoBehaviour
{
	protected UISprite itemQuality;

	protected UISprite itemIcon;

	protected UISprite petIcon;

	protected UISprite lopetIcon;

	protected UISprite fashionIcon;

	protected UILabel itemName;

	protected UILabel itemCount;

	protected UISprite itemFrag;

	protected UILabel itemNum;

	protected UILabel itemDesc;

	public void Init()
	{
		Tools.GetSafeComponent<UIDragScrollView>(base.gameObject);
		this.itemQuality = base.transform.GetComponent<UISprite>();
		this.itemIcon = this.itemQuality.transform.Find("itemIcon").GetComponent<UISprite>();
		this.itemIcon.gameObject.SetActive(false);
		this.petIcon = this.itemQuality.transform.Find("petIcon").GetComponent<UISprite>();
		this.petIcon.gameObject.SetActive(false);
		this.lopetIcon = this.itemQuality.transform.Find("lopetIcon").GetComponent<UISprite>();
		this.lopetIcon.gameObject.SetActive(false);
		Transform transform = this.itemQuality.transform.Find("fashionIcon");
		if (transform != null)
		{
			this.fashionIcon = transform.GetComponent<UISprite>();
			this.fashionIcon.gameObject.SetActive(false);
		}
		this.itemName = this.itemQuality.transform.Find("itemName").GetComponent<UILabel>();
		Transform transform2 = this.itemQuality.transform.Find("itemCount");
		if (transform2 != null)
		{
			this.itemCount = transform2.GetComponent<UILabel>();
		}
		this.itemFrag = this.itemQuality.transform.Find("Frag").GetComponent<UISprite>();
		this.itemNum = this.itemQuality.transform.Find("itemNum").GetComponent<UILabel>();
		Transform transform3 = this.itemQuality.transform.Find("itemDesc");
		if (transform3 != null)
		{
			this.itemDesc = transform3.GetComponent<UILabel>();
		}
	}

	public void Refresh(ShopGridData gridData)
	{
		LocalPlayer player = Globals.Instance.Player;
		if (gridData.shopInfo.IsFashion != 0)
		{
			this.itemQuality.spriteName = Tools.GetItemQualityIcon(gridData.fashionInfo.Quality);
			this.itemIcon.gameObject.SetActive(false);
			this.petIcon.gameObject.SetActive(false);
			this.lopetIcon.gameObject.SetActive(false);
			this.fashionIcon.gameObject.SetActive(true);
			this.fashionIcon.spriteName = gridData.fashionInfo.Icon;
			this.itemName.text = gridData.fashionInfo.Name;
			this.itemName.color = Tools.GetItemQualityColor(gridData.fashionInfo.Quality);
			this.itemFrag.gameObject.SetActive(false);
			if (this.itemCount != null)
			{
				this.itemCount.gameObject.SetActive(false);
			}
			if (this.itemNum != null)
			{
				this.itemNum.gameObject.SetActive(false);
			}
			if (this.itemDesc != null)
			{
				this.itemDesc.text = gridData.fashionInfo.Desc;
			}
			return;
		}
		if (this.fashionIcon != null)
		{
			this.fashionIcon.gameObject.SetActive(false);
		}
		this.itemQuality.spriteName = Tools.GetItemQualityIcon(gridData.itemInfo.Quality);
		if (gridData.itemInfo.Type == 3 && gridData.itemInfo.SubType == 0)
		{
			PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(gridData.itemInfo.Value2);
			if (info != null)
			{
				this.petIcon.spriteName = info.Icon;
			}
			this.itemIcon.gameObject.SetActive(false);
			this.petIcon.gameObject.SetActive(true);
			this.lopetIcon.gameObject.SetActive(false);
			this.itemFrag.spriteName = "frag";
		}
		else if (gridData.itemInfo.Type == 3 && gridData.itemInfo.SubType == 3)
		{
			LopetInfo info2 = Globals.Instance.AttDB.LopetDict.GetInfo(gridData.itemInfo.Value2);
			if (info2 != null)
			{
				this.lopetIcon.spriteName = info2.Icon;
			}
			this.itemIcon.gameObject.SetActive(false);
			this.petIcon.gameObject.SetActive(false);
			this.lopetIcon.gameObject.SetActive(true);
			this.itemFrag.spriteName = "frag";
		}
		else
		{
			this.itemIcon.spriteName = gridData.itemInfo.Icon;
			this.itemIcon.gameObject.SetActive(true);
			this.petIcon.gameObject.SetActive(false);
			this.lopetIcon.gameObject.SetActive(false);
			this.itemFrag.spriteName = "frag2";
		}
		this.itemName.text = gridData.itemInfo.Name;
		this.itemName.color = Tools.GetItemQualityColor(gridData.itemInfo.Quality);
		if (this.itemDesc != null)
		{
			this.itemDesc.text = gridData.itemInfo.Desc;
		}
		this.itemFrag.gameObject.SetActive(gridData.itemInfo.Type == 3);
		if (this.itemCount != null)
		{
			if (gridData.itemInfo.Type == 3)
			{
				this.itemCount.text = string.Format("({0}/{1})", player.ItemSystem.GetItemCount(gridData.itemInfo.ID), gridData.itemInfo.Value1);
				this.itemCount.gameObject.SetActive(true);
			}
			else
			{
				this.itemCount.gameObject.SetActive(false);
			}
		}
		if (gridData.shopInfo.Count > 1)
		{
			this.itemNum.text = gridData.shopInfo.Count.ToString();
			this.itemNum.gameObject.SetActive(true);
		}
		else
		{
			this.itemNum.gameObject.SetActive(false);
		}
	}

	public void Refresh(RTShopGridData gridData)
	{
		LocalPlayer player = Globals.Instance.Player;
		this.fashionIcon.gameObject.SetActive(false);
		if (gridData.shopData.InfoType == 1)
		{
			if (gridData.itemInfo == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.itemQuality.spriteName = Tools.GetItemQualityIcon(gridData.itemInfo.Quality);
			if (gridData.itemInfo.Type == 3 && gridData.itemInfo.SubType == 0)
			{
				this.petIcon.spriteName = gridData.petInfo.Icon;
				this.itemIcon.gameObject.SetActive(false);
				this.petIcon.gameObject.SetActive(true);
				this.lopetIcon.gameObject.SetActive(false);
				this.itemFrag.spriteName = "frag";
			}
			else if (gridData.itemInfo.Type == 3 && gridData.itemInfo.SubType == 3)
			{
				this.lopetIcon.spriteName = gridData.lopetInfo.Icon;
				this.itemIcon.gameObject.SetActive(false);
				this.petIcon.gameObject.SetActive(false);
				this.lopetIcon.gameObject.SetActive(true);
				this.itemFrag.spriteName = "frag";
			}
			else
			{
				this.itemIcon.spriteName = gridData.itemInfo.Icon;
				this.itemIcon.gameObject.SetActive(true);
				this.petIcon.gameObject.SetActive(false);
				this.lopetIcon.gameObject.SetActive(false);
				this.itemFrag.spriteName = "frag2";
			}
			this.itemName.text = gridData.itemInfo.Name;
			this.itemName.color = Tools.GetItemQualityColor(gridData.itemInfo.Quality);
			this.itemFrag.gameObject.SetActive(gridData.itemInfo.Type == 3);
			if (gridData.itemInfo.Type == 3)
			{
				if (this.itemCount != null)
				{
					this.itemCount.text = string.Format("({0}/{1})", player.ItemSystem.GetItemCount(gridData.itemInfo.ID), gridData.itemInfo.Value1);
					this.itemCount.gameObject.SetActive(true);
				}
			}
			else if (this.itemCount != null)
			{
				this.itemCount.gameObject.SetActive(false);
			}
		}
		else if (gridData.shopData.InfoType == 2)
		{
			if (gridData.petInfo == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.itemQuality.spriteName = Tools.GetItemQualityIcon(gridData.petInfo.Quality);
			this.petIcon.spriteName = gridData.petInfo.Icon;
			this.itemIcon.gameObject.SetActive(false);
			this.petIcon.gameObject.SetActive(true);
			this.lopetIcon.gameObject.SetActive(false);
			this.itemName.text = Tools.GetPetName(gridData.petInfo);
			this.itemName.color = Tools.GetItemQualityColor(gridData.petInfo.Quality);
			this.itemFrag.gameObject.SetActive(false);
			if (this.itemCount != null)
			{
				this.itemCount.gameObject.SetActive(false);
			}
		}
		else
		{
			if (gridData.shopData.InfoType != 3)
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (gridData.petInfo == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.itemQuality.spriteName = Tools.GetItemQualityIcon(gridData.lopetInfo.Quality);
			this.lopetIcon.spriteName = gridData.lopetInfo.Icon;
			this.itemIcon.gameObject.SetActive(false);
			this.petIcon.gameObject.SetActive(false);
			this.lopetIcon.gameObject.SetActive(true);
			this.itemName.text = gridData.lopetInfo.Name;
			this.itemName.color = Tools.GetItemQualityColor(gridData.lopetInfo.Quality);
			this.itemFrag.gameObject.SetActive(false);
			if (this.itemCount != null)
			{
				this.itemCount.gameObject.SetActive(false);
			}
		}
		if (gridData.shopData.Count == 0u)
		{
			this.itemNum.gameObject.SetActive(false);
		}
		else
		{
			this.itemNum.text = gridData.shopData.Count.ToString();
			this.itemNum.gameObject.SetActive(true);
		}
	}
}
