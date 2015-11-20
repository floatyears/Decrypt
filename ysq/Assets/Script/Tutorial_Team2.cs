using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_Team2 : TutorialEntity
{
	public static int PetInfoID = 10104;

	private GUITeamManageModelItem teamManageModelItem;

	private GUIPartnerFightScene partnerFightScene;

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
			base.StartCoroutine(this.Step_04());
			break;
		case 5:
			this.Step_05();
			break;
		}
		return false;
	}

	private void Step_01()
	{
		PetDataEx petByInfoID = Globals.Instance.Player.PetSystem.GetPetByInfoID(Tutorial_Team2.PetInfoID);
		if (Globals.Instance.Player.TeamSystem.HasPetInfoID(Tutorial_Team2.PetInfoID) || petByInfoID == null || petByInfoID.Data.Level > 1u)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Team2, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_TeamBtn("tutorial15");
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.PlaySound("tutorial_027");
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			this.Step_02();
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
			CreateObjIntervalFrame = 1,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial6")
		});
		UIEventListener expr_A7 = UIEventListener.Get(this.area.gameObject);
		expr_A7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A7.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
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
			Tips = Singleton<StringManager>.Instance.GetString("tutorial7")
		});
		UIEventListener expr_CA = UIEventListener.Get(this.area.gameObject);
		expr_CA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_CA.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_03MaskAreaClick(GameObject obj)
	{
		this.teamManageModelItem.OnBattleClick(null);
	}

	[DebuggerHidden]
	private IEnumerator Step_04()
	{
        return null;
        //Tutorial_Team2.<Step_04>c__Iterator2B <Step_04>c__Iterator2B = new Tutorial_Team2.<Step_04>c__Iterator2B();
        //<Step_04>c__Iterator2B.<>f__this = this;
        //return <Step_04>c__Iterator2B;
	}

	private void OnStep_04MaskAreaClick(GameObject obj)
	{
		this.targetObj.transform.parent.GetComponent<PartnerFightBagItem>().OnFightBtnClick(null);
	}

	private void Step_05()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Team2, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1034, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
		}
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num != 1)
		{
			if (num == 2)
			{
				base.PlaySound("tutorial_029");
			}
		}
		else
		{
			base.PlaySound("tutorial_028");
		}
	}
}
