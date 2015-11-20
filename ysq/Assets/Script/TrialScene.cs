using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public sealed class TrialScene : BaseScene
{
	public delegate void AllMonsterDeadCallback(int curLvl);

	public delegate void NextWaveCallback();

	public TrialScene.AllMonsterDeadCallback AllMonsterDeadEvent;

	public TrialScene.NextWaveCallback NextWaveEvent;

	private RespawnPoint[] respawnPoints = new RespawnPoint[15];

	private List<RespawnData>[] respawnActors = new List<RespawnData>[5];

	private int curWave;

	private int index;

	private float timeStamp;

	private float resetTimer;

	private int score;

	private int money;

	private bool failed;

	public int CurWave
	{
		get
		{
			return this.curWave + this.index;
		}
	}

	public int Score
	{
		get
		{
			return this.score;
		}
	}

	public int Money
	{
		get
		{
			return this.money;
		}
	}

	public TrialScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		this.status = 1;
		this.curWave = Globals.Instance.Player.Data.TrialWave + 1;
		this.index = 0;
		this.timeStamp = 0f;
		this.resetTimer = 0f;
		this.maxCombatTimer = 300f;
		this.combatTimer = this.maxCombatTimer;
		this.stopTimer = true;
		this.score = 0;
		this.money = 0;
		this.failed = false;
		Globals.Instance.CliSession.Register(631, new ClientSession.MsgHandler(this.HandleTrialWave));
	}

	public override void Update(float elapse)
	{
		switch (this.status)
		{
		case 2:
			if (this.resetTimer > 0f && Time.time >= this.resetTimer)
			{
				this.resetTimer = -1f;
				this.actorMgr.Resurrect(false);
				GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.WaveNum, this.curWave + this.index);
				if (this.NextWaveEvent != null)
				{
					this.NextWaveEvent();
				}
			}
			if (!this.stopTimer && this.combatTimer > 0f)
			{
				this.combatTimer -= elapse;
				if (this.combatTimer <= 0f)
				{
					GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.TimeUp, 0);
					this.actorMgr.LockAllActorAI();
					this.status = 3;
					this.timeStamp = Time.time + 3f;
					this.failed = true;
					return;
				}
			}
			if (this.timeStamp > 0f)
			{
				float num = Time.time - this.timeStamp;
				if (num >= 0f)
				{
					int num2 = 0;
					for (int i = 0; i < this.respawnActors[this.index].Count; i++)
					{
						if (this.respawnActors[this.index][i].DelayTime >= 0f && num >= this.respawnActors[this.index][i].DelayTime)
						{
							this.respawnActors[this.index][i].Actor.gameObject.SetActive(true);
							this.actorMgr.Actors.Add(this.respawnActors[this.index][i].Actor);
							this.respawnActors[this.index][i].DelayTime = -1f;
							this.respawnActors[this.index][i].Actor.PlayAction("Skill/misc_004", null);
						}
						if (this.respawnActors[this.index][i].DelayTime < 0f)
						{
							num2++;
						}
					}
					if (this.respawnActors[this.index].Count == num2)
					{
						this.respawnActors[this.index].Clear();
						this.timeStamp = -1f;
					}
				}
			}
			break;
		case 3:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				this.SendTrialWave();
			}
			break;
		case 4:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.BattleEnd, 0);
				this.status = 3;
				this.timeStamp = Time.time + 2f;
			}
			break;
		}
	}

	public override void Destroy()
	{
		this.actorMgr.BuildCombatLog(!this.failed);
		MC2S_CombatLog mC2S_CombatLog = new MC2S_CombatLog();
		mC2S_CombatLog.Type = 2;
		mC2S_CombatLog.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.SendPacket(649, mC2S_CombatLog);
		GameUIManager.mInstance.CloseBattleCDMsg();
		for (int i = 0; i < 15; i++)
		{
			this.respawnPoints[i] = null;
		}
		for (int j = 0; j < 5; j++)
		{
			if (this.respawnActors[j] != null)
			{
				this.respawnActors[j].Clear();
			}
		}
		Globals.Instance.CliSession.Unregister(631, new ClientSession.MsgHandler(this.HandleTrialWave));
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		if (groupID < 0 || groupID >= 15)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("RespawnPoint error, groupID = {0}", groupID)
			});
			return;
		}
		if (this.respawnPoints[groupID] == null)
		{
			this.respawnPoints[groupID] = new RespawnPoint();
			if (this.respawnPoints[groupID] == null)
			{
				global::Debug.LogError(new object[]
				{
					"Allocate RespawnPoint error!"
				});
				return;
			}
		}
		this.respawnPoints[groupID].Position = position;
		this.respawnPoints[groupID].Rotation = Quaternion.Euler(0f, rotationY, 0f);
	}

	public override void OnLoadRespawnOK()
	{
		this.actorMgr.CreateLocalActors();
		this.actorMgr.StartCoroutine(this.Create5WaveMonster(false, 0f));
	}

	public override void OnPreStart()
	{
		GameUIManager.mInstance.ShowBattleCDMsg(null);
	}

	public override void OnStart()
	{
		this.status = 2;
		this.stopTimer = false;
		this.actorMgr.Actors[0].AiCtrler.EnableAI = GameCache.Data.EnableAI;
		this.StartNextWave(0f);
	}

	public override void OnChangeAIMode()
	{
		if (!this.actorMgr.Actors[0].AiCtrler.EnableAI && this.actorMgr.Actors[0].CanMove(false, false))
		{
			this.actorMgr.Actors[0].StopMove();
		}
	}

	public override void OnPlayerDead()
	{
		this.actorMgr.LockAllActorAI();
		this.stopTimer = true;
		this.failed = true;
		this.status = 4;
		this.timeStamp = Time.time + 2f;
	}

	public override void OnAllMonsterDead()
	{
		if (this.CurWave > 1 && this.CurWave % 5 == 0 && this.AllMonsterDeadEvent != null)
		{
			this.AllMonsterDeadEvent(this.CurWave);
		}
		if (this.respawnActors[this.index].Count == 0)
		{
			this.index++;
			this.SendTrialWave();
		}
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 4.5f;
	}

	public override float GetCombatTimer()
	{
		if (this.combatTimer > this.maxCombatTimer)
		{
			return this.maxCombatTimer;
		}
		if (this.combatTimer < 0f)
		{
			return 0f;
		}
		return this.combatTimer;
	}

	private void StartNextWave(float delay)
	{
		this.resetTimer = Time.time + delay;
		this.timeStamp = this.resetTimer + 1.5f;
		this.combatTimer = delay + 1.5f + this.maxCombatTimer;
		this.stopTimer = false;
	}

	public void StartNext5Wave(float delay)
	{
		this.actorMgr.StartCoroutine(this.Create5WaveMonster(true, delay));
	}

	[DebuggerHidden]
	public IEnumerator Create5WaveMonster(bool flag, float dalay)
	{
        return null;
        //TrialScene.<Create5WaveMonster>c__IteratorE <Create5WaveMonster>c__IteratorE = new TrialScene.<Create5WaveMonster>c__IteratorE();
        //<Create5WaveMonster>c__IteratorE.flag = flag;
        //<Create5WaveMonster>c__IteratorE.dalay = dalay;
        //<Create5WaveMonster>c__IteratorE.<$>flag = flag;
        //<Create5WaveMonster>c__IteratorE.<$>dalay = dalay;
        //<Create5WaveMonster>c__IteratorE.<>f__this = this;
        //return <Create5WaveMonster>c__IteratorE;
	}

	public void SendTrialWave()
	{
		MC2S_TrialWave mC2S_TrialWave = new MC2S_TrialWave();
		mC2S_TrialWave.Wave = this.CurWave - 1;
		if (!this.failed)
		{
			mC2S_TrialWave.ResultKey = (this.actorMgr.Key ^ 2014);
		}
		else
		{
			mC2S_TrialWave.ResultKey = (this.actorMgr.Key ^ 2010);
		}
		mC2S_TrialWave.RecvStartTime = this.actorMgr.RecvStartTime;
		mC2S_TrialWave.SendResultTime = Globals.Instance.Player.GetTimeStamp();
		this.actorMgr.RecvStartTime = mC2S_TrialWave.SendResultTime;
		Globals.Instance.CliSession.Send(630, mC2S_TrialWave);
	}

	public void HandleTrialWave(MemoryStream stream)
	{
		MS2C_TrialWave mS2C_TrialWave = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrialWave), stream) as MS2C_TrialWave;
		if (mS2C_TrialWave.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		bool flag = false;
		if (mS2C_TrialWave.Wave == 0)
		{
			flag = true;
		}
		if (mS2C_TrialWave.Result != 0 && mS2C_TrialWave.Result != 5)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_TrialWave.Result);
			flag = true;
		}
		if (flag)
		{
			Globals.Instance.SenceMgr.CloseScene();
			GameUIManager.mInstance.ChangeSession<GUITrailTowerSucV2>(null, false, false);
			return;
		}
		if (mS2C_TrialWave.Result == 0)
		{
			this.actorMgr.Key = mS2C_TrialWave.Key;
			this.actorMgr.RecvStartTime = Globals.Instance.Player.GetTimeStamp();
		}
		TrialInfo info = Globals.Instance.AttDB.TrialDict.GetInfo(mS2C_TrialWave.Wave);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("TrialDict.GetInfo error, ID = {0}", mS2C_TrialWave.Wave)
			});
		}
		else
		{
			this.score += info.Value;
			this.money += info.Money;
		}
		if (this.actorMgr.TrialScoreEvent != null)
		{
			this.actorMgr.TrialScoreEvent(this.curWave + this.index, this.score);
		}
		if (mS2C_TrialWave.Wave % 5 == 0)
		{
			this.curWave = mS2C_TrialWave.Wave + 1;
			this.index = 0;
			this.stopTimer = true;
			this.StartNext5Wave(1.5f);
		}
		else
		{
			this.StartNextWave(1.5f);
		}
	}
}
