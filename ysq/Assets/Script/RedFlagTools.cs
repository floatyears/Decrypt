using System;

public static class RedFlagTools
{
	public static bool HasAssitPetFull()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		for (int i = 0; i < 6; i++)
		{
			if (Tools.IsAssistUnlocked(i) && teamSystem.GetAssist(i) == null)
			{
				return false;
			}
		}
		return true;
	}

	public static bool HasBattlePetFull()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		for (int i = 1; i <= 3; i++)
		{
			if (teamSystem.GetPet(i) == null)
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanShowZhuWeiMark()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return false;
		}
		if (!Tools.CanPlay(GameConst.GetInt32(29), true))
		{
			return false;
		}
		if (!RedFlagTools.HasAssitPetFull())
		{
			return false;
		}
		for (int i = 0; i < 6; i++)
		{
			PetDataEx assist = teamSystem.GetAssist(i, true);
			if (assist == null)
			{
				return false;
			}
			if (Tools.CanPetLvlUp(assist))
			{
				return true;
			}
		}
		bool flag = Tools.CanPlay(GameConst.GetInt32(30), true);
		if (flag)
		{
			for (int j = 0; j < 6; j++)
			{
				PetDataEx assist2 = teamSystem.GetAssist(j, true);
				if (assist2 == null)
				{
					return false;
				}
				if (Tools.CanShowJiJieMark(assist2))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool CanShowXingZuoMark()
	{
		return Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) >= GUIRightInfo.GetCost() && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(7)) && Globals.Instance.Player.Data.ConstellationLevel < 60;
	}
}
