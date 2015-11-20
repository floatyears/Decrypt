using Att;
using System;
using UnityEngine;

public sealed class Buff
{
	public ActorController Owner;

	public ActorController Caster;

	public BuffInfo Info;

	public int StackCount;

	public bool Deleted;

	public int Value;

	private bool permanent;

	private float duration;

	private float periodic;

	private Transform addAction;

	private float triggerCD;

	public float Duration
	{
		get
		{
			if (this.permanent)
			{
				return -1f;
			}
			return this.duration;
		}
		set
		{
			if (this.permanent)
			{
				return;
			}
			this.duration = value;
		}
	}

	public bool Periodic
	{
		get
		{
			return this.Info != null && this.Info.TickType != 0;
		}
	}

	public int SerialID
	{
		get;
		private set;
	}

	public void Init(ActorController owner, ActorController caster, BuffInfo info, int serialID)
	{
		this.Owner = owner;
		this.Caster = caster;
		this.Info = info;
		this.SerialID = serialID;
		this.StackCount = info.InitStackCount;
		this.duration = info.MaxDuration;
		if (this.duration <= 0f)
		{
			this.permanent = true;
		}
		this.periodic = info.PeriodicTime;
		if (this.Info.TickType == 2)
		{
			this.periodic = 0f;
		}
		for (int i = 0; i < this.Info.EffectType.Count; i++)
		{
			if (this.Info.EffectType[i] == 11)
			{
				this.Value += this.Info.Value3[i];
			}
		}
	}

	public void Update(float elapse)
	{
		if (this.Deleted)
		{
			return;
		}
		if (this.Info.TickType != 0)
		{
			this.periodic -= elapse;
			if (this.periodic <= 0f)
			{
				this.periodic = this.Info.PeriodicTime;
				this.ApplyEffect(true, true);
			}
		}
		if (!this.permanent)
		{
			this.duration -= elapse;
			if (this.duration <= 0f)
			{
				this.Owner.RemoveBuff(this);
				return;
			}
		}
		for (int i = 0; i < this.Info.EffectType.Count; i++)
		{
			if (this.Info.EffectType[i] == 28)
			{
				if (Time.time > this.triggerCD)
				{
					int num = (int)(this.Owner.CurHP * 10000L / this.Owner.MaxHP);
					if (num <= this.Info.Value1[i])
					{
						this.triggerCD = Time.time + (float)this.Info.Value2[i];
						if (this.Info.Value3[i] == 0)
						{
							SkillInfo info = Globals.Instance.AttDB.SkillDict.GetInfo(this.Info.Value4[i]);
							if (info == null)
							{
								global::Debug.LogErrorFormat("SkillDict.GetInfo error, id = {0}", new object[]
								{
									this.Info.Value4[i]
								});
								return;
							}
							this.Owner.OnSkillCast(info, this.Owner, this.Owner.transform.position, 0);
						}
						else
						{
							this.Owner.AddBuff(this.Info.Value4[i], this.Owner);
						}
					}
				}
			}
		}
	}

	public void OnAdd()
	{
		if (this.Owner == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.Info.AddAction))
		{
			return;
		}
		if (this.Owner.BuffAction != null)
		{
			PoolMgr.Despawn(this.Owner.BuffAction);
			this.Owner.BuffAction = null;
		}
		this.addAction = PoolMgr.Spawn(this.Info.AddAction, this.Owner.transform.position, this.Owner.transform.rotation);
		this.Owner.BuffAction = this.addAction;
		if (this.addAction != null)
		{
			SkillVariables skillVariables = new SkillVariables();
			skillVariables.skillCaster = this.Owner;
			skillVariables.skillTarget = this.Caster;
			this.addAction.SendMessage("OnInit", skillVariables, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnRemove()
	{
		if (this.addAction != null && this.addAction == this.Owner.BuffAction)
		{
			PoolMgr.Despawn(this.addAction);
			this.addAction = null;
			this.Owner.BuffAction = null;
		}
	}

	public void ApplyEffect(bool apply, bool tick)
	{
		long num = 0L;
		long num2 = 0L;
		for (int i = 0; i < this.Info.EffectType.Count; i++)
		{
			if (this.Info.EffectType[i] != 0)
			{
				switch (this.Info.EffectType[i])
				{
				case 1:
					if (!tick)
					{
						this.Owner.HandleAttMod(EAttMod.EAM_Value, this.Info.Value3[i], this.Info.Value4[i] * this.StackCount, apply);
					}
					break;
				case 2:
					if (!tick)
					{
						this.Owner.HandleAttMod(EAttMod.EAM_Pct, this.Info.Value3[i], this.Info.Value4[i] * this.StackCount, apply);
					}
					break;
				case 3:
					if (tick && this.Caster != null)
					{
						num += (long)this.Caster.CalculateDamage(this.Info.Value1[i] * this.StackCount, this.Info.Value2[i] * this.StackCount, this.Owner);
					}
					break;
				case 4:
					if (tick && this.Caster != null)
					{
						num2 += (long)this.Caster.CalculateHeal(this.Info.Value1[i] * this.StackCount, this.Info.Value2[i] * this.StackCount, this.Owner);
					}
					break;
				case 5:
					if (!tick)
					{
						this.Owner.BuffRoot(apply);
					}
					break;
				case 6:
					if (!tick)
					{
						this.Owner.BuffSilence(apply);
					}
					break;
				case 7:
				case 19:
				case 20:
					if (!tick)
					{
						this.Owner.BuffStun(apply);
					}
					break;
				case 8:
					if (!tick)
					{
						this.Owner.BuffModSpeed(this.Info.Value1[i], apply);
					}
					break;
				case 9:
					if (!tick)
					{
						this.Owner.BuffModSpeed(this.Info.Value1[i], apply);
					}
					break;
				case 10:
					if (!tick)
					{
						this.Owner.BuffImmunity(apply);
					}
					break;
				case 11:
					if (!tick)
					{
						this.Owner.BuffAbsorb(apply);
					}
					break;
				case 12:
					if (!tick)
					{
						this.Owner.BuffDamageReflect(this.Info.Value1[i], apply);
					}
					break;
				case 13:
					if (!tick)
					{
						this.Owner.BuffDamageMod(this.Info.Value1[i], apply);
					}
					break;
				case 14:
					if (!tick)
					{
						this.Owner.BuffDamageTakenMod(this.Info.Value1[i], apply);
					}
					break;
				case 15:
					if (!tick)
					{
						this.Owner.BuffHealMod(this.Info.Value1[i], apply);
					}
					break;
				case 16:
					if (!tick)
					{
						this.Owner.BuffHealTakenMod(this.Info.Value1[i], apply);
					}
					break;
				case 17:
					if (!tick)
					{
						this.Owner.BuffFear(apply);
					}
					break;
				case 18:
					if (!tick)
					{
						this.Owner.BuffAttackSpeedMod(this.Info.Value1[i], apply);
					}
					break;
				case 21:
					if (!tick)
					{
						if (this.Info.Value3[i] == 0)
						{
							for (int j = 1; j < 7; j++)
							{
								this.Owner.BuffModResist(j, this.Info.Value1[i] * this.StackCount, apply);
							}
						}
						else
						{
							this.Owner.BuffModResist(this.Info.Value3[i], this.Info.Value1[i] * this.StackCount, apply);
						}
					}
					break;
				case 22:
					if (!tick)
					{
						this.Owner.BuffResurrectRate(this.Info.Value1[i], this.Info.Value2[i], apply);
					}
					break;
				case 23:
					if (!tick)
					{
						this.Owner.BuffChangeFaction(apply);
					}
					break;
				case 24:
					if (!tick)
					{
						this.Owner.BuffSuck(this.Info.Value1[i], apply);
					}
					break;
				case 25:
					if (tick)
					{
						ActorController actorController;
						if (this.Owner.FactionType == ActorController.EFactionType.EBlue)
						{
							actorController = Globals.Instance.ActorMgr.GetActor(0);
						}
						else
						{
							actorController = Globals.Instance.ActorMgr.GetRemotePlayerActor();
						}
						if (actorController != null)
						{
							num2 += actorController.MaxHP * (long)this.Info.Value1[i] / 10000L;
						}
					}
					break;
				case 26:
					if (!tick)
					{
						this.Owner.BuffModAutoAttack(this.Info.Value3[i], apply);
					}
					break;
				case 27:
					if (!tick)
					{
						this.Owner.BuffModHitEffect(this.Info.Value3[i], apply);
					}
					break;
				case 28:
					break;
				case 29:
					if (!tick)
					{
						this.Owner.BuffSuperShield(this.Info.Value1[i], apply);
					}
					break;
				case 30:
					if (!tick)
					{
						this.Owner.BuffChangeModel(this.Info, i, apply);
					}
					break;
				case 31:
					if (!tick)
					{
						this.Owner.BuffShareDamage(this.Info.Value1[i], apply);
					}
					break;
				default:
					global::Debug.LogError(new object[]
					{
						string.Concat(new object[]
						{
							"BuffID = [",
							this.Info.ID,
							"] invalid EffectType = [",
							this.Info.EffectType[i],
							"]"
						})
					});
					break;
				}
				if (num > 0L)
				{
					int num3 = this.Owner.DoDamage(num, this.Caster, false);
					if (this.Owner.UIText != null && num3 > 0 && this.Caster != null)
					{
						if (this.Owner.FactionType == ActorController.EFactionType.ERed)
						{
							if (this.Caster.ActorType == ActorController.EActorType.EPlayer)
							{
								this.Owner.UIText.AddSkillDamage(num3);
							}
							else
							{
								this.Owner.UIText.AddDamage(num3, 0);
							}
						}
						else
						{
							this.Owner.UIText.AddDamage(-num3, 2);
						}
					}
				}
				if (num2 > 0L)
				{
					if (this.Caster != null)
					{
						num2 = num2 * (long)(10000 + this.Caster.HealMod) / 10000L;
					}
					int value = this.Owner.DoHeal(num2, this.Caster);
					if (this.Owner.UIText != null && this.Owner.FactionType == ActorController.EFactionType.EBlue)
					{
						this.Owner.UIText.AddHeal(value);
					}
				}
			}
		}
	}

	public void OnDamage(int damage, out int mod)
	{
		mod = 0;
		if ((float)damage <= 0f)
		{
			return;
		}
		for (int i = 0; i < this.Info.EffectType.Count; i++)
		{
			if (this.Info.EffectType[i] == 11)
			{
				if (this.Value > damage)
				{
					mod = damage;
					this.Value -= damage;
				}
				else
				{
					mod = this.Value;
					this.Owner.RemoveBuff(this);
				}
				return;
			}
		}
	}
}
