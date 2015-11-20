using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class WorldBossSubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public delegate void BossRespawnCallback(int slot, MonsterInfo info);

	public delegate void BossDeadCallback(int slot, MonsterInfo info, string playerName);

	public delegate void DoBossDamageCallback(int slot, MonsterInfo info, string playerName, long damage, int type);

	public WorldBossSubSystem.VoidCallback GetBossDataEvent;

	public WorldBossSubSystem.BossRespawnCallback BossRespawnEvent;

	public WorldBossSubSystem.BossDeadCallback BossDeadEvent;

	public WorldBossSubSystem.DoBossDamageCallback DoBossDamageEvent;

	public WorldBossSubSystem.VoidCallback DamageRankEvent;

	public WorldBossSubSystem.VoidCallback DoDamageFailEvent;

	private BossData[] bossData = new BossData[6];

	private MonsterInfo[] bossInfo = new MonsterInfo[6];

	private RankData[] damageRank = new RankData[32];

	private uint bossRankVersionID;

	private int mHasReward;

	public int AutoResurrectTimeStamp;

	private List<int> mFDSRewardTaked;

	public int Status
	{
		get;
		private set;
	}

	public int TimeStamp
	{
		get;
		private set;
	}

	public int Rank
	{
		get;
		private set;
	}

	public long TotalDamage
	{
		get;
		private set;
	}

	public int TotalCount
	{
		get;
		private set;
	}

	public int KillElapsedTime
	{
		get;
		private set;
	}

	public int CurSlot
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(615, new ClientSession.MsgHandler(this.OnMsgGetBossData));
		Globals.Instance.CliSession.Register(616, new ClientSession.MsgHandler(this.OnMsgBossRespawn));
		Globals.Instance.CliSession.Register(617, new ClientSession.MsgHandler(this.OnMsgBossDead));
		Globals.Instance.CliSession.Register(619, new ClientSession.MsgHandler(this.OnMsgDoBossDamage));
		Globals.Instance.CliSession.Register(620, new ClientSession.MsgHandler(this.OnMsgBroadcastDamage));
		Globals.Instance.CliSession.Register(621, new ClientSession.MsgHandler(this.OnMsgBroadcastDamageRank));
		Globals.Instance.CliSession.Register(625, new ClientSession.MsgHandler(this.OnMsgWorldBossStart));
		Globals.Instance.CliSession.Register(205, new ClientSession.MsgHandler(this.OnMsgUpdateFDSReward));
	}

	public void LoadData(List<int> fdsReward)
	{
		this.mFDSRewardTaked = fdsReward;
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Status = 0;
		this.TimeStamp = 0;
		for (int i = 0; i < 6; i++)
		{
			this.bossData[i] = null;
			this.bossInfo[i] = null;
		}
		for (int j = 0; j < this.damageRank.Length; j++)
		{
			this.damageRank[j] = null;
		}
		this.bossRankVersionID = 0u;
		this.Rank = 0;
		this.TotalDamage = 0L;
		this.TotalCount = 0;
		this.KillElapsedTime = 0;
		this.mHasReward = 0;
		this.CurSlot = 0;
		this.AutoResurrectTimeStamp = 0;
	}

	public BossData GetBossData(int slot)
	{
		if (slot <= 0 || slot >= 6)
		{
			return null;
		}
		if (this.bossData[slot] == null)
		{
			this.bossData[slot] = new BossData();
			this.bossData[slot].Slot = slot;
		}
		return this.bossData[slot];
	}

	public MonsterInfo GetBossInfo(int slot)
	{
		if (slot <= 0 || slot >= 6)
		{
			return null;
		}
		return this.bossInfo[slot];
	}

	public RankData GetRankData(int rank)
	{
		if (rank <= 0 || rank >= 31)
		{
			return null;
		}
		return this.damageRank[rank];
	}

	public RankData GetKillerData()
	{
		if (this.Status == 1)
		{
			return null;
		}
		return (this.damageRank[0] == null || this.damageRank[0].Rank > 0) ? this.damageRank[0] : null;
	}

	public RankData GetLocalData()
	{
		return this.damageRank[0];
	}

	public void SetCurSlot(int slot)
	{
		if (slot <= 0 || slot >= 6)
		{
			Debug.LogError(new object[]
			{
				"slot error"
			});
		}
		this.CurSlot = slot;
	}

	public BossData GetCurBossData()
	{
		return this.GetBossData(this.CurSlot);
	}

	public void UpdateCurBossData(int infoID, float healthPct)
	{
		BossData bossData = this.GetBossData(this.CurSlot);
		if (bossData == null)
		{
			return;
		}
		bossData.HealthPct = healthPct;
		if (bossData.InfoID != infoID)
		{
			bossData.InfoID = infoID;
			this.bossInfo[this.CurSlot] = Globals.Instance.AttDB.MonsterDict.GetInfo(infoID);
			if (this.bossInfo[this.CurSlot] == null)
			{
				Debug.LogError(new object[]
				{
					string.Format("MonsterDict.GetInfo error, ID = {0}", infoID)
				});
			}
		}
	}

	public bool WorldBossIsOver()
	{
		if (this.Status == 2)
		{
			return true;
		}
		if (Globals.Instance.Player.GetTimeStamp() >= this.TimeStamp)
		{
			return true;
		}
		BossData bossData = this.GetBossData(5);
		MonsterInfo monsterInfo = this.GetBossInfo(5);
		return bossData != null && monsterInfo != null && bossData.HealthPct == 0f;
	}

	public void OnMsgGetBossData(MemoryStream stream)
	{
		MS2C_GetBossData mS2C_GetBossData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetBossData), stream) as MS2C_GetBossData;
		if (mS2C_GetBossData.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (this.Status != mS2C_GetBossData.Status || this.TimeStamp != mS2C_GetBossData.TimeStamp)
		{
			this.Status = mS2C_GetBossData.Status;
			this.TimeStamp = mS2C_GetBossData.TimeStamp;
			if (this.Status == 1)
			{
				for (int i = 0; i < 6; i++)
				{
					this.bossData[i] = null;
					this.bossInfo[i] = null;
				}
				for (int j = 0; j < mS2C_GetBossData.Data.Count; j++)
				{
					if (mS2C_GetBossData.Data[j].InfoID != 0 && mS2C_GetBossData.Data[j].Slot >= 0 && mS2C_GetBossData.Data[j].Slot < 6)
					{
						this.bossData[mS2C_GetBossData.Data[j].Slot] = mS2C_GetBossData.Data[j];
						if (this.bossInfo[mS2C_GetBossData.Data[j].Slot] == null || this.bossInfo[mS2C_GetBossData.Data[j].Slot].ID != mS2C_GetBossData.Data[j].InfoID)
						{
							this.bossInfo[mS2C_GetBossData.Data[j].Slot] = Globals.Instance.AttDB.MonsterDict.GetInfo(mS2C_GetBossData.Data[j].InfoID);
							if (this.bossInfo[mS2C_GetBossData.Data[j].Slot] == null)
							{
								Debug.LogError(new object[]
								{
									string.Format("MonsterDict.GetInfo error, ID = {0}", mS2C_GetBossData.Data[j].InfoID)
								});
							}
						}
					}
				}
			}
			else if (this.Status == 2)
			{
				this.KillElapsedTime = mS2C_GetBossData.KillElapsedTime;
			}
			if (mS2C_GetBossData.DeadTimestamp > this.AutoResurrectTimeStamp)
			{
				this.AutoResurrectTimeStamp = mS2C_GetBossData.DeadTimestamp;
			}
		}
		for (int k = 0; k <= 31; k++)
		{
			this.damageRank[k] = null;
		}
		for (int l = 0; l < mS2C_GetBossData.RData.Count; l++)
		{
			if ((mS2C_GetBossData.RData[l].Rank > 0 && mS2C_GetBossData.RData[l].Rank < 31) || l == 0)
			{
				this.damageRank[l] = mS2C_GetBossData.RData[l];
			}
		}
		this.TotalCount = mS2C_GetBossData.TotalCount;
		this.Rank = mS2C_GetBossData.Rank;
		this.TotalDamage = mS2C_GetBossData.TotalDamage;
		this.mHasReward = mS2C_GetBossData.HasReward;
		if (this.GetBossDataEvent != null)
		{
			this.GetBossDataEvent();
		}
	}

	public void InitBossRankData(uint versionID, List<RankData> datas)
	{
		if (this.bossRankVersionID == versionID)
		{
			return;
		}
		this.bossRankVersionID = versionID;
		Array.Clear(this.damageRank, 0, this.damageRank.Length);
		for (int i = 0; i < datas.Count; i++)
		{
			if ((datas[i].Rank > 0 && datas[i].Rank < 31) || i == 0)
			{
				this.damageRank[i] = datas[i];
			}
		}
	}

	public void SendGetBossRank()
	{
		MC2S_GetBossRank mC2S_GetBossRank = new MC2S_GetBossRank();
		mC2S_GetBossRank.BossRankVersion = this.bossRankVersionID;
		Globals.Instance.CliSession.Send(645, mC2S_GetBossRank);
	}

	public void OnMsgBossRespawn(MemoryStream stream)
	{
		MS2C_BossRespawn mS2C_BossRespawn = Serializer.NonGeneric.Deserialize(typeof(MS2C_BossRespawn), stream) as MS2C_BossRespawn;
		BossData bossData = this.GetBossData(mS2C_BossRespawn.Slot);
		if (bossData == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetBossData error, slot = {0}", mS2C_BossRespawn.Slot)
			});
			return;
		}
		bossData.InfoID = mS2C_BossRespawn.InfoID;
		bossData.HealthPct = 100f;
		bossData.MaxHP = mS2C_BossRespawn.MaxHP;
		bossData.Scale = mS2C_BossRespawn.Scale;
		this.bossInfo[mS2C_BossRespawn.Slot] = Globals.Instance.AttDB.MonsterDict.GetInfo(mS2C_BossRespawn.InfoID);
		if (this.bossInfo[mS2C_BossRespawn.Slot] == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("MonsterDict.GetInfo error, ID = {0}", mS2C_BossRespawn.InfoID)
			});
			return;
		}
		if (this.BossRespawnEvent != null)
		{
			this.BossRespawnEvent(mS2C_BossRespawn.Slot, this.bossInfo[mS2C_BossRespawn.Slot]);
		}
	}

	public void OnMsgBossDead(MemoryStream stream)
	{
		MS2C_BossDead mS2C_BossDead = Serializer.NonGeneric.Deserialize(typeof(MS2C_BossDead), stream) as MS2C_BossDead;
		BossData bossData = this.GetBossData(mS2C_BossDead.Slot);
		if (bossData == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetBossData error, slot = {0}", mS2C_BossDead.Slot)
			});
			return;
		}
		bossData.HealthPct = 0f;
		if (this.BossDeadEvent != null)
		{
			this.BossDeadEvent(mS2C_BossDead.Slot, this.bossInfo[mS2C_BossDead.Slot], mS2C_BossDead.Name);
		}
		bossData.InfoID = 0;
	}

	public void OnMsgDoBossDamage(MemoryStream stream)
	{
		MS2C_DoBossDamage mS2C_DoBossDamage = Serializer.NonGeneric.Deserialize(typeof(MS2C_DoBossDamage), stream) as MS2C_DoBossDamage;
		if (mS2C_DoBossDamage.Rank != 0)
		{
			this.Rank = mS2C_DoBossDamage.Rank;
			if (this.DamageRankEvent != null)
			{
				this.DamageRankEvent();
			}
		}
		if (mS2C_DoBossDamage.Result != 0 && this.DoDamageFailEvent != null)
		{
			this.DoDamageFailEvent();
		}
	}

	public void OnMsgBroadcastDamage(MemoryStream stream)
	{
		MS2C_BroadcastDamage mS2C_BroadcastDamage = Serializer.NonGeneric.Deserialize(typeof(MS2C_BroadcastDamage), stream) as MS2C_BroadcastDamage;
		BossData bossData = this.GetBossData(mS2C_BroadcastDamage.Slot);
		if (bossData == null)
		{
			Debug.LogError(new object[]
			{
				string.Format("GetBossData error, slot = {0}", mS2C_BroadcastDamage.Slot)
			});
			return;
		}
		bossData.HealthPct = mS2C_BroadcastDamage.HealthPct;
		if (bossData.InfoID != mS2C_BroadcastDamage.InfoID)
		{
			bossData.InfoID = mS2C_BroadcastDamage.InfoID;
			this.bossInfo[mS2C_BroadcastDamage.Slot] = Globals.Instance.AttDB.MonsterDict.GetInfo(mS2C_BroadcastDamage.InfoID);
			if (this.bossInfo[mS2C_BroadcastDamage.Slot] == null)
			{
				Debug.LogError(new object[]
				{
					string.Format("MonsterDict.GetInfo error, ID = {0}", mS2C_BroadcastDamage.InfoID)
				});
				return;
			}
		}
		if (Globals.Instance.Player.Data.Name.Equals(mS2C_BroadcastDamage.Name))
		{
			return;
		}
		if (this.DoBossDamageEvent != null)
		{
			this.DoBossDamageEvent(mS2C_BroadcastDamage.Slot, this.bossInfo[mS2C_BroadcastDamage.Slot], mS2C_BroadcastDamage.Name, mS2C_BroadcastDamage.Damage, mS2C_BroadcastDamage.ResurrectType);
		}
	}

	public void OnMsgBroadcastDamageRank(MemoryStream stream)
	{
		MS2C_BroadcastDamageRank mS2C_BroadcastDamageRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_BroadcastDamageRank), stream) as MS2C_BroadcastDamageRank;
		for (int i = 0; i < mS2C_BroadcastDamageRank.Data.Count; i++)
		{
			if ((mS2C_BroadcastDamageRank.Data[i].Rank > 0 && mS2C_BroadcastDamageRank.Data[i].Rank < 31) || i == 0)
			{
				this.damageRank[i] = mS2C_BroadcastDamageRank.Data[i];
			}
		}
		if (this.DamageRankEvent != null)
		{
			this.DamageRankEvent();
		}
	}

	public void OnMsgWorldBossStart(MemoryStream stream)
	{
		MS2C_WorldBossStart mS2C_WorldBossStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_WorldBossStart), stream) as MS2C_WorldBossStart;
		if (mS2C_WorldBossStart.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_WorldBossStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_WorldBossStart.Result);
			return;
		}
		this.SetCurSlot(mS2C_WorldBossStart.Slot);
		this.UpdateCurBossData(mS2C_WorldBossStart.InfoID, mS2C_WorldBossStart.HealthPct);
		Globals.Instance.ActorMgr.SetServerData(mS2C_WorldBossStart.Key, mS2C_WorldBossStart.Data);
		GameUIManager.mInstance.LoadScene(mS2C_WorldBossStart.SceneID);
	}

	private void OnMsgUpdateFDSReward(MemoryStream stream)
	{
		MS2C_UpdateFDSReward mS2C_UpdateFDSReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateFDSReward), stream) as MS2C_UpdateFDSReward;
		if (this.mFDSRewardTaked != null)
		{
			if (mS2C_UpdateFDSReward.ID == 0)
			{
				this.mFDSRewardTaked.Clear();
			}
			else
			{
				int num = mS2C_UpdateFDSReward.ID / 32;
				int num2 = mS2C_UpdateFDSReward.ID % 32;
				while (this.mFDSRewardTaked.Count <= num)
				{
					this.mFDSRewardTaked.Add(0);
				}
				List<int> list;
				List<int> expr_78 = list = this.mFDSRewardTaked;
				int num3;
				int expr_7B = num3 = num;
				num3 = list[num3];
				expr_78[expr_7B] = (num3 | 1 << num2);
			}
		}
	}

	public bool IsFDSRewardTaken(int id)
	{
		bool result = false;
		int num = id / 32;
		int num2 = id % 32;
		if (num < this.mFDSRewardTaked.Count)
		{
			result = ((this.mFDSRewardTaked[num] & 1 << num2) != 0);
		}
		return result;
	}

	private bool IsWBDeadAM(int slot)
	{
		return (this.mHasReward & 1 << slot) != 0;
	}

	private bool IsWBDeadPM(int slot)
	{
		return (this.mHasReward & 1 << slot + 8) != 0;
	}

	private bool IsWBTakendAM(int slot)
	{
		return (this.mHasReward & 1 << slot + 16) != 0;
	}

	private bool IsWBTakendPM(int slot)
	{
		return (this.mHasReward & 1 << slot + 24) != 0;
	}

	public bool IsWBRewardTaken(int slot)
	{
		return (this.IsWBDeadAM(slot) && this.IsWBTakendAM(slot) && !this.IsWBDeadPM(slot)) || (!this.IsWBDeadAM(slot) && this.IsWBDeadPM(slot) && this.IsWBTakendPM(slot)) || (this.IsWBDeadAM(slot) && this.IsWBTakendAM(slot) && this.IsWBDeadPM(slot) && this.IsWBTakendPM(slot));
	}

	public bool IsWBRewrdDouble(int slot)
	{
		return this.IsWBDeadAM(slot) && !this.IsWBTakendAM(slot) && this.IsWBDeadPM(slot) && !this.IsWBTakendPM(slot);
	}

	public bool IsWBRewrdCanTaken(int slot)
	{
		return (this.IsWBDeadAM(slot) && !this.IsWBTakendAM(slot)) || (this.IsWBDeadPM(slot) && !this.IsWBTakendPM(slot));
	}

	public void SetWBRewardFlag(int flag)
	{
		this.mHasReward = flag;
	}
}
