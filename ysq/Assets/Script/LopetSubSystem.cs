using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class LopetSubSystem : ISubSystem
{
	public delegate void AddLopetCallback(LopetDataEx data);

	public delegate void RemoveLopetCallback(ulong id);

	public delegate void UpdateLopetCallback(LopetDataEx data);

	public delegate void VoidCallback();

	public LopetSubSystem.VoidCallback SetCombatLopetEvent;

	public LopetSubSystem.AddLopetCallback AddLopetEvent;

	public LopetSubSystem.RemoveLopetCallback RemoveLopetEvent;

	public LopetSubSystem.UpdateLopetCallback LevelupLopetEvent;

	public LopetSubSystem.UpdateLopetCallback AwakeLopetEvent;

	public LopetSubSystem.VoidCallback BreakupLopetEvent;

	public LopetSubSystem.VoidCallback RebornLopetEvent;

	private ulong combatLopetID;

	private Dictionary<ulong, LopetDataEx> lopets = new Dictionary<ulong, LopetDataEx>();

	public uint Version
	{
		get;
		private set;
	}

	public ICollection<LopetDataEx> Values
	{
		get
		{
			return this.lopets.Values;
		}
	}

	public LopetSubSystem()
	{
		this.Version = 0u;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(1061, new ClientSession.MsgHandler(this.OnMsgLopetSetCombat));
		Globals.Instance.CliSession.Register(1063, new ClientSession.MsgHandler(this.OnMsgLopetLevelup));
		Globals.Instance.CliSession.Register(1065, new ClientSession.MsgHandler(this.OnMsgLopetAwake));
		Globals.Instance.CliSession.Register(1071, new ClientSession.MsgHandler(this.OnMsgAddLopet));
		Globals.Instance.CliSession.Register(1073, new ClientSession.MsgHandler(this.OnMsgLopetUpdate));
		Globals.Instance.CliSession.Register(1075, new ClientSession.MsgHandler(this.OnMsgLopetRemove));
		Globals.Instance.CliSession.Register(1067, new ClientSession.MsgHandler(this.OnMsgLopetBreakUp));
		Globals.Instance.CliSession.Register(1069, new ClientSession.MsgHandler(this.OnMsgLopetReborn));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Version = 0u;
		this.lopets.Clear();
	}

	public void LoadData(uint version, ulong lopetID, List<LopetData> data)
	{
		if (version == 0u || version == this.Version)
		{
			return;
		}
		this.Version = version;
		this.lopets.Clear();
		this.combatLopetID = lopetID;
		for (int i = 0; i < data.Count; i++)
		{
			this.AddLopet(data[i]);
		}
	}

	public LopetDataEx GetLopet(ulong id)
	{
		if (id == 0uL)
		{
			return null;
		}
		LopetDataEx result = null;
		this.lopets.TryGetValue(id, out result);
		return result;
	}

	private void AddLopet(LopetData data)
	{
		LopetInfo info = Globals.Instance.AttDB.LopetDict.GetInfo(data.InfoID);
		if (info == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("LopetDict.GetInfo error, id = {0}", data.InfoID)
			});
			return;
		}
		LopetDataEx lopetDataEx = new LopetDataEx(data, info);
		this.lopets.Add(lopetDataEx.Data.ID, lopetDataEx);
	}

	private void RemoveLopet(ulong id)
	{
		if (this.GetLopet(id) == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetLopet error, ID = {0}", id)
			});
			return;
		}
		this.lopets.Remove(id);
	}

	public bool IsCombatLopet(ulong id)
	{
		return this.combatLopetID == id;
	}

	public LopetDataEx GetCurLopet(bool local = true)
	{
		if (local)
		{
			return this.GetLopet(this.combatLopetID);
		}
		return Globals.Instance.Player.TeamSystem.GetRemoteLopet();
	}

	public bool HasLopet2Change()
	{
		LopetDataEx curLopet = this.GetCurLopet(true);
		if (curLopet == null)
		{
			return this.Values.Count > 0;
		}
		foreach (LopetDataEx current in this.Values)
		{
			if (current.GetID() != curLopet.GetID() && current.Info.Quality > curLopet.Info.Quality)
			{
				return true;
			}
		}
		return false;
	}

	public int GetAwakePetCount(ulong id, int infoID)
	{
		int num = 0;
		foreach (LopetDataEx current in this.Values)
		{
			if (current.Data.InfoID == infoID && current.Data.ID != this.combatLopetID && current.Data.ID != id && current.Data.Awake == 0u && current.Data.Level == 1u && current.Data.Exp == 0u)
			{
				num++;
			}
		}
		return num;
	}

	public void OnMsgLopetSetCombat(MemoryStream stream)
	{
		MS2C_LopetSetCombat mS2C_LopetSetCombat = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetSetCombat), stream) as MS2C_LopetSetCombat;
		if (mS2C_LopetSetCombat.Result != ELopetResult.ELR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("LopetR", (int)mS2C_LopetSetCombat.Result);
			return;
		}
		this.combatLopetID = mS2C_LopetSetCombat.LopetID;
		if (mS2C_LopetSetCombat.Version != 0u)
		{
			this.Version = mS2C_LopetSetCombat.Version;
		}
		Globals.Instance.Player.TeamSystem.OnLopetUpdate();
		if (this.SetCombatLopetEvent != null)
		{
			this.SetCombatLopetEvent();
		}
	}

	public void OnMsgLopetLevelup(MemoryStream stream)
	{
		MS2C_LopetLevelup mS2C_LopetLevelup = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetLevelup), stream) as MS2C_LopetLevelup;
		if (mS2C_LopetLevelup.Result != ELopetResult.ELR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("LopetR", (int)mS2C_LopetLevelup.Result);
			return;
		}
		LopetDataEx lopet = this.GetLopet(mS2C_LopetLevelup.LopetID);
		if (lopet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetLopet error, lopetID = {0}", mS2C_LopetLevelup.LopetID)
			});
			return;
		}
		lopet.Data.Level = mS2C_LopetLevelup.Level;
		lopet.Data.Exp = mS2C_LopetLevelup.Exp;
		if (lopet.Data.ID == this.combatLopetID)
		{
			Globals.Instance.Player.TeamSystem.OnLopetUpdate();
		}
		if (mS2C_LopetLevelup.Version != 0u)
		{
			this.Version = mS2C_LopetLevelup.Version;
		}
		if (this.LevelupLopetEvent != null)
		{
			this.LevelupLopetEvent(lopet);
		}
	}

	public void OnMsgLopetAwake(MemoryStream stream)
	{
		MS2C_LopetAwake mS2C_LopetAwake = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetAwake), stream) as MS2C_LopetAwake;
		if (mS2C_LopetAwake.Result != ELopetResult.ELR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("LopetR", (int)mS2C_LopetAwake.Result);
			return;
		}
		for (int i = 0; i < mS2C_LopetAwake.RemoveLopetID.Count; i++)
		{
			this.RemoveLopet(mS2C_LopetAwake.RemoveLopetID[i]);
		}
		LopetDataEx lopet = this.GetLopet(mS2C_LopetAwake.LopetID);
		if (lopet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetLopet error, lopetID = {0}", mS2C_LopetAwake.LopetID)
			});
			return;
		}
		lopet.Data.Awake = mS2C_LopetAwake.Awake;
		if (lopet.Data.ID == this.combatLopetID)
		{
			Globals.Instance.Player.TeamSystem.OnLopetUpdate();
		}
		if (mS2C_LopetAwake.Version != 0u)
		{
			this.Version = mS2C_LopetAwake.Version;
		}
		if (this.AwakeLopetEvent != null)
		{
			this.AwakeLopetEvent(lopet);
		}
	}

	public void OnMsgAddLopet(MemoryStream stream)
	{
		MS2C_LopetAdd mS2C_LopetAdd = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetAdd), stream) as MS2C_LopetAdd;
		for (int i = 0; i < mS2C_LopetAdd.Data.Count; i++)
		{
			this.AddLopet(mS2C_LopetAdd.Data[i]);
			LopetDataEx lopet = this.GetLopet(mS2C_LopetAdd.Data[i].ID);
			if (lopet == null)
			{
				Debug.LogError(new object[]
				{
					string.Format("GetLopet error, lopetID = {0}", mS2C_LopetAdd.Data[i].ID)
				});
				return;
			}
			if (this.AddLopetEvent != null)
			{
				this.AddLopetEvent(lopet);
			}
		}
		if (mS2C_LopetAdd.Version != 0u)
		{
			this.Version = mS2C_LopetAdd.Version;
		}
	}

	public void OnMsgLopetUpdate(MemoryStream stream)
	{
		MS2C_LopetUpdate mS2C_LopetUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetUpdate), stream) as MS2C_LopetUpdate;
		for (int i = 0; i < mS2C_LopetUpdate.Data.Count; i++)
		{
			LopetDataEx lopet = this.GetLopet(mS2C_LopetUpdate.Data[i].ID);
			if (lopet == null)
			{
				Debug.LogError(new object[]
				{
					string.Format("GetLopet error, lopetID = {0}", mS2C_LopetUpdate.Data[i].ID)
				});
			}
			else
			{
				lopet.Data.Level = mS2C_LopetUpdate.Data[i].Level;
				lopet.Data.Exp = mS2C_LopetUpdate.Data[i].Exp;
				lopet.Data.Awake = mS2C_LopetUpdate.Data[i].Awake;
				if (lopet.Data.ID == this.combatLopetID)
				{
					Globals.Instance.Player.TeamSystem.OnLopetUpdate();
				}
			}
		}
		if (mS2C_LopetUpdate.Version != 0u)
		{
			this.Version = mS2C_LopetUpdate.Version;
		}
	}

	public void OnMsgLopetRemove(MemoryStream stream)
	{
		MS2C_LopetRemove mS2C_LopetRemove = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetRemove), stream) as MS2C_LopetRemove;
		for (int i = 0; i < mS2C_LopetRemove.LopetID.Count; i++)
		{
			this.RemoveLopet(mS2C_LopetRemove.LopetID[i]);
			if (this.RemoveLopetEvent != null)
			{
				this.RemoveLopetEvent(mS2C_LopetRemove.LopetID[i]);
			}
		}
		if (mS2C_LopetRemove.Version != 0u)
		{
			this.Version = mS2C_LopetRemove.Version;
		}
	}

	public void OnMsgLopetBreakUp(MemoryStream stream)
	{
		MS2C_LopetBreakUp mS2C_LopetBreakUp = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetBreakUp), stream) as MS2C_LopetBreakUp;
		if (mS2C_LopetBreakUp.Result != ELopetResult.ELR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("LopetR", (int)mS2C_LopetBreakUp.Result);
			return;
		}
		for (int i = 0; i < mS2C_LopetBreakUp.LopetID.Count; i++)
		{
			this.RemoveLopet(mS2C_LopetBreakUp.LopetID[i]);
		}
		if (mS2C_LopetBreakUp.Version != 0u)
		{
			this.Version = mS2C_LopetBreakUp.Version;
		}
		if (this.BreakupLopetEvent != null)
		{
			this.BreakupLopetEvent();
		}
	}

	public void OnMsgLopetReborn(MemoryStream stream)
	{
		MS2C_LopetReborn mS2C_LopetReborn = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetReborn), stream) as MS2C_LopetReborn;
		if (mS2C_LopetReborn.Result != ELopetResult.ELR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("LopetR", (int)mS2C_LopetReborn.Result);
			return;
		}
		LopetDataEx lopet = this.GetLopet(mS2C_LopetReborn.SrcLopet.ID);
		if (lopet == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetLopet error, lopetID = {0}", mS2C_LopetReborn.SrcLopet.ID)
			});
			return;
		}
		lopet.Data.Level = mS2C_LopetReborn.SrcLopet.Level;
		lopet.Data.Exp = mS2C_LopetReborn.SrcLopet.Exp;
		lopet.Data.Awake = mS2C_LopetReborn.SrcLopet.Awake;
		if (lopet.Data.ID == this.combatLopetID)
		{
			Globals.Instance.Player.TeamSystem.OnLopetUpdate();
		}
		for (int i = 0; i < mS2C_LopetReborn.AddLopet.Count; i++)
		{
			this.AddLopet(mS2C_LopetReborn.AddLopet[i]);
		}
		if (mS2C_LopetReborn.Version != 0u)
		{
			this.Version = mS2C_LopetReborn.Version;
		}
		if (this.RebornLopetEvent != null)
		{
			this.RebornLopetEvent();
		}
	}

	public bool HasLopet2Break()
	{
		foreach (LopetDataEx current in this.Values)
		{
			if (!current.IsBattling())
			{
				return true;
			}
		}
		return false;
	}

	public bool HasLopet2Reborn()
	{
		foreach (LopetDataEx current in this.Values)
		{
			if (!current.IsBattling() && current.IsOld())
			{
				return true;
			}
		}
		return false;
	}
}
