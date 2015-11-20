using Att;
using System;

public sealed class KingRewardScene2 : DefenseScene
{
	public KingRewardScene2(ActorManager actorManager) : base(actorManager)
	{
	}

	public override int GetStartID()
	{
		KRQuestInfo kRQuest = GameUIManager.mInstance.uiState.KRQuest;
		if (kRQuest == null)
		{
			Debug.LogError(new object[]
			{
				"KRQuestInfo == null!"
			});
			return 0;
		}
		return kRQuest.StartID;
	}

	protected override void ShowDeadUI()
	{
		base.SendFailLog(Globals.Instance.SenceMgr.sceneInfo);
		Globals.Instance.SenceMgr.CloseScene();
		GUIKingRewardResultScene.ShowKingRewardResult(null);
	}
}
