using Att;
using System;

public class LevelExp
{
	public static uint[,] TotalExp = new uint[151, 4];

	public static uint[,] TotalRefineExp = new uint[81, 5];

	public static uint[,] TotalRefineItemCount = new uint[31, 5];

	public static uint[,] TotalRefineTrinketCount = new uint[31, 5];

	public static uint[,] TotalFurtherItemCount = new uint[16, 4];

	public static uint[,] TotalFurtherPetCount = new uint[16, 4];

	public static uint[] TotalSkillItemCount = new uint[17];

	public static uint[,] TotalTrinketEnhanceExp = new uint[151, 5];

	public static uint[,] TotalLopetExp = new uint[121, 4];

	public static uint[,] TotalAwakeLopetItemCount = new uint[GameConst.GetInt32(251) + 1, 4];

	public static uint[,] TotalAwakeLopetCount = new uint[GameConst.GetInt32(251) + 1, 4];

	public static void Init()
	{
		Array.Clear(LevelExp.TotalExp, 0, LevelExp.TotalExp.Length);
		Array.Clear(LevelExp.TotalRefineExp, 0, LevelExp.TotalRefineExp.Length);
		Array.Clear(LevelExp.TotalRefineItemCount, 0, LevelExp.TotalRefineItemCount.Length);
		Array.Clear(LevelExp.TotalRefineTrinketCount, 0, LevelExp.TotalRefineTrinketCount.Length);
		Array.Clear(LevelExp.TotalFurtherItemCount, 0, LevelExp.TotalFurtherItemCount.Length);
		Array.Clear(LevelExp.TotalFurtherItemCount, 0, LevelExp.TotalFurtherItemCount.Length);
		Array.Clear(LevelExp.TotalFurtherPetCount, 0, LevelExp.TotalFurtherPetCount.Length);
		Array.Clear(LevelExp.TotalSkillItemCount, 0, LevelExp.TotalSkillItemCount.Length);
		Array.Clear(LevelExp.TotalTrinketEnhanceExp, 0, LevelExp.TotalTrinketEnhanceExp.Length);
		Array.Clear(LevelExp.TotalLopetExp, 0, LevelExp.TotalLopetExp.Length);
		Array.Clear(LevelExp.TotalAwakeLopetItemCount, 0, LevelExp.TotalAwakeLopetItemCount.Length);
		Array.Clear(LevelExp.TotalAwakeLopetCount, 0, LevelExp.TotalAwakeLopetCount.Length);
		foreach (LevelInfo current in Globals.Instance.AttDB.LevelDict.Values)
		{
			if (current.ID > 0 && current.ID <= 150)
			{
				for (int i = 0; i < 4; i++)
				{
					LevelExp.TotalExp[current.ID, i] = current.Exp[i];
				}
			}
			if (current.ID > 0 && current.ID <= 80)
			{
				for (int j = 0; j < 5; j++)
				{
					LevelExp.TotalRefineExp[current.ID, j] = current.RefineExp[j];
				}
			}
			if (current.ID > 0 && current.ID <= 150)
			{
				for (int k = 0; k < 5; k++)
				{
					LevelExp.TotalTrinketEnhanceExp[current.ID, k] = current.EnhanceExp[k];
				}
			}
			if (current.LPExp.Count >= 4 && current.ID > 0 && current.ID <= 120)
			{
				for (int l = 0; l < 4; l++)
				{
					LevelExp.TotalLopetExp[current.ID, l] = current.LPExp[l];
				}
			}
		}
		foreach (TinyLevelInfo current2 in Globals.Instance.AttDB.TinyLevelDict.Values)
		{
			if (current2.ID > 0 && current2.ID <= 30)
			{
				for (int m = 0; m < 5; m++)
				{
					LevelExp.TotalRefineItemCount[current2.ID, m] = current2.RefineItemCount[m];
					LevelExp.TotalRefineTrinketCount[current2.ID, m] = current2.RefineTrinketCount[m];
				}
			}
			if (current2.ID > 0 && current2.ID <= 15)
			{
				for (int n = 0; n < 4; n++)
				{
					LevelExp.TotalFurtherItemCount[current2.ID, n] = current2.FurtherItemCount[n];
					LevelExp.TotalFurtherPetCount[current2.ID, n] = current2.FurtherPetCount[n];
				}
			}
			if (current2.ID > 0 && current2.ID <= 16)
			{
				LevelExp.TotalSkillItemCount[current2.ID] = current2.SkillItemCount;
			}
			if (current2.LopetAwakeItemCount.Count >= 4 && current2.LopetAwakeCount.Count >= 4 && current2.ID > 0 && current2.ID <= GameConst.GetInt32(251))
			{
				for (int num = 0; num < 4; num++)
				{
					LevelExp.TotalAwakeLopetItemCount[current2.ID, num] = current2.LopetAwakeItemCount[num];
					LevelExp.TotalAwakeLopetCount[current2.ID, num] = current2.LopetAwakeCount[num];
				}
			}
		}
		for (int num2 = 0; num2 < 4; num2++)
		{
			for (int num3 = 150; num3 >= 0; num3--)
			{
				uint num4 = 0u;
				for (int num5 = 0; num5 <= num3; num5++)
				{
					num4 += LevelExp.TotalExp[num5, num2];
				}
				LevelExp.TotalExp[num3, num2] = num4;
			}
			for (int num6 = 15; num6 >= 0; num6--)
			{
				uint num7 = 0u;
				uint num8 = 0u;
				for (int num9 = 0; num9 <= num6; num9++)
				{
					num7 += LevelExp.TotalFurtherItemCount[num9, num2];
					num8 += LevelExp.TotalFurtherPetCount[num9, num2];
				}
				LevelExp.TotalFurtherItemCount[num6, num2] = num7;
				LevelExp.TotalFurtherPetCount[num6, num2] = num8;
			}
			for (int num10 = 120; num10 >= 0; num10--)
			{
				uint num11 = 0u;
				for (int num12 = 0; num12 <= num10; num12++)
				{
					num11 += LevelExp.TotalLopetExp[num12, num2];
				}
				LevelExp.TotalLopetExp[num10, num2] = num11;
			}
			for (int num13 = GameConst.GetInt32(251); num13 >= 0; num13--)
			{
				uint num14 = 0u;
				uint num15 = 0u;
				for (int num16 = 0; num16 <= num13; num16++)
				{
					num14 += LevelExp.TotalAwakeLopetItemCount[num16, num2];
					num15 += LevelExp.TotalAwakeLopetCount[num16, num2];
				}
				LevelExp.TotalAwakeLopetItemCount[num13, num2] = num14;
				LevelExp.TotalAwakeLopetCount[num13, num2] = num15;
			}
		}
		for (int num17 = 0; num17 < 5; num17++)
		{
			for (int num18 = 80; num18 >= 0; num18--)
			{
				uint num19 = 0u;
				for (int num20 = 0; num20 <= num18; num20++)
				{
					num19 += LevelExp.TotalRefineExp[num20, num17];
				}
				LevelExp.TotalRefineExp[num18, num17] = num19;
			}
			for (int num21 = 150; num21 >= 0; num21--)
			{
				uint num22 = 0u;
				for (int num23 = 0; num23 <= num21; num23++)
				{
					num22 += LevelExp.TotalTrinketEnhanceExp[num23, num17];
				}
				LevelExp.TotalTrinketEnhanceExp[num21, num17] = num22;
			}
			for (int num24 = 30; num24 >= 0; num24--)
			{
				uint num25 = 0u;
				uint num26 = 0u;
				for (int num27 = 0; num27 <= num24; num27++)
				{
					num25 += LevelExp.TotalRefineItemCount[num27, num17];
					num26 += LevelExp.TotalRefineTrinketCount[num27, num17];
				}
				LevelExp.TotalRefineItemCount[num24, num17] = num25;
				LevelExp.TotalRefineTrinketCount[num24, num17] = num26;
			}
		}
		for (int num28 = 16; num28 >= 0; num28--)
		{
			uint num29 = 0u;
			for (int num30 = 0; num30 <= num28; num30++)
			{
				num29 += LevelExp.TotalSkillItemCount[num30];
			}
			LevelExp.TotalSkillItemCount[num28] = num29;
		}
	}

	public static uint GetTotalExp(int level, int quality)
	{
		if (level <= 0 || level > 150 || quality < 0 || quality >= 4)
		{
			return 0u;
		}
		return LevelExp.TotalExp[level, quality];
	}

	public static uint GetTotalEquipRefineExp(int level, int quality)
	{
		if (level <= 0 || level > 80 || quality < 0 || quality >= 5)
		{
			return 0u;
		}
		return LevelExp.TotalRefineExp[level, quality];
	}

	public static void GetTotalTrinketRefineCount(int level, int quality, out uint itemCount, out uint trinketCount)
	{
		itemCount = 0u;
		trinketCount = 0u;
		if (level <= 0 || level > 30 || quality < 0 || quality >= 5)
		{
			return;
		}
		itemCount = LevelExp.TotalRefineItemCount[level, quality];
		trinketCount = LevelExp.TotalRefineTrinketCount[level, quality];
	}

	public static void GetTotalFurtherData(int further, int quality, out uint itemCount, out uint petCount)
	{
		itemCount = 0u;
		petCount = 0u;
		if (further <= 0 || further > 15 || quality < 0 || quality >= 4)
		{
			return;
		}
		itemCount = LevelExp.TotalFurtherItemCount[further, quality];
		petCount = LevelExp.TotalFurtherPetCount[further, quality];
	}

	public static uint GetTotalSkillItemCount(int level)
	{
		if (level <= 0 || level > 16)
		{
			return 0u;
		}
		return LevelExp.TotalSkillItemCount[level];
	}

	public static uint GetTotalLopetExp(int level, int quality)
	{
		if (level <= 0 || level > 120 || quality < 0 || quality >= 4)
		{
			return 0u;
		}
		return LevelExp.TotalLopetExp[level, quality];
	}

	public static void GetTotalLopetAwakeData(int awake, int quality, out uint itemCount, out uint lopetCount)
	{
		itemCount = 0u;
		lopetCount = 0u;
		if (awake <= 0 || awake > GameConst.GetInt32(251) || quality < 0 || quality >= 4)
		{
			return;
		}
		itemCount = LevelExp.TotalAwakeLopetItemCount[awake, quality];
		lopetCount = LevelExp.TotalAwakeLopetCount[awake, quality];
	}
}
