using System;
using UnityEngine;

[AddComponentMenu("Game/PlayerTest")]
public sealed class PlayerTest : MonoBehaviour
{
	public LayerMask targetRaycastLayerMask = 1 << LayerDefine.MonsterLayer | 1 << LayerDefine.CollisionLayer;

	private float farPlane = 100f;

	public string PoseStd = "std";

	public string PoseIdle = "idle";

	public string PoseRun = "run";

	public bool Free;

	private GameObject attackArea;

	private CameraManager cameraMgr;

	private NavMeshAgent navAgent;

	private void Start()
	{
		if (!string.IsNullOrEmpty(this.PoseStd) && base.gameObject.animation)
		{
			base.gameObject.animation.CrossFade(this.PoseStd);
		}
		this.cameraMgr = Tools.GetSafeComponent<CameraManager>(base.gameObject);
		this.navAgent = Tools.GetSafeComponent<NavMeshAgent>(base.gameObject);
		this.navAgent.radius = 0.2f;
		this.navAgent.speed = 2.8f;
		this.navAgent.acceleration = 40960f;
		this.navAgent.autoRepath = false;
		this.navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		this.navAgent.updateRotation = false;
		GameObject gameObject = Res.Load<GameObject>("Skill/st_002", false);
		if (gameObject != null)
		{
			this.attackArea = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			if (this.attackArea != null)
			{
				this.attackArea.transform.parent = base.transform;
				this.attackArea.transform.localPosition = Vector3.zero;
				this.attackArea.transform.localRotation = Quaternion.identity;
				this.attackArea.transform.localScale = Vector3.one;
				this.attackArea.layer = base.gameObject.layer;
			}
		}
	}

	private void Update()
	{
		if (this.navAgent != null)
		{
			if (this.navAgent.hasPath && this.navAgent.velocity.sqrMagnitude > 0f)
			{
				Quaternion to = Quaternion.LookRotation(this.navAgent.velocity);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, Time.deltaTime * 15f);
			}
			if (this.navAgent.desiredVelocity.sqrMagnitude < 0.1f)
			{
				base.gameObject.animation.CrossFade(this.PoseStd);
				base.gameObject.animation.wrapMode = WrapMode.Loop;
			}
			else
			{
				base.gameObject.animation.CrossFade(this.PoseRun);
				base.gameObject.animation.wrapMode = WrapMode.Loop;
			}
		}
	}

	private void LateUpdate()
	{
		this.cameraMgr.NormalUpdate(base.gameObject, null, false);
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, this.farPlane, this.targetRaycastLayerMask))
			{
				if (raycastHit.collider.gameObject.layer != 12)
				{
					this.navAgent.destination = raycastHit.point;
				}
			}
		}
	}
}
