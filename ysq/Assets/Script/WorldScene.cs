using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldScene : BaseWorld
{
	private int respawnIndex;

	private List<List<ActorController>> respawnActors = new List<List<ActorController>>();

	public WorldScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		this.status = 1;
		this.findBossDialogID = 0;
		this.respawnIndex = 0;
		this.respawnActors.Clear();
		this.maxCombatTimer = 300f;
		this.combatTimer = this.maxCombatTimer;
		this.stopTimer = false;
	}

	public override void Update(float elapse)
	{
		switch (this.status)
		{
		case 2:
			if (!this.actorMgr.PlayerCtrler.ActorCtrler.AiCtrler.EnableAI)
			{
				base.CheckBossDialog(false);
			}
			if (this.timer > 0f)
			{
				this.timer -= elapse;
				if (this.timer <= 0f)
				{
					GameCache.SetDialogShowed(this.findBossDialogID);
					if (GameUIManager.mInstance.ShowPlotDialog(this.findBossDialogID, new GUIPlotDialog.FinishCallback(base.DialogFinish), null))
					{
						this.actorMgr.Pause(true);
					}
				}
			}
			if (!this.stopTimer && this.combatTimer > 0f)
			{
				this.combatTimer -= elapse;
				if (this.combatTimer <= 0f)
				{
					GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.TimeUp, 0);
					this.actorMgr.LockAllActorAI();
					this.status = 5;
					this.timer = 3f;
				}
			}
			break;
		case 3:
			if (this.timer > 0f)
			{
				this.timer -= elapse;
				if (this.timer <= 0f)
				{
					if (GameUIManager.mInstance.ShowSceneWinDialog(this.senceInfo.ID, new GUIPlotDialog.FinishCallback(base.WinDialogFinish)))
					{
						Globals.Instance.GameMgr.ClearSpeedMod();
						this.actorMgr.Pause(true);
					}
					else
					{
						Globals.Instance.GameMgr.SpeedDown(-0.6f, false);
						this.actorMgr.GoPosePoint();
					}
				}
			}
			break;
		case 4:
			if (this.timer > 0f)
			{
				this.timer -= elapse;
				if (this.timer <= 0f)
				{
					this.ShowDeadUI();
					this.status = 2;
				}
			}
			break;
		case 5:
			if (this.timer > 0f)
			{
				this.timer -= elapse;
				if (this.timer <= 0f)
				{
					if (this.senceInfo.Type == 6)
					{
						this.ShowDeadUI();
					}
					else
					{
						GameUIManager.mInstance.uiState.CurSceneInfo = Globals.Instance.SenceMgr.sceneInfo;
						GameAnalytics.OnFailScene(GameUIManager.mInstance.uiState.CurSceneInfo, GameAnalytics.ESceneFailed.CombatEffectiveness);
						Globals.Instance.SenceMgr.CloseScene();
						GameUIManager.mInstance.ChangeSession<GUIGameResultFailureScene>(null, false, false);
					}
				}
			}
			break;
		}
	}

	public override void Destroy()
	{
		this.respawnActors.Clear();
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		if (this.senceInfo.RespawnInfoID == 0)
		{
			while (groupID >= this.respawnActors.Count)
			{
				this.respawnActors.Add(null);
			}
		}
		else
		{
			int num = groupID;
			groupID = 0;
			if (this.respawnActors.Count <= 0)
			{
				this.respawnActors.Add(null);
			}
			TrialRespawnInfo info = Globals.Instance.AttDB.TrialRespawnDict.GetInfo(this.senceInfo.StartID);
			if (info == null)
			{
				global::Debug.LogErrorFormat("TrialRespawnDict.GetInfo error, ID = {0}", new object[]
				{
					this.senceInfo.StartID
				});
				return;
			}
			if (num >= info.InfoID.Count)
			{
				global::Debug.LogErrorFormat("index {0} >= maxCount {1}", new object[]
				{
					num,
					info.InfoID.Count
				});
				return;
			}
			infoID = info.InfoID[num];
			if (infoID == 0)
			{
				return;
			}
			scale = new Vector3(info.Scale[num], info.Scale[num], info.Scale[num]);
		}
		if (this.respawnActors[groupID] == null)
		{
			this.respawnActors[groupID] = new List<ActorController>();
		}
		ActorController actorController = this.actorMgr.CreateMonster(infoID, position, Quaternion.Euler(0f, rotationY, 0f), scale, 10000);
		if (actorController != null)
		{
			actorController.gameObject.SetActive(false);
			this.respawnActors[groupID].Add(actorController);
		}
	}

	public override void OnLoadRespawnOK()
	{
		if (!this.actorMgr.HasAssistant() && this.senceInfo.AssistantPetID != 0 && Globals.Instance.Player.GetSceneScore(this.senceInfo.ID) == 0)
		{
			this.actorMgr.SetAssistant(this.senceInfo.AssistantPetID, this.senceInfo.AssistantAttID);
		}
		this.actorMgr.CreateLocalActors();
		this.RespawnMonster();
	}

	public override void OnPreStart()
	{
		if (GameUIManager.mInstance.ShowSceneStartDialog(this.senceInfo.ID, new GUIPlotDialog.FinishCallback(base.DialogFinish)))
		{
			this.actorMgr.Pause(true);
		}
		else
		{
			base.DialogFinish();
		}
	}

	public override void OnStart()
	{
		this.status = 2;
		GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.START, 0);
		this.actorMgr.Actors[0].AiCtrler.EnableAI = GameCache.Data.EnableAI;
		if (this.actorMgr.Actors[0].AiCtrler.EnableAI)
		{
			this.actorMgr.Actors[0].UpdateSpeedScale(1.5f);
			Globals.Instance.CameraMgr.SetAutoCamValue();
		}
		else
		{
			Globals.Instance.CameraMgr.SetSingleCamValue();
		}
	}

	public override void OnChangeAIMode()
	{
		if (this.actorMgr.Actors[0].AiCtrler.EnableAI)
		{
			Globals.Instance.CameraMgr.SetAutoCamValue();
		}
		else
		{
			Globals.Instance.CameraMgr.SetSingleCamValue();
			if (this.actorMgr.Actors[0].CanMove(false, false))
			{
				this.actorMgr.Actors[0].StopMove();
			}
		}
		if (!this.actorMgr.Combat)
		{
			if (this.actorMgr.Actors[0].AiCtrler.EnableAI)
			{
				this.actorMgr.Actors[0].UpdateSpeedScale(1.5f);
			}
			else
			{
				this.actorMgr.Actors[0].UpdateSpeedScale(1f);
			}
		}
		else
		{
			this.actorMgr.Actors[0].UpdateSpeedScale(1f);
		}
	}

	public override void OnPlayerFindEnemy(ActorController actor)
	{
		Globals.Instance.CameraMgr.Target = actor.gameObject;
		Globals.Instance.CameraMgr.targetCamTest = true;
		for (int i = 1; i < 5; i++)
		{
			if (!(this.actorMgr.Actors[i] == null))
			{
				this.actorMgr.Actors[i].UpdateSpeedScale(1f);
				this.actorMgr.Actors[i].AiCtrler.SetTarget(actor);
			}
		}
		base.CheckBossDialog(true);
	}

	public override void OnPlayerDead()
	{
		if (this.status != 2)
		{
			return;
		}
		this.stopTimer = true;
		this.status = 4;
		this.timer = 3f;
	}

	public override void OnAllMonsterDead()
	{
		if (this.status != 2)
		{
			return;
		}
		this.respawnIndex++;
		while (this.respawnIndex < this.respawnActors.Count && this.respawnActors[this.respawnIndex] == null)
		{
			this.respawnIndex++;
		}
		if (this.respawnIndex >= this.respawnActors.Count)
		{
			this.respawnActors.Clear();
			Globals.Instance.GameMgr.SpeedDown(-0.6f, true);
			this.actorMgr.OnWin();
			this.status = 3;
			this.timer = 1f;
		}
		else
		{
			if (this.actorMgr.Actors[0].AiCtrler.EnableAI)
			{
				this.actorMgr.Actors[0].UpdateSpeedScale(1.5f);
			}
			for (int i = 0; i < 5; i++)
			{
				if (this.actorMgr.Actors[i] != null)
				{
					this.actorMgr.Actors[i].AiCtrler.ClearWarningFlag();
				}
			}
			this.RespawnMonster();
		}
	}

	protected void RespawnMonster()
	{
		while (this.respawnIndex < this.respawnActors.Count && this.respawnActors[this.respawnIndex] == null)
		{
			this.respawnIndex++;
		}
		if (this.respawnIndex >= this.respawnActors.Count)
		{
			return;
		}
		for (int i = 0; i < this.respawnActors[this.respawnIndex].Count; i++)
		{
			this.respawnActors[this.respawnIndex][i].gameObject.SetActive(true);
			this.actorMgr.Actors.Add(this.respawnActors[this.respawnIndex][i]);
		}
		this.respawnActors[this.respawnIndex].Clear();
	}

	public int GetRespawnIndex()
	{
		return this.respawnIndex;
	}
}
