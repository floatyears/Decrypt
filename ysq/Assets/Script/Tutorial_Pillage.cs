using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_Pillage : TutorialEntity
{
	public static int TrinketInfoID = 1031;

	public static int TrinketFragmentInfoID1 = 310311;

	public static int TrinketFragmentInfoID2 = 310312;

	public static int TrinketFragmentInfoID3 = 310313;

	private GUIPillageScene pillageScene;

	private GUIPillageTargetList pillageTargetList;

	private GUIEquipInfoPopUp equipInfoPopUp;

	private GUISelectEquipBagScene selectEquipBagScene;

	private ItemDataEx trinketData;

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
		case 10:
			this.Step_10();
			break;
		case 11:
			this.Step_11();
			break;
		case 12:
			base.StartCoroutine(this.Step_12());
			break;
		case 13:
			this.Step_13();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(8), true) && (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp || Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene);
	}

	private void Step_01()
	{
		if ((ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(8)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Pillage, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(1068, new GUIPlotDialog.FinishCallback(base.Step_UnlockGoBtn), null);
			GUIPillageScene.LastSelectRecipe = Tutorial_Pillage.TrinketInfoID;
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			this.trinketData = Globals.Instance.Player.TeamSystem.GetSocket(0).GetEquip(4);
			if (this.trinketData != null && this.trinketData.Info.ID == Tutorial_Pillage.TrinketInfoID)
			{
				TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Pillage, true, true, true);
				return;
			}
			if (Globals.Instance.Player.ItemSystem.GetItemByInfoID(Tutorial_Pillage.TrinketInfoID) != null)
			{
				this.Step_09();
				return;
			}
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1068, new GUIPlotDialog.FinishCallback(base.Step_MysteryBtn), null);
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
			TargetName = "Panel/Pillage",
			TargetParent = this.mysteryScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			AnimationPosition = new Vector3(10f, 104f, 0f)
		});
		UIEventListener expr_A5 = UIEventListener.Get(this.area.gameObject);
		expr_A5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A5.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_02MaskAreaClick(GameObject go)
	{
		GUIPillageScene.LastSelectRecipe = Tutorial_Pillage.TrinketInfoID;
		this.mysteryScene.OnPillageClick();
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPillageScene)
		{
			if (Globals.Instance.Player.ItemSystem.GetItemCount(Tutorial_Pillage.TrinketFragmentInfoID1) != 0 && Globals.Instance.Player.ItemSystem.GetItemCount(Tutorial_Pillage.TrinketFragmentInfoID2) != 0 && Globals.Instance.Player.ItemSystem.GetItemCount(Tutorial_Pillage.TrinketFragmentInfoID3) != 0)
			{
				this.Step_05();
			}
			else if (Globals.Instance.Player.Data.Stamina < GameConst.GetInt32(36) || Globals.Instance.Player.ItemSystem.GetItemCount(Tutorial_Pillage.TrinketFragmentInfoID1) == 0 || Globals.Instance.Player.ItemSystem.GetItemCount(Tutorial_Pillage.TrinketFragmentInfoID2) == 0)
			{
				this.Step_07();
			}
			else
			{
				this.pillageScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPillageScene>();
				if (this.pillageScene == null)
				{
					return;
				}
				base.InitGuideMask(new TutorialInitParams
				{
					MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
					TargetObj = this.pillageScene.TrinketItems[2].gameObject,
					HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
					Tips = Singleton<StringManager>.Instance.GetString("tutorial34")
				});
				UIEventListener expr_168 = UIEventListener.Get(this.area.gameObject);
				expr_168.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_168.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
				TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			}
		}
	}

	private void OnStep_03MaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<TrinketItem>().OnTrinketItemClick(this.targetObj);
	}

	[DebuggerHidden]
	private IEnumerator Step_04()
	{
        return null;
        //Tutorial_Pillage.<Step_04>c__Iterator27 <Step_04>c__Iterator = new Tutorial_Pillage.<Step_04>c__Iterator27();
        //<Step_04>c__Iterator.<>f__this = this;
        //return <Step_04>c__Iterator;
	}

	private void OnStep_04MaskAreaClick(GameObject go)
	{
		this.targetObj.transform.parent.parent.GetComponent<PillageTargetItem>().OnPkTraget(null);
	}

	private void Step_05()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPillageScene)
		{
			this.pillageScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIPillageScene>();
		}
		if (this.pillageScene == null)
		{
			return;
		}
		if (Globals.Instance.Player.ItemSystem.GetItemCount(Tutorial_Pillage.TrinketFragmentInfoID3) == 0)
		{
			this.Step_07();
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.pillageScene.btnComposite.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorialPillage2")
		});
		UIEventListener expr_BF = UIEventListener.Get(this.area.gameObject);
		expr_BF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_BF.onClick, new UIEventListener.VoidDelegate(this.OnStep_05MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_05MaskAreaClick(GameObject go)
	{
		this.pillageScene.OnCompositeBtnClick(null);
		base.ShowFadeBG();
	}

	private void Step_06()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIEquipInfoPopUp)
		{
			this.equipInfoPopUp = TutorialEntity.ConvertObject2UnityOrPrefab<GUIEquipInfoPopUp>();
		}
		if (this.equipInfoPopUp == null)
		{
			return;
		}
		base.HideFadeBG();
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.equipInfoPopUp.gameObject,
			TargetName = "CloseBtn",
			TargetParent = this.equipInfoPopUp.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial22")
		});
		UIEventListener expr_A2 = UIEventListener.Get(this.area.gameObject);
		expr_A2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A2.onClick, new UIEventListener.VoidDelegate(this.OnStep_06MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(7, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_06MaskAreaClick(GameObject go)
	{
		this.equipInfoPopUp.OnCloseBtnClick(null);
		Globals.Instance.TutorialMgr.CurrentScene = GameUIManager.mInstance.GetSession<GUIPillageScene>();
		this.SelectStep();
	}

	private void Step_07()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPillageScene)
		{
			TutorialEntity.SetNextTutorialStep(8, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1071, new GUIPlotDialog.FinishCallback(base.Step_BackBtn), null);
			this.step_BackCB = delegate(GameObject obj)
			{
				GameUIManager.mInstance.ChangeSession<GUIMysteryScene>(null, false, true);
			};
		}
	}

	private void Step_08()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMysteryScene)
		{
			TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.Step_Back2Main(null);
		}
	}

	private void Step_09()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			TutorialEntity.SetNextTutorialStep(10, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.Step_TeamBtn();
		}
	}

	private void Step_10()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUITeamManageSceneV2)
		{
			if (this.trinketData != null)
			{
				TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Pillage, false, false, false);
				GameUIManager.mInstance.ShowPlotDialog(1073, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
			}
			else if (Globals.Instance.Player.ItemSystem.GetItemByInfoID(Tutorial_Pillage.TrinketInfoID) == null)
			{
				this.Step_13();
			}
			else
			{
				this.Step_11();
			}
		}
	}

	private void Step_11()
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
			TargetName = "UIMiddle/commonPet/midInfo/item4",
			TargetParent = this.teamManageSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial35")
		});
		UIEventListener expr_A0 = UIEventListener.Get(this.area.gameObject);
		expr_A0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A0.onClick, new UIEventListener.VoidDelegate(this.OnStep_11MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(12, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_11MaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<GUITeamManageEquipItem>().OnItemClick(null);
	}

	[DebuggerHidden]
	private IEnumerator Step_12()
	{
        return null;
        //Tutorial_Pillage.<Step_12>c__Iterator28 <Step_12>c__Iterator = new Tutorial_Pillage.<Step_12>c__Iterator28();
        //<Step_12>c__Iterator.<>f__this = this;
        //return <Step_12>c__Iterator;
	}

	private void OnStep_12MaskAreaClick(GameObject obj)
	{
		this.targetObj.transform.parent.GetComponent<GUISelectEquipBagItem>().OnClickableBtnClick(null);
	}

	private void Step_13()
	{
		TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Pillage, false, false, false);
		GameUIManager.mInstance.ShowPlotDialog(1073, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
	}
}
