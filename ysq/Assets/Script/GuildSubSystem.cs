using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class GuildSubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public delegate void MemberCallback(GuildMember member);

	public delegate void RemoveMemberCallback(ulong id);

	public delegate void GuildApplyCallback(ulong id);

	public delegate void GuildSendGiftCallback(ulong id);

	public delegate void GuildTakeGiftCallback(ulong id);

	public delegate void QueryGuildRankCallback(int rank);

	public delegate void GuildBossDataUpdateCallback(int id);

	public delegate void GuildBossDeadCallback(int id, MonsterInfo info, string playerName);

	public delegate void DoGuildBossDamageCallback(int id, MonsterInfo info, string playerName, long damage);

	public delegate void GetChallengeGuardCallback(ulong id);

	public delegate void IntCallback(int id);

	public delegate void ScoreAddCallback(List<GuildWarAddScore> addScores);

	public delegate void GuildWarPromptCallback(string msg);

	public GuildSubSystem.VoidCallback QueryGuildDataEvent;

	public GuildSubSystem.VoidCallback GuildListUpdateEvent;

	public GuildSubSystem.VoidCallback GuildSearchListUpdateEvent;

	public GuildSubSystem.VoidCallback GuildUpdateEvent;

	public GuildSubSystem.MemberCallback AddMemberEvent;

	public GuildSubSystem.MemberCallback MemberUpdateEvent;

	public GuildSubSystem.MemberCallback AddMemberUpdateEvent;

	public GuildSubSystem.RemoveMemberCallback RemoveMemberEvent;

	public GuildSubSystem.VoidCallback LeaveGuildEvent;

	public GuildSubSystem.VoidCallback GuildNameChangedEvent;

	public GuildSubSystem.VoidCallback ImpeachGuildMasterEvent;

	public GuildSubSystem.VoidCallback GuildEventListUpdateEvent;

	public GuildSubSystem.GuildApplyCallback GuildApplyEvent;

	public GuildSubSystem.GuildSendGiftCallback GuildSendGiftEvent;

	public GuildSubSystem.GuildTakeGiftCallback GuildTakeGiftEvent;

	public GuildSubSystem.VoidCallback GuildInitDataEvent;

	public GuildSubSystem.QueryGuildRankCallback QueryGuildRankEvent;

	public GuildSubSystem.VoidCallback GetGuildBossDataEvent;

	public GuildSubSystem.GuildBossDataUpdateCallback GuildBossDataUpdateEvent;

	public GuildSubSystem.GuildBossDeadCallback GuildBossDeadEvent;

	public GuildSubSystem.DoGuildBossDamageCallback DoGuildBossDamageEvent;

	public GuildSubSystem.VoidCallback GuildBossDamageRankEvent;

	public GuildSubSystem.VoidCallback GuildStatUpEvent;

	public GuildSubSystem.VoidCallback SchoolLootDatasInitEvent;

	public GuildSubSystem.VoidCallback SchoolLootDatasUpdateEvent;

	public GuildSubSystem.VoidCallback DoDamageFailEvent;

	public GuildSubSystem.VoidCallback GetWarStateInfoEvent;

	public GuildSubSystem.VoidCallback SetDefensiveDataEvent;

	public GuildSubSystem.VoidCallback CastleUpdateEvent;

	public GuildSubSystem.VoidCallback QueryOrePillageDataEvent;

	public GuildSubSystem.IntCallback TakeOreRewardEvent;

	public GuildSubSystem.VoidCallback UpdateOrePillageDataEvent;

	public GuildSubSystem.VoidCallback QueryMyOreDataEvent;

	public GuildSubSystem.VoidCallback QueryGWKillRankEvent;

	public GuildSubSystem.VoidCallback GuildWarEnterEvent;

	public GuildSubSystem.VoidCallback QueryStrongHoldInfoEvent;

	public GuildSubSystem.VoidCallback LocalMemberUpDateEvent;

	public GuildSubSystem.VoidCallback GetBattleRecordEvent;

	public GuildSubSystem.VoidCallback StrongholdUpdateEvent;

	public GuildSubSystem.VoidCallback GuildWarRecoverHpEvent;

	public GuildSubSystem.ScoreAddCallback ScoreAddEvent;

	public GuildSubSystem.VoidCallback GuildWarRewardUpdateEvent;

	public GuildSubSystem.VoidCallback GuildWarEndEvent;

	public GuildSubSystem.GuildWarPromptCallback GuildWarPromptEvent;

	public GuildSubSystem.VoidCallback GuildWarPlayerDeadEvent;

	public GuildSubSystem.VoidCallback GuildWarDefendSurEvent;

	public GuildSubSystem.VoidCallback GuildWarBattleInfoEvent;

	public GuildSubSystem.VoidCallback GuildWarSupportEvent;

	public GuildSubSystem.VoidCallback SignRecordsEvent;

	public GuildData Guild;

	public List<GuildMember> Members;

	public List<BriefGuildData> GuildList;

	public List<BriefGuildData> GuildListForSearch;

	public List<GuildEvent> GuildEventList;

	public List<GuildRank> GuildRankDataList;

	private int joinGuildCD;

	private List<GuildBoss> guildBossData = new List<GuildBoss>();

	private int mGuildBossGMOpenTime;

	public int Rank;

	public Dictionary<int, List<RankData>> GuildBossRankDataDict = new Dictionary<int, List<RankData>>();

	public Dictionary<int, MS2C_GetLootReward> mSchoolLootDataCaches = new Dictionary<int, MS2C_GetLootReward>();

	public GuildWarStateInfo mWarStateInfo;

	public GuildWarPlayerData mGWPlayerData;

	public GuildWarClient mGWKillRankData;

	public GuildWarClient mGWEnterData;

	public bool mReqGWUpdate;

	public GuildWarClientTeamMember LocalClientMember = new GuildWarClientTeamMember();

	public List<GuildWarClientTeamMember> StrongHoldMembers = new List<GuildWarClientTeamMember>();

	public List<GuildWarBattleRecord> BattleRecords = new List<GuildWarBattleRecord>();

	public List<GuildWarClientSupportInfo> BattleSupportInfo = new List<GuildWarClientSupportInfo>();

	public List<string> mInteractionMsgs = new List<string>();

	public MS2C_QueryOrePillageData GuildMines;

	public MS2C_QueryMyOreData MyOreData;

	public int MyHpPct = 10000;

	public int EnemyHpPct = 10000;

	public uint mSignRecordVersion;

	public List<GuildSignRecord> mSignRecords = new List<GuildSignRecord>();

	private bool IsFirst;

	public int CurBossID
	{
		get;
		private set;
	}

	public int GuildBossGMOpenTime
	{
		get
		{
			return this.mGuildBossGMOpenTime;
		}
	}

	public GuildWarStronghold StrongHold
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(904, new ClientSession.MsgHandler(this.OnMsgCreateGuild));
		Globals.Instance.CliSession.Register(902, new ClientSession.MsgHandler(this.OnMsgGetGuildData));
		Globals.Instance.CliSession.Register(910, new ClientSession.MsgHandler(this.OnMsgGetGuildList));
		Globals.Instance.CliSession.Register(900, new ClientSession.MsgHandler(this.OnMsgInitGuildData));
		Globals.Instance.CliSession.Register(927, new ClientSession.MsgHandler(this.OnMsgGuildDataUpdate));
		Globals.Instance.CliSession.Register(928, new ClientSession.MsgHandler(this.OnMsgGuildMemberUpdate));
		Globals.Instance.CliSession.Register(929, new ClientSession.MsgHandler(this.OnMsgAddGuildMember));
		Globals.Instance.CliSession.Register(908, new ClientSession.MsgHandler(this.OnMsgRemoveGuildMember));
		Globals.Instance.CliSession.Register(906, new ClientSession.MsgHandler(this.OnMsgLeaveGuild));
		Globals.Instance.CliSession.Register(953, new ClientSession.MsgHandler(this.OnMsgChangeGuildName));
		Globals.Instance.CliSession.Register(924, new ClientSession.MsgHandler(this.OnMsgImpeachGuildMaster));
		Globals.Instance.CliSession.Register(922, new ClientSession.MsgHandler(this.OnMsgQueryGuildEvent));
		Globals.Instance.CliSession.Register(912, new ClientSession.MsgHandler(this.OnMsgGuildApply));
		Globals.Instance.CliSession.Register(951, new ClientSession.MsgHandler(this.OnMsgGiveGift));
		Globals.Instance.CliSession.Register(949, new ClientSession.MsgHandler(this.OnMsgTakeGift));
		Globals.Instance.CliSession.Register(959, new ClientSession.MsgHandler(this.OnMsgGuildRankData));
		Globals.Instance.CliSession.Register(935, new ClientSession.MsgHandler(this.OnMsgGetGuildBoss));
		Globals.Instance.CliSession.Register(937, new ClientSession.MsgHandler(this.OnMsgGuildBossUpdate));
		Globals.Instance.CliSession.Register(939, new ClientSession.MsgHandler(this.OnMsgDoGuildBossDamage));
		Globals.Instance.CliSession.Register(964, new ClientSession.MsgHandler(this.OnMsgGuildBroadcastDamage));
		Globals.Instance.CliSession.Register(966, new ClientSession.MsgHandler(this.OnMsgGuildBossDead));
		Globals.Instance.CliSession.Register(976, new ClientSession.MsgHandler(this.OnMsgQueryGuildWarState));
		Globals.Instance.CliSession.Register(998, new ClientSession.MsgHandler(this.OnMsgGuildWarKillRank));
		Globals.Instance.CliSession.Register(980, new ClientSession.MsgHandler(this.OnMsgGuildWarEnter));
		Globals.Instance.CliSession.Register(982, new ClientSession.MsgHandler(this.OnMsgQueryStrongholdInfo));
		Globals.Instance.CliSession.Register(990, new ClientSession.MsgHandler(this.OnMsgGuildWarHold));
		Globals.Instance.CliSession.Register(1004, new ClientSession.MsgHandler(this.OnMsgGuildWarQuitHold));
		Globals.Instance.CliSession.Register(1002, new ClientSession.MsgHandler(this.OnMsgQueryCombatRecord));
		Globals.Instance.CliSession.Register(1000, new ClientSession.MsgHandler(this.OnMsgGuildWarReqRevive));
		Globals.Instance.CliSession.Register(1006, new ClientSession.MsgHandler(this.OnMsgGuildWarRecoverHP));
		Globals.Instance.CliSession.Register(1008, new ClientSession.MsgHandler(this.OnMsgGuildWarPlayerUpdate));
		Globals.Instance.CliSession.Register(1010, new ClientSession.MsgHandler(this.OnMsgGuildWarStrongholdUpdate));
		Globals.Instance.CliSession.Register(994, new ClientSession.MsgHandler(this.OnMsgGuildWarUpdate));
		Globals.Instance.CliSession.Register(1012, new ClientSession.MsgHandler(this.OnMsgGuildWarRewardUpdate));
		Globals.Instance.CliSession.Register(1014, new ClientSession.MsgHandler(this.OnMsgGuildWarEnd));
		Globals.Instance.CliSession.Register(1018, new ClientSession.MsgHandler(this.OnMsgGuildWarGetSupportInfo));
		Globals.Instance.CliSession.Register(995, new ClientSession.MsgHandler(this.OnMsgGuildWarSupport));
		Globals.Instance.CliSession.Register(1016, new ClientSession.MsgHandler(this.OnMsgGuildWarKickHold));
		Globals.Instance.CliSession.Register(945, new ClientSession.MsgHandler(this.OnMsgGetLootReward));
		Globals.Instance.CliSession.Register(1021, new ClientSession.MsgHandler(this.OnMsgQueryOrePillageData));
		Globals.Instance.CliSession.Register(1025, new ClientSession.MsgHandler(this.OnMsgTakeOreReward));
		Globals.Instance.CliSession.Register(1034, new ClientSession.MsgHandler(this.OnMsgUpdateOrePillageData));
		Globals.Instance.CliSession.Register(1023, new ClientSession.MsgHandler(this.OnMsgQueryMyOreData));
		Globals.Instance.CliSession.Register(1027, new ClientSession.MsgHandler(this.OnMsgOrePillageStart));
		Globals.Instance.CliSession.Register(984, new ClientSession.MsgHandler(this.OnMsgGuildWarFightBegin));
		Globals.Instance.CliSession.Register(996, new ClientSession.MsgHandler(this.OnMsgGuildWarPrompt));
		Globals.Instance.CliSession.Register(957, new ClientSession.MsgHandler(this.OnMsgGuildSignRecord));
		TeamSubSystem expr_5EF = Globals.Instance.Player.TeamSystem;
		expr_5EF.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Combine(expr_5EF.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Clear();
		this.joinGuildCD = 0;
		this.CurBossID = 0;
		this.Rank = 0;
		this.GuildBossRankDataDict.Clear();
		this.mSchoolLootDataCaches.Clear();
		this.GuildMines = null;
		this.MyOreData = null;
		this.mGWEnterData = null;
		this.IsFirst = false;
		if (Globals.Instance != null)
		{
			TeamSubSystem expr_6C = Globals.Instance.Player.TeamSystem;
			expr_6C.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Remove(expr_6C.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
		}
	}

	public void Clear()
	{
		this.Guild = null;
		this.Members = null;
		this.GuildList = null;
		this.GuildListForSearch = null;
		this.GuildEventList = null;
		this.GuildRankDataList = null;
		this.mGWPlayerData = null;
		this.mGWKillRankData = null;
		this.StrongHold = null;
		this.mWarStateInfo = null;
		this.LocalClientMember = null;
		this.guildBossData.Clear();
		this.StrongHoldMembers.Clear();
		this.BattleRecords.Clear();
		this.BattleSupportInfo.Clear();
		this.mInteractionMsgs.Clear();
		this.mSignRecords.Clear();
		if (Globals.Instance != null && Globals.Instance.Player != null)
		{
			Globals.Instance.Player.ShowChatGuildNewMark = false;
			Globals.Instance.Player.GuildMsgs.Clear();
		}
	}

	public void LoadData(GuildWarPlayerData gwData)
	{
		this.mGWPlayerData = gwData;
	}

	public bool HasGuild()
	{
		return Globals.Instance.Player.Data.HasGuild == 1 || (this.Guild != null && this.Guild.ID != 0uL);
	}

	public GuildMember GetMember(ulong id)
	{
		if (this.Members != null)
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				if (this.Members[i].ID == id)
				{
					return this.Members[i];
				}
			}
		}
		return null;
	}

	public BriefGuildData GetJoinTabMember(ulong id)
	{
		for (int i = 0; i < this.GuildList.Count; i++)
		{
			if (this.GuildList[i].ID == id)
			{
				return this.GuildList[i];
			}
		}
		return null;
	}

	public BriefGuildData GetSearchTabMember(ulong id)
	{
		for (int i = 0; i < this.GuildListForSearch.Count; i++)
		{
			if (this.GuildListForSearch[i].ID == id)
			{
				return this.GuildListForSearch[i];
			}
		}
		return null;
	}

	public void RemoveMember(ulong id)
	{
		for (int i = 0; i < this.Members.Count; i++)
		{
			if (this.Members[i].ID == id)
			{
				this.Members.RemoveAt(i);
				return;
			}
		}
	}

	public int GetJoinGuildCD()
	{
		if (this.joinGuildCD > Globals.Instance.Player.GetTimeStamp())
		{
			return this.joinGuildCD - Globals.Instance.Player.GetTimeStamp();
		}
		return 0;
	}

	public GuildBoss GetGuildBoss(int id)
	{
		for (int i = 0; i < this.guildBossData.Count; i++)
		{
			if (this.guildBossData[i] != null && this.guildBossData[i].Data != null && this.guildBossData[i].Data.ID == id)
			{
				return this.guildBossData[i];
			}
		}
		return null;
	}

	public GuildBoss GetCurGuildBoss()
	{
		if (this.CurBossID == 0)
		{
			return null;
		}
		return this.GetGuildBoss(this.CurBossID);
	}

	public GuildBossData GetGuildBossData(int id)
	{
		GuildBoss guildBoss = this.GetGuildBoss(id);
		if (guildBoss == null)
		{
			return null;
		}
		return guildBoss.Data;
	}

	public bool IsBossDead(int id)
	{
		GuildBoss guildBoss = this.GetGuildBoss(id);
		return guildBoss != null && guildBoss.Data.HealthPct == 0f;
	}

	public GuildBossData GetCurGuildBossData()
	{
		if (this.CurBossID == 0)
		{
			return null;
		}
		return this.GetGuildBossData(this.CurBossID);
	}

	public void SetCurBossID(int id)
	{
		if (this.GetGuildBoss(id) == null)
		{
			Debug.LogErrorFormat("boss id invalid, ID = {0}", new object[]
			{
				id
			});
			return;
		}
		this.CurBossID = id;
	}

	public void UpdateCurBossData(int infoID, float healthPct)
	{
		GuildBoss guildBoss = this.GetGuildBoss(this.CurBossID);
		if (guildBoss == null)
		{
			return;
		}
		guildBoss.Data.HealthPct = healthPct;
		guildBoss.Data.InfoID = infoID;
		guildBoss.ResetInfo();
	}

	public void OnMsgGetGuildData(MemoryStream stream)
	{
		MS2C_GetGuildData mS2C_GetGuildData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetGuildData), stream) as MS2C_GetGuildData;
		if (mS2C_GetGuildData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GetGuildData.Result);
			return;
		}
		if (this.QueryGuildDataEvent != null)
		{
			this.QueryGuildDataEvent();
		}
	}

	public void OnMsgGetGuildList(MemoryStream stream)
	{
		MS2C_GetGuildList mS2C_GetGuildList = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetGuildList), stream) as MS2C_GetGuildList;
		this.Guild = null;
		this.Members = null;
		this.joinGuildCD = mS2C_GetGuildList.JoinGuildCD;
		if (mS2C_GetGuildList.Flag)
		{
			this.GuildListForSearch = mS2C_GetGuildList.Data;
			if (this.GuildSearchListUpdateEvent != null)
			{
				this.GuildSearchListUpdateEvent();
			}
		}
		else
		{
			this.GuildList = mS2C_GetGuildList.Data;
			if (this.GuildListUpdateEvent != null)
			{
				this.GuildListUpdateEvent();
			}
		}
	}

	public void OnMsgInitGuildData(MemoryStream stream)
	{
		MS2C_InitGuildData mS2C_InitGuildData = Serializer.NonGeneric.Deserialize(typeof(MS2C_InitGuildData), stream) as MS2C_InitGuildData;
		this.Guild = mS2C_InitGuildData.Data;
		this.Members = mS2C_InitGuildData.MemberData;
		if (this.GuildInitDataEvent != null)
		{
			this.GuildInitDataEvent();
		}
		if (!string.IsNullOrEmpty(this.Guild.Manifesto) && !this.IsFirst)
		{
			this.IsFirst = true;
			LocalPlayer player = Globals.Instance.Player;
			player.PushGuildMessage(new ChatMessage
			{
				Channel = 0,
				Message = Singleton<StringManager>.Instance.GetString("FairyR_3", new object[]
				{
					this.Guild.Manifesto
				}),
				Name = Singleton<StringManager>.Instance.GetString("FairyR_2"),
				TimeStamp = GameCache.Data.HasReadedWorldMsgTimeStamp
			});
		}
	}

	public void OnMsgGuildDataUpdate(MemoryStream stream)
	{
		if (this.Guild == null)
		{
			return;
		}
		MS2C_GuildDataUpdate mS2C_GuildDataUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildDataUpdate), stream) as MS2C_GuildDataUpdate;
		if (!string.IsNullOrEmpty(mS2C_GuildDataUpdate.Name))
		{
			this.Guild.Name = mS2C_GuildDataUpdate.Name;
		}
		if (mS2C_GuildDataUpdate.Level != 0)
		{
			this.Guild.Level = mS2C_GuildDataUpdate.Level;
		}
		if (mS2C_GuildDataUpdate.Exp != 0)
		{
			if (mS2C_GuildDataUpdate.Exp == -1)
			{
				this.Guild.Exp = 0;
			}
			else
			{
				this.Guild.Exp = mS2C_GuildDataUpdate.Exp;
			}
		}
		if (mS2C_GuildDataUpdate.Money != 0)
		{
			if (mS2C_GuildDataUpdate.Money == -1)
			{
				this.Guild.Money = 0;
			}
			else
			{
				this.Guild.Money = mS2C_GuildDataUpdate.Money;
			}
		}
		if (mS2C_GuildDataUpdate.Prosperity != 0)
		{
			this.Guild.Prosperity = mS2C_GuildDataUpdate.Prosperity;
		}
		if (mS2C_GuildDataUpdate.HasManifesto)
		{
			this.Guild.Manifesto = mS2C_GuildDataUpdate.Manifesto;
		}
		if (mS2C_GuildDataUpdate.ImpeachTimeStamp != 0)
		{
			this.Guild.ImpeachTimeStamp = mS2C_GuildDataUpdate.ImpeachTimeStamp;
			this.Guild.ImpeachID = mS2C_GuildDataUpdate.ImpeachID;
		}
		if (mS2C_GuildDataUpdate.ApplyLevel != 0)
		{
			this.Guild.ApplyLevel = mS2C_GuildDataUpdate.ApplyLevel;
		}
		if (mS2C_GuildDataUpdate.Verify != 0)
		{
			if (mS2C_GuildDataUpdate.Verify == -1)
			{
				this.Guild.Verify = false;
			}
			else
			{
				this.Guild.Verify = true;
			}
		}
		if (mS2C_GuildDataUpdate.CombatValue != 0)
		{
			if (mS2C_GuildDataUpdate.CombatValue == -1)
			{
				this.Guild.CombatValue = 0;
			}
			else
			{
				this.Guild.CombatValue = mS2C_GuildDataUpdate.CombatValue;
			}
		}
		if (mS2C_GuildDataUpdate.Score != 0)
		{
			if (mS2C_GuildDataUpdate.Score == -1)
			{
				this.Guild.Score = 0;
			}
			else
			{
				this.Guild.Score = mS2C_GuildDataUpdate.Score;
			}
		}
		if (mS2C_GuildDataUpdate.SignNum != 0)
		{
			if (mS2C_GuildDataUpdate.SignNum == -1)
			{
				this.Guild.SignNum = 0;
			}
			else
			{
				this.Guild.SignNum = mS2C_GuildDataUpdate.SignNum;
			}
		}
		if (mS2C_GuildDataUpdate.MaxAcademyID != 0)
		{
			if (mS2C_GuildDataUpdate.MaxAcademyID == -1)
			{
				this.Guild.MaxAcademyID = 0;
			}
			else
			{
				this.Guild.MaxAcademyID = mS2C_GuildDataUpdate.MaxAcademyID;
			}
		}
		if (mS2C_GuildDataUpdate.AttackAcademyID1 != 0)
		{
			if (mS2C_GuildDataUpdate.AttackAcademyID1 == -1)
			{
				this.Guild.AttackAcademyID1 = 0;
			}
			else
			{
				this.Guild.AttackAcademyID1 = mS2C_GuildDataUpdate.AttackAcademyID1;
			}
		}
		if (mS2C_GuildDataUpdate.AttackAcademyID2 != 0)
		{
			if (mS2C_GuildDataUpdate.AttackAcademyID2 == -1)
			{
				this.Guild.AttackAcademyID2 = 0;
			}
			else
			{
				this.Guild.AttackAcademyID2 = mS2C_GuildDataUpdate.AttackAcademyID2;
			}
		}
		if (this.GuildUpdateEvent != null)
		{
			this.GuildUpdateEvent();
		}
	}

	public void OnMsgGuildMemberUpdate(MemoryStream stream)
	{
		MS2C_GuildMemberUpdate mS2C_GuildMemberUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildMemberUpdate), stream) as MS2C_GuildMemberUpdate;
		GuildMember guildMember = this.GetMember(mS2C_GuildMemberUpdate.ID);
		if (guildMember == null)
		{
			if (mS2C_GuildMemberUpdate.ID == 0uL)
			{
				return;
			}
			guildMember = new GuildMember();
			guildMember.ID = mS2C_GuildMemberUpdate.ID;
			guildMember.Level = mS2C_GuildMemberUpdate.Level;
			guildMember.TotalReputation = mS2C_GuildMemberUpdate.TotalReputation;
			guildMember.CurReputation = mS2C_GuildMemberUpdate.CurReputation;
			guildMember.Rank = mS2C_GuildMemberUpdate.Rank;
			guildMember.Flag = mS2C_GuildMemberUpdate.Flag;
			guildMember.LastOnlineTime = mS2C_GuildMemberUpdate.LastOnlineTime;
			guildMember.Name = mS2C_GuildMemberUpdate.Name;
			this.Members.Add(guildMember);
			if (this.AddMemberUpdateEvent != null)
			{
				this.AddMemberUpdateEvent(guildMember);
			}
		}
		else
		{
			if (mS2C_GuildMemberUpdate.Level != 0)
			{
				guildMember.Level = mS2C_GuildMemberUpdate.Level;
			}
			if (mS2C_GuildMemberUpdate.TotalReputation != 0)
			{
				guildMember.TotalReputation = mS2C_GuildMemberUpdate.TotalReputation;
			}
			if (mS2C_GuildMemberUpdate.CurReputation != 0)
			{
				if (mS2C_GuildMemberUpdate.CurReputation == -1)
				{
					guildMember.CurReputation = 0;
				}
				else
				{
					guildMember.CurReputation = mS2C_GuildMemberUpdate.CurReputation;
				}
			}
			if (mS2C_GuildMemberUpdate.Rank != 0)
			{
				guildMember.Rank = mS2C_GuildMemberUpdate.Rank;
			}
			if (mS2C_GuildMemberUpdate.Flag != 0)
			{
				guildMember.Flag = mS2C_GuildMemberUpdate.Flag;
			}
			if (this.MemberUpdateEvent != null)
			{
				this.MemberUpdateEvent(guildMember);
			}
		}
	}

	public void OnMsgAddGuildMember(MemoryStream stream)
	{
		MS2C_AddGuildMember mS2C_AddGuildMember = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddGuildMember), stream) as MS2C_AddGuildMember;
		if (this.GetMember(mS2C_AddGuildMember.Data.ID) == null)
		{
			this.Members.Add(mS2C_AddGuildMember.Data);
			if (this.AddMemberEvent != null)
			{
				this.AddMemberEvent(mS2C_AddGuildMember.Data);
			}
		}
	}

	public void OnMsgRemoveGuildMember(MemoryStream stream)
	{
		MS2C_RemoveGuildMember mS2C_RemoveGuildMember = Serializer.NonGeneric.Deserialize(typeof(MS2C_RemoveGuildMember), stream) as MS2C_RemoveGuildMember;
		if (mS2C_RemoveGuildMember.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_RemoveGuildMember.Result);
			return;
		}
		this.RemoveMember(mS2C_RemoveGuildMember.ID);
		if (this.RemoveMemberEvent != null)
		{
			this.RemoveMemberEvent(mS2C_RemoveGuildMember.ID);
		}
		if (mS2C_RemoveGuildMember.ID == Globals.Instance.Player.Data.ID)
		{
			this.Clear();
			if (this.LeaveGuildEvent != null)
			{
				this.LeaveGuildEvent();
			}
			if (GameUIManager.mInstance == null)
			{
				return;
			}
			GUIChatWindowV2.TryCloseMe();
		}
	}

	public void OnMsgLeaveGuild(MemoryStream stream)
	{
		MS2C_LeaveGuild mS2C_LeaveGuild = Serializer.NonGeneric.Deserialize(typeof(MS2C_LeaveGuild), stream) as MS2C_LeaveGuild;
		if (mS2C_LeaveGuild.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_LeaveGuild.Result);
			return;
		}
		this.Clear();
		if (this.LeaveGuildEvent != null)
		{
			this.LeaveGuildEvent();
		}
		if (GameUIManager.mInstance == null)
		{
			return;
		}
		GUIChatWindowV2.TryCloseMe();
	}

	private void OnMsgChangeGuildName(MemoryStream stream)
	{
		MS2C_ChangeGuildName mS2C_ChangeGuildName = Serializer.NonGeneric.Deserialize(typeof(MS2C_ChangeGuildName), stream) as MS2C_ChangeGuildName;
		if (mS2C_ChangeGuildName.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_ChangeGuildName.Result);
			return;
		}
		if (this.GuildNameChangedEvent != null)
		{
			this.GuildNameChangedEvent();
		}
	}

	private void OnMsgImpeachGuildMaster(MemoryStream stream)
	{
		MS2C_ImpeachGuildMaster mS2C_ImpeachGuildMaster = Serializer.NonGeneric.Deserialize(typeof(MS2C_ImpeachGuildMaster), stream) as MS2C_ImpeachGuildMaster;
		if (mS2C_ImpeachGuildMaster.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_ImpeachGuildMaster.Result);
			return;
		}
		if (this.ImpeachGuildMasterEvent != null)
		{
			this.ImpeachGuildMasterEvent();
		}
	}

	private int GetMaxEventID()
	{
		int num = 0;
		if (this.GuildEventList != null)
		{
			for (int i = 0; i < this.GuildEventList.Count; i++)
			{
				if (this.GuildEventList[i].ID > num)
				{
					num = this.GuildEventList[i].ID;
				}
			}
		}
		return num;
	}

	public void SendQueryGuildEvent()
	{
		MC2S_QueryGuildEvent mC2S_QueryGuildEvent = new MC2S_QueryGuildEvent();
		mC2S_QueryGuildEvent.MaxEventID = this.GetMaxEventID();
		Globals.Instance.CliSession.Send(921, mC2S_QueryGuildEvent);
	}

	private void OnMsgQueryGuildEvent(MemoryStream stream)
	{
		MS2C_QueryGuildEvent mS2C_QueryGuildEvent = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryGuildEvent), stream) as MS2C_QueryGuildEvent;
		if (mS2C_QueryGuildEvent.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_QueryGuildEvent.Result);
			return;
		}
		if (this.GuildEventList == null)
		{
			this.GuildEventList = mS2C_QueryGuildEvent.Data;
		}
		else
		{
			for (int i = 0; i < mS2C_QueryGuildEvent.Data.Count; i++)
			{
				this.GuildEventList.Add(mS2C_QueryGuildEvent.Data[i]);
			}
		}
		if (this.GuildEventList != null)
		{
			this.GuildEventList.Sort(delegate(GuildEvent a, GuildEvent b)
			{
				if (a.TimeStamp > b.TimeStamp)
				{
					return -1;
				}
				if (a.TimeStamp < b.TimeStamp)
				{
					return 1;
				}
				return 0;
			});
			if (this.GuildEventListUpdateEvent != null)
			{
				this.GuildEventListUpdateEvent();
			}
		}
	}

	private void OnMsgGuildApply(MemoryStream stream)
	{
		MS2C_GuildApply mS2C_GuildApply = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildApply), stream) as MS2C_GuildApply;
		if (mS2C_GuildApply.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildApply.Result);
			return;
		}
		if (this.HasGuild())
		{
			if (this.GuildList != null)
			{
				for (int i = 0; i < this.GuildList.Count; i++)
				{
					if (this.GuildList[i].ID == mS2C_GuildApply.ID)
					{
						this.GuildList.Remove(this.GuildList[i]);
						break;
					}
				}
			}
			if (this.GuildListForSearch != null)
			{
				for (int j = 0; j < this.GuildListForSearch.Count; j++)
				{
					if (this.GuildListForSearch[j].ID == mS2C_GuildApply.ID)
					{
						this.GuildListForSearch.Remove(this.GuildListForSearch[j]);
						break;
					}
				}
			}
			if (GameUIManager.mInstance.CurUISession.GetType() != typeof(GUIGuildManageScene))
			{
				GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
			}
		}
		else
		{
			if (this.GuildList != null)
			{
				for (int k = 0; k < this.GuildList.Count; k++)
				{
					if (this.GuildList[k].ID == mS2C_GuildApply.ID)
					{
						this.GuildList[k].Flag = 1;
						break;
					}
				}
			}
			if (this.GuildListForSearch != null)
			{
				for (int l = 0; l < this.GuildListForSearch.Count; l++)
				{
					if (this.GuildListForSearch[l].ID == mS2C_GuildApply.ID)
					{
						this.GuildListForSearch[l].Flag = 1;
						break;
					}
				}
			}
			if (this.GuildApplyEvent != null)
			{
				this.GuildApplyEvent(mS2C_GuildApply.ID);
			}
		}
	}

	private void OnMsgGiveGift(MemoryStream stream)
	{
		MS2C_GiveGift mS2C_GiveGift = Serializer.NonGeneric.Deserialize(typeof(MS2C_GiveGift), stream) as MS2C_GiveGift;
		if (mS2C_GiveGift.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GiveGift.Result);
			return;
		}
		if (mS2C_GiveGift.PlayerID != 0uL)
		{
			GuildMember member = this.GetMember(mS2C_GiveGift.PlayerID);
			if (member == null)
			{
				return;
			}
			member.Flag |= 2;
		}
		else
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				this.Members[i].Flag |= 2;
			}
		}
		if (this.GuildSendGiftEvent != null)
		{
			this.GuildSendGiftEvent(mS2C_GiveGift.PlayerID);
		}
	}

	private void OnMsgTakeGift(MemoryStream stream)
	{
		MS2C_TakeGift mS2C_TakeGift = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeGift), stream) as MS2C_TakeGift;
		if (mS2C_TakeGift.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_TakeGift.Result);
			return;
		}
		if (mS2C_TakeGift.PlayerID != 0uL)
		{
			GuildMember member = this.GetMember(mS2C_TakeGift.PlayerID);
			if (member == null)
			{
				return;
			}
			member.Flag |= 1;
		}
		else
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				this.Members[i].Flag |= 1;
			}
		}
		if (this.GuildTakeGiftEvent != null)
		{
			this.GuildSendGiftEvent(mS2C_TakeGift.PlayerID);
		}
	}

	public int GetGuildRank(GuildRank rankData)
	{
		for (int i = 0; i < this.GuildRankDataList.Count; i++)
		{
			if (rankData.ID == this.GuildRankDataList[i].ID)
			{
				return i + 1;
			}
		}
		return 0;
	}

	public void OnMsgGuildRankData(MemoryStream stream)
	{
		MS2C_GuildRankData mS2C_GuildRankData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildRankData), stream) as MS2C_GuildRankData;
		if (mS2C_GuildRankData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildRankData.Result);
			return;
		}
		this.GuildRankDataList = mS2C_GuildRankData.Data;
		this.Rank = mS2C_GuildRankData.Rank;
		if (this.QueryGuildRankEvent != null)
		{
			this.QueryGuildRankEvent(mS2C_GuildRankData.Rank);
		}
	}

	private void OnMsgGetGuildBoss(MemoryStream stream)
	{
		MS2C_GetGuildBoss mS2C_GetGuildBoss = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetGuildBoss), stream) as MS2C_GetGuildBoss;
		if (mS2C_GetGuildBoss.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GetGuildBoss.Result);
			return;
		}
		this.guildBossData.Clear();
		for (int i = 0; i < mS2C_GetGuildBoss.Data.Count; i++)
		{
			this.guildBossData.Add(new GuildBoss(mS2C_GetGuildBoss.Data[i]));
		}
		this.mGuildBossGMOpenTime = mS2C_GetGuildBoss.GMOpenTime;
		if (this.GetGuildBossDataEvent != null)
		{
			this.GetGuildBossDataEvent();
		}
	}

	private void OnMsgGuildBossUpdate(MemoryStream stream)
	{
		MS2C_GuildBossUpdate mS2C_GuildBossUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildBossUpdate), stream) as MS2C_GuildBossUpdate;
		if (mS2C_GuildBossUpdate.Data != null)
		{
			GuildBoss guildBoss = this.GetGuildBoss(mS2C_GuildBossUpdate.Data.ID);
			if (guildBoss == null)
			{
				if (mS2C_GuildBossUpdate.Data.ID != 0)
				{
					guildBoss = new GuildBoss(mS2C_GuildBossUpdate.Data);
					this.guildBossData.Add(guildBoss);
				}
			}
			else
			{
				guildBoss.Data.HealthPct = mS2C_GuildBossUpdate.Data.HealthPct;
				guildBoss.Data.InfoID = mS2C_GuildBossUpdate.Data.InfoID;
				guildBoss.Data.MaxHP = mS2C_GuildBossUpdate.Data.MaxHP;
				guildBoss.ResetInfo();
			}
		}
		if (mS2C_GuildBossUpdate.GMOpenTime != 0)
		{
			if (mS2C_GuildBossUpdate.GMOpenTime == -1)
			{
				this.mGuildBossGMOpenTime = 0;
			}
			else
			{
				this.mGuildBossGMOpenTime = mS2C_GuildBossUpdate.GMOpenTime;
			}
		}
		if (this.GuildBossDataUpdateEvent != null)
		{
			this.GuildBossDataUpdateEvent(mS2C_GuildBossUpdate.Data.ID);
		}
	}

	public void OnMsgDoGuildBossDamage(MemoryStream stream)
	{
		MS2C_DoGuildBossDamage mS2C_DoGuildBossDamage = Serializer.NonGeneric.Deserialize(typeof(MS2C_DoGuildBossDamage), stream) as MS2C_DoGuildBossDamage;
		if (mS2C_DoGuildBossDamage.Result != 0 && this.DoDamageFailEvent != null)
		{
			this.DoDamageFailEvent();
		}
	}

	public void OnMsgGuildBroadcastDamage(MemoryStream stream)
	{
		MS2C_GuildBroadcastDamage mS2C_GuildBroadcastDamage = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildBroadcastDamage), stream) as MS2C_GuildBroadcastDamage;
		GuildBoss guildBoss = this.GetGuildBoss(mS2C_GuildBroadcastDamage.ID);
		if (guildBoss == null)
		{
			return;
		}
		guildBoss.Data.HealthPct = mS2C_GuildBroadcastDamage.HealthPct;
		guildBoss.Data.InfoID = mS2C_GuildBroadcastDamage.InfoID;
		guildBoss.ResetInfo();
		if (Globals.Instance.Player.Data.Name.Equals(mS2C_GuildBroadcastDamage.Name))
		{
			return;
		}
		if (this.DoGuildBossDamageEvent != null)
		{
			this.DoGuildBossDamageEvent(mS2C_GuildBroadcastDamage.ID, guildBoss.Info, mS2C_GuildBroadcastDamage.Name, mS2C_GuildBroadcastDamage.Damage);
		}
	}

	public RankData GetCurSchoolBossDamageRankData(int index)
	{
		GuildBoss curGuildBoss = this.GetCurGuildBoss();
		if (curGuildBoss != null)
		{
			return curGuildBoss.GetDamageRankData(index);
		}
		return null;
	}

	public void OnMsgGuildBossDead(MemoryStream stream)
	{
		MS2C_GuildBossDead mS2C_GuildBossDead = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildBossDead), stream) as MS2C_GuildBossDead;
		GuildBoss guildBoss = this.GetGuildBoss(mS2C_GuildBossDead.ID);
		if (guildBoss == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetGuildBoss error, ID = {0}", mS2C_GuildBossDead.ID)
			});
			return;
		}
		guildBoss.Data.HealthPct = 0f;
		if (this.GuildBossDeadEvent != null)
		{
			this.GuildBossDeadEvent(mS2C_GuildBossDead.ID, guildBoss.Info, mS2C_GuildBossDead.Name);
		}
	}

	public int GetOpendGuildBossLvl()
	{
		int num = 0;
		for (int i = 0; i < this.guildBossData.Count; i++)
		{
			if (this.guildBossData[i].Data.ID > num)
			{
				num = this.guildBossData[i].Data.ID;
			}
		}
		return num;
	}

	public List<RankData> GetGuildBossDamageRank(int schoolId)
	{
		List<RankData> result;
		if (this.GuildBossRankDataDict.TryGetValue(schoolId, out result))
		{
			return result;
		}
		return null;
	}

	public int GetSelfGuildBossDamageRank(int schoolId, out long damage)
	{
		int result = 0;
		damage = 0L;
		List<RankData> list;
		if (this.GuildBossRankDataDict.TryGetValue(schoolId, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Data.GUID == Globals.Instance.Player.Data.ID)
				{
					result = list[i].Rank;
					damage = list[i].Value;
				}
			}
		}
		return result;
	}

	public void DoSendGetLootRequest(int schoolId)
	{
		MC2S_GetLootReward mC2S_GetLootReward = new MC2S_GetLootReward();
		mC2S_GetLootReward.ID = schoolId;
		Globals.Instance.CliSession.Send(944, mC2S_GetLootReward);
	}

	private void OnMsgGetLootReward(MemoryStream stream)
	{
		MS2C_GetLootReward mS2C_GetLootReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetLootReward), stream) as MS2C_GetLootReward;
		this.mSchoolLootDataCaches[mS2C_GetLootReward.ID] = mS2C_GetLootReward;
		if (this.SchoolLootDatasInitEvent != null)
		{
			this.SchoolLootDatasInitEvent();
		}
	}

	public void ClearSchoolLootDataCache()
	{
		this.mSchoolLootDataCaches.Clear();
	}

	public void RequestQueryWarState()
	{
		MC2S_GuildWarQueryInfo ojb = new MC2S_GuildWarQueryInfo();
		Globals.Instance.CliSession.Send(975, ojb);
	}

	public void RequestWarUpdate()
	{
		if (this.mReqGWUpdate)
		{
			return;
		}
		this.mReqGWUpdate = true;
		GuildWarClient guildWarClient = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (guildWarClient != null)
		{
			MC2S_GuildWarUpdate mC2S_GuildWarUpdate = new MC2S_GuildWarUpdate();
			mC2S_GuildWarUpdate.WarID = guildWarClient.WarID;
			Globals.Instance.CliSession.Send(993, mC2S_GuildWarUpdate);
		}
	}

	public void RequestKillRank()
	{
		GuildWarClient guildWarClient = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (guildWarClient != null)
		{
			MC2S_GuildWarKillRank mC2S_GuildWarKillRank = new MC2S_GuildWarKillRank();
			mC2S_GuildWarKillRank.WarID = guildWarClient.WarID;
			Globals.Instance.CliSession.Send(997, mC2S_GuildWarKillRank);
		}
	}

	private void OnMsgQueryGuildWarState(MemoryStream stream)
	{
		MS2C_GuildWarQueryInfo mS2C_GuildWarQueryInfo = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarQueryInfo), stream) as MS2C_GuildWarQueryInfo;
		if (mS2C_GuildWarQueryInfo.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarQueryInfo.Result);
			return;
		}
		if (this.mWarStateInfo == null)
		{
			this.mWarStateInfo = new GuildWarStateInfo();
		}
		this.mWarStateInfo.ResetStateInfo(mS2C_GuildWarQueryInfo.Status, mS2C_GuildWarQueryInfo.Timestamp, mS2C_GuildWarQueryInfo.Citys, mS2C_GuildWarQueryInfo.WarData);
		if (this.GetWarStateInfoEvent != null)
		{
			this.GetWarStateInfoEvent();
		}
	}

	private void OnMsgGuildWarEnter(MemoryStream stream)
	{
		MS2C_GuildWarEnter mS2C_GuildWarEnter = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarEnter), stream) as MS2C_GuildWarEnter;
		if (mS2C_GuildWarEnter.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarEnter.Result);
			return;
		}
		this.mGWEnterData = mS2C_GuildWarEnter.WarData;
		if (this.LocalClientMember == null)
		{
			this.LocalClientMember = new GuildWarClientTeamMember();
		}
		if (this.LocalClientMember != null)
		{
			this.LocalClientMember.Member = mS2C_GuildWarEnter.Player;
			this.LocalClientMember.Data = Tools.LocalPlayerToRemote();
		}
		if (this.GuildWarEnterEvent != null)
		{
			this.GuildWarEnterEvent();
		}
	}

	private void OnMsgQueryStrongholdInfo(MemoryStream stream)
	{
		MS2C_GuildWarQueryStrongholdInfo mS2C_GuildWarQueryStrongholdInfo = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarQueryStrongholdInfo), stream) as MS2C_GuildWarQueryStrongholdInfo;
		if (mS2C_GuildWarQueryStrongholdInfo.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarQueryStrongholdInfo.Result);
			return;
		}
		this.StrongHold = mS2C_GuildWarQueryStrongholdInfo.Stronghold;
		this.StrongHoldMembers = mS2C_GuildWarQueryStrongholdInfo.Members;
		if (this.QueryStrongHoldInfoEvent != null)
		{
			this.QueryStrongHoldInfoEvent();
		}
	}

	private void OnMsgGuildWarHold(MemoryStream stream)
	{
		MS2C_GuildWarHold mS2C_GuildWarHold = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarHold), stream) as MS2C_GuildWarHold;
		if (mS2C_GuildWarHold.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarHold.Result);
			return;
		}
		if (this.LocalClientMember == null)
		{
			this.LocalClientMember = new GuildWarClientTeamMember();
		}
		if (this.LocalClientMember != null)
		{
			this.LocalClientMember.Member = mS2C_GuildWarHold.Player;
			this.LocalClientMember.Data = Tools.LocalPlayerToRemote();
		}
		if (0 <= mS2C_GuildWarHold.SlotIndex - 1 && mS2C_GuildWarHold.SlotIndex - 1 < this.StrongHoldMembers.Count)
		{
			this.StrongHoldMembers[mS2C_GuildWarHold.SlotIndex - 1] = this.LocalClientMember;
		}
		if (this.LocalMemberUpDateEvent != null)
		{
			this.LocalMemberUpDateEvent();
		}
	}

	private void OnMsgGuildWarKickHold(MemoryStream stream)
	{
		MS2C_GuildWarKickHold mS2C_GuildWarKickHold = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarKickHold), stream) as MS2C_GuildWarKickHold;
		if (mS2C_GuildWarKickHold.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarKickHold.Result);
			return;
		}
	}

	private void OnMsgGuildWarQuitHold(MemoryStream stream)
	{
		MS2C_GuildWarQuitHold mS2C_GuildWarQuitHold = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarQuitHold), stream) as MS2C_GuildWarQuitHold;
		if (mS2C_GuildWarQuitHold.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarQuitHold.Result);
			return;
		}
		this.LocalClientMember = new GuildWarClientTeamMember();
		if (this.LocalClientMember != null)
		{
			this.LocalClientMember.Member = mS2C_GuildWarQuitHold.Player;
			this.LocalClientMember.Data = Tools.LocalPlayerToRemote();
		}
		if (0 <= mS2C_GuildWarQuitHold.SlotIndex - 1 && mS2C_GuildWarQuitHold.SlotIndex - 1 < this.StrongHoldMembers.Count && this.StrongHoldMembers[mS2C_GuildWarQuitHold.SlotIndex - 1] != null)
		{
			this.StrongHoldMembers[mS2C_GuildWarQuitHold.SlotIndex - 1].Member = null;
		}
		if (mS2C_GuildWarQuitHold.Kick)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildCraft74", 0f, 0f);
		}
		if (this.LocalMemberUpDateEvent != null)
		{
			this.LocalMemberUpDateEvent();
		}
	}

	private void OnMsgQueryCombatRecord(MemoryStream stream)
	{
		MS2C_GuildWarQueryCombatRecord mS2C_GuildWarQueryCombatRecord = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarQueryCombatRecord), stream) as MS2C_GuildWarQueryCombatRecord;
		if (mS2C_GuildWarQueryCombatRecord.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarQueryCombatRecord.Result);
			return;
		}
		this.BattleRecords = mS2C_GuildWarQueryCombatRecord.records;
		if (this.GetBattleRecordEvent != null)
		{
			this.GetBattleRecordEvent();
		}
	}

	private void OnMsgGuildWarReqRevive(MemoryStream stream)
	{
		MS2C_GuildWarReqRevive mS2C_GuildWarReqRevive = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarReqRevive), stream) as MS2C_GuildWarReqRevive;
		if (mS2C_GuildWarReqRevive.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarReqRevive.Result);
			return;
		}
		GameUIManager.mInstance.ShowMessageTipByKey("guildCraft67", 0f, 0f);
		if (this.LocalClientMember == null)
		{
			this.LocalClientMember = new GuildWarClientTeamMember();
		}
		if (this.LocalClientMember != null)
		{
			this.LocalClientMember.Member = mS2C_GuildWarReqRevive.Player;
			this.LocalClientMember.Data = Tools.LocalPlayerToRemote();
			if (this.LocalMemberUpDateEvent != null)
			{
				this.LocalMemberUpDateEvent();
			}
		}
	}

	private void OnMsgGuildWarRecoverHP(MemoryStream stream)
	{
		MS2C_GuildWarRecoverHP mS2C_GuildWarRecoverHP = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarRecoverHP), stream) as MS2C_GuildWarRecoverHP;
		if (mS2C_GuildWarRecoverHP.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarRecoverHP.Result);
			return;
		}
		if (this.LocalClientMember == null)
		{
			this.LocalClientMember = new GuildWarClientTeamMember();
		}
		if (this.LocalClientMember != null)
		{
			this.LocalClientMember.Member = mS2C_GuildWarRecoverHP.Player;
			this.LocalClientMember.Data = Tools.LocalPlayerToRemote();
			if (this.LocalMemberUpDateEvent != null)
			{
				this.LocalMemberUpDateEvent();
			}
			if (this.GuildWarRecoverHpEvent != null)
			{
				this.GuildWarRecoverHpEvent();
			}
		}
	}

	private void OnMsgGuildWarPlayerUpdate(MemoryStream stream)
	{
		MS2C_GuildWarPlayerUpdate mS2C_GuildWarPlayerUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarPlayerUpdate), stream) as MS2C_GuildWarPlayerUpdate;
		if (this.LocalClientMember == null)
		{
			return;
		}
		if (this.LocalClientMember.Member != null && this.LocalClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Fighting && mS2C_GuildWarPlayerUpdate.Player != null && mS2C_GuildWarPlayerUpdate.Player.Status == EGuardWarTeamMemState.EGWTMS_Dead && this.GuildWarPlayerDeadEvent != null)
		{
			this.GuildWarPlayerDeadEvent();
		}
		this.LocalClientMember.Member = mS2C_GuildWarPlayerUpdate.Player;
		if (this.LocalMemberUpDateEvent != null)
		{
			this.LocalMemberUpDateEvent();
		}
	}

	private void OnMsgGuildWarStrongholdUpdate(MemoryStream stream)
	{
		MS2C_GuildWarStrongholdUpdate mS2C_GuildWarStrongholdUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarStrongholdUpdate), stream) as MS2C_GuildWarStrongholdUpdate;
		if (1 <= mS2C_GuildWarStrongholdUpdate.SlotIndex && mS2C_GuildWarStrongholdUpdate.SlotIndex <= this.StrongHoldMembers.Count && this.StrongHold != null && mS2C_GuildWarStrongholdUpdate.StrongholdID == this.StrongHold.ID)
		{
			if (mS2C_GuildWarStrongholdUpdate.HealthPct > 0)
			{
				if (this.StrongHoldMembers[mS2C_GuildWarStrongholdUpdate.SlotIndex - 1] != null && this.StrongHoldMembers[mS2C_GuildWarStrongholdUpdate.SlotIndex - 1].Member != null)
				{
					this.StrongHoldMembers[mS2C_GuildWarStrongholdUpdate.SlotIndex - 1].Member.HealthPct = mS2C_GuildWarStrongholdUpdate.HealthPct;
					if (this.StrongholdUpdateEvent != null)
					{
						this.StrongholdUpdateEvent();
					}
				}
			}
			else if (mS2C_GuildWarStrongholdUpdate.Member != null)
			{
				this.StrongHoldMembers[mS2C_GuildWarStrongholdUpdate.SlotIndex - 1] = mS2C_GuildWarStrongholdUpdate.Member;
				if (this.StrongholdUpdateEvent != null)
				{
					this.StrongholdUpdateEvent();
				}
			}
		}
	}

	private void OnMsgGuildWarRewardUpdate(MemoryStream stream)
	{
		MS2C_GuildWarRewardUpdate mS2C_GuildWarRewardUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarRewardUpdate), stream) as MS2C_GuildWarRewardUpdate;
		if (this.mGWPlayerData == null)
		{
			this.mGWPlayerData = new GuildWarPlayerData();
		}
		if (this.mGWPlayerData != null)
		{
			this.mGWPlayerData.Reward = (EGuildWarReward)mS2C_GuildWarRewardUpdate.Reward;
			if (this.GuildWarRewardUpdateEvent != null)
			{
				this.GuildWarRewardUpdateEvent();
			}
		}
	}

	private void OnMsgGuildWarEnd(MemoryStream stream)
	{
		MS2C_GuildWarEnd mS2C_GuildWarEnd = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarEnd), stream) as MS2C_GuildWarEnd;
		this.mGWKillRankData = mS2C_GuildWarEnd.WarData;
		if (this.mWarStateInfo != null)
		{
			if (this.mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
			{
				this.mWarStateInfo.mWarState = EGuildWarState.EGWS_FinalFourEnd;
			}
			else if (this.mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				this.mWarStateInfo.mWarState = EGuildWarState.EGWS_FinalEnd;
			}
		}
		if (this.mGWEnterData != null && mS2C_GuildWarEnd.WarData.Winner != EGuildWarTeamId.EGWTI_None)
		{
			this.mGWEnterData.Winner = mS2C_GuildWarEnd.WarData.Winner;
		}
		if (this.GuildWarEndEvent != null)
		{
			this.GuildWarEndEvent();
		}
	}

	private void OnMsgGuildWarUpdate(MemoryStream stream)
	{
		this.mReqGWUpdate = false;
		MS2C_GuildWarUpdate mS2C_GuildWarUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarUpdate), stream) as MS2C_GuildWarUpdate;
		if (this.mWarStateInfo == null)
		{
			return;
		}
		this.mWarStateInfo.mWarState = mS2C_GuildWarUpdate.Status;
		this.mWarStateInfo.mTimeStamp = mS2C_GuildWarUpdate.Timestamp;
		this.mWarStateInfo.ResetStateInfo(mS2C_GuildWarUpdate.Status, mS2C_GuildWarUpdate.Timestamp, null, null);
		if (this.mGWEnterData != null && this.mGWEnterData.WarID == mS2C_GuildWarUpdate.WarData.WarID)
		{
			if (mS2C_GuildWarUpdate.WarData.Red != null && mS2C_GuildWarUpdate.WarData.Red.KillNum != 0u)
			{
				this.mGWEnterData.Red.KillNum = mS2C_GuildWarUpdate.WarData.Red.KillNum;
			}
			if (mS2C_GuildWarUpdate.WarData.Blue != null && mS2C_GuildWarUpdate.WarData.Blue.KillNum != 0u)
			{
				this.mGWEnterData.Blue.KillNum = mS2C_GuildWarUpdate.WarData.Blue.KillNum;
			}
			if (mS2C_GuildWarUpdate.WarData.Red != null && mS2C_GuildWarUpdate.WarData.Red.Score != 0)
			{
				this.mGWEnterData.Red.Score = mS2C_GuildWarUpdate.WarData.Red.Score;
			}
			if (mS2C_GuildWarUpdate.WarData.Blue != null && mS2C_GuildWarUpdate.WarData.Blue.Score != 0)
			{
				this.mGWEnterData.Blue.Score = mS2C_GuildWarUpdate.WarData.Blue.Score;
			}
			if (mS2C_GuildWarUpdate.WarData.Winner != EGuildWarTeamId.EGWTI_None)
			{
				this.mGWEnterData.Winner = mS2C_GuildWarUpdate.WarData.Winner;
			}
			for (int i = 0; i < mS2C_GuildWarUpdate.WarData.Strongholds.Count; i++)
			{
				if (this.mGWEnterData.Strongholds[mS2C_GuildWarUpdate.WarData.Strongholds[i].ID - 1].ID == mS2C_GuildWarUpdate.WarData.Strongholds[i].ID)
				{
					this.mGWEnterData.Strongholds[mS2C_GuildWarUpdate.WarData.Strongholds[i].ID - 1] = mS2C_GuildWarUpdate.WarData.Strongholds[i];
				}
				if (this.StrongHold != null && this.StrongHold.ID == mS2C_GuildWarUpdate.WarData.Strongholds[i].ID)
				{
					this.StrongHold = mS2C_GuildWarUpdate.WarData.Strongholds[i];
					if (this.mGWEnterData != null && this.IsGuanZhanMember(this.mGWEnterData.WarID))
					{
						for (int j = 0; j < this.StrongHold.Slots.Count; j++)
						{
							GuildWarStrongholdSlot guildWarStrongholdSlot = this.StrongHold.Slots[j];
							if (guildWarStrongholdSlot != null && guildWarStrongholdSlot.Status == EGuardWarStrongholdSlotState.EGWPSS_Empty && j < this.StrongHoldMembers.Count && this.StrongHoldMembers[j] != null && this.StrongHoldMembers[j].Member != null)
							{
								this.StrongHoldMembers[j].Member = null;
								break;
							}
						}
					}
				}
			}
			if (mS2C_GuildWarUpdate.Scores != null && this.ScoreAddEvent != null)
			{
				this.ScoreAddEvent(mS2C_GuildWarUpdate.Scores);
			}
		}
		if (this.CastleUpdateEvent != null)
		{
			this.CastleUpdateEvent();
		}
	}

	public void RequestGuildWarFuHuo()
	{
		GuildWarStateInfo guildWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (guildWarStateInfo == null)
		{
			return;
		}
		if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
			if (localClientMember == null)
			{
				return;
			}
			GuildWarClient guildWarClient = Globals.Instance.Player.GuildSystem.mGWEnterData;
			if (guildWarClient == null)
			{
				return;
			}
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
			{
				return;
			}
			if (localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Dead)
			{
				MC2S_GuildWarReqRevive mC2S_GuildWarReqRevive = new MC2S_GuildWarReqRevive();
				mC2S_GuildWarReqRevive.WarID = guildWarClient.WarID;
				mC2S_GuildWarReqRevive.TeamID = selfTeamFlag;
				Globals.Instance.CliSession.Send(999, mC2S_GuildWarReqRevive);
			}
		}
	}

	public bool IsGuildMinesOpen()
	{
		return this.GuildMines != null && this.GuildMines.Status != 0;
	}

	public bool HasGuildWarReward()
	{
		return this.mGWPlayerData != null && this.mGWPlayerData.Reward == EGuildWarReward.EGWR_NotTake;
	}

	private void OnMsgQueryOrePillageData(MemoryStream stream)
	{
		MS2C_QueryOrePillageData mS2C_QueryOrePillageData = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryOrePillageData), stream) as MS2C_QueryOrePillageData;
		if (mS2C_QueryOrePillageData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_QueryOrePillageData.Result);
			return;
		}
		this.GuildMines = mS2C_QueryOrePillageData;
		if (this.QueryOrePillageDataEvent != null)
		{
			this.QueryOrePillageDataEvent();
		}
	}

	public bool CanTakeMinesReward(int ores)
	{
		return this.GuildMines != null && Globals.Instance.Player.GuildSystem.GuildMines.RewardTime > Globals.Instance.Player.GetTimeStamp() && this.GuildMines.OreAmount >= ores;
	}

	public bool IsTakenMinesReward(int ID)
	{
		return this.GuildMines != null && (this.GuildMines.RewardFlag & 1 << ID) != 0;
	}

	public bool IsMinesRecordRed()
	{
		return (Globals.Instance.Player.Data.RedFlag & 16384) != 0;
	}

	private void OnMsgTakeOreReward(MemoryStream stream)
	{
		MS2C_TakeOreReward mS2C_TakeOreReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeOreReward), stream) as MS2C_TakeOreReward;
		if (mS2C_TakeOreReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_TakeOreReward.Result);
			return;
		}
		if (this.TakeOreRewardEvent != null)
		{
			this.TakeOreRewardEvent(mS2C_TakeOreReward.Index);
		}
	}

	private void OnMsgUpdateOrePillageData(MemoryStream stream)
	{
		MS2C_UpdateOrePillageData mS2C_UpdateOrePillageData = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateOrePillageData), stream) as MS2C_UpdateOrePillageData;
		if (this.GuildMines != null)
		{
			if (mS2C_UpdateOrePillageData.PillageCount != 0)
			{
				if (mS2C_UpdateOrePillageData.PillageCount == -1)
				{
					this.GuildMines.PillageCount = 0;
				}
				else
				{
					this.GuildMines.PillageCount = mS2C_UpdateOrePillageData.PillageCount;
				}
			}
			if (mS2C_UpdateOrePillageData.RewardFlag != 0)
			{
				if (mS2C_UpdateOrePillageData.RewardFlag == -1)
				{
					this.GuildMines.RewardFlag = 0;
				}
				else
				{
					this.GuildMines.RewardFlag = mS2C_UpdateOrePillageData.RewardFlag;
				}
			}
			if (mS2C_UpdateOrePillageData.PillageCountTimestamp != 0)
			{
				if (mS2C_UpdateOrePillageData.PillageCountTimestamp == -1)
				{
					this.GuildMines.PillageCountTimestamp = 0;
				}
				else
				{
					this.GuildMines.PillageCountTimestamp = mS2C_UpdateOrePillageData.PillageCountTimestamp;
				}
			}
			if (mS2C_UpdateOrePillageData.BuyPillageCount != 0)
			{
				if (mS2C_UpdateOrePillageData.BuyPillageCount == -1)
				{
					this.GuildMines.BuyPillageCount = 0;
				}
				else
				{
					this.GuildMines.BuyPillageCount = mS2C_UpdateOrePillageData.BuyPillageCount;
				}
			}
		}
		if (this.UpdateOrePillageDataEvent != null)
		{
			this.UpdateOrePillageDataEvent();
		}
	}

	private void OnMsgQueryMyOreData(MemoryStream stream)
	{
		MS2C_QueryMyOreData mS2C_QueryMyOreData = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryMyOreData), stream) as MS2C_QueryMyOreData;
		if (mS2C_QueryMyOreData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_QueryMyOreData.Result);
			return;
		}
		this.MyOreData = mS2C_QueryMyOreData;
		if (this.QueryMyOreDataEvent != null)
		{
			this.QueryMyOreDataEvent();
		}
	}

	private void OnMsgGuildWarKillRank(MemoryStream stream)
	{
		MS2C_GuildWarKillRank mS2C_GuildWarKillRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarKillRank), stream) as MS2C_GuildWarKillRank;
		this.mGWKillRankData = mS2C_GuildWarKillRank.WarData;
		if (mS2C_GuildWarKillRank.WarData.Winner != EGuildWarTeamId.EGWTI_None)
		{
			this.mGWEnterData.Winner = mS2C_GuildWarKillRank.WarData.Winner;
		}
		if (this.QueryGWKillRankEvent != null)
		{
			this.QueryGWKillRankEvent();
		}
	}

	private void OnMsgCreateGuild(MemoryStream stream)
	{
		MS2C_CreateGuild mS2C_CreateGuild = Serializer.NonGeneric.Deserialize(typeof(MS2C_CreateGuild), stream) as MS2C_CreateGuild;
		if (mS2C_CreateGuild.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_CreateGuild.Result);
			return;
		}
		GUIGuildManageScene session = GameUIManager.mInstance.GetSession<GUIGuildManageScene>();
		if (session == null)
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
		}
	}

	private void OnMsgOrePillageStart(MemoryStream stream)
	{
		MS2C_OrePillageStart mS2C_OrePillageStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_OrePillageStart), stream) as MS2C_OrePillageStart;
		if (mS2C_OrePillageStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_OrePillageStart.Result);
			return;
		}
		GameAnalytics.OnStartScene(Globals.Instance.AttDB.SceneDict.GetInfo(GameConst.GetInt32(119)));
		Globals.Instance.ActorMgr.SetServerData(mS2C_OrePillageStart.Key, mS2C_OrePillageStart.Data3);
		Globals.Instance.Player.TeamSystem.SetRemotePlayerData(mS2C_OrePillageStart.Data2, mS2C_OrePillageStart.Data);
		GameUIManager.mInstance.LoadScene(GameConst.GetInt32(119));
	}

	private void OnMsgGuildWarFightBegin(MemoryStream stream)
	{
		MS2C_GuildWarFightBegin mS2C_GuildWarFightBegin = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarFightBegin), stream) as MS2C_GuildWarFightBegin;
		if (mS2C_GuildWarFightBegin.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarFightBegin.Result);
			return;
		}
		GameAnalytics.OnStartScene(Globals.Instance.AttDB.SceneDict.GetInfo(GameConst.GetInt32(121)));
		Globals.Instance.ActorMgr.SetServerData(mS2C_GuildWarFightBegin.Key, mS2C_GuildWarFightBegin.Data3);
		Globals.Instance.Player.TeamSystem.SetRemotePlayerData(mS2C_GuildWarFightBegin.Data, mS2C_GuildWarFightBegin.Data2);
		this.MyHpPct = mS2C_GuildWarFightBegin.MyHpPct;
		this.EnemyHpPct = mS2C_GuildWarFightBegin.EnemyHpPct;
		GameUIManager.mInstance.LoadScene(GameConst.GetInt32(121));
	}

	public EGuildWarTeamId GetSelfTeamFlag()
	{
		GuildWarClient guildWarClient = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (guildWarClient == null)
		{
			return EGuildWarTeamId.EGWTI_None;
		}
		if (Globals.Instance.Player.GuildSystem.Guild.ID == guildWarClient.Red.GuildID)
		{
			return EGuildWarTeamId.EGWTI_Red;
		}
		if (Globals.Instance.Player.GuildSystem.Guild.ID == guildWarClient.Blue.GuildID)
		{
			return EGuildWarTeamId.EGWTI_Blue;
		}
		return EGuildWarTeamId.EGWTI_None;
	}

	public GuildWarStronghold GetSelfStrongHold()
	{
		if (Globals.Instance.Player.GuildSystem.mGWEnterData != null && this.LocalClientMember != null && this.LocalClientMember.Member != null && this.LocalClientMember.Member.StrongholdId != 0)
		{
			int num = this.LocalClientMember.Member.StrongholdId - 1;
			if (num < Globals.Instance.Player.GuildSystem.mGWEnterData.Strongholds.Count)
			{
				return Globals.Instance.Player.GuildSystem.mGWEnterData.Strongholds[num];
			}
		}
		return null;
	}

	public int GetSelfStrongHoldIndex()
	{
		if (Globals.Instance.Player.GuildSystem.mGWEnterData != null && this.LocalClientMember != null && this.LocalClientMember.Member != null && this.LocalClientMember.Member.StrongholdId != 0)
		{
			return this.LocalClientMember.Member.StrongholdId;
		}
		return 0;
	}

	public GuildWarClientTeamMember GetStrongHoldMember(ulong playerID)
	{
		if (this.StrongHoldMembers != null)
		{
			for (int i = 0; i < this.StrongHoldMembers.Count; i++)
			{
				if (this.StrongHoldMembers[i] != null && this.StrongHoldMembers[i].Member != null && this.StrongHoldMembers[i].Member.PlayerID == playerID)
				{
					return this.StrongHoldMembers[i];
				}
			}
		}
		return null;
	}

	public bool IsGuarding()
	{
		return this.LocalClientMember != null && this.LocalClientMember.Member != null && this.LocalClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Guard;
	}

	public int GetGuardIndex()
	{
		if (this.IsGuarding())
		{
			return this.LocalClientMember.Member.SlotId;
		}
		return 0;
	}

	private void OnMsgGuildSignRecord(MemoryStream stream)
	{
		MS2C_QueryGuildSignRecord mS2C_QueryGuildSignRecord = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryGuildSignRecord), stream) as MS2C_QueryGuildSignRecord;
		if (mS2C_QueryGuildSignRecord.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_QueryGuildSignRecord.Result);
			return;
		}
		if (this.mSignRecordVersion != mS2C_QueryGuildSignRecord.Version)
		{
			this.mSignRecordVersion = mS2C_QueryGuildSignRecord.Version;
			this.mSignRecords = mS2C_QueryGuildSignRecord.Data;
		}
		if (this.SignRecordsEvent != null)
		{
			this.SignRecordsEvent();
		}
	}

	private void DoSendGuildWarMsg(string tipMsg)
	{
		if (!string.IsNullOrEmpty(tipMsg))
		{
			if (this.mInteractionMsgs.Count >= 30)
			{
				this.mInteractionMsgs.RemoveAt(0);
			}
			this.mInteractionMsgs.Add(tipMsg);
			if (this.GuildWarPromptEvent != null)
			{
				this.GuildWarPromptEvent(tipMsg);
			}
		}
	}

	private void OnMsgGuildWarPrompt(MemoryStream stream)
	{
		MS2C_GuildWarPrompt mS2C_GuildWarPrompt = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarPrompt), stream) as MS2C_GuildWarPrompt;
		GuildWarStateInfo guildWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (guildWarStateInfo == null)
		{
			return;
		}
		string tipMsg = string.Empty;
		if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_Kill1)
		{
			if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				if (mS2C_GuildWarPrompt.Value5 == 0)
				{
					GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
					if (info != null)
					{
						string text = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						string text2 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[0066ff]" : "[ff0000]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog14", new object[]
						{
							text,
							mS2C_GuildWarPrompt.Value1,
							info.StrongholdName,
							text2,
							mS2C_GuildWarPrompt.Value2
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
				}
				else
				{
					GuildInfo info2 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
					if (info2 != null)
					{
						string text3 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[0066ff]" : "[ff0000]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog15", new object[]
						{
							text3,
							mS2C_GuildWarPrompt.Value2,
							info2.StrongholdName
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					if (Globals.Instance.Player.Data.Name.Equals(mS2C_GuildWarPrompt.Value1) && this.GuildWarDefendSurEvent != null)
					{
						this.GuildWarDefendSurEvent();
					}
				}
				if (mS2C_GuildWarPrompt.Value4 != 1)
				{
					if (mS2C_GuildWarPrompt.Value4 == 2)
					{
						string text4 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog1", new object[]
						{
							text4,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					else if (mS2C_GuildWarPrompt.Value4 == 3)
					{
						string text5 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog2", new object[]
						{
							text5,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					else if (mS2C_GuildWarPrompt.Value4 == 4)
					{
						string text6 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog16", new object[]
						{
							text6,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					else if (mS2C_GuildWarPrompt.Value4 == 5)
					{
						string text7 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog17", new object[]
						{
							text7,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					else if (mS2C_GuildWarPrompt.Value4 == 6)
					{
						string text8 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog3", new object[]
						{
							text8,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					else if (mS2C_GuildWarPrompt.Value4 == 7)
					{
						string text9 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog4", new object[]
						{
							text9,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
					else if (mS2C_GuildWarPrompt.Value4 > 7)
					{
						string text10 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog5", new object[]
						{
							text10,
							mS2C_GuildWarPrompt.Value1
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
				}
			}
		}
		else if (mS2C_GuildWarPrompt.ID != EGuildWarPromptType.EWNT_KillAll)
		{
			if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_PositionLost)
			{
				if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
				{
					EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
					if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
					{
						return;
					}
					if (selfTeamFlag == (EGuildWarTeamId)mS2C_GuildWarPrompt.Value4)
					{
						GuildInfo info3 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
						if (info3 != null)
						{
							tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog11", new object[]
							{
								info3.StrongholdName
							});
							this.DoSendGuildWarMsg(tipMsg);
						}
					}
					else
					{
						GuildInfo info4 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
						if (info4 != null)
						{
							tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog7", new object[]
							{
								info4.StrongholdName
							});
							this.DoSendGuildWarMsg(tipMsg);
						}
					}
				}
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_PositionChanging)
			{
				if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
				{
					GuildInfo info5 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
					if (info5 != null)
					{
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog8", new object[]
						{
							info5.StrongholdName
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
				}
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_PositionWeak)
			{
				if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
				{
					GuildInfo info6 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
					if (info6 != null)
					{
						tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog9", new object[]
						{
							info6.StrongholdName
						});
						this.DoSendGuildWarMsg(tipMsg);
					}
				}
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_WarBegin)
			{
				if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
				{
					int timecount = guildWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
					string text11 = Tools.FormatTimeStr(timecount, false, false);
					tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog10", new object[]
					{
						text11
					});
					this.DoSendGuildWarMsg(tipMsg);
				}
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_EnterProtected)
			{
				if (guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || guildWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
				{
					EGuildWarTeamId selfTeamFlag2 = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
					if (selfTeamFlag2 == EGuildWarTeamId.EGWTI_None)
					{
						return;
					}
					if (selfTeamFlag2 == (EGuildWarTeamId)mS2C_GuildWarPrompt.Value4)
					{
						GuildInfo info7 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
						if (info7 != null)
						{
							string text12 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
							tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog6", new object[]
							{
								text12,
								mS2C_GuildWarPrompt.Value1,
								info7.StrongholdName
							});
							this.DoSendGuildWarMsg(tipMsg);
						}
					}
					else
					{
						GuildInfo info8 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
						if (info8 != null)
						{
							tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog12", new object[]
							{
								info8.StrongholdName
							});
							this.DoSendGuildWarMsg(tipMsg);
						}
					}
				}
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_Defence)
			{
				GuildInfo info9 = Globals.Instance.AttDB.GuildDict.GetInfo(mS2C_GuildWarPrompt.Value3);
				if (info9 != null)
				{
					string text13 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
					tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog13", new object[]
					{
						text13,
						mS2C_GuildWarPrompt.Value1,
						info9.StrongholdName
					});
					this.DoSendGuildWarMsg(tipMsg);
				}
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_FirstBlood)
			{
				string text14 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
				string text15 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[0066ff]" : "[ff0000]";
				tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog18", new object[]
				{
					text14,
					mS2C_GuildWarPrompt.Value1,
					text15,
					mS2C_GuildWarPrompt.Value2
				});
				this.DoSendGuildWarMsg(tipMsg);
			}
			else if (mS2C_GuildWarPrompt.ID == EGuildWarPromptType.EWNT_EndKiller)
			{
				string text16 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[ff0000]" : "[0066ff]";
				string text17 = (mS2C_GuildWarPrompt.Value6 != 3) ? "[0066ff]" : "[ff0000]";
				if (mS2C_GuildWarPrompt.Value3 == 3)
				{
					tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog19", new object[]
					{
						text16,
						mS2C_GuildWarPrompt.Value1,
						text17,
						mS2C_GuildWarPrompt.Value2
					});
				}
				else if (mS2C_GuildWarPrompt.Value3 == 6)
				{
					tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog20", new object[]
					{
						text16,
						mS2C_GuildWarPrompt.Value1,
						text17,
						mS2C_GuildWarPrompt.Value2
					});
				}
				else if (mS2C_GuildWarPrompt.Value3 == 7)
				{
					tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog21", new object[]
					{
						text16,
						mS2C_GuildWarPrompt.Value1,
						text17,
						mS2C_GuildWarPrompt.Value2
					});
				}
				else if (mS2C_GuildWarPrompt.Value3 > 7)
				{
					tipMsg = Singleton<StringManager>.Instance.GetString("craftKillLog22", new object[]
					{
						text16,
						mS2C_GuildWarPrompt.Value1,
						text17,
						mS2C_GuildWarPrompt.Value2
					});
				}
				this.DoSendGuildWarMsg(tipMsg);
			}
		}
	}

	public void ClearGuildWarPromptMsg()
	{
		if (this.mInteractionMsgs != null)
		{
			this.mInteractionMsgs.Clear();
		}
	}

	private void OnEquipPetEvent(int slotIndex)
	{
		if (this.LocalClientMember != null)
		{
			this.LocalClientMember.Data = Tools.LocalPlayerToRemote();
		}
	}

	public bool IsCanZhanMember(EGuildWarId warID)
	{
		if (this.mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = this.Guild.ID;
		for (int i = 0; i < this.mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = this.mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && guildWarClient.WarID == warID && (iD == guildWarClient.Red.GuildID || iD == guildWarClient.Blue.GuildID))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsGuanZhanMember(EGuildWarId warID)
	{
		if (this.mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = this.Guild.ID;
		for (int i = 0; i < this.mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = this.mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && guildWarClient.WarID == warID && iD != guildWarClient.Red.GuildID && iD != guildWarClient.Blue.GuildID)
			{
				return true;
			}
		}
		return false;
	}

	private void OnMsgGuildWarGetSupportInfo(MemoryStream stream)
	{
		MS2C_GuildWarGetSupportInfo mS2C_GuildWarGetSupportInfo = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarGetSupportInfo), stream) as MS2C_GuildWarGetSupportInfo;
		if (mS2C_GuildWarGetSupportInfo.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarGetSupportInfo.Result);
			return;
		}
		this.BattleSupportInfo = mS2C_GuildWarGetSupportInfo.SupportInfo;
		if (this.GuildWarBattleInfoEvent != null)
		{
			this.GuildWarBattleInfoEvent();
		}
	}

	private void OnMsgGuildWarSupport(MemoryStream stream)
	{
		MS2C_GuildWarSupport mS2C_GuildWarSupport = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarSupport), stream) as MS2C_GuildWarSupport;
		if (mS2C_GuildWarSupport.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarSupport.Result);
			return;
		}
		this.BattleSupportInfo = mS2C_GuildWarSupport.SupportInfo;
		if (this.GuildWarSupportEvent != null)
		{
			this.GuildWarSupportEvent();
		}
	}
}
