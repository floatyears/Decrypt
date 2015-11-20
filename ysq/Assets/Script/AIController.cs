using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Character/AIController")]
public class AIController : MonoBehaviour
{
	protected ActorController actorCtrler;

	protected ThreatMgr threatMgr;

	protected ActorController target;

	public ActorController selectTarget;

	public bool EnableAI = true;

	public float PlayerAIFindEnemyDistance = 3.40282347E+38f;

	protected bool autoAttack = true;

	public float AttackDistance = 1f;

	public float FindEnemyDistance = 5f;

	private float findEnemyTimer;

	private float threatTimer;

	private ActorController[] meleeChaser = new ActorController[12];

	private ActorController[] rangeChaser = new ActorController[24];

	private MoveState[] moveStates = new MoveState[3];

	private int curSlot;

	private MoveStateIdle idleMove;

	private MoveStateFollow followMove;

	private MoveStateTarget targetMove;

	private MoveStatePoint pointMove;

	private MoveStateFear fearMove;

	private int followSlot;

	private bool warningFlag;

	public bool Locked;

	public bool Win;

	public bool ForceFollow;

	private bool cacheSkillFlag;

	private bool healSkill;

	private bool enterCombat;

	private bool buffSkill;

	private bool takenDamage;

	public int AssistSkill;

	private float delayTimer;

	private int delayIndex;

	private int delayCastType;

	public ThreatMgr ThreatMgr
	{
		get
		{
			return this.threatMgr;
		}
	}

	public ActorController Target
	{
		get
		{
			return this.target;
		}
	}

	public int FollowSlot
	{
		get
		{
			return this.followSlot;
		}
	}

	public virtual void Init()
	{
		this.actorCtrler = base.GetComponent<ActorController>();
		this.threatMgr = new ThreatMgr(this.actorCtrler);
	}

	private void Update()
	{
		if (this.actorCtrler == null || this.actorCtrler.IsDead)
		{
			return;
		}
		if (this.Locked)
		{
			if (this.actorCtrler.ActorType == ActorController.EActorType.EPlayer && !this.Win)
			{
				this.UpdateFindEnemy(Time.deltaTime);
			}
			if (this.curSlot == 2 && this.moveStates[this.curSlot] != null)
			{
				this.moveStates[this.curSlot].Update(Time.deltaTime);
			}
			return;
		}
		this.UpdateMoveState(Time.deltaTime);
		if (this.actorCtrler.ActorType == ActorController.EActorType.ELopet)
		{
			return;
		}
		if (this.Win)
		{
			return;
		}
		this.UpdateFindEnemy(Time.deltaTime);
		if (this.actorCtrler.ActorType == ActorController.EActorType.EPlayer)
		{
			this.UpdateWarning(Time.deltaTime);
			this.UpdatePlayerAttack();
		}
		else
		{
			this.UpdateTarget(Time.deltaTime);
			this.UpdateAttack(Time.deltaTime);
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < this.moveStates.Length; i++)
		{
			this.moveStates[i] = null;
		}
		this.idleMove = null;
		this.followMove = null;
		this.targetMove = null;
	}

	public void SetTarget(ActorController actor)
	{
		if (this.target == actor)
		{
			return;
		}
		bool flag = false;
		if (this.target != null)
		{
			flag = true;
		}
		this.target = actor;
		if (this.actorCtrler.ActorType == ActorController.EActorType.EPlayer)
		{
			if (this.actorCtrler.FactionType == ActorController.EFactionType.EBlue)
			{
				Globals.Instance.ActorMgr.OnSelect(this.target);
			}
			if (this.target != null)
			{
				if (this.EnableAI)
				{
					this.OnTargetChange(this.target);
				}
			}
			else if (this.curSlot > 0 && this.curSlot <= 1 && this.moveStates[this.curSlot] == this.targetMove)
			{
				this.Expired();
			}
		}
		else if (this.target != null)
		{
			if (!this.enterCombat)
			{
				this.enterCombat = true;
				this.actorCtrler.InitMonsterSkillCD();
			}
			if (!flag)
			{
				this.OnFindEnemy(this.target);
			}
			else
			{
				this.OnTargetChange(this.target);
			}
		}
	}

	public void SetSelectTarget(ActorController actor)
	{
		if (this.selectTarget == actor)
		{
			return;
		}
		this.selectTarget = actor;
		if (this.selectTarget != null)
		{
			this.SetTarget(this.selectTarget);
		}
	}

	private void SetMoveState(MoveState state, int slot)
	{
		if (state == null || slot < 0 || slot >= 3)
		{
			return;
		}
		if (this.curSlot < slot)
		{
			this.curSlot = slot;
		}
		if (this.moveStates[slot] != state)
		{
			if (this.moveStates[slot] != null)
			{
				this.moveStates[slot].Exit();
			}
			this.moveStates[slot] = state;
		}
		if (this.curSlot == slot)
		{
			state.Enter();
		}
	}

	protected void UpdateMoveState(float elapse)
	{
		if (this.moveStates[this.curSlot] != null && !this.moveStates[this.curSlot].Update(elapse))
		{
			this.Expired();
		}
	}

	private void Expired()
	{
		if (this.curSlot > 0 && this.curSlot < 3 && this.moveStates[this.curSlot] != null)
		{
			this.moveStates[this.curSlot].Exit();
			this.moveStates[this.curSlot] = null;
			this.curSlot--;
		}
		while (this.curSlot >= 0 && this.moveStates[this.curSlot] == null)
		{
			this.curSlot--;
		}
		if (this.curSlot < 0)
		{
			this.ClearMoveState();
			this.Idle();
		}
		else if (!this.Locked)
		{
			this.moveStates[this.curSlot].Enter();
		}
	}

	private void ClearMoveState()
	{
		for (int i = 2; i >= 0; i--)
		{
			if (this.moveStates[i] != null)
			{
				this.moveStates[i].Exit();
				this.moveStates[i] = null;
			}
		}
	}

	public void Idle()
	{
		if (this.idleMove == null)
		{
			this.idleMove = new MoveStateIdle(this.actorCtrler);
		}
		this.SetMoveState(this.idleMove, 0);
	}

	public void Follow(ActorController actor, int slot)
	{
		if (this.followMove == null)
		{
			this.followMove = new MoveStateFollow(this.actorCtrler);
		}
		this.followSlot = slot;
		this.followMove.SetTarget(actor, slot);
		this.SetMoveState(this.followMove, 0);
	}

	public void FollowForce(ActorController actor, int slot)
	{
		if (this.followMove == null)
		{
			this.followMove = new MoveStateFollow(this.actorCtrler);
		}
		this.followSlot = slot;
		this.followMove.SetTarget(actor, slot);
		this.SetMoveState(this.followMove, 1);
	}

	public void Chase(ActorController actor)
	{
		if (this.ForceFollow)
		{
			return;
		}
		if (this.targetMove == null)
		{
			this.targetMove = new MoveStateTarget(this.actorCtrler);
		}
		this.targetMove.SetTarget(actor, this.AttackDistance);
		this.SetMoveState(this.targetMove, 1);
	}

	public void GoPoint(Vector3 targetPos, int pointID, float timeout = 2f)
	{
		if (this.pointMove == null)
		{
			this.pointMove = new MoveStatePoint(this.actorCtrler);
		}
		this.pointMove.SetTarget(targetPos, pointID, timeout);
		this.SetMoveState(this.pointMove, 1);
	}

	public void Fear(float raduis = 1.2f)
	{
		if (this.fearMove == null)
		{
			this.fearMove = new MoveStateFear(this.actorCtrler);
		}
		this.fearMove.SetCenterPos(base.transform.position, raduis);
		this.SetMoveState(this.fearMove, 2);
	}

	public void CancelFear()
	{
		if (this.curSlot == 2 && this.moveStates[this.curSlot] == this.fearMove)
		{
			this.Expired();
		}
	}

	public void StartAttack()
	{
		this.autoAttack = true;
	}

	public void StopAttack()
	{
		this.autoAttack = false;
	}

	public static ActorController FindMinDistEnemy(ActorController actor, float findDist)
	{
		if (actor == null)
		{
			return null;
		}
		float num = 3.40282347E+38f;
		ActorController result = null;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !actorController.IsDead && (!actorController.IsImmunity || actorController.ActorType != ActorController.EActorType.EMonster) && actor.IsHostileTo(actorController))
			{
				float num2 = actor.GetDistance2D(actorController);
				if (num2 <= findDist)
				{
					if (actorController.ActorType == ActorController.EActorType.EPlayer)
					{
						num2 *= 1.5f;
						if (num2 < 1.5f)
						{
							num2 = 1.5f;
						}
					}
					if (num2 < num)
					{
						num = num2;
						result = actorController;
					}
				}
			}
		}
		return result;
	}

	public ActorController FindHealTarget()
	{
		float num = 1f;
		ActorController result = null;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !actorController.IsDead && !actorController.Unhealed && this.actorCtrler.IsFriendlyTo(actorController) && actorController.MaxHP != actorController.CurHP)
			{
				if (this.actorCtrler.GetDistance2D(actorController) <= 8f)
				{
					float num2 = (float)actorController.CurHP / (float)actorController.MaxHP;
					if ((double)num2 < 0.8 && num2 < num)
					{
						num = num2;
						result = actorController;
					}
				}
			}
		}
		return result;
	}

	protected void UpdateFindEnemy(float elapse)
	{
		if (this.ForceFollow)
		{
			return;
		}
		if (this.selectTarget != null && !this.selectTarget.IsDead && this.Target != null)
		{
			return;
		}
		float findDist = this.FindEnemyDistance;
		if (this.actorCtrler.ActorType == ActorController.EActorType.EPlayer)
		{
			if (this.EnableAI)
			{
				findDist = this.PlayerAIFindEnemyDistance;
			}
			else
			{
				findDist = this.AttackDistance;
			}
		}
		else
		{
			if (this.actorCtrler.ActorType == ActorController.EActorType.EMonster && this.takenDamage)
			{
				findDist = 3.40282347E+38f;
			}
			if (!(this.Target == null) && !this.Target.IsDead && (!this.Target.IsImmunity || this.Target.ActorType != ActorController.EActorType.EMonster))
			{
				return;
			}
		}
		this.findEnemyTimer += elapse;
		if (this.findEnemyTimer > 0.3f)
		{
			this.findEnemyTimer = 0f;
			if (this.actorCtrler.FactionType == ActorController.EFactionType.ERed && Globals.Instance.ActorMgr.MemoryGearScene != null && Globals.Instance.ActorMgr.CurScene == Globals.Instance.ActorMgr.MemoryGearScene)
			{
				this.threatTimer = 0f;
				ActorController actorController = Globals.Instance.ActorMgr.MemoryGearScene.GearActor;
				if (actorController == null)
				{
					return;
				}
				if (this.actorCtrler.IsHostileTo(actorController))
				{
					this.actorCtrler.AiCtrler.SetInitTarget(actorController);
				}
				else
				{
					actorController = AIController.FindMinDistEnemy(this.actorCtrler, 3.40282347E+38f);
					if (actorController == null)
					{
						this.Expired();
						return;
					}
					this.SetTarget(actorController);
				}
				return;
			}
			else
			{
				ActorController actorController2 = AIController.FindMinDistEnemy(this.actorCtrler, findDist);
				this.SetTarget(actorController2);
				if (actorController2 != null && this.actorCtrler.ActorType == ActorController.EActorType.EMonster)
				{
					List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
					for (int i = 0; i < actors.Count; i++)
					{
						ActorController actorController3 = actors[i];
						if (actorController3 && !actorController3.IsDead && actorController3.ActorType == ActorController.EActorType.EMonster && !(actorController3.AiCtrler.Target != null) && actorController2.IsHostileTo(actorController3))
						{
							actorController3.AiCtrler.SetTarget(actorController2);
						}
					}
				}
			}
		}
	}

	public void CacheSkillFlag()
	{
		if (!this.cacheSkillFlag)
		{
			this.cacheSkillFlag = true;
			for (int i = 1; i < this.actorCtrler.Skills.Length; i++)
			{
				if (this.actorCtrler.Skills[i] != null && this.actorCtrler.Skills[i].Info.CastType == 0)
				{
					if (this.actorCtrler.Skills[i].Info.CastTargetType == 0 && (this.actorCtrler.Skills[i].Info.EffectTargetType == 3 || this.actorCtrler.Skills[i].Info.EffectTargetType == 1))
					{
						for (int j = 0; j < this.actorCtrler.Skills[i].Info.EffectType.Count; j++)
						{
							if (this.actorCtrler.Skills[i].Info.EffectType[j] == 2)
							{
								this.buffSkill = true;
								BuffInfo info = Globals.Instance.AttDB.BuffDict.GetInfo(this.actorCtrler.Skills[i].Info.Value3[j]);
								if (info != null)
								{
									for (int k = 0; k < info.EffectType.Count; k++)
									{
										if (info.EffectType[k] == 10)
										{
											this.actorCtrler.Skills[i].Effect |= 1;
											break;
										}
									}
								}
							}
							else if (this.actorCtrler.Skills[i].Info.EffectType[j] == 7)
							{
								SkillInfo info2 = Globals.Instance.AttDB.SkillDict.GetInfo(this.actorCtrler.Skills[i].Info.Value3[j]);
								if (info2 != null)
								{
									for (int l = 0; l < info2.EffectType.Count; l++)
									{
										if (info2.EffectType[l] == 6)
										{
											this.healSkill = true;
											this.actorCtrler.Skills[i].Effect |= 2;
										}
										else if (info2.EffectType[l] == 4 || info2.EffectType[l] == 5)
										{
											this.actorCtrler.Skills[i].Effect |= 4;
										}
									}
								}
							}
						}
					}
					else
					{
						if (this.actorCtrler.Skills[i].Info.CastTargetType == 2)
						{
							this.healSkill = true;
						}
						if (this.actorCtrler.Skills[i].Info.CastTargetType == 3)
						{
							for (int m = 0; m < this.actorCtrler.Skills[i].Info.EffectType.Count; m++)
							{
								if (this.actorCtrler.Skills[i].Info.EffectType[m] == 7)
								{
									SkillInfo info2 = Globals.Instance.AttDB.SkillDict.GetInfo(this.actorCtrler.Skills[i].Info.Value3[m]);
									if (info2 != null)
									{
										for (int n = 0; n < info2.EffectType.Count; n++)
										{
											if (info2.EffectType[n] == 6)
											{
												this.healSkill = true;
												this.actorCtrler.Skills[i].Effect |= 2;
											}
											else if (info2.EffectType[n] == 4 || info2.EffectType[n] == 5)
											{
												this.actorCtrler.Skills[i].Effect |= 4;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public static Vector3 GetPlayerAOEPos(ActorController targetActor, ActorController.EFactionType factionType)
	{
		Vector3 a = targetActor.transform.position;
		int num = 1;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		int num2;
		int num3;
		if (factionType == ActorController.EFactionType.EBlue)
		{
			num2 = 5;
			num3 = actors.Count;
		}
		else
		{
			num2 = 0;
			num3 = 5;
		}
		for (int i = num2; i < num3; i++)
		{
			if (actors[i] != null && actors[i] != targetActor && CombatHelper.DistanceSquared2D(targetActor.transform.position, actors[i].transform.position) < 6.25f)
			{
				a += actors[i].transform.position;
				num++;
			}
		}
		return a / (float)num;
	}

	private void UpdatePlayerAttack()
	{
		if (this.ForceFollow || !this.autoAttack || this.actorCtrler.LockSkillIndex != -1 || this.actorCtrler.ActorType != ActorController.EActorType.EPlayer)
		{
			return;
		}
		bool flag = false;
		if ((this.AssistSkill & 2) != 0 || GameCache.Data.AutoSkill)
		{
			flag = true;
		}
		if (flag && this.EnableAI && !this.cacheSkillFlag)
		{
			this.CacheSkillFlag();
		}
		ECastSkillResult eCastSkillResult;
		if (this.healSkill && this.EnableAI && flag)
		{
			ActorController x = this.FindHealTarget();
			if (x != null)
			{
				for (int i = 0; i < this.actorCtrler.Skills.Length; i++)
				{
					if (this.actorCtrler.CanSkillReady(i))
					{
						if (this.actorCtrler.Skills[i].Info.CastTargetType == 2 || (this.actorCtrler.Skills[i].Effect & 2) != 0)
						{
							eCastSkillResult = this.actorCtrler.TryCastSkill(i, x);
							if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
							{
								return;
							}
						}
					}
				}
			}
		}
		if (this.Target == null || this.Target.IsDead || this.Target.Unattacked)
		{
			return;
		}
		if (this.Target.NavAgent != null && this.Target.NavAgent.velocity.sqrMagnitude <= 0f && this.actorCtrler.NavAgent != null && this.actorCtrler.NavAgent.velocity.sqrMagnitude > 0f)
		{
			return;
		}
		if (this.actorCtrler.GetDistance2D(this.Target) - 0.7f > this.AttackDistance)
		{
			return;
		}
		if (this.buffSkill && this.EnableAI && flag)
		{
			for (int j = 0; j < this.actorCtrler.Skills.Length; j++)
			{
				if (this.actorCtrler.CanSkillReady(j))
				{
					if ((this.actorCtrler.Skills[j].Effect & 1) != 0)
					{
						if (this.actorCtrler.IsImmunity)
						{
                            continue;
						}
						if (this.actorCtrler.Skills[j].Info.EffectTargetType == 3)
						{
							bool flag2 = false;
							for (int k = 1; k < 5; k++)
							{
								ActorController actor = Globals.Instance.ActorMgr.GetActor(k);
								if (actor != null && actor.IsImmunity)
								{
									flag2 = true;
									break;
								}
							}
							if (flag2)
							{
                                continue;
							}
						}
					}
					if (this.actorCtrler.Skills[j].Info.CastTargetType == 0 && (this.actorCtrler.Skills[j].Info.EffectTargetType == 3 || this.actorCtrler.Skills[j].Info.EffectTargetType == 1))
					{
						eCastSkillResult = this.actorCtrler.TryCastSkill(j, this.actorCtrler);
						if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
						{
							return;
						}
					}
				}
			}
		}
		if (this.EnableAI && flag && !this.Target.IsBox && !this.Target.IsImmunity)
		{
			if (this.Target.IsBoss || this.Target.CurHP * 100L / this.Target.MaxHP > 20L)
			{
				for (int l = 1; l < this.actorCtrler.Skills.Length; l++)
				{
					if (this.actorCtrler.CanSkillReady(l))
					{
						if (this.actorCtrler.Skills[l].Effect != 2)
						{
							if (this.actorCtrler.Skills[l].Info.CastTargetType == 3)
							{
								Vector3 playerAOEPos = AIController.GetPlayerAOEPos(this.Target, this.actorCtrler.FactionType);
								eCastSkillResult = this.actorCtrler.TryCastSkill(l, playerAOEPos);
								if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
								{
									return;
								}
							}
						}
					}
				}
			}
			for (int m = 1; m < this.actorCtrler.Skills.Length; m++)
			{
				if (this.actorCtrler.CanSkillReady(m))
				{
					if (this.actorCtrler.Skills[m].Info.CastTargetType != 3)
					{
						if (this.actorCtrler.Skills[m].Info.CastTargetType == 0)
						{
							eCastSkillResult = this.actorCtrler.TryCastSkill(m, this.actorCtrler);
						}
						else
						{
							eCastSkillResult = this.actorCtrler.TryCastSkill(m, this.Target);
						}
						if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
						{
							return;
						}
					}
				}
			}
		}
		eCastSkillResult = this.actorCtrler.TryCastSkill(0, this.Target);
		if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
		{
			return;
		}
	}

	protected void UpdateAttack(float elapse)
	{
		if (this.ForceFollow || !this.autoAttack || this.actorCtrler.LockSkillIndex != -1 || this.actorCtrler.ActorType == ActorController.EActorType.EPlayer)
		{
			return;
		}
		if (!this.cacheSkillFlag)
		{
			this.CacheSkillFlag();
		}
		ECastSkillResult eCastSkillResult = ECastSkillResult.ECSR_NotCoolDown;
		if (this.delayIndex != 0)
		{
			this.delayTimer -= elapse;
			if (this.delayTimer <= 0f)
			{
				switch (this.delayCastType)
				{
				case 0:
				{
					ActorController actorController = this.FindHealTarget();
					this.actorCtrler.TryCastSkill(this.delayIndex, actorController);
					eCastSkillResult = ECastSkillResult.ECSR_Sucess;
					break;
				}
				case 1:
				{
					ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
					if (actor != null)
					{
						eCastSkillResult = this.actorCtrler.TryCastSkill(this.delayIndex, actor.transform.position);
					}
					break;
				}
				case 2:
					eCastSkillResult = this.actorCtrler.TryCastSkill(this.delayIndex, this.Target.transform.position);
					break;
				case 3:
				{
					ActorController actor2 = Globals.Instance.ActorMgr.GetActor(0);
					if (actor2 != null)
					{
						eCastSkillResult = this.actorCtrler.TryCastSkill(this.delayIndex, actor2);
					}
					break;
				}
				case 4:
					eCastSkillResult = this.actorCtrler.TryCastSkill(this.delayIndex, this.Target);
					break;
				}
			}
			if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
			{
				this.delayIndex = 0;
			}
			else
			{
				eCastSkillResult = this.actorCtrler.TryCastSkill(0, this.Target);
				if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
				{
					for (int i = 1; i < this.actorCtrler.Skills.Length; i++)
					{
						if (this.actorCtrler.Skills[i] != null && this.actorCtrler.Skills[i].IsCooldown && this.actorCtrler.Skills[i].Info.CastType == 0)
						{
							this.actorCtrler.Skills[i].Rate += this.actorCtrler.Skills[i].Info.CastRate;
						}
					}
				}
			}
			return;
		}
		if (this.healSkill)
		{
			ActorController x = this.FindHealTarget();
			if (x != null)
			{
				for (int j = 0; j < this.actorCtrler.Skills.Length; j++)
				{
					if (this.actorCtrler.Skills[j] != null && this.actorCtrler.Skills[j].IsCooldown && this.actorCtrler.Skills[j].Info.CastType == 0 && this.actorCtrler.Skills[j].Info.CastTargetType == 2)
					{
						if (this.actorCtrler.Skills[j].Rate >= 10000 || UtilFunc.RangeRandom(0, 10000) <= this.actorCtrler.Skills[j].Rate)
						{
							if (this.actorCtrler.Skills[j].Info.SayTime > 0f)
							{
								GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, this.actorCtrler.Skills[j].Info.SayID, 3f);
								this.delayTimer = this.actorCtrler.Skills[j].Info.SayTime;
								this.delayIndex = j;
								this.delayCastType = 0;
								return;
							}
							eCastSkillResult = this.actorCtrler.TryCastSkill(j, x);
							if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
							{
								GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, this.actorCtrler.Skills[j].Info.SayID, 3f);
								return;
							}
						}
					}
				}
			}
		}
		if (this.Target == null || this.Target.IsDead || this.Target.Unattacked)
		{
			return;
		}
		if (this.Target.NavAgent != null && this.Target.NavAgent.velocity.sqrMagnitude <= 0f && this.actorCtrler.NavAgent != null && this.actorCtrler.NavAgent.velocity.sqrMagnitude > 0f)
		{
			return;
		}
		float num = this.actorCtrler.GetDistance2D(this.Target);
		if (this.actorCtrler.IsMelee)
		{
			num -= 1f;
		}
		else
		{
			num -= 0.7f;
		}
		if (num > this.AttackDistance)
		{
			return;
		}
		if ((!this.Target.IsImmunity || this.Target.ActorType == ActorController.EActorType.EMonster) && !this.Target.IsBox)
		{
			for (int k = 1; k < this.actorCtrler.Skills.Length; k++)
			{
				if (this.actorCtrler.Skills[k] != null && this.actorCtrler.Skills[k].IsCooldown && this.actorCtrler.Skills[k].Info.CastType == 0)
				{
					if (this.actorCtrler.Skills[k].Rate >= 10000 || UtilFunc.RangeRandom(0, 10000) <= this.actorCtrler.Skills[k].Rate)
					{
						if ((this.actorCtrler.Skills[k].Effect & 1) == 0 || !this.actorCtrler.IsImmunity)
						{
							if (this.actorCtrler.Skills[k].Info.SayTime > 0f)
							{
								GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, this.actorCtrler.Skills[k].Info.SayID, 3f);
								this.delayTimer = this.actorCtrler.Skills[k].Info.SayTime;
								this.delayIndex = k;
							}
							if (this.actorCtrler.Skills[k].Info.CastTargetType == 3)
							{
								if (this.actorCtrler.ActorType == ActorController.EActorType.EMonster)
								{
									ActorController actor3 = Globals.Instance.ActorMgr.GetActor(0);
									if (actor3 != null)
									{
										if (this.actorCtrler.Skills[k].Info.SayTime > 0f)
										{
											this.delayCastType = 1;
											return;
										}
										eCastSkillResult = this.actorCtrler.TryCastSkill(k, actor3.transform.position);
									}
								}
								else
								{
									if (this.actorCtrler.Skills[k].Info.SayTime > 0f)
									{
										this.delayCastType = 2;
										return;
									}
									eCastSkillResult = this.actorCtrler.TryCastSkill(k, this.Target.transform.position);
								}
							}
							else if (this.actorCtrler.Skills[k].Info.CastTargetType != 2)
							{
								if (this.actorCtrler.ActorType == ActorController.EActorType.EMonster && this.actorCtrler.monsterInfo != null && this.actorCtrler.monsterInfo.SkillToPlayer)
								{
									ActorController actor4 = Globals.Instance.ActorMgr.GetActor(0);
									if (actor4 != null)
									{
										if (this.actorCtrler.Skills[k].Info.SayTime > 0f)
										{
											this.delayCastType = 3;
											return;
										}
										eCastSkillResult = this.actorCtrler.TryCastSkill(k, actor4);
									}
								}
								else
								{
									if (this.actorCtrler.Skills[k].Info.SayTime > 0f)
									{
										this.delayCastType = 4;
										return;
									}
									eCastSkillResult = this.actorCtrler.TryCastSkill(k, this.Target);
								}
							}
							if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
							{
								GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, this.actorCtrler.Skills[k].Info.SayID, 3f);
								return;
							}
						}
					}
				}
			}
		}
		eCastSkillResult = this.actorCtrler.TryCastSkill(0, this.Target);
		if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
		{
			for (int l = 1; l < this.actorCtrler.Skills.Length; l++)
			{
				if (this.actorCtrler.Skills[l] != null && this.actorCtrler.Skills[l].IsCooldown && this.actorCtrler.Skills[l].Info.CastType == 0)
				{
					this.actorCtrler.Skills[l].Rate += this.actorCtrler.Skills[l].Info.CastRate;
				}
			}
			return;
		}
	}

	protected void UpdateTarget(float elapse)
	{
		if (this.ForceFollow || this.Target == null || (this.selectTarget != null && !this.selectTarget.IsDead))
		{
			return;
		}
		if (this.Target != null && this.Target.IsDead)
		{
			this.threatTimer = 1f;
		}
		this.threatTimer += elapse;
		if (this.threatTimer > 0.2f)
		{
			this.threatTimer = 0f;
			ActorController attackTarget = this.threatMgr.GetAttackTarget();
			this.SetTarget(attackTarget);
			if (attackTarget == null)
			{
				this.findEnemyTimer = 1f;
			}
		}
	}

	protected void UpdateWarning(float elapse)
	{
		if (!this.EnableAI || this.warningFlag || this.Target == null || this.Target.IsDead || Globals.Instance.ActorMgr.IsPvpScene())
		{
			return;
		}
		this.threatTimer += elapse;
		if (this.threatTimer > 0.1f)
		{
			this.threatTimer = 0f;
			float distance2D = this.actorCtrler.GetDistance2D(this.Target);
			if (distance2D < this.AttackDistance * 2.5f && !this.Target.IsBox)
			{
				this.warningFlag = true;
				this.actorCtrler.UpdateSpeedScale(1f);
				if (distance2D > this.AttackDistance * 2f)
				{
					this.actorCtrler.PlayAction("Skill/Tan", null);
					if (this.targetMove == null)
					{
						this.targetMove = new MoveStateTarget(this.actorCtrler);
					}
					this.targetMove.Stop(1f);
					Globals.Instance.ActorMgr.OnPlayerFindEnemy(this.Target);
				}
			}
		}
	}

	public void ReleaseChaser(ActorController actor)
	{
		if (actor.IsMelee)
		{
			if (actor.ActorType == ActorController.EActorType.EPet)
			{
				if (this.meleeChaser[0] == actor)
				{
					this.meleeChaser[0] = null;
				}
				return;
			}
			for (int i = 0; i < this.meleeChaser.Length; i++)
			{
				if (this.meleeChaser[i] == actor)
				{
					this.meleeChaser[i] = null;
					break;
				}
			}
		}
		else
		{
			if (actor.ActorType == ActorController.EActorType.EPet)
			{
				if (this.rangeChaser[0] == actor)
				{
					this.rangeChaser[0] = null;
				}
				return;
			}
			for (int j = 0; j < this.rangeChaser.Length; j++)
			{
				if (this.rangeChaser[j] == actor)
				{
					this.rangeChaser[j] = null;
					break;
				}
			}
		}
	}

	private float GetPetChaseAngle(ActorController actor)
	{
		this.ReleaseChaser(actor);
		if (actor.IsMelee)
		{
			if (this.meleeChaser[0] == null)
			{
				if (actor.AiCtrler.FollowSlot != 7)
				{
					this.meleeChaser[0] = actor;
				}
				Vector3 forward = actor.transform.position - base.transform.position;
				if (forward.sqrMagnitude > 0f)
				{
					return Quaternion.LookRotation(forward).eulerAngles.y;
				}
			}
			else
			{
				int num = 0;
				if (actor.AiCtrler.FollowSlot != 7)
				{
					num = actor.AiCtrler.FollowSlot - this.meleeChaser[0].AiCtrler.FollowSlot;
				}
				Vector3 forward2 = this.meleeChaser[0].transform.position - base.transform.position;
				if (forward2.sqrMagnitude > 0f)
				{
					return Quaternion.LookRotation(forward2).eulerAngles.y - (float)num * 45f;
				}
				return (float)num * 45f;
			}
		}
		else if (this.rangeChaser[0] == null)
		{
			if (actor.AiCtrler.FollowSlot != 7)
			{
				this.rangeChaser[0] = actor;
			}
			Vector3 forward3 = actor.transform.position - base.transform.position;
			if (forward3.sqrMagnitude > 0f)
			{
				return Quaternion.LookRotation(forward3).eulerAngles.y;
			}
		}
		else
		{
			int num2 = 0;
			if (actor.AiCtrler.FollowSlot != 7)
			{
				num2 = actor.AiCtrler.FollowSlot - this.rangeChaser[0].AiCtrler.FollowSlot;
			}
			Vector3 forward4 = this.rangeChaser[0].transform.position - base.transform.position;
			if (forward4.sqrMagnitude > 0f)
			{
				return Quaternion.LookRotation(forward4).eulerAngles.y - (float)num2 * 15f;
			}
			return (float)num2 * 15f;
		}
		return 0f;
	}

	private float GetMonsterChaseAngle(ActorController actor)
	{
		float num = 0f;
		if (this.actorCtrler.Unattacked)
		{
			Vector3 forward = actor.transform.position - base.transform.position;
			if (forward.sqrMagnitude > 0f)
			{
				num = Quaternion.LookRotation(forward).eulerAngles.y - base.transform.rotation.eulerAngles.y;
				if (num < 0f)
				{
					num += 360f;
				}
			}
			return num;
		}
		this.ReleaseChaser(actor);
		if (actor.IsMelee)
		{
			Vector3 forward2 = actor.transform.position - base.transform.position;
			if (forward2.sqrMagnitude > 0f)
			{
				num = Quaternion.LookRotation(forward2).eulerAngles.y - base.transform.rotation.eulerAngles.y;
				if (num < 0f)
				{
					num += 360f;
				}
			}
			int num2 = (int)num / 30;
			num2 %= 12;
			if (this.meleeChaser[num2] == null)
			{
				this.meleeChaser[num2] = actor;
				return num;
			}
			for (int i = 1; i <= 6; i++)
			{
				int num3 = (num2 + i) % 12;
				if (this.meleeChaser[num3] == null)
				{
					this.meleeChaser[num3] = actor;
					return (float)(num3 + i) * 30f;
				}
				num3 = num2 - i;
				if (num3 < 0)
				{
					num3 += 12;
				}
				if (this.meleeChaser[num3] == null)
				{
					this.meleeChaser[num3] = actor;
					return (float)(num3 - i) * 30f;
				}
			}
		}
		else
		{
			Vector3 forward3 = actor.transform.position - base.transform.position;
			if (forward3.sqrMagnitude > 0f)
			{
				num = Quaternion.LookRotation(forward3).eulerAngles.y - base.transform.rotation.eulerAngles.y;
				if (num < 0f)
				{
					num += 360f;
				}
			}
			int num2 = (int)num / 15;
			num2 %= 24;
			if (this.rangeChaser[num2] == null)
			{
				this.rangeChaser[num2] = actor;
				return num;
			}
			for (int j = 1; j <= 12; j++)
			{
				int num3 = (num2 + j) % 24;
				if (this.rangeChaser[num3] == null)
				{
					this.rangeChaser[num3] = actor;
					return (float)(num3 + j) * 15f;
				}
				num3 = num2 - j;
				if (num3 < 0)
				{
					num3 += 24;
				}
				if (this.rangeChaser[num3] == null)
				{
					this.rangeChaser[num3] = actor;
					return (float)(num3 - j) * 15f;
				}
			}
		}
		return 0f;
	}

	public Vector3 GetChaserPos(ActorController actor, float nearDist)
	{
		float num = 0f;
		switch (actor.ActorType)
		{
		case ActorController.EActorType.EPlayer:
		{
			Vector3 forward = actor.transform.position - base.transform.position;
			if (forward.sqrMagnitude > 0f)
			{
				num = Quaternion.LookRotation(forward).eulerAngles.y;
			}
			break;
		}
		case ActorController.EActorType.EPet:
			num = this.GetPetChaseAngle(actor);
			break;
		case ActorController.EActorType.EMonster:
			num = this.GetMonsterChaseAngle(actor) + base.transform.rotation.eulerAngles.y;
			break;
		}
		Vector3 zero = Vector3.zero;
		zero.x = base.transform.position.x + nearDist * Mathf.Sin(0.0174532924f * num);
		zero.z = base.transform.position.z + nearDist * Mathf.Cos(0.0174532924f * num);
		zero.y = base.transform.position.y;
		return zero;
	}

	public void SetFellowSlot(int slot)
	{
		this.followSlot = slot;
	}

	public void ClearWarningFlag()
	{
		this.warningFlag = false;
		this.SetTarget(null);
	}

	public virtual void OnFindEnemy(ActorController enemy)
	{
		if (this.actorCtrler.ActorType != ActorController.EActorType.EPlayer)
		{
			this.actorCtrler.PlayAction("Skill/Tan", null);
		}
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null || actor == enemy)
		{
			this.threatMgr.AddThreat(enemy, 1f);
		}
		else
		{
			int num = actor.CalculateDamage(10000, 0, this.actorCtrler) * 5;
			this.threatMgr.AddThreat(enemy, (float)num);
		}
		this.Chase(enemy);
		this.StartAttack();
	}

	public virtual void OnTargetChange(ActorController enemy)
	{
		this.Chase(enemy);
		this.StartAttack();
	}

	public virtual void OnDead()
	{
		this.ClearMoveState();
	}

	public virtual void OnArrivedPoint(int pointID, bool timeout)
	{
		if (pointID < 0)
		{
			Quaternion rotation = Quaternion.Euler(0f, Globals.Instance.ActorMgr.BornRotationY, 0f);
			this.actorCtrler.RotateTo(rotation);
			return;
		}
		if (this.actorCtrler.ActorType == ActorController.EActorType.EPlayer || this.actorCtrler.ActorType == ActorController.EActorType.EPet)
		{
			Globals.Instance.ActorMgr.OnArrivedPosePoint(pointID);
		}
	}

	public virtual void OnDamageTaken()
	{
		this.takenDamage = true;
	}

	public void LeaveCombat()
	{
		this.enterCombat = false;
		this.target = null;
		this.selectTarget = null;
		this.threatMgr.Clear();
		this.actorCtrler.RemoveAllBuff();
		this.Expired();
		this.findEnemyTimer = -1f;
	}

	public void SetInitTarget(ActorController actorTarget)
	{
		this.target = actorTarget;
		this.threatMgr.AddThreat(actorTarget, 0.01f);
		this.Chase(actorTarget);
	}

	public void SetFindEnemyTimer(float timer)
	{
		this.findEnemyTimer = timer;
	}
}
