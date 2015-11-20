using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGearScene : BaseWorld
{
	public delegate void VoidCallback();

	public delegate void ValueCallback(EMGEventType type, int value);

	public MemoryGearScene.VoidCallback GearDamageEvent;

	public MemoryGearScene.VoidCallback WaveUpdateEvent;

	public MemoryGearScene.VoidCallback PlayerDeadEvent;

	public MemoryGearScene.ValueCallback CombatEvent;

	public MemoryGearScene.VoidCallback PlayerResurrectEvent;

	private MGInfo mgInfo;

	private RespawnPoint[,] respawnPoints = new RespawnPoint[3, 5];

	private List<RespawnData>[] respawnActors = new List<RespawnData>[6];

	private int[] way = new int[6];

	private int index;

	private int maxWave;

	private float timeStamp;

	private ActorController gearActor;

	private ActorController towerActor1;

	private ActorController towerActor2;

	private ActorController towerActor3;

	private int damageCount;

	private bool respawning;

	private bool repaired;

	private float respawnTimer;

	private ActorController playerActor;

	private float[] resurrectTimer = new float[5];

	public int CurWave
	{
		get
		{
			return this.index + 1;
		}
	}

	public int MaxWave
	{
		get
		{
			return this.maxWave;
		}
	}

	public ActorController GearActor
	{
		get
		{
			return this.gearActor;
		}
	}

	public int DamageCount
	{
		get
		{
			return this.damageCount;
		}
	}

	public int MaxDamageCount
	{
		get
		{
			if (this.mgInfo == null)
			{
				return 0;
			}
			return this.mgInfo.MaxValue;
		}
	}

	public float RespawnTimer
	{
		get
		{
			if (this.mgInfo == null || this.index >= this.mgInfo.Delay.Count)
			{
				return 0f;
			}
			return this.mgInfo.Delay[this.index] - this.respawnTimer;
		}
	}

	public MemoryGearScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		this.status = 1;
		this.mgInfo = Globals.Instance.AttDB.MGDict.GetInfo(GameUIManager.mInstance.uiState.PveSceneValue);
		if (this.mgInfo == null)
		{
			global::Debug.LogErrorFormat("MGDict.GetInfo error, id = {0}", new object[]
			{
				GameUIManager.mInstance.uiState.PveSceneValue
			});
			return;
		}
		this.index = 0;
		this.timeStamp = 0f;
		this.maxWave = 0;
		this.damageCount = 0;
		this.respawnTimer = 0f;
		this.respawning = false;
		this.repaired = false;
		this.combatTimer = 0f;
	}

	public override void Update(float elapse)
	{
		switch (this.status)
		{
		case 2:
			this.combatTimer += elapse;
			this.UpdateResurrect(elapse);
			if (this.damageCount >= this.MaxDamageCount)
			{
				if (this.CombatEvent != null)
				{
					this.CombatEvent(EMGEventType.EMGET_GearDead, 0);
				}
				this.actorMgr.LockAllActorAI();
				this.status = 5;
				this.timeStamp = Time.time + 3f;
				return;
			}
			if (this.index < this.maxWave)
			{
				this.respawnTimer += elapse;
				if (this.respawnTimer >= this.mgInfo.Delay[this.index])
				{
					this.respawnTimer = 0f;
					this.index++;
					if (this.index >= this.maxWave)
					{
						return;
					}
					this.respawning = true;
					this.repaired = false;
					if (this.WaveUpdateEvent != null)
					{
						this.WaveUpdateEvent();
					}
					if (this.CombatEvent != null)
					{
						if (this.index + 1 == this.maxWave)
						{
							this.CombatEvent(EMGEventType.EMGET_BossRespawn, this.way[this.index]);
						}
						else
						{
							this.CombatEvent(EMGEventType.EMGET_Respawn, this.way[this.index]);
						}
					}
				}
				if (this.respawning)
				{
					int num = 0;
					for (int i = 0; i < this.respawnActors[this.index].Count; i++)
					{
						if (this.respawnActors[this.index][i] != null)
						{
							if (this.respawnActors[this.index][i].Actor != null && this.respawnActors[this.index][i].DelayTime > 0f)
							{
								this.respawnActors[this.index][i].DelayTime -= elapse;
								if (this.respawnActors[this.index][i].DelayTime <= 0f)
								{
									this.respawnActors[this.index][i].Actor.gameObject.SetActive(true);
									NJGMapItem safeComponent = Tools.GetSafeComponent<NJGMapItem>(this.respawnActors[this.index][i].Actor.gameObject);
									if (this.respawnActors[this.index][i].Actor.monsterInfo.BossType != 0)
									{
										safeComponent.type = 4;
										safeComponent.showDeath = true;
									}
									else
									{
										safeComponent.type = 3;
									}
									this.actorMgr.Actors.Add(this.respawnActors[this.index][i].Actor);
									this.respawnActors[this.index][i].Actor.PlayAction("Skill/misc_004", null);
									this.respawnActors[this.index][i] = null;
								}
							}
						}
						else
						{
							num++;
						}
					}
					if (this.respawnActors[this.index].Count == num)
					{
						this.respawnActors[this.index].Clear();
						this.respawning = false;
					}
				}
				else if (!this.repaired && !this.HasMonster())
				{
					if (this.index + 1 == this.maxWave)
					{
						this.actorMgr.LockAllActorAI();
						Globals.Instance.GameMgr.SpeedDown(-0.6f, true);
						this.actorMgr.OnWin();
						this.status = 3;
						this.timeStamp = 1f;
						return;
					}
					if (this.respawnTimer + 6f < this.mgInfo.Delay[this.index])
					{
						this.respawnTimer = this.mgInfo.Delay[this.index] - 6f;
						this.respawning = false;
					}
					this.repaired = true;
				}
			}
			else if (!this.HasMonster())
			{
				this.actorMgr.LockAllActorAI();
				Globals.Instance.GameMgr.SpeedDown(-0.6f, true);
				this.actorMgr.OnWin();
				this.status = 3;
				this.timeStamp = 1f;
				return;
			}
			break;
		case 3:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				Globals.Instance.GameMgr.SpeedDown(-0.6f, false);
				if (this.playerActor != null && this.playerActor.IsDead)
				{
					Globals.Instance.ActorMgr.Resurrect(true);
					Globals.Instance.ActorMgr.LockAllActorAI();
					if (this.PlayerResurrectEvent != null)
					{
						this.PlayerResurrectEvent();
					}
					Array.Clear(this.resurrectTimer, 0, this.resurrectTimer.Length);
				}
				this.actorMgr.GoPosePoint();
			}
			break;
		case 5:
			if (this.timeStamp > 0f && Time.time >= this.timeStamp)
			{
				this.timeStamp = -1f;
				GameUIManager.mInstance.uiState.CurSceneInfo = Globals.Instance.SenceMgr.sceneInfo;
				base.SendFailLog(GameUIManager.mInstance.uiState.CurSceneInfo);
				GameAnalytics.OnFailScene(GameUIManager.mInstance.uiState.CurSceneInfo, GameAnalytics.ESceneFailed.CombatEffectiveness);
				Globals.Instance.SenceMgr.CloseScene();
				GUIGuardResultScene.ShowResult(null);
			}
			break;
		}
	}

	public override void Destroy()
	{
		GameUIManager.mInstance.CloseBattleCDMsg();
		Array.Clear(this.respawnPoints, 0, this.respawnPoints.Length);
		Array.Clear(this.way, 0, this.way.Length);
		Array.Clear(this.resurrectTimer, 0, this.resurrectTimer.Length);
		for (int i = 0; i < 6; i++)
		{
			if (this.respawnActors[i] != null)
			{
				this.respawnActors[i].Clear();
			}
		}
		this.index = 0;
		this.maxWave = 0;
		this.timeStamp = 0f;
		this.damageCount = 0;
		this.combatTimer = 0f;
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		if (groupID > 0 && groupID < 100)
		{
			switch (groupID)
			{
			case 1:
				this.gearActor = this.actorMgr.CreateBuilding(90083, position, Quaternion.Euler(0f, rotationY, 0f), scale, ActorController.EFactionType.EBlue, false, false);
				if (this.gearActor != null)
				{
					this.gearActor.Unattacked = true;
				}
				break;
			case 2:
				this.towerActor1 = this.actorMgr.CreateBuilding(this.mgInfo.TowerID1, position, Quaternion.Euler(0f, rotationY, 0f), scale, ActorController.EFactionType.EBlue, true, true);
				if (this.towerActor1 != null)
				{
					this.towerActor1.Unhealed = true;
					((AITower)this.towerActor1.AiCtrler).SetWayIndex(0);
					this.actorMgr.Actors.Add(this.towerActor1);
				}
				break;
			case 3:
				this.towerActor2 = this.actorMgr.CreateBuilding(this.mgInfo.TowerID2, position, Quaternion.Euler(0f, rotationY, 0f), scale, ActorController.EFactionType.EBlue, true, true);
				if (this.towerActor2 != null)
				{
					this.towerActor2.Unhealed = true;
					((AITower)this.towerActor2.AiCtrler).SetWayIndex(1);
					this.actorMgr.Actors.Add(this.towerActor2);
				}
				break;
			case 4:
				this.towerActor3 = this.actorMgr.CreateBuilding(this.mgInfo.TowerID3, position, Quaternion.Euler(0f, rotationY, 0f), scale, ActorController.EFactionType.EBlue, true, true);
				if (this.towerActor3 != null)
				{
					this.towerActor3.Unhealed = true;
					((AITower)this.towerActor3.AiCtrler).SetWayIndex(2);
					this.actorMgr.Actors.Add(this.towerActor3);
				}
				break;
			}
		}
		else if (groupID > 100)
		{
			int num = groupID / 100;
			int num2 = groupID % 100;
			if (num <= 0 || num > 3 || num2 <= 0 || num2 > 5)
			{
				global::Debug.LogErrorFormat("groupID invalid, groupID = {0}", new object[]
				{
					groupID
				});
				return;
			}
			if (this.respawnPoints[num - 1, num2 - 1] == null)
			{
				this.respawnPoints[num - 1, num2 - 1] = new RespawnPoint();
				if (this.respawnPoints[num - 1, num2 - 1] == null)
				{
					global::Debug.LogError(new object[]
					{
						"Allocate RespawnPoint error!"
					});
					return;
				}
			}
			this.respawnPoints[num - 1, num2 - 1].Position = position;
			this.respawnPoints[num - 1, num2 - 1].Rotation = Quaternion.Euler(0f, rotationY, 0f);
		}
	}

	public override void OnLoadRespawnOK()
	{
		this.actorMgr.CreateLocalActors();
		this.CreateMonster();
		this.playerActor = Globals.Instance.ActorMgr.GetActor(0);
	}

	public override void OnPreStart()
	{
		GameUIManager.mInstance.ShowBattleCDMsg(null);
		this.actorMgr.Actors[0].AiCtrler.EnableAI = GameCache.Data.EnableAI;
	}

	public override void OnStart()
	{
		this.status = 2;
		this.respawnTimer = 0f;
		this.respawning = true;
		this.repaired = false;
		if (this.CombatEvent != null)
		{
			this.CombatEvent(EMGEventType.EMGET_Respawn, this.way[this.index]);
		}
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
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < 5; i++)
		{
			if (!(actors[i] == null) && !actors[i].IsDead)
			{
				actors[i].DoDamage(actors[i].MaxHP * 2L, null, false);
			}
		}
		this.ClearMonsterThreat();
		this.resurrectTimer[0] = 5f;
		if (this.PlayerDeadEvent != null)
		{
			this.PlayerDeadEvent();
		}
	}

	public override void OnPetDead()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < 5; i++)
		{
			if (!(actors[i] == null) && actors[i].IsDead && this.resurrectTimer[i] <= 0f)
			{
				this.resurrectTimer[i] = 20f;
			}
		}
	}

	private void CreateMonster()
	{
		if (this.mgInfo == null)
		{
			return;
		}
		this.index = 0;
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (i >= this.mgInfo.RespawnID.Count || this.mgInfo.RespawnID[i] == 0)
			{
				break;
			}
			MGRespawnInfo info = Globals.Instance.AttDB.MGRespawnDict.GetInfo(this.mgInfo.RespawnID[i]);
			if (info == null)
			{
				break;
			}
			if (this.respawnActors[this.maxWave] == null)
			{
				this.respawnActors[this.maxWave] = new List<RespawnData>();
			}
			int num2 = UtilFunc.RangeRandom(0, 100) % 3;
			this.way[i] = num2;
			if (i <= 0)
			{
				num = 1;
			}
			else
			{
				if (this.way[i - 1] == num2)
				{
					num++;
				}
				else
				{
					num = 1;
				}
				if (num >= 3)
				{
					num2 = (num2 + 1) % 3;
					this.way[i] = num2;
					num = 1;
				}
			}
			this.timer += this.mgInfo.Delay[i];
			for (int j = 0; j < info.InfoID.Count; j++)
			{
				if (info.InfoID[j] != 0)
				{
					int num3 = info.PosIndex[i];
					if (num3 > 5)
					{
						global::Debug.LogErrorFormat("MGRespawnInfo InfoID count = {0} > {1}", new object[]
						{
							j,
							5
						});
						break;
					}
					if (this.respawnPoints[num2, num3] == null)
					{
						global::Debug.LogError(new object[]
						{
							string.Format("respawnPoints error, wayIndex = {0}, posIndex = {1}", num2, num3)
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
					respawnData.Actor = this.actorMgr.CreateMonster(info.InfoID[j], this.respawnPoints[num2, num3].Position, this.respawnPoints[num2, num3].Rotation, new Vector3(info.Scale[j], info.Scale[j], info.Scale[j]), 10000);
					if (respawnData.Actor != null)
					{
						respawnData.Actor.AiCtrler.FindEnemyDistance = 3.40282347E+38f;
						respawnData.Actor.AiCtrler.SetInitTarget(this.gearActor);
						respawnData.Actor.gameObject.SetActive(false);
						respawnData.DelayTime = info.Delay[j];
						this.respawnActors[this.maxWave].Add(respawnData);
					}
				}
			}
			this.maxWave++;
		}
		this.index = 0;
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 4.5f;
	}

	public void AddDamageCount(int count)
	{
		this.damageCount += count;
		if (this.damageCount > this.MaxDamageCount)
		{
			this.damageCount = this.MaxDamageCount;
			return;
		}
		if (this.GearDamageEvent != null)
		{
			this.GearDamageEvent();
		}
		if (this.CombatEvent != null)
		{
			this.CombatEvent(EMGEventType.EMGET_GearDamaged, count);
		}
	}

	private bool HasMonster()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 5; i < actors.Count; i++)
		{
			if (!(actors[i] == null) && actors[i].FactionType != ActorController.EFactionType.EBlue)
			{
				return true;
			}
		}
		return false;
	}

	public ActorController FindTower(ActorController actor, float findDist)
	{
		if (actor == null)
		{
			return null;
		}
		float num = 3.40282347E+38f;
		ActorController result = null;
		if (this.towerActor1 != null && !this.towerActor1.IsDead)
		{
			float distance2D = actor.GetDistance2D(this.towerActor1);
			if (distance2D <= findDist && distance2D < num)
			{
				num = distance2D;
				result = this.towerActor1;
			}
		}
		if (this.towerActor2 != null && !this.towerActor2.IsDead)
		{
			float distance2D = actor.GetDistance2D(this.towerActor2);
			if (distance2D <= findDist && distance2D < num)
			{
				num = distance2D;
				result = this.towerActor2;
			}
		}
		if (this.towerActor3 != null && !this.towerActor3.IsDead)
		{
			float distance2D = actor.GetDistance2D(this.towerActor3);
			if (distance2D <= findDist && distance2D < num)
			{
				result = this.towerActor3;
			}
		}
		return result;
	}

	private void ClearMonsterThreat()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 5; i < actors.Count; i++)
		{
			if (!(actors[i] == null) && actors[i].FactionType != ActorController.EFactionType.EBlue)
			{
				actors[i].AiCtrler.LeaveCombat();
				actors[i].AiCtrler.SetInitTarget(this.gearActor);
			}
		}
	}

	public void UpdateTarget(ActorController actor, float findDist)
	{
		actor.AiCtrler.SetInitTarget(this.gearActor);
	}

	private void UpdateResurrect(float elapse)
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.resurrectTimer[i] > 0f)
			{
				this.resurrectTimer[i] -= elapse;
				if (this.resurrectTimer[i] < 0f)
				{
					if (i == 0)
					{
						if (this.playerActor != null && this.playerActor.IsDead)
						{
							this.playerActor.NavAgent.Warp(Globals.Instance.ActorMgr.BornPosition);
							this.playerActor.transform.rotation = Quaternion.Euler(0f, Globals.Instance.ActorMgr.BornRotationY, 0f);
							Globals.Instance.ActorMgr.Resurrect(true);
							if (this.PlayerResurrectEvent != null)
							{
								this.PlayerResurrectEvent();
							}
							Array.Clear(this.resurrectTimer, 0, this.resurrectTimer.Length);
							break;
						}
					}
					else
					{
						Globals.Instance.ActorMgr.PetResurrect(i);
						if (this.PlayerResurrectEvent != null)
						{
							this.PlayerResurrectEvent();
						}
					}
				}
				if (i == 0)
				{
					break;
				}
			}
		}
	}

	public void DestroyAllTower()
	{
		if (this.towerActor1 != null)
		{
			this.towerActor1.DoDamage(this.towerActor1.MaxHP * 2L, null, false);
		}
		if (this.towerActor2 != null)
		{
			this.towerActor2.DoDamage(this.towerActor2.MaxHP * 2L, null, false);
		}
		if (this.towerActor3 != null)
		{
			this.towerActor3.DoDamage(this.towerActor3.MaxHP * 2L, null, false);
		}
	}

	public override float GetCombatTimer()
	{
		return 0f;
	}

	public override float GetMaxCombatTimer()
	{
		return this.combatTimer;
	}
}
