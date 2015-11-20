using System;
using UnityEngine;

public class Tutorial_ElitePVE : TutorialEntity
{
	public static int SceneID = 105010;

	public static int SceneID1 = 106001;

	public static int SceneID2 = 201001;

	public static int QuestID = 105010;

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
		case 5:
			this.Step_05();
			break;
		case 6:
			this.Step_06();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Globals.Instance.Player.GetSceneScore(Tutorial_ElitePVE.SceneID) > 0;
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.GetSceneScore(Tutorial_ElitePVE.SceneID1) > 0 || Globals.Instance.Player.GetSceneScore(Tutorial_ElitePVE.SceneID2) > 0)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_ElitePVE, true, true, true);
			return;
		}
		TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		this.Step_02();
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Null, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			base.Step_VictoryOKBtn(null);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_PVEBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			base.Step_QuestReceiveBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			if (Globals.Instance.Player.GetQuestState(Tutorial_ElitePVE.QuestID) == 2)
			{
				TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
				this.Step_03();
			}
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			this.Step_03();
		}
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			this.worldMap = TutorialEntity.ConvertObject2UnityOrPrefab<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			this.worldMap = GameUIManager.mInstance.GetSession<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			return;
		}
		GameUIManager.mInstance.ShowPlotDialog(1105, new GUIPlotDialog.FinishCallback(this.Step_04), null);
	}

	private void Step_04()
	{
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "topPanel/1",
			TargetParent = this.worldMap.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial40")
		});
		if (this.worldMap.instruction != null && this.worldMap.instruction.activeInHierarchy)
		{
			this.worldMap.instruction.SetActive(false);
		}
		UIEventListener expr_A6 = UIEventListener.Get(this.area.gameObject);
		expr_A6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A6.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_04MaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<UIToggle>().value = true;
		this.worldMap.OnDifficultyCheck(this.targetObj.gameObject);
	}

	private void Step_05()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.Step_SceneBtn(1, "tutorial41");
		}
	}

	private void Step_06()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIAdventureReady)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_ElitePVE, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1107, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
		}
	}
}
