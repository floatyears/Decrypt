using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_PetSkillLU : TutorialEntity
{
	public static int PetFurtherLevel = 3;

	private static PetDataEx tempPetData;

	private GUIPartnerManageScene partnertManageScene;

	private GUIPetTrainSceneV2 petTrainSceneV2;

	private PartnerManageInventoryItem tempBagItem;

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
		case 5:
			base.StartCoroutine(this.Step_05());
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
			this.Step_09(false);
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (current.Data.ID != 100uL && current.Data.InfoID != 90000)
			{
				if ((ulong)current.Data.Further >= (ulong)((long)Tutorial_PetSkillLU.PetFurtherLevel))
				{
					Tutorial_PetSkillLU.tempPetData = current;
					return true;
				}
			}
		}
		return false;
	}

	private void Step_01()
	{
		if (Tutorial_PetSkillLU.tempPetData == null)
		{
			global::Debug.LogError(new object[]
			{
				"petData is null"
			});
			return;
		}
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (current.Data.ID != 100uL && current.Data.InfoID != 90000)
			{
				int num = 0;
				while (num < 4 && num < current.Info.SkillID.Count)
				{
					if (current.GetSkillLevel(num) > 1)
					{
						TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetSkillLU, true, true, true);
						return;
					}
					num++;
				}
			}
		}
		TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		this.Step_02();
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1108, new GUIPlotDialog.FinishCallback(this.Step_03), null);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIPetFurtherSucV2)
		{
			GameUIManager.mInstance.ShowPlotDialog(1108, new GUIPlotDialog.FinishCallback(this.Step_07), null);
		}
	}

	private void Step_03()
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
			TargetName = "UI_Edge/petsBag",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown
		});
		UIEventListener expr_96 = UIEventListener.Get(this.area.gameObject);
		expr_96.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_96.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_03MaskAreaClick(GameObject go)
	{
		if (!Tutorial_PetSkillLU.tempPetData.IsBattling())
		{
			GameUIManager.mInstance.uiState.SelectPetID = Tutorial_PetSkillLU.tempPetData.GetID();
		}
		this.mainMenuScene.OnPetsBagClick(null);
	}

	[DebuggerHidden]
	private IEnumerator Step_05()
	{
        return null;
        //Tutorial_PetSkillLU.<Step_05>c__Iterator26 <Step_05>c__Iterator = new Tutorial_PetSkillLU.<Step_05>c__Iterator26();
        //<Step_05>c__Iterator.<>f__this = this;
        //return <Step_05>c__Iterator;
	}

	private void OnStep_05MaskAreaClick(GameObject obj)
	{
		this.tempBagItem.OnChangeBtnClicked(null);
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		base.Invoke("Step_06", 0.3f);
	}

	private void Step_06()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.tempBagItem.mSkillBtn,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left
		});
		UIEventListener expr_55 = UIEventListener.Get(this.area.gameObject);
		expr_55.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_55.onClick, new UIEventListener.VoidDelegate(this.OnStep_06MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(8, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_06MaskAreaClick(GameObject obj)
	{
		this.tempBagItem.OnSkillBtnClicked(null);
	}

	private void Step_07()
	{
		if (!(Globals.Instance.TutorialMgr.CurrentScene is GUIPetFurtherSucV2))
		{
			return;
		}
		this.petTrainSceneV2 = GameUIManager.mInstance.GetSession<GUIPetTrainSceneV2>();
		if (this.petTrainSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.petTrainSceneV2.Tab0s[3],
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial39")
		});
		UIEventListener expr_9D = UIEventListener.Get(this.area.gameObject);
		expr_9D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9D.onClick, new UIEventListener.VoidDelegate(this.OnStep_07MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(8, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_07MaskAreaClick(GameObject obj)
	{
		this.petTrainSceneV2.OnTab0Click(this.targetObj);
	}

	private void Step_08()
	{
		this.petTrainSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPetTrainSceneV2>();
		if (this.petTrainSceneV2 == null)
		{
			return;
		}
		if (!this.petTrainSceneV2.PetTrainSkillInfo.mShengjiBtn.isEnabled)
		{
			this.Step_09(true);
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.petTrainSceneV2.PetTrainSkillInfo.mShengjiBtn.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			CreateObjIntervalFrame = 1
		});
		UIEventListener expr_9A = UIEventListener.Get(this.area.gameObject);
		expr_9A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9A.onClick, new UIEventListener.VoidDelegate(this.OnStep_08MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_08MaskAreaClick(GameObject obj)
	{
		base.ShowFadeBG();
		this.petTrainSceneV2.PetTrainSkillInfo.OnShengjiBtnClick(null);
	}

	private void Step_09(bool showPlot = false)
	{
		if (showPlot || Globals.Instance.TutorialMgr.CurrentScene is GUIPetTrainSkillInfo)
		{
			base.HideFadeBG();
			GameUIManager.mInstance.ShowPlotDialog(1109, null, null);
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_PetSkillLU, true, true, true);
			Tutorial_PetSkillLU.tempPetData = null;
		}
	}
}
