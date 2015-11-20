using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class ItemSubSystem : ISubSystem
{
	public delegate void AddItemCallback(ItemDataEx data);

	public delegate void RemoveItemCallback(ulong id);

	public delegate void UpdateItemCallback(ItemDataEx data);

	public delegate void UpdateFashionCallback(int infoID);

	public ItemSubSystem.AddItemCallback AddItemEvent;

	public ItemSubSystem.RemoveItemCallback RemoveItemEvent;

	public ItemSubSystem.UpdateItemCallback UpdateItemEvent;

	public ItemSubSystem.UpdateFashionCallback AddFashionEvent;

	public ItemSubSystem.UpdateFashionCallback RemoveFashionEvent;

	public ItemSubSystem.UpdateFashionCallback UpdateFashionEvent;

	private Dictionary<ulong, ItemDataEx> items = new Dictionary<ulong, ItemDataEx>();

	private List<int> fashions = new List<int>();

	private List<int> mFashionTimes = new List<int>();

	private int awakeItemCost;

	private Dictionary<int, int> awakeItems = new Dictionary<int, int>();

	public uint Version
	{
		get;
		private set;
	}

	public ICollection<ItemDataEx> Values
	{
		get
		{
			return this.items.Values;
		}
	}

	public uint FashionVersion
	{
		get;
		private set;
	}

	public List<int> Fashions
	{
		get
		{
			return this.fashions;
		}
	}

	public List<int> FashionTimes
	{
		get
		{
			return this.mFashionTimes;
		}
	}

	public ItemSubSystem()
	{
		this.Version = 0u;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(500, new ClientSession.MsgHandler(this.OnMsgAddItem));
		Globals.Instance.CliSession.Register(501, new ClientSession.MsgHandler(this.OnMsgUpdateItem));
		Globals.Instance.CliSession.Register(518, new ClientSession.MsgHandler(this.OnMsgUpdateItemData));
		Globals.Instance.CliSession.Register(528, new ClientSession.MsgHandler(this.OnMsgAddFashion));
		Globals.Instance.CliSession.Register(529, new ClientSession.MsgHandler(this.OnMsgRemoveFashion));
		Globals.Instance.CliSession.Register(546, new ClientSession.MsgHandler(this.OnMsgUpdateFashion));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Version = 0u;
		this.items.Clear();
		this.FashionVersion = 0u;
		this.fashions.Clear();
		this.mFashionTimes.Clear();
	}

	public void LoadData(uint version, List<ItemData> data, uint fashionVersion, List<int> fashionData, List<int> fashionTimesData)
	{
		if (version != 0u && version != this.Version)
		{
			this.Version = version;
			this.items.Clear();
			for (int i = 0; i < data.Count; i++)
			{
				this.AddItem(data[i]);
			}
		}
		if (fashionVersion != 0u && fashionVersion != this.FashionVersion)
		{
			this.FashionVersion = fashionVersion;
			this.fashions = fashionData;
			this.mFashionTimes = fashionTimesData;
		}
	}

	public ItemDataEx GetItem(ulong id)
	{
		ItemDataEx result = null;
		this.items.TryGetValue(id, out result);
		return result;
	}

	public ItemDataEx GetItemByInfoID(int infoID)
	{
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Data.InfoID == infoID)
			{
				return current;
			}
		}
		return null;
	}

	public List<ItemDataEx> GetItemsByInfoID(int infoID)
	{
		List<ItemDataEx> list = new List<ItemDataEx>();
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Data.InfoID == infoID)
			{
				list.Add(current);
			}
		}
		return list;
	}

	public int GetItemCount(int infoID)
	{
		ItemDataEx itemByInfoID = this.GetItemByInfoID(infoID);
		if (itemByInfoID != null)
		{
			return itemByInfoID.GetCount();
		}
		return 0;
	}

	public int GetEquipCount(int infoID)
	{
		int num = 0;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.ID == infoID)
			{
				num++;
			}
		}
		return num;
	}

	private void AddItem(ItemData data)
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(data.InfoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
			{
				data.InfoID
			});
			return;
		}
		ItemDataEx itemDataEx = new ItemDataEx(data, info);
		this.items.Add(itemDataEx.Data.ID, itemDataEx);
	}

	private void RemoveItem(ulong id)
	{
		if (this.GetItem(id) == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("GetItem error, ID = {0}", id)
			});
			return;
		}
		this.items.Remove(id);
	}

	public void OnMsgAddItem(MemoryStream stream)
	{
		MS2C_AddItem mS2C_AddItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddItem), stream) as MS2C_AddItem;
		this.AddItem(mS2C_AddItem.Data);
		ItemDataEx item = this.GetItem(mS2C_AddItem.Data.ID);
		if (item == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("GetItem error, itemID = {0}", mS2C_AddItem.Data.ID)
			});
			return;
		}
		if (mS2C_AddItem.ItemVersion != 0u)
		{
			this.Version = mS2C_AddItem.ItemVersion;
		}
		if (this.AddItemEvent != null)
		{
			this.AddItemEvent(item);
		}
	}

	public void OnMsgUpdateItem(MemoryStream stream)
	{
		MS2C_UpdateItem mS2C_UpdateItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateItem), stream) as MS2C_UpdateItem;
		for (int i = 0; i < mS2C_UpdateItem.Data.Count; i++)
		{
			if (mS2C_UpdateItem.Data[i].Count == 0)
			{
				this.RemoveItem(mS2C_UpdateItem.Data[i].ItemID);
				if (this.RemoveItemEvent != null)
				{
					this.RemoveItemEvent(mS2C_UpdateItem.Data[i].ItemID);
				}
			}
			else
			{
				ItemDataEx item = this.GetItem(mS2C_UpdateItem.Data[i].ItemID);
				if (item == null)
				{
					global::Debug.LogError(new object[]
					{
						string.Format("GetItem error, itemID = {0}", mS2C_UpdateItem.Data[i].ItemID)
					});
					return;
				}
				item.Data.Value1 = mS2C_UpdateItem.Data[i].Count;
				if (this.UpdateItemEvent != null)
				{
					this.UpdateItemEvent(item);
				}
			}
		}
		if (mS2C_UpdateItem.ItemVersion != 0u)
		{
			this.Version = mS2C_UpdateItem.ItemVersion;
		}
	}

	public void OnMsgUpdateItemData(MemoryStream stream)
	{
		MS2C_UpdateItemData mS2C_UpdateItemData = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateItemData), stream) as MS2C_UpdateItemData;
		ItemDataEx item = this.GetItem(mS2C_UpdateItemData.Data.ID);
		if (item == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("GetItem error, itemID = {0}", mS2C_UpdateItemData.Data.ID)
			});
			return;
		}
		int value = item.Data.Value1;
		int value2 = item.Data.Value2;
		if (mS2C_UpdateItemData.Data.Value1 != 0)
		{
			if (mS2C_UpdateItemData.Data.Value1 == -1)
			{
				item.Data.Value1 = 0;
			}
			else
			{
				item.Data.Value1 = mS2C_UpdateItemData.Data.Value1;
			}
		}
		if (mS2C_UpdateItemData.Data.Value2 != 0)
		{
			if (mS2C_UpdateItemData.Data.Value2 == -1)
			{
				item.Data.Value2 = 0;
			}
			else
			{
				item.Data.Value2 = mS2C_UpdateItemData.Data.Value2;
			}
		}
		if (mS2C_UpdateItemData.Data.Value3 != 0)
		{
			if (mS2C_UpdateItemData.Data.Value3 == -1)
			{
				item.Data.Value3 = 0;
			}
			else
			{
				item.Data.Value3 = mS2C_UpdateItemData.Data.Value3;
			}
		}
		if (mS2C_UpdateItemData.Data.Value4 != 0)
		{
			if (mS2C_UpdateItemData.Data.Value4 == -1)
			{
				item.Data.Value4 = 0;
			}
			else
			{
				item.Data.Value4 = mS2C_UpdateItemData.Data.Value4;
			}
		}
		int value3 = item.Data.Value1;
		int value4 = item.Data.Value2;
		if (item.GetSocketSlot() >= 0)
		{
			Globals.Instance.Player.TeamSystem.OnItemEnhance(item.GetSocketSlot(), item.GetEquipSlot(), value3 - value, value4 - value2);
		}
		if (this.UpdateItemEvent != null)
		{
			this.UpdateItemEvent(item);
		}
		if (mS2C_UpdateItemData.ItemVersion != 0u)
		{
			this.Version = mS2C_UpdateItemData.ItemVersion;
		}
	}

	public void OnMsgAddFashion(MemoryStream stream)
	{
		MS2C_AddFashion mS2C_AddFashion = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddFashion), stream) as MS2C_AddFashion;
		this.fashions.Add(mS2C_AddFashion.FashionID);
		this.mFashionTimes.Add(mS2C_AddFashion.FashionTime);
		GameCache.Data.HasNewFashion = true;
		GameCache.UpdateNow = true;
		if (mS2C_AddFashion.FashionVersion != 0u)
		{
			this.FashionVersion = mS2C_AddFashion.FashionVersion;
		}
		Globals.Instance.Player.TeamSystem.OnFashionUpdate();
		if (this.AddFashionEvent != null)
		{
			this.AddFashionEvent(mS2C_AddFashion.FashionID);
		}
	}

	public int GetFashionTime(int fashionID)
	{
		int num = 0;
		while (num < this.fashions.Count && num < this.mFashionTimes.Count)
		{
			if (this.fashions[num] == fashionID)
			{
				return this.mFashionTimes[num];
			}
			num++;
		}
		return 0;
	}

	public int GetFashionIndex(int fashionID)
	{
		int num = 0;
		while (num < this.fashions.Count && num < this.mFashionTimes.Count)
		{
			if (this.fashions[num] == fashionID)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public void OnMsgRemoveFashion(MemoryStream stream)
	{
		MS2C_RemoveFashion mS2C_RemoveFashion = Serializer.NonGeneric.Deserialize(typeof(MS2C_RemoveFashion), stream) as MS2C_RemoveFashion;
		int fashionIndex = this.GetFashionIndex(mS2C_RemoveFashion.FashionID);
		if (0 <= fashionIndex && fashionIndex < this.mFashionTimes.Count)
		{
			this.mFashionTimes.RemoveAt(fashionIndex);
		}
		this.fashions.Remove(mS2C_RemoveFashion.FashionID);
		if (mS2C_RemoveFashion.FashionVersion != 0u)
		{
			this.FashionVersion = mS2C_RemoveFashion.FashionVersion;
		}
		Globals.Instance.Player.TeamSystem.OnFashionUpdate();
		if (this.RemoveFashionEvent != null)
		{
			this.RemoveFashionEvent(mS2C_RemoveFashion.FashionID);
		}
	}

	private void OnMsgUpdateFashion(MemoryStream stream)
	{
		MS2C_UpdateFashion mS2C_UpdateFashion = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateFashion), stream) as MS2C_UpdateFashion;
		if (this.HasFashion(mS2C_UpdateFashion.FashionID))
		{
			int fashionIndex = this.GetFashionIndex(mS2C_UpdateFashion.FashionID);
			if (0 <= fashionIndex && fashionIndex < this.mFashionTimes.Count)
			{
				this.mFashionTimes[fashionIndex] = mS2C_UpdateFashion.FashionTime;
			}
		}
		if (mS2C_UpdateFashion.FashionVersion != 0u)
		{
			this.FashionVersion = mS2C_UpdateFashion.FashionVersion;
		}
		Globals.Instance.Player.TeamSystem.OnFashionUpdate();
		if (this.UpdateFashionEvent != null)
		{
			this.UpdateFashionEvent(mS2C_UpdateFashion.FashionID);
		}
	}

	public int GetValidFashionCount()
	{
		int num = 0;
		int num2 = 0;
		while (num2 < this.fashions.Count && num2 < this.mFashionTimes.Count)
		{
			FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(this.fashions[num2]);
			if (info != null)
			{
				if (info.Gender - 1 == Globals.Instance.Player.Data.Gender)
				{
					if (this.mFashionTimes[num2] == 0 || Globals.Instance.Player.GetTimeStamp() < this.mFashionTimes[num2])
					{
						num++;
					}
				}
			}
			num2++;
		}
		return num;
	}

	public int GetValidShiXiaoCount()
	{
		int num = 0;
		int num2 = 0;
		while (num2 < this.fashions.Count && num2 < this.mFashionTimes.Count)
		{
			FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(this.fashions[num2]);
			if (info != null)
			{
				if (info.Gender - 1 == Globals.Instance.Player.Data.Gender)
				{
					if (Globals.Instance.Player.GetTimeStamp() < this.mFashionTimes[num2])
					{
						num++;
					}
				}
			}
			num2++;
		}
		return num;
	}

	public bool HasFashion(int id)
	{
		return this.fashions.Contains(id);
	}

	public bool IsShiXiaoFashion(FashionInfo fInfo)
	{
		return fInfo != null && fInfo.Day > 0;
	}

	public bool IsShiXiaoFashion(int id)
	{
		FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(id);
		return info != null && info.Day > 0;
	}

	public bool IsFashionGuoqi(int id)
	{
		bool result = false;
		if (this.HasFashion(id))
		{
			int fashionTime = this.GetFashionTime(id);
			if (fashionTime > 0 && Globals.Instance.Player.GetTimeStamp() >= fashionTime)
			{
				result = true;
			}
		}
		return result;
	}

	public List<ItemDataEx> GetAllCanSaleEquips()
	{
		List<ItemDataEx> list = new List<ItemDataEx>();
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 0 && !current.IsEquiped())
			{
				list.Add(current);
			}
		}
		return list;
	}

	public int GetTrinketRefineTrinketCount(ItemDataEx self)
	{
		if (self.Info.Type != 1)
		{
			return 0;
		}
		int num = 0;
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.ID == self.Info.ID && !current.IsEquiped() && current != self && current.Data.Value1 == 1 && current.Data.Value2 == 0 && current.Data.Value3 == 0 && current.Data.Value4 == 0)
			{
				num++;
			}
		}
		return num;
	}

	public List<ItemDataEx> GetAllEquipTrinketBySlot(int equipSlot, bool ShowEquiped = false)
	{
		List<ItemDataEx> list = new List<ItemDataEx>();
		if (equipSlot < 0 || equipSlot > 6 - 1)
		{
			global::Debug.LogErrorFormat("EquipSlot Error, slot : {0} ", new object[]
			{
				equipSlot
			});
			return list;
		}
		if (equipSlot < 4)
		{
			foreach (ItemDataEx current in this.Values)
			{
				if (current.Info.Type == 0 && current.Info.SubType == equipSlot)
				{
					if (ShowEquiped || !current.IsEquiped())
					{
						list.Add(current);
					}
				}
			}
		}
		else
		{
			int num = equipSlot - 4;
			foreach (ItemDataEx current2 in this.Values)
			{
				if (current2.Info.Type == 1 && current2.Info.SubType == num)
				{
					bool flag = false;
					for (int i = 0; i < GameConst.TRINKET_ENHANCE_EXP_ITEM_ID.Length; i++)
					{
						if (current2.Info.ID == GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						if (ShowEquiped || !current2.IsEquiped())
						{
							list.Add(current2);
						}
					}
				}
			}
		}
		return list;
	}

	public void GetAllEquip2BreakUp(out List<ItemDataEx> list, out bool hasOrangeEquip)
	{
		hasOrangeEquip = false;
		list = new List<ItemDataEx>();
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 0 && !current.IsEquiped())
			{
				if (current.Info.Quality >= 2)
				{
					hasOrangeEquip = true;
				}
				else
				{
					list.Add(current);
				}
			}
		}
	}

	public bool HasTrinket2Reborn()
	{
		foreach (ItemDataEx current in this.Values)
		{
			if (current.IsTrinketAndCanReborn())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsTrinketEnhanceExp(int infoID)
	{
		for (int i = 0; i < GameConst.TRINKET_ENHANCE_EXP_ITEM_ID.Length; i++)
		{
			if (infoID == GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[i])
			{
				return true;
			}
		}
		return false;
	}

	public int GetMaxEquipEnhanceLevel(bool isLocal = true)
	{
		return Mathf.Min((!isLocal) ? (Globals.Instance.Player.TeamSystem.GetRemoteLevel() * 2) : ((int)(Globals.Instance.Player.Data.Level * 2u)), GameConst.GetInt32(227));
	}

	public int GetMaxTrinketEnhanceLevel()
	{
		return GameConst.GetInt32(229);
	}

	public int GetMaxEquipRefineLevel()
	{
		return GameConst.GetInt32(228);
	}

	public int GetMaxTrinketRefineLevel()
	{
		return GameConst.GetInt32(230);
	}

	public bool CanEquipRefine()
	{
		return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(11));
	}

	public bool CanTrinketRefine()
	{
		return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(13));
	}

	public List<ItemDataEx> GetPetFragments()
	{
		List<ItemDataEx> list = new List<ItemDataEx>();
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 3 && current.Info.SubType == 0)
			{
				list.Add(current);
			}
		}
		return list;
	}

	public bool HasPetCreateItem()
	{
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 3 && current.Info.SubType == 0 && current.CanCreate())
			{
				return true;
			}
		}
		return false;
	}

	public bool HasEquip2Create()
	{
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 3 && current.Info.SubType == 1 && current.CanCreate())
			{
				return true;
			}
		}
		return false;
	}

	public int GetLowRollItemCount()
	{
		return this.GetItemCount(GameConst.GetInt32(80));
	}

	public int GetHighRollItemCount()
	{
		return this.GetItemCount(GameConst.GetInt32(81));
	}

	public int GetEquipRollItemCount()
	{
		return this.GetItemCount(GameConst.GetInt32(188));
	}

	public void ClearAllEquipSlot()
	{
		foreach (ItemDataEx current in this.Values)
		{
			current.equipSlot = -1;
		}
	}

	private void InitAwakeItems()
	{
		this.awakeItems.Clear();
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 5)
			{
				this.awakeItems.Add(current.Info.ID, current.GetCount());
			}
		}
	}

	public bool EnoughItem2CreateAwakeItem(int infoID, int count = 1, bool recursive = true)
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemDict get info error , ID : {0} ", new object[]
			{
				infoID
			});
			return false;
		}
		AwakeRecipeInfo info2 = Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(info.ID);
		if (info2 == null)
		{
			return false;
		}
		this.InitAwakeItems();
		int num = 0;
		while (num < info2.ItemID.Count && num < info2.Count.Count)
		{
			if (info2.ItemID[num] != 0 && info2.Count[num] > 0 && !this.HasEnoughAwakeItem(info2.ItemID[num], info2.Count[num] * count, recursive))
			{
				return false;
			}
			num++;
		}
		QualityInfo info3 = Globals.Instance.AttDB.QualityDict.GetInfo(info.Quality + 1);
		if (info3 == null)
		{
			global::Debug.LogErrorFormat("QualitDict get info error , ID : {0} ", new object[]
			{
				info.Quality + 1
			});
			return true;
		}
		this.awakeItemCost += info3.AwakeCreateMoney * count;
		return true;
	}

	private bool HasEnoughAwakeItem(int infoID, int count, bool recursive = true)
	{
		if (this.awakeItems.ContainsKey(infoID))
		{
			int num = this.awakeItems[infoID];
			if (num >= count)
			{
				Dictionary<int, int> dictionary;
				Dictionary<int, int> expr_2B = dictionary = this.awakeItems;
				int num2 = dictionary[infoID];
				expr_2B[infoID] = num2 - count;
				return true;
			}
			this.awakeItems[infoID] = 0;
			count -= num;
		}
		if (!recursive)
		{
			return false;
		}
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemDict get info error , ID : {0} ", new object[]
			{
				infoID
			});
			return false;
		}
		AwakeRecipeInfo info2 = Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(info.ID);
		if (info2 == null)
		{
			return false;
		}
		int num3 = 0;
		while (num3 < info2.ItemID.Count && num3 < info2.Count.Count)
		{
			if (info2.ItemID[num3] != 0 && info2.Count[num3] > 0 && !this.HasEnoughAwakeItem(info2.ItemID[num3], info2.Count[num3] * count, true))
			{
				return false;
			}
			num3++;
		}
		QualityInfo info3 = Globals.Instance.AttDB.QualityDict.GetInfo(info.Quality + 1);
		if (info3 == null)
		{
			global::Debug.LogErrorFormat("QualitDict get info error , ID : {0} ", new object[]
			{
				info.Quality + 1
			});
			return true;
		}
		this.awakeItemCost += info3.AwakeCreateMoney * count;
		return true;
	}

	public bool CanCreateAwakeItem(int infoID, int count = 1)
	{
		this.awakeItemCost = 0;
		return this.EnoughItem2CreateAwakeItem(infoID, count, true) && Globals.Instance.Player.Data.Money >= this.awakeItemCost;
	}

	public int GetAwakeItemCreateCount(int childID, int parentID)
	{
		if (childID <= 0 || parentID <= 0)
		{
			return 0;
		}
		AwakeRecipeInfo info = Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(parentID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("AwakeRecipeDice get info error , ID : {0} ", new object[]
			{
				parentID
			});
			return 0;
		}
		for (int i = 0; i < info.ItemID.Count; i++)
		{
			if (info.ItemID[i] == childID)
			{
				return info.Count[i];
			}
		}
		return 0;
	}

	public bool HasLopet2Create()
	{
		foreach (ItemDataEx current in this.Values)
		{
			if (current.Info.Type == 3 && current.Info.SubType == 3 && current.CanCreate())
			{
				return true;
			}
		}
		return false;
	}
}
