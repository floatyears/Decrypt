using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIWizardQueen : AIController
{
	private int status;

	private int skillIndex;

	private int counter;

	private int counter2;

	private bool flag;

	private bool dialog;

	private bool tips;

	private float timer;

	private Vector3 targetPos = Vector3.zero;

	private GameObject go;

	public override void Init()
	{
		base.Init();
		this.FindEnemyDistance = 3f;
		this.actorCtrler.Undead = true;
	}

	private void Update()
	{
		if (this.actorCtrler == null || this.actorCtrler.IsDead || this.Locked)
		{
			return;
		}
		base.UpdateMoveState(Time.deltaTime);
		base.UpdateFindEnemy(Time.deltaTime);
		base.UpdateTarget(Time.deltaTime);
		this.UpdateAttack(Time.deltaTime);
	}

	public override void OnFindEnemy(ActorController enemy)
	{
		base.OnFindEnemy(enemy);
		if (!this.dialog)
		{
			this.dialog = true;
			base.StopAttack();
			this.AllActorStop();
			if (GameUIManager.mInstance.ShowPlotDialog(1002, new GUIPlotDialog.FinishCallback(this.DialogFinish), null))
			{
				Globals.Instance.ActorMgr.Pause(true);
			}
		}
	}

	private void UpdateAtuoAttack(float elapse)
	{
		if (base.Target == null || base.Target.IsDead)
		{
			return;
		}
		if (base.Target.NavAgent.velocity.sqrMagnitude <= 0f && this.actorCtrler.NavAgent.velocity.sqrMagnitude > 0f)
		{
			return;
		}
		float num = this.actorCtrler.GetDistance2D(base.Target);
		if (this.actorCtrler.IsMelee)
		{
			num -= 1f;
		}
		else
		{
			num -= 0.7f;
		}
		if (num > this.AttackDistance)
		{
			return;
		}
		ECastSkillResult eCastSkillResult = this.actorCtrler.TryCastSkill(0, base.Target);
		if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
		{
			this.counter++;
			switch (this.skillIndex)
			{
			case 0:
				if (this.counter >= 1)
				{
					this.status = 1;
					this.skillIndex = 1;
				}
				break;
			case 1:
				if (this.counter >= 1)
				{
					if (this.counter2 == 0)
					{
						this.status = 1;
						this.skillIndex = 2;
						this.tips = false;
						this.counter2++;
					}
					else
					{
						this.status = 1;
						this.skillIndex = 3;
						this.tips = false;
					}
				}
				break;
			case 2:
				if (this.counter >= 1)
				{
					this.status = 1;
					this.skillIndex = 1;
				}
				break;
			case 3:
				if (this.counter >= 1)
				{
					this.status = 1;
					this.skillIndex = 1;
				}
				break;
			}
			return;
		}
	}

	private void UpdateSkill(float elapse)
	{
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"GetActor(0) error"
			});
			return;
		}
		switch (this.skillIndex)
		{
		case 1:
			if (!this.tips)
			{
				this.tips = true;
				this.timer = 0f;
				GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, 54, 3f);
			}
			else
			{
				this.timer += elapse;
				if (this.timer < 0.5f)
				{
					return;
				}
				ECastSkillResult eCastSkillResult = this.actorCtrler.TryCastSkill(this.skillIndex, actor);
				if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
				{
					this.status = 0;
					this.counter = 0;
				}
			}
			break;
		case 2:
			if (!this.tips)
			{
				this.tips = true;
				this.timer = 0f;
				GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, 55, 3f);
			}
			else
			{
				this.timer += elapse;
				if (this.timer < 0.5f)
				{
					return;
				}
				if (!this.flag)
				{
					this.targetPos = actor.transform.position;
					GameObject gameObject = Res.Load<GameObject>("Skill/com/st_048", false);
					if (gameObject == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load error, name = Skill/com/st_048"
						});
						return;
					}
					this.go = (UnityEngine.Object.Instantiate(gameObject, this.targetPos, Quaternion.identity) as GameObject);
					ParticleSystem[] componentsInChildren = this.go.gameObject.GetComponentsInChildren<ParticleSystem>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].loop = true;
					}
					this.flag = true;
					this.timer = 0f;
				}
				else
				{
					this.timer += elapse;
					if (this.timer < 1.5f)
					{
						return;
					}
					UnityEngine.Object.Destroy(this.go);
					ECastSkillResult eCastSkillResult2 = this.actorCtrler.TryCastSkill(this.skillIndex, this.targetPos);
					if (eCastSkillResult2 == ECastSkillResult.ECSR_Sucess || eCastSkillResult2 == ECastSkillResult.ECSR_Cache)
					{
						this.status = 0;
						this.counter = 0;
					}
				}
			}
			break;
		case 3:
			if (!this.tips)
			{
				this.tips = true;
				this.timer = 0f;
				GameUIManager.mInstance.ShowCombatPaopaoTip(this.actorCtrler, 56, 3f);
			}
			else
			{
				this.timer += elapse;
				if (this.timer < 0.5f)
				{
					return;
				}
				this.actorCtrler.RemoveAllBuff();
				this.actorCtrler.HandleAttMod(EAttMod.EAM_Value, 301, 10000, true);
				ECastSkillResult eCastSkillResult3 = this.actorCtrler.TryCastSkill(this.skillIndex, actor);
				base.Invoke("PlayFx", 2.8f);
				if (eCastSkillResult3 == ECastSkillResult.ECSR_Sucess || eCastSkillResult3 == ECastSkillResult.ECSR_Cache)
				{
					this.status = 2;
				}
			}
			break;
		}
	}

	private void PlayFx()
	{
		GameObject gameObject = Res.Load<GameObject>("Skill/com/st_074", false);
		if (gameObject != null)
		{
			GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
			Tools.SetParticleRenderQueue2(gameObject2, 7000);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.AddComponent<UIFx074>().Init();
		}
	}

	private new void UpdateAttack(float elapse)
	{
		if (!this.autoAttack || this.actorCtrler.LockSkillIndex != -1 || this.actorCtrler.SkillCastCache != -1)
		{
			return;
		}
		switch (this.status)
		{
		case 0:
			this.UpdateAtuoAttack(elapse);
			break;
		case 1:
			this.UpdateSkill(elapse);
			break;
		case 2:
			Globals.Instance.SenceMgr.CloseScene();
			GameUIManager.mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
			GameUIManager.mInstance.ClearGobackSession();
			break;
		}
	}

	private void DialogFinish()
	{
		base.StartAttack();
		Globals.Instance.ActorMgr.Pause(false);
		BaseScene curScene = Globals.Instance.ActorMgr.CurScene;
		if (curScene.GetType() == typeof(StartScene))
		{
			StartScene startScene = (StartScene)curScene;
			startScene.Tutorial.GuideSkill();
		}
	}

	private void AllActorStop()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < 5; i++)
		{
			if (actors[i] != null)
			{
				actors[i].StopMove();
			}
		}
	}
}
