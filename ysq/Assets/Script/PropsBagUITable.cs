using System;

public class PropsBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		ItemDataEx itemDataEx = (ItemDataEx)a;
		ItemDataEx itemDataEx2 = (ItemDataEx)b;
		if (itemDataEx.Info.Type > itemDataEx2.Info.Type)
		{
			return 1;
		}
		if (itemDataEx.Info.Type < itemDataEx2.Info.Type)
		{
			return -1;
		}
		if (itemDataEx.Info.SubType > itemDataEx2.Info.SubType)
		{
			return 1;
		}
		if (itemDataEx.Info.SubType < itemDataEx2.Info.SubType)
		{
			return -1;
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
