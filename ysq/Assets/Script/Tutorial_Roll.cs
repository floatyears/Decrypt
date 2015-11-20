using System;
using UnityEngine;

public class Tutorial_Roll : TutorialEntity
{
	private int passLevel = 2;

	private GUIRollSceneV2 rollSceneV2;

	private GUIRollingSceneV2 rollingSceneV2;

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
		}
		return false;
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.PetSystem.Values.Count > 2 || (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)this.passLevel))
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Roll, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			GameUIManager.mInstance.ShowPlotDialog(1006, new GUIPlotDialog.FinishCallback(this.Step_02), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
			base.PlaySound("tutorial_001");
		}
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		switch (this.plotIndex)
		{
		case 2:
			base.PlaySound("tutorial_002");
			break;
		case 4:
			base.PlaySound("tutorial_003");
			break;
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
			TargetName = "UI_Edge/Roll",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			HideTargetObj = true,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial0")
		});
		UIEventListener expr_B2 = UIEventListener.Get(this.area.gameObject);
		expr_B2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B2.onClick, new UIEventListener.VoidDelegate(this.OnStep_02MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_004");
	}

	private void OnStep_02MaskAreaClick(GameObject obj)
	{
		this.mainMenuScene.OnRollClick(null);
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRollSceneV2)
		{
			this.rollSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUIRollSceneV2>();
		}
		if (this.rollSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "High",
			TargetParent = this.rollSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right,
			AnimationPosition = new Vector3(0f, -180f, 0f),
			Tips = Singleton<StringManager>.Instance.GetString("tutorial1")
		});
		UIEventListener expr_BA = UIEventListener.Get(this.area.gameObject);
		expr_BA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_BA.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_005");
	}

	private void OnStep_03MaskAreaClick(GameObject obj)
	{
		this.rollSceneV2.OnHighClick(null);
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRollingSceneV2)
		{
			this.rollingSceneV2 = TutorialEntity.ConvertObject2UnityOrPrefab<GUIRollingSceneV2>();
		}
		if (this.rollingSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.rollingSceneV2.gameObject,
			TargetName = "Window/RollOne",
			TargetParent = this.rollingSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial2")
		});
		UIEventListener expr_9C = UIEventListener.Get(this.area.gameObject);
		expr_9C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9C.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_006");
	}

	private void OnStep_04MaskAreaClick(GameObject obj)
	{
		this.rollingSceneV2.OnRollOneClick(null);
	}

	private void Step_05()
	{
		if (!(Globals.Instance.TutorialMgr.CurrentScene is GetPetLayer))
		{
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
			{
				this.Step_01();
			}
			return;
		}
		if (this.rollingSceneV2 == null)
		{
			this.rollingSceneV2 = GameUIManager.mInstance.GetSession<GUIRollingSceneV2>();
		}
		if (this.rollingSceneV2 == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.rollingSceneV2.gameObject,
			TargetName = "Window/OK",
			TargetParent = this.rollingSceneV2.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial13")
		});
		UIEventListener expr_D7 = UIEventListener.Get(this.area.gameObject);
		expr_D7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D7.onClick, new UIEventListener.VoidDelegate(this.OnStep_05MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_007");
	}

	private void OnStep_05MaskAreaClick(GameObject obj)
	{
		this.rollingSceneV2.OnOKClick(null);
	}

	private void Step_06()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRollSceneV2)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Roll, true, false, false);
			base.Step_BackBtn("tutorial20");
			base.PlaySound("tutorial_008");
		}
	}
}
