using Att;
using Proto;
using System;
using UnityEngine;

public class LopetDataEx : BaseData
{
	public int State;

	public LopetData Data
	{
		get;
		private set;
	}

	public LopetInfo Info
	{
		get;
		private set;
	}

	public LopetDataEx(LopetData data, LopetInfo info)
	{
		this.Data = data;
		this.Info = info;
	}

	public void ClearUIData()
	{
		this.State = 0;
	}

	public override ulong GetID()
	{
		return this.Data.ID;
	}

	public bool IsLevelMax()
	{
		return (ulong)this.Data.Level >= (ulong)((long)GameConst.GetInt32(240));
	}

	public bool IsAwakeMax()
	{
		return (ulong)this.Data.Awake >= (ulong)((long)GameConst.GetInt32(251));
	}

	public bool IsLocal()
	{
		return Globals.Instance.Player.LopetSystem.GetLopet(this.Data.ID) != null;
	}

	public bool IsOld()
	{
		return this.Data.Exp > 0u || this.Data.Level > 1u || this.Data.Awake > 0u;
	}

	public bool IsBattling()
	{
		return Globals.Instance.Player.LopetSystem.IsCombatLopet(this.Data.ID);
	}

	public void GetAttribute(ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		LopetDataEx.GetAttribute(this.Info, this.Data, ref maxHP, ref attack, ref physicDefense, ref magicDefense);
	}

	public static void GetAttribute(LopetInfo lInfo, LopetData lData, ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		if (lInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"lInfo == null"
			});
			return;
		}
		maxHP = lInfo.MaxHP + lInfo.MaxHPInc * (int)lData.Level;
		attack = lInfo.Attack + lInfo.AttackInc * (int)lData.Level;
		physicDefense = lInfo.PhysicDefense + lInfo.PhysicDefenseInc * (int)lData.Level;
		magicDefense = lInfo.MagicDefense + lInfo.MagicDefenseInc * (int)lData.Level;
		int num = (int)(lData.Awake - 1u);
		if (num >= 0 && num < GameConst.GetInt32(251))
		{
			if (num < lInfo.AwakeAttack.Count)
			{
				attack += lInfo.AwakeAttack[num];
			}
			if (num < lInfo.AwakePhysicDefense.Count)
			{
				physicDefense += lInfo.AwakePhysicDefense[num];
			}
			if (num < lInfo.AwakeMagicDefense.Count)
			{
				magicDefense += lInfo.AwakeMagicDefense[num];
			}
			if (num < lInfo.AwakeMaxHP.Count)
			{
				maxHP += lInfo.AwakeMaxHP[num];
			}
		}
	}

	public uint GetAwakeNeedLvl()
	{
		return (this.Data.Awake != 0u) ? (15u + (this.Data.Awake - 1u) * 10u) : 0u;
	}

	public int GetMaxAwake(bool isIgnoneLevel = false)
	{
		int @int = GameConst.GetInt32(251);
		if (isIgnoneLevel)
		{
			return @int;
		}
		int b;
		if (this.Data.Level < 15u)
		{
			b = 1;
		}
		else if (this.Data.Level < this.Data.Level / 10u * 10u + 5u)
		{
			b = (int)(this.Data.Level / 10u);
		}
		else
		{
			b = (int)(this.Data.Level / 10u + 1u);
		}
		return Mathf.Min(@int, b);
	}

	public uint GetMaxExp()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo((int)this.Data.Level);
		if (info != null && this.Info.Quality < info.LPExp.Count && this.Info.Quality >= 0)
		{
			return info.LPExp[this.Info.Quality];
		}
		return 0u;
	}

	public uint GetExpRate()
	{
		uint maxExp = this.GetMaxExp();
		return (maxExp != 0u) ? (this.Data.Exp * 100u / maxExp) : 1u;
	}

	public bool GetLevelupInfo(uint exp, out uint level, out uint expRate, out uint overflowExp)
	{
		uint num = this.Data.Exp + exp;
		level = this.Data.Level;
		expRate = 0u;
		overflowExp = 0u;
		if (exp == 0u || this.Info.Quality < 0 || this.Info.Quality >= 4)
		{
			return false;
		}
		if ((ulong)level >= (ulong)((long)GameConst.GetInt32(240)))
		{
			return true;
		}
		bool result = false;
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo((int)level);
		if (info == null)
		{
			return false;
		}
		while (info != null)
		{
			if (num < info.LPExp[this.Info.Quality])
			{
				break;
			}
			level += 1u;
			num -= info.LPExp[this.Info.Quality];
			if ((ulong)level >= (ulong)((long)GameConst.GetInt32(240)))
			{
				result = true;
				break;
			}
			info = Globals.Instance.AttDB.LevelDict.GetInfo((int)level);
		}
		expRate = num * 100u / info.LPExp[this.Info.Quality];
		return result;
	}

	public bool GetLevelupDetailInfo(uint exp, out uint level, out int maxHP, out int attack, out int physicDefense, out int magicDefense, out uint expRate, out uint overflowExp)
	{
		bool levelupInfo = this.GetLevelupInfo(exp, out level, out expRate, out overflowExp);
		maxHP = this.Info.MaxHP + this.Info.MaxHPInc * (int)level;
		attack = this.Info.Attack + this.Info.AttackInc * (int)level;
		physicDefense = this.Info.PhysicDefense + this.Info.PhysicDefenseInc * (int)level;
		magicDefense = this.Info.MagicDefense + this.Info.MagicDefenseInc * (int)level;
		int num = (int)(this.Data.Awake - 1u);
		if (num >= 0 && num < GameConst.GetInt32(251))
		{
			if (num < this.Info.AwakeAttack.Count)
			{
				attack += this.Info.AwakeAttack[num];
			}
			if (num < this.Info.AwakePhysicDefense.Count)
			{
				physicDefense += this.Info.AwakePhysicDefense[num];
			}
			if (num < this.Info.AwakeMagicDefense.Count)
			{
				magicDefense += this.Info.AwakeMagicDefense[num];
			}
			if (num < this.Info.AwakeMaxHP.Count)
			{
				maxHP += this.Info.AwakeMaxHP[num];
			}
		}
		return levelupInfo;
	}

	public void GetFurtherData(out int curItemCount, out int needItemCount, out int needMoney, out int curLopetCount, out int needLopetCount)
	{
		curItemCount = 0;
		needItemCount = 0;
		needMoney = 0;
		curLopetCount = 0;
		needLopetCount = 0;
		if ((ulong)this.Data.Awake >= (ulong)((long)GameConst.GetInt32(251)))
		{
			return;
		}
		TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo((int)(this.Data.Awake + 1u));
		if (info == null)
		{
			return;
		}
		if (this.Info.Quality >= 0 && this.Info.Quality < info.LopetAwakeItemCount.Count)
		{
			needItemCount = (int)info.LopetAwakeItemCount[this.Info.Quality];
		}
		if (this.Info.Quality >= 0 && this.Info.Quality < info.LopetAwakeCost.Count)
		{
			needMoney = (int)info.LopetAwakeCost[this.Info.Quality];
		}
		if (this.Info.Quality >= 0 && this.Info.Quality < info.LopetAwakeCount.Count)
		{
			needLopetCount = (int)info.LopetAwakeCount[this.Info.Quality];
		}
		curLopetCount = Globals.Instance.Player.LopetSystem.GetAwakePetCount(this.Data.ID, this.Data.InfoID);
		curItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(205));
	}

	public SkillInfo GetPlayerSkillInfo()
	{
		return Globals.Instance.AttDB.SkillDict.GetInfo(this.Info.PlayerSkillID);
	}

	public void GetRebornData(out uint lopetCount, out uint money, out uint awakeItemCount, out uint[] itemCount)
	{
		lopetCount = 0u;
		money = 0u;
		awakeItemCount = 0u;
		itemCount = new uint[GameConst.LOPET_EXP_ITEM_ID.Length];
		LevelExp.GetTotalLopetAwakeData((int)this.Data.Awake, this.Info.Quality, out awakeItemCount, out lopetCount);
		lopetCount += 1u;
		if (this.Data.Level > 1u)
		{
			money = LevelExp.TotalLopetExp[(int)((UIntPtr)(this.Data.Level - 1u)), this.Info.Quality];
		}
		money += this.Data.Exp;
		int num = (int)money;
		money = (uint)(money / ((float)GameConst.GetInt32(250) / 10000f));
		if (num > 0)
		{
			for (int i = itemCount.Length - 1; i >= 0; i--)
			{
				if (num <= 0)
				{
					break;
				}
				ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.LOPET_EXP_ITEM_ID[i]);
				if (info == null)
				{
					global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
					{
						GameConst.LOPET_EXP_ITEM_ID[i]
					});
					return;
				}
				itemCount[i] = (uint)(num / info.Value1);
				num -= (int)(itemCount[i] * (uint)info.Value1);
			}
		}
	}

	public void GetBreakData(out uint value, out uint money, out uint awakeItemCount, out uint[] itemCount)
	{
		this.GetRebornData(out value, out money, out awakeItemCount, out itemCount);
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.Info.Quality + 1);
		if (info == null)
		{
			global::Debug.LogErrorFormat("QualityDict.GetInfo error, id = {0}", new object[]
			{
				this.Info.Quality + 1
			});
			return;
		}
		value *= info.LopetValue;
	}

	public int GetCombatValue()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		this.GetAttribute(ref num, ref num2, ref num3, ref num4);
		return num * 8 / 100 + num2 + num3 + num4;
	}
}
