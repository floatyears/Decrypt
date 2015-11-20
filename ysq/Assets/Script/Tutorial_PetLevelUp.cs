using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_PetLevelUp : TutorialEntity
{
	private GUITeamManageModelItem teamManageModelItem;

	private GUIPetTrainSceneV2 petTrainSceneV2;

	private PetDataEx petData;

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
			base.StartCoroutine(this.Step_06());
			break;
		case 7:
			this.Step_07();
			break;
		}
		return false;
	}

	private void Step_01()
	{
		this.petData = Globals.Instance.Player.PetSystem.GetPetByInfoID(Tutorial_Team2.PetInfoID);
		if (this.petData == null || this.petData.Data.Level > 1u)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetLevelUp, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_TeamBtn();
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.Step_03();
		}
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			this.teamManageSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUITeamManageSceneV2>();
		}
		if (this.teamManageSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "UIMiddle/leftInfo/Content/pet3",
			TargetParent = this.teamManageSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			CreateObjIntervalFrame = 1
		});
		UIEventListener expr_92 = UIEventListener.Get(this.area.gameObject);
		expr_92.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_92.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_02MaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<GUITeamManageSelectItem>().OnItemClick(null);
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		GUICenterModelItem expr_30 = this.teamManageSceneV2.mModelCenterChild;
		expr_30.onFinished = (SpringPanel.OnFinished)Delegate.Combine(expr_30.onFinished, new SpringPanel.OnFinished(this.Step_03));
	}

	private void Step_03()
	{
		if (this.teamManageSceneV2 == null)
		{
			this.teamManageSceneV2 = GameUIManager.mInstance.GetSession<GUITeamManageSceneV2>();
		}
		if (this.teamManageSceneV2 == null)
		{
			return;
		}
		this.teamManageModelItem = this.teamManageSceneV2.mModelCenterChild.centeredObject.GetComponent<GUITeamManageModelItem>();
		if (this.teamManageModelItem == null)
		{
			return;
		}
		GameUIManager.mInstance.HideFadeBG(false);
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.teamManageModelItem.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			HideTargetObj = true,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial16")
		});
		UIEventListener expr_D1 = UIEventListener.Get(this.area.gameObject);
		expr_D1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D1.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_030");
	}

	private void OnStep_03MaskAreaClick(GameObject obj)
	{
		this.teamManageModelItem.OnGameModelClick(null);
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
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.petTrainSceneV2.gameObject,
			TargetObj = this.petTrainSceneV2.Tab0s[1],
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial17")
		});
		UIEventListener expr_93 = UIEventListener.Get(this.area.gameObject);
		expr_93.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_93.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_031");
	}

	private void OnStep_04MaskAreaClick(GameObject obj)
	{
		this.petTrainSceneV2.OnTab0Click(this.targetObj);
	}

	private void Step_05()
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
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "UIMiddle/winBg/rightInfo/lvlInfo/yaoshui/lvlUp5Btn",
			TargetParent = this.petTrainSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			CreateObjIntervalFrame = 1,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial18")
		});
		UIEventListener expr_A7 = UIEventListener.Get(this.area.gameObject);
		expr_A7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A7.onClick, new UIEventListener.VoidDelegate(this.OnStep_05MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_032");
	}

	private void OnStep_05MaskAreaClick(GameObject obj)
	{
		base.ShowFadeBG();
		this.petTrainSceneV2.PetTrainLvlUpInfo.mGUIPetTrainYaoShuiLayer.OnLvlUp5BtnClick(null);
	}

	[DebuggerHidden]
	private IEnumerator Step_06()
	{
        return null;
        //Tutorial_PetLevelUp.<Step_06>c__Iterator25 <Step_06>c__Iterator = new Tutorial_PetLevelUp.<Step_06>c__Iterator25();
        //<Step_06>c__Iterator.<>f__this = this;
        //return <Step_06>c__Iterator;
	}

	private void Step_07()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			UnityEngine.Object.Destroy(this.guideAnimation);
			this.intervalFrame = 1;
			base.Step_BackBtnFree();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			UnityEngine.Object.Destroy(this.guideAnimation);
			base.Step_PVEBtnFree();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			UnityEngine.Object.Destroy(this.guideAnimation);
			base.Step_SceneBtnFree(4, "tutorial47");
			base.PlaySound("tutorial_034");
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Null, true, false, false);
		}
	}
}
