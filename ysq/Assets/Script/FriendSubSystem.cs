using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class FriendSubSystem : ISubSystem
{
	public delegate void FriendDataCallback(FriendData data);

	public delegate void FriendIDCallback(ulong id);

	public FriendSubSystem.FriendDataCallback AddFriendDataEvent;

	public FriendSubSystem.FriendIDCallback UpdateFriendEvent;

	public FriendSubSystem.FriendIDCallback RemoveFriendEvent;

	public List<FriendData> friends = new List<FriendData>();

	public List<FriendData> blackList = new List<FriendData>();

	public List<FriendData> applyList = new List<FriendData>();

	public int PendingGiveFriendEnergy
	{
		get;
		private set;
	}

	public int PendingGetFriendEnergy
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(304, new ClientSession.MsgHandler(this.OnMsgAddFriendData));
		Globals.Instance.CliSession.Register(305, new ClientSession.MsgHandler(this.OnMsgRemoveFriendData));
		Globals.Instance.CliSession.Register(306, new ClientSession.MsgHandler(this.OnMsgUpdateFriendData));
		Globals.Instance.CliSession.Register(323, new ClientSession.MsgHandler(this.OnMsgUpdateEnergyFlag));
		Globals.Instance.CliSession.Register(320, new ClientSession.MsgHandler(this.OnMsgGiveFriendEnergy));
		Globals.Instance.CliSession.Register(322, new ClientSession.MsgHandler(this.OnMsgTakeFriendEnergy));
		Globals.Instance.CliSession.Register(310, new ClientSession.MsgHandler(this.OnMsgRequestFriend));
		Globals.Instance.CliSession.Register(312, new ClientSession.MsgHandler(this.OnMsgReplyFriend));
		Globals.Instance.CliSession.Register(314, new ClientSession.MsgHandler(this.OnMsgRemoveFriend));
		Globals.Instance.CliSession.Register(316, new ClientSession.MsgHandler(this.OnMsgAddBlackList));
		Globals.Instance.CliSession.Register(318, new ClientSession.MsgHandler(this.OnMsgRemoveBlackList));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.PendingGiveFriendEnergy = 0;
		this.PendingGetFriendEnergy = 0;
		this.friends.Clear();
		this.blackList.Clear();
		this.applyList.Clear();
		FriendRecommendLayer.ClearRecommendFriends();
	}

	public void LoadData(List<FriendData> data)
	{
		this.Destroy();
		for (int i = 0; i < data.Count; i++)
		{
			this.AddFriendData(data[i]);
		}
		this.UpdateFriendEnergyCount();
	}

	public FriendData GetFriend(ulong id)
	{
		for (int i = 0; i < this.friends.Count; i++)
		{
			if (this.friends[i].GUID == id)
			{
				return this.friends[i];
			}
		}
		return null;
	}

	public FriendData GetBlack(ulong id)
	{
		for (int i = 0; i < this.blackList.Count; i++)
		{
			if (this.blackList[i].GUID == id)
			{
				return this.blackList[i];
			}
		}
		return null;
	}

	public FriendData GetApply(ulong id)
	{
		for (int i = 0; i < this.applyList.Count; i++)
		{
			if (this.applyList[i].GUID == id)
			{
				return this.applyList[i];
			}
		}
		return null;
	}

	public FriendData GetFriendData(ulong id)
	{
		for (int i = 0; i < this.friends.Count; i++)
		{
			if (this.friends[i].GUID == id)
			{
				return this.friends[i];
			}
		}
		for (int j = 0; j < this.blackList.Count; j++)
		{
			if (this.blackList[j].GUID == id)
			{
				return this.blackList[j];
			}
		}
		for (int k = 0; k < this.applyList.Count; k++)
		{
			if (this.applyList[k].GUID == id)
			{
				return this.applyList[k];
			}
		}
		return null;
	}

	public bool IsFriend(ulong id)
	{
		return this.GetFriend(id) != null;
	}

	public bool RedMark()
	{
		return (this.applyList.Count > 0 && this.friends.Count < GameConst.GetInt32(194)) || this.PendingGiveFriendEnergy > 0 || (Globals.Instance.Player.Data.TakeFriendEnergy < GameConst.GetInt32(127) && this.PendingGetFriendEnergy > 0);
	}

	private void UpdateFriendEnergyCount()
	{
		this.PendingGiveFriendEnergy = 0;
		this.PendingGetFriendEnergy = 0;
		for (int i = 0; i < this.friends.Count; i++)
		{
			if (this.friends[i] != null)
			{
				if ((this.friends[i].Flag & 1) != 0)
				{
					this.PendingGiveFriendEnergy++;
				}
				if ((this.friends[i].Flag & 2) != 0 && (this.friends[i].Flag & 4) == 0)
				{
					this.PendingGetFriendEnergy++;
				}
			}
		}
		this.PendingGiveFriendEnergy = this.friends.Count - this.PendingGiveFriendEnergy;
	}

	private void AddFriendData(FriendData data)
	{
		switch (data.FriendType)
		{
		case 1:
			this.friends.Add(data);
			break;
		case 2:
			this.blackList.Add(data);
			break;
		case 3:
			this.applyList.Add(data);
			break;
		default:
			Debug.LogErrorFormat("FriendType error, type = {0}", new object[]
			{
				data.FriendType
			});
			break;
		}
	}

	private void RemoveFriendData(ulong id, int friendType)
	{
		switch (friendType)
		{
		case 1:
		{
			int i = 0;
			while (i < this.friends.Count)
			{
				if (this.friends[i].GUID == id)
				{
					this.friends.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			break;
		}
		case 2:
		{
			int j = 0;
			while (j < this.blackList.Count)
			{
				if (this.blackList[j].GUID == id)
				{
					this.blackList.RemoveAt(j);
				}
				else
				{
					j++;
				}
			}
			break;
		}
		case 3:
		{
			int k = 0;
			while (k < this.applyList.Count)
			{
				if (this.applyList[k].GUID == id)
				{
					this.applyList.RemoveAt(k);
				}
				else
				{
					k++;
				}
			}
			break;
		}
		default:
			Debug.LogErrorFormat("FriendType error, type = {0}", new object[]
			{
				friendType
			});
			break;
		}
	}

	public void OnMsgAddFriendData(MemoryStream stream)
	{
		MS2C_AddFriendData mS2C_AddFriendData = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddFriendData), stream) as MS2C_AddFriendData;
		this.AddFriendData(mS2C_AddFriendData.Data);
		this.UpdateFriendEnergyCount();
		if (this.AddFriendDataEvent != null)
		{
			this.AddFriendDataEvent(mS2C_AddFriendData.Data);
		}
	}

	public void OnMsgRemoveFriendData(MemoryStream stream)
	{
		MS2C_RemoveFriendData mS2C_RemoveFriendData = Serializer.NonGeneric.Deserialize(typeof(MS2C_RemoveFriendData), stream) as MS2C_RemoveFriendData;
		this.RemoveFriendData(mS2C_RemoveFriendData.GUID, mS2C_RemoveFriendData.FriendType);
		this.UpdateFriendEnergyCount();
		if (this.RemoveFriendEvent != null)
		{
			this.RemoveFriendEvent(mS2C_RemoveFriendData.GUID);
		}
	}

	public void OnMsgUpdateFriendData(MemoryStream stream)
	{
		MS2C_UpdateFriendData mS2C_UpdateFriendData = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateFriendData), stream) as MS2C_UpdateFriendData;
		FriendData friendData = this.GetFriendData(mS2C_UpdateFriendData.Data.GUID);
		if (friendData == null)
		{
			return;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 1) != 0)
		{
			friendData.Name = mS2C_UpdateFriendData.Data.Name;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 2) != 0)
		{
			friendData.Level = mS2C_UpdateFriendData.Data.Level;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 4) != 0)
		{
			friendData.VipLevel = mS2C_UpdateFriendData.Data.VipLevel;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 8) != 0)
		{
			friendData.ConLevel = mS2C_UpdateFriendData.Data.ConLevel;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 16) != 0)
		{
			friendData.FashionID = mS2C_UpdateFriendData.Data.FashionID;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 32) != 0)
		{
			friendData.GuildName = mS2C_UpdateFriendData.Data.GuildName;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 64) != 0)
		{
			friendData.CombatValue = mS2C_UpdateFriendData.Data.CombatValue;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 128) != 0)
		{
			friendData.Offline = 0;
		}
		if ((mS2C_UpdateFriendData.UpdateFlag & 256) != 0)
		{
			friendData.Offline = mS2C_UpdateFriendData.Data.Offline;
		}
		if (this.UpdateFriendEvent != null)
		{
			this.UpdateFriendEvent(mS2C_UpdateFriendData.Data.GUID);
		}
	}

	public void OnMsgUpdateEnergyFlag(MemoryStream stream)
	{
		MS2C_UpdateEnergyFlag mS2C_UpdateEnergyFlag = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateEnergyFlag), stream) as MS2C_UpdateEnergyFlag;
		if (mS2C_UpdateEnergyFlag.GUID != 0uL)
		{
			FriendData friend = this.GetFriend(mS2C_UpdateEnergyFlag.GUID);
			if (friend == null)
			{
				return;
			}
			if (mS2C_UpdateEnergyFlag.Flag == 0)
			{
				friend.Flag = 0;
			}
			else
			{
				friend.Flag |= mS2C_UpdateEnergyFlag.Flag;
			}
		}
		else
		{
			for (int i = 0; i < this.friends.Count; i++)
			{
				if (mS2C_UpdateEnergyFlag.Flag == 0)
				{
					this.friends[i].Flag = 0;
				}
				else if (mS2C_UpdateEnergyFlag.Flag == 1 || (mS2C_UpdateEnergyFlag.Flag == 4 && (this.friends[i].Flag & 2) != 0))
				{
					this.friends[i].Flag |= mS2C_UpdateEnergyFlag.Flag;
				}
			}
		}
		this.UpdateFriendEnergyCount();
		if (this.UpdateFriendEvent != null)
		{
			this.UpdateFriendEvent(mS2C_UpdateEnergyFlag.GUID);
		}
	}

	public void OnMsgGiveFriendEnergy(MemoryStream stream)
	{
		MS2C_GiveFriendEnergy mS2C_GiveFriendEnergy = Serializer.NonGeneric.Deserialize(typeof(MS2C_GiveFriendEnergy), stream) as MS2C_GiveFriendEnergy;
		if (mS2C_GiveFriendEnergy.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_GiveFriendEnergy.Result);
			return;
		}
	}

	public void OnMsgTakeFriendEnergy(MemoryStream stream)
	{
		MS2C_TakeFriendEnergy mS2C_TakeFriendEnergy = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeFriendEnergy), stream) as MS2C_TakeFriendEnergy;
		if (mS2C_TakeFriendEnergy.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeFriendEnergy.Result);
			return;
		}
	}

	public void OnMsgRequestFriend(MemoryStream stream)
	{
		MS2C_RequestFriend mS2C_RequestFriend = Serializer.NonGeneric.Deserialize(typeof(MS2C_RequestFriend), stream) as MS2C_RequestFriend;
		if (mS2C_RequestFriend.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_RequestFriend.Result);
			return;
		}
		GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_RequestFriend.Result);
	}

	public void OnMsgReplyFriend(MemoryStream stream)
	{
		MS2C_ReplyFriend mS2C_ReplyFriend = Serializer.NonGeneric.Deserialize(typeof(MS2C_ReplyFriend), stream) as MS2C_ReplyFriend;
		if (mS2C_ReplyFriend.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_ReplyFriend.Result);
			return;
		}
	}

	public void OnMsgRemoveFriend(MemoryStream stream)
	{
		MS2C_RemoveFriend mS2C_RemoveFriend = Serializer.NonGeneric.Deserialize(typeof(MS2C_RemoveFriend), stream) as MS2C_RemoveFriend;
		if (mS2C_RemoveFriend.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_RemoveFriend.Result);
			return;
		}
	}

	public void OnMsgAddBlackList(MemoryStream stream)
	{
		MS2C_AddBlackList mS2C_AddBlackList = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddBlackList), stream) as MS2C_AddBlackList;
		if (mS2C_AddBlackList.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_AddBlackList.Result);
			return;
		}
	}

	public void OnMsgRemoveBlackList(MemoryStream stream)
	{
		MS2C_RemoveBlackList mS2C_RemoveBlackList = Serializer.NonGeneric.Deserialize(typeof(MS2C_RemoveBlackList), stream) as MS2C_RemoveBlackList;
		if (mS2C_RemoveBlackList.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_RemoveBlackList.Result);
			return;
		}
	}

	public void SendRequestFriend(ulong id, string name)
	{
		if (this.IsFriend(id))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("friend_18", new object[]
			{
				name
			}), 0f, 0f);
			return;
		}
		MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
		mC2S_RequestFriend.GUID = id;
		Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
	}
}
