using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_Constellation : TutorialEntity
{
	private GUIConstellationScene constellationScene;

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
			base.StartCoroutine(this.Step_04());
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(7), true) && (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp || Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene);
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.Data.ConstellationLevel > 0 || (ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(7)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Constellation, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(1078, new GUIPlotDialog.FinishCallback(base.Step_UnlockGoBtn), null);
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1078, new GUIPlotDialog.FinishCallback(this.Step_02), null);
		}
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			this.mainMenuScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIMainMenuScene>();
		}
		if (this.mainMenuScene == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mainMenuScene.transform.Find("UI_Edge").gameObject,
			TargetName = "UI_Edge/xingZuoBtn",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			HideTargetObj = true
		});
		UIEventListener expr_9D = UIEventListener.Get(this.area.gameObject);
		expr_9D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9D.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_02MaskAreaClick(GameObject go)
	{
		this.mainMenuScene.OnXingZuoBtnClick(null);
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIConstellationScene)
		{
			this.constellationScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIConstellationScene>();
		}
		if (this.constellationScene == null)
		{
			return;
		}
		if (Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) == 0)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Constellation, true, true, true);
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = GUIRightInfo.mLightBtn.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorialConstellation0"),
			CreateObjIntervalFrame = 1
		});
		UIEventListener expr_C7 = UIEventListener.Get(this.area.gameObject);
		expr_C7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C7.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_03MaskAreaClick(GameObject go)
	{
		this.constellationScene.mGUIRightInfo.OnLightBtnClicked(null);
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Constellation, false, false, false);
	}

	[DebuggerHidden]
	private IEnumerator Step_04()
	{
        return null;
        //Tutorial_Constellation.<Step_04>c__Iterator1E <Step_04>c__Iterator1E = new Tutorial_Constellation.<Step_04>c__Iterator1E();
        //<Step_04>c__Iterator1E.<>f__this = this;
        //return <Step_04>c__Iterator1E;
	}
}
