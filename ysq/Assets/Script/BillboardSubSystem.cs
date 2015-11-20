using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public class BillboardSubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public BillboardSubSystem.VoidCallback QueryCombatValueRankEvent;

	public BillboardSubSystem.VoidCallback QueryPVEStarsRankEvent;

	public BillboardSubSystem.VoidCallback QueryLevelRankEvent;

	public BillboardSubSystem.VoidCallback QueryTrialRankEvent;

	public BillboardSubSystem.VoidCallback QueryRoseRankEvent;

	public BillboardSubSystem.VoidCallback QueryTurtleRankEvent;

	public BillboardSubSystem.VoidCallback GetOreRankListEvent;

	public BillboardSubSystem.VoidCallback GetGGOreRankListEvent;

	public BillboardSubSystem.VoidCallback GetGOreRankListEvent;

	public BillboardSubSystem.VoidCallback GuildBossDamageRankDataEvent;

	public List<RankData> CombatValueRankData
	{
		get;
		private set;
	}

	public uint CombatValueRankVersion
	{
		get;
		private set;
	}

	public uint CombatValueRank
	{
		get;
		private set;
	}

	public List<RankData> PVEStarsRankData
	{
		get;
		private set;
	}

	public uint PVEStarsRankVersion
	{
		get;
		private set;
	}

	public uint PVEStarsRank
	{
		get;
		private set;
	}

	public List<RankData> LevelRankData
	{
		get;
		private set;
	}

	public uint LevelRankVersion
	{
		get;
		private set;
	}

	public uint LevelRank
	{
		get;
		private set;
	}

	public List<RankData> TrialRankData
	{
		get;
		private set;
	}

	public uint TrialRankVersion
	{
		get;
		private set;
	}

	public uint TrialRank
	{
		get;
		private set;
	}

	public List<RankData> RoseRankData
	{
		get;
		private set;
	}

	public uint RoseRankVersion
	{
		get;
		private set;
	}

	public uint RoseRank
	{
		get;
		private set;
	}

	public uint RoseValue
	{
		get;
		private set;
	}

	public List<RankData> TurtleRankData
	{
		get;
		private set;
	}

	public uint TurtleRankVersion
	{
		get;
		private set;
	}

	public uint TurtleRank
	{
		get;
		private set;
	}

	public uint TurtleValue
	{
		get;
		private set;
	}

	public List<RankData> OreRankData
	{
		get;
		private set;
	}

	public uint OreRankVersion
	{
		get;
		private set;
	}

	public uint OreRank
	{
		get;
		private set;
	}

	public List<GuildRank> GGOreRankData
	{
		get;
		private set;
	}

	public uint GGOreRankVersion
	{
		get;
		private set;
	}

	public uint GGOreRank
	{
		get;
		private set;
	}

	public List<RankData> GOreRankData
	{
		get;
		private set;
	}

	public uint GOreRankVersion
	{
		get;
		private set;
	}

	public uint GOreRank
	{
		get;
		private set;
	}

	public List<RankData> GuildBossDamageRankData
	{
		get;
		private set;
	}

	public uint GuildBossDamageRankVersion
	{
		get;
		private set;
	}

	public uint GuildBossDamageRank
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(285, new ClientSession.MsgHandler(this.OnMsgQueryCombatValueRank));
		Globals.Instance.CliSession.Register(291, new ClientSession.MsgHandler(this.OnMsgQueryPVEStarsRank));
		Globals.Instance.CliSession.Register(295, new ClientSession.MsgHandler(this.OnMsgQueryLevelRank));
		Globals.Instance.CliSession.Register(607, new ClientSession.MsgHandler(this.OnMsgQueryTrialRank));
		Globals.Instance.CliSession.Register(1495, new ClientSession.MsgHandler(this.OnMsgQueryRoseRank));
		Globals.Instance.CliSession.Register(1497, new ClientSession.MsgHandler(this.OnMsgQueryTurtleRank));
		Globals.Instance.CliSession.Register(1036, new ClientSession.MsgHandler(this.OnMsgGetOreRankList));
		Globals.Instance.CliSession.Register(1038, new ClientSession.MsgHandler(this.OnMsgGetGGOreRankList));
		Globals.Instance.CliSession.Register(1040, new ClientSession.MsgHandler(this.OnMsgGetGOreRankList));
		Globals.Instance.CliSession.Register(970, new ClientSession.MsgHandler(this.OnMsgQueryGuildBossDamageRank));
	}

	private void OnMsgQueryCombatValueRank(MemoryStream stream)
	{
		MS2C_CombatValueRank mS2C_CombatValueRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_CombatValueRank), stream) as MS2C_CombatValueRank;
		if (this.CombatValueRankVersion == 0u || this.CombatValueRankVersion != mS2C_CombatValueRank.Version)
		{
			this.CombatValueRankVersion = mS2C_CombatValueRank.Version;
			this.CombatValueRankData = mS2C_CombatValueRank.Data;
			this.CombatValueRank = mS2C_CombatValueRank.SelfRank;
		}
		if (this.QueryCombatValueRankEvent != null)
		{
			this.QueryCombatValueRankEvent();
		}
	}

	private void OnMsgQueryPVEStarsRank(MemoryStream stream)
	{
		MS2C_PVEStarsRank mS2C_PVEStarsRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_PVEStarsRank), stream) as MS2C_PVEStarsRank;
		if (this.PVEStarsRankVersion == 0u || this.PVEStarsRankVersion != mS2C_PVEStarsRank.Version)
		{
			this.PVEStarsRankVersion = mS2C_PVEStarsRank.Version;
			this.PVEStarsRankData = mS2C_PVEStarsRank.Data;
			this.PVEStarsRank = mS2C_PVEStarsRank.SelfRank;
		}
		if (this.QueryPVEStarsRankEvent != null)
		{
			this.QueryPVEStarsRankEvent();
		}
	}

	private void OnMsgQueryLevelRank(MemoryStream stream)
	{
		MS2C_LevelRank mS2C_LevelRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_LevelRank), stream) as MS2C_LevelRank;
		if (this.LevelRankVersion == 0u || this.LevelRankVersion != mS2C_LevelRank.Version)
		{
			this.LevelRankVersion = mS2C_LevelRank.Version;
			this.LevelRankData = mS2C_LevelRank.Data;
			this.LevelRank = mS2C_LevelRank.SelfRank;
		}
		if (this.QueryLevelRankEvent != null)
		{
			this.QueryLevelRankEvent();
		}
	}

	private void OnMsgQueryTrialRank(MemoryStream stream)
	{
		MS2C_QueryTrialRank mS2C_QueryTrialRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryTrialRank), stream) as MS2C_QueryTrialRank;
		if (this.TrialRankVersion == 0u || this.TrialRankVersion != mS2C_QueryTrialRank.TrialRankVersion)
		{
			this.TrialRankVersion = mS2C_QueryTrialRank.TrialRankVersion;
			this.TrialRankData = mS2C_QueryTrialRank.Data;
			this.TrialRank = mS2C_QueryTrialRank.SelfRank;
		}
		if (this.QueryTrialRankEvent != null)
		{
			this.QueryTrialRankEvent();
		}
	}

	private void OnMsgQueryRoseRank(MemoryStream stream)
	{
		MS2C_QueryRoseRank mS2C_QueryRoseRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryRoseRank), stream) as MS2C_QueryRoseRank;
		if (this.RoseRankVersion == 0u || this.RoseRankVersion != mS2C_QueryRoseRank.Version)
		{
			this.RoseRankVersion = mS2C_QueryRoseRank.Version;
			this.RoseRankData = mS2C_QueryRoseRank.Data;
			this.RoseRank = mS2C_QueryRoseRank.SelfRank;
			this.RoseValue = mS2C_QueryRoseRank.Count;
		}
		if (this.QueryRoseRankEvent != null)
		{
			this.QueryRoseRankEvent();
		}
	}

	private void OnMsgQueryTurtleRank(MemoryStream stream)
	{
		MS2C_QueryTortoiseRank mS2C_QueryTortoiseRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryTortoiseRank), stream) as MS2C_QueryTortoiseRank;
		if (this.TurtleRankVersion == 0u || this.TurtleRankVersion != mS2C_QueryTortoiseRank.Version)
		{
			this.TurtleRankVersion = mS2C_QueryTortoiseRank.Version;
			this.TurtleRankData = mS2C_QueryTortoiseRank.Data;
			this.TurtleRank = mS2C_QueryTortoiseRank.SelfRank;
			this.TurtleValue = mS2C_QueryTortoiseRank.Count;
		}
		if (this.QueryTurtleRankEvent != null)
		{
			this.QueryTurtleRankEvent();
		}
	}

	private void OnMsgGetOreRankList(MemoryStream stream)
	{
		MS2C_GetOreRankList mS2C_GetOreRankList = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetOreRankList), stream) as MS2C_GetOreRankList;
		if (this.OreRankVersion == 0u || this.OreRankVersion != mS2C_GetOreRankList.Version)
		{
			this.OreRankVersion = mS2C_GetOreRankList.Version;
			this.OreRankData = mS2C_GetOreRankList.Data;
			this.OreRank = mS2C_GetOreRankList.SelfRank;
		}
		if (this.GetOreRankListEvent != null)
		{
			this.GetOreRankListEvent();
		}
	}

	private void OnMsgGetGGOreRankList(MemoryStream stream)
	{
		MS2C_GetGGOreRankList mS2C_GetGGOreRankList = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetGGOreRankList), stream) as MS2C_GetGGOreRankList;
		if (mS2C_GetGGOreRankList.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GetGGOreRankList.Result);
			return;
		}
		if (this.GGOreRankVersion == 0u || this.GGOreRankVersion != mS2C_GetGGOreRankList.Version)
		{
			this.GGOreRankVersion = mS2C_GetGGOreRankList.Version;
			this.GGOreRankData = mS2C_GetGGOreRankList.Data;
			this.GGOreRank = mS2C_GetGGOreRankList.Rank;
		}
		if (this.GetGGOreRankListEvent != null)
		{
			this.GetGGOreRankListEvent();
		}
	}

	private void OnMsgGetGOreRankList(MemoryStream stream)
	{
		MS2C_GetGOreRankList mS2C_GetGOreRankList = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetGOreRankList), stream) as MS2C_GetGOreRankList;
		if (mS2C_GetGOreRankList.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GetGOreRankList.Result);
			return;
		}
		if (this.GOreRankVersion == 0u || this.GOreRankVersion != mS2C_GetGOreRankList.Version)
		{
			this.GOreRankVersion = mS2C_GetGOreRankList.Version;
			this.GOreRankData = mS2C_GetGOreRankList.Data;
			this.GOreRank = mS2C_GetGOreRankList.SelfRank;
		}
		if (this.GetGOreRankListEvent != null)
		{
			this.GetGOreRankListEvent();
		}
	}

	private void OnMsgQueryGuildBossDamageRank(MemoryStream stream)
	{
		MS2C_QueryGuildBossDamageRank mS2C_QueryGuildBossDamageRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryGuildBossDamageRank), stream) as MS2C_QueryGuildBossDamageRank;
		if (mS2C_QueryGuildBossDamageRank.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_QueryGuildBossDamageRank.Result);
			return;
		}
		if (this.GuildBossDamageRankVersion == 0u || this.GuildBossDamageRankVersion != mS2C_QueryGuildBossDamageRank.Version)
		{
			this.GuildBossDamageRankVersion = mS2C_QueryGuildBossDamageRank.Version;
			this.GuildBossDamageRankData = mS2C_QueryGuildBossDamageRank.Data;
			this.GuildBossDamageRank = mS2C_QueryGuildBossDamageRank.SelfRank;
		}
		if (this.GuildBossDamageRankDataEvent != null)
		{
			this.GuildBossDamageRankDataEvent();
		}
	}

	public void SendTrialRankRequest()
	{
		MC2S_QueryTrialRank mC2S_QueryTrialRank = new MC2S_QueryTrialRank();
		mC2S_QueryTrialRank.TrialRankVersion = this.TrialRankVersion;
		Globals.Instance.CliSession.Send(606, mC2S_QueryTrialRank);
	}

	public void GetOreRankList()
	{
		MC2S_GetOreRankList mC2S_GetOreRankList = new MC2S_GetOreRankList();
		mC2S_GetOreRankList.Version = this.OreRankVersion;
		Globals.Instance.CliSession.Send(1035, mC2S_GetOreRankList);
	}

	public void GetGGOreRankList()
	{
		MC2S_GetGGOreRankList mC2S_GetGGOreRankList = new MC2S_GetGGOreRankList();
		mC2S_GetGGOreRankList.Version = this.GGOreRankVersion;
		Globals.Instance.CliSession.Send(1037, mC2S_GetGGOreRankList);
	}

	public void GetGOreRankList()
	{
		MC2S_GetGOreRankList mC2S_GetGOreRankList = new MC2S_GetGOreRankList();
		mC2S_GetGOreRankList.Version = this.GOreRankVersion;
		Globals.Instance.CliSession.Send(1039, mC2S_GetGOreRankList);
	}

	public void QueryGuildBossDamageRank()
	{
		MC2S_QueryGuildBossDamageRank mC2S_QueryGuildBossDamageRank = new MC2S_QueryGuildBossDamageRank();
		mC2S_QueryGuildBossDamageRank.Version = this.GuildBossDamageRankVersion;
		Globals.Instance.CliSession.Send(969, mC2S_QueryGuildBossDamageRank);
	}

	public int GetGGOreRank(GuildRank rankData)
	{
		for (int i = 0; i < this.GGOreRankData.Count; i++)
		{
			if (rankData.ID == this.GGOreRankData[i].ID)
			{
				return i + 1;
			}
		}
		return 0;
	}

	public void Destroy()
	{
		this.CombatValueRankData = null;
		this.CombatValueRankVersion = 0u;
		this.CombatValueRank = 0u;
		this.PVEStarsRankData = null;
		this.PVEStarsRankVersion = 0u;
		this.PVEStarsRank = 0u;
		this.LevelRankData = null;
		this.LevelRankVersion = 0u;
		this.LevelRank = 0u;
		this.TrialRankData = null;
		this.TrialRankVersion = 0u;
		this.TrialRank = 0u;
		this.RoseRankData = null;
		this.RoseRankVersion = 0u;
		this.RoseRank = 0u;
		this.TurtleRankData = null;
		this.TurtleRankVersion = 0u;
		this.TurtleRank = 0u;
		this.OreRankData = null;
		this.OreRankVersion = 0u;
		this.OreRank = 0u;
		this.GGOreRankData = null;
		this.GGOreRankVersion = 0u;
		this.GGOreRank = 0u;
		this.GOreRankData = null;
		this.GOreRankVersion = 0u;
		this.GOreRank = 0u;
		this.GuildBossDamageRankData = null;
		this.GuildBossDamageRankVersion = 0u;
		this.GuildBossDamageRank = 0u;
	}

	public void Update(float elapse)
	{
	}
}
