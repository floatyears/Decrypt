using System;

public class TrinketBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		ItemDataEx itemDataEx = (ItemDataEx)a;
		ItemDataEx itemDataEx2 = (ItemDataEx)b;
		if (itemDataEx == null || itemDataEx2 == null)
		{
			return 0;
		}
		if (!itemDataEx.IsEquiped())
		{
			if (!itemDataEx2.IsEquiped())
			{
				return this.SortByQuality(itemDataEx, itemDataEx2);
			}
			return 1;
		}
		else
		{
			if (!itemDataEx2.IsEquiped())
			{
				return -1;
			}
			return this.SortByQuality(itemDataEx, itemDataEx2);
		}
	}

	private int SortByQuality(ItemDataEx aItem, ItemDataEx bItem)
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
		if (aItem.GetTrinketRefineLevel() > bItem.GetTrinketRefineLevel())
		{
			return -1;
		}
		if (aItem.GetTrinketRefineLevel() < bItem.GetTrinketRefineLevel())
		{
			return 1;
		}
		if (aItem.GetTrinketEnhanceLevel() > bItem.GetTrinketEnhanceLevel())
		{
			return -1;
		}
		if (aItem.GetTrinketEnhanceLevel() < bItem.GetTrinketEnhanceLevel())
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
		if (aItem.GetID() > bItem.GetID())
		{
			return 1;
		}
		if (aItem.GetID() < bItem.GetID())
		{
			return -1;
		}
		return 0;
	}
}
