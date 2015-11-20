using Att;
using System;

public class ConLevelInfo
{
	public const int MAX_CON_LEVEL = 60;

	public static ConInfo[] Infos = new ConInfo[61];

	public static void Init()
	{
		Array.Clear(ConLevelInfo.Infos, 0, ConLevelInfo.Infos.Length);
		ConstellationInfo info = Globals.Instance.AttDB.ConstellationDict.GetInfo(1);
		if (info == null)
		{
			Debug.LogErrorFormat("ConstellationDict.GetInfo error, id = {0}", new object[]
			{
				1
			});
			return;
		}
		int num = 1;
		while (info != null)
		{
			for (int i = 0; i < info.Type.Count; i++)
			{
				if (ConLevelInfo.Infos[num] == null)
				{
					ConLevelInfo.Infos[num] = new ConInfo();
					if (ConLevelInfo.Infos[num] == null)
					{
						Debug.LogError(new object[]
						{
							"new ConInfo() error"
						});
						return;
					}
				}
				if (info.Type[i] == 0)
				{
					int num2 = info.Value1[i];
					switch (num2)
					{
					case 1:
						ConLevelInfo.Infos[num].MaxHP = info.Value2[i];
						break;
					case 2:
						ConLevelInfo.Infos[num].Attack = info.Value2[i];
						break;
					case 3:
						ConLevelInfo.Infos[num].PhysicDefense = info.Value2[i];
						break;
					case 4:
						ConLevelInfo.Infos[num].MagicDefense = info.Value2[i];
						break;
					default:
						if (num2 == 20)
						{
							ConLevelInfo.Infos[num].PhysicDefense = info.Value2[i];
							ConLevelInfo.Infos[num].MagicDefense = info.Value2[i];
						}
						break;
					}
				}
				ConLevelInfo.Infos[num].Cost = info.Cost[i];
				num++;
			}
			info = Globals.Instance.AttDB.ConstellationDict.GetInfo(info.NextID);
		}
		for (int j = ConLevelInfo.Infos.Length - 1; j > 1; j--)
		{
			if (ConLevelInfo.Infos[j] != null)
			{
				for (int k = 1; k < j; k++)
				{
					if (ConLevelInfo.Infos[k] != null)
					{
						ConLevelInfo.Infos[j].MaxHP += ConLevelInfo.Infos[k].MaxHP;
						ConLevelInfo.Infos[j].Attack += ConLevelInfo.Infos[k].Attack;
						ConLevelInfo.Infos[j].PhysicDefense += ConLevelInfo.Infos[k].PhysicDefense;
						ConLevelInfo.Infos[j].MagicDefense += ConLevelInfo.Infos[k].MagicDefense;
					}
				}
			}
		}
	}

	public static ConInfo GetConInfo(int level)
	{
		if (level < 0 || level > 60)
		{
			return null;
		}
		return ConLevelInfo.Infos[level];
	}
}
