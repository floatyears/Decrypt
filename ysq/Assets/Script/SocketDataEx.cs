using Att;
using Proto;
using System;
using System.Collections.Generic;

public sealed class SocketDataEx
{
	private bool local = true;

	private int socketSlot;

	private PetDataEx petData;

	private ItemDataEx[] equips = new ItemDataEx[6];

	private int[] attValueMod = new int[11];

	private int[] attPctMod = new int[11];

	private int[] resist = new int[7];

	private List<TalentInfo> talents = new List<TalentInfo>();

	public int EquipMasterEnhanceLevel;

	public int EquipMasterRefineLevel;

	public int TrinketMasterEnhanceLevel;

	public int TrinketMasterRefineLevel;

	private int constellationLevel;

	private int fashionLevel;

	private FashionInfo fashion;

	private LegendSkillData[] legendSkills = new LegendSkillData[6];

	private int assistLevelID;

	private int assistFurtherID;

	private int lopetAttack;

	private int lopetPhysicDefense;

	private int lopetMagicDefense;

	private int lopetMaxHP;

	public int RelationFlag
	{
		get;
		private set;
	}

	public List<TalentInfo> Talents
	{
		get
		{
			return this.talents;
		}
	}

	public int MaxHP
	{
		get
		{
			return this.GetAtt(1);
		}
	}

	public int Attack
	{
		get
		{
			return this.GetAtt(2);
		}
	}

	public int PhysicDefense
	{
		get
		{
			return this.GetAtt(3);
		}
	}

	public int MagicDefense
	{
		get
		{
			return this.GetAtt(4);
		}
	}

	public PetDataEx GetPet()
	{
		return this.petData;
	}

	public ItemDataEx GetEquip(int slot)
	{
		if (slot < 0 || slot >= this.equips.Length)
		{
			return null;
		}
		return this.equips[slot];
	}

	public void Init(SocketData data, int slot, int fashionID)
	{
		this.local = true;
		this.socketSlot = slot;
		for (int i = 0; i < data.ItemID.Count; i++)
		{
			if (i >= this.equips.Length)
			{
				Debug.LogError(new object[]
				{
					"SocketData item size error"
				});
				break;
			}
			this.equips[i] = Globals.Instance.Player.ItemSystem.GetItem(data.ItemID[i]);
			if (this.equips[i] != null)
			{
				this.equips[i].SetEquipSlot(this.socketSlot * 6 + i);
			}
		}
		this.EquipFashion(fashionID);
		PetDataEx data2 = null;
		if (data.PetID != 0uL)
		{
			data2 = Globals.Instance.Player.PetSystem.GetPet(data.PetID);
		}
		this.EquipPet(data2);
	}

	public void Init(int slot, int fashionID, RemotePlayerDetail data)
	{
		this.local = false;
		this.socketSlot = slot;
		int num = this.socketSlot * 6;
		for (int i = 0; i < data.Equips.Count; i++)
		{
			int num2 = (int)data.Equips[i].ID - num;
			if (num2 >= 0 && num2 < 6)
			{
				ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(data.Equips[i].InfoID);
				if (info == null)
				{
					Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
					{
						data.Equips[i].InfoID
					});
				}
				else
				{
					this.equips[num2] = new ItemDataEx(data.Equips[i], info);
					if (this.equips[num2] != null)
					{
						this.equips[num2].SetEquipSlot(this.socketSlot * 6 + num2);
					}
				}
			}
		}
		if (fashionID != 0)
		{
			this.fashion = Globals.Instance.AttDB.FashionDict.GetInfo(fashionID);
			if (this.fashion == null)
			{
				Debug.LogErrorFormat("FashionDict.GetInfo error, id = {0}", new object[]
				{
					fashionID
				});
			}
			else
			{
				PlayerPetInfo.Info2.Icon = this.fashion.Icon;
			}
		}
		for (int j = 0; j < data.Pets.Count; j++)
		{
			if (this.socketSlot == (int)data.Pets[j].ID)
			{
				PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(data.Pets[j].InfoID);
				if (info2 != null)
				{
					this.petData = new PetDataEx(data.Pets[j], info2);
					this.EquipPet(this.petData);
					break;
				}
				Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
				{
					data.Pets[j].InfoID
				});
			}
		}
	}

	public void Clear()
	{
		this.ClearAttMod();
		this.RelationFlag = 0;
		this.EquipMasterEnhanceLevel = 0;
		this.EquipMasterRefineLevel = 0;
		this.TrinketMasterEnhanceLevel = 0;
		this.TrinketMasterRefineLevel = 0;
		this.constellationLevel = 0;
		this.fashionLevel = 0;
		for (int i = 0; i < this.legendSkills.Length; i++)
		{
			if (this.legendSkills[i] != null)
			{
				this.legendSkills[i].EffectType = 0;
				this.legendSkills[i].Value1 = 0;
				this.legendSkills[i].Value2 = 0;
				this.legendSkills[i].Value3 = 0;
			}
		}
		this.assistLevelID = 0;
		this.assistFurtherID = 0;
		this.lopetAttack = 0;
		this.lopetPhysicDefense = 0;
		this.lopetMagicDefense = 0;
		this.lopetMaxHP = 0;
	}

	public int GetAtt(int type)
	{
		if (this.petData == null || type < 0 || type >= 11)
		{
			return 0;
		}
		long num = (long)this.attValueMod[type];
		long num2 = (long)this.attPctMod[type];
		int num3 = (int)(num + num * num2 / 10000L);
		if (num3 < 0)
		{
			return 0;
		}
		return num3;
	}

	public int GetResist(int type)
	{
		if (this.petData == null || type < 0 || type >= 7 || this.resist[type] < 0)
		{
			return 0;
		}
		return this.resist[type];
	}

	public string GetResLoc()
	{
		if (this.fashion != null)
		{
			return this.fashion.ResLoc;
		}
		return this.petData.Info.ResLoc;
	}

	public string GetWeaponResLoc()
	{
		if (this.fashion != null)
		{
			return this.fashion.WeaponResLoc;
		}
		return string.Empty;
	}

	public string GetIcon()
	{
		if (this.fashion != null)
		{
			return this.fashion.Icon;
		}
		return this.petData.Info.Icon;
	}

	public int GetGender()
	{
		return this.petData.Info.Type;
	}

	public LegendSkillData GetLegendSkill(int slot)
	{
		if (slot < 0 || slot >= this.legendSkills.Length)
		{
			return null;
		}
		return this.legendSkills[slot];
	}

	public void SetSocketSlot(int slot)
	{
		this.socketSlot = slot;
		if (this.petData != null)
		{
			this.petData.SetSocketSlot(this.socketSlot);
		}
		for (int i = 0; i < 6; i++)
		{
			if (this.equips[i] != null)
			{
				if (this.socketSlot >= 0)
				{
					this.equips[i].SetEquipSlot(this.socketSlot * 6 + i);
				}
				else
				{
					this.equips[i].SetEquipSlot(-1);
				}
			}
		}
	}

	private void ClearAttMod()
	{
		Array.Clear(this.attValueMod, 0, this.attValueMod.Length);
		Array.Clear(this.attPctMod, 0, this.attPctMod.Length);
		Array.Clear(this.resist, 0, this.resist.Length);
	}

	private void HandleAttMod(int modType, int attID, int attValue, bool apply)
	{
		if (attID <= 0)
		{
			return;
		}
		if (attID < 11)
		{
			if (modType == 0)
			{
				this.attValueMod[attID] += ((!apply) ? (-attValue) : attValue);
			}
			else
			{
				this.attPctMod[attID] += ((!apply) ? (-attValue) : attValue);
			}
		}
		else if (attID == 20)
		{
			if (modType == 0)
			{
				this.attValueMod[3] += ((!apply) ? (-attValue) : attValue);
				this.attValueMod[4] += ((!apply) ? (-attValue) : attValue);
			}
			else
			{
				this.attPctMod[3] += ((!apply) ? (-attValue) : attValue);
				this.attPctMod[4] += ((!apply) ? (-attValue) : attValue);
			}
		}
		else
		{
			int num = attID - 300;
			if (num <= 0 || num >= 7)
			{
				Debug.LogErrorFormat("attID error, attID = {0}", new object[]
				{
					attID
				});
				return;
			}
			this.resist[num] += ((!apply) ? (-attValue) : attValue);
		}
	}

	private void CalculatePetAttPart()
	{
		if (this.petData == null)
		{
			return;
		}
		int attValue = 0;
		int attValue2 = 0;
		int attValue3 = 0;
		int attValue4 = 0;
		this.petData.GetBastAtt(ref attValue, ref attValue2, ref attValue3, ref attValue4);
		this.HandleAttMod(0, 1, attValue, true);
		this.HandleAttMod(0, 2, attValue2, true);
		this.HandleAttMod(0, 3, attValue3, true);
		this.HandleAttMod(0, 4, attValue4, true);
		this.HandleAttMod(0, 5, this.petData.Info.Hit, true);
		this.HandleAttMod(0, 6, this.petData.Info.Dodge, true);
		this.HandleAttMod(0, 7, this.petData.Info.Crit, true);
		this.HandleAttMod(0, 8, this.petData.Info.CritResist, true);
		for (int i = 0; i < 4; i++)
		{
			if (i >= this.equips.Length)
			{
				break;
			}
			if (this.equips[i] != null)
			{
				this.HandleItemEffect(this.equips[i], this.equips[i].GetEquipEnhanceLevel(), this.equips[i].GetEquipRefineLevel());
			}
		}
		for (int j = 0; j < 2; j++)
		{
			int num = 4 + j;
			if (num >= this.equips.Length)
			{
				break;
			}
			if (this.equips[num] != null)
			{
				this.HandleItemEffect(this.equips[num], this.equips[num].GetTrinketEnhanceLevel(), this.equips[num].GetTrinketRefineLevel());
			}
		}
		this.HandleMasterEffect();
		this.HandleItemSetEffect();
		this.HandleConstellationEffect();
		this.HandleFashionEffect();
		this.HandleAwakeEffect();
		this.HandleAssistLevelEffect(-1);
		this.HandleAssistFurtherEffect(-1);
		this.HandleCultivateEffect(this.petData.Data.Attack, this.petData.Data.PhysicDefense, this.petData.Data.MagicDefense, this.petData.Data.MaxHP);
		this.HandleLopetEffect();
	}

	public void UpdateData()
	{
		this.EquipPet(this.petData);
	}

	public void EquipPet(PetDataEx data)
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (this.talents.Count > 0)
		{
			for (int i = 0; i < 4; i++)
			{
				SocketDataEx socket = teamSystem.GetSocket(i, this.local);
				if (socket != null && socket != this)
				{
					for (int j = 0; j < this.talents.Count; j++)
					{
						socket.HandleTalentEffect(this.talents[j], false);
					}
				}
			}
		}
		this.talents.Clear();
		this.Clear();
		if (this.petData != null)
		{
			this.petData.SetSocketSlot(-1);
		}
		this.petData = data;
		for (int k = 0; k < 4; k++)
		{
			SocketDataEx socket = teamSystem.GetSocket(k, this.local);
			if (socket != null)
			{
				socket.HandleRelationEffect();
			}
		}
		if (this.petData == null)
		{
			return;
		}
		this.petData.SetSocketSlot(this.socketSlot);
		int num = 0;
		while ((long)num < (long)((ulong)this.petData.Data.Further) && num < this.petData.Info.TalentID.Count)
		{
			if (this.petData.Info.TalentID[num] != 0)
			{
				TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(this.petData.Info.TalentID[num]);
				if (info == null)
				{
					Debug.LogErrorFormat("TalentDict.GetInfo error, id = {0}", new object[]
					{
						this.petData.Info.TalentID[num]
					});
				}
				else if (info.TargetType == 1 || info.TargetType == 2)
				{
					this.talents.Add(info);
					for (int l = 0; l < 4; l++)
					{
						SocketDataEx socket = teamSystem.GetSocket(l, this.local);
						if (socket != null)
						{
							socket.HandleTalentEffect(info, true);
						}
					}
				}
				else
				{
					this.HandleTalentEffect(info, true);
				}
			}
			num++;
		}
		for (int m = 0; m < 4; m++)
		{
			SocketDataEx socket = teamSystem.GetSocket(m, this.local);
			if (socket != null && socket != this)
			{
				for (int n = 0; n < socket.Talents.Count; n++)
				{
					this.HandleTalentEffect(socket.Talents[n], true);
				}
			}
		}
		this.CalculatePetAttPart();
	}

	public void EquipItem(int slot, ItemDataEx data)
	{
		if (slot < 0 || slot >= 6)
		{
			Debug.LogErrorFormat("equip slot error, slot = {0}", new object[]
			{
				slot
			});
			return;
		}
		if (this.equips[slot] != null)
		{
			this.equips[slot].SetEquipSlot(-1);
		}
		this.equips[slot] = data;
		if (this.equips[slot] != null)
		{
			this.equips[slot].SetEquipSlot(this.socketSlot * 6 + slot);
		}
		if (this.petData == null)
		{
			return;
		}
		this.RecalculateSelf();
	}

	public void RecalculateSelf()
	{
		this.Clear();
		this.HandleRelationEffect();
		int num = 0;
		while ((long)num < (long)((ulong)this.petData.Data.Further) && num < this.petData.Info.TalentID.Count)
		{
			if (this.petData.Info.TalentID[num] != 0)
			{
				TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(this.petData.Info.TalentID[num]);
				if (info == null)
				{
					Debug.LogErrorFormat("TalentDict.GetInfo error, id = {0}", new object[]
					{
						this.petData.Info.TalentID[num]
					});
				}
				else
				{
					this.HandleTalentEffect(info, true);
				}
			}
			num++;
		}
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(i, this.local);
			if (socket != null && socket != this)
			{
				for (int j = 0; j < socket.Talents.Count; j++)
				{
					this.HandleTalentEffect(socket.Talents[j], true);
				}
			}
		}
		this.CalculatePetAttPart();
	}

	public void HandleFashionEffect()
	{
		if (!this.IsPlayer())
		{
			return;
		}
		int num;
		if (this.local)
		{
			num = Globals.Instance.Player.ItemSystem.GetValidFashionCount();
		}
		else
		{
			num = Globals.Instance.Player.TeamSystem.GeRemoteFashionLevel();
		}
		if (this.fashionLevel == num)
		{
			return;
		}
		if (this.fashionLevel != 0)
		{
			TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(this.fashionLevel);
			if (info == null)
			{
				Debug.LogErrorFormat("TinyLevelDict.GetInfo error, ID = {0}", new object[]
				{
					this.fashionLevel
				});
				return;
			}
			this.HandleAttMod(0, 1, info.FashionMaxHP, false);
			this.HandleAttMod(0, 2, info.FashionAttack, false);
		}
		this.fashionLevel = num;
		if (this.fashionLevel != 0)
		{
			TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(this.fashionLevel);
			if (info == null)
			{
				Debug.LogErrorFormat("TinyLevelDict.GetInfo error, level = {0}", new object[]
				{
					this.fashionLevel
				});
				return;
			}
			this.HandleAttMod(0, 1, info.FashionMaxHP, true);
			this.HandleAttMod(0, 2, info.FashionAttack, true);
		}
	}

	public void HandleAwakeEffect()
	{
		int awake = (int)this.petData.Data.Awake;
		int elementType = this.petData.Info.ElementType;
		if (awake > 0)
		{
			AttMod attMod = Awake.GetAttValueMod(elementType, awake);
			if (attMod == null)
			{
				Debug.LogErrorFormat("Awake.GetAttValueMod error, element = {0}, level = {1}", new object[]
				{
					elementType,
					awake
				});
				return;
			}
			this.HandleAttMod(0, 2, attMod.Attack, true);
			this.HandleAttMod(0, 3, attMod.Defense, true);
			this.HandleAttMod(0, 4, attMod.Defense, true);
			this.HandleAttMod(0, 1, attMod.MaxHP, true);
			int num = Awake.GetAttPctMod(awake);
			if (num > 0)
			{
				this.HandleAttMod(1, 2, num, true);
				this.HandleAttMod(1, 3, num, true);
				this.HandleAttMod(1, 4, num, true);
				this.HandleAttMod(1, 1, num, true);
			}
		}
		AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo(awake + 1);
		if (info == null)
		{
			Debug.LogErrorFormat("AwakeDict.GetInfo error, level = {0}", new object[]
			{
				awake + 1
			});
			return;
		}
		int itemFlag = (int)this.petData.Data.ItemFlag;
		for (int i = 0; i < 4; i++)
		{
			int index = elementType * 4 + i;
			if (info.ItemID[index] != 0 && (itemFlag & 1 << i) != 0)
			{
				ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(info.ItemID[index]);
				if (info2 == null)
				{
					Debug.LogErrorFormat("ItemDict.GetInfo error, ID = {0}", new object[]
					{
						info.ItemID[index]
					});
				}
				else
				{
					if (info2.Value1 > 0)
					{
						this.HandleAttMod(0, 2, info2.Value1, true);
					}
					if (info2.Value2 > 0)
					{
						this.HandleAttMod(0, 3, info2.Value2, true);
						this.HandleAttMod(0, 4, info2.Value2, true);
					}
					if (info2.Value3 > 0)
					{
						this.HandleAttMod(0, 1, info2.Value3, true);
					}
				}
			}
		}
	}

	public void HandleAssistLevelEffect(int id = -1)
	{
		if (Globals.Instance.Player.TeamSystem.GetLevel(this.local) < GameConst.GetInt32(29))
		{
			return;
		}
		if (id < 0)
		{
			int minAssistLevel = Globals.Instance.Player.TeamSystem.GetMinAssistLevel(this.local);
			id = TinyLevel.GetAssistLevelID(minAssistLevel);
		}
		if (this.assistLevelID == id)
		{
			return;
		}
		if (this.assistLevelID != 0)
		{
			TinyLevelInfo info = TinyLevel.GetInfo(this.assistLevelID);
			if (info == null)
			{
				Debug.LogErrorFormat("TinyLevel.GetInfo error, ID = {0}", new object[]
				{
					this.assistLevelID
				});
				return;
			}
			this.HandleAttMod(0, 1, info.ALMaxHP, false);
			this.HandleAttMod(0, 2, info.ALAttack, false);
			this.HandleAttMod(0, 3, info.ALDefense, false);
			this.HandleAttMod(0, 4, info.ALDefense, false);
		}
		this.assistLevelID = id;
		if (this.assistLevelID != 0)
		{
			TinyLevelInfo info = TinyLevel.GetInfo(this.assistLevelID);
			if (info == null)
			{
				Debug.LogErrorFormat("TinyLevel.GetInfo error, ID = {0}", new object[]
				{
					this.assistLevelID
				});
				return;
			}
			this.HandleAttMod(0, 1, info.ALMaxHP, true);
			this.HandleAttMod(0, 2, info.ALAttack, true);
			this.HandleAttMod(0, 3, info.ALDefense, true);
			this.HandleAttMod(0, 4, info.ALDefense, true);
		}
	}

	public void HandleAssistFurtherEffect(int id = -1)
	{
		if (Globals.Instance.Player.TeamSystem.GetLevel(this.local) < GameConst.GetInt32(30))
		{
			return;
		}
		if (id < 0)
		{
			int minAssistFurther = Globals.Instance.Player.TeamSystem.GetMinAssistFurther(this.local);
			id = TinyLevel.GetAssistFurtherID(minAssistFurther);
		}
		if (this.assistFurtherID == id)
		{
			return;
		}
		if (this.assistFurtherID != 0)
		{
			TinyLevelInfo info = TinyLevel.GetInfo(this.assistFurtherID);
			if (info == null)
			{
				Debug.LogErrorFormat("TinyLevel.GetInfo error, ID = {0}", new object[]
				{
					this.assistFurtherID
				});
				return;
			}
			this.HandleAttMod(0, 1, info.AFMaxHP, false);
			this.HandleAttMod(0, 2, info.AFAttack, false);
			this.HandleAttMod(0, 3, info.AFDefense, false);
			this.HandleAttMod(0, 4, info.AFDefense, false);
		}
		this.assistFurtherID = id;
		if (this.assistFurtherID != 0)
		{
			TinyLevelInfo info = TinyLevel.GetInfo(this.assistFurtherID);
			if (info == null)
			{
				Debug.LogErrorFormat("TinyLevel.GetInfo error, ID = {0}", new object[]
				{
					this.assistFurtherID
				});
				return;
			}
			this.HandleAttMod(0, 1, info.AFMaxHP, true);
			this.HandleAttMod(0, 2, info.AFAttack, true);
			this.HandleAttMod(0, 3, info.AFDefense, true);
			this.HandleAttMod(0, 4, info.AFDefense, true);
		}
	}

	public void EquipFashion(int fashionID)
	{
		if (this.socketSlot != 0 || fashionID == 0)
		{
			return;
		}
		FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(fashionID);
		if (info == null)
		{
			Debug.LogErrorFormat("FashionDict.GetInfo error, id = {0}", new object[]
			{
				fashionID
			});
			return;
		}
		this.fashion = info;
		PlayerPetInfo.Info.Icon = this.fashion.Icon;
	}

	public int GetFashionID()
	{
		if (this.fashion == null)
		{
			return 0;
		}
		return this.fashion.ID;
	}

	public bool IsPlayer()
	{
		return this.socketSlot == 0;
	}

	public void OnPetUpdate(int addLevel, int addFurther)
	{
		if (this.petData == null)
		{
			Debug.LogError(new object[]
			{
				"petData error, == null"
			});
			return;
		}
		int num = (int)(this.petData.Data.Level - (uint)addLevel);
		int num2 = (int)(this.petData.Data.Further - (uint)addFurther);
		this.petData.Data.Level = (uint)num;
		this.petData.Data.Further = (uint)num2;
		int attValue = 0;
		int attValue2 = 0;
		int attValue3 = 0;
		int attValue4 = 0;
		this.petData.GetBastAtt(ref attValue, ref attValue2, ref attValue3, ref attValue4);
		this.HandleAttMod(0, 1, attValue, false);
		this.HandleAttMod(0, 2, attValue2, false);
		this.HandleAttMod(0, 3, attValue3, false);
		this.HandleAttMod(0, 4, attValue4, false);
		this.petData.Data.Level = (uint)(num + addLevel);
		this.petData.Data.Further = (uint)(num2 + addFurther);
		attValue = 0;
		attValue2 = 0;
		attValue3 = 0;
		attValue4 = 0;
		this.petData.GetBastAtt(ref attValue, ref attValue2, ref attValue3, ref attValue4);
		this.HandleAttMod(0, 1, attValue, true);
		this.HandleAttMod(0, 2, attValue2, true);
		this.HandleAttMod(0, 3, attValue3, true);
		this.HandleAttMod(0, 4, attValue4, true);
		if (addFurther > 0)
		{
			TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
			int num3 = num2;
			while ((long)num3 < (long)((ulong)this.petData.Data.Further) && num3 < this.petData.Info.TalentID.Count)
			{
				if (this.petData.Info.TalentID[num3] != 0)
				{
					TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(this.petData.Info.TalentID[num3]);
					if (info == null)
					{
						Debug.LogErrorFormat("TalentDict.GetInfo error, id = {0}", new object[]
						{
							this.petData.Info.TalentID[num3]
						});
					}
					else if (info.TargetType == 1 || info.TargetType == 2)
					{
						this.talents.Add(info);
						for (int i = 0; i < 4; i++)
						{
							SocketDataEx socket = teamSystem.GetSocket(i, this.local);
							if (socket != null)
							{
								socket.HandleTalentEffect(info, true);
							}
						}
					}
					else
					{
						this.HandleTalentEffect(info, true);
					}
				}
				num3++;
			}
		}
	}

	public bool HasItemInfoID(int infoID)
	{
		for (int i = 0; i < this.equips.Length; i++)
		{
			if (this.equips[i] != null)
			{
				if (this.equips[i].Info.ID == infoID)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsRelationActive(RelationInfo info)
	{
		if (info == null || this.petData == null)
		{
			return false;
		}
		if (info.Type == 0)
		{
			for (int i = 0; i < info.PetID.Count; i++)
			{
				if (info.PetID[i] != 0 && info.PetID[i] != this.petData.Info.ID)
				{
					if (!Globals.Instance.Player.TeamSystem.HasPetInfoID(info.PetID[i], this.local))
					{
						return false;
					}
				}
			}
		}
		else if (info.ItemID != 0 && !this.HasItemInfoID(info.ItemID))
		{
			return false;
		}
		return true;
	}

	public bool IsRelationActive(int index)
	{
		return 0 != (this.RelationFlag & 1 << index);
	}

	public void HandleRelationEffect()
	{
		int relationFlag = this.RelationFlag;
		this.RelationFlag = 0;
		if (this.petData == null)
		{
			return;
		}
		for (int i = 0; i < this.petData.Info.RelationID.Count; i++)
		{
			if (this.petData.Info.RelationID[i] != 0)
			{
				RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(this.petData.Info.RelationID[i]);
				if (info == null)
				{
					Debug.LogErrorFormat("RelationDict.GetInfo error, id = {0}", new object[]
					{
						this.petData.Info.RelationID[i]
					});
				}
				else
				{
					if (this.IsRelationActive(info))
					{
						this.RelationFlag |= 1 << i;
					}
					if ((relationFlag & 1 << i) != (this.RelationFlag & 1 << i))
					{
						bool apply = this.IsRelationActive(i);
						for (int j = 0; j < info.AttID.Count; j++)
						{
							this.HandleAttMod(1, info.AttID[j], info.AttPct[j], apply);
						}
					}
				}
			}
		}
	}

	public void HandleTalentEffect(TalentInfo info, bool apply)
	{
		if (info == null || this.petData == null)
		{
			return;
		}
		if (info.TargetType == 2 && this.petData.Info.ElementType != info.TargetValue)
		{
			return;
		}
		int effectType = info.EffectType;
		if (effectType != 1)
		{
			if (effectType == 2)
			{
				this.HandleAttMod(1, info.Value1, info.Value2, apply);
			}
		}
		else
		{
			this.HandleAttMod(0, info.Value1, info.Value2, apply);
		}
	}

	public void HandleItemEffect(ItemDataEx data, int addEnhanceLevel, int addRefineLevel)
	{
		if (data == null || this.petData == null)
		{
			return;
		}
		int type = data.Info.Type;
		if (type != 0)
		{
			if (type != 1)
			{
				Debug.LogErrorFormat("ItemInfo error, ID = {0}, type = {1}", new object[]
				{
					data.Info.ID,
					data.Info.Type
				});
			}
			else
			{
				int subType = data.Info.SubType;
				if (subType != 0)
				{
					if (subType != 1)
					{
						Debug.LogErrorFormat("ItemInfo error, ID = {0}, sub_type = {1}", new object[]
						{
							data.Info.ID,
							data.Info.SubType
						});
					}
					else
					{
						this.HandleAttMod(0, 1, data.Info.Value1 * addEnhanceLevel, true);
						this.HandleAttMod(1, 1, data.Info.Value3 * addRefineLevel, true);
						if (data.Info.Value5 == 0)
						{
							this.HandleAttMod(0, 6, data.Info.Value2 * addEnhanceLevel, true);
							this.HandleAttMod(0, 8, data.Info.Value4 * addRefineLevel, true);
						}
						else
						{
							this.HandleAttMod(0, 8, data.Info.Value2 * addEnhanceLevel, true);
							this.HandleAttMod(0, 6, data.Info.Value4 * addRefineLevel, true);
						}
					}
				}
				else
				{
					this.HandleAttMod(0, 2, data.Info.Value1 * addEnhanceLevel, true);
					this.HandleAttMod(1, 2, data.Info.Value3 * addRefineLevel, true);
					if (data.Info.Value5 == 0)
					{
						this.HandleAttMod(0, 7, data.Info.Value2 * addEnhanceLevel, true);
						this.HandleAttMod(0, 5, data.Info.Value4 * addRefineLevel, true);
					}
					else
					{
						this.HandleAttMod(0, 5, data.Info.Value2 * addEnhanceLevel, true);
						this.HandleAttMod(0, 7, data.Info.Value4 * addRefineLevel, true);
					}
				}
			}
		}
		else
		{
			switch (data.Info.SubType)
			{
			case 0:
				this.HandleAttMod(0, 2, data.Info.Value1 * addEnhanceLevel, true);
				this.HandleAttMod(0, 2, data.Info.Value3 * addRefineLevel, true);
				this.HandleAttMod(0, 7, data.Info.Value4 * addRefineLevel, true);
				break;
			case 1:
				this.HandleAttMod(0, 3, data.Info.Value1 * addEnhanceLevel, true);
				this.HandleAttMod(0, 3, data.Info.Value3 * addRefineLevel, true);
				this.HandleAttMod(0, 8, data.Info.Value4 * addRefineLevel, true);
				break;
			case 2:
				this.HandleAttMod(0, 4, data.Info.Value1 * addEnhanceLevel, true);
				this.HandleAttMod(0, 4, data.Info.Value3 * addRefineLevel, true);
				this.HandleAttMod(0, 6, data.Info.Value4 * addRefineLevel, true);
				break;
			case 3:
				this.HandleAttMod(0, 1, data.Info.Value1 * addEnhanceLevel, true);
				this.HandleAttMod(0, 1, data.Info.Value3 * addRefineLevel, true);
				this.HandleAttMod(0, 5, data.Info.Value4 * addRefineLevel, true);
				break;
			default:
				Debug.LogErrorFormat("ItemInfo error, ID = {0}, sub_type = {1}", new object[]
				{
					data.Info.ID,
					data.Info.SubType
				});
				break;
			}
		}
		LegendInfo info = Globals.Instance.AttDB.LegendDict.GetInfo(data.Info.Quality * 10000 + data.Info.Type * 100 + data.Info.SubType);
		if (info != null)
		{
			int value = data.Data.Value2;
			int num = value - addRefineLevel;
			if (num < 0)
			{
				num = 0;
			}
			for (int i = 0; i < info.EffectType.Count; i++)
			{
				if (num < info.RefineLevel[i])
				{
					if (info.EffectType[i] == 0)
					{
						break;
					}
					if (value < info.RefineLevel[i])
					{
						break;
					}
					if (info.EffectType[i] == 1)
					{
						this.HandleAttMod(0, info.Value1[i], info.Value2[i], true);
					}
					else if (info.EffectType[i] == 2)
					{
						this.HandleAttMod(1, info.Value1[i], info.Value2[i], true);
					}
					else
					{
						int equipSlot = data.GetEquipSlot();
						if (this.legendSkills[equipSlot] == null)
						{
							this.legendSkills[equipSlot] = new LegendSkillData();
							if (this.legendSkills[equipSlot] == null)
							{
								Debug.LogError(new object[]
								{
									"allocate LegendSkillData error"
								});
								goto IL_55B;
							}
						}
						this.legendSkills[equipSlot].EffectType = info.EffectType[i];
						this.legendSkills[equipSlot].Value1 = info.Value1[i];
						this.legendSkills[equipSlot].Value2 = info.Value2[i];
						this.legendSkills[equipSlot].Value3 = info.Value3[i];
					}
				}
				IL_55B:;
			}
		}
	}

	public void HandleMasterEffect()
	{
		this.HandleEquipMasterEnhanceEffect();
		this.HandleEquipMasterRefineEffect();
		this.HandleTrinketMasterEnhanceEffect();
		this.HandleTrinketMasterRefineEffect();
	}

	public void HandleEquipMasterEnhanceEffect()
	{
		int num = 2147483647;
		for (int i = 0; i < 4; i++)
		{
			if (i >= this.equips.Length)
			{
				break;
			}
			if (this.equips[i] == null)
			{
				num = 0;
				break;
			}
			if (num > this.equips[i].GetEquipEnhanceLevel())
			{
				num = this.equips[i].GetEquipEnhanceLevel();
			}
		}
		num = Master.GetEquipMasterEnhanceLevel(num);
		if (this.EquipMasterEnhanceLevel != num)
		{
			MasterInfo info = Master.GetInfo(this.EquipMasterEnhanceLevel);
			if (info != null)
			{
				for (int j = 0; j < info.EEAttID.Count; j++)
				{
					this.HandleAttMod(0, info.EEAttID[j], info.EEAttValue[j], false);
				}
			}
			this.EquipMasterEnhanceLevel = num;
			info = Master.GetInfo(this.EquipMasterEnhanceLevel);
			if (info != null)
			{
				for (int k = 0; k < info.EEAttID.Count; k++)
				{
					this.HandleAttMod(0, info.EEAttID[k], info.EEAttValue[k], true);
				}
			}
		}
	}

	public void HandleEquipMasterRefineEffect()
	{
		int num = 2147483647;
		for (int i = 0; i < 4; i++)
		{
			if (i >= this.equips.Length)
			{
				break;
			}
			if (this.equips[i] == null)
			{
				num = 0;
				break;
			}
			if (num > this.equips[i].GetEquipRefineLevel())
			{
				num = this.equips[i].GetEquipRefineLevel();
			}
		}
		num = Master.GetEquipMasterRefineLevel(num);
		if (this.EquipMasterRefineLevel != num)
		{
			MasterInfo info = Master.GetInfo(this.EquipMasterRefineLevel);
			if (info != null)
			{
				for (int j = 0; j < info.ERAttID.Count; j++)
				{
					this.HandleAttMod(0, info.ERAttID[j], info.ERAttValue[j], false);
				}
			}
			this.EquipMasterRefineLevel = num;
			info = Master.GetInfo(this.EquipMasterRefineLevel);
			if (info != null)
			{
				for (int k = 0; k < info.ERAttID.Count; k++)
				{
					this.HandleAttMod(0, info.ERAttID[k], info.ERAttValue[k], true);
				}
			}
		}
	}

	public void HandleTrinketMasterEnhanceEffect()
	{
		int num = 2147483647;
		for (int i = 0; i < 2; i++)
		{
			int num2 = 4 + i;
			if (num2 >= this.equips.Length)
			{
				break;
			}
			if (this.equips[num2] == null)
			{
				num = 0;
				break;
			}
			if (num > this.equips[num2].GetTrinketEnhanceLevel())
			{
				num = this.equips[num2].GetTrinketEnhanceLevel();
			}
		}
		num = Master.GetTrinketMasterEnhanceLevel(num);
		if (this.TrinketMasterEnhanceLevel != num)
		{
			MasterInfo info = Master.GetInfo(this.TrinketMasterEnhanceLevel);
			if (info != null)
			{
				for (int j = 0; j < info.TEAttID.Count; j++)
				{
					this.HandleAttMod(0, info.TEAttID[j], info.TEAttValue[j], false);
				}
			}
			this.TrinketMasterEnhanceLevel = num;
			info = Master.GetInfo(this.TrinketMasterEnhanceLevel);
			if (info != null)
			{
				for (int k = 0; k < info.TEAttID.Count; k++)
				{
					this.HandleAttMod(0, info.TEAttID[k], info.TEAttValue[k], true);
				}
			}
		}
	}

	public void HandleTrinketMasterRefineEffect()
	{
		int num = 2147483647;
		for (int i = 0; i < 2; i++)
		{
			int num2 = 4 + i;
			if (num2 >= this.equips.Length)
			{
				break;
			}
			if (this.equips[num2] == null)
			{
				num = 0;
				break;
			}
			if (num > this.equips[num2].GetTrinketRefineLevel())
			{
				num = this.equips[num2].GetTrinketRefineLevel();
			}
		}
		num = Master.GetTrinketMasterRefineLevel(num);
		if (this.TrinketMasterRefineLevel != num)
		{
			MasterInfo info = Master.GetInfo(this.TrinketMasterRefineLevel);
			if (info != null)
			{
				for (int j = 0; j < info.TRAttID.Count; j++)
				{
					this.HandleAttMod(0, info.TRAttID[j], info.TRAttValue[j], false);
				}
			}
			this.TrinketMasterRefineLevel = num;
			info = Master.GetInfo(this.TrinketMasterRefineLevel);
			if (info != null)
			{
				for (int k = 0; k < info.TRAttID.Count; k++)
				{
					this.HandleAttMod(0, info.TRAttID[k], info.TRAttValue[k], true);
				}
			}
		}
	}

	public void HandleItemSetEffect()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < 4; i++)
		{
			if (i >= this.equips.Length)
			{
				break;
			}
			if (this.equips[i] != null)
			{
				int value = this.equips[i].Info.Value5;
				if (value != 0)
				{
					if (dictionary.ContainsKey(value))
					{
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_5F = dictionary2 = dictionary;
						int num;
						int expr_63 = num = value;
						num = dictionary2[num];
						expr_5F[expr_63] = num + 1;
					}
					else
					{
						dictionary[value] = 1;
					}
				}
			}
		}
		foreach (KeyValuePair<int, int> current in dictionary)
		{
			ItemSetInfo info = Globals.Instance.AttDB.ItemSetDict.GetInfo(current.Key);
			if (info == null)
			{
				Debug.LogErrorFormat("ItemSetDict.GetInfo error, id = {0}", new object[]
				{
					current.Key
				});
			}
			else
			{
				for (int j = 0; j < info.Count.Count; j++)
				{
					if (info.Count[j] != 0 && current.Value >= info.Count[j])
					{
						this.HandleAttMod(0, info.AttID1[j], info.AttValue1[j], true);
						this.HandleAttMod(0, info.AttID2[j], info.AttValue2[j], true);
					}
				}
			}
		}
	}

	public void HandleConstellationEffect()
	{
		int num;
		if (this.local)
		{
			num = Globals.Instance.Player.Data.ConstellationLevel;
		}
		else
		{
			num = Globals.Instance.Player.TeamSystem.GeRemoteConstellationLevel();
		}
		if (this.constellationLevel == num)
		{
			return;
		}
		if (this.constellationLevel != 0)
		{
			ConInfo conInfo = ConLevelInfo.GetConInfo(this.constellationLevel);
			if (conInfo == null)
			{
				Debug.LogErrorFormat("ConLevelInfo.GetConInfo error, level = {0}", new object[]
				{
					this.constellationLevel
				});
				return;
			}
			this.HandleAttMod(0, 1, conInfo.MaxHP, false);
			this.HandleAttMod(0, 2, conInfo.Attack, false);
			this.HandleAttMod(0, 3, conInfo.PhysicDefense, false);
			this.HandleAttMod(0, 4, conInfo.MagicDefense, false);
		}
		this.constellationLevel = num;
		if (num != 0)
		{
			ConInfo conInfo = ConLevelInfo.GetConInfo(this.constellationLevel);
			if (conInfo == null)
			{
				Debug.LogErrorFormat("ConLevelInfo.GetConInfo error, level = {0}", new object[]
				{
					this.constellationLevel
				});
				return;
			}
			this.HandleAttMod(0, 1, conInfo.MaxHP, true);
			this.HandleAttMod(0, 2, conInfo.Attack, true);
			this.HandleAttMod(0, 3, conInfo.PhysicDefense, true);
			this.HandleAttMod(0, 4, conInfo.MagicDefense, true);
		}
	}

	public void OnAwakeUpdate()
	{
		AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo((int)this.petData.Data.Awake);
		if (info == null)
		{
			Debug.LogErrorFormat("AwakeDict.GetInfo error, level = {0}", new object[]
			{
				this.petData.Data.Awake
			});
			return;
		}
		if (info.Attack > 0)
		{
			this.HandleAttMod(0, 2, info.Attack, true);
		}
		if (info.Defense > 0)
		{
			this.HandleAttMod(0, 3, info.Defense, true);
			this.HandleAttMod(0, 4, info.Defense, true);
		}
		if (info.MaxHP > 0)
		{
			this.HandleAttMod(0, 1, info.MaxHP, true);
		}
		if (info.AttPct > 0)
		{
			this.HandleAttMod(1, 2, info.AttPct, true);
			this.HandleAttMod(1, 3, info.AttPct, true);
			this.HandleAttMod(1, 4, info.AttPct, true);
			this.HandleAttMod(1, 1, info.AttPct, true);
		}
	}

	public void OnEquipAwakeItem(int itemID)
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(itemID);
		if (info == null)
		{
			Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
			{
				itemID
			});
			return;
		}
		if (info.Value1 > 0)
		{
			this.HandleAttMod(0, 2, info.Value1, true);
		}
		if (info.Value2 > 0)
		{
			this.HandleAttMod(0, 3, info.Value2, true);
			this.HandleAttMod(0, 4, info.Value2, true);
		}
		if (info.Value3 > 0)
		{
			this.HandleAttMod(0, 1, info.Value3, true);
		}
	}

	public int GetCombatValue()
	{
		if (this.petData == null)
		{
			return 0;
		}
		int num = (int)((long)(this.GetAtt(1) * 8 / 100) + (long)this.GetAtt(2) * (long)(10000 + this.GetAtt(5) + this.GetAtt(7) * 150 / 100 + this.GetAtt(9)) / 10000L + (long)(this.GetAtt(3) + this.GetAtt(4)) * (long)(10000 + this.GetAtt(6) + this.GetAtt(8) + this.GetAtt(10)) * 163L / 1000000L);
		if (!this.IsPlayer())
		{
			SkillInfo skillInfo = this.petData.GetPlayerSkillInfo();
			if (skillInfo != null)
			{
				num += skillInfo.CombatValue;
			}
			else
			{
				Debug.LogErrorFormat("GetPlayerSkillInfo error, petID = {0}", new object[]
				{
					this.petData.Info.ID
				});
			}
			for (int i = 1; i < this.petData.Info.SkillID.Count; i++)
			{
				skillInfo = this.petData.GetSkillInfo(i);
				if (skillInfo != null)
				{
					if (i != 2 || this.petData.Data.Further >= 3u)
					{
						if (i != 3 || this.petData.Data.Further >= 4u)
						{
							num += skillInfo.CombatValue;
						}
					}
				}
			}
		}
		return num;
	}

	public bool GetEquipMasterState()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i >= this.equips.Length)
			{
				break;
			}
			if (this.equips[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public bool GetTrinketMasterState()
	{
		for (int i = 4; i < 6; i++)
		{
			if (i >= this.equips.Length)
			{
				break;
			}
			if (this.equips[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public List<int> GetRelationEquip(int slot)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.petData.Info.RelationID.Count; i++)
		{
			if (this.petData.Info.RelationID[i] != 0)
			{
				RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(this.petData.Info.RelationID[i]);
				if (info == null)
				{
					Debug.LogErrorFormat("RelationDict.GetInfo error, id = {0}", new object[]
					{
						this.petData.Info.RelationID[i]
					});
				}
				else if (info.Type != 0)
				{
					ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(info.ItemID);
					if (info2 == null)
					{
						Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
						{
							info.ItemID
						});
					}
					else if (info2.Type * 4 + info2.SubType == slot)
					{
						list.Add(info.ItemID);
					}
				}
			}
		}
		return list;
	}

	public void HandleCultivateEffect(int attackMod, int physicDefenseMod, int magicDefenseMod, int maxHPMod)
	{
		if (attackMod != 0)
		{
			this.HandleAttMod(0, 2, attackMod, true);
		}
		if (physicDefenseMod != 0)
		{
			this.HandleAttMod(0, 3, physicDefenseMod, true);
		}
		if (magicDefenseMod != 0)
		{
			this.HandleAttMod(0, 4, magicDefenseMod, true);
		}
		if (maxHPMod != 0)
		{
			this.HandleAttMod(0, 1, maxHPMod, true);
		}
	}

	public void HandleLopetEffect()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		LopetDataEx lopet = Globals.Instance.Player.TeamSystem.GetLopet(this.local);
		if (lopet != null)
		{
			lopet.GetAttribute(ref num4, ref num, ref num2, ref num3);
		}
		if (this.lopetAttack != num)
		{
			this.HandleAttMod(0, 2, num - this.lopetAttack, true);
			this.lopetAttack = num;
		}
		if (this.lopetPhysicDefense != num2)
		{
			this.HandleAttMod(0, 3, num2 - this.lopetPhysicDefense, true);
			this.lopetPhysicDefense = num2;
		}
		if (this.lopetMagicDefense != num3)
		{
			this.HandleAttMod(0, 4, num3 - this.lopetMagicDefense, true);
			this.lopetMagicDefense = num3;
		}
		if (this.lopetMaxHP != num4)
		{
			this.HandleAttMod(0, 1, num4 - this.lopetMaxHP, true);
			this.lopetMaxHP = num4;
		}
	}
}
