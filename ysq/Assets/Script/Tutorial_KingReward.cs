using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_KingReward : TutorialEntity
{
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
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(2), true) && (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp || Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene);
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.Data.KingMedal > 0 || (ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(2)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_KingReward, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(1094, new GUIPlotDialog.FinishCallback(base.Step_UnlockGoBtn), null);
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1094, new GUIPlotDialog.FinishCallback(base.Step_MysteryBtn), null);
		}
	}

	[DebuggerHidden]
	private IEnumerator Step_02()
	{
        return null;
        //Tutorial_KingReward.<Step_02>c__Iterator22 <Step_02>c__Iterator = new Tutorial_KingReward.<Step_02>c__Iterator22();
        //<Step_02>c__Iterator.<>f__this = this;
        //return <Step_02>c__Iterator;
	}

	private void OnStep_02MaskAreaClick(GameObject go)
	{
		this.mysteryScene.OnKingRewardClick();
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIKingRewardScene)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_KingReward, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1095, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
		}
	}
}
