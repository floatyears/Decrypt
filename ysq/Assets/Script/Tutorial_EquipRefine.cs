using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_EquipRefine : TutorialEntity
{
	private GUIEquipBagScene equipBagScene;

	private GUIEquipBagItem tempBagItem;

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
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		return Tools.CanPlay(GameConst.GetInt32(11), true) && TutorialEntity.IsLevelUpTutorialScene();
	}

	private void Step_01()
	{
		if ((ulong)Globals.Instance.Player.Data.Level > (ulong)((long)GameConst.GetInt32(11)))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_EquipRefine, true, true, true);
			return;
		}
		bool flag = false;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 0)
			{
				flag = true;
				if (current.GetEquipRefineLevel() > 0)
				{
					TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_EquipRefine, true, true, true);
					return;
				}
			}
		}
		if (!flag)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_EquipRefine, true, true, true);
			return;
		}
		TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		this.Step_02();
	}

	private void Step_02()
	{
		base.LevelUpFilter(1099, 3);
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
			TargetName = "UI_Edge/equipBag",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial25")
		});
		UIEventListener expr_AB = UIEventListener.Get(this.area.gameObject);
		expr_AB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AB.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_03MaskAreaClick(GameObject go)
	{
		this.mainMenuScene.OnEquipsBagClick(null);
	}

	[DebuggerHidden]
	private IEnumerator Step_05()
	{
        return null;
        //Tutorial_EquipRefine.<Step_05>c__Iterator21 <Step_05>c__Iterator = new Tutorial_EquipRefine.<Step_05>c__Iterator21();
        //<Step_05>c__Iterator.<>f__this = this;
        //return <Step_05>c__Iterator;
	}

	private void OnStep_05MaskAreaClick(GameObject obj)
	{
		this.tempBagItem.OnListBtnClick(null);
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		base.Invoke("Step_06", 0.3f);
	}

	private void Step_06()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetObj = this.tempBagItem.mRefineBtn,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial38")
		});
		UIEventListener expr_6A = UIEventListener.Get(this.area.gameObject);
		expr_6A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6A.onClick, new UIEventListener.VoidDelegate(this.OnStep_06MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(7, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_06MaskAreaClick(GameObject obj)
	{
		this.tempBagItem.OnRefineClick(null);
	}

	private void Step_07()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIEquipUpgradeScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1097, null, null);
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_EquipRefine, true, true, true);
		}
	}
}
