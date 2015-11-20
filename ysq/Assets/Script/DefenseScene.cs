using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DefenseScene : BaseWorld
{
	private RespawnPoint[] respawnPoints = new RespawnPoint[15];

	private List<RespawnData>[] respawnActors = new List<RespawnData>[5];

	private int index;

	private float timeStamp;

	private float messageTimer;

	public DefenseScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		this.status = 1;
		this.findBossDialogID = 0;
		this.index = 0;
		this.timeStamp = 0f;
		this.messageTimer = 0f;
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
			if (this.messageTimer > 0f && Time.time >= this.messageTimer)
			{
				this.messageTimer = -1f;
				GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.WaveNum, this.index + 1);
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
					return;
				}
			}
			if (this.timeStamp > 0f)
			{
				float num = Time.time - this.timeStamp;
				if (num >= 0f && this.index < this.respawnActors.Length && this.respawnActors[this.index] != null)
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
			break;
		case 4:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				this.ShowDeadUI();
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
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		if (groupID >= 0 && groupID < 15)
		{
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
			return;
		}
		MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("MonsterDict.GetInfo error, {0}", infoID)
			});
			return;
		}
		GameObject gameObject = Res.Load<GameObject>(info.ResLoc, false);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Res.Load error, path = {0}", info.ResLoc)
			});
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.Euler(0f, rotationY, 0f)) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Instantiate error, path = {0}", info.ResLoc)
			});
			return;
		}
		gameObject2.layer = LayerDefine.CharLayer;
	}

	public override void OnLoadRespawnOK()
	{
		if (!this.actorMgr.HasAssistant() && this.senceInfo.AssistantPetID != 0 && Globals.Instance.Player.GetSceneScore(this.senceInfo.ID) == 0)
		{
			this.actorMgr.SetAssistant(this.senceInfo.AssistantPetID, this.senceInfo.AssistantAttID);
		}
		this.actorMgr.CreateLocalActors();
		this.CreateMonster();
	}

	public override void OnPreStart()
	{
		if (GameUIManager.mInstance.ShowSceneStartDialog(this.senceInfo.ID, new GUIPlotDialog.FinishCallback(base.DialogFinish2)))
		{
			this.actorMgr.Pause(true);
		}
		else
		{
			base.DialogFinish2();
		}
	}

	public override void OnStart()
	{
		this.status = 2;
		this.actorMgr.Actors[0].AiCtrler.EnableAI = GameCache.Data.EnableAI;
		this.actorMgr.Actors[0].AiCtrler.PlayerAIFindEnemyDistance = 7f;
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
		if (this.status != 2)
		{
			return;
		}
		this.actorMgr.LockAllActorAI();
		this.stopTimer = true;
		this.status = 4;
		this.timeStamp = Time.time + 3f;
	}

	public override void OnAllMonsterDead()
	{
		if (this.status != 2 || this.respawnActors[this.index].Count > 0)
		{
			return;
		}
		this.index++;
		if (this.index >= 5 || this.respawnActors[this.index] == null || this.respawnActors[this.index].Count == 0)
		{
			this.status = 3;
			this.actorMgr.OnWin();
			this.timeStamp = Time.time + 3f;
		}
		else
		{
			this.actorMgr.PlayerCtrler.ActorCtrler.AiCtrler.GoPoint(this.actorMgr.BornPosition, -1, 2f);
			this.StartNextWave(1.5f);
		}
	}

	private void CreateMonster()
	{
		int startID = this.GetStartID();
		for (int i = 0; i < 5; i++)
		{
			TrialRespawnInfo info = Globals.Instance.AttDB.TrialRespawnDict.GetInfo(startID + i);
			if (info == null)
			{
				if (this.respawnActors[i] != null)
				{
					this.respawnActors[i].Clear();
				}
				break;
			}
			if (this.respawnActors[i] == null)
			{
				this.respawnActors[i] = new List<RespawnData>();
			}
			this.respawnActors[i].Clear();
			for (int j = 0; j < info.InfoID.Count; j++)
			{
				if (j >= 15)
				{
					global::Debug.LogError(new object[]
					{
						string.Format("TrialRespawnInfo InfoID count = {0} >= {1}", j, 15)
					});
					break;
				}
				if (info.InfoID[j] != 0)
				{
					if (this.respawnPoints[j] == null)
					{
						global::Debug.LogError(new object[]
						{
							string.Format("respawnPoints error, index = {0}", j)
						});
						break;
					}
					RespawnData respawnData = new RespawnData();
					if (respawnData == null)
					{
						global::Debug.LogError(new object[]
						{
							"Allocate RespawnData error"
						});
						break;
					}
					respawnData.Actor = this.actorMgr.CreateMonster(info.InfoID[j], this.respawnPoints[j].Position, this.respawnPoints[j].Rotation, new Vector3(info.Scale[j], info.Scale[j], info.Scale[j]), 10000);
					if (respawnData.Actor != null)
					{
						respawnData.Actor.AiCtrler.FindEnemyDistance = 3.40282347E+38f;
						respawnData.Actor.gameObject.SetActive(false);
						respawnData.DelayTime = info.Delay[j];
						this.respawnActors[i].Add(respawnData);
					}
				}
			}
		}
	}

	private void StartNextWave(float delay)
	{
		this.messageTimer = Time.time + delay;
		this.timeStamp = this.messageTimer + 1.5f;
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 4.5f;
	}

	public virtual int GetStartID()
	{
		return this.senceInfo.StartID;
	}

	public override void OnPlayerFindEnemy(ActorController actor)
	{
		base.CheckBossDialog(true);
	}
}
