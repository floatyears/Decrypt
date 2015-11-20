using System;

public abstract class GoalBase
{
	protected ActorController actorCtrler;

	public GoalBase(ActorController actor)
	{
		this.actorCtrler = actor;
	}

	public virtual bool Update(float elapse)
	{
		return true;
	}

	public virtual void OnInterrupt()
	{
	}
}
