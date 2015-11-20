using System;

public class AwakeItemsBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		ItemDataEx itemDataEx = (ItemDataEx)a;
		ItemDataEx itemDataEx2 = (ItemDataEx)b;
		if (itemDataEx.Info.Quality > itemDataEx2.Info.Quality)
		{
			return -1;
		}
		if (itemDataEx.Info.Quality < itemDataEx2.Info.Quality)
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
		return 0;
	}
}
