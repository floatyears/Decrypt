using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_Recycle : TutorialEntity
{
	public static int SceneID = 102005;

	public static int SceneID1 = 102006;

	public static int QuestID = 102005;

	private GUIRecycleScene recycleScene;

	private GUILvlUpSelPetSceneV2 lvlUpSelPetSceneV2;

	private GUIRecycleGetItemsPopUp recycleGetItemsPopUp;

	private int plotIndex;

	public static void PassThis()
	{
		if (Globals.Instance.TutorialMgr.IsPassTutorial(TutorialManager.ETutorialNum.Tutorial_Recycle))
		{
			return;
		}
		TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Recycle, true, true, true);
	}

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
		case 7:
			this.Step_07();
			break;
		case 8:
			base.StartCoroutine(this.Step_08());
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
			this.Step_12();
			break;
		case 13:
			this.Step_13();
			break;
		case 14:
			this.Step_14();
			break;
		case 15:
			this.Step_15();
			break;
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Globals.Instance.Player.GetSceneScore(Tutorial_Recycle.SceneID) > 0;
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.GetSceneScore(Tutorial_Recycle.SceneID1) > 0)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Recycle, true, true, true);
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
			if (Globals.Instance.Player.GetQuestState(Tutorial_Recycle.QuestID) == 1)
			{
				base.Step_PVEBtn();
			}
			else if (Globals.Instance.Player.GetQuestState(Tutorial_Recycle.QuestID) == 2)
			{
				bool flag = false;
				foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
				{
					if (current.Info.Quality == 0 && !current.IsBattling() && !current.IsPetAssisting())
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Recycle, true, true, true);
					return;
				}
				this.Step_04();
			}
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			base.Step_QuestReceiveBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel && Globals.Instance.Player.GetQuestState(Tutorial_Recycle.QuestID) == 2)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.Step_03();
		}
	}

	private void Step_03()
	{
		GameUIManager.mInstance.ShowPlotDialog(1048, new GUIPlotDialog.FinishCallback(this._PlaySound), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num != 1)
		{
			if (num == 2)
			{
				base.PlaySound("tutorial_058");
			}
		}
		else
		{
			base.PlaySound("tutorial_057");
		}
	}

	private void _PlaySound()
	{
		base.Step_Back2Main("tutorial54");
		base.PlaySound("tutorial_059");
	}

	private void Step_04()
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
			TargetName = "UI_Edge/Recycle",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial29")
		});
		UIEventListener expr_AB = UIEventListener.Get(this.area.gameObject);
		expr_AB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AB.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(7, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_04MaskAreaClick(GameObject go)
	{
		this.mainMenuScene.OnRecycleClick(null);
	}

	private void Step_07()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRecycleScene)
		{
			this.recycleScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIRecycleScene>();
		}
		if (this.recycleScene == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetParent = this.recycleScene.mRecycleLayer.gameObject,
			TargetName = "AutoAddBtn",
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial30")
		});
		UIEventListener expr_A5 = UIEventListener.Get(this.area.gameObject);
		expr_A5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A5.onClick, new UIEventListener.VoidDelegate(this.OnStep_07MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(10, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_061");
	}

	private void OnStep_07MaskAreaClick(GameObject go)
	{
		this.recycleScene.mRecycleLayer.OnAutoAddBtnClick(null);
		this.SelectStep();
	}

	[DebuggerHidden]
	private IEnumerator Step_08()
	{
        return null;
        //Tutorial_Recycle.<Step_08>c__Iterator29 <Step_08>c__Iterator = new Tutorial_Recycle.<Step_08>c__Iterator29();
        //<Step_08>c__Iterator.<>f__this = this;
        //return <Step_08>c__Iterator;
	}

	private void OnStep_08MaskAreaClick(GameObject go)
	{
		((GUILvlUpSelectItem)this.lvlUpSelPetSceneV2.mGUILvlUpSelectItemTable.gridItems[0]).OnToggleBtnClick();
		this.SelectStep();
	}

	private void Step_09()
	{
		if (this.lvlUpSelPetSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "UIMiddle/WindowBg/sureBtn",
			TargetParent = this.lvlUpSelPetSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown
		});
		UIEventListener expr_67 = UIEventListener.Get(this.area.gameObject);
		expr_67.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_67.onClick, new UIEventListener.VoidDelegate(this.OnStep_09MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(10, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_09MaskAreaClick(GameObject go)
	{
		this.lvlUpSelPetSceneV2.OnSureBtnClick(null);
	}

	private void Step_10()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRecycleScene)
		{
			this.recycleScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIRecycleScene>();
		}
		if (this.recycleScene == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.recycleScene.mRecycleLayer.mBreakBtn,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial31")
		});
		UIEventListener expr_9A = UIEventListener.Get(this.area.gameObject);
		expr_9A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9A.onClick, new UIEventListener.VoidDelegate(this.OnStep_10MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(11, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_062");
	}

	private void OnStep_10MaskAreaClick(GameObject go)
	{
		this.recycleScene.mRecycleLayer.OnBreakBtnClick(null);
	}

	private void Step_11()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRecycleGetItemsPopUp)
		{
			this.recycleGetItemsPopUp = TutorialEntity.ConvertObject2UnityOrPrefab<GUIRecycleGetItemsPopUp>();
		}
		if (this.recycleGetItemsPopUp == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "OKBtn",
			TargetParent = this.recycleGetItemsPopUp.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial55")
		});
		UIEventListener expr_A0 = UIEventListener.Get(this.area.gameObject);
		expr_A0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A0.onClick, new UIEventListener.VoidDelegate(this.OnStep_11MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(12, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_063");
	}

	private void OnStep_11MaskAreaClick(GameObject go)
	{
		this.recycleGetItemsPopUp.OnOKClick(null);
		base.ShowFadeBG();
	}

	private void Step_12()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			base.HideFadeBG();
			TutorialEntity.SetNextTutorialStep(13, TutorialManager.ETutorialNum.Tutorial_Recycle, false, false, false);
			this.Step_13();
		}
	}

	private void Step_13()
	{
		GameUIManager.mInstance.ShowPlotDialog(1051, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent1));
		base.PlaySound("tutorial_064");
		this.plotIndex = 0;
	}

	private void OnPlotClickEvent1()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_065");
		}
	}

	private void Step_14()
	{
		if (this.recycleScene == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.recycleScene.mRecycleLayer.mShop.gameObject,
			TargetParent = this.recycleScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right
		});
		UIEventListener expr_77 = UIEventListener.Get(this.area.gameObject);
		expr_77.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_77.onClick, new UIEventListener.VoidDelegate(this.OnStep_14MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(15, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_14MaskAreaClick(GameObject go)
	{
		this.recycleScene.mRecycleLayer.OnShopClick(null);
	}

	private void Step_15()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIShopScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1054, new GUIPlotDialog.FinishCallback(base.Step_BackBtn), null);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_PVEBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			base.StartNextTutorial();
		}
		else
		{
			base.Step_BackBtn();
		}
	}
}
