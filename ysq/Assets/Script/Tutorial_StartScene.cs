using System;
using UnityEngine;

public class Tutorial_StartScene : TutorialEntity
{
	private int status;

	private float timer;

	private ActorController player;

	private GameObject moveTarget;

	private int skillIndex;

	private bool flag;

	public void GuideMove()
	{
		if (this.status != 0)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("status must be 0, status = {0}", this.status)
			});
			return;
		}
		this.player = Globals.Instance.ActorMgr.GetActor(0);
		if (this.player == null)
		{
			global::Debug.LogError(new object[]
			{
				"ActorMgr has no player"
			});
			return;
		}
		this.CreateGuidePosition(new Vector3(4.494694f, -1.204232f, 3.529153f), "tutorialMove", false, TutorialEntity.ETutorialHandDirection.ETHD_RightDown);
		this.status = 1;
		this.timer = 0f;
	}

	public void GuideSkill()
	{
		this.status = 2;
		this.timer = 0f;
	}

	private void CreateGuidePosition(Vector3 position, string key, bool force = false, TutorialEntity.ETutorialHandDirection handDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown)
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
		this.moveTarget = (UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity) as GameObject);
		position.y += 0.5f;
		base.CreateGuideMask();
		if (this.maskPanel == null)
		{
			this.maskPanel = this.guideMask.AddComponent<UIPanel>();
			this.maskPanel.enabled = true;
			this.maskPanel.depth = 2500;
			this.maskPanel.renderQueue = UIPanel.RenderQueue.StartAt;
			this.maskPanel.startingRenderQueue = 5000;
		}
		GameUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, this.guideMask);
		base.SetHandDirection(handDirection);
		TweenPosition component = this.hand.GetComponent<TweenPosition>();
		component.enabled = true;
		this.guideMask.SetActive(true);
		this.guideAnimation.SetActive(true);
		this.tips.gameObject.SetActive(true);
		this.tips.text = Singleton<StringManager>.Instance.GetString(key);
		GameObject gameObject2 = GameUITools.FindGameObject("q", this.ui38);
		gameObject2.SetActive(false);
		if (!force)
		{
			this.fadeBG.gameObject.SetActive(false);
			this.area.gameObject.SetActive(false);
		}
		Vector3 position2 = Camera.main.WorldToViewportPoint(position);
		position2 = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(position2);
		position2.z = 0f;
		this.guideMask.transform.position = position2;
	}

	protected void OnMoveClicked(GameObject obj)
	{
		Globals.Instance.ActorMgr.Pause(false);
		if (this.moveTarget != null)
		{
			Globals.Instance.ActorMgr.PlayerCtrler.MoveToPosition(this.moveTarget.transform.position);
			UnityEngine.Object.Destroy(this.moveTarget, 2f);
		}
	}

	private void Update()
	{
		switch (this.status)
		{
		case 1:
			if (!this.player.AiCtrler.EnableAI && this.player.NavAgent.hasPath && this.player.NavAgent.velocity.sqrMagnitude > 0f)
			{
				this.player.AiCtrler.EnableAI = true;
				UnityEngine.Object.Destroy(this.guideMask);
				this.flag = true;
				this.timer = 0f;
			}
			if (this.moveTarget != null && CombatHelper.DistanceSquared2D(this.moveTarget.transform.position, this.player.transform.position) < 0.425f)
			{
				UnityEngine.Object.Destroy(this.moveTarget);
				this.moveTarget = null;
				return;
			}
			if (this.flag)
			{
				this.timer += Time.deltaTime;
				if (this.timer > 2f)
				{
					UnityEngine.Object.Destroy(this.moveTarget);
					this.moveTarget = null;
					return;
				}
			}
			break;
		case 2:
		{
			this.timer += Time.deltaTime;
			if (this.timer < 2f || this.player.AiCtrler.Target == null)
			{
				return;
			}
			if (this.player.GetDistance2D(this.player.AiCtrler.Target) > this.player.AiCtrler.AttackDistance + 0.7f)
			{
				return;
			}
			bool flag = true;
			for (int i = 1; i < this.player.Skills.Length; i++)
			{
				if (this.player.Skills[i] != null && this.player.Skills[i].IsCooldown)
				{
					flag = false;
					this.skillIndex = i;
					break;
				}
			}
			if (flag)
			{
				return;
			}
			GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
			if (session == null)
			{
				return;
			}
			base.InitGuideMask(new TutorialInitParams
			{
				MaskParent = session.gameObject,
				TargetName = string.Format("right-bottom/skill{0}/skill_btn", this.skillIndex),
				TargetParent = session.gameObject,
				HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
				SlightTutorial = true,
				Tips = Singleton<StringManager>.Instance.GetString("tutorialUseSkill1")
			});
			this.guideAnimation.SetActive(true);
			this.status = 3;
			this.timer = 0f;
			break;
		}
		case 3:
			this.timer += Time.deltaTime;
			if (this.timer > 5f || (this.skillIndex != 0 && this.skillIndex < this.player.Skills.Length && this.player.Skills[this.skillIndex] != null && !this.player.Skills[this.skillIndex].IsCooldown))
			{
				if (this.guideMask != null)
				{
					UnityEngine.Object.Destroy(this.guideMask);
				}
				if (this.guideAnimation != null)
				{
					UnityEngine.Object.Destroy(this.guideAnimation);
				}
				this.status = 4;
				this.timer = 0f;
				this.skillIndex = 0;
				return;
			}
			break;
		case 4:
			this.timer += Time.deltaTime;
			if (this.timer > 0.2f)
			{
				this.timer = 0f;
				if (this.moveTarget != null && CombatHelper.DistanceSquared2D(this.moveTarget.transform.position, this.player.transform.position) < 0.425f)
				{
					UnityEngine.Object.Destroy(this.moveTarget);
					this.moveTarget = null;
					this.status = 0;
				}
			}
			break;
		}
	}
}
