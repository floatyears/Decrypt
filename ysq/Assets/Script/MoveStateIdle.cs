using System;

public sealed class MoveStateIdle : MoveState
{
	public MoveStateIdle(ActorController actor) : base(actor)
	{
	}

	public override void Enter()
	{
		this.actorCtrler.StopMove();
	}

	public override bool Update(float elapse)
	{
		return true;
	}

	public override void Exit()
	{
	}
}
