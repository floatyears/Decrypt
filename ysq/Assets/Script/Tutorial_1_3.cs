using System;

public class Tutorial_1_3 : TutorialEntity
{
	public static int SceneID = 101003;

	public static int QuestID = 101003;

	public override bool SelectStep()
	{
		switch (Globals.Instance.TutorialMgr.CurrentTutorialStep)
		{
		case 1:
			this.Step_01();
			break;
		case 2:
			this.Step_02();
			break;
		case 3:
			this.Step_03();
			break;
		case 4:
			this.Step_04();
			break;
		}
		return false;
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.GetQuestState(Tutorial_1_3.QuestID) == 2)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_1_3, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			if (Globals.Instance.Player.GetSceneScore(Tutorial_1_3.SceneID) <= 0)
			{
				base.Step_PVEBtn();
				TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			}
			else
			{
				base.Step_PVEBtn();
				TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			}
		}
		else
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.Step_02();
		}
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene != null)
		{
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultFailureScene)
			{
				base.ResetFadeBGArea();
				base.Step_FailureOKBtn();
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
			{
				base.Step_PVEBtn();
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
			{
				base.ResetFadeBGArea();
				if (Globals.Instance.Player.GetSceneScore(Tutorial_1_2.SceneID) <= 0)
				{
					base.Step_SceneBtn(2, null);
				}
				else
				{
					base.Step_SceneBtn(3, "tutorial12");
				}
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GameUIAdventureReady)
			{
				base.Step_StartSceneBtn();
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
			{
				base.Step_VictoryOKBtn(null);
				TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
				return;
			}
		}
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.Step_QuestReceiveBtn("tutorial45");
			base.PlaySound("tutorial_026");
		}
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			this.waitTime = 2f;
			base.Step_WorldTeam("tutorial46");
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_1_3, true, false, false);
			base.PlaySound("tutorial_027");
		}
	}
}
