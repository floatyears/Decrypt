using System;

public sealed class KingRewardScene1 : WorldScene
{
	public KingRewardScene1(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void OnPreStart()
	{
		base.OnPreStart();
		if (this.senceInfo.SubType == 1)
		{
			ActorController bossActor = this.actorMgr.GetBossActor();
			if (bossActor != null)
			{
				Globals.Instance.CameraMgr.Target = bossActor.gameObject;
				Globals.Instance.CameraMgr.resultCamTest = true;
			}
		}
	}

	public override void OnStart()
	{
		this.status = 2;
		GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.START, 0);
		this.actorMgr.Actors[0].AiCtrler.EnableAI = GameCache.Data.EnableAI;
		if (this.actorMgr.Actors[0].AiCtrler.EnableAI)
		{
			this.actorMgr.Actors[0].UpdateSpeedScale(1.5f);
		}
	}

	protected override void ShowDeadUI()
	{
		base.SendFailLog(Globals.Instance.SenceMgr.sceneInfo);
		Globals.Instance.SenceMgr.CloseScene();
		GUIKingRewardResultScene.ShowKingRewardResult(null);
	}
}
