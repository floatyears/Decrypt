using Att;
using System;
using UnityEngine;

public class PillageFarmItem : UICustomGridItem
{
	private PillageFarmItemData mData;

	private UILabel mNum;

	private UILabel mExp;

	private UILabel mMoney;

	private UILabel mItemTips;

	private UILabel mItemName;

	private GameObject mItem;

	private Transform mPos;

	public void Init()
	{
		this.mNum = base.transform.Find("Sprite/Label").GetComponent<UILabel>();
		this.mMoney = base.transform.Find("money/Label").GetComponent<UILabel>();
		this.mExp = base.transform.Find("exp/Label").GetComponent<UILabel>();
		this.mItemTips = base.transform.Find("ItemTips").GetComponent<UILabel>();
		this.mPos = base.transform.FindChild("item");
		this.mItemName = this.mPos.FindChild("itemName").GetComponent<UILabel>();
	}

	public override void Refresh(object data)
	{
		if (data == this.mData)
		{
			return;
		}
		this.mData = (PillageFarmItemData)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mData == null)
		{
			return;
		}
		this.mNum.text = Singleton<StringManager>.Instance.GetString("Pillage11", new object[]
		{
			this.mData.GetID()
		});
		this.mMoney.text = string.Format("{0:#,###0}", this.mData.mData.Money);
		this.mExp.text = this.mData.mData.Exp.ToString();
		this.mItemTips.color = Color.white;
		if (this.mData.mData.ItemID == 0)
		{
			this.mItemTips.text = Singleton<StringManager>.Instance.GetString("Pillage3");
		}
		else
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(this.mData.mData.ItemID);
			if (info == null)
			{
				this.mItemTips.text = string.Empty;
			}
			else
			{
				this.mItemTips.text = string.Format(Singleton<StringManager>.Instance.GetString("Pillage4"), Tools.GetItemQualityColorHex(info.Quality), info.Name);
			}
		}
		if (this.mItem != null)
		{
			UnityEngine.Object.Destroy(this.mItem.gameObject);
		}
		if (this.mData.mData.ExtraItemID != 0 && this.mData.mData.ExtraItemCount != 0)
		{
			this.RefreshItemReward(this.mData.mData.ExtraItemID, this.mData.mData.ExtraItemCount);
		}
		else if (this.mData.mData.ExtraDiamond != 0)
		{
			Transform transform = base.transform.FindChild("item");
			this.mItem = GameUITools.CreateReward(2, this.mData.mData.ExtraDiamond, this.mData.mData.ExtraDiamond, transform, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
			if (this.mItem == null)
			{
				global::Debug.LogErrorFormat("GameUITools.CreateReward Diamond Error", new object[0]);
				transform.gameObject.SetActive(false);
			}
			else
			{
				transform.gameObject.SetActive(true);
			}
			this.mItemName.enabled = false;
		}
		else if (this.mData.mData.ExtraMoney != 0)
		{
			this.mItem = GameUITools.CreateReward(1, this.mData.mData.ExtraMoney, this.mData.mData.ExtraMoney, this.mPos, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
			if (this.mItem == null)
			{
				global::Debug.LogErrorFormat("GameUITools.CreateReward Money Error", new object[0]);
				this.mPos.gameObject.SetActive(false);
			}
			else
			{
				this.mPos.gameObject.SetActive(true);
			}
			this.mItemName.enabled = false;
		}
	}

	private void RefreshItemReward(int itemID, int itenCount)
	{
		this.mPos.gameObject.SetActive(false);
		if (itemID != 0)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(itemID);
			if (info == null)
			{
				global::Debug.LogErrorFormat("Can not find item Info {0}", new object[]
				{
					itemID
				});
			}
			else
			{
				this.mItem = GameUITools.CreateReward(3, itemID, itenCount, this.mPos, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
				if (this.mItem == null)
				{
					global::Debug.LogErrorFormat("GameUITools.CreateReward Error {0}", new object[]
					{
						itemID
					});
				}
				this.mItemName.text = info.Name;
				this.mItemName.color = Tools.GetItemQualityColor(info.Quality);
				this.mItemName.enabled = true;
				this.mPos.gameObject.SetActive(true);
			}
		}
	}
}
