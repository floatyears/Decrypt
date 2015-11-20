using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/FanShapedBlowAction")]
public sealed class FanShapedBlowAction : ActionBase
{
	public float Angle = 120f;

	public float Duration = 3f;

	public float TickInterval = 0.5f;

	public bool LiftToRight = true;

	public float DamageAngle = 10f;

	public float DamageRadius = 10f;

	public string SocketName = string.Empty;

	private float rotateTimer;

	private float timer;

	private float curAngle;

	private GameObject socket;

	protected override void DoAction()
	{
		this.rotateTimer = 0f;
		this.timer = 0f;
		if (!string.IsNullOrEmpty(this.SocketName))
		{
			this.socket = ObjectUtil.FindChildObject(base.variables.skillCaster.gameObject, this.SocketName);
		}
		if (this.socket != null)
		{
			this.curAngle = this.socket.transform.rotation.eulerAngles.y - 270f;
		}
		else
		{
			this.curAngle = base.variables.skillCaster.transform.rotation.eulerAngles.y;
			if (this.LiftToRight)
			{
				this.curAngle -= this.Angle / 2f;
			}
			else
			{
				this.curAngle += this.Angle / 2f;
			}
		}
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.skillCaster == null || base.variables.skillCaster.IsDead)
		{
			base.Finish();
			return;
		}
		this.rotateTimer += Time.deltaTime;
		if (this.rotateTimer >= this.Duration)
		{
			base.Finish();
			return;
		}
		if (this.socket != null)
		{
			this.curAngle = this.socket.transform.rotation.eulerAngles.y - 270f;
		}
		else if (this.LiftToRight)
		{
			this.curAngle += Time.deltaTime * this.Angle / this.Duration;
		}
		else
		{
			this.curAngle -= Time.deltaTime * this.Angle / this.Duration;
		}
		this.timer -= Time.deltaTime;
		if (this.timer > 0f)
		{
			return;
		}
		this.timer = this.TickInterval;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
			{
				if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (this.DamageRadius + actorController.GetBoundsRadius()) * (this.DamageRadius + actorController.GetBoundsRadius()))
				{
					float num = Mathf.Abs(Quaternion.LookRotation(actorController.transform.position - base.variables.skillCaster.transform.position).eulerAngles.y - this.curAngle);
					if (num <= this.DamageAngle / 2f)
					{
						base.variables.skillCaster.DoEffectOnTarget(base.variables.skillInfo, actorController, 0);
					}
				}
			}
		}
	}
}
