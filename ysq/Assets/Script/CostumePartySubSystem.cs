using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class CostumePartySubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public delegate void OneUlongParmCallback(ulong playerID);

	public CostumePartySubSystem.VoidCallback GetCostumePartyDataEvent;

	public CostumePartySubSystem.VoidCallback CreateCostumePartyEvent;

	public CostumePartySubSystem.VoidCallback JoinCostumePartyEvent;

	public CostumePartySubSystem.VoidCallback UpdateCostumePartyDataEvent;

	public CostumePartySubSystem.OneUlongParmCallback RemoveGuestEvent;

	public CostumePartySubSystem.OneUlongParmCallback StartCarnivalEvent;

	public CostumePartySubSystem.OneUlongParmCallback WandEvent;

	public CostumePartySubSystem.OneUlongParmCallback TakeCostumePartyRewardEvent;

	public bool FinishInit;

	public uint PartyID;

	public ulong MasterID;

	public List<CostumePartyGuest> Guests = new List<CostumePartyGuest>();

	public int TimeStamp;

	public List<int> CD = new List<int>();

	public bool HasInteractionReward;

	public List<MS2C_InteractionMessage> InteractionMsgs = new List<MS2C_InteractionMessage>();

	public ECarnivalType CarnivalType;

	public int Count;

	public void Init()
	{
		Globals.Instance.CliSession.Register(263, new ClientSession.MsgHandler(this.OnMsgGetCostumePartyData));
		Globals.Instance.CliSession.Register(261, new ClientSession.MsgHandler(this.OnMsgUpdateCostumePartyData));
		Globals.Instance.CliSession.Register(265, new ClientSession.MsgHandler(this.OnMsgCreateCostumeParty));
		Globals.Instance.CliSession.Register(267, new ClientSession.MsgHandler(this.OnMsgJoinCostumeParty));
		Globals.Instance.CliSession.Register(271, new ClientSession.MsgHandler(this.OnMsgRemoveGuest));
		Globals.Instance.CliSession.Register(280, new ClientSession.MsgHandler(this.OnMsgUpdateCostumePartyGuest));
	}

	public void OnMsgUpdateCostumePartyGuest(MemoryStream stream)
	{
		MS2C_UpdateCostumePartyGuest mS2C_UpdateCostumePartyGuest = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateCostumePartyGuest), stream) as MS2C_UpdateCostumePartyGuest;
		CostumePartyGuest costumePartyGuest = null;
		foreach (CostumePartyGuest current in this.Guests)
		{
			if (current.PlayerID == mS2C_UpdateCostumePartyGuest.PlayerID)
			{
				costumePartyGuest = current;
				break;
			}
		}
		if (costumePartyGuest == null)
		{
			return;
		}
		bool flag = costumePartyGuest.PlayerID == Globals.Instance.Player.Data.ID;
		if (mS2C_UpdateCostumePartyGuest.Timestamp != 0)
		{
			if (mS2C_UpdateCostumePartyGuest.Timestamp == -1)
			{
				costumePartyGuest.Timestamp = 0;
				if (flag)
				{
					this.TimeStamp = 0;
				}
			}
			else
			{
				costumePartyGuest.Timestamp = mS2C_UpdateCostumePartyGuest.Timestamp;
				if (flag)
				{
					this.TimeStamp = mS2C_UpdateCostumePartyGuest.Timestamp;
				}
			}
		}
		if (mS2C_UpdateCostumePartyGuest.Timestamp == -1)
		{
			if (this.TakeCostumePartyRewardEvent != null)
			{
				this.TakeCostumePartyRewardEvent(mS2C_UpdateCostumePartyGuest.PlayerID);
			}
		}
		else if (mS2C_UpdateCostumePartyGuest.Timestamp > Globals.Instance.Player.GetTimeStamp() && this.StartCarnivalEvent != null)
		{
			this.StartCarnivalEvent(mS2C_UpdateCostumePartyGuest.PlayerID);
		}
		if (mS2C_UpdateCostumePartyGuest.PetTimestamp != 0)
		{
			if (mS2C_UpdateCostumePartyGuest.PetTimestamp == -1)
			{
				costumePartyGuest.PetTimestamp = 0;
			}
			else
			{
				costumePartyGuest.PetTimestamp = mS2C_UpdateCostumePartyGuest.PetTimestamp;
			}
			if (mS2C_UpdateCostumePartyGuest.PetID != 0)
			{
				if (mS2C_UpdateCostumePartyGuest.PetID == -1)
				{
					costumePartyGuest.PetID = 0;
				}
				else
				{
					costumePartyGuest.PetID = mS2C_UpdateCostumePartyGuest.PetID;
				}
				if (this.WandEvent != null)
				{
					this.WandEvent(mS2C_UpdateCostumePartyGuest.PlayerID);
				}
			}
		}
	}

	public int AddGuest(CostumePartyGuest data, int slot)
	{
		if (this.IsPartyFull())
		{
			Debug.Log(new object[]
			{
				"party is full"
			});
			return -1;
		}
		if (data.PlayerID == 0uL)
		{
			Debug.LogError(new object[]
			{
				"CostumePartySystem Guests Error"
			});
			return -1;
		}
		switch (slot)
		{
		case 0:
			this.Guests[2] = data;
			return 2;
		case 1:
			this.Guests[3] = data;
			return 3;
		case 2:
			this.Guests[1] = data;
			return 1;
		case 3:
			this.Guests[4] = data;
			return 4;
		case 4:
			this.Guests[0] = data;
			return 0;
		case 5:
			this.Guests[5] = data;
			return 5;
		default:
			Debug.LogError(new object[]
			{
				string.Format("CostumeParty System AddGuest Error , slot : {0} ", slot)
			});
			return -1;
		}
	}

	private void UpdateData(CostumePartyData data)
	{
		if (data.RoomData != null && data.RoomData.ID > 0u)
		{
			this.PartyID = data.RoomData.ID;
			this.MasterID = data.RoomData.MasterID;
			this.Guests.Clear();
			this.Guests.Add(data.RoomData.Data[4]);
			this.Guests.Add(data.RoomData.Data[2]);
			this.Guests.Add(data.RoomData.Data[0]);
			this.Guests.Add(data.RoomData.Data[1]);
			this.Guests.Add(data.RoomData.Data[3]);
			this.Guests.Add(data.RoomData.Data[5]);
			this.InteractionMsgs = data.RoomData.msg;
			foreach (CostumePartyGuest current in this.Guests)
			{
				if (current.PlayerID == Globals.Instance.Player.Data.ID)
				{
					this.TimeStamp = current.Timestamp;
				}
			}
			this.CD = data.CD;
			this.Count = data.Count;
			this.HasInteractionReward = data.HasReward;
			this.CarnivalType = (ECarnivalType)data.CarnivalType;
		}
		else
		{
			this.PartyID = 0u;
			this.MasterID = 0uL;
			this.Guests.Clear();
			this.TimeStamp = 0;
			this.HasInteractionReward = false;
			this.InteractionMsgs.Clear();
		}
	}

	private void OnMsgUpdateCostumePartyData(MemoryStream stream)
	{
		MS2C_UpdateCostumePartyData mS2C_UpdateCostumePartyData = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateCostumePartyData), stream) as MS2C_UpdateCostumePartyData;
		if (mS2C_UpdateCostumePartyData.CDType != 0)
		{
			this.CD[mS2C_UpdateCostumePartyData.CDType - 1] = mS2C_UpdateCostumePartyData.CD;
		}
		if (mS2C_UpdateCostumePartyData.HasReward != 0)
		{
			this.HasInteractionReward = (mS2C_UpdateCostumePartyData.HasReward > 0);
		}
		if (mS2C_UpdateCostumePartyData.CarnivalType != 0)
		{
			this.CarnivalType = (ECarnivalType)mS2C_UpdateCostumePartyData.CarnivalType;
		}
		if (mS2C_UpdateCostumePartyData.Count != 0)
		{
			if (mS2C_UpdateCostumePartyData.Count == -1)
			{
				this.Count = 0;
			}
			else
			{
				this.Count = mS2C_UpdateCostumePartyData.Count;
			}
		}
		if (this.UpdateCostumePartyDataEvent != null)
		{
			this.UpdateCostumePartyDataEvent();
		}
	}

	private void OnMsgGetCostumePartyData(MemoryStream stream)
	{
		MS2C_GetCostumePartyData mS2C_GetCostumePartyData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetCostumePartyData), stream) as MS2C_GetCostumePartyData;
		if (mS2C_GetCostumePartyData.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_GetCostumePartyData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_GetCostumePartyData.Result);
			return;
		}
		if (!this.FinishInit)
		{
			this.FinishInit = true;
		}
		if (mS2C_GetCostumePartyData.Data != null)
		{
			this.UpdateData(mS2C_GetCostumePartyData.Data);
		}
		if (this.GetCostumePartyDataEvent != null)
		{
			this.GetCostumePartyDataEvent();
		}
	}

	private void OnMsgCreateCostumeParty(MemoryStream stream)
	{
		MS2C_CreateCostumeParty mS2C_CreateCostumeParty = Serializer.NonGeneric.Deserialize(typeof(MS2C_CreateCostumeParty), stream) as MS2C_CreateCostumeParty;
		if (mS2C_CreateCostumeParty.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_CreateCostumeParty.Result);
			return;
		}
		if (mS2C_CreateCostumeParty.Data == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetCostumePartyData Error", new object[0])
			});
			return;
		}
		if (mS2C_CreateCostumeParty.Data.RoomData == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetCostumePartyData Error RoomData is Null ", new object[0])
			});
			return;
		}
		this.UpdateData(mS2C_CreateCostumeParty.Data);
		if (this.CreateCostumePartyEvent != null)
		{
			this.CreateCostumePartyEvent();
		}
	}

	private void OnMsgJoinCostumeParty(MemoryStream stream)
	{
		MS2C_JoinCostumeParty mS2C_JoinCostumeParty = Serializer.NonGeneric.Deserialize(typeof(MS2C_JoinCostumeParty), stream) as MS2C_JoinCostumeParty;
		if (mS2C_JoinCostumeParty.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_JoinCostumeParty.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_JoinCostumeParty.Result);
			return;
		}
		if (mS2C_JoinCostumeParty.Data == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetCostumePartyData Error", new object[0])
			});
			return;
		}
		if (mS2C_JoinCostumeParty.Data.RoomData == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetCostumePartyData Error RoomData is Null ", new object[0])
			});
			return;
		}
		this.UpdateData(mS2C_JoinCostumeParty.Data);
		if (this.JoinCostumePartyEvent != null)
		{
			this.JoinCostumePartyEvent();
		}
	}

	private void OnMsgRemoveGuest(MemoryStream stream)
	{
		MS2C_RemoveGuest mS2C_RemoveGuest = Serializer.NonGeneric.Deserialize(typeof(MS2C_RemoveGuest), stream) as MS2C_RemoveGuest;
		if (mS2C_RemoveGuest.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_RemoveGuest.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_RemoveGuest.Result);
			return;
		}
		if (mS2C_RemoveGuest.PlayerID == Globals.Instance.Player.Data.ID)
		{
			this.PartyID = 0u;
		}
		if (mS2C_RemoveGuest.MasterID != 0uL)
		{
			this.MasterID = mS2C_RemoveGuest.MasterID;
		}
		if (this.RemoveGuestEvent != null)
		{
			this.RemoveGuestEvent(mS2C_RemoveGuest.PlayerID);
		}
	}

	public void LeaveParty(ulong playerID)
	{
		foreach (CostumePartyGuest current in this.Guests)
		{
			if (current.PlayerID == playerID)
			{
				current.PlayerID = 0uL;
			}
		}
	}

	public bool IsNew()
	{
		return (this.IsInParty() && (this.CanTakeInteractionRewards() || this.CanTakeRewards() || this.CanCarnival() || this.CanTakeICountReward())) || (Tools.CanPlay(GameConst.GetInt32(10), true) && !this.IsInParty());
	}

	public bool CanTakeInteractionRewards()
	{
		return Tools.CanPlay(GameConst.GetInt32(10), true) && this.HasInteractionReward;
	}

	public bool CanTakeRewards()
	{
		return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(10)) && this.TimeStamp != 0 && Globals.Instance.Player.GetTimeStamp() > this.TimeStamp;
	}

	public bool CanTakeICountReward()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.CanTakeICountReward(i))
			{
				return true;
			}
		}
		return false;
	}

	public bool CanTakeICountReward(int index)
	{
		return this.Count >= Globals.Instance.AttDB.CostumePartyDict.GetInfo(index + 1).Count && (Globals.Instance.Player.Data.DataFlag & 262144 << index) == 0;
	}

	public bool IsCarnival()
	{
		return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(10)) && this.TimeStamp != 0 && Globals.Instance.Player.GetTimeStamp() <= this.TimeStamp;
	}

	public int GetTakeRewardsTime()
	{
		int num = this.TimeStamp - Globals.Instance.Player.GetTimeStamp();
		if (num < 0)
		{
			return 0;
		}
        if (num < (int)((int)this.CarnivalType * GameConst.GetInt32(63)))
		{
			return num;
		}
		return (int)((int)this.CarnivalType * GameConst.GetInt32(63));
	}

	public bool CanCarnival()
	{
		return Tools.CanPlay(GameConst.GetInt32(10), true) && this.TimeStamp == 0;
	}

	public bool IsInParty()
	{
		return this.PartyID > 0u;
	}

	public bool IsMaster()
	{
		return this.MasterID == Globals.Instance.Player.Data.ID;
	}

	public bool IsMaster(ulong playerID)
	{
		return this.MasterID == playerID;
	}

	public bool IsPartyFull()
	{
		if (this.Guests.Count != 6)
		{
			Debug.LogError(new object[]
			{
				string.Format("CostumeParty Guests Count Error ,Count : {0}", this.Guests.Count)
			});
			return false;
		}
		foreach (CostumePartyGuest current in this.Guests)
		{
			if (current == null || current.PlayerID <= 0uL)
			{
				return false;
			}
		}
		return true;
	}

	public bool CanLeave()
	{
		return this.TimeStamp == 0;
	}

	public int GetCD(EInteractionType type)
	{
		return this.CD[type - EInteractionType.EInteraction_Rose];
	}

	public void AddInteractionMsg(MS2C_InteractionMessage msg)
	{
		if (this.InteractionMsgs.Count >= 30)
		{
			this.InteractionMsgs.RemoveAt(0);
		}
		this.InteractionMsgs.Add(msg);
	}

	public void LoadData(uint roomID, int timeStamp, int count, bool hasReward)
	{
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(10)) && !this.FinishInit)
		{
			this.PartyID = roomID;
			this.TimeStamp = timeStamp;
			this.Count = count;
			this.HasInteractionReward = hasReward;
		}
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.FinishInit = false;
		this.PartyID = 0u;
		this.MasterID = 0uL;
		this.Guests.Clear();
		this.TimeStamp = 0;
		this.CD.Clear();
		this.Count = 0;
		this.HasInteractionReward = false;
		this.InteractionMsgs.Clear();
	}
}
