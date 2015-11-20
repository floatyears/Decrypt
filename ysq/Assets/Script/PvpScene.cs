using Att;
using Proto;
using System;
using UnityEngine;

public class PvpScene : BaseScene
{
	protected RespawnPoint[] localRespawnPoints = new RespawnPoint[7];

	protected RespawnPoint[] remoteRespawnPoints = new RespawnPoint[7];

	protected ActorController[] remoteActor = new ActorController[4];

	protected bool win;

	public PvpScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		this.status = 1;
		this.maxCombatTimer = 180f;
		this.combatTimer = this.maxCombatTimer;
		this.win = false;
	}

	public override void Update(float elapse)
	{
		int status = this.status;
		if (status != 2)
		{
			if (status == 3)
			{
				if (this.timer > 0f)
				{
					this.timer -= elapse;
					if (this.timer <= 0f)
					{
						this.actorMgr.LockAllActorAI();
						this.actorMgr.PlayWin(true, this.win);
					}
				}
			}
		}
		else if (this.combatTimer > 0f)
		{
			this.combatTimer -= elapse;
			if (this.combatTimer <= 0f)
			{
				this.actorMgr.LockAllActorAI();
				this.status = 3;
				this.timer = 1f;
				this.win = false;
				return;
			}
		}
	}

	public override void Destroy()
	{
		for (int i = 0; i < 7; i++)
		{
			this.localRespawnPoints[i] = null;
			this.remoteRespawnPoints[i] = null;
		}
		for (int j = 0; j < 4; j++)
		{
			this.remoteActor[j] = null;
		}
	}

	public override void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
		if (groupID < 7)
		{
			if (groupID < 0)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Respawn error, groupID = {0}", groupID)
				});
				return;
			}
			if (this.localRespawnPoints[groupID] == null)
			{
				this.localRespawnPoints[groupID] = new RespawnPoint();
				if (this.localRespawnPoints[groupID] == null)
				{
					global::Debug.LogError(new object[]
					{
						"Allocate RespawnPoint error!"
					});
					return;
				}
			}
			this.localRespawnPoints[groupID].Position = position;
			this.localRespawnPoints[groupID].Rotation = Quaternion.Euler(0f, rotationY, 0f);
		}
		else
		{
			if (groupID >= 14)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Respawn error, groupID = {0}", groupID)
				});
				return;
			}
			groupID -= 7;
			if (this.remoteRespawnPoints[groupID] == null)
			{
				this.remoteRespawnPoints[groupID] = new RespawnPoint();
				if (this.remoteRespawnPoints[groupID] == null)
				{
					global::Debug.LogError(new object[]
					{
						"Allocate RespawnPoint error!"
					});
					return;
				}
			}
			this.remoteRespawnPoints[groupID].Position = position;
			this.remoteRespawnPoints[groupID].Rotation = Quaternion.Euler(0f, rotationY, 0f);
		}
	}

	public override void OnLoadRespawnOK()
	{
		int[] array = new int[6];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 1; i < 4; i++)
		{
			SocketDataEx socketDataEx = Globals.Instance.Player.TeamSystem.GetSocket(i);
			if (socketDataEx == null)
			{
				global::Debug.LogError(new object[]
				{
					"GetSocket error, slot = {0}",
					i
				});
			}
			else if (socketDataEx.GetPet() != null)
			{
				this.actorMgr.Actors[i] = this.actorMgr.CreateActor(socketDataEx, ActorController.EFactionType.EBlue, this.localRespawnPoints[0].Position, this.localRespawnPoints[0].Rotation);
				if (!(this.actorMgr.Actors[i] == null))
				{
					if (i < 6)
					{
						array[i] = this.actorMgr.Actors[i].GetPlayerSkillID();
					}
					if (this.actorMgr.Actors[i].IsMelee)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
		}
		array[0] = 16;
		this.actorMgr.Actors[0] = this.actorMgr.CreateLocalActor(0, this.localRespawnPoints[0].Position, this.localRespawnPoints[0].Rotation);
		if (this.actorMgr.Actors[0] != null)
		{
			int num5 = 9;
			Vector3 vector = CombatHelper.GetSlotPos(this.actorMgr.Actors[0].transform.position, this.actorMgr.Actors[0].transform, num5, false, false);
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
			{
				vector = navMeshHit.position;
			}
			this.actorMgr.LopetActor = this.actorMgr.CreateLocalLopet(vector, this.localRespawnPoints[0].Rotation);
			if (this.actorMgr.LopetActor != null)
			{
				this.actorMgr.LopetActor.AiCtrler.SetFellowSlot(num5);
				this.actorMgr.LopetActor.AiCtrler.Follow(this.actorMgr.Actors[0], num5);
				array[4] = this.actorMgr.LopetActor.GetPlayerSkillID();
				this.actorMgr.Actors[0].AddEP(GameConst.GetInt32(252));
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != 0)
				{
					this.actorMgr.Actors[0].AddSkill(j, array[j], true);
				}
			}
			this.actorMgr.Actors[0].AiCtrler.AssistSkill = 2;
		}
		for (int k = 0; k < array.Length; k++)
		{
			array[k] = 0;
		}
		for (int l = 1; l < 4; l++)
		{
			SocketDataEx socketDataEx = Globals.Instance.Player.TeamSystem.GetRemoteSocket(l);
			if (socketDataEx == null)
			{
				global::Debug.LogErrorFormat("GetRemoteSocket error, slot = {0}", new object[]
				{
					l
				});
			}
			else if (socketDataEx.GetPet() != null)
			{
				this.remoteActor[l] = this.actorMgr.CreateActor(socketDataEx, ActorController.EFactionType.ERed, this.remoteRespawnPoints[0].Position, this.remoteRespawnPoints[0].Rotation);
				if (!(this.remoteActor[l] == null))
				{
					if (l < 6)
					{
						array[l] = this.remoteActor[l].GetPlayerSkillID();
					}
					this.actorMgr.Actors.Add(this.remoteActor[l]);
					if (this.remoteActor[l].IsMelee)
					{
						num3++;
					}
					else
					{
						num4++;
					}
				}
			}
		}
		array[0] = 16;
		this.remoteActor[0] = this.actorMgr.CreateRemoteActor(0, this.remoteRespawnPoints[0].Position, this.remoteRespawnPoints[0].Rotation);
		if (this.remoteActor[0] != null)
		{
			int num5 = 9;
			Vector3 vector = CombatHelper.GetSlotPos(this.remoteActor[0].transform.position, this.remoteActor[0].transform, num5, false, false);
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
			{
				vector = navMeshHit.position;
			}
			this.actorMgr.RemoteLopetActor = this.actorMgr.CreateRemoteLopet(vector, this.remoteRespawnPoints[0].Rotation);
			if (this.actorMgr.RemoteLopetActor != null)
			{
				this.actorMgr.RemoteLopetActor.AiCtrler.SetFellowSlot(num5);
				this.actorMgr.RemoteLopetActor.AiCtrler.Follow(this.remoteActor[0], num5);
				array[4] = this.actorMgr.RemoteLopetActor.GetPlayerSkillID();
				this.remoteActor[0].AddEP(GameConst.GetInt32(252));
			}
			for (int m = 0; m < array.Length; m++)
			{
				if (array[m] != 0)
				{
					this.remoteActor[0].AddSkill(m, array[m], true);
				}
			}
			this.remoteActor[0].AiCtrler.AssistSkill = 2;
			this.actorMgr.Actors.Add(this.remoteActor[0]);
		}
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int n = 0;
		while (n < 4)
		{
			if (!(this.actorMgr.Actors[n] != null))
			{
				goto IL_717;
			}
			this.actorMgr.Actors[n].CastPassiveSkill();
			this.actorMgr.Actors[n].AiCtrler.Locked = true;
			this.actorMgr.Actors[n].AiCtrler.FindEnemyDistance = 3.40282347E+38f;
			int num5;
			Vector3 vector;
			NavMeshHit navMeshHit;
			if (n != 0 && !(this.actorMgr.Actors[0] == null))
			{
				if (this.actorMgr.Actors[n].IsMelee)
				{
					num6++;
					num5 = 100 + num * 10 + num6;
				}
				else
				{
					num7++;
					num5 = 200 + num2 * 10 + num7;
				}
				vector = CombatHelper.GetSlotPos(this.actorMgr.Actors[0].transform.position, this.actorMgr.Actors[0].transform, num5, false, false);
				if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
				{
					vector = navMeshHit.position;
				}
				this.actorMgr.Actors[n].NavAgent.Warp(vector);
				this.actorMgr.Actors[n].AiCtrler.SetFellowSlot(num5);
				goto IL_717;
			}
			IL_82E:
			n++;
			continue;
			IL_717:
			if (!(this.remoteActor[n] != null))
			{
				goto IL_82E;
			}
			this.remoteActor[n].CastPassiveSkill();
			this.remoteActor[n].AiCtrler.Locked = true;
			this.remoteActor[n].AiCtrler.FindEnemyDistance = 3.40282347E+38f;
			if (n == 0 || this.remoteActor[0] == null)
			{
				goto IL_82E;
			}
			if (this.remoteActor[n].IsMelee)
			{
				num8++;
				num5 = 100 + num3 * 10 + num8;
			}
			else
			{
				num9++;
				num5 = 200 + num4 * 10 + num9;
			}
			vector = CombatHelper.GetSlotPos(this.remoteActor[0].transform.position, this.remoteActor[0].transform, num5, false, false);
			if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
			{
				vector = navMeshHit.position;
			}
			this.remoteActor[n].NavAgent.Warp(vector);
			this.remoteActor[n].AiCtrler.SetFellowSlot(num5);
			goto IL_82E;
		}
		this.actorMgr.PlayerCtrler.SetControlLocked(true);
	}

	protected void HpMpMod()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.actorMgr.Actors[i] != null)
			{
				this.actorMgr.Actors[i].HandleAttMod(EAttMod.EAM_Pct, 1, 10000, true);
				this.actorMgr.Actors[i].CurHP = this.actorMgr.Actors[i].MaxHP;
			}
			if (this.remoteActor[i] != null)
			{
				this.remoteActor[i].HandleAttMod(EAttMod.EAM_Pct, 1, 10000, true);
				this.remoteActor[i].CurHP = this.remoteActor[i].MaxHP;
			}
		}
		if (this.actorMgr.Actors[0] != null)
		{
			this.actorMgr.Actors[0].MaxMP *= 3L;
			this.actorMgr.Actors[0].CurMP = this.actorMgr.Actors[0].MaxMP;
		}
		if (this.remoteActor[0] != null)
		{
			this.remoteActor[0].MaxMP *= 3L;
			this.remoteActor[0].CurMP = this.remoteActor[0].MaxMP;
		}
	}

	public override void OnPreStart()
	{
		GameUIManager.mInstance.ShowBattleCDMsg(null);
	}

	public override void OnStart()
	{
		this.status = 2;
		Globals.Instance.GameMgr.SpeedUp(0.2f, true);
		for (int i = 0; i < 4; i++)
		{
			if (this.actorMgr.Actors[i] != null)
			{
				this.actorMgr.Actors[i].AiCtrler.Locked = false;
				this.actorMgr.Actors[i].AiCtrler.EnableAI = true;
			}
			if (this.remoteActor[i] != null)
			{
				this.remoteActor[i].AiCtrler.Locked = false;
				this.remoteActor[i].AiCtrler.EnableAI = true;
			}
		}
		MC2S_PvpCombatStart ojb = new MC2S_PvpCombatStart();
		Globals.Instance.CliSession.Send(805, ojb);
	}

	public override void OnPlayerDead()
	{
		this.actorMgr.UnlockAllActorAI();
		this.actorMgr.PlayerCtrler.SetControlLocked(true);
		if (this.actorMgr.LopetActor != null)
		{
			this.actorMgr.LopetActor.gameObject.SetActive(false);
		}
		this.CheckResult();
	}

	public override void OnPetDead()
	{
		this.CheckResult();
	}

	public override void OnRemoteActorDead()
	{
		if (this.actorMgr.RemoteLopetActor != null && (this.remoteActor[0] == null || this.remoteActor[0].IsDead))
		{
			this.actorMgr.RemoteLopetActor.gameObject.SetActive(false);
		}
		this.CheckResult();
	}

	public override void OnPlayWinOver()
	{
		this.SendPvpResultMsg();
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 4.5f;
	}

	public override ActorController GetRemotePlayerActor()
	{
		return this.remoteActor[0];
	}

	public ActorController GetRemoteActor(int index)
	{
		if (index >= 4 || index < 0)
		{
			return null;
		}
		if (index >= this.remoteActor.Length)
		{
			return null;
		}
		return this.remoteActor[index];
	}

	private void CheckResult()
	{
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			if (this.actorMgr.Actors[i] != null && !this.actorMgr.Actors[i].IsDead)
			{
				flag = true;
				break;
			}
		}
		bool flag2 = false;
		for (int j = 0; j < this.remoteActor.Length; j++)
		{
			if (this.remoteActor[j] != null && !this.remoteActor[j].IsDead)
			{
				flag2 = true;
				break;
			}
		}
		if (flag && flag2)
		{
			return;
		}
		if (flag)
		{
			this.win = true;
		}
		else
		{
			this.win = false;
		}
		this.status = 3;
		this.timer = 1f;
	}

	public virtual void SendPvpResultMsg()
	{
	}

	public override void OnSkip(bool result)
	{
		if (result)
		{
			this.win = true;
		}
		else
		{
			this.win = false;
		}
		this.status = 3;
		this.timer = 0.01f;
	}
}
