using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public enum ETutorialNum
	{
		Tutorial_Null,
		Tutorial_Roll,
		Tutorial_Team1,
		Tutorial_1_1,
		Tutorial_1_2,
		Tutorial_1_3,
		Tutorial_Team2,
		Tutorial_PetLevelUp,
		Tutorial_PVP,
		Tutorial_Pillage,
		Tutorial_Constellation,
		Tutorial_Trial,
		Tutorial_CostumeParty,
		Tutorial_EquipRefine,
		Tutorial_KingReward,
		Tutorial_AwakeRoad,
		Tutorial_MapReward,
		Tutorial_PetFurther,
		Tutorial_Equip,
		Tutorial_Recycle,
		Tutorial_ElitePVE,
		Tutorial_PetSkillLU,
		Tutorial_Stop
	}

	public UnityEngine.Object CurrentScene;

	private TutorialEntity tutorial;

	public TutorialManager.ETutorialNum CurrentTutorialNum;

	public List<int> TutorialSteps = new List<int>();

	public int CurrentTutorialStep = 1;

	public int LastTutorialStep = 1;

	private GUIPlotDialog.FinishCallback FinishEvent;

	public bool StartTutorial = true;

	private bool finishInit;

	private bool hasFinishTutorial;

	public TutorialEntity Tutorial
	{
		get
		{
			return this.tutorial;
		}
		private set
		{
			this.tutorial = value;
		}
	}

	public bool IsNull
	{
		get
		{
			return this.CurrentTutorialNum == TutorialManager.ETutorialNum.Tutorial_Null || this.CurrentTutorialNum == TutorialManager.ETutorialNum.Tutorial_Stop;
		}
	}

	public void Destroy()
	{
		this.CurrentScene = null;
		this.Tutorial = null;
		this.CurrentTutorialNum = TutorialManager.ETutorialNum.Tutorial_Null;
		this.TutorialSteps.Clear();
		this.CurrentTutorialStep = 1;
		this.LastTutorialStep = 1;
		this.FinishEvent = null;
		this.StartTutorial = true;
		this.finishInit = false;
		this.hasFinishTutorial = false;
	}

	public void Init(List<int> guideSteps)
	{
		if (this.finishInit)
		{
			return;
		}
		this.TutorialSteps = guideSteps;
		this.finishInit = true;
		this.hasFinishTutorial = true;
		for (TutorialManager.ETutorialNum eTutorialNum = TutorialManager.ETutorialNum.Tutorial_1_1; eTutorialNum < TutorialManager.ETutorialNum.Tutorial_Stop; eTutorialNum++)
		{
			if (!this.IsPassTutorial(eTutorialNum))
			{
				this.hasFinishTutorial = false;
				break;
			}
		}
	}

	public void UpdateTutorialSteps(TutorialManager.ETutorialNum currentNum)
	{
		int i = (int)((int)currentNum / 32);
		if (i >= 10)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("TutorialStep Index Error , {0} ", i)
			});
			return;
		}
		while (i >= this.TutorialSteps.Count)
		{
			this.TutorialSteps.Add(0);
		}
		if (this.TutorialSteps.Count > 10)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("TutorialStep Count Error , {0} ", this.TutorialSteps.Count)
			});
			return;
		}
		int num = (int)((int)currentNum % 32);
		List<int> tutorialSteps;
		List<int> expr_9B = tutorialSteps = this.TutorialSteps;
		int num2;
		int expr_9E = num2 = i;
		num2 = tutorialSteps[num2];
		expr_9B[expr_9E] = (num2 | 1 << num);
	}

	public bool IsPassTutorial(TutorialManager.ETutorialNum curStep)
	{
        int num = (int)((int)curStep / 32);
		if (num >= 10)
		{
			global::Debug.LogErrorFormat("TutorialStep Index Error , {0} ", new object[]
			{
				num
			});
			return true;
		}
		if (num >= this.TutorialSteps.Count)
		{
			return false;
		}
        int num2 = (int)((int)curStep % 32);
		return (this.TutorialSteps[num] & 1 << num2) != 0;
	}

	public void UpdateCurrentTutorialNum()
	{
		switch (this.CurrentTutorialNum)
		{
		case TutorialManager.ETutorialNum.Tutorial_Null:
			this.CurrentTutorialNum++;
			this.UpdateCurrentTutorialNum();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Roll:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Team1:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_1_1:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_1_2:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_1_3:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Team2:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_PetLevelUp:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_PVP:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_PVP.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Pillage:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_Pillage.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Constellation:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_Constellation.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Trial:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_Trial.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_CostumeParty:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_CostumeParty.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_EquipRefine:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_EquipRefine.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_KingReward:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_KingReward.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_AwakeRoad:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_AwakeRoad.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_MapReward:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_MapReward.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_PetFurther:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_PetFurther.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Equip:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_Equip.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Recycle:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_Recycle.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_ElitePVE:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_ElitePVE.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_PetSkillLU:
			if (this.IsPassTutorial(this.CurrentTutorialNum))
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			else if (!Tutorial_PetSkillLU.IsMeetConditions())
			{
				this.CurrentTutorialNum++;
				this.UpdateCurrentTutorialNum();
			}
			break;
		case TutorialManager.ETutorialNum.Tutorial_Stop:
			this.CurrentTutorialNum = TutorialManager.ETutorialNum.Tutorial_Null;
			break;
		}
	}

	public bool IsMatineeBoss(int monsterInfoID)
	{
		return Globals.Instance.SenceMgr.sceneInfo != null && ((Globals.Instance.Player.GetSceneScore(101010) <= 0 && Globals.Instance.SenceMgr.sceneInfo.ID == 101010) || (Globals.Instance.Player.GetSceneScore(102002) <= 0 && Globals.Instance.SenceMgr.sceneInfo.ID == 102002) || (Globals.Instance.Player.GetSceneScore(102008) <= 0 && Globals.Instance.SenceMgr.sceneInfo.ID == 102008));
	}

	public bool HasTutorialInThisScene(UnityEngine.Object scene)
	{
		if (this.CurrentTutorialNum == TutorialManager.ETutorialNum.Tutorial_Null)
		{
			this.CurrentScene = scene;
			this.UpdateCurrentTutorialNum();
			this.CurrentScene = null;
		}
		return this.CurrentTutorialNum != TutorialManager.ETutorialNum.Tutorial_Null && this.CurrentTutorialNum != TutorialManager.ETutorialNum.Tutorial_Stop;
	}

	public void InvokeFinishEvent()
	{
		if (this.FinishEvent != null)
		{
			this.FinishEvent();
			this.FinishEvent = null;
		}
	}

	public void InitializationCompleted(UnityEngine.Object scene, GUIPlotDialog.FinishCallback callBack = null)
	{
		if (!this.StartTutorial || !this.finishInit || this.hasFinishTutorial)
		{
			if (callBack != null)
			{
				callBack();
			}
			return;
		}
		this.CurrentScene = scene;
		this.FinishEvent = callBack;
		if (this.CurrentTutorialNum == TutorialManager.ETutorialNum.Tutorial_Null)
		{
			this.UpdateCurrentTutorialNum();
		}
		if (this.CurrentTutorialNum == TutorialManager.ETutorialNum.Tutorial_Null || this.CurrentTutorialNum == TutorialManager.ETutorialNum.Tutorial_Stop)
		{
			if (this.Tutorial != null && this.Tutorial.guideAnimation != null)
			{
				UnityEngine.Object.Destroy(this.Tutorial.guideAnimation);
				this.Tutorial.guideAnimation = null;
			}
			this.InvokeFinishEvent();
			return;
		}
		switch (this.CurrentTutorialNum)
		{
		case TutorialManager.ETutorialNum.Tutorial_Roll:
			this.InvokeTutorial_Roll();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Team1:
			this.InvokeTutorial_Team1();
			break;
		case TutorialManager.ETutorialNum.Tutorial_1_1:
			this.InvokeTutorial_1_1();
			break;
		case TutorialManager.ETutorialNum.Tutorial_1_2:
			this.InvokeTutorial_1_2();
			break;
		case TutorialManager.ETutorialNum.Tutorial_1_3:
			this.InvokeTutorial_1_3();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Team2:
			this.InvokeTutorial_Team2();
			break;
		case TutorialManager.ETutorialNum.Tutorial_PetLevelUp:
			this.InvokeTutorial_PetLevelUp();
			break;
		case TutorialManager.ETutorialNum.Tutorial_PVP:
			this.InvokeTutorial_PVP();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Pillage:
			this.InvokeTutorial_Pillage();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Constellation:
			this.InvokeTutorial_Constellation();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Trial:
			this.InvokeTutorial_Trial();
			break;
		case TutorialManager.ETutorialNum.Tutorial_CostumeParty:
			this.InvokeTutorial_CostumeParty();
			break;
		case TutorialManager.ETutorialNum.Tutorial_EquipRefine:
			this.InvokeTutorial_EquipRefine();
			break;
		case TutorialManager.ETutorialNum.Tutorial_KingReward:
			this.InvokeTutorial_KingReward();
			break;
		case TutorialManager.ETutorialNum.Tutorial_AwakeRoad:
			this.InvokeTutorial_AwakeRoad();
			break;
		case TutorialManager.ETutorialNum.Tutorial_MapReward:
			this.InvokeTutorial_MapReward();
			break;
		case TutorialManager.ETutorialNum.Tutorial_PetFurther:
			this.InvokeTutorial_PetFurther();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Equip:
			this.InvokeTutorial_Equip();
			break;
		case TutorialManager.ETutorialNum.Tutorial_Recycle:
			this.InvokeTutorial_Recycle();
			break;
		case TutorialManager.ETutorialNum.Tutorial_ElitePVE:
			this.InvokeTutorial_ElitePVE();
			break;
		case TutorialManager.ETutorialNum.Tutorial_PetSkillLU:
			this.InvokeTutorial_PetSkillLU();
			break;
		}
	}

	public void ClearGuideMask()
	{
		if (this.Tutorial != null && this.Tutorial.guideMask != null)
		{
			UnityEngine.Object.Destroy(this.Tutorial.guideMask);
			this.Tutorial.guideMask = null;
		}
	}

	private void InstantiateTutorialInstance<T>() where T : TutorialEntity
	{
		if (!(this.Tutorial is T) || this.Tutorial == null)
		{
			this.ClearTutorial();
			T component = base.gameObject.GetComponent<T>();
			if (component != null)
			{
				this.Tutorial = component;
			}
			else
			{
				this.Tutorial = base.gameObject.AddComponent<T>();
			}
		}
	}

	public void ClearTutorial()
	{
		UnityEngine.Object.Destroy(this.Tutorial);
		this.Tutorial = null;
	}

	private void InvokeTutorial_1_1()
	{
		this.InstantiateTutorialInstance<Tutorial_1_1>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Roll()
	{
		this.InstantiateTutorialInstance<Tutorial_Roll>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Team1()
	{
		this.InstantiateTutorialInstance<Tutorial_Team1>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_1_2()
	{
		this.InstantiateTutorialInstance<Tutorial_1_2>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_1_3()
	{
		this.InstantiateTutorialInstance<Tutorial_1_3>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Team2()
	{
		this.InstantiateTutorialInstance<Tutorial_Team2>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_PetLevelUp()
	{
		this.InstantiateTutorialInstance<Tutorial_PetLevelUp>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Equip()
	{
		this.InstantiateTutorialInstance<Tutorial_Equip>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_MapReward()
	{
		this.InstantiateTutorialInstance<Tutorial_MapReward>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_PetFurther()
	{
		this.InstantiateTutorialInstance<Tutorial_PetFurther>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_PVP()
	{
		this.InstantiateTutorialInstance<Tutorial_PVP>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Pillage()
	{
		this.InstantiateTutorialInstance<Tutorial_Pillage>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Constellation()
	{
		this.InstantiateTutorialInstance<Tutorial_Constellation>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Trial()
	{
		this.InstantiateTutorialInstance<Tutorial_Trial>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_CostumeParty()
	{
		this.InstantiateTutorialInstance<Tutorial_CostumeParty>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_EquipRefine()
	{
		this.InstantiateTutorialInstance<Tutorial_EquipRefine>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_KingReward()
	{
		this.InstantiateTutorialInstance<Tutorial_KingReward>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_AwakeRoad()
	{
		this.InstantiateTutorialInstance<Tutorial_AwakeRoad>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_Recycle()
	{
		this.InstantiateTutorialInstance<Tutorial_Recycle>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_ElitePVE()
	{
		this.InstantiateTutorialInstance<Tutorial_ElitePVE>();
		this.Tutorial.SelectStep();
	}

	private void InvokeTutorial_PetSkillLU()
	{
		this.InstantiateTutorialInstance<Tutorial_PetSkillLU>();
		this.Tutorial.SelectStep();
	}

	private void InitTutorial()
	{
		this.CurrentTutorialStep = 1;
		this.CurrentTutorialNum = TutorialManager.ETutorialNum.Tutorial_Null;
		this.ClearGuideMask();
		this.ClearTutorial();
		this.InitializationCompleted(this.CurrentScene, null);
	}
}
