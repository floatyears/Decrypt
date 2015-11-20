using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class PvpSubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public PvpSubSystem.VoidCallback QueryArenaDataEvent;

	public PvpSubSystem.VoidCallback QueryArenaRankEvent;

	public PvpSubSystem.VoidCallback QueryPvpRecordEvent;

	public PvpSubSystem.VoidCallback QueryPillageTargetEvent;

	public PvpSubSystem.VoidCallback QueryPillageRecordEvent;

	private RemotePlayer targetData;

	public int Rank
	{
		get;
		private set;
	}

	public uint ArenaRankVersion
	{
		get;
		private set;
	}

	public List<RankData> ArenaRank
	{
		get;
		private set;
	}

	public uint ArenaRecordVersion
	{
		get;
		private set;
	}

	public List<PvpRecord> ArenaRecord
	{
		get;
		private set;
	}

	public List<RankData> ArenaTargets
	{
		get;
		private set;
	}

	public List<RankData> PillageTargets
	{
		get;
		private set;
	}

	public uint PillageRecordVersion
	{
		get;
		private set;
	}

	public List<PillageRecord> PillageRecord
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(802, new ClientSession.MsgHandler(this.OnMsgQueryArenaData));
		Globals.Instance.CliSession.Register(804, new ClientSession.MsgHandler(this.OnMsgPvpArenaStart));
		Globals.Instance.CliSession.Register(809, new ClientSession.MsgHandler(this.OnMsgQueryArenaRank));
		Globals.Instance.CliSession.Register(811, new ClientSession.MsgHandler(this.OnMsgQueryPvpRecord));
		Globals.Instance.CliSession.Register(815, new ClientSession.MsgHandler(this.OnMsgQueryPillageTarget));
		Globals.Instance.CliSession.Register(817, new ClientSession.MsgHandler(this.OnMsgPvpPillageStart));
		Globals.Instance.CliSession.Register(821, new ClientSession.MsgHandler(this.OnMsgQueryPillageRecord));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Rank = 0;
		this.ArenaRankVersion = 0u;
		this.ArenaRank = null;
		this.ArenaRecordVersion = 0u;
		this.ArenaRecord = null;
		this.ArenaTargets = null;
		this.PillageTargets = null;
		this.PillageRecordVersion = 0u;
		this.PillageRecord = null;
	}

	public void SetArenaTargetID(ulong id)
	{
		if (this.ArenaTargets == null)
		{
			return;
		}
		for (int i = 0; i < this.ArenaTargets.Count; i++)
		{
			if (this.ArenaTargets[i].Data.GUID == id)
			{
				this.targetData = this.ArenaTargets[i].Data;
				return;
			}
		}
	}

	public bool ShowSelfRank()
	{
		return (Globals.Instance.Player.Data.DataFlag & 64) != 0;
	}

	public void ClearArenaTargets()
	{
		this.ArenaTargets = null;
	}

	public void SetPillageTargetID(ulong id)
	{
		this.targetData = null;
		if (this.PillageTargets == null)
		{
			return;
		}
		for (int i = 0; i < this.PillageTargets.Count; i++)
		{
			if (this.PillageTargets[i].Data.GUID == id)
			{
				this.targetData = this.PillageTargets[i].Data;
				return;
			}
		}
	}

	public void ClearPillageTargets()
	{
		this.PillageTargets = null;
	}

	public void OnMsgQueryArenaData(MemoryStream stream)
	{
		MS2C_QueryArenaData mS2C_QueryArenaData = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryArenaData), stream) as MS2C_QueryArenaData;
		if (mS2C_QueryArenaData.Result == 34)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		this.Rank = mS2C_QueryArenaData.Rank;
		if (player.Data.ArenaHighestRank == 0)
		{
			player.Data.ArenaHighestRank = mS2C_QueryArenaData.Rank;
		}
		this.ArenaTargets = mS2C_QueryArenaData.Opponents;
		if (this.QueryArenaDataEvent != null)
		{
			this.QueryArenaDataEvent();
		}
	}

	public void OnMsgPvpArenaStart(MemoryStream stream)
	{
		MS2C_PvpArenaStart mS2C_PvpArenaStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpArenaStart), stream) as MS2C_PvpArenaStart;
		if (mS2C_PvpArenaStart.Result == 34)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_PvpArenaStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpArenaStart.Result);
			return;
		}
		if (this.targetData == null)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpArenaStart.Result);
			return;
		}
		GameAnalytics.OnStartScene(Globals.Instance.AttDB.SceneDict.GetInfo(GameConst.GetInt32(105)));
		Globals.Instance.ActorMgr.SetServerData(mS2C_PvpArenaStart.Key, mS2C_PvpArenaStart.Data2);
		Globals.Instance.Player.TeamSystem.SetRemotePlayerData(this.targetData, mS2C_PvpArenaStart.Data);
		GameUIManager.mInstance.LoadScene(GameConst.GetInt32(105));
	}

	public void OnMsgQueryArenaRank(MemoryStream stream)
	{
		MS2C_QueryArenaRank mS2C_QueryArenaRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryArenaRank), stream) as MS2C_QueryArenaRank;
		if (this.ArenaRankVersion == 0u || this.ArenaRankVersion != mS2C_QueryArenaRank.RankVersion)
		{
			this.ArenaRankVersion = mS2C_QueryArenaRank.RankVersion;
			this.ArenaRank = mS2C_QueryArenaRank.Data;
		}
		if (this.QueryArenaRankEvent != null)
		{
			this.QueryArenaRankEvent();
		}
	}

	public void OnMsgQueryPvpRecord(MemoryStream stream)
	{
		MS2C_QueryPvpRecord mS2C_QueryPvpRecord = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryPvpRecord), stream) as MS2C_QueryPvpRecord;
		if (this.ArenaRecordVersion == 0u || this.ArenaRecordVersion != mS2C_QueryPvpRecord.RecordVersion)
		{
			this.ArenaRecordVersion = mS2C_QueryPvpRecord.RecordVersion;
			this.ArenaRecord = mS2C_QueryPvpRecord.Data;
		}
		if (this.QueryPvpRecordEvent != null)
		{
			this.QueryPvpRecordEvent();
		}
	}

	public void OnMsgQueryPillageTarget(MemoryStream stream)
	{
		MS2C_QueryPillageTarget mS2C_QueryPillageTarget = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryPillageTarget), stream) as MS2C_QueryPillageTarget;
		if (mS2C_QueryPillageTarget.Result == 34)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		this.PillageTargets = mS2C_QueryPillageTarget.Targets;
		if (this.QueryPillageTargetEvent != null)
		{
			this.QueryPillageTargetEvent();
		}
	}

	public void OnMsgPvpPillageStart(MemoryStream stream)
	{
		MS2C_PvpPillageStart mS2C_PvpPillageStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpPillageStart), stream) as MS2C_PvpPillageStart;
		if (mS2C_PvpPillageStart.Result == 34)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_PvpPillageStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpPillageStart.Result);
			return;
		}
		if (this.targetData == null)
		{
			if (mS2C_PvpPillageStart.Data2 == null || mS2C_PvpPillageStart.Data2.GUID == 0uL)
			{
				GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpPillageStart.Result);
				return;
			}
			this.targetData = mS2C_PvpPillageStart.Data2;
		}
		GameAnalytics.OnStartScene(Globals.Instance.AttDB.SceneDict.GetInfo(GameConst.GetInt32(105)));
		Globals.Instance.ActorMgr.SetServerData(mS2C_PvpPillageStart.Key, mS2C_PvpPillageStart.Data3);
		Globals.Instance.Player.TeamSystem.SetRemotePlayerData(this.targetData, mS2C_PvpPillageStart.Data);
		GameUIManager.mInstance.LoadScene(GameConst.GetInt32(106));
	}

	public void OnMsgQueryPillageRecord(MemoryStream stream)
	{
		MS2C_QueryPillageRecord mS2C_QueryPillageRecord = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryPillageRecord), stream) as MS2C_QueryPillageRecord;
		if (this.PillageRecordVersion == 0u || this.PillageRecordVersion != mS2C_QueryPillageRecord.RecordVersion)
		{
			this.PillageRecordVersion = mS2C_QueryPillageRecord.RecordVersion;
			this.PillageRecord = mS2C_QueryPillageRecord.Data;
		}
		if (this.QueryPillageRecordEvent != null)
		{
			this.QueryPillageRecordEvent();
		}
	}
}
