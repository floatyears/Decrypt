using System;
using System.Collections.Generic;
using UnityEngine;

public class PartnerManageInventoryTable : UICustomGrid
{
	private GUIPartnerManageScene mBaseScene;

	public void InitWithBaseScene(GUIPartnerManageScene baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/PartnerItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		PartnerManageInventoryItem partnerManageInventoryItem = gameObject.AddComponent<PartnerManageInventoryItem>();
		partnerManageInventoryItem.InitItemData(this.mBaseScene);
		return partnerManageInventoryItem;
	}

	public int SortByQuality(PetDataEx aItem, PetDataEx bItem)
	{
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
		if (aItem.Data.Level > bItem.Data.Level)
		{
			return -1;
		}
		if (aItem.Data.Level < bItem.Data.Level)
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

	protected int Sort(BaseData a, BaseData b)
	{
		PetDataEx petDataEx = (PetDataEx)a;
		PetDataEx petDataEx2 = (PetDataEx)b;
		if (petDataEx == null || petDataEx2 == null)
		{
			return 0;
		}
		if (!petDataEx.IsPlayerBattling())
		{
			if (!petDataEx2.IsPlayerBattling())
			{
				return this.SortByPetBattling(petDataEx, petDataEx2);
			}
			return 1;
		}
		else
		{
			if (!petDataEx2.IsPlayerBattling())
			{
				return -1;
			}
			return this.SortByPetBattling(petDataEx, petDataEx2);
		}
	}

	private int SortByPetBattling(PetDataEx aItem, PetDataEx bItem)
	{
		if (!aItem.IsPetBattling())
		{
			if (!bItem.IsPetBattling())
			{
				return this.SortByPetAssisting(aItem, bItem);
			}
			return 1;
		}
		else
		{
			if (!bItem.IsPetBattling())
			{
				return -1;
			}
			return this.SortByPetAssisting(aItem, bItem);
		}
	}

	private int SortByPetAssisting(PetDataEx aItem, PetDataEx bItem)
	{
		if (!aItem.IsPetAssisting())
		{
			if (!bItem.IsPetAssisting())
			{
				return this.SortByQuality(aItem, bItem);
			}
			return 1;
		}
		else
		{
			if (!bItem.IsPetAssisting())
			{
				return -1;
			}
			return this.SortByQuality(aItem, bItem);
		}
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}
}
