using System;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_2 : TutorialEntity
{
	public static int ConcentratedFireMonsterID = 10012;

	public static int SceneID = 101002;

	private PlayerController player;

	private bool hasCastAOESkill;

	private int status;

	private CombatMainSkillButton aoeSkillBtn;

	private CombatMainSkillButton singleSkillBtn;

	private float freshTime;

	private WorldScene mWorldScene;

	private ActorController catActor;

	private bool hasConcentratedFire;

	public override bool SelectStep()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			Globals.Instance.TutorialMgr.CurrentTutorialStep = 1;
		}
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
		}
		return false;
	}

	private void Step_01()
	{
		if (Globals.Instance.Player.GetSceneScore(Tutorial_1_2.SceneID) > 0)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_1_2, true, true, true);
			return;
		}
		TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
		this.Step_02();
	}

	private void Step_02()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene != null)
		{
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultFailureScene)
			{
				this.InitParms();
				base.ResetFadeBGArea();
				base.Step_FailureOKBtn();
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
			{
				this.InitParms();
				base.Step_PVEBtn();
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
			{
				this.InitParms();
				base.ResetFadeBGArea();
				base.Step_SceneBtn(2, "tutorial43");
				base.PlaySound("tutorial_021");
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GameUIAdventureReady)
			{
				this.InitParms();
				base.Step_StartSceneBtn("tutorial44");
				base.PlaySound("tutorial_022");
				return;
			}
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (this.hasCastAOESkill)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUICombatMain)
		{
			this.combatMain = TutorialEntity.ConvertObject2UnityOrPrefab<GUICombatMain>();
			if (this.combatMain == null)
			{
				return;
			}
			this.player = Globals.Instance.ActorMgr.PlayerCtrler;
			this.singleSkillBtn = GameUITools.FindGameObject("right-bottom/skill1", this.combatMain.gameObject).GetComponent<CombatMainSkillButton>();
			this.aoeSkillBtn = GameUITools.FindGameObject("right-bottom/skill2", this.combatMain.gameObject).GetComponent<CombatMainSkillButton>();
			if (Globals.Instance.ActorMgr.CurScene is WorldScene)
			{
				this.mWorldScene = (WorldScene)Globals.Instance.ActorMgr.CurScene;
			}
			if (this.aoeSkillBtn == null)
			{
				global::Debug.LogError(new object[]
				{
					"skill is null"
				});
				return;
			}
			this.status = 3;
		}
	}

	private void Update()
	{
		if (this.player != null)
		{
			switch (this.status)
			{
			case 1:
				if (Globals.Instance.ActorMgr.GetBossActor() != null && Globals.Instance.ActorMgr.GetBossActor().gameObject.activeInHierarchy)
				{
					this.status = 10;
					return;
				}
				if (this.mWorldScene == null)
				{
					return;
				}
				if (this.mWorldScene.GetRespawnIndex() > 1)
				{
					this.status = 10;
					return;
				}
				if (this.mWorldScene.GetRespawnIndex() == 1)
				{
					this.SkillTutorialEnd();
					return;
				}
				if (this.player.ActorCtrler.AiCtrler.Target != null && !this.player.ActorCtrler.AiCtrler.Target.IsBox && this.player.ActorCtrler.GetDistance2D(this.player.ActorCtrler.AiCtrler.Target) < 5f)
				{
					if (!this.player.ActorCtrler.Skills[this.aoeSkillBtn.SkillIndex].IsCooldown)
					{
						this.player.ActorCtrler.ClearSkillCD(this.aoeSkillBtn.SkillIndex);
					}
					this.player.ActorCtrler.InterruptSkill(0);
					this.status = 10;
					Globals.Instance.ActorMgr.Pause(true);
					this.Step_CastAOESkill();
				}
				break;
			case 2:
				if (!this.player.ActorCtrler.Skills[this.aoeSkillBtn.SkillIndex].IsCooldown || Time.time - this.freshTime > 8f)
				{
					this.SkillTutorialEnd();
				}
				break;
			case 3:
				if (Globals.Instance.ActorMgr.GetBossActor() != null && Globals.Instance.ActorMgr.GetBossActor().gameObject.activeInHierarchy)
				{
					this.status = 10;
					return;
				}
				if (this.mWorldScene == null)
				{
					this.status = 10;
					return;
				}
				if (this.catActor == null)
				{
					if (this.mWorldScene.GetRespawnIndex() > 0)
					{
						this.status = 10;
						return;
					}
					if (this.mWorldScene.GetRespawnIndex() == 0)
					{
						foreach (ActorController current in Globals.Instance.ActorMgr.Actors)
						{
							if (current != null && !current.IsDead && current.monsterInfo != null && current.monsterInfo.ID == Tutorial_1_2.ConcentratedFireMonsterID && current.gameObject.activeInHierarchy)
							{
								this.catActor = current;
							}
						}
					}
				}
				if (this.catActor == null)
				{
					return;
				}
				if (this.player.ActorCtrler.GetDistance2D(this.catActor) < 3.7f)
				{
					this.Step_ConcentratedFire();
					this.aoeSkillBtn.TutorialUsing = true;
					this.singleSkillBtn.TutorialUsing = true;
					this.status = 4;
					Globals.Instance.ActorMgr.Pause(true);
				}
				break;
			case 4:
				if (Globals.Instance.ActorMgr.GetBossActor() != null && Globals.Instance.ActorMgr.GetBossActor().gameObject.activeInHierarchy)
				{
					this.status = 10;
					return;
				}
				if (this.catActor != null && this.catActor.IsDead)
				{
					Globals.Instance.ActorMgr.Pause(false);
					this.status = 10;
					this.hasConcentratedFire = true;
					this.aoeSkillBtn.TutorialUsing = false;
					this.singleSkillBtn.TutorialUsing = false;
					UnityEngine.Object.Destroy(this.guideMask);
				}
				break;
			}
		}
	}

	private void InitParms()
	{
		this.status = 0;
		this.hasCastAOESkill = false;
		this.catActor = null;
		this.mWorldScene = null;
		this.hasConcentratedFire = false;
	}

	private void Step_CastAOESkill()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultFailureScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (this.hasCastAOESkill)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			return;
		}
		GameUIManager.mInstance.ShowPlotDialog(1101, new GUIPlotDialog.FinishCallback(this.CastAOESkill), null);
	}

	private void CastAOESkill()
	{
		if (this.combatMain == null)
		{
			this.combatMain = GameUIManager.mInstance.GetSession<GUICombatMain>();
		}
		if (this.combatMain == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.combatMain.gameObject,
			TargetObj = GameUITools.FindGameObject("skill_btn", this.aoeSkillBtn.gameObject),
			TargetParent = this.combatMain.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			IsRemovePanel = true,
			FreeTutorial = true,
			Tips = Singleton<StringManager>.Instance.GetString("tutorialUseSkill2")
		});
		this.guideAnimation.transform.localPosition = Vector3.zero;
		this.tips.gameObject.SetActive(true);
		this.guideAnimation.SetActive(true);
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		ActorController actorController = null;
		foreach (ActorController current in actors)
		{
			if (current != null && current.ActorType == ActorController.EActorType.EMonster && current.gameObject.activeInHierarchy && !current.IsDead)
			{
				if (actorController == null)
				{
					actorController = current;
				}
				else if (this.player.ActorCtrler.GetDistance2D(actorController) > this.player.ActorCtrler.GetDistance2D(current))
				{
					actorController = current;
				}
			}
		}
		if (actorController == null)
		{
			this.SkillTutorialEnd();
			return;
		}
		this.singleSkillBtn.TutorialUsing = true;
		Vector3 position = Camera.main.WorldToViewportPoint(new Vector3(actorController.transform.position.x, actorController.transform.position.y, actorController.transform.position.z));
		position = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(position);
		TweenPosition component = this.hand.GetComponent<TweenPosition>();
		component.from = this.hand.transform.localPosition;
		this.hand.transform.position = position;
		component.to = this.hand.transform.localPosition;
		component.duration = 3f;
		component.style = UITweener.Style.Loop;
		this.hand.GetComponent<UIWidget>().depth += 5;
		this.freshTime = Time.time;
		this.status = 2;
	}

	private void SkillTutorialEnd()
	{
		this.singleSkillBtn.TutorialUsing = false;
		this.aoeSkillBtn.TutorialUsing = false;
		this.aoeSkillBtn.TutorialCanDragSkill = false;
		this.singleSkillBtn.TutorialUsing = false;
		this.hasCastAOESkill = true;
		this.status = 3;
		UnityEngine.Object.Destroy(this.guideAnimation);
		UnityEngine.Object.Destroy(this.guideMask);
		Globals.Instance.ActorMgr.Pause(false);
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
	}

	private void Step_ConcentratedFire()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultFailureScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (this.hasConcentratedFire)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			return;
		}
		if (this.catActor == null || this.catActor.IsDead)
		{
			return;
		}
		GameUIManager.mInstance.ShowPlotDialog(1012, new GUIPlotDialog.FinishCallback(this.ConcentratedFire), null);
		base.PlaySound("tutorial_023");
	}

	private void ConcentratedFire()
	{
		this.aoeSkillBtn.TutorialUsing = true;
		this.singleSkillBtn.TutorialUsing = true;
		base.CreateGuideMask();
		base.ResetGuideMask();
		GameUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, this.guideMask);
		base.SetHandDirection(TutorialEntity.ETutorialHandDirection.ETHD_Right);
		TweenPosition component = this.hand.GetComponent<TweenPosition>();
		component.enabled = true;
		this.guideMask.SetActive(true);
		this.guideAnimation.SetActive(true);
		this.tips.gameObject.SetActive(true);
		this.tips.text = Singleton<StringManager>.Instance.GetString("tutorialConcentratedFire");
		this.fadeBG.enabled = false;
		this.animationPanel = this.guideAnimation.GetComponent<UIPanel>();
		if (this.animationPanel == null)
		{
			this.animationPanel = this.guideAnimation.AddComponent<UIPanel>();
		}
		this.animationPanel.enabled = true;
		this.animationPanel.depth = 100;
		this.animationPanel.renderQueue = UIPanel.RenderQueue.StartAt;
		this.animationPanel.startingRenderQueue = 4000;
		Vector3 position = Camera.main.WorldToViewportPoint(new Vector3(this.catActor.transform.position.x, this.catActor.transform.position.y + 0.5f, this.catActor.transform.position.z));
		position = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(position);
		position.z = 0f;
		this.guideMask.transform.position = position;
		UIEventListener expr_1B1 = UIEventListener.Get(this.area.gameObject);
		expr_1B1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1B1.onClick, new UIEventListener.VoidDelegate(this.OnConcentratedFireClick));
		base.SetParRQ();
	}

	private void OnConcentratedFireClick(GameObject go)
	{
		UnityEngine.Object.Destroy(this.animationPanel);
		this.animationPanel = null;
		Globals.Instance.ActorMgr.Pause(false);
		this.player.AttackTarget(this.catActor, 0, true);
		UIIngameActorTarget.GetInstance().Init(this.catActor);
		this.hasConcentratedFire = true;
		this.fadeBG.enabled = true;
		this.aoeSkillBtn.TutorialUsing = false;
		this.singleSkillBtn.TutorialUsing = false;
	}

	private void Step_03()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultFailureScene)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_1_2, false, false, false);
			base.Step_VictoryOKBtn(null);
		}
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap || Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			GameUIManager.mInstance.ShowPlotDialog(1016, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), null);
			base.PlaySound("tutorial_024");
		}
	}
}
