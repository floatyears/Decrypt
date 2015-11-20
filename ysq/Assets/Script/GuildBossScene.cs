using Att;
using Proto;
using System;
using System.IO;
using UnityEngine;

public sealed class GuildBossScene : BaseScene
{
	private bool respawnFlag;

	private ActorController bossActor;

	private float sendDamageTimer;

	private int sendDamageFrame;

	private bool hpFlag;

	private float timeStamp;

	private long totalDamage;

	private int totalCount;

	private bool isBossDead;

	public GuildBossScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		this.status = 1;
		this.respawnFlag = false;
		this.bossActor = null;
		this.totalDamage = 0L;
		this.totalCount = 0;
		this.sendDamageTimer = 0f;
		this.sendDamageFrame = Time.frameCount + 150;
		this.hpFlag = false;
		this.actorMgr.ForceShowBossActor = true;
		this.isBossDead = false;
		this.maxCombatTimer = 300f;
		this.combatTimer = this.maxCombatTimer;
		GuildSubSystem expr_80 = Globals.Instance.Player.GuildSystem;
		expr_80.GuildBossDeadEvent = (GuildSubSystem.GuildBossDeadCallback)Delegate.Combine(expr_80.GuildBossDeadEvent, new GuildSubSystem.GuildBossDeadCallback(this.OnGuildBossDeadEvent));
		GuildSubSystem expr_B0 = Globals.Instance.Player.GuildSystem;
		expr_B0.DoGuildBossDamageEvent = (GuildSubSystem.DoGuildBossDamageCallback)Delegate.Combine(expr_B0.DoGuildBossDamageEvent, new GuildSubSystem.DoGuildBossDamageCallback(this.OnGuildDoBossDamageEvent));
		GuildSubSystem expr_E0 = Globals.Instance.Player.GuildSystem;
		expr_E0.DoDamageFailEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_E0.DoDamageFailEvent, new GuildSubSystem.VoidCallback(this.OnDoDamageFail));
		LocalPlayer expr_10B = Globals.Instance.Player;
		expr_10B.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Combine(expr_10B.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInitEvent));
		Globals.Instance.CliSession.Register(1502, new ClientSession.MsgHandler(this.HandleDisconnect));
	}

	public override void Update(float elapse)
	{
		if (!this.hpFlag && this.bossActor != null && this.bossActor.MaxHP != 0L)
		{
			GuildBossData curGuildBossData = Globals.Instance.Player.GuildSystem.GetCurGuildBossData();
			if (curGuildBossData != null)
			{
				this.bossActor.ResetMaxHP(curGuildBossData.MaxHP);
				this.bossActor.CurHP = (long)((float)this.bossActor.MaxHP * curGuildBossData.HealthPct);
				this.bossActor.AiCtrler.Locked = true;
				this.hpFlag = true;
			}
		}
		switch (this.status)
		{
		case 2:
			if (this.isBossDead && Time.time >= this.timeStamp)
			{
				GameAnalytics.OnFinishScene(GameUIManager.mInstance.uiState.CurSceneInfo);
				Globals.Instance.SenceMgr.CloseScene();
				GameUIManager.mInstance.ChangeSession<GUIGuildSchoolVictoryScene>(null, false, false);
				this.status = 3;
				return;
			}
			if (this.combatTimer > 0f)
			{
				this.combatTimer -= elapse;
				if (this.combatTimer <= 0f)
				{
					GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.TimeUp, 0);
					this.actorMgr.LockAllActorAI();
					this.status = 3;
					this.timeStamp = Time.time + 3f;
					return;
				}
			}
			this.sendDamageTimer += elapse;
			if (this.sendDamageTimer >= 10f && Time.frameCount > this.sendDamageFrame)
			{
				if (this.totalDamage > 0L)
				{
					MC2S_DoGuildBossDamage mC2S_DoGuildBossDamage = new MC2S_DoGuildBossDamage();
					mC2S_DoGuildBossDamage.ID = Globals.Instance.Player.GuildSystem.CurBossID;
					mC2S_DoGuildBossDamage.ResultKey = this.actorMgr.Key;
					mC2S_DoGuildBossDamage.Damage = this.totalDamage;
					mC2S_DoGuildBossDamage.Count = this.totalCount;
					mC2S_DoGuildBossDamage.RecvStartTime = this.actorMgr.RecvStartTime;
					mC2S_DoGuildBossDamage.SendResultTime = Globals.Instance.Player.GetTimeStamp();
					if (Globals.Instance.CliSession.SendPacket(938, mC2S_DoGuildBossDamage))
					{
						this.sendDamageTimer = 0f;
						this.sendDamageFrame = Time.frameCount + 150;
						this.totalDamage = 0L;
						this.totalCount = 0;
					}
				}
				else
				{
					this.sendDamageTimer = 0f;
					this.sendDamageFrame = Time.frameCount + 150;
				}
			}
			break;
		case 3:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				this.ChallengeFail(true);
			}
			break;
		case 4:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				this.ChallengeFail(false);
			}
			break;
		}
	}

	public override void Destroy()
	{
		if (this.totalDamage > 0L)
		{
			MC2S_DoGuildBossDamage mC2S_DoGuildBossDamage = new MC2S_DoGuildBossDamage();
			mC2S_DoGuildBossDamage.ID = Globals.Instance.Player.GuildSystem.CurBossID;
			mC2S_DoGuildBossDamage.ResultKey = this.actorMgr.Key;
			mC2S_DoGuildBossDamage.Damage = this.totalDamage;
			mC2S_DoGuildBossDamage.Count = this.totalCount;
			mC2S_DoGuildBossDamage.RecvStartTime = this.actorMgr.RecvStartTime;
			mC2S_DoGuildBossDamage.SendResultTime = Globals.Instance.Player.GetTimeStamp();
			Globals.Instance.CliSession.SendPacket(938, mC2S_DoGuildBossDamage);
		}
		this.actorMgr.BuildCombatLog(this.isBossDead);
		MC2S_CombatLog mC2S_CombatLog = new MC2S_CombatLog();
		mC2S_CombatLog.Type = 1;
		mC2S_CombatLog.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.SendPacket(649, mC2S_CombatLog);
		this.actorMgr.ForceShowBossActor = false;
		Globals.Instance.CliSession.ShowReconnect(false);
		Globals.Instance.CliSession.Unregister(1502, new ClientSession.MsgHandler(this.HandleDisconnect));
		LocalPlayer expr_11D = Globals.Instance.Player;
		expr_11D.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Remove(expr_11D.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInitEvent));
		GuildSubSystem expr_14D = Globals.Instance.Player.GuildSystem;
		expr_14D.DoDamageFailEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_14D.DoDamageFailEvent, new GuildSubSystem.VoidCallback(this.OnDoDamageFail));
		GuildSubSystem expr_17D = Globals.Instance.Player.GuildSystem;
		expr_17D.DoGuildBossDamageEvent = (GuildSubSystem.DoGuildBossDamageCallback)Delegate.Remove(expr_17D.DoGuildBossDamageEvent, new GuildSubSystem.DoGuildBossDamageCallback(this.OnGuildDoBossDamageEvent));
		GuildSubSystem expr_1AD = Globals.Instance.Player.GuildSystem;
		expr_1AD.GuildBossDeadEvent = (GuildSubSystem.GuildBossDeadCallback)Delegate.Remove(expr_1AD.GuildBossDeadEvent, new GuildSubSystem.GuildBossDeadCallback(this.OnGuildBossDeadEvent));
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		GuildBossData curGuildBossData = Globals.Instance.Player.GuildSystem.GetCurGuildBossData();
		if (curGuildBossData == null)
		{
			return;
		}
		if (infoID != curGuildBossData.InfoID)
		{
			return;
		}
		if (!this.respawnFlag)
		{
			this.respawnFlag = true;
			this.bossActor = this.actorMgr.CreateMonster(infoID, position, Quaternion.Euler(0f, rotationY, 0f), scale, 10000);
			if (this.bossActor != null)
			{
				this.bossActor.Undead = true;
				this.bossActor.DamageRecount = true;
				this.actorMgr.Actors.Add(this.bossActor);
			}
		}
	}

	public override void OnLoadRespawnOK()
	{
		this.actorMgr.CreateLocalActors();
	}

	public override void OnPreStart()
	{
		if (this.bossActor != null)
		{
			Globals.Instance.CameraMgr.Target = this.bossActor.gameObject;
			Globals.Instance.CameraMgr.resultCamTest = true;
		}
	}

	public override void OnStart()
	{
		this.status = 2;
		Globals.Instance.CliSession.ShowReconnect(true);
		if (this.bossActor != null)
		{
			this.bossActor.AiCtrler.Locked = false;
		}
		this.actorMgr.Actors[0].AiCtrler.EnableAI = GameCache.Data.EnableAI;
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
		this.status = 4;
		this.timeStamp = Time.time + 2f;
		if (this.bossActor != null)
		{
			this.bossActor.AiCtrler.LeaveCombat();
		}
	}

	public override void OnPlayWinOver()
	{
		Globals.Instance.CliSession.ShowReconnect(false);
	}

	public override void OnDoDamage(MonsterInfo info, long damage)
	{
		this.totalDamage += damage;
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 3.5f;
	}

	private void OnGuildBossDeadEvent(int id, MonsterInfo info, string playerName)
	{
		GuildBossData curGuildBossData = Globals.Instance.Player.GuildSystem.GetCurGuildBossData();
		if (curGuildBossData == null || info == null || id != curGuildBossData.ID || info.ID != curGuildBossData.InfoID)
		{
			return;
		}
		if (this.bossActor != null)
		{
			this.bossActor.Undead = false;
			this.bossActor.DamageRecount = false;
			this.bossActor.DoDamage(this.bossActor.MaxHP, null, false);
		}
		this.isBossDead = true;
		this.timeStamp = Time.time + 3f;
		this.actorMgr.LockAllActorAI();
		GameUIManager.mInstance.uiState.GuildBossKillerName = playerName;
	}

	private void OnGuildDoBossDamageEvent(int id, MonsterInfo info, string playerName, long damage)
	{
		GuildBossData curGuildBossData = Globals.Instance.Player.GuildSystem.GetCurGuildBossData();
		if (curGuildBossData == null || info == null || id != curGuildBossData.ID || info.ID != curGuildBossData.InfoID || this.bossActor == null)
		{
			return;
		}
		this.bossActor.DamageRecount = false;
		this.bossActor.DoDamage(damage, null, false);
		this.bossActor.DamageRecount = true;
		long num = (long)((float)this.bossActor.MaxHP * curGuildBossData.HealthPct);
		if (this.bossActor.CurHP > num)
		{
			this.bossActor.CurHP = num;
		}
	}

	private void OnDoDamageFail()
	{
		if (!this.isBossDead)
		{
			this.isBossDead = true;
			this.timeStamp = Time.time + 1f;
		}
	}

	private void OnDataInitEvent(bool versionInit, bool newPlayer)
	{
		this.actorMgr.UnlockAllActorAI();
	}

	public void HandleDisconnect(MemoryStream stream)
	{
		this.actorMgr.LockAllActorAI();
	}

	private void ChallengeFail(bool timeOut)
	{
		GameAnalytics.OnFailScene(Globals.Instance.SenceMgr.sceneInfo, (!timeOut) ? GameAnalytics.ESceneFailed.CombatEffectiveness : GameAnalytics.ESceneFailed.Timeup);
		Globals.Instance.SenceMgr.CloseScene();
		GameUIManager.mInstance.ChangeSession<GUIGuildSchoolVictoryScene>(null, false, false);
	}
}
