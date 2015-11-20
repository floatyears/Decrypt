using System;
using UnityEngine;

public class Tutorial_PVP : TutorialEntity
{
	private GUIPVP4ReadyScene pvp4ReadyScene;

	private int plotIndex;

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
		case 7:
			this.Step_07();
			break;
		case 8:
			this.Step_08();
			break;
		case 9:
			this.Step_09();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(6), true) && (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp || Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene);
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.Data.Honor > 0 || (ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(6)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PVP, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(1057, new GUIPlotDialog.FinishCallback(base.Step_UnlockGoBtn), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent2));
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.PlaySound("tutorial_066");
			this.plotIndex = 0;
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1057, new GUIPlotDialog.FinishCallback(base.Step_MysteryBtn), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent2));
			base.PlaySound("tutorial_066");
			this.plotIndex = 0;
		}
	}

	private void OnPlotClickEvent2()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_067");
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
			TargetName = "Panel/PVP",
			TargetParent = this.mysteryScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left
		});
		UIEventListener expr_8B = UIEventListener.Get(this.area.gameObject);
		expr_8B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8B.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_02MaskAreaClick(GameObject go)
	{
		this.mysteryScene.OnPVPClick();
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPVP4ReadyScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1059, new GUIPlotDialog.FinishCallback(this.Step_04), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
			this.plotIndex = 0;
			base.PlaySound("tutorial_068");
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene || Globals.Instance.TutorialMgr.CurrentScene is GUIShopScene || Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_PVP, false, false, false);
			base.StartNextTutorial();
		}
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_069");
		}
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPVP4ReadyScene)
		{
			this.pvp4ReadyScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPVP4ReadyScene>();
		}
		if (this.pvp4ReadyScene == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown;
		tutorialInitParams.ScaleFactor = 1.42857146f;
		tutorialInitParams.CloneScale = 0.7f;
		tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial32");
		PVPTargetGrid mTargetTable = this.pvp4ReadyScene.mTargetTable;
		if (mTargetTable == null || mTargetTable.gridItems.Length == 0)
		{
			global::Debug.LogError(new object[]
			{
				"PVP gridItems length is 0"
			});
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PVP, true, true, true);
			return;
		}
		int num = 0;
		UICustomGridItem[] gridItems = mTargetTable.gridItems;
		for (int i = 0; i < gridItems.Length; i++)
		{
			GUIPVP4TargetItem gUIPVP4TargetItem = (GUIPVP4TargetItem)gridItems[i];
			if (gUIPVP4TargetItem.data != null && gUIPVP4TargetItem.data.RankData != null && gUIPVP4TargetItem.data.RankData.Rank > num && gUIPVP4TargetItem.data.GetID() != Globals.Instance.Player.Data.ID)
			{
				num = gUIPVP4TargetItem.data.RankData.Rank;
				tutorialInitParams.TargetObj = gUIPVP4TargetItem.pk.gameObject;
			}
		}
		if (tutorialInitParams.TargetObj == null)
		{
			global::Debug.LogError(new object[]
			{
				"pvp target is null"
			});
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PVP, true, true, true);
			return;
		}
		base.InitGuideMask(tutorialInitParams);
		UIEventListener expr_1B1 = UIEventListener.Get(this.area.gameObject);
		expr_1B1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1B1.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		base.PlaySound("tutorial_070");
	}

	private void OnStep_04MaskAreaClick(GameObject go)
	{
		this.targetObj.transform.parent.parent.GetComponent<GUIPVP4TargetItem>().OnPkTraget(null);
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_PVP, false, false, false);
	}

	private void Step_05()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPVP4ReadyScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1062, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent1));
			this.plotIndex = 0;
			base.PlaySound("tutorial_071");
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.StartNextTutorial();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIShopScene)
		{
			base.StartNextTutorial();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			base.StartNextTutorial();
		}
	}

	private void OnPlotClickEvent1()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_072");
		}
	}

	private void Step_06()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPVP4ReadyScene)
		{
			this.pvp4ReadyScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPVP4ReadyScene>();
		}
		if (this.pvp4ReadyScene == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "LeftBottom/honorShop",
			TargetParent = this.pvp4ReadyScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left
		});
		UIEventListener expr_8B = UIEventListener.Get(this.area.gameObject);
		expr_8B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8B.onClick, new UIEventListener.VoidDelegate(this.OnStep_06MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(7, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_06MaskAreaClick(GameObject go)
	{
		this.pvp4ReadyScene.OnHonorShopClick(null);
	}

	private void Step_07()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIShopScene)
		{
			TutorialEntity.SetNextTutorialStep(8, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1064, new GUIPlotDialog.FinishCallback(base.Step_BackBtn), null);
		}
	}

	private void Step_08()
	{
		TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.Step_Back2Main(null);
	}

	private void Step_09()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_PVEBtn();
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Null, true, false, false);
		}
	}
}
