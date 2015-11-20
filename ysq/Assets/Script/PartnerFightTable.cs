using System;
using System.Collections.Generic;
using UnityEngine;

public class PartnerFightTable : UICustomGrid
{
	private GUIPartnerFightScene mBaseScene;

	public void InitWithBaseScene(GUIPartnerFightScene baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/PartnerFightItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		PartnerFightBagItem partnerFightBagItem = gameObject.AddComponent<PartnerFightBagItem>();
		partnerFightBagItem.InitItemData(this.mBaseScene);
		return partnerFightBagItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}

	protected int Sort(BaseData a, BaseData b)
	{
		PetDataEx petDataEx = (PetDataEx)a;
		PetDataEx petDataEx2 = (PetDataEx)b;
		if (petDataEx == null || petDataEx2 == null)
		{
			return 0;
		}
		if (!petDataEx.IsPetBattling())
		{
			if (!petDataEx2.IsPetBattling())
			{
				return this.SortByPetAssisting(petDataEx, petDataEx2);
			}
			return 1;
		}
		else
		{
			if (!petDataEx2.IsPetBattling())
			{
				return -1;
			}
			return this.SortByPetAssisting(petDataEx, petDataEx2);
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

	public int SortByQuality(PetDataEx aItem, PetDataEx bItem)
	{
		if (aItem == null || bItem == null)
		{
			return 0;
		}
		if (aItem.Relation > bItem.Relation)
		{
			return -1;
		}
		if (aItem.Relation < bItem.Relation)
		{
			return 1;
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
}
