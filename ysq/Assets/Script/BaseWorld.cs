using Att;
using System;

public class BaseWorld : BaseScene
{
	protected ActorController bossActor;

	protected int findBossDialogID;

	public BaseWorld(ActorManager actorManager) : base(actorManager)
	{
	}

	protected void CheckBossDialog(bool flag = true)
	{
		if (this.findBossDialogID != 0)
		{
			return;
		}
		if (this.bossActor == null)
		{
			this.bossActor = this.actorMgr.GetBossActor();
		}
		if (this.bossActor == null || !this.bossActor.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.actorMgr.PlayerCtrler.ActorCtrler != null && (flag || this.actorMgr.PlayerCtrler.ActorCtrler.GetDistance2D(this.bossActor) < 4.5f))
		{
			QuestInfo info = Globals.Instance.AttDB.QuestDict.GetInfo(this.senceInfo.ID);
			if (info != null && Globals.Instance.Player.GetQuestState(this.senceInfo.ID) == 0)
			{
				this.findBossDialogID = info.SceneBossDialogID;
			}
			if (this.findBossDialogID != 0 && this.status == 2 && !GameCache.HasDialogShowed(this.findBossDialogID))
			{
				if (!flag)
				{
					this.timer = 0.001f;
				}
				else
				{
					this.timer = 1f;
				}
			}
		}
	}

	public override void OnPlayWinOver()
	{
		GameUIManager.mInstance.uiState.LastScore = Globals.Instance.Player.GetSceneScore(this.senceInfo.ID);
		Globals.Instance.Player.Data.Energy -= this.senceInfo.CostValue;
		GUIGameResultVictoryScene.CacheOldData();
		Globals.Instance.ActorMgr.SendPveResultMsg();
	}

	protected void DialogFinish()
	{
		this.actorMgr.Pause(false);
		Globals.Instance.TutorialMgr.InitializationCompleted(null, null);
	}

	protected void DialogFinish2()
	{
		this.actorMgr.Pause(false);
		Globals.Instance.TutorialMgr.InitializationCompleted(null, delegate
		{
			GameUIManager.mInstance.ShowBattleCDMsg(null);
		});
	}

	protected void WinDialogFinish()
	{
		ActorController actorController = this.actorMgr.GetBossActor();
		if (actorController != null && actorController.PlayMatinee)
		{
			actorController.DestroyActor();
		}
		this.actorMgr.Pause(false);
		this.actorMgr.PlayerCtrler.SetControlLocked(true);
		this.actorMgr.GoPosePoint();
	}
}
