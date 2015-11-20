using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataEx : BaseData
{
	public int equipSlot = -1;

	public bool BtnsVisible;

	public bool IsSelected;

	public int RelationCount;

	public ItemData Data
	{
		get;
		private set;
	}

	public ItemInfo Info
	{
		get;
		private set;
	}

	public ItemDataEx(ItemData data, ItemInfo info)
	{
		this.Data = data;
		this.Info = info;
		this.equipSlot = -1;
	}

	public void ClearUIData()
	{
		this.BtnsVisible = false;
		this.IsSelected = false;
		this.RelationCount = 0;
	}

	public override ulong GetID()
	{
		return this.Data.ID;
	}

	public void SetEquipSlot(int slot)
	{
		this.equipSlot = slot;
	}

	public int GetEquipSlot()
	{
		return this.equipSlot % 6;
	}

	public int GetSocketSlot()
	{
		if (this.equipSlot < 0)
		{
			return -1;
		}
		return this.equipSlot / 6;
	}

	public bool IsEquiped()
	{
		return this.equipSlot >= 0;
	}

	public PetDataEx GetEquipPet()
	{
		int socketSlot = this.GetSocketSlot();
		if (socketSlot < 0)
		{
			return null;
		}
		return Globals.Instance.Player.TeamSystem.GetPet(socketSlot);
	}

	public int GetCount()
	{
		if (this.Info.Stackable)
		{
			return this.Data.Value1;
		}
		return 1;
	}

	public int GetAmount2Create()
	{
		if (this.Info.Type != 3)
		{
			return 0;
		}
		return this.Info.Value1;
	}

	public ItemSetInfo GetItemSetInfo(out int count, out int mask, bool isLocal = true)
	{
		count = 0;
		mask = 0;
		if (this.Info.Type != 0)
		{
			return null;
		}
		ItemSetInfo info = Globals.Instance.AttDB.ItemSetDict.GetInfo(this.Info.Value5);
		if (info == null)
		{
			return null;
		}
		int socketSlot = this.GetSocketSlot();
		if (socketSlot < 0)
		{
			return info;
		}
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(socketSlot, isLocal);
		if (socket == null)
		{
			return info;
		}
		for (int i = 0; i < info.ItemID.Count; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				ItemDataEx equip = socket.GetEquip(j);
				if (equip != null && equip.Info.ID == info.ItemID[i])
				{
					count++;
					mask |= 1 << i;
					break;
				}
			}
		}
		return info;
	}

	public int GetPetExp()
	{
		if (this.Info.Type == 4 && this.Info.SubType == 1)
		{
			return this.Info.Value1;
		}
		return 0;
	}

	public bool CanCreate()
	{
		if (this.Info.Type != 3)
		{
			return false;
		}
		if (this.Info.SubType == 1 || this.Info.SubType == 0)
		{
			return this.Data.Value1 >= this.Info.Value1;
		}
		return this.Info.SubType != 2 && this.Info.SubType == 3 && this.Data.Value1 >= this.Info.Value1;
	}

	public int GetPrice()
	{
		return this.Info.Price + this.Data.Value4;
	}

	public void GetEquipBreakValue(out uint value, out uint value2, out uint money, out uint[] itemCount)
	{
		value = 0u;
		value2 = 0u;
		money = 0u;
		itemCount = new uint[GameConst.EQUIP_REFINE_ITEM_ID.Length];
		if (this.Info.Type != 0)
		{
			return;
		}
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.Info.Quality + 1);
		if (info != null)
		{
			value = info.EquipValue;
			value2 = info.EquipValue2;
			money = info.EquipMoney;
		}
		money += (uint)this.Data.Value4;
		if (this.Data.Value2 > 0 || this.Data.Value3 > 0)
		{
			int num = (int)(LevelExp.GetTotalEquipRefineExp(this.Data.Value2, this.Info.Quality) + (uint)this.Data.Value3);
			for (int i = itemCount.Length - 1; i >= 0; i--)
			{
				if (num <= 0)
				{
					break;
				}
				ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.EQUIP_REFINE_ITEM_ID[i]);
				if (info2 == null)
				{
					global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
					{
						GameConst.EQUIP_REFINE_ITEM_ID[i]
					});
					return;
				}
				itemCount[i] = (uint)(num / info2.Value1);
				num -= (int)(itemCount[i] * (uint)info2.Value1);
			}
		}
	}

	public void GetTrinketRebornValue(out uint money, out uint trinketCount, out uint refineItemCount, out uint[] itemCount, out uint boxCount)
	{
		money = (uint)this.Data.Value4;
		LevelExp.GetTotalTrinketRefineCount(this.Data.Value2, this.Info.Quality, out refineItemCount, out trinketCount);
		trinketCount += 1u;
		itemCount = new uint[GameConst.TRINKET_ENHANCE_EXP_ITEM_ID.Length];
		int num = (int)(LevelExp.TotalTrinketEnhanceExp[this.GetTrinketEnhanceLevel() - 1, this.Info.Quality] + (uint)this.Data.Value3);
		boxCount = 0u;
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(161));
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDict get info error , ID : {0}",
				GameConst.GetInt32(161)
			});
		}
		if (num > 0)
		{
			for (int i = itemCount.Length - 1; i >= 0; i--)
			{
				if (num <= 0)
				{
					break;
				}
				itemCount[i] = (uint)(num / GameConst.TRINKET_ENHANCE_EXP[i]);
				num -= (int)(itemCount[i] * (uint)GameConst.TRINKET_ENHANCE_EXP[i]);
			}
			if (info != null)
			{
				int num2 = -1;
				for (int j = 0; j < GameConst.TRINKET_ENHANCE_EXP_ITEM_ID.Length; j++)
				{
					if (GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[j] == info.Value1)
					{
						num2 = j;
						break;
					}
				}
				if (num2 >= 0)
				{
					boxCount = itemCount[num2] / (uint)info.Value2;
					itemCount[num2] %= (uint)info.Value2;
				}
			}
		}
	}

	public bool CanEnhance()
	{
		if (this.Info.Type == 0)
		{
			return this.GetEquipEnhanceLevel() < Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(true);
		}
		return this.Info.Type == 1 && this.GetTrinketEnhanceLevel() < Globals.Instance.Player.ItemSystem.GetMaxTrinketEnhanceLevel();
	}

	public bool CanRefine()
	{
		if (this.Info.Type == 0)
		{
			return this.GetEquipRefineLevel() < Globals.Instance.Player.ItemSystem.GetMaxEquipRefineLevel();
		}
		return this.Info.Type == 1 && this.GetTrinketRefineLevel() < Globals.Instance.Player.ItemSystem.GetMaxTrinketRefineLevel();
	}

	public int GetEquipEnhanceAttDelta()
	{
		if (this.Info.Type != 0)
		{
			return 0;
		}
		return this.Info.Value1;
	}

	public int GetEquipEnhanceAttValue()
	{
		if (this.Info.Type != 0)
		{
			return 0;
		}
		return this.Data.Value1 * this.Info.Value1;
	}

	public int GetEquipRefineAttDelta0()
	{
		if (this.Info.Type != 0)
		{
			return 0;
		}
		return this.Info.Value3;
	}

	public float GetEquipRefineAttDelta1()
	{
		if (this.Info.Type != 0)
		{
			return 0f;
		}
		return (float)this.Info.Value4 / 100f;
	}

	public int GetEquipRefineAttValue0()
	{
		if (this.Info.Type != 0)
		{
			return 0;
		}
		return this.Data.Value2 * this.Info.Value3;
	}

	public float GetEquipRefineAttValue1()
	{
		if (this.Info.Type != 0)
		{
			return 0f;
		}
		return (float)(this.Data.Value2 * this.Info.Value4) / 100f;
	}

	public int GetTrinketEnhanceAttDelta0()
	{
		if (this.Info.Type != 1)
		{
			return 0;
		}
		return this.Info.Value1;
	}

	public float GetTrinketEnhanceAttDelta1()
	{
		if (this.Info.Type != 1)
		{
			return 0f;
		}
		return (float)this.Info.Value2 / 100f;
	}

	public float GetTrinketRefineAttDelta0()
	{
		if (this.Info.Type != 1)
		{
			return 0f;
		}
		return (float)this.Info.Value3 / 100f;
	}

	public float GetTrinketRefineAttDelta1()
	{
		if (this.Info.Type != 1)
		{
			return 0f;
		}
		return (float)this.Info.Value4 / 100f;
	}

	public int GetTrinketEnhanceAttValue0()
	{
		if (this.Info.Type != 1)
		{
			return 0;
		}
		return this.Data.Value1 * this.Info.Value1;
	}

	public float GetTrinketEnhanceAttValue1()
	{
		if (this.Info.Type != 1)
		{
			return 0f;
		}
		return (float)(this.Data.Value1 * this.Info.Value2) / 100f;
	}

	public float GetTrinketRefineAttValue0()
	{
		if (this.Info.Type != 1)
		{
			return 0f;
		}
		return (float)(this.Data.Value2 * this.Info.Value3) / 100f;
	}

	public float GetTrinketRefineAttValue1()
	{
		if (this.Info.Type != 1)
		{
			return 0f;
		}
		return (float)(this.Data.Value2 * this.Info.Value4) / 100f;
	}

	public bool IsEnhanceMax()
	{
		if (this.Info.Type == 0)
		{
			return this.GetEquipEnhanceLevel() >= GameConst.GetInt32(227);
		}
		return this.Info.Type == 1 && this.GetTrinketEnhanceLevel() >= GameConst.GetInt32(229);
	}

	public bool IsRefineMax()
	{
		if (this.Info.Type == 0)
		{
			return this.GetEquipRefineLevel() >= GameConst.GetInt32(228);
		}
		return this.Info.Type == 1 && this.GetTrinketRefineLevel() >= GameConst.GetInt32(230);
	}

	public int GetEquipEnhanceLevel()
	{
		return this.Data.Value1;
	}

	public uint GetEquipEnhanceCost()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo(this.Data.Value1);
		if (info != null && this.Info.Quality >= 0 && this.Info.Quality < info.EnhanceCost.Count)
		{
			return info.EnhanceCost[this.Info.Quality];
		}
		return 0u;
	}

	public int GetEquipRefineLevel()
	{
		return this.Data.Value2;
	}

	public int GetEquipRefineExp()
	{
		return this.Data.Value3;
	}

	public uint GetEquipRefineMaxExp()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo(this.Data.Value2 + 1);
		if (info != null && this.Info.Quality >= 0 && this.Info.Quality < info.RefineExp.Count)
		{
			return info.RefineExp[this.Info.Quality];
		}
		return 0u;
	}

	public int GetEquipRefineExp2Upgrade()
	{
		return (int)(this.GetEquipRefineMaxExp() - (uint)this.GetEquipRefineExp());
	}

	public int GetTrinketOrItem2EnhanceExp()
	{
		if (this.Info.Type == 1)
		{
			return (int)((ulong)LevelExp.TotalTrinketEnhanceExp[this.GetTrinketEnhanceLevel() - 1, this.Info.Quality] + (ulong)((long)this.GetTrinketOrItem2EnhanceCost()) + (ulong)((long)this.GetTrinketEnhanceExp()));
		}
		if (this.Info.Type == 4 && this.Info.SubType == 9)
		{
			return this.Info.Value1;
		}
		if (this.Info.Type == 3 && this.Info.SubType == 2)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(this.Info.Value2);
			if (info == null)
			{
				global::Debug.LogErrorFormat("ItemDict get info error , ID : {0}", new object[]
				{
					this.Info.Value2
				});
				return 0;
			}
			if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(info.ID))
			{
				return info.Value1;
			}
		}
		return 0;
	}

	public int GetTrinketOrItem2EnhanceCost()
	{
		return (int)Tools.GetTrinketEnhanceItemExp(this.Info.Quality);
	}

	public bool IsTrinketAndCastItem()
	{
		return this.Info.Type == 1 && this.GetTrinketRefineLevel() <= 0 && this.GetTrinketEnhanceLevel() <= 1 && !this.IsEquiped() && this.GetTrinketEnhanceExp() <= 0;
	}

	public bool IsTrinketAndCanReborn()
	{
		return this.Info.Type == 1 && !this.IsEquiped() && (this.GetTrinketRefineLevel() > 0 || this.GetTrinketEnhanceLevel() > 1 || this.GetTrinketEnhanceExp() > 0);
	}

	public int GetTrinketEnhanceLevel()
	{
		return this.Data.Value1;
	}

	public int GetTrinketEnhanceExp()
	{
		return this.Data.Value3;
	}

	public uint GetTrinketEnhanceMaxExp()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo(this.Data.Value1);
		if (info != null && this.Info.Quality >= 0 && this.Info.Quality < info.EnhanceExp.Count)
		{
			return info.EnhanceExp[this.Info.Quality];
		}
		return 0u;
	}

	public uint GetTrinketEnhanceMaxExp1(int level)
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo(level);
		if (info != null && this.Info.Quality >= 0 && this.Info.Quality < info.EnhanceExp.Count)
		{
			return info.EnhanceExp[this.Info.Quality];
		}
		return 0u;
	}

	public int GetTrinketEnhanceExp2Upgrade()
	{
		int num = (int)(this.GetTrinketEnhanceMaxExp() - (uint)this.GetTrinketEnhanceExp());
		return Mathf.Clamp(num, 0, num);
	}

	public int GetTrinketEnhanceLevelWithExp(int exp)
	{
		int num = this.GetTrinketEnhanceLevel();
		int trinketEnhanceExp2Upgrade = this.GetTrinketEnhanceExp2Upgrade();
		if (exp <= 0 || trinketEnhanceExp2Upgrade <= 0 || exp < trinketEnhanceExp2Upgrade)
		{
			return num;
		}
		num++;
		exp -= trinketEnhanceExp2Upgrade;
		while (num < Globals.Instance.Player.ItemSystem.GetMaxTrinketEnhanceLevel() && (long)exp >= (long)((ulong)this.GetTrinketEnhanceMaxExp1(num)))
		{
			exp -= (int)this.GetTrinketEnhanceMaxExp1(num);
			num++;
		}
		return num;
	}

	public int GetTrinketEnhanceLevelWithItems(List<ItemDataEx> items)
	{
		if (items == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDataEx List is null"
			});
			return 0;
		}
		int num = 0;
		foreach (ItemDataEx current in items)
		{
			num += current.GetTrinketOrItem2EnhanceExp();
		}
		return this.GetTrinketEnhanceLevelWithExp(num);
	}

	public int GetTrinketRefineLevel()
	{
		return this.Data.Value2;
	}

	public void GetTrinketRefineCost(out int curItemCount, out int needItemCount, out int needMoney, out int needTrinketCount)
	{
		curItemCount = 0;
		needItemCount = 0;
		needMoney = 0;
		needTrinketCount = 0;
		TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(this.Data.Value2 + 1);
		if (info == null)
		{
			return;
		}
		if (this.Info.Quality >= 0 && this.Info.Quality < info.RefineItemCount.Count)
		{
			needItemCount = (int)info.RefineItemCount[this.Info.Quality];
		}
		if (this.Info.Quality >= 0 && this.Info.Quality < info.RefineCost.Count)
		{
			needMoney = (int)info.RefineCost[this.Info.Quality];
		}
		if (this.Info.Quality >= 0 && this.Info.Quality < info.RefineTrinketCount.Count)
		{
			needTrinketCount = (int)info.RefineTrinketCount[this.Info.Quality];
		}
		curItemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(102));
	}

	public bool IsTrinketEnoughItem2Refine()
	{
		int num;
		int num2;
		int num3;
		int num4;
		this.GetTrinketRefineCost(out num, out num2, out num3, out num4);
		return this.CanRefine() && num >= num2 && num4 <= Globals.Instance.Player.ItemSystem.GetTrinketRefineTrinketCount(this);
	}

	public bool CanActiveRelation(List<int> equips)
	{
		return equips != null && equips.Count != 0 && equips.Contains(this.Data.InfoID);
	}
}
