using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PetDataEx : BaseData
{
	private int socketSlot = -1;

	public int State;

	public PetData Data
	{
		get;
		private set;
	}

	public PetInfo Info
	{
		get;
		private set;
	}

	public int Relation
	{
		get;
		private set;
	}

	public PetDataEx(PetData data, PetInfo info)
	{
		this.Data = data;
		this.Info = info;
		this.Relation = 0;
		this.socketSlot = -1;
	}

	public override ulong GetID()
	{
		return this.Data.ID;
	}

	public void ResetInfo(PetInfo info)
	{
		if (this.Data.InfoID != info.ID || this.Info == info)
		{
			return;
		}
		this.Info = info;
	}

	public void SetSocketSlot(int slot)
	{
		this.socketSlot = slot;
	}

	public int GetSocketSlot()
	{
		return this.socketSlot;
	}

	public bool IsPlayerBattling()
	{
		return 0 == this.socketSlot;
	}

	public bool IsPetBattling()
	{
		return 0 < this.socketSlot && this.socketSlot < 4;
	}

	public bool IsBattling()
	{
		return this.IsPlayerBattling() || this.IsPetBattling();
	}

	public bool IsOld()
	{
		return this.Data.Exp > 0u || this.Data.Level > 1u || this.Data.Further > 0u || this.Data.CultivateCount > 0;
	}

	public bool IsPetAssisting()
	{
		return 4 <= this.socketSlot && this.socketSlot < 10;
	}

	public void GetBastAtt(ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (this.Info.ID == 90000 || this.Info.ID == 90001)
		{
			QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.Info.Quality + 1);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("QualityDict.GetInfo error, id = {0}", this.Info.Quality + 1)
				});
				return;
			}
			if (this.Data.Further > 0u)
			{
				TinyLevelInfo info2 = Globals.Instance.AttDB.TinyLevelDict.GetInfo((int)this.Data.Further);
				if (info2 == null)
				{
					global::Debug.LogError(new object[]
					{
						string.Format("TinyLevelDict.GetInfo error, id = {0}", this.Data.Further)
					});
					return;
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info2.MaxHPFurther.Count)
				{
					num = info2.MaxHPFurther[this.Info.Quality];
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info2.AttackFurther.Count)
				{
					num2 = info2.AttackFurther[this.Info.Quality];
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info2.PhysicDefenseFurther.Count)
				{
					num3 = info2.PhysicDefenseFurther[this.Info.Quality];
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info2.MagicDefenseFurther.Count)
				{
					num4 = info2.MagicDefenseFurther[this.Info.Quality];
				}
			}
			maxHP = info.MaxHP + (info.MaxHPInc + num) * (int)this.Data.Level;
			attack = info.Attack + (info.AttackInc + num2) * (int)this.Data.Level;
			physicDefense = info.PhysicDefense + (info.PhysicDefenseInc + num3) * (int)this.Data.Level;
			magicDefense = info.MagicDefense + (info.MagicDefenseInc + num4) * (int)this.Data.Level;
		}
		else
		{
			if (this.Data.Further > 0u)
			{
				TinyLevelInfo info3 = Globals.Instance.AttDB.TinyLevelDict.GetInfo((int)this.Data.Further);
				if (info3 == null)
				{
					global::Debug.LogError(new object[]
					{
						string.Format("TinyLevelDict.GetInfo error, id = {0}", this.Data.Further)
					});
					return;
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info3.PetMaxHPFurther.Count)
				{
					num = info3.PetMaxHPFurther[this.Info.Quality];
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info3.PetAttackFurther.Count)
				{
					num2 = info3.PetAttackFurther[this.Info.Quality];
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info3.PetPhysicDefenseFurther.Count)
				{
					num3 = info3.PetPhysicDefenseFurther[this.Info.Quality];
				}
				if (this.Info.Quality >= 0 && this.Info.Quality < info3.PetMagicDefenseFurther.Count)
				{
					num4 = info3.PetMagicDefenseFurther[this.Info.Quality];
				}
			}
			maxHP = this.Info.MaxHP + (this.Info.MaxHPInc + num) * (int)this.Data.Level;
			attack = this.Info.Attack + (this.Info.AttackInc + num2) * (int)this.Data.Level;
			physicDefense = this.Info.PhysicDefense + (this.Info.PhysicDefenseInc + num3) * (int)this.Data.Level;
			magicDefense = this.Info.MagicDefense + (this.Info.MagicDefenseInc + num4) * (int)this.Data.Level;
		}
	}

	public void GetAttribute(ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		if (this.Data.ID >= 100uL)
		{
			int num = Globals.Instance.Player.TeamSystem.GetSocketSlot(this.Data.ID);
			if (num != -1)
			{
				Globals.Instance.Player.TeamSystem.GetAttribute(num, ref maxHP, ref attack, ref physicDefense, ref magicDefense);
				return;
			}
		}
		else
		{
			int remoteSocketSlot = Globals.Instance.Player.TeamSystem.GetRemoteSocketSlot(this.Data.ID);
			if (remoteSocketSlot != -1)
			{
				Globals.Instance.Player.TeamSystem.GetRemoteAttribute(remoteSocketSlot, ref maxHP, ref attack, ref physicDefense, ref magicDefense);
				return;
			}
		}
		PetDataEx.GetAttribute(this.Info, this.Data, ref maxHP, ref attack, ref physicDefense, ref magicDefense);
	}

	public static void GetAttribute(PetInfo petInfo, PetData petData, ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		if (petInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"petInfo == null"
			});
			return;
		}
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		long num4 = 0L;
		long num5 = 0L;
		long num6 = 0L;
		long num7 = 0L;
		long num8 = 0L;
		int elementType = petInfo.ElementType;
		if (petData.Further > 0u)
		{
			TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo((int)petData.Further);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("TinyLevelDict.GetInfo error, id = {0}", petData.Further)
				});
				return;
			}
			if (petInfo.Quality >= 0 && petInfo.Quality < info.PetMaxHPFurther.Count)
			{
				num = (long)info.PetMaxHPFurther[petInfo.Quality];
			}
			if (petInfo.Quality >= 0 && petInfo.Quality < info.PetAttackFurther.Count)
			{
				num2 = (long)info.PetAttackFurther[petInfo.Quality];
			}
			if (petInfo.Quality >= 0 && petInfo.Quality < info.PetPhysicDefenseFurther.Count)
			{
				num3 = (long)info.PetPhysicDefenseFurther[petInfo.Quality];
			}
			if (petInfo.Quality >= 0 && petInfo.Quality < info.PetMagicDefenseFurther.Count)
			{
				num4 = (long)info.PetMagicDefenseFurther[petInfo.Quality];
			}
			num = (long)petInfo.MaxHP + ((long)petInfo.MaxHPInc + num) * (long)petData.Level;
			num2 = (long)petInfo.Attack + ((long)petInfo.AttackInc + num2) * (long)petData.Level;
			num3 = (long)petInfo.PhysicDefense + ((long)petInfo.PhysicDefenseInc + num3) * (long)petData.Level;
			num4 = (long)petInfo.MagicDefense + ((long)petInfo.MagicDefenseInc + num4) * (long)petData.Level;
			int num9 = 0;
			while (num9 < petInfo.TalentID.Count && num9 < (int)petData.Further)
			{
				if (petInfo.TalentID[num9] != 0)
				{
					TalentInfo info2 = Globals.Instance.AttDB.TalentDict.GetInfo(petInfo.TalentID[num9]);
					if (info2 == null)
					{
						global::Debug.LogError(new object[]
						{
							string.Format("TalentDict.GetInfo error, id = {0}", petInfo.TalentID[num9])
						});
						return;
					}
					if (info2.TargetType != 2 || elementType == info2.TargetValue)
					{
						if (info2.EffectType == 1)
						{
							int value = info2.Value1;
							switch (value)
							{
							case 1:
								num += (long)info2.Value2;
								break;
							case 2:
								num2 += (long)info2.Value2;
								break;
							case 3:
								num3 += (long)info2.Value2;
								break;
							case 4:
								num4 += (long)info2.Value2;
								break;
							default:
								if (value == 20)
								{
									num3 += (long)info2.Value2;
									num4 += (long)info2.Value2;
								}
								break;
							}
						}
						else if (info2.EffectType == 2)
						{
							int value = info2.Value1;
							switch (value)
							{
							case 1:
								num5 += (long)info2.Value2;
								break;
							case 2:
								num6 += (long)info2.Value2;
								break;
							case 3:
								num7 += (long)info2.Value2;
								break;
							case 4:
								num8 += (long)info2.Value2;
								break;
							default:
								if (value == 20)
								{
									num7 += (long)info2.Value2;
									num8 += (long)info2.Value2;
								}
								break;
							}
						}
					}
				}
				num9++;
			}
		}
		else
		{
			num = (long)(petInfo.MaxHP + petInfo.MaxHPInc * (int)petData.Level);
			num2 = (long)(petInfo.Attack + petInfo.AttackInc * (int)petData.Level);
			num3 = (long)(petInfo.PhysicDefense + petInfo.PhysicDefenseInc * (int)petData.Level);
			num4 = (long)(petInfo.MagicDefense + petInfo.MagicDefenseInc * (int)petData.Level);
		}
		int awake = (int)petData.Awake;
		if (awake > 0)
		{
			AttMod attValueMod = Awake.GetAttValueMod(elementType, awake);
			if (attValueMod != null)
			{
				num += (long)attValueMod.MaxHP;
				num2 += (long)attValueMod.Attack;
				num3 += (long)attValueMod.Defense;
				num4 += (long)attValueMod.Defense;
			}
			int attPctMod = Awake.GetAttPctMod(awake);
			if (attPctMod > 0)
			{
				num5 += (long)attPctMod;
				num6 += (long)attPctMod;
				num7 += (long)attPctMod;
				num8 += (long)attPctMod;
			}
		}
		AwakeInfo info3 = Globals.Instance.AttDB.AwakeDict.GetInfo(awake + 1);
		if (info3 != null)
		{
			int itemFlag = (int)petData.ItemFlag;
			for (int i = 0; i < 4; i++)
			{
				int index = elementType * 4 + i;
				if (info3.ItemID[index] != 0 && (itemFlag & 1 << i) != 0)
				{
					ItemInfo info4 = Globals.Instance.AttDB.ItemDict.GetInfo(info3.ItemID[index]);
					if (info4 == null)
					{
						global::Debug.LogErrorFormat("ItemDict.GetInfo error, ID = {0}", new object[]
						{
							info3.ItemID[index]
						});
					}
					else
					{
						if (info4.Value1 > 0)
						{
							num2 += (long)info4.Value1;
						}
						if (info4.Value2 > 0)
						{
							num3 += (long)info4.Value2;
							num4 += (long)info4.Value2;
						}
						if (info4.Value3 > 0)
						{
							num += (long)info4.Value3;
						}
					}
				}
			}
		}
		num += (long)petData.MaxHP;
		num2 += (long)petData.Attack;
		num3 += (long)petData.PhysicDefense;
		num4 += (long)petData.MagicDefense;
		maxHP = (int)(num * (10000L + num5) / 10000L);
		attack = (int)(num2 * (10000L + num6) / 10000L);
		physicDefense = (int)(num3 * (10000L + num7) / 10000L);
		magicDefense = (int)(num4 * (10000L + num8) / 10000L);
	}

	public uint GetMaxExp()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo((int)this.Data.Level);
		if (info != null && this.Info.Quality < info.Exp.Count && this.Info.Quality >= 0)
		{
			return info.Exp[this.Info.Quality];
		}
		return 0u;
	}

	public uint GetExpRate()
	{
		uint maxExp = this.GetMaxExp();
		return (maxExp != 0u) ? (this.Data.Exp * 100u / maxExp) : 1u;
	}

	public uint GetFurtherNeedLvl()
	{
		if (this.Data.Further == 0u)
		{
			return 0u;
		}
		if (this.Data.Further <= 9u)
		{
			return 15u + (this.Data.Further - 1u) * 10u;
		}
		return (this.Data.Further + 1u) * 10u;
	}

	public uint GetJueXingNeedLvl()
	{
		uint num = 0u;
		uint num2 = Tools.GetPetStarAndLvl(this.Data.Awake, out num);
		int @int = GameConst.GetInt32(24);
		if (num == 9u)
		{
			num2 += 1u;
		}
		return (uint)((num2 != 0u) ? (@int + (int)(num2 * 10u)) : @int);
	}

	public int GetMaxFurther(bool isIgnoneLevel = false)
	{
		int @int = GameConst.GetInt32(231);
		if (isIgnoneLevel)
		{
			return @int;
		}
		int a;
		if (this.Data.Level < 15u)
		{
			a = 1;
		}
		else if (this.Data.Level < this.Data.Level / 10u * 10u + 5u)
		{
			a = (int)(this.Data.Level / 10u);
		}
		else
		{
			a = (int)(this.Data.Level / 10u + 1u);
		}
		return Mathf.Min(a, @int);
	}

	public void GetFurtherData(out int curItemCount, out int needItemCount, out int needMoney, out int curPetCount, out int needPetCount)
	{
		curItemCount = 0;
		needItemCount = 0;
		needMoney = 0;
		curPetCount = 0;
		needPetCount = 0;
		if ((ulong)this.Data.Further >= (ulong)((long)GameConst.GetInt32(231)))
		{
			return;
		}
		TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo((int)(this.Data.Further + 1u));
		if (info == null)
		{
			return;
		}
		if (this.Data.ID != 100uL)
		{
			if (this.Info.Quality >= 0 && this.Info.Quality < info.FurtherItemCount.Count)
			{
				needItemCount = (int)info.FurtherItemCount[this.Info.Quality];
			}
			if (this.Info.Quality >= 0 && this.Info.Quality < info.FurtherCost.Count)
			{
				needMoney = (int)info.FurtherCost[this.Info.Quality];
			}
			if (this.Info.Quality >= 0 && this.Info.Quality < info.FurtherPetCount.Count)
			{
				needPetCount = (int)info.FurtherPetCount[this.Info.Quality];
			}
			curPetCount = Globals.Instance.Player.PetSystem.GetFurtherPetCount(this.Data.ID, this.Data.InfoID);
		}
		else
		{
			needItemCount = (int)info.PFurtherItemCount;
			needMoney = (int)info.PFurtherCost;
		}
		curItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(100));
	}

	public uint GetToExp()
	{
		if (this.Data.Level <= 0u || this.Data.Level > 150u || this.Info.Quality < 0 || this.Info.Quality >= 4)
		{
			return 0u;
		}
		uint num = LevelExp.TotalExp[(int)((UIntPtr)(this.Data.Level - 1u)), this.Info.Quality];
		num += this.Data.Exp;
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.Info.Quality + 1);
		if (info != null)
		{
			num += info.PetExp;
		}
		return num;
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
		if (level >= Globals.Instance.Player.Data.Level)
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
			if (num < info.Exp[this.Info.Quality])
			{
				break;
			}
			level += 1u;
			num -= info.Exp[this.Info.Quality];
			if (level >= Globals.Instance.Player.Data.Level)
			{
				result = true;
				break;
			}
			info = Globals.Instance.AttDB.LevelDict.GetInfo((int)level);
		}
		expRate = num * 100u / info.Exp[this.Info.Quality];
		return result;
	}

	public bool GetLevelupDetailInfo(uint exp, out uint level, out uint maxHP, out uint attack, out uint physicDefense, out uint magicDefense, out uint expRate, out uint overflowExp)
	{
		bool levelupInfo = this.GetLevelupInfo(exp, out level, out expRate, out overflowExp);
		maxHP = 0u;
		attack = 0u;
		physicDefense = 0u;
		magicDefense = 0u;
		if (level > this.Data.Level)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo((int)this.Data.Further);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("TinyLevelDict.GetInfo error, id = {0}", this.Data.Further)
				});
				return false;
			}
			if (this.Info.Quality >= 0 && this.Info.Quality < info.PetMaxHPFurther.Count)
			{
				num = (float)info.PetMaxHPFurther[this.Info.Quality];
			}
			if (this.Info.Quality >= 0 && this.Info.Quality < info.PetAttackFurther.Count)
			{
				num2 = (float)info.PetAttackFurther[this.Info.Quality];
			}
			if (this.Info.Quality >= 0 && this.Info.Quality < info.PetPhysicDefenseFurther.Count)
			{
				num3 = (float)info.PetPhysicDefenseFurther[this.Info.Quality];
			}
			if (this.Info.Quality >= 0 && this.Info.Quality < info.PetMagicDefenseFurther.Count)
			{
				num4 = (float)info.PetMagicDefenseFurther[this.Info.Quality];
			}
			maxHP = (uint)((level - this.Data.Level) * ((float)this.Info.MaxHPInc + num));
			attack = (uint)((level - this.Data.Level) * ((float)this.Info.AttackInc + num2));
			physicDefense = (uint)((level - this.Data.Level) * ((float)this.Info.PhysicDefenseInc + num3));
			magicDefense = (uint)((level - this.Data.Level) * ((float)this.Info.MagicDefenseInc + num4));
		}
		return levelupInfo;
	}

	public int GetPlayerSkillID()
	{
		if (this.Info.PlayerSkillID == 0)
		{
			return 0;
		}
		return this.Info.PlayerSkillID + (int)(this.Data.SkillLevel & 255u);
	}

	public int GetSkillLevel(int slot)
	{
		if (slot < 0 || slot >= 4 || slot >= this.Info.SkillID.Count)
		{
			return 0;
		}
		return (int)(this.Data.SkillLevel >> slot * 8 & 255u);
	}

	public int GetSkillID(int slot)
	{
		if (slot < 0 || slot >= 4 || slot >= this.Info.SkillID.Count)
		{
			return 0;
		}
		if (this.Info.SkillID[slot] == 0)
		{
			return 0;
		}
		if (slot == 0)
		{
			return this.Info.SkillID[slot];
		}
		return this.Info.SkillID[slot] + (int)(this.Data.SkillLevel >> slot * 8 & 255u);
	}

	public SkillInfo GetPlayerSkillInfo()
	{
		int playerSkillID = this.GetPlayerSkillID();
		if (playerSkillID == 0)
		{
			return null;
		}
		return Globals.Instance.AttDB.SkillDict.GetInfo(playerSkillID);
	}

	public SkillInfo GetSkillInfo(int slot)
	{
		int skillID = this.GetSkillID(slot);
		if (skillID == 0)
		{
			return null;
		}
		return Globals.Instance.AttDB.SkillDict.GetInfo(skillID);
	}

	public void GetSkillCost(int index, out int curItemCount, out int needItemCount, out int needMoney)
	{
		curItemCount = 0;
		needItemCount = 0;
		needMoney = 0;
		int skillLevel = this.GetSkillLevel(index);
		if (skillLevel >= GameConst.GetInt32(232))
		{
			return;
		}
		TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(skillLevel + 1);
		if (info == null)
		{
			return;
		}
		needItemCount = (int)info.SkillItemCount;
		needMoney = (int)info.SkillCost;
		curItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(101));
	}

	public void GetRebornData(out uint petCount, out uint money, out uint furtherItemCount, out uint skillItemCount, out uint[] itemCount, out uint awakeItemCount, ref List<OpenLootData> awakeData, out uint cultivateItemCount, bool broken = false)
	{
		petCount = 0u;
		money = 0u;
		furtherItemCount = 0u;
		skillItemCount = 0u;
		awakeItemCount = 0u;
		cultivateItemCount = 0u;
		itemCount = new uint[GameConst.PET_EXP_ITEM_ID.Length];
		LevelExp.GetTotalFurtherData((int)this.Data.Further, this.Info.Quality, out furtherItemCount, out petCount);
		petCount += 1u;
		if (this.Data.Level > 1u)
		{
			money = LevelExp.TotalExp[(int)((UIntPtr)(this.Data.Level - 1u)), this.Info.Quality];
		}
		money += this.Data.Exp;
		int num = (int)money;
		if (broken)
		{
			QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.Info.Quality + 1);
			if (info != null)
			{
				money += info.PetExp;
			}
		}
		money /= 5u;
		if (num > 0)
		{
			for (int i = itemCount.Length - 1; i >= 0; i--)
			{
				if (num <= 0)
				{
					break;
				}
				ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.PET_EXP_ITEM_ID[i]);
				if (info2 == null)
				{
					global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
					{
						GameConst.PET_EXP_ITEM_ID[i]
					});
					return;
				}
				itemCount[i] = (uint)(num / info2.Value1);
				num -= (int)(itemCount[i] * (uint)info2.Value1);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			skillItemCount += LevelExp.GetTotalSkillItemCount(this.GetSkillLevel(j));
		}
		if (this.Data.Awake > 0u || this.Data.ItemFlag != 0u)
		{
			int num2 = 1;
			while ((long)num2 <= (long)((ulong)(this.Data.Awake + 1u)))
			{
				AwakeInfo info3 = Globals.Instance.AttDB.AwakeDict.GetInfo(num2);
				if (info3 == null)
				{
					global::Debug.LogErrorFormat("AwakeDict.GetInfo error, id = {0}", new object[]
					{
						num2
					});
				}
				else
				{
					if ((long)num2 <= (long)((ulong)this.Data.Awake))
					{
						if (info3.PetCount > 0)
						{
							petCount += (uint)info3.PetCount;
						}
						if (this.socketSlot == 0)
						{
							awakeItemCount += (uint)info3.PlayerItemCount;
						}
						else
						{
							awakeItemCount += (uint)info3.ItemCount;
						}
						money += (uint)info3.Money;
					}
					for (int k = 0; k < 4; k++)
					{
						if ((long)num2 != (long)((ulong)(this.Data.Awake + 1u)) || this.IsAwakeItemEquip(k))
						{
							int num3 = this.Info.ElementType * 4 + k;
							if (num3 >= 0 && num3 < info3.ItemID.Count)
							{
								if (info3.ItemID[num3] != 0)
								{
									bool flag = false;
									for (int l = 0; l < awakeData.Count; l++)
									{
										if (awakeData[l].InfoID == info3.ItemID[num3])
										{
											flag = true;
											awakeData[l].Count += 1u;
											break;
										}
									}
									if (!flag)
									{
										OpenLootData openLootData = new OpenLootData();
										openLootData.InfoID = info3.ItemID[num3];
										openLootData.Count = 1u;
										awakeData.Add(openLootData);
									}
								}
							}
						}
					}
				}
				num2++;
			}
		}
		cultivateItemCount = (uint)this.Data.CultivateCount;
	}

	public void GetBreakData(out uint value, out uint money, out uint furtherItemCount, out uint skillItemCount, out uint[] itemCount, out uint starSoul, out uint awakeItemCount, out uint cultivateItemCount)
	{
		starSoul = 0u;
		List<OpenLootData> list = new List<OpenLootData>();
		this.GetRebornData(out value, out money, out furtherItemCount, out skillItemCount, out itemCount, out awakeItemCount, ref list, out cultivateItemCount, true);
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.Info.Quality + 1);
		if (info == null)
		{
			global::Debug.LogErrorFormat("QualityDict.GetInfo error, id = {0}", new object[]
			{
				this.Info.Quality + 1
			});
			return;
		}
		value *= info.PetValue;
		for (int i = 0; i < list.Count; i++)
		{
			ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(list[i].InfoID);
			if (info2 != null)
			{
				info = Globals.Instance.AttDB.QualityDict.GetInfo(info2.Quality + 1);
				if (info != null)
				{
					starSoul += (uint)(info.AwakeBreakupValue * (int)list[i].Count);
				}
			}
		}
	}

	public void SetRelationActive(int value)
	{
		this.Relation = value;
	}

	public int GetAwakeItemID(int index)
	{
		AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo((int)(this.Data.Awake + 1u));
		if (info == null)
		{
			return 0;
		}
		index = this.Info.ElementType * 4 + index;
		if (index < 0 || index >= info.ItemID.Count)
		{
			return 0;
		}
		return info.ItemID[index];
	}

	public bool IsAwakeNeedItem(int infoId)
	{
		AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo((int)(this.Data.Awake + 1u));
		if (info != null)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = this.Info.ElementType * 4 + i;
				if (num >= 0 && num < info.ItemID.Count && info.ItemID[num] == infoId && !this.IsAwakeItemEquip(num))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsAwakeItemEquip(int index)
	{
		return ((ulong)this.Data.ItemFlag & (ulong)(1L << (index & 31))) != 0uL;
	}

	public void GetAwakeLevelupData(out int money, out int petCount, out int awakeItemCount, out int curAwakeItemCount)
	{
		money = 0;
		petCount = 0;
		awakeItemCount = 0;
		curAwakeItemCount = 0;
		AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo((int)(this.Data.Awake + 1u));
		if (info == null)
		{
			return;
		}
		money = info.Money;
		if (this.Info.ID == 90000)
		{
			awakeItemCount = info.PlayerItemCount;
		}
		else
		{
			petCount = info.PetCount;
			awakeItemCount = info.ItemCount;
		}
		curAwakeItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(118));
	}
}
