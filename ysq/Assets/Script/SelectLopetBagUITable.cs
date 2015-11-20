using System;

public class SelectLopetBagUITable : CommonBagUITable
{
	protected override int Sort(BaseData a, BaseData b)
	{
		LopetDataEx lopetDataEx = (LopetDataEx)a;
		LopetDataEx lopetDataEx2 = (LopetDataEx)b;
		if (lopetDataEx != null && lopetDataEx2 != null)
		{
			if (lopetDataEx.IsBattling() && !lopetDataEx2.IsBattling())
			{
				return -1;
			}
			if (!lopetDataEx.IsBattling() && lopetDataEx2.IsBattling())
			{
				return 1;
			}
			if (lopetDataEx.Info.Quality > lopetDataEx2.Info.Quality)
			{
				return -1;
			}
			if (lopetDataEx.Info.Quality < lopetDataEx2.Info.Quality)
			{
				return 1;
			}
			if (lopetDataEx.Data.Awake > lopetDataEx2.Data.Awake)
			{
				return -1;
			}
			if (lopetDataEx.Data.Awake < lopetDataEx2.Data.Awake)
			{
				return 1;
			}
			if (lopetDataEx.Data.Level > lopetDataEx2.Data.Level)
			{
				return -1;
			}
			if (lopetDataEx.Data.Level < lopetDataEx2.Data.Level)
			{
				return 1;
			}
			if (lopetDataEx.Data.Exp > lopetDataEx2.Data.Exp)
			{
				return -1;
			}
			if (lopetDataEx.Data.Exp < lopetDataEx2.Data.Exp)
			{
				return 1;
			}
			if (lopetDataEx.Info.ID < lopetDataEx2.Info.ID)
			{
				return -1;
			}
			if (lopetDataEx.Info.ID > lopetDataEx2.Info.ID)
			{
				return 1;
			}
			if (lopetDataEx.GetID() > lopetDataEx.GetID())
			{
				return 1;
			}
			if (lopetDataEx.GetID() < lopetDataEx.GetID())
			{
				return -1;
			}
		}
		return 0;
	}
}
