using System;

public class TrinketEnhanceSelectItemBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		ItemDataEx aItem = (ItemDataEx)a;
		ItemDataEx bItem = (ItemDataEx)b;
		return TrinketEnhanceSelectItemBagUITable.Sort(aItem, bItem);
	}

	public static int Sort(ItemDataEx aItem, ItemDataEx bItem)
	{
		if (aItem == null || bItem == null)
		{
			return 0;
		}
		if (aItem.Info.Type == 4 && aItem.Info.SubType == 9 && (bItem.Info.Type != 4 || bItem.Info.SubType != 9))
		{
			return -1;
		}
		if ((aItem.Info.Type != 4 || aItem.Info.SubType != 9) && bItem.Info.Type == 4 && bItem.Info.SubType == 9)
		{
			return 1;
		}
		if (aItem.Info.Quality > bItem.Info.Quality)
		{
			return 1;
		}
		if (aItem.Info.Quality < bItem.Info.Quality)
		{
			return -1;
		}
		if (aItem.Info.SubQuality > bItem.Info.SubQuality)
		{
			return 1;
		}
		if (aItem.Info.SubQuality < bItem.Info.SubQuality)
		{
			return -1;
		}
		if (aItem.GetEquipRefineLevel() > bItem.GetEquipRefineLevel())
		{
			return 1;
		}
		if (aItem.GetEquipRefineLevel() < bItem.GetEquipRefineLevel())
		{
			return -1;
		}
		if (aItem.GetEquipEnhanceLevel() > bItem.GetEquipEnhanceLevel())
		{
			return 1;
		}
		if (aItem.GetEquipEnhanceLevel() < bItem.GetEquipEnhanceLevel())
		{
			return -1;
		}
		if (aItem.Info.ID < bItem.Info.ID)
		{
			return 1;
		}
		if (aItem.Info.ID > bItem.Info.ID)
		{
			return -1;
		}
		return 0;
	}
}
