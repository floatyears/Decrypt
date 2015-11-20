using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/Arcane Missiles Action")]
public sealed class ArcaneMissilesAction : ActionBase
{
	[Serializable]
	public class MissileContent
	{
		public float delayTime;

		public Vector3 offest;

		public float speed = 10f;

		public float DurationMin = 0.25f;

		public bool DoDamage;
	}

	private class MissileInstance
	{
		public int status;

		public GameObject go;

		public float timeStamp;

		public float duration = 10f;

		public long damage;

		public GameObject target;

		public Vector3 startPos = Vector3.zero;

		public Vector3 targetPos = Vector3.zero;
	}

	public GameObject MissilePrefab;

	public float MissileDeleteDelay = 0.1f;

	public bool Spine = true;

	public float YOffset;

	public float ForwardOffset;

	public float RightOffset;

	public List<ArcaneMissilesAction.MissileContent> missileOffests = new List<ArcaneMissilesAction.MissileContent>();

	public GameObject explodePrefab;

	public float explodeLifeTime = 1f;

	private List<ArcaneMissilesAction.MissileInstance> missileInsts = new List<ArcaneMissilesAction.MissileInstance>();

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		if (this.missileOffests.Count <= 0)
		{
			global::Debug.Log(new object[]
			{
				"Must editor Missile Offsets."
			});
			base.Finish();
			return;
		}
		if (this.MissilePrefab == null)
		{
			global::Debug.Log(new object[]
			{
				"MissilePrefab == null"
			});
			base.Finish();
			return;
		}
		this.missileInsts.Clear();
		for (int i = 0; i < this.missileOffests.Count; i++)
		{
			ArcaneMissilesAction.MissileInstance missileInstance = new ArcaneMissilesAction.MissileInstance();
			missileInstance.timeStamp = 0f;
			this.missileInsts.Add(missileInstance);
		}
	}

	protected override void UpdateAction(float elapse)
	{
		bool flag = true;
		for (int i = 0; i < this.missileInsts.Count; i++)
		{
			this.missileInsts[i].timeStamp += elapse;
			if (this.missileInsts[i].status == 0)
			{
				flag = false;
				if (this.missileInsts[i].timeStamp >= this.missileOffests[i].delayTime)
				{
					this.missileInsts[i].status = 1;
					if (base.variables.skillTarget != null)
					{
						if (this.Spine && base.variables.skillTarget.SpineTransform != null)
						{
							this.missileInsts[i].target = base.variables.skillTarget.SpineTransform.gameObject;
							this.missileInsts[i].startPos = this.missileInsts[i].target.transform.position;
						}
						else
						{
							this.missileInsts[i].target = base.variables.skillTarget.gameObject;
							this.missileInsts[i].startPos = this.missileInsts[i].target.transform.position;
						}
					}
					if (base.variables.skillCaster != null)
					{
						this.missileInsts[i].targetPos = base.variables.skillCaster.transform.position;
						this.missileInsts[i].targetPos += base.variables.skillCaster.transform.forward * this.ForwardOffset;
						ArcaneMissilesAction.MissileInstance expr_1DC_cp_0 = this.missileInsts[i];
						expr_1DC_cp_0.targetPos.y = expr_1DC_cp_0.targetPos.y + this.YOffset;
					}
					float num = Vector3.Distance(this.missileInsts[i].startPos, this.missileInsts[i].targetPos);
					Transform transform = PoolMgr.SpawnParticleSystem(this.MissilePrefab.transform, this.missileInsts[i].startPos, Quaternion.identity, 1f);
					if (transform == null)
					{
						global::Debug.LogError(new object[]
						{
							"Instantiate MissilePrefab object error!"
						});
					}
					this.missileInsts[i].go = transform.gameObject;
					this.missileInsts[i].duration = Mathf.Max(this.missileOffests[i].DurationMin, num / this.missileOffests[i].speed);
					this.missileInsts[i].timeStamp = 0f;
					if (this.missileOffests[i].DoDamage)
					{
						this.missileInsts[i].damage = (long)this.DoEffect(base.variables.skillInfo, base.variables.skillCaster, base.variables.skillTarget);
					}
				}
			}
			else if (this.missileInsts[i].go != null)
			{
				flag = false;
				if (this.missileInsts[i].timeStamp >= this.missileInsts[i].duration)
				{
					this.OnReachTarget(this.missileInsts[i]);
				}
				else
				{
					float num2 = this.missileInsts[i].timeStamp / this.missileInsts[i].duration;
					num2 *= num2;
					Vector3 vector = base.variables.skillCaster.transform.position;
					vector.y += this.YOffset;
					vector += base.variables.skillCaster.transform.forward * this.ForwardOffset;
					Bezier bezier = new Bezier(this.missileInsts[i].startPos, base.variables.skillCaster.transform.right * this.missileOffests[i].offest.x + Vector3.up * this.missileOffests[i].offest.y - base.variables.skillCaster.transform.forward * this.missileOffests[i].offest.z, Vector3.zero, vector);
					Vector3 pointAtTime = bezier.GetPointAtTime(Mathf.Clamp01(num2));
					this.missileInsts[i].go.transform.position = pointAtTime;
					Vector3 forward = this.missileInsts[i].startPos - pointAtTime;
					if (forward.sqrMagnitude > 0.1f)
					{
						Quaternion to = Quaternion.LookRotation(forward);
						this.missileInsts[i].go.transform.rotation = Quaternion.Slerp(this.missileInsts[i].go.transform.rotation, to, Time.deltaTime * 10f);
					}
				}
			}
		}
		if (flag)
		{
			base.Finish();
		}
	}

	private void OnReachTarget(ArcaneMissilesAction.MissileInstance inst)
	{
		if (this.explodePrefab != null)
		{
			Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, inst.go.transform.position, Quaternion.identity, 1f);
			PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
		}
		PoolMgr.spawnPool.Despawn(inst.go.transform, this.MissileDeleteDelay);
		inst.go = null;
		if (inst.damage > 0L)
		{
			int num = 0;
			SkillInfo skillInfo = base.variables.skillInfo;
			for (int i = 0; i < skillInfo.EffectType.Count; i++)
			{
				if (skillInfo.EffectType[i] == 4 || skillInfo.EffectType[i] == 5)
				{
					num += skillInfo.Value4[i];
				}
			}
			int num2 = base.variables.skillCaster.DoHeal(inst.damage * (long)num / 10000L, base.variables.skillCaster);
			if (num2 > 0 && base.variables.skillCaster.UIText != null && base.variables.skillCaster.FactionType == ActorController.EFactionType.EBlue)
			{
				GameUIManager.mInstance.HUDTextMgr.RequestShow(base.variables.skillCaster, EShowType.EST_Heal, num2, null, 0);
			}
		}
	}

	private int DoEffect(SkillInfo info, ActorController caster, ActorController target)
	{
		long num = 0L;
		for (int i = 0; i < info.EffectType.Count; i++)
		{
			if (info.EffectType[i] != 0)
			{
				if (info.Rate[i] >= 10000 || UtilFunc.RangeRandom(0, 10000) <= info.Rate[i])
				{
					switch (info.EffectType[i])
					{
					case 2:
						target.AddBuff(info.Value3[i], caster);
						goto IL_143;
					case 4:
						num += (long)caster.CalculateDamage(info.Value1[i], info.Value2[i], target);
						goto IL_143;
					case 5:
						num += (long)caster.CalculateDamage(info.Value1[i], info.Value2[i], target);
						goto IL_143;
					}
					global::Debug.LogError(new object[]
					{
						string.Concat(new object[]
						{
							"SkillID = [",
							info.ID,
							"] invalid EffectType = [",
							info.EffectType[i],
							"]"
						})
					});
				}
			}
			IL_143:;
		}
		Singleton<ActionMgr>.Instance.PlayHitAction(caster, info, target);
		int num2 = 0;
		float num3 = info.ThreatBase;
		if (num > 0L)
		{
			bool flag = caster.CanCrit(target);
			if (flag)
			{
				num = num * 150L / 100L;
			}
			num = num * (long)(10000 + caster.DamageMod) / 10000L;
			num2 = target.DoDamage(num, caster, ActorController.CanHitBack(info));
			if (target.UIText != null && num2 > 0)
			{
				if (target.FactionType == ActorController.EFactionType.ERed)
				{
					if (flag)
					{
						GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_CriticalDamage, num2, null, 0);
					}
					else
					{
						GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Damage, num2, null, 0);
					}
				}
				else
				{
					GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Damage, -num2, null, 2);
				}
			}
			num3 += (float)num2 * info.ThreatCoef;
		}
		if (num3 > 0f && caster.IsHostileTo(target))
		{
			target.AiCtrler.ThreatMgr.AddThreat(caster, num3);
		}
		return num2;
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		for (int i = 0; i < this.missileInsts.Count; i++)
		{
			if (this.missileInsts[i] != null && this.missileInsts[i].go != null)
			{
				PoolMgr.spawnPool.Despawn(this.missileInsts[i].go.transform);
			}
		}
		this.missileInsts.Clear();
	}
}
