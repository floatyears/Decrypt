using System;
using System.Collections.Generic;
using UnityEngine;

public class BagPartnerSliceInventoryTable : UICustomGrid
{
	public ItemDataEx mDataEx;

	public PartnerItemSliceTabLayer baseUISession;

	public GUIPartnerManageScene mBaseScene;

	public void InitWithBaseScene(GUIPartnerManageScene baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/PartnerChipItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		BagParInventoryItem bagParInventoryItem = gameObject.AddComponent<BagParInventoryItem>();
		bagParInventoryItem.InitItemData(this.mBaseScene);
		return bagParInventoryItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}

	protected int Sort(BaseData a, BaseData b)
	{
		ItemDataEx itemDataEx = (ItemDataEx)a;
		ItemDataEx itemDataEx2 = (ItemDataEx)b;
		if (itemDataEx == null || itemDataEx2 == null)
		{
			return 0;
		}
		if (!itemDataEx.CanCreate())
		{
			if (!itemDataEx2.CanCreate())
			{
				return this.SortByQuality(itemDataEx, itemDataEx2);
			}
			return 1;
		}
		else
		{
			if (!itemDataEx2.CanCreate())
			{
				return -1;
			}
			return this.SortByQuality(itemDataEx, itemDataEx2);
		}
	}

	public int SortByQuality(ItemDataEx aItem, ItemDataEx bItem)
	{
		if (aItem == null || bItem == null)
		{
			return 0;
		}
		if (aItem.Info.Quality > bItem.Info.Quality)
		{
			return -1;
		}
		if (aItem.Info.Quality < bItem.Info.Quality)
		{
			return 1;
		}
		if (aItem.Info.SubQuality > bItem.Info.SubQuality)
		{
			return -1;
		}
		if (aItem.Info.SubQuality < bItem.Info.SubQuality)
		{
			return 1;
		}
		if (aItem.GetCount() > bItem.GetCount())
		{
			return -1;
		}
		if (aItem.GetCount() < bItem.GetCount())
		{
			return 1;
		}
		if (aItem.Info.ID < bItem.Info.ID)
		{
			return -1;
		}
		if (aItem.Info.ID > bItem.Info.ID)
		{
			return 1;
		}
		return 0;
	}
}
