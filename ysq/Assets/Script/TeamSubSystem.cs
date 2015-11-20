using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class TeamSubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public delegate void PetUpdateCallback(int slot);

	public delegate void ItemUpdateCallback(int slot, int equipSlot);

	public delegate void UpdateFashionCallback(int infoId);

	public TeamSubSystem.VoidCallback ChangeSocketEvent;

	public TeamSubSystem.PetUpdateCallback EquipPetEvent;

	public TeamSubSystem.ItemUpdateCallback EquipItemEvent;

	public TeamSubSystem.UpdateFashionCallback EquipFashionEvent;

	private int combatValue;

	private SocketDataEx[] sockets = new SocketDataEx[4];

	private PetDataEx[] assistPets = new PetDataEx[6];

	private ulong remoteID;

	private string remoteName = string.Empty;

	private int remoteLevel;

	private int remoteFurther;

	private int remoteVipLevel;

	private int remoteGender;

	private int remoteCombatValue;

	private int remoteConstellationLevel;

	private int remoteFashionLevel;

	private int remoteAwake;

	private int remoteAwakeItemFlag;

	private SocketDataEx[] remoteSockets = new SocketDataEx[4];

	private PetDataEx[] remoteAssistPets = new PetDataEx[6];

	private LopetDataEx remoteLopet;

	public uint Version
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(194, new ClientSession.MsgHandler(this.OnMsgChangeSocket));
		Globals.Instance.CliSession.Register(196, new ClientSession.MsgHandler(this.OnMsgSetCombatPet));
		Globals.Instance.CliSession.Register(198, new ClientSession.MsgHandler(this.OnMsgEquipItem));
		Globals.Instance.CliSession.Register(200, new ClientSession.MsgHandler(this.OnMsgAutoEquipItem));
		Globals.Instance.CliSession.Register(202, new ClientSession.MsgHandler(this.OnMsgChangeFashion));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		for (int i = 0; i < this.sockets.Length; i++)
		{
			this.sockets[i] = null;
			this.remoteSockets[i] = null;
		}
		for (int j = 0; j < this.assistPets.Length; j++)
		{
			this.assistPets[j] = null;
			this.remoteAssistPets[j] = null;
		}
		this.remoteLopet = null;
		this.combatValue = 0;
		this.Version = 0u;
	}

	public void LoadData(uint version, List<SocketData> data, List<ulong> assistPetIDs)
	{
		if (version != 0u)
		{
			if (data.Count != 4 || assistPetIDs.Count != 6)
			{
				Debug.LogError(new object[]
				{
					"SocketData size or AssistPetIDs size error!"
				});
				return;
			}
			this.Version = version;
			Globals.Instance.Player.ItemSystem.ClearAllEquipSlot();
			Globals.Instance.Player.PetSystem.ClearAllSocketSlot();
			for (int i = 0; i < assistPetIDs.Count; i++)
			{
				if (assistPetIDs[i] == 0uL)
				{
					this.assistPets[i] = null;
				}
				else
				{
					this.assistPets[i] = Globals.Instance.Player.PetSystem.GetPet(assistPetIDs[i]);
					if (this.assistPets[i] != null)
					{
						this.assistPets[i].SetSocketSlot(4 + i);
					}
				}
			}
			for (int j = 0; j < 4; j++)
			{
				this.sockets[j] = null;
			}
			for (int k = 0; k < 4; k++)
			{
				this.sockets[k] = new SocketDataEx();
				if (this.sockets[k] == null)
				{
					Debug.LogError(new object[]
					{
						"allocate SocketDataEx error!"
					});
					return;
				}
				int fashionID;
				if (k == 0)
				{
					fashionID = (int)data[k].PetID;
					data[k].PetID = 100uL;
				}
				else
				{
					fashionID = 0;
				}
				this.sockets[k].Init(data[k], k, fashionID);
			}
			this.UpdateCombatValue();
		}
	}

	public int GetSocketSlot(ulong petID)
	{
		for (int i = 0; i < this.sockets.Length; i++)
		{
			if (this.sockets[i] != null)
			{
				PetDataEx pet = this.sockets[i].GetPet();
				if (pet != null && pet.Data.ID == petID)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public int GetAssistSlot(ulong petID)
	{
		for (int i = 0; i < this.assistPets.Length; i++)
		{
			if (this.assistPets[i] != null)
			{
				if (this.assistPets[i] != null && this.assistPets[i].Data.ID == petID)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public bool IsCombatPet(ulong id)
	{
		return -1 != this.GetSocketSlot(id);
	}

	public bool IsAssistPet(ulong id)
	{
		return -1 != this.GetAssistSlot(id);
	}

	public bool HasPetInfoID(int infoID)
	{
		for (int i = 0; i < this.sockets.Length; i++)
		{
			if (this.sockets[i] != null)
			{
				PetDataEx pet = this.sockets[i].GetPet();
				if (pet != null && pet.Info.ID == infoID)
				{
					return true;
				}
			}
		}
		for (int j = 0; j < this.assistPets.Length; j++)
		{
			if (this.assistPets[j] != null)
			{
				if (this.assistPets[j] != null && this.assistPets[j].Info.ID == infoID)
				{
					return true;
				}
			}
		}
		return false;
	}

	public SocketDataEx GetSocket(int slot)
	{
		if (slot < 0 || slot >= 4)
		{
			return null;
		}
		return this.sockets[slot];
	}

	private void SetSocket(int slot, SocketDataEx data)
	{
		if (slot < 0 || slot >= 4)
		{
			return;
		}
		if (this.sockets[slot] != null)
		{
			this.sockets[slot].SetSocketSlot(-1);
		}
		this.sockets[slot] = data;
		if (this.sockets[slot] != null)
		{
			this.sockets[slot].SetSocketSlot(slot);
		}
	}

	public PetDataEx GetPet(int slot)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return null;
		}
		return socket.GetPet();
	}

	public PetDataEx GetAssist(int slot)
	{
		if (slot < 0 || slot >= this.assistPets.Length)
		{
			return null;
		}
		return this.assistPets[slot];
	}

	public ItemDataEx GetEquip(int slot, int equipSlot)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return null;
		}
		return socket.GetEquip(equipSlot);
	}

	public void EquipAssist(int slot, PetDataEx data)
	{
		if (slot < 0 || slot >= 6)
		{
			return;
		}
		if (this.assistPets[slot] != null)
		{
			this.assistPets[slot].SetSocketSlot(-1);
		}
		this.assistPets[slot] = data;
		if (this.assistPets[slot] != null)
		{
			this.assistPets[slot].SetSocketSlot(4 + slot);
		}
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i);
			if (socket != null)
			{
				socket.HandleRelationEffect();
			}
		}
		this.HandleAssistLevelEffect();
		this.HandleAssistFurtherEffect();
	}

	public void GetAttribute(int slot, ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return;
		}
		maxHP = socket.GetAtt(1);
		attack = socket.GetAtt(2);
		physicDefense = socket.GetAtt(3);
		magicDefense = socket.GetAtt(4);
	}

	public void UpdateCombatValue()
	{
		this.combatValue = 0;
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i);
			if (socket != null)
			{
				this.combatValue += socket.GetCombatValue();
			}
		}
	}

	public int GetCombatValue()
	{
		return this.combatValue;
	}

	public int GetMinAssistLevel()
	{
		int num = 150;
		for (int i = 0; i < 6; i++)
		{
			if (this.assistPets[i] == null)
			{
				return 0;
			}
			if ((long)num > (long)((ulong)this.assistPets[i].Data.Level))
			{
				num = (int)this.assistPets[i].Data.Level;
			}
		}
		return num;
	}

	public int GetMinAssistFurther()
	{
		int num = 15;
		for (int i = 0; i < 6; i++)
		{
			if (this.assistPets[i] == null)
			{
				return 0;
			}
			if ((long)num > (long)((ulong)this.assistPets[i].Data.Further))
			{
				num = (int)this.assistPets[i].Data.Further;
			}
		}
		return num;
	}

	private void HandleAssistLevelEffect()
	{
		if (Globals.Instance.Player.Data.Level < (uint)GameConst.GetInt32(29))
		{
			return;
		}
		int minAssistLevel = this.GetMinAssistLevel();
		int assistLevelID = TinyLevel.GetAssistLevelID(minAssistLevel);
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i);
			if (socket != null)
			{
				socket.HandleAssistLevelEffect(assistLevelID);
			}
		}
	}

	private void HandleAssistFurtherEffect()
	{
		if (Globals.Instance.Player.Data.Level < (uint)GameConst.GetInt32(30))
		{
			return;
		}
		int minAssistFurther = this.GetMinAssistFurther();
		int assistFurtherID = TinyLevel.GetAssistFurtherID(minAssistFurther);
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i);
			if (socket != null)
			{
				socket.HandleAssistFurtherEffect(assistFurtherID);
			}
		}
	}

	private bool IsRelationActive(List<int> petIDs, List<int> assistIDs, RelationInfo info)
	{
		if (petIDs == null || info == null)
		{
			return false;
		}
		if (info.Type != 0)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < info.PetID.Count; i++)
		{
			if (info.PetID[i] != 0)
			{
				bool flag = false;
				for (int j = 0; j < petIDs.Count; j++)
				{
					if (info.PetID[i] == petIDs[j])
					{
						flag = true;
						result = true;
						break;
					}
				}
				if (!flag)
				{
					for (int k = 0; k < assistIDs.Count; k++)
					{
						if (info.PetID[i] == assistIDs[k])
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return result;
	}

	private void UpdatePetToActiveRelation(PetDataEx petData, List<int> petIDs, List<int> assistIDs, List<RelationInfo> inactiveRelations)
	{
		if (petData == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < inactiveRelations.Count; i++)
		{
			if (this.IsRelationActive(petIDs, assistIDs, inactiveRelations[i]))
			{
				num++;
			}
		}
		if (petIDs[0] != 0)
		{
			int num2 = 0;
			while (num2 < 3 && num2 < petData.Info.RelationID.Count)
			{
				if (petData.Info.RelationID[num2] != 0)
				{
					RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(petData.Info.RelationID[num2]);
					if (info == null)
					{
						Debug.LogErrorFormat("RelationDict.GetInfo error, id = {0}", new object[]
						{
							petData.Info.RelationID[num2]
						});
					}
					else if (this.IsRelationActive(petIDs, assistIDs, info))
					{
						num++;
					}
				}
				num2++;
			}
		}
		petData.SetRelationActive(num);
	}

	public void UpdateToActiveRelation(int slot)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<RelationInfo> list3 = new List<RelationInfo>();
		list.Add(0);
		list2.Add(0);
		for (int i = 1; i < 4; i++)
		{
			if (slot != i)
			{
				SocketDataEx socket = this.GetSocket(i);
				if (socket != null)
				{
					PetDataEx pet = socket.GetPet();
					if (pet != null)
					{
						list.Add(pet.Data.InfoID);
						int num = 0;
						while (num < 3 && num < pet.Info.RelationID.Count)
						{
							if (pet.Info.RelationID[num] != 0 && !socket.IsRelationActive(num))
							{
								RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[num]);
								if (info == null)
								{
									Debug.LogErrorFormat("RelationDict.GetInfo error, id = {0}", new object[]
									{
										pet.Info.RelationID[num]
									});
								}
								else
								{
									list3.Add(info);
								}
							}
							num++;
						}
					}
				}
			}
		}
		int num2 = slot - 4;
		for (int j = 0; j < this.assistPets.Length; j++)
		{
			if (j != num2)
			{
				if (this.assistPets[j] != null)
				{
					list2.Add(this.assistPets[j].Data.InfoID);
				}
			}
		}
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (current.GetSocketSlot() < 0 || current.GetSocketSlot() == slot)
			{
				if (slot < 4)
				{
					list[0] = current.Data.InfoID;
					list2[0] = 0;
				}
				else
				{
					list[0] = 0;
					list2[0] = current.Data.InfoID;
				}
				this.UpdatePetToActiveRelation(current, list, list2, list3);
			}
		}
	}

	public void OnPlayerLevelup(uint level)
	{
		PetDataEx pet = this.GetPet(0);
		if (pet == null || pet.Data.ID != 100uL || pet.Data.InfoID != 90000)
		{
			Debug.LogError(new object[]
			{
				"Player pet data error"
			});
			return;
		}
		uint level2 = pet.Data.Level;
		pet.Data.Level = level;
		if ((ulong)level2 < (ulong)((long)GameConst.GetInt32(29)) && (ulong)level >= (ulong)((long)GameConst.GetInt32(29)))
		{
			this.HandleAssistLevelEffect();
		}
		if ((ulong)level2 < (ulong)((long)GameConst.GetInt32(30)) && (ulong)level >= (ulong)((long)GameConst.GetInt32(30)))
		{
			this.HandleAssistFurtherEffect();
		}
		this.OnPetUpdate(0, (int)(pet.Data.Level - level2), 0);
	}

	public void OnPlayerFurther(int level)
	{
		PetDataEx pet = this.GetPet(0);
		if (pet == null || pet.Data.ID != 100uL || pet.Data.InfoID != 90000)
		{
			Debug.LogError(new object[]
			{
				"Player pet data error"
			});
			return;
		}
		uint further = pet.Data.Further;
		pet.Data.Further = (uint)level;
		this.OnPetUpdate(0, 0, (int)(pet.Data.Further - further));
	}

	public void OnPlayerAwakeLevelup(int level)
	{
		PetDataEx pet = this.GetPet(0);
		if (pet == null || pet.Data.ID != 100uL || pet.Data.InfoID != 90000)
		{
			Debug.LogError(new object[]
			{
				"Player pet data error"
			});
			return;
		}
		pet.Data.Awake = (uint)level;
		this.OnAwakeLevelup(0);
	}

	public void OnPlayerAwakeEquipItem(int flag)
	{
		PetDataEx pet = this.GetPet(0);
		if (pet == null || pet.Data.ID != 100uL || pet.Data.InfoID != 90000)
		{
			Debug.LogError(new object[]
			{
				"Player pet data error"
			});
			return;
		}
		uint itemFlag = pet.Data.ItemFlag;
		pet.Data.ItemFlag = (uint)flag;
		this.OnEquipAwakeItem(0, itemFlag);
	}

	public void OnPlayerCultivate(int attack, int physicDefense, int magicDefense, int maxHP)
	{
		PetDataEx pet = this.GetPet(0);
		if (pet == null || pet.Data.ID != 100uL || pet.Data.InfoID != 90000)
		{
			Debug.LogError(new object[]
			{
				"Player pet data error"
			});
			return;
		}
		int attackMod = attack - pet.Data.Attack;
		pet.Data.Attack = attack;
		int physicDefenseMod = physicDefense - pet.Data.PhysicDefense;
		pet.Data.PhysicDefense = physicDefense;
		int magicDefenseMod = magicDefense - pet.Data.MagicDefense;
		pet.Data.MagicDefense = magicDefense;
		int maxHPMod = maxHP - pet.Data.MaxHP;
		pet.Data.MaxHP = maxHP;
		this.OnPetCultivate(0, attackMod, physicDefenseMod, magicDefenseMod, maxHPMod);
	}

	public void OnConLevelup(int conLevel)
	{
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i);
			if (socket != null)
			{
				if (socket.IsPlayer())
				{
					socket.UpdateData();
				}
				else
				{
					socket.HandleConstellationEffect();
				}
			}
		}
		this.UpdateCombatValue();
	}

	public void OnFashionUpdate()
	{
		SocketDataEx socket = this.GetSocket(0);
		if (socket == null)
		{
			return;
		}
		socket.HandleFashionEffect();
		this.UpdateCombatValue();
	}

	public void OnPetUpdate(int slot, int addLevel, int addFurther)
	{
		if (slot >= 4)
		{
			if (addLevel > 0)
			{
				this.HandleAssistLevelEffect();
			}
			if (addFurther > 0)
			{
				this.HandleAssistFurtherEffect();
			}
			this.UpdateCombatValue();
			return;
		}
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null || socket.GetPet() == null)
		{
			return;
		}
		socket.OnPetUpdate(addLevel, addFurther);
		this.UpdateCombatValue();
	}

	public void OnPetSkillUpdate(int slot, uint oldSkillLevel)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null || socket.GetPet() == null)
		{
			Debug.LogError(new object[]
			{
				"GetSocket error"
			});
			return;
		}
		this.UpdateCombatValue();
	}

	public void OnItemEnhance(int slot, int equipSlot, int addEnhanceLevel, int addRefineLevel)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return;
		}
		ItemDataEx equip = socket.GetEquip(equipSlot);
		if (equip == null)
		{
			return;
		}
		socket.HandleItemEffect(equip, addEnhanceLevel, addRefineLevel);
		socket.HandleMasterEffect();
		this.UpdateCombatValue();
	}

	public void OnAwakeUpdate(int slot)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return;
		}
		socket.RecalculateSelf();
		this.UpdateCombatValue();
	}

	public void OnAwakeLevelup(int slot)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return;
		}
		socket.OnAwakeUpdate();
		this.UpdateCombatValue();
	}

	public void OnEquipAwakeItem(int slot, uint oldFlag)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return;
		}
		PetDataEx pet = socket.GetPet();
		if (pet == null)
		{
			Debug.LogError(new object[]
			{
				"socketData.GetPet error"
			});
			return;
		}
		AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo((int)(pet.Data.Awake + 1u));
		if (info == null)
		{
			Debug.LogErrorFormat("AwakeDict.GetInfo error, level = {0}", new object[]
			{
				(int)(pet.Data.Awake + 1u)
			});
			return;
		}
		uint itemFlag = pet.Data.ItemFlag;
		for (int i = 0; i < 4; i++)
		{
			int index = pet.Info.ElementType * 4 + i;
			if (info.ItemID[index] != 0 && ((ulong)itemFlag & (ulong)(1L << (i & 31))) != 0uL && ((ulong)oldFlag & (ulong)(1L << (i & 31))) == 0uL)
			{
				socket.OnEquipAwakeItem(info.ItemID[index]);
			}
		}
		this.UpdateCombatValue();
	}

	public void OnPetCultivate(int slot, int attackMod, int physicDefenseMod, int magicDefenseMod, int maxHPMod)
	{
		SocketDataEx socket = this.GetSocket(slot);
		if (socket == null)
		{
			return;
		}
		socket.HandleCultivateEffect(attackMod, physicDefenseMod, magicDefenseMod, maxHPMod);
		this.UpdateCombatValue();
	}

	public void OnLopetUpdate()
	{
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i);
			if (socket != null)
			{
				socket.HandleLopetEffect();
			}
		}
		this.UpdateCombatValue();
	}

	public SocketDataEx GetRemoteSocket(int slot)
	{
		if (slot < 0 || slot >= 4)
		{
			return null;
		}
		return this.remoteSockets[slot];
	}

	public int GetRemoteSocketSlot(ulong petID)
	{
		for (int i = 0; i < this.remoteSockets.Length; i++)
		{
			if (this.remoteSockets[i] != null)
			{
				PetDataEx pet = this.remoteSockets[i].GetPet();
				if (pet != null && pet.Data.ID == petID)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public ulong GetRemoteID()
	{
		return this.remoteID;
	}

	public PetDataEx GetRemotePet(int slot)
	{
		SocketDataEx remoteSocket = this.GetRemoteSocket(slot);
		if (remoteSocket == null)
		{
			return null;
		}
		return remoteSocket.GetPet();
	}

	public PetDataEx GetRemoteAssist(int slot)
	{
		if (slot < 0 || slot >= this.assistPets.Length)
		{
			return null;
		}
		return this.remoteAssistPets[slot];
	}

	public ItemDataEx GetRemoteEquip(int slot, int equipSlot)
	{
		SocketDataEx remoteSocket = this.GetRemoteSocket(slot);
		if (remoteSocket == null)
		{
			return null;
		}
		return remoteSocket.GetEquip(equipSlot);
	}

	public void GetRemoteAttribute(int slot, ref int maxHP, ref int attack, ref int physicDefense, ref int magicDefense)
	{
		SocketDataEx remoteSocket = this.GetRemoteSocket(slot);
		if (remoteSocket == null)
		{
			return;
		}
		maxHP = remoteSocket.GetAtt(1);
		attack = remoteSocket.GetAtt(2);
		physicDefense = remoteSocket.GetAtt(3);
		magicDefense = remoteSocket.GetAtt(4);
	}

	public string GetRemoteName()
	{
		return this.remoteName;
	}

	public int GetRemoteLevel()
	{
		return this.remoteLevel;
	}

	public int GetRemoteVipLevel()
	{
		return this.remoteVipLevel;
	}

	public int GetRemoteQuality()
	{
		return LocalPlayer.GetQuality(this.remoteConstellationLevel);
	}

	public int GetRemoteGender()
	{
		return this.remoteGender;
	}

	public int GeRemoteCombatValue()
	{
		return this.remoteCombatValue;
	}

	public int GeRemoteConstellationLevel()
	{
		return this.remoteConstellationLevel;
	}

	public int GeRemoteFashionLevel()
	{
		return this.remoteFashionLevel;
	}

	public void UpdateRemoteCombatValue()
	{
		this.remoteCombatValue = 0;
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = this.GetSocket(i, false);
			if (socket != null)
			{
				this.remoteCombatValue += socket.GetCombatValue();
			}
		}
	}

	public bool HasRemotePetInfoID(int infoID)
	{
		for (int i = 0; i < this.remoteSockets.Length; i++)
		{
			if (this.remoteSockets[i] != null)
			{
				PetDataEx pet = this.remoteSockets[i].GetPet();
				if (pet != null && pet.Info.ID == infoID)
				{
					return true;
				}
			}
		}
		for (int j = 0; j < this.remoteAssistPets.Length; j++)
		{
			if (this.remoteAssistPets[j] != null)
			{
				if (this.remoteAssistPets[j] != null && this.remoteAssistPets[j].Info.ID == infoID)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetRemoteMinAssistLevel()
	{
		int num = 150;
		for (int i = 0; i < 6; i++)
		{
			if (this.remoteAssistPets[i] == null)
			{
				return 0;
			}
			if ((long)num > (long)((ulong)this.remoteAssistPets[i].Data.Level))
			{
				num = (int)this.remoteAssistPets[i].Data.Level;
			}
		}
		return num;
	}

	public int GetRemoteMinAssistFurther()
	{
		int num = 15;
		for (int i = 0; i < 6; i++)
		{
			if (this.remoteAssistPets[i] == null)
			{
				return 0;
			}
			if ((long)num > (long)((ulong)this.remoteAssistPets[i].Data.Further))
			{
				num = (int)this.remoteAssistPets[i].Data.Further;
			}
		}
		return num;
	}

	public LopetDataEx GetRemoteLopet()
	{
		return this.remoteLopet;
	}

	public SocketDataEx GetSocket(int slot, bool local)
	{
		if (local)
		{
			return this.GetSocket(slot);
		}
		return this.GetRemoteSocket(slot);
	}

	public bool HasPetInfoID(int infoID, bool local)
	{
		if (local)
		{
			return this.HasPetInfoID(infoID);
		}
		return this.HasRemotePetInfoID(infoID);
	}

	public PetDataEx GetAssist(int slot, bool local)
	{
		if (local)
		{
			return this.GetAssist(slot);
		}
		return this.GetRemoteAssist(slot);
	}

	public int GetMinAssistLevel(bool local)
	{
		if (local)
		{
			return this.GetMinAssistLevel();
		}
		return this.GetRemoteMinAssistLevel();
	}

	public int GetMinAssistFurther(bool local)
	{
		if (local)
		{
			return this.GetMinAssistFurther();
		}
		return this.GetRemoteMinAssistFurther();
	}

	public int GetLevel(bool local)
	{
		if (local)
		{
			return (int)Globals.Instance.Player.Data.Level;
		}
		return this.GetRemoteLevel();
	}

	public LopetDataEx GetLopet(bool local)
	{
		if (local)
		{
			return Globals.Instance.Player.LopetSystem.GetCurLopet(true);
		}
		return this.GetRemoteLopet();
	}

	public void SetRemotePlayerData(RemotePlayer data1, RemotePlayerDetail data2)
	{
		if (data1 == null || data2 == null)
		{
			Debug.LogError(new object[]
			{
				"data1 == null || data2 == null"
			});
			return;
		}
		this.ClearRemotePlayerData();
		this.remoteID = data1.GUID;
		this.remoteName = data1.Name;
		this.remoteLevel = data1.Level;
		this.remoteFurther = data1.FurtherLevel;
		this.remoteAwake = data1.AwakeLevel;
		this.remoteAwakeItemFlag = data1.AwakeItemFlag;
		this.remoteVipLevel = data1.VipLevel;
		this.remoteGender = data1.Gender;
		this.remoteConstellationLevel = data1.ConstellationLevel;
		this.remoteFashionLevel = data2.FashionLevel;
		if (data2.Lopet != null && data2.Lopet.InfoID != 0)
		{
			LopetInfo info = Globals.Instance.AttDB.LopetDict.GetInfo(data2.Lopet.InfoID);
			if (info == null)
			{
				Debug.LogErrorFormat("LopetDict.GetInfo error, id = {0}", new object[]
				{
					data2.Lopet.InfoID
				});
				return;
			}
			this.remoteLopet = new LopetDataEx(data2.Lopet, info);
		}
		PlayerPetInfo.Info2.Name = this.remoteName;
		PlayerPetInfo.Info2.Quality = this.GetRemoteQuality();
		PlayerPetInfo.Info2.Type = this.remoteGender;
		PetData petData = new PetData();
		if (petData == null)
		{
			Debug.LogError(new object[]
			{
				"new PetData error"
			});
			return;
		}
		petData.ID = 0uL;
		petData.InfoID = 90001;
		petData.Level = (uint)this.remoteLevel;
		petData.Further = (uint)this.remoteFurther;
		petData.Awake = (uint)this.remoteAwake;
		petData.ItemFlag = (uint)this.remoteAwakeItemFlag;
		data2.Pets.Add(petData);
		for (int i = 0; i < data2.Pets.Count; i++)
		{
			int num = (int)data2.Pets[i].ID;
			if (num >= 4)
			{
				num -= 4;
				if (num >= 6)
				{
					Debug.LogErrorFormat("remote assist pet slot error, slot = {0}", new object[]
					{
						num
					});
				}
				else
				{
					PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(data2.Pets[i].InfoID);
					if (info2 == null)
					{
						Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
						{
							data2.Pets[i].InfoID
						});
					}
					else
					{
						this.remoteAssistPets[num] = new PetDataEx(data2.Pets[i], info2);
					}
				}
			}
		}
		int fashionID = data1.FashionID;
		for (int j = 0; j < 4; j++)
		{
			this.remoteSockets[j] = new SocketDataEx();
			if (this.remoteSockets[j] == null)
			{
				Debug.LogError(new object[]
				{
					"allocate SocketDataEx error!"
				});
			}
			else
			{
				this.remoteSockets[j].Init(j, (j != 0) ? 0 : fashionID, data2);
			}
		}
		this.UpdateRemoteCombatValue();
	}

	public void ClearRemotePlayerData()
	{
		this.remoteID = 0uL;
		this.remoteName = string.Empty;
		this.remoteLevel = 0;
		this.remoteFurther = 0;
		this.remoteVipLevel = 0;
		this.remoteGender = 0;
		this.remoteCombatValue = 0;
		this.remoteConstellationLevel = 0;
		this.remoteFashionLevel = 0;
		this.remoteLopet = null;
		for (int i = 0; i < this.remoteSockets.Length; i++)
		{
			this.remoteSockets[i] = null;
		}
		for (int j = 0; j < this.remoteAssistPets.Length; j++)
		{
			this.remoteAssistPets[j] = null;
		}
	}

	public void OnMsgChangeSocket(MemoryStream stream)
	{
		MS2C_ChangeSocket mS2C_ChangeSocket = Serializer.NonGeneric.Deserialize(typeof(MS2C_ChangeSocket), stream) as MS2C_ChangeSocket;
		if (mS2C_ChangeSocket.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_ChangeSocket.Result);
			return;
		}
		SocketDataEx socket = this.GetSocket(mS2C_ChangeSocket.Slot1);
		SocketDataEx socket2 = this.GetSocket(mS2C_ChangeSocket.Slot2);
		this.SetSocket(mS2C_ChangeSocket.Slot1, socket2);
		this.SetSocket(mS2C_ChangeSocket.Slot2, socket);
		if (mS2C_ChangeSocket.SocketVersion != 0u)
		{
			this.Version = mS2C_ChangeSocket.SocketVersion;
		}
		if (this.ChangeSocketEvent != null)
		{
			this.ChangeSocketEvent();
		}
	}

	public void OnMsgSetCombatPet(MemoryStream stream)
	{
		MS2C_SetCombatPet mS2C_SetCombatPet = Serializer.NonGeneric.Deserialize(typeof(MS2C_SetCombatPet), stream) as MS2C_SetCombatPet;
		if (mS2C_SetCombatPet.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_SetCombatPet.Result);
			return;
		}
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.SetOldRelationFlag();
		PetDataEx pet = this.GetPet(mS2C_SetCombatPet.Slot);
		if (pet != null)
		{
			pet.GetAttribute(ref uiState.mOldHpNum, ref uiState.mOldAttackNum, ref uiState.mOldWufangNum, ref uiState.mOldFafangNum);
		}
		else
		{
			uiState.mOldHpNum = 0;
			uiState.mOldAttackNum = 0;
			uiState.mOldWufangNum = 0;
			uiState.mOldFafangNum = 0;
		}
		PetDataEx pet2 = Globals.Instance.Player.PetSystem.GetPet(mS2C_SetCombatPet.PetID);
		if (mS2C_SetCombatPet.Slot > 0 && mS2C_SetCombatPet.Slot < 4)
		{
			SocketDataEx socket = this.GetSocket(mS2C_SetCombatPet.Slot);
			if (socket != null)
			{
				socket.EquipPet(pet2);
			}
		}
		else if (mS2C_SetCombatPet.Slot >= 4)
		{
			int num = mS2C_SetCombatPet.Slot - 4;
			if (num >= 0 && num < 6)
			{
				this.EquipAssist(num, pet2);
			}
		}
		this.UpdateCombatValue();
		if (mS2C_SetCombatPet.SocketVersion != 0u)
		{
			this.Version = mS2C_SetCombatPet.SocketVersion;
		}
		if (this.EquipPetEvent != null && mS2C_SetCombatPet.Slot > 0)
		{
			this.EquipPetEvent(mS2C_SetCombatPet.Slot);
		}
	}

	public void OnMsgEquipItem(MemoryStream stream)
	{
		MS2C_EquipItem mS2C_EquipItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipItem), stream) as MS2C_EquipItem;
		if (mS2C_EquipItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_EquipItem.Result);
			return;
		}
		SocketDataEx socket = this.GetSocket(mS2C_EquipItem.SocketSlot);
		if (socket != null)
		{
			ItemDataEx item = Globals.Instance.Player.ItemSystem.GetItem(mS2C_EquipItem.ItemID);
			socket.EquipItem(mS2C_EquipItem.EquipSlot, item);
			this.UpdateCombatValue();
			if (this.EquipItemEvent != null)
			{
				this.EquipItemEvent(mS2C_EquipItem.SocketSlot, mS2C_EquipItem.EquipSlot);
			}
		}
		if (mS2C_EquipItem.SocketVersion != 0u)
		{
			this.Version = mS2C_EquipItem.SocketVersion;
		}
	}

	public void OnMsgAutoEquipItem(MemoryStream stream)
	{
		MS2C_AutoEquipItem mS2C_AutoEquipItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_AutoEquipItem), stream) as MS2C_AutoEquipItem;
		if (mS2C_AutoEquipItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_AutoEquipItem.Result);
			return;
		}
		SocketDataEx socket = this.GetSocket(mS2C_AutoEquipItem.SocketSlot);
		if (socket != null)
		{
			for (int i = 0; i < mS2C_AutoEquipItem.ItemID.Count; i++)
			{
				if (mS2C_AutoEquipItem.ItemID[i] != 0uL)
				{
					ItemDataEx item = Globals.Instance.Player.ItemSystem.GetItem(mS2C_AutoEquipItem.ItemID[i]);
					socket.EquipItem(i, item);
					if (this.EquipItemEvent != null)
					{
						this.EquipItemEvent(mS2C_AutoEquipItem.SocketSlot, i);
					}
				}
			}
			this.UpdateCombatValue();
		}
		if (mS2C_AutoEquipItem.SocketVersion != 0u)
		{
			this.Version = mS2C_AutoEquipItem.SocketVersion;
		}
	}

	public void OnMsgChangeFashion(MemoryStream stream)
	{
		MS2C_ChangeFashion mS2C_ChangeFashion = Serializer.NonGeneric.Deserialize(typeof(MS2C_ChangeFashion), stream) as MS2C_ChangeFashion;
		if (mS2C_ChangeFashion.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_ChangeFashion.Result);
			return;
		}
		SocketDataEx socket = this.GetSocket(0);
		if (socket != null)
		{
			socket.EquipFashion(mS2C_ChangeFashion.FashionID);
		}
		if (mS2C_ChangeFashion.SocketVersion != 0u)
		{
			this.Version = mS2C_ChangeFashion.SocketVersion;
		}
		if (this.EquipFashionEvent != null)
		{
			this.EquipFashionEvent(mS2C_ChangeFashion.FashionID);
		}
	}
}
