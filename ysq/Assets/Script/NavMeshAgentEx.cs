using System;
using UnityEngine;

public sealed class NavMeshAgentEx : MonoBehaviour
{
	private NavMeshAgent navAgent;

	private NavMeshPath navPath;

	private Vector3[] navCorners;

	private int navPathIndex;

	private Vector3 targetPos;

	private Vector3 navVelocity;

	public bool hasPath
	{
		get;
		private set;
	}

	public Vector3 destination
	{
		get
		{
			return this.targetPos;
		}
		set
		{
			this.navAgent.CalculatePath(value, this.navPath);
			this.navCorners = this.navPath.corners;
			if (this.navCorners.Length == 0)
			{
				float[] array = new float[]
				{
					-2f,
					2f
				};
				for (int i = 0; i < 4; i++)
				{
					this.navAgent.CalculatePath(value + new Vector3(array[i / 2], 0f, array[i % 2]), this.navPath);
					this.navCorners = this.navPath.corners;
					if (this.navCorners.Length > 0)
					{
						break;
					}
				}
			}
			if (this.navCorners.Length == 0)
			{
				this.targetPos = base.transform.position;
				this.Stop();
				return;
			}
			if (this.navCorners.Length == 1)
			{
				this.targetPos = this.navCorners[0];
				this.position = this.targetPos;
				this.Stop();
				return;
			}
			this.targetPos = this.navCorners[this.navCorners.Length - 1];
			this.hasPath = true;
			this.navPathIndex = 0;
		}
	}

	public Vector3 position
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			this.navAgent.nextPosition = value;
			base.transform.position = this.navAgent.nextPosition;
		}
	}

	public float speed
	{
		get
		{
			return this.navAgent.speed;
		}
		set
		{
			this.navAgent.speed = value;
		}
	}

	public int walkableMask
	{
		get
		{
			return this.navAgent.walkableMask;
		}
		set
		{
			this.navAgent.walkableMask = value;
		}
	}

	public Vector3 velocity
	{
		get
		{
			return this.navVelocity;
		}
	}

	public Vector3 desiredVelocity
	{
		get
		{
			return this.navVelocity;
		}
	}

	public Vector3 steeringTarget
	{
		get
		{
			if (this.hasPath)
			{
				return this.navCorners[this.navPathIndex + 1];
			}
			return this.targetPos;
		}
	}

	public void Stop()
	{
		if (!this.hasPath)
		{
			return;
		}
		this.hasPath = false;
		this.navPath.ClearCorners();
		this.navCorners = null;
		this.navVelocity = Vector3.zero;
	}

	public bool CalculatePath(Vector3 targetPosition, NavMeshPath path)
	{
		return this.navAgent.CalculatePath(targetPosition, path);
	}

	public bool Raycast(Vector3 targetPosition, out NavMeshHit hit)
	{
		return this.navAgent.Raycast(targetPosition, out hit);
	}

	public void Warp(Vector3 newPosition)
	{
		this.navAgent.Warp(newPosition);
		this.Stop();
	}

	private void Awake()
	{
		this.navAgent = Tools.GetSafeComponent<NavMeshAgent>(base.gameObject);
		this.navAgent.radius = 0.2f;
		this.navAgent.speed = 2.8f;
		this.navAgent.acceleration = 40960f;
		this.navAgent.autoRepath = false;
		this.navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		this.navAgent.updateRotation = false;
		this.navAgent.updatePosition = false;
		this.navPath = new NavMeshPath();
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.hasPath)
		{
			return;
		}
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 a = new Vector2(this.navCorners[this.navCorners.Length - 1].x, this.navCorners[this.navCorners.Length - 1].z);
		Vector2 a2;
		Vector2 a3;
		while (true)
		{
			Vector2 b = new Vector2(this.navCorners[this.navPathIndex].x, this.navCorners[this.navPathIndex].z);
			Vector2 vector2 = new Vector2(this.navCorners[this.navPathIndex + 1].x, this.navCorners[this.navPathIndex + 1].z);
			Vector2 vector3 = vector2 - b;
			a2 = vector3;
			float num = this.navAgent.speed;
			float num2 = 2f * this.navAgent.radius;
			float magnitude = (a - vector).magnitude;
			float sqrMagnitude = this.navVelocity.sqrMagnitude;
			float magnitude2 = this.navVelocity.magnitude;
			float num3 = magnitude2 * deltaTime;
			if (magnitude < num3 + 0.01f && (magnitude < num2 || num2 < num3))
			{
				break;
			}
			if (magnitude < num2 && magnitude * this.navAgent.speed < num2 * magnitude2)
			{
				num = magnitude2 - sqrMagnitude / (2f * magnitude) * deltaTime;
			}
			float d = 1f / a2.magnitude * num;
			a2 *= d;
			a3 = vector + a2 * deltaTime;
			if (vector3.sqrMagnitude >= (a3 - b).sqrMagnitude)
			{
				goto Block_6;
			}
			this.navPathIndex++;
			if (this.navPathIndex >= this.navCorners.Length - 1)
			{
				goto Block_7;
			}
			this.navVelocity = new Vector3(a2.x, 0f, a2.y);
			vector = vector2;
			deltaTime = (a3 - vector2).magnitude / this.navAgent.speed;
		}
		this.position = this.targetPos;
		this.Stop();
		return;
		Block_6:
		this.position = new Vector3(a3.x, base.transform.position.y, a3.y);
		this.navVelocity = new Vector3(a2.x, 0f, a2.y);
		return;
		Block_7:
		this.position = this.targetPos;
		this.Stop();
	}
}
