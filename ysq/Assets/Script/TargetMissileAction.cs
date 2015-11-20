using System;
using UnityEngine;

[AddComponentMenu("Game/Action/TargetMissileAction")]
public class TargetMissileAction : MissileAction
{
	public enum CurveType
	{
		Normal,
		Bezier
	}

	public bool Spine = true;

	public Vector3 Speed = Vector3.one;

	public Vector3 Accel = Vector3.zero;

	public Vector3 SpeedMax = new Vector3(1000f, 1000f, 1000f);

	private float damping = 3f;

	public float parabolaAngle;

	private GameObject target;

	private Vector3 startPosition = Vector3.zero;

	private Vector3 targetPosition = Vector3.zero;

	private float distanceToTarget;

	private Vector3 speed;

	private float timeStamp;

	public TargetMissileAction.CurveType curveType;

	public Vector3 BezierOffest = new Vector3(0f, 5f, 0f);

	public float BezierDutationMin = 0.25f;

	protected override void DoAction()
	{
		base.CreateMissile();
		if (this.missile == null)
		{
			return;
		}
		if (base.variables.skillTarget != null)
		{
			if (this.Spine && base.variables.skillTarget.SpineTransform != null)
			{
				this.target = base.variables.skillTarget.SpineTransform.gameObject;
			}
			else
			{
				this.target = base.variables.skillTarget.gameObject;
			}
		}
		if (this.target == null)
		{
			this.targetPosition = base.variables.skillCaster.transform.position + base.variables.skillCaster.transform.forward * (base.variables.skillCaster.AiCtrler.AttackDistance + 1.5f);
		}
		else
		{
			this.targetPosition = this.target.transform.position;
		}
		this.startPosition = this.missile.transform.position;
		if (this.curveType == TargetMissileAction.CurveType.Normal)
		{
			this.distanceToTarget = CombatHelper.Distance2D(this.startPosition, this.targetPosition);
		}
		else
		{
			this.distanceToTarget = Vector3.Distance(this.startPosition, this.targetPosition);
		}
		this.speed = this.Speed;
		this.damping = 3f;
		this.timeStamp = Time.time;
		this.BezierDutationMin = ((this.speed.magnitude > 0f) ? Mathf.Max(this.BezierDutationMin, this.distanceToTarget / this.speed.magnitude) : 0f);
	}

	protected override void UpdateAction(float elapse)
	{
		if (this.missile == null)
		{
			base.Finish();
			return;
		}
		if (this.curveType == TargetMissileAction.CurveType.Bezier)
		{
			float num = Time.time - this.timeStamp;
			if (num > this.BezierDutationMin)
			{
				this.OnTouchTarget(this.targetPosition);
				return;
			}
			if (this.target != null && !base.variables.skillTarget.IsDead)
			{
				this.targetPosition = this.target.transform.position;
			}
			Bezier bezier = new Bezier(this.startPosition, base.transform.right * this.BezierOffest.x + Vector3.up * this.BezierOffest.y + base.transform.forward * this.BezierOffest.z, Vector3.zero, this.targetPosition);
			float num2 = num / this.BezierDutationMin;
			num2 *= num2;
			Vector3 pointAtTime = bezier.GetPointAtTime(num2);
			this.missile.transform.position = pointAtTime;
			Vector3 forward = this.targetPosition - pointAtTime;
			if (forward.sqrMagnitude > 0.1f)
			{
				Quaternion to = Quaternion.LookRotation(forward);
				this.damping += 0.9f;
				this.missile.transform.rotation = Quaternion.Slerp(this.missile.transform.rotation, to, Time.deltaTime * this.damping);
			}
		}
		else
		{
			if (this.target != null && !base.variables.skillTarget.IsDead)
			{
				this.targetPosition = this.target.transform.position;
			}
			Vector3 vector = this.Accel * Time.deltaTime;
			this.speed.x = Mathf.Clamp(this.speed.x + vector.x, 1f, this.SpeedMax.x);
			this.speed.y = Mathf.Clamp(this.speed.y + vector.y, 1f, this.SpeedMax.y);
			this.speed.z = Mathf.Clamp(this.speed.z + vector.z, 1f, this.SpeedMax.z);
			if (this.parabolaAngle <= 0f)
			{
				Vector3 vector2 = this.targetPosition - this.missile.transform.position;
				if (vector2 != Vector3.zero)
				{
					Quaternion to2 = Quaternion.LookRotation(vector2);
					this.damping += 0.9f;
					this.missile.transform.rotation = Quaternion.Slerp(this.missile.transform.rotation, to2, Time.deltaTime * this.damping);
				}
				Vector3 vector3 = this.speed * Time.deltaTime;
				this.missile.transform.position += vector2.normalized * vector3.x;
				this.missile.transform.position += vector2.normalized * vector3.y;
				this.missile.transform.position += vector2.normalized * vector3.z;
				if (vector2.magnitude < vector3.magnitude)
				{
					this.OnTouchTarget(this.targetPosition);
					return;
				}
			}
			else
			{
				this.missile.transform.LookAt(this.targetPosition);
				float num3 = this.Speed.x * this.Speed.y * this.Speed.z * Time.deltaTime;
				float num4 = CombatHelper.Distance2D(this.missile.transform.position, this.targetPosition);
				float num5 = (this.distanceToTarget > 0f) ? (Mathf.Min(1f, num4 / this.distanceToTarget) * this.parabolaAngle) : 0f;
				num5 = Mathf.Clamp(-num5, -this.parabolaAngle, this.parabolaAngle);
				this.missile.transform.rotation = this.missile.transform.rotation * Quaternion.Euler(num5, 0f, 0f);
				this.missile.transform.Translate(Vector3.forward * num3);
				if (num4 < num3)
				{
					this.OnTouchTarget(this.targetPosition);
					return;
				}
			}
		}
	}

	protected override void OnPlay()
	{
		base.OnPlay();
		if (this.missile == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = this.missile.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Play();
		}
		Animator[] componentsInChildren2 = this.missile.GetComponentsInChildren<Animator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].StopPlayback();
		}
	}

	protected override void OnPause()
	{
		base.OnPause();
		if (this.missile == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = this.missile.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Pause();
		}
		Animator[] componentsInChildren2 = this.missile.GetComponentsInChildren<Animator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].StartPlayback();
		}
	}
}
