using System;
using UnityEngine;

[AddComponentMenu("Game/Character/AnimationController")]
public sealed class AnimationController : MonoBehaviour
{
	public const string PoseStd = "std";

	public const string PoseRun = "run";

	private PlayAnimation lastAnimation;

	private AnimationState animState;

	private AnimationState stdAnimState;

	private AnimationState runAnimState;

	private ActorController actorCtrler;

	private Animation animCtrl;

	public Animation AnimCtrl
	{
		get
		{
			return this.animCtrl;
		}
	}

	private void Start()
	{
		this.actorCtrler = base.GetComponent<ActorController>();
		this.UpdateAnimCtrl();
		if (!string.IsNullOrEmpty("std") && this.animCtrl != null)
		{
			this.stdAnimState = this.animCtrl["std"];
			this.animCtrl.CrossFade("std");
		}
		if (!string.IsNullOrEmpty("run") && this.animCtrl != null)
		{
			this.runAnimState = this.animCtrl["run"];
		}
	}

	private void Update()
	{
		if (this.lastAnimation == null || this.animState == null || this.animCtrl == null)
		{
			this.UpdateBaseAnimation();
			return;
		}
		if (!this.animState.enabled || (this.animState.wrapMode == WrapMode.ClampForever && this.animState.time > this.animState.length))
		{
			this.lastAnimation = null;
			this.animState = null;
		}
	}

	private void UpdateBaseAnimation()
	{
		if (this.actorCtrler.NavAgent != null && !this.actorCtrler.IsDead)
		{
			if (this.actorCtrler.NavAgent.desiredVelocity.sqrMagnitude < 0.1f)
			{
				this.animCtrl.CrossFade("std", 0.2f);
				this.animCtrl.wrapMode = WrapMode.Loop;
			}
			else
			{
				this.animCtrl.CrossFade("run", 0.1f);
				this.animCtrl.wrapMode = WrapMode.Loop;
			}
		}
	}

	public void PlayAnimation(PlayAnimation anim)
	{
		if (this.animCtrl == null)
		{
			return;
		}
		if (this.lastAnimation != null && this.lastAnimation.priority > anim.priority)
		{
			return;
		}
		if (string.IsNullOrEmpty(anim.AnimName))
		{
			return;
		}
		if (anim.replay && this.animCtrl.IsPlaying(anim.AnimName))
		{
			this.animCtrl.Stop();
		}
		this.animState = this.animCtrl[anim.AnimName];
		if (this.animState == null)
		{
			return;
		}
		if (this.actorCtrler != null)
		{
			this.animState.speed = this.actorCtrler.AttackSpeed;
		}
		if (anim.BlendTime < 0.001f)
		{
			this.animCtrl.Play(anim.AnimName, anim.PlayMode);
		}
		else
		{
			this.animCtrl.CrossFade(anim.AnimName, anim.BlendTime, anim.PlayMode);
		}
		this.animCtrl.wrapMode = anim.WrapMode;
		this.lastAnimation = anim;
	}

	public void StopAnimation()
	{
		this.lastAnimation = null;
	}

	public void UpdateSpeed(float value)
	{
		if (this.actorCtrler == null)
		{
			return;
		}
		if (this.stdAnimState != null)
		{
			this.stdAnimState.speed = this.actorCtrler.SpeedScale;
		}
		if (this.runAnimState != null)
		{
			this.runAnimState.speed = this.actorCtrler.SpeedScale;
		}
	}

	public void PauseAnimation(bool value)
	{
		if (this.actorCtrler == null)
		{
			return;
		}
		if (this.stdAnimState != null)
		{
			this.stdAnimState.speed = ((!value) ? 1f : 0f);
		}
		if (this.runAnimState != null)
		{
			this.runAnimState.speed = ((!value) ? 1f : 0f);
		}
		if (this.animState != null)
		{
			this.animState.speed = ((!value) ? 1f : 0f);
		}
	}

	public void UpdateAnimCtrl()
	{
		ModelController component = base.GetComponent<ModelController>();
		if (component == null)
		{
			this.animCtrl = base.animation;
		}
		else
		{
			this.animCtrl = component.GetAnimation();
		}
	}
}
