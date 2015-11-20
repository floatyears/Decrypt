using Att;
using Proto;
using System;
using System.IO;
using UnityEngine;

public sealed class WorldBossScene : BaseScene
{
	private bool respawnFlag;

	private ActorController bossActor;

	private float sendDamageTimer;

	private int sendDamageFrame;

	private bool hpFlag;

	private float timeStamp;

	private long totalDamage;

	private int totalCount;

	private GameObject uiDead;

	private bool isBossDead;

	public WorldBossScene(ActorManager actorManager) : base(actorManager)
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
		this.uiDead = null;
		this.isBossDead = false;
		this.maxCombatTimer = (float)(Globals.Instance.Player.WorldBossSystem.TimeStamp - Globals.Instance.Player.GetTimeStamp());
		this.combatTimer = this.maxCombatTimer;
		WorldBossSubSystem expr_A7 = Globals.Instance.Player.WorldBossSystem;
		expr_A7.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Combine(expr_A7.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
		WorldBossSubSystem expr_D7 = Globals.Instance.Player.WorldBossSystem;
		expr_D7.DoBossDamageEvent = (WorldBossSubSystem.DoBossDamageCallback)Delegate.Combine(expr_D7.DoBossDamageEvent, new WorldBossSubSystem.DoBossDamageCallback(this.OnDoBossDamageEvent));
		WorldBossSubSystem expr_107 = Globals.Instance.Player.WorldBossSystem;
		expr_107.DoDamageFailEvent = (WorldBossSubSystem.VoidCallback)Delegate.Combine(expr_107.DoDamageFailEvent, new WorldBossSubSystem.VoidCallback(this.OnDoDamageFail));
		LocalPlayer expr_132 = Globals.Instance.Player;
		expr_132.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Combine(expr_132.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInitEvent));
		Globals.Instance.CliSession.Register(1502, new ClientSession.MsgHandler(this.HandleDisconnect));
	}

	public override void Update(float elapse)
	{
		if (!this.hpFlag && this.bossActor != null && this.bossActor.MaxHP != 0L)
		{
			BossData curBossData = Globals.Instance.Player.WorldBossSystem.GetCurBossData();
			if (curBossData != null)
			{
				this.bossActor.ResetMaxHP(curBossData.MaxHP);
				this.bossActor.CurHP = (long)((float)this.bossActor.MaxHP * curBossData.HealthPct);
				this.bossActor.AiCtrler.Locked = true;
				this.hpFlag = true;
			}
		}
		switch (this.status)
		{
		case 2:
			if (this.combatTimer > 0f)
			{
				this.combatTimer -= elapse;
			}
			if (this.isBossDead || Globals.Instance.Player.WorldBossSystem.WorldBossIsOver())
			{
				this.actorMgr.LockAllActorAI();
				GameAnalytics.OnFailScene(Globals.Instance.SenceMgr.sceneInfo, GameAnalytics.ESceneFailed.Timeup);
				Globals.Instance.SenceMgr.CloseScene();
				GameUIManager.mInstance.ChangeSession<GUIWorldBossVictoryScene>(null, false, false);
				this.status = 5;
				return;
			}
			this.sendDamageTimer += elapse;
			if (this.sendDamageTimer >= 10f && Time.frameCount > this.sendDamageFrame)
			{
				if (this.totalDamage > 0L)
				{
					MC2S_DoBossDamage mC2S_DoBossDamage = new MC2S_DoBossDamage();
					mC2S_DoBossDamage.Slot = Globals.Instance.Player.WorldBossSystem.CurSlot;
					mC2S_DoBossDamage.ResultKey = this.actorMgr.Key;
					mC2S_DoBossDamage.Damage = this.totalDamage;
					mC2S_DoBossDamage.Count = this.totalCount;
					mC2S_DoBossDamage.RecvStartTime = this.actorMgr.RecvStartTime;
					mC2S_DoBossDamage.SendResultTime = Globals.Instance.Player.GetTimeStamp();
					if (Globals.Instance.CliSession.SendPacket(618, mC2S_DoBossDamage))
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
		case 4:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				this.ShowDeadUI();
				this.status = 2;
			}
			break;
		}
	}

	public override void Destroy()
	{
		if (this.totalDamage > 0L)
		{
			MC2S_DoBossDamage mC2S_DoBossDamage = new MC2S_DoBossDamage();
			mC2S_DoBossDamage.Slot = Globals.Instance.Player.WorldBossSystem.CurSlot;
			mC2S_DoBossDamage.ResultKey = this.actorMgr.Key;
			mC2S_DoBossDamage.Damage = this.totalDamage;
			mC2S_DoBossDamage.Count = this.totalCount;
			mC2S_DoBossDamage.RecvStartTime = this.actorMgr.RecvStartTime;
			mC2S_DoBossDamage.SendResultTime = Globals.Instance.Player.GetTimeStamp();
			Globals.Instance.CliSession.SendPacket(618, mC2S_DoBossDamage);
		}
		this.actorMgr.BuildCombatLog(this.isBossDead);
		MC2S_CombatLog mC2S_CombatLog = new MC2S_CombatLog();
		mC2S_CombatLog.Type = 0;
		mC2S_CombatLog.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.SendPacket(649, mC2S_CombatLog);
		if (this.uiDead != null)
		{
			UnityEngine.Object.Destroy(this.uiDead);
			this.uiDead = null;
		}
		this.actorMgr.ForceShowBossActor = false;
		Globals.Instance.CliSession.ShowReconnect(false);
		Globals.Instance.CliSession.Unregister(1502, new ClientSession.MsgHandler(this.HandleDisconnect));
		LocalPlayer expr_140 = Globals.Instance.Player;
		expr_140.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Remove(expr_140.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInitEvent));
		WorldBossSubSystem expr_170 = Globals.Instance.Player.WorldBossSystem;
		expr_170.DoDamageFailEvent = (WorldBossSubSystem.VoidCallback)Delegate.Remove(expr_170.DoDamageFailEvent, new WorldBossSubSystem.VoidCallback(this.OnDoDamageFail));
		WorldBossSubSystem expr_1A0 = Globals.Instance.Player.WorldBossSystem;
		expr_1A0.DoBossDamageEvent = (WorldBossSubSystem.DoBossDamageCallback)Delegate.Remove(expr_1A0.DoBossDamageEvent, new WorldBossSubSystem.DoBossDamageCallback(this.OnDoBossDamageEvent));
		WorldBossSubSystem expr_1D0 = Globals.Instance.Player.WorldBossSystem;
		expr_1D0.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Remove(expr_1D0.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		BossData curBossData = Globals.Instance.Player.WorldBossSystem.GetCurBossData();
		if (curBossData == null)
		{
			return;
		}
		if (infoID != curBossData.InfoID)
		{
			return;
		}
		if (!this.respawnFlag)
		{
			this.respawnFlag = true;
			this.bossActor = this.actorMgr.CreateMonster(infoID, position, Quaternion.Euler(0f, rotationY, 0f), scale, curBossData.Scale);
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
			if (this.bossActor.AiCtrler.GetType() == typeof(AIDragon))
			{
				((AIDragon)this.bossActor.AiCtrler).SetInitSkillCD(false);
			}
		}
	}

	public override void OnAllMonsterDead()
	{
	}

	public override void OnPlayWinOver()
	{
		Globals.Instance.CliSession.ShowReconnect(false);
	}

	public override void OnDoDamage(MonsterInfo info, long damage)
	{
		this.totalDamage += damage;
		this.totalCount++;
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 3.5f;
	}

	private void OnBossDeadEvent(int slot, MonsterInfo info, string playerName)
	{
		BossData curBossData = Globals.Instance.Player.WorldBossSystem.GetCurBossData();
		if (curBossData == null || info == null || (slot != 5 && (info.ID != curBossData.InfoID || slot != curBossData.Slot)))
		{
			return;
		}
		if (this.bossActor != null)
		{
			this.bossActor.Undead = false;
			this.bossActor.DamageRecount = false;
			this.bossActor.DoDamage(this.bossActor.MaxHP, null, false);
		}
		GameUIManager.mInstance.uiState.WorldBossKillerName = playerName;
		if (this.status != 1)
		{
			GameAnalytics.OnFinishScene(GameUIManager.mInstance.uiState.CurSceneInfo);
			Globals.Instance.SenceMgr.CloseScene();
			GameUIManager.mInstance.ChangeSession<GUIWorldBossVictoryScene>(null, false, false);
			this.status = 3;
		}
		this.isBossDead = true;
	}

	private void OnDoBossDamageEvent(int slot, MonsterInfo info, string playerName, long damage, int type)
	{
		BossData curBossData = Globals.Instance.Player.WorldBossSystem.GetCurBossData();
		if (curBossData == null || info == null || info.ID != curBossData.InfoID || this.bossActor == null)
		{
			return;
		}
		this.bossActor.DamageRecount = false;
		this.bossActor.DoDamage(damage, null, false);
		this.bossActor.DamageRecount = true;
		long num = (long)((float)this.bossActor.MaxHP * curBossData.HealthPct);
		if (this.bossActor.CurHP > num)
		{
			this.bossActor.CurHP = num;
		}
	}

	private void OnDoDamageFail()
	{
		if (!this.isBossDead)
		{
			if (this.status != 1)
			{
				GameAnalytics.OnFinishScene(GameUIManager.mInstance.uiState.CurSceneInfo);
				Globals.Instance.SenceMgr.CloseScene();
				GameUIManager.mInstance.ChangeSession<GUIWorldBossVictoryScene>(null, false, false);
				this.status = 3;
			}
			this.isBossDead = true;
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

	protected override void ShowDeadUI()
	{
		GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
		if (session != null)
		{
			NGUITools.SetActive(session.gameObject, false);
		}
		GameObject gameObject = Res.LoadGUI("GUI/DieBoss");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/DieBoss error"
			});
			return;
		}
		this.uiDead = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (this.uiDead == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild error"
			});
			return;
		}
		Vector3 localPosition = this.uiDead.transform.localPosition;
		localPosition.z += 5000f;
		this.uiDead.transform.localPosition = localPosition;
		this.uiDead.AddComponent<GUIBossResurrect>();
	}
}
