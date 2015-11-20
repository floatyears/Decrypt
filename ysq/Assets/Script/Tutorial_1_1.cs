using System;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_1 : TutorialEntity
{
	private int status = -2;

	private PlayerController player;

	private bool hasCastSkill;

	private bool hasConcentratedFire;

	private float freshTime;

	private GameObject moveTarget;

	private ActorController diJingActor;

	private WorldScene mWorldScene;

	private CombatMainSkillButton aoeSkillBtn;

	private CombatMainSkillButton singleSkillBtn;

	private Vector3 targetPos = new Vector3(-5.15f, 0f, -2.54f);

	private int plotIndex;

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
		if (Globals.Instance.Player.GetSceneScore(101001) > 0)
		{
			TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_1_1, true, true, true);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			base.Step_PVEBtn("tutorial28");
			TutorialEntity.SetNextTutorialStep(2, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			base.PlaySound("tutorial_013");
		}
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
				GameUIManager.mInstance.ShowPlotDialog(1006, new GUIPlotDialog.FinishCallback(base.Step_PVEBtn), null);
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
			{
				this.InitParms();
				base.ResetFadeBGArea();
				base.Step_SceneBtn(1, "tutorial1_1");
				base.PlaySound("tutorial_014");
				return;
			}
			if (Globals.Instance.TutorialMgr.CurrentScene is GameUIAdventureReady)
			{
				this.InitParms();
				base.Step_StartSceneBtn("tutorial42");
				base.PlaySound("tutorial_015");
				return;
			}
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, true, false);
			return;
		}
		if (this.hasCastSkill || this.hasConcentratedFire)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUICombatMain)
		{
			this.combatMain = TutorialEntity.ConvertObject2UnityOrPrefab<GUICombatMain>();
		}
		if (this.combatMain == null)
		{
			return;
		}
		if (Globals.Instance.ActorMgr.CurScene is WorldScene)
		{
			this.mWorldScene = (WorldScene)Globals.Instance.ActorMgr.CurScene;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene == null && this.status == -2)
		{
			this.player = Globals.Instance.ActorMgr.PlayerCtrler;
			this.player.TutorialCanMove = false;
			GameUIManager.mInstance.ShowFadeBG(5900, 3000);
			this.status = -1;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameStateTip && this.status == -1)
		{
			GameUIManager.mInstance.HideFadeBG(false);
			Globals.Instance.ActorMgr.Pause(false);
			if (this.player == null)
			{
				this.player = Globals.Instance.ActorMgr.PlayerCtrler;
			}
			this.player.TutorialCanMove = true;
			this.Step_Move();
			this.status = 1;
		}
	}

	private void InitParms()
	{
		this.status = -2;
		this.hasCastSkill = false;
		this.hasConcentratedFire = false;
		this.moveTarget = null;
		this.diJingActor = null;
		this.mWorldScene = null;
	}

	private void Step_Move()
	{
		GameObject gameObject = Res.Load<GameObject>("Skill/com/st_059", false);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, name = Skill/com/st_059"
			});
			return;
		}
		this.singleSkillBtn = GameUITools.FindGameObject("right-bottom/skill1", this.combatMain.gameObject).GetComponent<CombatMainSkillButton>();
		this.aoeSkillBtn = GameUITools.FindGameObject("right-bottom/skill2", this.combatMain.gameObject).GetComponent<CombatMainSkillButton>();
		if (this.player.ActorCtrler.Skills[this.singleSkillBtn.SkillIndex] == null)
		{
			this.singleSkillBtn = null;
		}
		if (this.player.ActorCtrler.Skills[this.aoeSkillBtn.SkillIndex] == null)
		{
			this.aoeSkillBtn = null;
		}
		if (this.singleSkillBtn == null)
		{
			global::Debug.LogError(new object[]
			{
				"skill is null"
			});
			return;
		}
		this.singleSkillBtn.TutorialUsing = true;
		if (this.aoeSkillBtn != null)
		{
			this.aoeSkillBtn.TutorialUsing = true;
		}
		this.moveTarget = (UnityEngine.Object.Instantiate(gameObject, this.targetPos, Quaternion.identity) as GameObject);
		base.CreateGuideMask();
		base.ResetGuideMask();
		GameUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, this.guideMask);
		base.SetHandDirection(TutorialEntity.ETutorialHandDirection.ETHD_RightDown);
		this.guideMask.SetActive(true);
		if (this.player.ControlType == PlayerController.EControlType.ETouch)
		{
			this.tips.text = Singleton<StringManager>.Instance.GetString("tutorialMove3");
		}
		else
		{
			this.tips.text = Singleton<StringManager>.Instance.GetString("tutorialMove2");
		}
		GameObject gameObject2 = GameUITools.FindGameObject("q", this.ui38);
		gameObject2.SetActive(false);
		this.area.gameObject.SetActive(false);
		UIPanel uIPanel = this.guideMask.gameObject.GetComponent<UIPanel>();
		if (uIPanel == null)
		{
			uIPanel = this.guideMask.gameObject.AddComponent<UIPanel>();
		}
		uIPanel.depth = 100;
		uIPanel.renderQueue = UIPanel.RenderQueue.StartAt;
		uIPanel.startingRenderQueue = 4000;
		Vector3 position = Camera.main.WorldToViewportPoint(this.targetPos);
		position = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(position);
		position.z = 0f;
		this.guideMask.transform.position = position;
		this.moveTarget.SetActive(true);
		this.fadeBG.gameObject.SetActive(false);
		TweenPosition component = this.hand.GetComponent<TweenPosition>();
		component.enabled = true;
		this.guideAnimation.SetActive(true);
		this.tips.gameObject.SetActive(true);
		base.PlaySound("tutorial_016");
	}

	private void Update()
	{
		if (this.player != null)
		{
			switch (this.status)
			{
			case 1:
				if (this.player.ActorCtrler.NavAgent.hasPath && this.player.ActorCtrler.NavAgent.velocity.sqrMagnitude > 0f)
				{
					UnityEngine.Object.Destroy(this.guideMask);
					UnityEngine.Object.Destroy(this.moveTarget);
					this.moveTarget = null;
					this.status = 2;
					return;
				}
				if (this.moveTarget != null && CombatHelper.DistanceSquared2D(this.moveTarget.transform.position, this.player.transform.position) < 0.425f)
				{
					UnityEngine.Object.Destroy(this.moveTarget);
					this.moveTarget = null;
					this.status = 2;
					return;
				}
				break;
			case 2:
				if (Globals.Instance.ActorMgr.GetBossActor() != null && Globals.Instance.ActorMgr.GetBossActor().gameObject.activeInHierarchy)
				{
					this.status = 10;
					return;
				}
				if (this.player.ActorCtrler.AiCtrler.Target != null && this.player.ActorCtrler.GetDistance2D(this.player.ActorCtrler.AiCtrler.Target) < 4f)
				{
					this.Step_CastSingleSkill();
					this.status = 3;
					if (this.singleSkillBtn != null)
					{
						this.singleSkillBtn.TutorialUsing = false;
					}
					if (this.aoeSkillBtn != null)
					{
						this.aoeSkillBtn.TutorialUsing = false;
					}
					this.freshTime = Time.time;
				}
				break;
			case 3:
				if (Globals.Instance.ActorMgr.GetBossActor() != null && Globals.Instance.ActorMgr.GetBossActor().gameObject.activeInHierarchy)
				{
					this.status = 10;
					return;
				}
				if (this.diJingActor == null)
				{
					foreach (ActorController current in Globals.Instance.ActorMgr.Actors)
					{
						if (current != null && !current.IsDead && current.monsterInfo != null && current.monsterInfo.ID == 10002 && current.gameObject.activeInHierarchy)
						{
							this.diJingActor = current;
						}
					}
				}
				if (this.diJingActor != null)
				{
					this.hasCastSkill = true;
				}
				if (!this.hasCastSkill && this.player.ActorCtrler.LockSkillIndex > 0)
				{
					this.hasCastSkill = true;
				}
				if (Time.time - this.freshTime > 5f || this.hasCastSkill)
				{
					UnityEngine.Object.Destroy(this.guideMask);
					this.guideAnimation.SetActive(false);
					TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
					this.status = 6;
				}
				break;
			case 6:
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
					this.SkillTutorialEnd();
					return;
				}
				if (this.mWorldScene.GetRespawnIndex() == 1 && this.player.ActorCtrler.AiCtrler.Target != null && !this.player.ActorCtrler.AiCtrler.Target.IsBox && this.player.ActorCtrler.GetDistance2D(this.player.ActorCtrler.AiCtrler.Target) < 5f)
				{
					this.status = 10;
					if (this.aoeSkillBtn != null)
					{
						if (!this.player.ActorCtrler.Skills[this.aoeSkillBtn.SkillIndex].IsCooldown)
						{
							this.player.ActorCtrler.ClearSkillCD(this.aoeSkillBtn.SkillIndex);
						}
						this.player.ActorCtrler.InterruptSkill(0);
						Globals.Instance.ActorMgr.Pause(true);
						this.Step_CastAOESkill();
					}
				}
				break;
			case 7:
				if ((this.aoeSkillBtn != null && !this.player.ActorCtrler.Skills[this.aoeSkillBtn.SkillIndex].IsCooldown) || Time.time - this.freshTime > 8f)
				{
					this.SkillTutorialEnd();
				}
				break;
			}
		}
	}

	private void SkillTutorialEnd()
	{
		if (this.singleSkillBtn != null)
		{
			this.singleSkillBtn.TutorialUsing = false;
		}
		if (this.aoeSkillBtn != null)
		{
			this.aoeSkillBtn.TutorialUsing = false;
			this.aoeSkillBtn.TutorialCanDragSkill = false;
		}
		this.hasConcentratedFire = true;
		this.status = 10;
		UnityEngine.Object.Destroy(this.guideAnimation);
		UnityEngine.Object.Destroy(this.guideMask);
		Globals.Instance.ActorMgr.Pause(false);
		TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
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
		if (this.hasConcentratedFire)
		{
			TutorialEntity.SetNextTutorialStep(3, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			return;
		}
		this.CastAOESkill();
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
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = this.combatMain.gameObject;
		if (this.aoeSkillBtn == null)
		{
			return;
		}
		tutorialInitParams.TargetObj = GameUITools.FindGameObject("skill_btn", this.aoeSkillBtn.gameObject);
		tutorialInitParams.TargetParent = this.combatMain.gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown;
		tutorialInitParams.IsRemovePanel = true;
		tutorialInitParams.FreeTutorial = true;
		tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorialUseSkill2");
		base.InitGuideMask(tutorialInitParams);
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
		if (this.singleSkillBtn != null)
		{
			this.singleSkillBtn.TutorialUsing = true;
		}
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
		this.status = 7;
		base.PlaySound("tutorial_018");
	}

	private void Step_CastSingleSkill()
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
		if (Globals.Instance.TutorialMgr.CurrentScene is GUICombatMain)
		{
			this.combatMain = TutorialEntity.ConvertObject2UnityOrPrefab<GUICombatMain>();
		}
		if (this.combatMain == null)
		{
			return;
		}
		base.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.combatMain.gameObject,
			TargetObj = GameUITools.FindGameObject("skill_btn", this.singleSkillBtn.gameObject),
			TargetParent = this.combatMain.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			IsRemovePanel = true,
			FreeTutorial = true,
			Tips = Singleton<StringManager>.Instance.GetString("tutorialUseSkill1")
		});
		this.guideAnimation.transform.localPosition = Vector3.zero;
		this.tips.gameObject.SetActive(true);
		this.guideAnimation.SetActive(true);
		base.PlaySound("tutorial_017");
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
		if (this.diJingActor == null || this.diJingActor.IsDead)
		{
			return;
		}
		GameUIManager.mInstance.ShowPlotDialog(1012, new GUIPlotDialog.FinishCallback(this.ConcentratedFire), null);
	}

	private void ConcentratedFire()
	{
		if (this.aoeSkillBtn != null)
		{
			this.aoeSkillBtn.TutorialUsing = true;
		}
		if (this.singleSkillBtn != null)
		{
			this.singleSkillBtn.TutorialUsing = true;
		}
		base.CreateGuideMask();
		base.ResetGuideMask();
		GameUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, this.guideMask);
		base.SetHandDirection(TutorialEntity.ETutorialHandDirection.ETHD_RightDown);
		TweenPosition component = this.hand.GetComponent<TweenPosition>();
		component.enabled = true;
		this.guideMask.SetActive(true);
		this.guideAnimation.SetActive(true);
		this.tips.gameObject.SetActive(true);
		this.tips.text = Singleton<StringManager>.Instance.GetString("tutorialConcentratedFire");
		this.fadeBG.enabled = false;
		Vector3 position = Camera.main.WorldToViewportPoint(new Vector3(this.diJingActor.transform.position.x, this.diJingActor.transform.position.y + 0.5f, this.diJingActor.transform.position.z));
		position = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(position);
		position.z = 0f;
		this.guideMask.transform.position = position;
		UIEventListener expr_16B = UIEventListener.Get(this.area.gameObject);
		expr_16B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_16B.onClick, new UIEventListener.VoidDelegate(this.OnConcentratedFireClick));
		base.SetParRQ();
	}

	private void OnConcentratedFireClick(GameObject go)
	{
		UnityEngine.Object.Destroy(this.animationPanel);
		this.animationPanel = null;
		Globals.Instance.ActorMgr.Pause(false);
		this.player.AttackTarget(this.diJingActor, 0, true);
		UIIngameActorTarget.GetInstance().Init(this.diJingActor);
		this.hasConcentratedFire = true;
		this.fadeBG.enabled = true;
		if (this.aoeSkillBtn != null)
		{
			this.aoeSkillBtn.TutorialUsing = false;
		}
		if (this.singleSkillBtn != null)
		{
			this.singleSkillBtn.TutorialUsing = false;
		}
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
			TutorialEntity.SetNextTutorialStep(4, TutorialManager.ETutorialNum.Tutorial_1_1, false, false, false);
			base.Step_VictoryOKBtn(null);
		}
	}

	private void Step_04()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap || Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			GameUIManager.mInstance.ShowPlotDialog(1013, new GUIPlotDialog.FinishCallback(base.StartNextTutorial), new GUIPlotDialog.VoidCallback(this.OnPlotClickEvent));
		}
	}

	private void OnPlotClickEvent()
	{
		this.plotIndex++;
		int num = this.plotIndex;
		if (num == 2)
		{
			base.PlaySound("tutorial_020");
		}
	}
}
