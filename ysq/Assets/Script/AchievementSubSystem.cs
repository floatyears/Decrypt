using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class AchievementSubSystem : ISubSystem
{
	public delegate void AchievementUpdateCallback(AchievementInfo info);

	public AchievementSubSystem.AchievementUpdateCallback AchievementUpdateEvent;

	public AchievementSubSystem.AchievementUpdateCallback AchievementTakeRewardEvent;

	private Dictionary<int, AchievementData> achievements = new Dictionary<int, AchievementData>();

	private AchievementDataEx[,] curAchievements = new AchievementDataEx[2, 67];

	public uint Version
	{
		get;
		private set;
	}

	public AchievementSubSystem()
	{
		this.Version = 0u;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(245, new ClientSession.MsgHandler(this.OnMsgAchievementUpdate));
		Globals.Instance.CliSession.Register(247, new ClientSession.MsgHandler(this.OnMsgTakeAchievementReward));
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.Version = 0u;
		this.achievements.Clear();
		for (int i = 0; i < 67; i++)
		{
			this.curAchievements[0, i] = null;
			this.curAchievements[1, i] = null;
		}
	}

	public void LoadData(uint version, List<AchievementData> data)
	{
		if (version == 0u || version == this.Version)
		{
			return;
		}
		this.Version = version;
		this.achievements.Clear();
		for (int i = 0; i < data.Count; i++)
		{
			this.achievements.Add(data[i].AchievementID, data[i]);
		}
		for (int j = 0; j < 67; j++)
		{
			this.UpdateCurAchievement(j, false);
			this.UpdateCurAchievement(j, true);
		}
	}

	private void UpdateCurAchievement(int type, bool daily)
	{
		List<AchievementInfo> list = Achievement.GetAchievements(type);
		if (list == null)
		{
			return;
		}
		AchievementInfo achievementInfo = null;
		AchievementData achievementData = null;
		int num = 0;
		if (daily)
		{
			num = 1;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Daily == daily)
			{
				achievementInfo = list[i];
				achievementData = this.GetAchievement(achievementInfo.ID);
				if (achievementData == null || !achievementData.TakeReward)
				{
					break;
				}
			}
		}
		if (achievementInfo == null)
		{
			return;
		}
		if (this.curAchievements[num, type] == null)
		{
			this.curAchievements[num, type] = new AchievementDataEx();
		}
		if (achievementData == null)
		{
			achievementData = new AchievementData();
			achievementData.AchievementID = achievementInfo.ID;
			achievementData.Value = 0;
			achievementData.CoolDown = 0;
			achievementData.TakeReward = false;
			this.achievements.Add(achievementData.AchievementID, achievementData);
		}
		this.curAchievements[num, type].Data = achievementData;
		this.curAchievements[num, type].Info = achievementInfo;
	}

	public AchievementData GetAchievement(int id)
	{
		AchievementData result = null;
		this.achievements.TryGetValue(id, out result);
		return result;
	}

	public AchievementDataEx GetCurAchievement(int type, bool daily)
	{
		if (type < 0 || type >= 67)
		{
			return null;
		}
		if (daily)
		{
			return this.curAchievements[1, type];
		}
		return this.curAchievements[0, type];
	}

	public bool HasTakeReward(int id)
	{
		AchievementData achievement = this.GetAchievement(id);
		return achievement != null && achievement.TakeReward;
	}

	public bool HasTakeReward()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 67; j++)
			{
				if (this.curAchievements[i, j] != null && !this.curAchievements[i, j].Data.TakeReward && this.curAchievements[i, j].IsComplete())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasTakeReward(bool daily)
	{
		int num = 0;
		if (daily)
		{
			num = 1;
		}
		for (int i = 0; i < 67; i++)
		{
			if (this.curAchievements[num, i] != null && !this.curAchievements[num, i].Data.TakeReward && this.curAchievements[num, i].IsComplete())
			{
				return true;
			}
		}
		return false;
	}

	public void OnMsgAchievementUpdate(MemoryStream stream)
	{
		MS2C_AchievementUpdate mS2C_AchievementUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_AchievementUpdate), stream) as MS2C_AchievementUpdate;
		for (int i = 0; i < mS2C_AchievementUpdate.Data.Count; i++)
		{
			AchievementData achievementData = this.GetAchievement(mS2C_AchievementUpdate.Data[i].AchievementID);
			if (achievementData == null)
			{
				achievementData = mS2C_AchievementUpdate.Data[i];
				this.achievements.Add(achievementData.AchievementID, achievementData);
			}
			else
			{
				achievementData.Value = mS2C_AchievementUpdate.Data[i].Value;
				achievementData.CoolDown = mS2C_AchievementUpdate.Data[i].CoolDown;
				achievementData.TakeReward = mS2C_AchievementUpdate.Data[i].TakeReward;
			}
			AchievementInfo info = Globals.Instance.AttDB.AchievementDict.GetInfo(achievementData.AchievementID);
			if (info == null)
			{
				Debug.LogErrorFormat("AchievementDict.GetInfo error, id = {0}", new object[]
				{
					achievementData.AchievementID
				});
			}
			else
			{
				AchievementDataEx curAchievement = this.GetCurAchievement(info.ConditionType, info.Daily);
				if (curAchievement != null && curAchievement.Data.AchievementID == achievementData.AchievementID && this.AchievementUpdateEvent != null)
				{
					this.AchievementUpdateEvent(info);
				}
			}
		}
		this.Version = mS2C_AchievementUpdate.AchievementVersion;
	}

	public void OnMsgTakeAchievementReward(MemoryStream stream)
	{
		MS2C_TakeAchievementReward mS2C_TakeAchievementReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeAchievementReward), stream) as MS2C_TakeAchievementReward;
		AchievementData achievement = this.GetAchievement(mS2C_TakeAchievementReward.AchievementID);
		if (achievement == null)
		{
			return;
		}
		achievement.CoolDown = mS2C_TakeAchievementReward.TimeStamp;
		achievement.TakeReward = true;
		this.Version = mS2C_TakeAchievementReward.AchievementVersion;
		AchievementInfo info = Globals.Instance.AttDB.AchievementDict.GetInfo(achievement.AchievementID);
		if (info == null)
		{
			Debug.LogErrorFormat("AchievementDict.GetInfo error, id = {0}", new object[]
			{
				achievement.AchievementID
			});
			return;
		}
		this.UpdateCurAchievement(info.ConditionType, info.Daily);
		if (this.AchievementTakeRewardEvent != null)
		{
			this.AchievementTakeRewardEvent(info);
		}
	}
}
