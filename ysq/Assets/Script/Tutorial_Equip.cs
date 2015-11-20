using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_Equip : TutorialEntity
{
	public static int EquipInfoID = 5031;

	public static int SceneID = 102002;

	public static int QuestID = 102002;

	private GUISelectEquipBagScene selectEquipBagScene;

	private GUIEquipInfoPopUp equipInfoPopUp;

	private GUIEquipUpgradeScene equipUpgradeScene;

	private ItemDataEx equipData;

	private int plotIndex;

	private GUISelectEquipBagItem tempBagItem;

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
			this.Step_05();
			break;
		case 6:
			this.Step_06();
			break;
		case 7:
			base.StartCoroutine(this.Step_07());
			break;
		case 8:
			this.Step_08();
			break;
		case 9:
			base.StartCoroutine(this.Step_09());
			break;
		case 10:
			this.Step_10();
			break;
		case 11:
			this.Step_11();
			break;
		case 12:
			this.Step_12();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Globals.Instance.Player.GetSceneScore(Tutorial_Equip.SceneID) > 0;
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.GetQuestState(Tutorial_Equip.QuestID) == 0)
		{
			global::Debug.LogErrorFormat("Quest is null : {0}", new object[]
			{
				Tutorial_Equip.QuestID
			});
			return;
		}
		this.equipData = Globals.Instance.Player.TeamSystem.GetSocket(0).GetEquip(0);
		if (this.equipData != null && this.equipData.GetEquipEnhanceLevel() > 1)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Equip, true, true, true);
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
			if (Globals.Instance.Player.GetQuestState(Tutorial_Equip.QuestID) == 1)
			{
				base.Step_PVEBtn();
			}
			else if (Globals.Instance.Player.GetQuestState(Tutorial_Equip.QuestID) == 2)
			{
				this.Step_05();
			}
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			base.Step_QuestReceiveBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel && Globals.Instance.Player.GetQuestState(Tutorial_Equip.QuestID) == 2)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.Step_03();
		}
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			GameUIManager.mInstance.ShowPlotDialog(1024, new GUIPlotDialog.FinishCallback(this._PlaySound), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
			base.PlaySound("tutorial_046");
			if (this.equipData == null)
			{
				TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			}
			else
			{
				TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			}
		}
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 2)
		{
			base.PlaySound("tutorial_047");
		}
	}

	private void _PlaySound()
	{
		base.Step_WorldTeam("tutorial52");
		base.PlaySound("tutorial_048");
	}

	private void Step_05()
	{
		if (!(Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene))
		{
			return;
		}
		if (this.equipData == null)
		{
			TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.PlaySound("tutorial_048");
		}
		else
		{
			TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		base.Step_TeamBtn("tutorial52");
	}

	private void Step_06()
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
			TargetName = "UIMiddle/commonPet/midInfo/item0",
			TargetParent = this.teamManageSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial24")
		});
		UIEventListener expr_A0 = UIEventListener.Get(this.area.gameObject);
		expr_A0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A0.onClick, new UIEventListener.VoidDelegate(this.OnStep_06MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(7, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_049");
	}

	private void OnStep_06MaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<GUITeamManageEquipItem>().OnItemClick(null);
	}

	[DebuggerHidden]
	private IEnumerator Step_07()
	{
        return null;
        //Tutorial_Equip.<Step_07>c__Iterator1F <Step_07>c__Iterator1F = new Tutorial_Equip.<Step_07>c__Iterator1F();
        //<Step_07>c__Iterator1F.<>f__this = this;
        //return <Step_07>c__Iterator1F;
	}

	private void OnStep_07MaskAreaClick(GameObject obj)
	{
		this.tempBagItem.OnClickableBtnClick(null);
	}

	private void Step_08()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			this.teamManageSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUITeamManageSceneV2>();
		}
		if (this.teamManageSceneV2 == null)
		{
			return;
		}
		TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		GameUIManager.mInstance.ShowPlotDialog(1027, new GUIPlotDialog.FinishCallback(base.InvokeSelectStep), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent1));
		this.plotIndex = 0;
		base.PlaySound("tutorial_051");
	}

	private void OnPlotClickEvent1()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 2)
		{
			base.PlaySound("tutorial_052");
		}
	}

	[DebuggerHidden]
	private IEnumerator Step_09()
	{
        return null;
        //Tutorial_Equip.<Step_09>c__Iterator20 <Step_09>c__Iterator = new Tutorial_Equip.<Step_09>c__Iterator20();
        //<Step_09>c__Iterator.<>f__this = this;
        //return <Step_09>c__Iterator;
	}

	private void OnStep_09MaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<GUITeamManageEquipItem>().OnItemClick(null);
	}

	private void Step_10()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIEquipInfoPopUp)
		{
			this.equipInfoPopUp = TutorialEntity.ConvertObject2UnityOrPrefab<GUIEquipInfoPopUp>();
		}
		if (this.equipInfoPopUp == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.equipInfoPopUp.gameObject,
			TargetName = "ButtonGroup/Enhance",
			TargetParent = this.equipInfoPopUp.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorialEquip1")
		});
		UIEventListener expr_9C = UIEventListener.Get(this.area.gameObject);
		expr_9C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9C.onClick, new UIEventListener.VoidDelegate(this.OnStep_10MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(11, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_054");
	}

	private void OnStep_10MaskAreaClick(GameObject obj)
	{
		this.equipInfoPopUp.OnEnhanceBtnClick(null);
	}

	private void Step_11()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIEquipUpgradeScene)
		{
			this.equipUpgradeScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIEquipUpgradeScene>();
		}
		if (this.equipUpgradeScene == null)
		{
			return;
		}
		if ((ulong)this.equipUpgradeScene.mEquipData.GetEquipEnhanceCost() > (ulong)((long)Tools.GetCurrencyMoney(ECurrencyType.ECurrencyT_Money, 0)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Equip, true, true, true);
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "WindowBg/EnhanceLayer/EnhanceInfo/AutoEnhanceBtn",
			TargetParent = this.equipUpgradeScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorialEquip2")
		});
		UIEventListener expr_CA = UIEventListener.Get(this.area.gameObject);
		expr_CA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_CA.onClick, new UIEventListener.VoidDelegate(this.OnStep_11MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(12, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_055");
	}

	private void OnStep_11MaskAreaClick(GameObject obj)
	{
		base.ShowFadeBG();
		this.equipUpgradeScene.mEquipEnhanceLayer.OnAutoEnhanceBtnClick(null);
	}

	private void Step_12()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is EquipEnhanceLayer)
		{
			base.HideFadeBG();
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Equip, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1031, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent2));
			this.plotIndex = 0;
		}
	}

	private void OnPlotClickEvent2()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_056");
		}
	}
}
