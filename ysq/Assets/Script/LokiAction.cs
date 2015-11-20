using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/LokiAction")]
public sealed class LokiAction : ActionBase
{
	private enum AnimStep
	{
		StepStartAnim,
		StepEndAnim,
		StepDelayDamage,
		StepDelayLock,
		StepMAX
	}

	public GameObject explodeStartPrefab;

	public float explodeStartLifeTime = 1f;

	public float startAnimTime;

	public PlayAnimation startAnim = new PlayAnimation();

	public float endAnimTime = 1f;

	public PlayAnimation endAnim = new PlayAnimation();

	public float delayDamage = 0.1f;

	public float delayUnlock = 0.1f;

	public float overlayBound;

	public GameObject explodePrefab;

	public float explodeLifeTime = 1f;

	private float startTimer;

	private LokiAction.AnimStep step;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.startTimer = this.startAnimTime;
		this.step = LokiAction.AnimStep.StepStartAnim;
		base.variables.skillCaster.OnLockingStart(true, true);
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted())
		{
			base.Finish();
			return;
		}
		this.startTimer -= elapse;
		if (this.startTimer < 0f)
		{
			if (this.step == LokiAction.AnimStep.StepStartAnim)
			{
				if (this.explodeStartPrefab != null)
				{
					Transform instance = PoolMgr.SpawnParticleSystem(this.explodeStartPrefab.transform, base.variables.skillCaster.transform.position, base.variables.skillCaster.transform.rotation, 1f);
					PoolMgr.spawnPool.Despawn(instance, this.explodeStartLifeTime);
				}
				PlayAnimationAction.PlayAnimation(base.variables, this.startAnim);
				this.startTimer += this.endAnimTime;
				this.step = LokiAction.AnimStep.StepEndAnim;
			}
			else if (this.step == LokiAction.AnimStep.StepEndAnim)
			{
				if (base.variables.skillTarget != null)
				{
					base.variables.skillCaster.transform.LookAt(base.variables.skillTarget.transform.position);
					base.variables.skillCaster.transform.position = base.variables.skillTarget.transform.position + Vector3.Normalize(base.variables.skillCaster.transform.position - base.variables.skillTarget.transform.position) * (base.variables.skillTarget.GetBoundsRadius() + base.variables.skillCaster.GetBoundsRadius() + this.overlayBound);
				}
				PlayAnimationAction.PlayAnimation(base.variables, this.endAnim);
				this.startTimer += this.delayDamage;
				this.step = LokiAction.AnimStep.StepDelayDamage;
			}
			else if (this.step == LokiAction.AnimStep.StepDelayDamage)
			{
				if (this.explodePrefab != null)
				{
					Transform instance2 = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, base.variables.skillCaster.transform.position, base.variables.skillCaster.transform.rotation, 1f);
					PoolMgr.spawnPool.Despawn(instance2, this.explodeLifeTime);
				}
				this.CastSkill();
				this.startTimer += this.delayUnlock;
				this.step = LokiAction.AnimStep.StepDelayLock;
			}
			else if (this.step == LokiAction.AnimStep.StepDelayLock)
			{
				base.variables.skillCaster.OnLockingStop(base.variables.skillInfo);
				this.step = LokiAction.AnimStep.StepMAX;
			}
			else
			{
				this.startTimer = 3.40282347E+38f;
				base.Finish();
			}
		}
	}

	private void CastSkill()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
			{
				if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
				{
					base.variables.skillCaster.DoEffectOnTarget(base.variables.skillInfo, actorController, 0);
				}
			}
		}
	}
}
