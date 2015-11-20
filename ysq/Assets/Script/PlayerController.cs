using Att;
using NtUniSdk.Unity3d;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Character/PlayerController")]
public sealed class PlayerController : MonoBehaviour
{
	public enum EControlType
	{
		ETouch,
		EJoystick
	}

	public LayerMask targetRaycastLayerMask = 1 << LayerDefine.MonsterLayer | 1 << LayerDefine.CollisionLayer;

	private float farPlane = 100f;

	private ActorController actorCtrler;

	public bool Locked;

	private GoalBase curGoal;

	private GoalMoveToPosition mvoe2PosGoal;

	private GoalAttackTarget attackTargetGoal;

	private GameObject attackArea;

	public PlayerController.EControlType ControlType;

	private float touchDownTimeStamp;

	private bool touchMove = true;

	private GameObject touchDownEffect;

	private ParticleSystem[] touchDownPS;

	private GameObject touchMoveEffect;

	private UIVirtualPad joystick;

	private Vector3 touchBeginPosition = Vector3.zero;

	private GameObject targetDirection;

	private ActorController target;

	private float targetTimer;

	public bool TutorialCanMove = true;

	public ActorController ActorCtrler
	{
		get
		{
			return this.actorCtrler;
		}
	}

	public void Init()
	{
		this.actorCtrler = base.GetComponent<ActorController>();
		if (GameCache.Data.Joystick)
		{
			this.ControlType = PlayerController.EControlType.EJoystick;
		}
		GameObject gameObject = Res.Load<GameObject>("Skill/st_002", false);
		if (gameObject != null)
		{
			this.attackArea = Tools.AddChild(base.gameObject, gameObject);
		}
		gameObject = Res.Load<GameObject>("Skill/st_001", false);
		if (gameObject != null)
		{
			this.touchDownEffect = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			if (this.touchDownEffect != null)
			{
				this.touchDownPS = this.touchDownEffect.GetComponentsInChildren<ParticleSystem>();
				this.touchDownEffect.SetActive(false);
			}
		}
		gameObject = Res.Load<GameObject>("Skill/st_001a", false);
		if (gameObject != null)
		{
			this.touchMoveEffect = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			if (this.touchMoveEffect != null)
			{
				this.touchMoveEffect.SetActive(false);
			}
		}
		gameObject = Res.Load<GameObject>("Skill/com/st_060", false);
		if (gameObject != null)
		{
			this.targetDirection = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			this.targetDirection.SetActive(false);
		}
		Globals.Instance.CameraMgr.InitForGameScene(this.actorCtrler.transform.position, this.actorCtrler.AiCtrler.EnableAI);
	}

	private void OnDestroy()
	{
		if (this.touchDownEffect != null)
		{
			UnityEngine.Object.Destroy(this.touchDownEffect);
		}
		if (this.touchMoveEffect != null)
		{
			UnityEngine.Object.Destroy(this.touchMoveEffect);
		}
		if (this.targetDirection != null)
		{
			UnityEngine.Object.Destroy(this.targetDirection);
		}
	}

	private void BeginJoystick(Vector3 inputPosition)
	{
		if (this.actorCtrler == null || UICamera.isOverUI)
		{
			this.touchMove = false;
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(inputPosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, this.farPlane, this.targetRaycastLayerMask) && raycastHit.collider.gameObject.layer == LayerDefine.MonsterLayer)
		{
			ActorController component = raycastHit.collider.gameObject.GetComponent<ActorController>();
			if (component != null && !component.IsDead)
			{
				this.AttackTarget(component, 0, true);
				this.touchMove = false;
				UIIngameActorTarget.GetInstance().Init(component);
				return;
			}
		}
		if (Input.mousePosition.x >= (float)Screen.width * 0.5f || Input.mousePosition.y >= (float)Screen.height * 0.5f)
		{
			this.touchMove = false;
			return;
		}
		this.touchBeginPosition = inputPosition;
		this.joystick.EnableDPad(this.touchBeginPosition);
		this.actorCtrler.AiCtrler.Locked = true;
	}

	private void MoveJoystick(Vector3 inputPosition)
	{
		if (!this.touchMove)
		{
			return;
		}
		Vector3 vector = inputPosition - this.touchBeginPosition;
		float num = (!GameUIManager.mInstance) ? 960f : 720f;
		float num2 = (float)Screen.width / num;
		if (vector.magnitude >= 60f * num2)
		{
			this.touchBeginPosition = inputPosition - vector.normalized * 60f * num2;
			this.joystick.SetDPadInitDirection(this.touchBeginPosition);
		}
		this.joystick.SetDPadDirection(inputPosition);
		this.actorCtrler.AiCtrler.Locked = true;
		if (this.actorCtrler.CanMove(true, false))
		{
			if (this.curGoal != null)
			{
				this.curGoal.OnInterrupt();
				this.curGoal = null;
			}
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
			Vector3 targetPos = base.transform.position - vector2.normalized * 2f;
			this.actorCtrler.InterruptSkill(0);
			this.actorCtrler.AnimationCtrler.StopAnimation();
			this.actorCtrler.StartMove(targetPos);
		}
	}

	private void EndJoystick()
	{
		if (!this.touchMove)
		{
			this.touchMove = true;
			return;
		}
		this.actorCtrler.AiCtrler.Locked = false;
		this.actorCtrler.StopMove();
		this.joystick.NeutralDPad();
	}

	private void TouchDown(Vector3 inputPosition)
	{
		if (this.actorCtrler == null || UICamera.isOverUI)
		{
			this.touchMove = false;
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(inputPosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, this.farPlane, this.targetRaycastLayerMask))
		{
			if (raycastHit.collider.gameObject.layer == LayerDefine.MonsterLayer)
			{
				ActorController component = raycastHit.collider.gameObject.GetComponent<ActorController>();
				if (component != null && !component.IsDead)
				{
					this.AttackTarget(component, 0, true);
					this.touchMove = false;
					UIIngameActorTarget.GetInstance().Init(component);
				}
			}
			else
			{
				this.touchDownTimeStamp = Time.time;
				this.MoveToPosition(raycastHit.point);
			}
		}
	}

	private void TouchMove(Vector3 inputPosition)
	{
		if (!this.touchMove || !this.TutorialCanMove)
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(inputPosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, this.farPlane, 1 << LayerDefine.CollisionLayer))
		{
			if (this.curGoal != null && this.curGoal == this.mvoe2PosGoal)
			{
				this.mvoe2PosGoal.SetTouchMovePosition(raycastHit.point);
			}
			else
			{
				this.MoveToPosition(raycastHit.point);
			}
			if (this.touchMoveEffect != null && this.touchDownEffect != null)
			{
				this.touchDownEffect.transform.position = raycastHit.point;
				if (Time.time - this.touchDownTimeStamp > 0.3f)
				{
					this.touchDownEffect.SetActive(false);
					this.touchMoveEffect.SetActive(true);
					this.touchMoveEffect.transform.position = raycastHit.point;
				}
			}
		}
	}

	private void TouchUp()
	{
		if (!this.touchMove)
		{
			this.touchMove = true;
			return;
		}
		if (this.touchMoveEffect != null)
		{
			this.touchMoveEffect.SetActive(false);
		}
		if (this.curGoal != null && this.curGoal == this.mvoe2PosGoal)
		{
			this.mvoe2PosGoal.SetTouchUp(!this.touchDownEffect.activeInHierarchy);
		}
	}

	private void InputProcess()
	{
		if (!this.ProcessTouch())
		{
			this.ProcessGamePad();
		}
	}

	private void ProcessSimulater()
	{
		PlayerController.EControlType controlType = this.ControlType;
		if (controlType != PlayerController.EControlType.ETouch)
		{
			if (controlType == PlayerController.EControlType.EJoystick)
			{
				this.joystick = GameUIManager.mInstance.GetDPad();
				if (this.joystick == null)
				{
					return;
				}
				if (Input.GetMouseButtonDown(0))
				{
					this.BeginJoystick(Input.mousePosition);
				}
				if (Input.GetMouseButton(0))
				{
					this.MoveJoystick(Input.mousePosition);
				}
				if (Input.GetMouseButtonUp(0))
				{
					this.EndJoystick();
				}
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.TouchDown(Input.mousePosition);
			}
			if (Input.GetMouseButton(0))
			{
				this.TouchMove(Input.mousePosition);
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.TouchUp();
			}
		}
	}

	private bool ProcessTouch()
	{
		if (Input.touchCount <= 0)
		{
			return false;
		}
		Touch touch = Input.GetTouch(0);
		PlayerController.EControlType controlType = this.ControlType;
		if (controlType != PlayerController.EControlType.ETouch)
		{
			if (controlType == PlayerController.EControlType.EJoystick)
			{
				this.joystick = GameUIManager.mInstance.GetDPad();
				if (this.joystick == null)
				{
					return true;
				}
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.BeginJoystick(touch.position);
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					this.MoveJoystick(touch.position);
					break;
				case TouchPhase.Ended:
					this.EndJoystick();
					break;
				}
			}
		}
		else
		{
			switch (touch.phase)
			{
			case TouchPhase.Began:
				this.TouchDown(touch.position);
				break;
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
				this.TouchMove(touch.position);
				break;
			case TouchPhase.Ended:
				this.TouchUp();
				break;
			}
		}
		return true;
	}

	private void ProcessGamePad()
	{
		if (!GamePadMgr.isConnnected)
		{
			return;
		}
		PlayerController.EControlType controlType = this.ControlType;
		if (controlType != PlayerController.EControlType.ETouch)
		{
			if (controlType == PlayerController.EControlType.EJoystick)
			{
				this.joystick = GameUIManager.mInstance.GetDPad();
				if (this.joystick == null)
				{
					return;
				}
				if (GamePadMgr.GetKeyButtonDown())
				{
					this.BeginJoystick(this.joystick.GetDPadScreenPos());
				}
				if (GamePadMgr.GetKeyButton())
				{
					Vector3 vector = new Vector3(-GamePadMgr.moveX, -GamePadMgr.moveY, 0f);
					Vector3 a = (vector.magnitude <= 1f) ? vector : vector.normalized;
					vector = this.joystick.GetDPadScreenPos() - a * 40f;
					this.MoveJoystick(vector);
				}
				if (GamePadMgr.GetKeyButtonUp())
				{
					this.EndJoystick();
				}
			}
		}
		else
		{
			Vector3 inputPosition = new Vector3(GamePadMgr.moveX, -GamePadMgr.moveY, 0f);
			Vector3 normalized = inputPosition.normalized;
			Vector3 position = base.transform.position - normalized;
			inputPosition = Camera.main.WorldToScreenPoint(position);
			if (GamePadMgr.GetKeyButtonDown())
			{
				this.TouchDown(inputPosition);
			}
			if (GamePadMgr.GetKeyButton())
			{
				this.TouchMove(inputPosition);
			}
			if (GamePadMgr.GetKeyButtonUp())
			{
				this.TouchUp();
			}
		}
	}

	public void OnUpdate(float deltaTime)
	{
		Globals.Instance.CameraMgr.NormalUpdate(base.gameObject, this.actorCtrler, false);
	}

	private void LateUpdate()
	{
		if (this.Locked)
		{
			return;
		}
		if (!this.actorCtrler.IsDead)
		{
			this.InputProcess();
		}
		if (this.curGoal != null && this.curGoal.Update(Time.deltaTime))
		{
			this.curGoal = null;
		}
		this.UpdateTargetDirection();
	}

	public void SetControlLocked(bool locked)
	{
		if (locked)
		{
			if (this.curGoal != null)
			{
				this.curGoal.OnInterrupt();
				this.curGoal = null;
			}
			this.touchDownEffect.SetActive(false);
			this.touchMoveEffect.SetActive(false);
		}
		this.Locked = locked;
	}

	public void SetAttackArea(bool active)
	{
		if (this.attackArea != null)
		{
			this.attackArea.SetActive(active);
		}
	}

	public void ShowTouchDownEffect(bool active, Vector3 position)
	{
		if (this.touchDownEffect != null)
		{
			this.touchDownEffect.SetActive(active);
			if (active)
			{
				this.touchDownEffect.transform.position = position;
				for (int i = 0; i < this.touchDownPS.Length; i++)
				{
					this.touchDownPS[i].Simulate(0f, false, true);
					this.touchDownPS[i].Play();
				}
			}
		}
	}

	public void MoveToPosition(Vector3 targetPos)
	{
		if (this.curGoal != null)
		{
			this.curGoal.OnInterrupt();
			this.curGoal = null;
		}
		if (this.mvoe2PosGoal == null)
		{
			this.mvoe2PosGoal = new GoalMoveToPosition(this.actorCtrler, this);
		}
		if (!this.mvoe2PosGoal.SetTargetPosition(targetPos, 0.2f))
		{
			this.curGoal = this.mvoe2PosGoal;
		}
	}

	public void AttackTarget(ActorController target, int skillIndex, bool focusTarget = true)
	{
		if (this.curGoal != null)
		{
			this.curGoal.OnInterrupt();
			this.curGoal = null;
		}
		if (this.attackTargetGoal == null)
		{
			this.attackTargetGoal = new GoalAttackTarget(this.actorCtrler);
		}
		if (!this.attackTargetGoal.SetAttackTarget(target, skillIndex, focusTarget))
		{
			this.curGoal = this.attackTargetGoal;
		}
	}

	public void CastSkill(int skillIndex)
	{
		if (this.Locked || skillIndex >= this.actorCtrler.Skills.Length || skillIndex < 0)
		{
			return;
		}
		if (this.actorCtrler.Skills[skillIndex] == null || this.actorCtrler.Skills[skillIndex].Info == null)
		{
			return;
		}
		switch (this.actorCtrler.Skills[skillIndex].Info.CastTargetType)
		{
		case 0:
			this.actorCtrler.TryCastSkill(skillIndex, null);
			break;
		case 1:
		case 3:
		{
			ActorController actorController = this.actorCtrler.AiCtrler.Target;
			if (actorController == null || actorController.IsDead)
			{
				float num = 3.40282347E+38f;
				List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
				for (int i = 0; i < actors.Count; i++)
				{
					ActorController actorController2 = actors[i];
					if (actorController2 && !(actorController2.IsDead | !this.actorCtrler.IsHostileTo(actorController2)))
					{
						float distance2D = this.actorCtrler.GetDistance2D(actorController2);
						if (distance2D <= num)
						{
							num = distance2D;
							actorController = actorController2;
						}
					}
				}
			}
			if (this.actorCtrler.Skills[skillIndex].Info.CastTargetType == 3)
			{
				this.actorCtrler.AiCtrler.CacheSkillFlag();
			}
			if (this.actorCtrler.Skills[skillIndex].Effect == 2)
			{
				ActorController actorController3 = this.actorCtrler.AiCtrler.FindHealTarget();
				if (actorController3 != null)
				{
					actorController = actorController3;
				}
			}
			if (actorController != null)
			{
				this.AttackTarget(actorController, skillIndex, false);
			}
			break;
		}
		case 2:
		{
			ActorController x = (!this.actorCtrler.IsDead) ? this.actorCtrler : null;
			float num2 = 3.40282347E+38f;
			List<ActorController> actors2 = Globals.Instance.ActorMgr.Actors;
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorController actorController4 = actors2[j];
				if (actorController4 && !(actorController4 == this.actorCtrler) && !(actorController4.IsDead | !this.actorCtrler.IsFriendlyTo(actorController4)))
				{
					float distance2D2 = this.actorCtrler.GetDistance2D(actorController4);
					if (distance2D2 <= this.actorCtrler.Skills[skillIndex].Info.MaxRange)
					{
						if (distance2D2 <= num2)
						{
							num2 = distance2D2;
							x = actorController4;
						}
					}
				}
			}
			if (x != null)
			{
				this.actorCtrler.TryCastSkill(skillIndex, x);
			}
			break;
		}
		}
	}

	public void CastSkill(int skillIndex, Vector3 targetPos)
	{
		if (this.Locked || skillIndex >= this.actorCtrler.Skills.Length || skillIndex < 0)
		{
			return;
		}
		if (this.actorCtrler.Skills[skillIndex] == null || this.actorCtrler.Skills[skillIndex].Info == null)
		{
			return;
		}
		ECastTargetType castTargetType = (ECastTargetType)this.actorCtrler.Skills[skillIndex].Info.CastTargetType;
		if (castTargetType == ECastTargetType.ECTT_Point)
		{
			this.actorCtrler.TryCastSkill(skillIndex, targetPos);
		}
	}

	public bool ShouldShowSkillCache(int skillIndex)
	{
		if (this.ActorCtrler.SkillCastCache == skillIndex)
		{
			return true;
		}
		GoalAttackTarget goalAttackTarget = this.curGoal as GoalAttackTarget;
		return goalAttackTarget != null && goalAttackTarget.skillIndex == skillIndex;
	}

	private void UpdateTargetDirection()
	{
		if (this.target == null || this.target.IsDead)
		{
			this.target = this.actorCtrler.AiCtrler.Target;
			if (this.target == null || this.target.IsDead)
			{
				this.targetTimer += Time.deltaTime;
				if (this.targetTimer > 1f)
				{
					this.targetTimer = 0f;
					this.target = AIController.FindMinDistEnemy(this.actorCtrler, 3.40282347E+38f);
				}
			}
		}
		if (this.target == null || this.target.IsDead)
		{
			if (this.targetDirection.activeInHierarchy)
			{
				this.targetDirection.SetActive(false);
			}
		}
		else
		{
			float num = this.actorCtrler.GetDistance2D(this.target) - 1f;
			if (!this.actorCtrler.IsDead && num > this.actorCtrler.AiCtrler.AttackDistance)
			{
				if (!this.targetDirection.activeInHierarchy)
				{
					this.targetDirection.SetActive(true);
				}
				Vector3 forward = this.target.transform.position - base.transform.position;
				forward.y = 0f;
				Vector3 position = base.transform.position + forward.normalized * 2f;
				this.targetDirection.transform.position = position;
				this.targetDirection.transform.rotation = Quaternion.LookRotation(forward);
			}
			else if (this.targetDirection.activeInHierarchy)
			{
				this.targetDirection.SetActive(false);
			}
		}
	}

	public void HideTargetDirection()
	{
		if (this.targetDirection.activeInHierarchy)
		{
			this.targetDirection.SetActive(false);
		}
	}
}
