using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_AwakeRoad : TutorialEntity
{
	private int AwakeSceneID = 601001;

	public override bool SelectStep()
	{
		switch (Globals.Instance.TutorialMgr.CurrentTutorialStep)
		{
		case 1:
			this.Step_01();
			break;
		case 2:
			base.StartCoroutine(this.Step_02());
			break;
		case 3:
			this.Step_03();
			break;
		case 5:
			this.Step_05();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(24), true) && (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp || Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene);
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.GetSceneScore(this.AwakeSceneID) > 0 || (ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(24)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_AwakeRoad, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(1110, new GUIPlotDialog.FinishCallback(base.Step_UnlockGoBtn), null);
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1110, new GUIPlotDialog.FinishCallback(base.Step_MysteryBtn), null);
		}
	}

	[DebuggerHidden]
	private IEnumerator Step_02()
	{
        return null;
        //Tutorial_AwakeRoad.<Step_02>c__Iterator1D <Step_02>c__Iterator1D = new Tutorial_AwakeRoad.<Step_02>c__Iterator1D();
        //<Step_02>c__Iterator1D.<>f__this = this;
        //return <Step_02>c__Iterator1D;
	}

	private void OnStep_02MaskAreaClick(GameObject go)
	{
		this.mysteryScene.OnAwakeRoadClick();
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIAwakeRoadSceneV2)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_AwakeRoad, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1112, new GUIPlotDialog.FinishCallback(this.Step_04), null);
		}
	}

	private void Step_04()
	{
		GUIAwakeRoadSceneV2 gUIAwakeRoadSceneV = null;
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIAwakeRoadSceneV2)
		{
			gUIAwakeRoadSceneV = TutorialEntity.ConvertObject2UnityOrPrefab<GUIAwakeRoadSceneV2>();
		}
		if (gUIAwakeRoadSceneV == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = gUIAwakeRoadSceneV.gameObject,
			TargetObj = gUIAwakeRoadSceneV.mSceneNodes[0].gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			FreeTutorial = true,
			PanelRenderQueue = 3210
		});
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void Step_05()
	{
		UnityEngine.Object.Destroy(this.guideAnimation);
		base.StartNextTutorial();
	}
}
