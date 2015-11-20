using System;

public class EquipSaleBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		ItemDataEx itemDataEx = (ItemDataEx)a;
		ItemDataEx itemDataEx2 = (ItemDataEx)b;
		if (itemDataEx == null || itemDataEx2 == null)
		{
			return 0;
		}
		if (itemDataEx.Info.Quality > itemDataEx2.Info.Quality)
		{
			return 1;
		}
		if (itemDataEx.Info.Quality < itemDataEx2.Info.Quality)
		{
			return -1;
		}
		if (itemDataEx.Info.SubQuality > itemDataEx2.Info.SubQuality)
		{
			return 1;
		}
		if (itemDataEx.Info.SubQuality < itemDataEx2.Info.SubQuality)
		{
			return -1;
		}
		if (itemDataEx.GetEquipRefineLevel() > itemDataEx2.GetEquipRefineLevel())
		{
			return 1;
		}
		if (itemDataEx.GetEquipRefineLevel() < itemDataEx2.GetEquipRefineLevel())
		{
			return -1;
		}
		if (itemDataEx.GetEquipEnhanceLevel() > itemDataEx2.GetEquipEnhanceLevel())
		{
			return 1;
		}
		if (itemDataEx.GetEquipEnhanceLevel() < itemDataEx2.GetEquipEnhanceLevel())
		{
			return -1;
		}
		if (itemDataEx.Info.ID < itemDataEx2.Info.ID)
		{
			return 1;
		}
		if (itemDataEx.Info.ID > itemDataEx2.Info.ID)
		{
			return -1;
		}
		return 0;
	}
}
