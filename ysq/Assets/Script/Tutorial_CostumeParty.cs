using System;
using UnityEngine;

public class Tutorial_CostumeParty : TutorialEntity
{
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
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(10), true) && (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp || Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene);
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.CostumePartySystem.IsInParty() || (ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(10)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_CostumeParty, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(1090, new GUIPlotDialog.FinishCallback(base.Step_UnlockGoBtn), null);
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1090, new GUIPlotDialog.FinishCallback(base.Step_MysteryBtn), null);
		}
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMysteryScene)
		{
			this.mysteryScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIMysteryScene>();
		}
		if (this.mysteryScene == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "Panel/CostumeParty",
			TargetParent = this.mysteryScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right
		});
		UIEventListener expr_8B = UIEventListener.Get(this.area.gameObject);
		expr_8B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8B.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_02MaskAreaClick(GameObject go)
	{
		this.mysteryScene.OnCostumepartyClick();
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUICostumePartyScene)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_CostumeParty, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1092, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
		}
	}
}
