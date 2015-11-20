using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_PetFurther : TutorialEntity
{
	public static int SceneID = 102001;

	private GUITeamManageModelItem teamManageModelItem;

	private GUIPetTrainSceneV2 petTrainSceneV2;

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
		case 4:
			this.Step_04();
			break;
		case 5:
			this.Step_05();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Globals.Instance.TutorialMgr.IsPassTutorial(TutorialManager.ETutorialNum.Tutorial_MapReward);
	}

	private void Step_01()
	{
		PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(0);
		if (pet.Data.Further > 0u)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetFurther, true, true, true);
			return;
		}
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (current.Data.Further > 0u)
			{
				TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetFurther, true, true, true);
				return;
			}
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_TeamBtn("tutorial49");
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.PlaySound("tutorial_042");
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(Tutorial_PetFurther.SceneID);
			if (info != null)
			{
				GameUIManager.mInstance.uiState.ResultSceneInfo2 = info;
			}
			base.StartCoroutine(this.Step_02());
		}
	}

	[DebuggerHidden]
	private IEnumerator Step_02()
	{
        return null;
        //Tutorial_PetFurther.<Step_02>c__Iterator24 <Step_02>c__Iterator = new Tutorial_PetFurther.<Step_02>c__Iterator24();
        //<Step_02>c__Iterator.<>f__this = this;
        //return <Step_02>c__Iterator;
	}

	private void OnStep_02MaskAreaClick(GameObject obj)
	{
		this.teamManageModelItem.OnGameModelClick(null);
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPetTrainSceneV2)
		{
			this.petTrainSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPetTrainSceneV2>();
		}
		if (this.petTrainSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.petTrainSceneV2.gameObject,
			TargetObj = this.petTrainSceneV2.Tab10s[1],
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial23")
		});
		UIEventListener expr_93 = UIEventListener.Get(this.area.gameObject);
		expr_93.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_93.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_044");
	}

	private void OnStep_03MaskAreaClick(GameObject obj)
	{
		this.petTrainSceneV2.OnTab10Click(this.targetObj);
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPetTrainSceneV2)
		{
			this.petTrainSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPetTrainSceneV2>();
		}
		if (this.petTrainSceneV2 == null)
		{
			return;
		}
		TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(Globals.Instance.Player.Data.FurtherLevel + 1);
		if (info == null || Globals.Instance.Player.Data.Money < (int)info.PFurtherCost)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetFurther, true, true, true);
			return;
		}
		this.petTrainSceneV2.PetTrainJinjieInfo.IsForceJinjie = true;
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "UIMiddle/winBg/rightInfo/jinJieInfo/jinJieBtn",
			TargetParent = this.petTrainSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			CreateObjIntervalFrame = 1,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial51")
		});
		UIEventListener expr_114 = UIEventListener.Get(this.area.gameObject);
		expr_114.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_114.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_045");
	}

	private void OnStep_04MaskAreaClick(GameObject obj)
	{
		this.petTrainSceneV2.PetTrainJinjieInfo.OnJinjieBtnClick(null);
		this.petTrainSceneV2.PetTrainJinjieInfo.IsForceJinjie = false;
	}

	private void Step_05()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPetFurtherSucV2)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetFurther, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1046, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
		}
	}
}
