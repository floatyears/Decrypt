using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Tutorial_MapReward : TutorialEntity
{
	public static int MapID = 1001;

	public static int StarIndex = 2;

	public static int QuestID = 101010;

	public static int SceneID = 101009;

	private GameUIMapReward mapReward;

	private float hideTime;

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
			this.Step_06(this.hideTime);
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
		}
		return false;
	}

	public static bool IsMeetConditions()
	{
		MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(Tutorial_MapReward.MapID);
		return info != null && Tools.GetPVEStars(Tutorial_MapReward.MapID) >= info.NeedStar[Tutorial_MapReward.StarIndex];
	}

	private void Step_01()
	{
		if ((Globals.Instance.Player.GetMapRewardMask(Tutorial_MapReward.MapID) & 1 << Tutorial_MapReward.StarIndex) != 0)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_MapReward, true, true, true);
			return;
		}
		TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		this.Step_02();
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(Tutorial_MapReward.SceneID);
			if (info != null)
			{
				GameUIManager.mInstance.uiState.ResultSceneInfo = info;
			}
			base.Step_VictoryOKBtn(null);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			SceneInfo info2 = Globals.Instance.AttDB.SceneDict.GetInfo(Tutorial_MapReward.SceneID);
			if (info2 != null)
			{
				GameUIManager.mInstance.uiState.ResultSceneInfo2 = info2;
			}
			base.Step_PVEBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1042, new GUIPlotDialog.FinishCallback(this.Step_03), null);
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			base.Step_QuestReceiveBtn();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel && Globals.Instance.Player.GetQuestState(Tutorial_MapReward.QuestID) == 2)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			GameUIManager.mInstance.ShowPlotDialog(1042, new GUIPlotDialog.FinishCallback(this.Step_03), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
			base.PlaySound("tutorial_035");
		}
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_036");
		}
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			this.worldMap = TutorialEntity.ConvertObject2UnityOrPrefab<GUIWorldMap>();
		}
		else if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			this.worldMap = GameUIManager.mInstance.GetSession<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			TargetName = "topPanel/box",
			TargetParent = this.worldMap.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightUp,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial21")
		});
		UIEventListener expr_CE = UIEventListener.Get(this.area.gameObject);
		expr_CE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_CE.onClick, new UIEventListener.VoidDelegate(this.OnStep_03MaskAreaClick));
		base.PlaySound("tutorial_037");
	}

	private void OnStep_03MaskAreaClick(GameObject obj)
	{
		TutorialEntity.SetNextTutorialStep(8, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		this.worldMap.OnBoxClick(null);
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIMapReward)
		{
			this.mapReward = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIMapReward>();
		}
		if (this.mapReward == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mapReward.gameObject,
			TargetName = "winBG/group1/0",
			TargetParent = this.mapReward.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right
		});
		UIEventListener expr_87 = UIEventListener.Get(this.area.gameObject);
		expr_87.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_87.onClick, new UIEventListener.VoidDelegate(this.OnStep_04MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(5, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_04MaskAreaClick(GameObject obj)
	{
		this.mapReward.OnGroupClicked(this.targetObj);
	}

	private void Step_05()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			this.hideTime = 2f;
			TutorialEntity.SetNextTutorialStep(6, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.SelectStep();
		}
	}

	private void Step_06(float time)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIMapReward)
		{
			this.mapReward = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIMapReward>();
		}
		if (this.mapReward == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mapReward.gameObject,
			TargetName = "winBG/group2/1",
			TargetParent = this.mapReward.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right,
			HideGuideMask4Seconds = time
		});
		UIEventListener expr_8E = UIEventListener.Get(this.area.gameObject);
		expr_8E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8E.onClick, new UIEventListener.VoidDelegate(this.OnStep_06MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(7, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void OnStep_06MaskAreaClick(GameObject obj)
	{
		this.mapReward.OnGroupClicked(this.targetObj);
	}

	private void Step_07()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			this.hideTime = 2f;
			TutorialEntity.SetNextTutorialStep(8, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.SelectStep();
		}
	}

	private void Step_08()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIMapReward)
		{
			this.mapReward = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIMapReward>();
		}
		if (this.mapReward == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mapReward.gameObject,
			TargetName = "winBG/group3/2",
			TargetParent = this.mapReward.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Right,
			HideGuideMask4Seconds = this.hideTime,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial48")
		});
		UIEventListener expr_A8 = UIEventListener.Get(this.area.gameObject);
		expr_A8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A8.onClick, new UIEventListener.VoidDelegate(this.OnStep_08MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.PlaySound("tutorial_038");
	}

	private void OnStep_08MaskAreaClick(GameObject obj)
	{
		this.mapReward.OnGroupClicked(this.targetObj);
	}

	private void Step_09()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIRewardPanel)
		{
			TutorialEntity.SetNextTutorialStep(9, TutorialManager.ETutorialNum.Tutorial_MapReward, false, false, false);
			TutorialEntity.SetNextTutorialStep(10, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			this.SelectStep();
		}
	}

	private void Step_10()
	{
		if (this.mapReward == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mapReward.gameObject,
			TargetName = "winBG/closeBtn",
			TargetParent = this.mapReward.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left,
			HideGuideMask4Seconds = 2f,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial22")
		});
		UIEventListener expr_83 = UIEventListener.Get(this.area.gameObject);
		expr_83.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_83.onClick, new UIEventListener.VoidDelegate(this.OnStep_10MaskAreaClick));
		TutorialEntity.SetNextTutorialStep(11, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		base.StartCoroutine(this.DelayPlaySound());
	}

	[DebuggerHidden]
	private IEnumerator DelayPlaySound()
	{
        return null;
        //Tutorial_MapReward.<DelayPlaySound>c__Iterator23 <DelayPlaySound>c__Iterator = new Tutorial_MapReward.<DelayPlaySound>c__Iterator23();
        //<DelayPlaySound>c__Iterator.<>f__this = this;
        //return <DelayPlaySound>c__Iterator;
	}

	private void OnStep_10MaskAreaClick(GameObject obj)
	{
		this.mapReward.OnCloseBtnClicked(null);
		this.SelectStep();
	}

	private void Step_11()
	{
		GameUIManager.mInstance.ShowPlotDialog(1044, new GUIPlotDialog.FinishCallback(this._PlaySound), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent2));
		TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Null, true, false, false);
		base.PlaySound("tutorial_040");
		this.plotIndex = 0;
	}

	private void OnPlotClickEvent2()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 1)
		{
			base.PlaySound("tutorial_041");
		}
	}

	private void _PlaySound()
	{
		base.Step_WorldTeam("tutorial49");
		base.PlaySound("tutorial_042");
	}
}
