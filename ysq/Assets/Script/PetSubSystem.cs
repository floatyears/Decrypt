using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class PetSubSystem : ISubSystem
{
	public delegate void AddPetCallback(PetDataEx data);

	public delegate void RemovePetCallback(ulong id);

	public delegate void UpdatePetCallback(PetDataEx data);

	public delegate void VoidCallback();

	public delegate void AwakeItemCallback(PetDataEx data, int slot);

	public PetSubSystem.AddPetCallback AddPetEvent;

	public PetSubSystem.RemovePetCallback RemovePetEvent;

	public PetSubSystem.UpdatePetCallback LevelupPetEvent;

	public PetSubSystem.UpdatePetCallback FurtherPetEvent;

	public PetSubSystem.UpdatePetCallback SkillPetEvent;

	public PetSubSystem.VoidCallback BreakUpPetEvent;

	public PetSubSystem.AwakeItemCallback AwakeItemEvent;

	public PetSubSystem.UpdatePetCallback AwakeLevelupEvent;

	public PetSubSystem.UpdatePetCallback PetCultivateEvent;

	public PetSubSystem.UpdatePetCallback PetCultivateAckEvent;

	private Dictionary<ulong, PetDataEx> pets = new Dictionary<ulong, PetDataEx>();

	public uint Version
	{
		get;
		private set;
	}

	public ICollection<PetDataEx> Values
	{
		get
		{
			return this.pets.Values;
		}
	}

	public PetSubSystem()
	{
		this.Version = 0u;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(403, new ClientSession.MsgHandler(this.OnMsgPetLevelup));
		Globals.Instance.CliSession.Register(405, new ClientSession.MsgHandler(this.OnMsgPetFurther));
		Globals.Instance.CliSession.Register(407, new ClientSession.MsgHandler(this.OnMsgPetSkill));
		Globals.Instance.CliSession.Register(410, new ClientSession.MsgHandler(this.OnMsgAddPet));
		Globals.Instance.CliSession.Register(411, new ClientSession.MsgHandler(this.OnMsgPetUpdate));
		Globals.Instance.CliSession.Register(412, new ClientSession.MsgHandler(this.OnMsgPetRemove));
		Globals.Instance.CliSession.Register(414, new ClientSession.MsgHandler(this.OnMsgPetBreakUp));
		Globals.Instance.CliSession.Register(418, new ClientSession.MsgHandler(this.OnMsgEquipAwakeItem));
		Globals.Instance.CliSession.Register(420, new ClientSession.MsgHandler(this.OnMsgAwakeLevelup));
		Globals.Instance.CliSession.Register(422, new ClientSession.MsgHandler(this.OnMsgPetCultivate));
		Globals.Instance.CliSession.Register(424, new ClientSession.MsgHandler(this.OnMsgPetCultivateAck));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Version = 0u;
		this.pets.Clear();
	}

	public void LoadData(uint version, List<PetData> data)
	{
		if (version == 0u || version == this.Version)
		{
			return;
		}
		this.Version = version;
		this.pets.Clear();
		for (int i = 0; i < data.Count; i++)
		{
			this.AddPet(data[i]);
		}
		PetData petData = new PetData();
		if (petData != null)
		{
			ObscuredStats data2 = Globals.Instance.Player.Data;
			petData.ID = 100uL;
			petData.InfoID = 90000;
			petData.Level = data2.Level;
			petData.Further = (uint)data2.FurtherLevel;
			petData.Awake = (uint)data2.AwakeLevel;
			petData.ItemFlag = (uint)data2.AwakeItemFlag;
			petData.CultivateCount = data2.CultivateCount;
			petData.Attack = data2.Attack;
			petData.PhysicDefense = data2.PhysicDefense;
			petData.MagicDefense = data2.MagicDefense;
			petData.MaxHP = data2.MaxHP;
			petData.AttackPreview = data2.AttackPreview;
			petData.PhysicDefensePreview = data2.PhysicDefensePreview;
			petData.MagicDefensePreview = data2.MagicDefensePreview;
			petData.MaxHPPreview = data2.MaxHPPreview;
			PlayerPetInfo.Info.Name = data2.Name;
			PlayerPetInfo.Info.Quality = Globals.Instance.Player.GetQuality();
			this.AddPet(petData);
		}
	}

	public PetDataEx GetPet(ulong id)
	{
		if (id == 0uL)
		{
			return null;
		}
		PetDataEx result = null;
		this.pets.TryGetValue(id, out result);
		return result;
	}

	public PetDataEx GetPetByInfoID(int infoID)
	{
		foreach (PetDataEx current in this.Values)
		{
			if (current.Data.InfoID == infoID)
			{
				return current;
			}
		}
		return null;
	}

	public PetDataEx GetNewPetByInfoID(int infoID)
	{
		foreach (PetDataEx current in this.Values)
		{
			if (current.Data.InfoID == infoID && !current.IsOld())
			{
				return current;
			}
		}
		return null;
	}

	private void AddPet(PetData data)
	{
		PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(data.InfoID);
		if (info == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("PetDict.GetInfo error, id = {0}", data.InfoID)
			});
			return;
		}
		PetDataEx petDataEx = new PetDataEx(data, info);
		this.pets.Add(petDataEx.Data.ID, petDataEx);
	}

	private void RemovePet(ulong id)
	{
		if (this.GetPet(id) == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, ID = {0}", id)
			});
			return;
		}
		this.pets.Remove(id);
	}

	public void OnMsgPetLevelup(MemoryStream stream)
	{
		MS2C_PetLevelup mS2C_PetLevelup = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetLevelup), stream) as MS2C_PetLevelup;
		if (mS2C_PetLevelup.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetLevelup.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_PetLevelup.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_PetLevelup.PetID)
			});
			return;
		}
		uint level = pet.Data.Level;
		pet.Data.Exp = mS2C_PetLevelup.Exp;
		pet.Data.Level = mS2C_PetLevelup.Level;
		int socketSlot = pet.GetSocketSlot();
		if (socketSlot >= 0)
		{
			Globals.Instance.Player.TeamSystem.OnPetUpdate(socketSlot, (int)(pet.Data.Level - level), 0);
		}
		for (int i = 0; i < mS2C_PetLevelup.Pets.Count; i++)
		{
			this.RemovePet(mS2C_PetLevelup.Pets[i]);
			if (this.RemovePetEvent != null)
			{
				this.RemovePetEvent(mS2C_PetLevelup.Pets[i]);
			}
		}
		if (mS2C_PetLevelup.PetVersion != 0u)
		{
			this.Version = mS2C_PetLevelup.PetVersion;
		}
		if (this.LevelupPetEvent != null)
		{
			this.LevelupPetEvent(pet);
		}
	}

	public void OnMsgPetFurther(MemoryStream stream)
	{
		MS2C_PetFurther mS2C_PetFurther = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetFurther), stream) as MS2C_PetFurther;
		if (mS2C_PetFurther.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetFurther.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_PetFurther.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_PetFurther.PetID)
			});
			return;
		}
		for (int i = 0; i < mS2C_PetFurther.Pets.Count; i++)
		{
			this.RemovePet(mS2C_PetFurther.Pets[i]);
			if (this.RemovePetEvent != null)
			{
				this.RemovePetEvent(mS2C_PetFurther.Pets[i]);
			}
		}
		if (pet.Data.ID != 100uL)
		{
			uint further = pet.Data.Further;
			pet.Data.Further = mS2C_PetFurther.Further;
			int socketSlot = pet.GetSocketSlot();
			if (socketSlot >= 0)
			{
				Globals.Instance.Player.TeamSystem.OnPetUpdate(socketSlot, 0, (int)(pet.Data.Further - further));
			}
			if (mS2C_PetFurther.PetVersion != 0u)
			{
				this.Version = mS2C_PetFurther.PetVersion;
			}
		}
		if (this.FurtherPetEvent != null)
		{
			this.FurtherPetEvent(pet);
		}
	}

	public void OnMsgPetSkill(MemoryStream stream)
	{
		MS2C_PetSkill mS2C_PetSkill = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetSkill), stream) as MS2C_PetSkill;
		if (mS2C_PetSkill.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetSkill.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_PetSkill.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_PetSkill.PetID)
			});
			return;
		}
		uint skillLevel = pet.Data.SkillLevel;
		pet.Data.SkillLevel = mS2C_PetSkill.SkillLevel;
		int socketSlot = pet.GetSocketSlot();
		if (socketSlot >= 0)
		{
			Globals.Instance.Player.TeamSystem.OnPetSkillUpdate(socketSlot, skillLevel);
		}
		if (mS2C_PetSkill.PetVersion != 0u)
		{
			this.Version = mS2C_PetSkill.PetVersion;
		}
		if (this.SkillPetEvent != null)
		{
			this.SkillPetEvent(pet);
		}
	}

	public void OnMsgAddPet(MemoryStream stream)
	{
		MS2C_AddPet mS2C_AddPet = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddPet), stream) as MS2C_AddPet;
		this.AddPet(mS2C_AddPet.Data);
		PetDataEx pet = this.GetPet(mS2C_AddPet.Data.ID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_AddPet.Data.ID)
			});
			return;
		}
		if (mS2C_AddPet.PetVersion != 0u)
		{
			this.Version = mS2C_AddPet.PetVersion;
		}
		GameCache.Data.HasShowChangePetMark = true;
		GameCache.UpdateNow = true;
		if (this.AddPetEvent != null)
		{
			this.AddPetEvent(pet);
		}
	}

	public void OnMsgPetUpdate(MemoryStream stream)
	{
		MS2C_PetUpdate mS2C_PetUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetUpdate), stream) as MS2C_PetUpdate;
		for (int i = 0; i < mS2C_PetUpdate.Data.Count; i++)
		{
			PetDataEx pet = this.GetPet(mS2C_PetUpdate.Data[i].ID);
			if (pet == null)
			{
				Debug.LogError(new object[]
				{
					string.Format("GetPet error, petID = {0}", mS2C_PetUpdate.Data[i].ID)
				});
			}
			else
			{
				uint level = pet.Data.Level;
				uint further = pet.Data.Further;
				uint skillLevel = pet.Data.SkillLevel;
				uint awake = pet.Data.Awake;
				uint itemFlag = pet.Data.ItemFlag;
				pet.Data.Exp = mS2C_PetUpdate.Data[i].Exp;
				pet.Data.Level = mS2C_PetUpdate.Data[i].Level;
				pet.Data.Further = mS2C_PetUpdate.Data[i].Further;
				pet.Data.SkillLevel = mS2C_PetUpdate.Data[i].SkillLevel;
				pet.Data.Awake = mS2C_PetUpdate.Data[i].Awake;
				pet.Data.ItemFlag = mS2C_PetUpdate.Data[i].ItemFlag;
				int num = mS2C_PetUpdate.Data[i].Attack - pet.Data.Attack;
				int num2 = mS2C_PetUpdate.Data[i].PhysicDefense - pet.Data.PhysicDefense;
				int num3 = mS2C_PetUpdate.Data[i].MagicDefense - pet.Data.MagicDefense;
				int num4 = mS2C_PetUpdate.Data[i].MaxHP - pet.Data.MaxHP;
				pet.Data.CultivateCount = mS2C_PetUpdate.Data[i].CultivateCount;
				pet.Data.Attack = mS2C_PetUpdate.Data[i].Attack;
				pet.Data.PhysicDefense = mS2C_PetUpdate.Data[i].PhysicDefense;
				pet.Data.MagicDefense = mS2C_PetUpdate.Data[i].MagicDefense;
				pet.Data.MaxHP = mS2C_PetUpdate.Data[i].MaxHP;
				pet.Data.AttackPreview = mS2C_PetUpdate.Data[i].AttackPreview;
				pet.Data.PhysicDefensePreview = mS2C_PetUpdate.Data[i].PhysicDefensePreview;
				pet.Data.MagicDefensePreview = mS2C_PetUpdate.Data[i].MagicDefensePreview;
				pet.Data.MaxHPPreview = mS2C_PetUpdate.Data[i].MaxHPPreview;
				int socketSlot = pet.GetSocketSlot();
				if (socketSlot >= 0)
				{
					if (level != pet.Data.Level || further != pet.Data.Further)
					{
						Globals.Instance.Player.TeamSystem.OnPetUpdate(socketSlot, (int)(pet.Data.Level - level), (int)(pet.Data.Further - further));
					}
					if (skillLevel != pet.Data.SkillLevel)
					{
						Globals.Instance.Player.TeamSystem.OnPetSkillUpdate(socketSlot, skillLevel);
					}
					if (awake != pet.Data.Awake || itemFlag != pet.Data.ItemFlag)
					{
						Globals.Instance.Player.TeamSystem.OnAwakeUpdate(socketSlot);
					}
					if (num != 0 || num2 != 0 || num3 != 0 || num4 != 0)
					{
						Globals.Instance.Player.TeamSystem.OnPetCultivate(socketSlot, num, num2, num3, num4);
					}
				}
			}
		}
		if (mS2C_PetUpdate.PetVersion != 0u)
		{
			this.Version = mS2C_PetUpdate.PetVersion;
		}
	}

	public void OnMsgPetRemove(MemoryStream stream)
	{
		MS2C_PetRemove mS2C_PetRemove = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetRemove), stream) as MS2C_PetRemove;
		this.RemovePet(mS2C_PetRemove.PetID);
		if (this.RemovePetEvent != null)
		{
			this.RemovePetEvent(mS2C_PetRemove.PetID);
		}
		if (mS2C_PetRemove.PetVersion != 0u)
		{
			this.Version = mS2C_PetRemove.PetVersion;
		}
	}

	public void OnMsgPetBreakUp(MemoryStream stream)
	{
		MS2C_PetBreakUp mS2C_PetBreakUp = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetBreakUp), stream) as MS2C_PetBreakUp;
		if (mS2C_PetBreakUp.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetBreakUp.Result);
			return;
		}
		for (int i = 0; i < mS2C_PetBreakUp.Pets.Count; i++)
		{
			this.RemovePet(mS2C_PetBreakUp.Pets[i]);
			if (this.RemovePetEvent != null)
			{
				this.RemovePetEvent(mS2C_PetBreakUp.Pets[i]);
			}
		}
		if (mS2C_PetBreakUp.PetVersion != 0u)
		{
			this.Version = mS2C_PetBreakUp.PetVersion;
		}
		if (this.BreakUpPetEvent != null)
		{
			this.BreakUpPetEvent();
		}
	}

	public void OnMsgEquipAwakeItem(MemoryStream stream)
	{
		MS2C_EquipAwakeItem mS2C_EquipAwakeItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipAwakeItem), stream) as MS2C_EquipAwakeItem;
		if (mS2C_EquipAwakeItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_EquipAwakeItem.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_EquipAwakeItem.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_EquipAwakeItem.PetID)
			});
			return;
		}
		if (pet.Data.ID != 100uL)
		{
			uint itemFlag = pet.Data.ItemFlag;
			pet.Data.ItemFlag = mS2C_EquipAwakeItem.Flag;
			int socketSlot = pet.GetSocketSlot();
			if (socketSlot >= 0)
			{
				Globals.Instance.Player.TeamSystem.OnEquipAwakeItem(socketSlot, itemFlag);
			}
			if (mS2C_EquipAwakeItem.PetVersion != 0u)
			{
				this.Version = mS2C_EquipAwakeItem.PetVersion;
			}
		}
		if (this.AwakeItemEvent != null)
		{
			this.AwakeItemEvent(pet, mS2C_EquipAwakeItem.Index);
		}
	}

	public void OnMsgAwakeLevelup(MemoryStream stream)
	{
		MS2C_AwakeLevelup mS2C_AwakeLevelup = Serializer.NonGeneric.Deserialize(typeof(MS2C_AwakeLevelup), stream) as MS2C_AwakeLevelup;
		if (mS2C_AwakeLevelup.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_AwakeLevelup.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_AwakeLevelup.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_AwakeLevelup.PetID)
			});
			return;
		}
		if (pet.Data.ID != 100uL)
		{
			pet.Data.Awake = mS2C_AwakeLevelup.Awake;
			pet.Data.ItemFlag = mS2C_AwakeLevelup.Flag;
			int socketSlot = pet.GetSocketSlot();
			if (socketSlot >= 0)
			{
				Globals.Instance.Player.TeamSystem.OnAwakeLevelup(socketSlot);
			}
			for (int i = 0; i < mS2C_AwakeLevelup.Pets.Count; i++)
			{
				this.RemovePet(mS2C_AwakeLevelup.Pets[i]);
				if (this.RemovePetEvent != null)
				{
					this.RemovePetEvent(mS2C_AwakeLevelup.Pets[i]);
				}
			}
			if (mS2C_AwakeLevelup.PetVersion != 0u)
			{
				this.Version = mS2C_AwakeLevelup.PetVersion;
			}
		}
		if (this.AwakeLevelupEvent != null)
		{
			this.AwakeLevelupEvent(pet);
		}
	}

	public void OnMsgPetCultivate(MemoryStream stream)
	{
		MS2C_PetCultivate mS2C_PetCultivate = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetCultivate), stream) as MS2C_PetCultivate;
		if (mS2C_PetCultivate.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetCultivate.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_PetCultivate.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_PetCultivate.PetID)
			});
			return;
		}
		if (pet.Data.ID != 100uL)
		{
			pet.Data.AttackPreview = mS2C_PetCultivate.AttackPreview;
			pet.Data.PhysicDefensePreview = mS2C_PetCultivate.PhysicDefensePreview;
			pet.Data.MagicDefensePreview = mS2C_PetCultivate.MagicDefensePreview;
			pet.Data.MaxHPPreview = mS2C_PetCultivate.MaxHPPreview;
			pet.Data.CultivateCount = mS2C_PetCultivate.CultivateCount;
			if (mS2C_PetCultivate.PetVersion != 0u)
			{
				this.Version = mS2C_PetCultivate.PetVersion;
			}
		}
		if (this.PetCultivateEvent != null)
		{
			this.PetCultivateEvent(pet);
		}
	}

	public void OnMsgPetCultivateAck(MemoryStream stream)
	{
		MS2C_PetCultivateAck mS2C_PetCultivateAck = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetCultivateAck), stream) as MS2C_PetCultivateAck;
		if (mS2C_PetCultivateAck.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetCultivateAck.Result);
			return;
		}
		PetDataEx pet = this.GetPet(mS2C_PetCultivateAck.PetID);
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetPet error, petID = {0}", mS2C_PetCultivateAck.PetID)
			});
			return;
		}
		if (pet.Data.ID != 100uL)
		{
			int attackMod = mS2C_PetCultivateAck.Attack - pet.Data.Attack;
			int physicDefenseMod = mS2C_PetCultivateAck.PhysicDefense - pet.Data.PhysicDefense;
			int magicDefenseMod = mS2C_PetCultivateAck.MagicDefense - pet.Data.MagicDefense;
			int maxHPMod = mS2C_PetCultivateAck.MaxHP - pet.Data.MaxHP;
			pet.Data.AttackPreview = 0;
			pet.Data.PhysicDefensePreview = 0;
			pet.Data.MagicDefensePreview = 0;
			pet.Data.MaxHPPreview = 0;
			pet.Data.Attack = mS2C_PetCultivateAck.Attack;
			pet.Data.PhysicDefense = mS2C_PetCultivateAck.PhysicDefense;
			pet.Data.MagicDefense = mS2C_PetCultivateAck.MagicDefense;
			pet.Data.MaxHP = mS2C_PetCultivateAck.MaxHP;
			int socketSlot = pet.GetSocketSlot();
			if (socketSlot >= 0)
			{
				Globals.Instance.Player.TeamSystem.OnPetCultivate(socketSlot, attackMod, physicDefenseMod, magicDefenseMod, maxHPMod);
			}
			if (mS2C_PetCultivateAck.PetVersion != 0u)
			{
				this.Version = mS2C_PetCultivateAck.PetVersion;
			}
		}
		if (this.PetCultivateAckEvent != null)
		{
			this.PetCultivateAckEvent(pet);
		}
	}

	public int GetUnBattlePetNum()
	{
		int num = 0;
		foreach (PetDataEx current in this.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting())
			{
				num++;
			}
		}
		return num;
	}

	public int GetPetCount(int petInfoID)
	{
		int num = 0;
		foreach (PetDataEx current in this.Values)
		{
			if (current.Data.InfoID == petInfoID)
			{
				num++;
			}
		}
		return num;
	}

	public int GetFurtherPetCount(ulong petId, int petInfoID)
	{
		int num = 0;
		foreach (PetDataEx current in this.Values)
		{
			if (current.Data.InfoID == petInfoID && !current.IsBattling() && !current.IsPetAssisting() && current.Data.ID != petId && current.Data.Further == 0u && current.Data.Level == 1u && current.Data.Exp == 0u && current.Data.SkillLevel == 0u && current.Data.Awake == 0u && current.Data.ItemFlag == 0u && current.Data.Attack == 0 && current.Data.PhysicDefense == 0 && current.Data.MagicDefense == 0 && current.Data.MaxHP == 0)
			{
				num++;
			}
		}
		return num;
	}

	public void GetAllPet2BreakUp(out List<PetDataEx> list, out bool hasOrangePet)
	{
		hasOrangePet = false;
		list = new List<PetDataEx>();
		foreach (PetDataEx current in this.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting())
			{
				if (current.Info.Quality >= 2)
				{
					hasOrangePet = true;
				}
				else
				{
					list.Add(current);
				}
			}
		}
	}

	public bool HasPet2Reborn()
	{
		foreach (PetDataEx current in this.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting())
			{
				if (current.IsOld())
				{
					bool result = true;
					return result;
				}
				int num = 0;
				while (num < 4 && num < current.Info.SkillID.Count)
				{
					if (current.Data.SkillLevel != 0u)
					{
						bool result = true;
						return result;
					}
					num++;
				}
			}
		}
		return false;
	}

	public bool IsAwakeNeedItem(int itemInfoId)
	{
		foreach (PetDataEx current in this.Values)
		{
			if (current != null && current.IsBattling() && current.IsAwakeNeedItem(itemInfoId))
			{
				return true;
			}
		}
		return false;
	}

	public void ClearAllSocketSlot()
	{
		foreach (PetDataEx current in this.Values)
		{
			current.SetSocketSlot(-1);
		}
	}
}
