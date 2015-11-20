using Att;
using System;

public class Awake
{
	private static int[] attPct = new int[52];

	private static AttMod[,] attValue = new AttMod[52, 6];

	public static void Init()
	{
		Array.Clear(Awake.attPct, 0, Awake.attPct.Length);
		Array.Clear(Awake.attValue, 0, Awake.attValue.Length);
		foreach (AwakeInfo current in Globals.Instance.AttDB.AwakeDict.Values)
		{
			if (current.ID <= 0 || current.ID > 51)
			{
				Debug.LogErrorFormat("AwakeInfo error, id = {0}", new object[]
				{
					current.ID
				});
			}
			else
			{
				Awake.attPct[current.ID] = current.AttPct;
				for (int i = 0; i < 6; i++)
				{
					Awake.attValue[current.ID, i] = new AttMod();
					if (Awake.attValue[current.ID, i] == null)
					{
						Debug.LogError(new object[]
						{
							"Allocate AttMod error"
						});
					}
					else
					{
						for (int j = 0; j < 4; j++)
						{
							int num = current.ItemID[i * 4 + j];
							if (num != 0)
							{
								ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(num);
								if (info != null)
								{
									Awake.attValue[current.ID, i].Attack += info.Value1;
									Awake.attValue[current.ID, i].Defense += info.Value2;
									Awake.attValue[current.ID, i].MaxHP += info.Value3;
								}
							}
						}
						Awake.attValue[current.ID, i].Attack += current.Attack;
						Awake.attValue[current.ID, i].Defense += current.Defense;
						Awake.attValue[current.ID, i].MaxHP += current.MaxHP;
					}
				}
			}
		}
		for (int k = 51; k > 1; k--)
		{
			for (int l = 1; l < k; l++)
			{
				Awake.attPct[k] += Awake.attPct[l];
				for (int m = 0; m < 6; m++)
				{
					if (Awake.attValue[k, m] != null)
					{
						Awake.attValue[k, m].Attack += Awake.attValue[l, m].Attack;
						Awake.attValue[k, m].Defense += Awake.attValue[l, m].Defense;
						Awake.attValue[k, m].MaxHP += Awake.attValue[l, m].MaxHP;
					}
				}
			}
		}
	}

	public static int GetAttPctMod(int level)
	{
		if (level <= 0 || level > 51)
		{
			return 0;
		}
		return Awake.attPct[level];
	}

	public static AttMod GetAttValueMod(int elementType, int level)
	{
		if (elementType < 0 || elementType >= 6 || level <= 0 || level > 51)
		{
			return null;
		}
		return Awake.attValue[level, elementType];
	}
}
