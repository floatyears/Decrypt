using System;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
	public float delayTime;

	protected bool interrupt;

	private bool doAction;

	private float updateTimer;

	protected SkillVariables variables
	{
		get;
		private set;
	}

	protected int actionIndex
	{
		get;
		private set;
	}

	protected bool pause
	{
		get;
		private set;
	}

	private void OnInit(object value)
	{
		if (!base.enabled)
		{
			return;
		}
		this.variables = (value as SkillVariables);
		if (this.variables == null)
		{
			global::Debug.LogError(new object[]
			{
				"SkillVariables could not be null"
			});
			return;
		}
		this.doAction = false;
		this.pause = false;
		this.actionIndex = this.variables.GenerateActionIndex();
		if (this.delayTime > 0f)
		{
			this.updateTimer = this.delayTime / this.variables.skillCaster.AttackSpeed;
		}
		else
		{
			this.updateTimer = 0f;
			this.DoBaseAction();
		}
	}

	protected void Finish()
	{
		this.variables.ActionDone(this.actionIndex);
		this.actionIndex = 0;
	}

	private void Update()
	{
		if (this.actionIndex == 0 || this.pause)
		{
			return;
		}
		if (!this.doAction)
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer <= 0f)
			{
				this.DoBaseAction();
			}
		}
		else
		{
			this.UpdateAction(Time.deltaTime);
		}
	}

	private void DoBaseAction()
	{
		if (this.variables == null || this.variables.IsInterrupted())
		{
			this.Finish();
			return;
		}
		if (this.interrupt)
		{
			this.variables.skillCaster.OnSkillStart(this.variables.skillInfo);
			this.variables.CheckInterrupt();
		}
		this.doAction = true;
		this.DoAction();
	}

	protected virtual void OnDespawned()
	{
		this.actionIndex = 0;
		this.pause = false;
	}

	protected virtual void OnPlay()
	{
		this.pause = false;
	}

	protected virtual void OnPause()
	{
		this.pause = true;
	}

	protected abstract void DoAction();

	protected virtual void UpdateAction(float elapse)
	{
	}
}
