using System;

public abstract class MoveState
{
	protected ActorController actorCtrler;

	public MoveState(ActorController actor)
	{
		this.actorCtrler = actor;
	}

	public virtual void Enter()
	{
	}

	public virtual bool Update(float elapse)
	{
		return true;
	}

	public virtual void Exit()
	{
	}
}
