using System;

public class LopetFragmentBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		ItemDataEx itemDataEx = (ItemDataEx)a;
		ItemDataEx itemDataEx2 = (ItemDataEx)b;
		if (itemDataEx != null && itemDataEx2 != null)
		{
			if (itemDataEx.CanCreate() && !itemDataEx2.CanCreate())
			{
				return -1;
			}
			if (!itemDataEx.CanCreate() && itemDataEx2.CanCreate())
			{
				return 1;
			}
			if (itemDataEx.Info.Quality > itemDataEx2.Info.Quality)
			{
				return -1;
			}
			if (itemDataEx.Info.Quality < itemDataEx2.Info.Quality)
			{
				return 1;
			}
			if (itemDataEx.Info.SubQuality > itemDataEx2.Info.SubQuality)
			{
				return -1;
			}
			if (itemDataEx.Info.SubQuality < itemDataEx2.Info.SubQuality)
			{
				return 1;
			}
			if (itemDataEx.GetCount() > itemDataEx2.GetCount())
			{
				return -1;
			}
			if (itemDataEx.GetCount() < itemDataEx2.GetCount())
			{
				return 1;
			}
			if (itemDataEx.Info.ID < itemDataEx2.Info.ID)
			{
				return -1;
			}
			if (itemDataEx.Info.ID > itemDataEx2.Info.ID)
			{
				return 1;
			}
		}
		return 0;
	}
}
