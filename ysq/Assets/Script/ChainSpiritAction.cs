using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/ChainSpiritAction")]
public class ChainSpiritAction : ActionBase
{
	public class TargetData
	{
		public ActorController actor;

		public float timer;
	}

	public class LightningData
	{
		public ChainLightning lightning;

		public float lifeTime;

		public int status;
	}

	private enum EStatus
	{
		ES_None,
		ES_Effect,
		ES_Finish,
		ES_Destroy
	}

	public GameObject MissilePrefab;

	public float MissileDeleteDelay = 0.2f;

	public float MissileInterval = 0.2f;

	public float YOffset;

	public bool Spine = true;

	public GameObject explodePrefab;

	public float explodeLifeTime = 1f;

	public float NextInterval = 0.2f;

	public float MinLiftTime = 0.5f;

	public int JumpCount = 5;

	public int JumpMaxDist = 5;

	private List<ChainSpiritAction.TargetData> oldActorList = new List<ChainSpiritAction.TargetData>();

	private ActorController nextActor;

	private float startTimestamp;

	private List<ChainSpiritAction.LightningData> missileList = new List<ChainSpiritAction.LightningData>();

	private ChainSpiritAction.EStatus status;

	private float timer;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.status = ChainSpiritAction.EStatus.ES_Effect;
		this.startTimestamp = this.MinLiftTime;
		this.nextActor = base.variables.skillTarget;
		Vector3 vector = Vector3.zero;
		base.variables.skillCaster.transform.position = new Vector3(vector.x, vector.y + this.YOffset,vector.z);
		if (this.nextActor == null)
		{
			this.nextActor = base.variables.skillCaster;
		}
	}

	protected override void UpdateAction(float elapse)
	{
		this.startTimestamp -= elapse;
		if (this.status == ChainSpiritAction.EStatus.ES_Destroy)
		{
			if (this.startTimestamp <= 0f)
			{
				base.Finish();
			}
			return;
		}
		this.UpdateLighting(elapse);
		if (this.status == ChainSpiritAction.EStatus.ES_None)
		{
			this.status = ChainSpiritAction.EStatus.ES_Finish;
			this.timer = 0f;
			return;
		}
		for (int i = 0; i < this.oldActorList.Count; i++)
		{
			if (this.oldActorList[i] != null && this.oldActorList[i].actor != null && !this.oldActorList[i].actor.IsDead)
			{
				this.oldActorList[i].timer -= elapse;
				if (this.oldActorList[i].timer <= 0f)
				{
					this.DoEffectOnTarget(this.oldActorList[i].actor);
				}
			}
		}
		if (this.timer > 0f)
		{
			this.timer -= elapse;
		}
		ChainSpiritAction.EStatus eStatus = this.status;
		if (eStatus != ChainSpiritAction.EStatus.ES_Effect)
		{
			if (eStatus == ChainSpiritAction.EStatus.ES_Finish)
			{
				if (this.timer <= 0f)
				{
					this.oldActorList.Clear();
					for (int j = 0; j < this.missileList.Count; j++)
					{
						if (this.missileList[j] != null && this.missileList[j].lightning != null)
						{
							UnityEngine.Object.Destroy(this.missileList[j].lightning.gameObject);
						}
					}
					this.missileList.Clear();
					this.status = ChainSpiritAction.EStatus.ES_Destroy;
				}
			}
		}
		else
		{
			if (base.variables == null || base.variables.skillCaster == null)
			{
				this.status = ChainSpiritAction.EStatus.ES_Finish;
				this.timer = 0f;
				return;
			}
			if (base.variables.skillCaster.IsDead)
			{
				this.DoFinish();
				return;
			}
			if (this.timer <= 0f)
			{
				if (this.nextActor != null)
				{
					ChainSpiritAction.TargetData targetData = new ChainSpiritAction.TargetData();
					targetData.actor = this.nextActor;
					targetData.timer = 0.1f;
					this.oldActorList.Add(targetData);
				}
				this.nextActor = this.GetNextTarget();
				if (this.nextActor == null)
				{
					this.DoFinish();
					return;
				}
				this.timer = 0.1f;
			}
		}
	}

	private void UpdateLighting(float elapse)
	{
		if (base.variables == null || base.variables.skillCaster == null)
		{
			return;
		}
		if (this.status != ChainSpiritAction.EStatus.ES_Effect && this.status != ChainSpiritAction.EStatus.ES_Finish)
		{
			return;
		}
		Vector3 headPos = base.variables.skillCaster.transform.position;
		headPos.y += this.YOffset;
		Vector3 vector = Vector3.zero;
		int num = 0;
		for (int i = 0; i < this.oldActorList.Count; i++)
		{
			if (this.oldActorList[i] != null && this.oldActorList[i].actor != null)
			{
				if (this.Spine && this.oldActorList[i].actor.SpineTransform != null)
				{
					vector = this.oldActorList[i].actor.SpineTransform.position;
				}
				else
				{
					vector = this.oldActorList[i].actor.transform.position;
					vector.y += this.YOffset;
				}
				this.SetChainLightning(num, headPos, vector, elapse);
				num++;
				headPos = vector;
			}
		}
	}

	private void DoEffectOnTarget(ActorController actor)
	{
		if (this.explodePrefab != null)
		{
			Vector3 position = actor.transform.position;
			if (this.Spine && actor.SpineTransform != null)
			{
				position = actor.SpineTransform.position;
			}
			else
			{
				position.y += this.YOffset;
			}
			GameObject obj = UnityEngine.Object.Instantiate(this.explodePrefab, position, actor.transform.rotation) as GameObject;
			UnityEngine.Object.Destroy(obj, this.explodeLifeTime);
		}
		if (base.variables != null && base.variables.skillCaster != null)
		{
			base.variables.skillCaster.OnMissileHit(base.variables.skillInfo, actor, actor.transform.position, 0);
		}
	}

	private bool HasTarget(ActorController actor)
	{
		for (int i = 0; i < this.oldActorList.Count; i++)
		{
			if (this.oldActorList[i] != null && this.oldActorList[i].actor == actor)
			{
				return true;
			}
		}
		return false;
	}

	private ActorController GetNextTarget()
	{
		if (this.oldActorList.Count >= this.JumpCount)
		{
			return null;
		}
		if (base.variables == null || base.variables.IsInterrupted())
		{
			return null;
		}
		float num = 3.40282347E+38f;
		ActorController result = null;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (!(actorController == null) && !(actorController == base.variables.skillCaster) && !actorController.IsDead && !actorController.IsImmunity && base.variables.skillCaster.IsFriendlyTo(actorController))
			{
				float num2 = Vector3.Distance(actorController.transform.position, this.nextActor.transform.position);
				if (num2 < (float)this.JumpMaxDist && num2 < num && !this.HasTarget(actorController))
				{
					num = num2;
					result = actorController;
				}
			}
		}
		return result;
	}

	private void DoFinish()
	{
		if (this.status != ChainSpiritAction.EStatus.ES_Effect)
		{
			return;
		}
		this.status = ChainSpiritAction.EStatus.ES_Finish;
		this.timer = this.MissileDeleteDelay * 2f + this.MissileInterval;
		this.nextActor = null;
	}

	private ChainLightning CreateChainLightning()
	{
		if (!(this.MissilePrefab != null))
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(this.MissilePrefab, base.transform.position, base.transform.rotation) as GameObject;
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Instantiate MissilePrefab object error!"
			});
			return null;
		}
		ChainLightning component = gameObject.GetComponent<ChainLightning>();
		if (component == null)
		{
			global::Debug.LogError(new object[]
			{
				"Get ChainLightning Error"
			});
			return null;
		}
		return component;
	}

	private void SetChainLightning(int index, Vector3 headPos, Vector3 tailPos, float elapse)
	{
		while (index >= this.missileList.Count)
		{
			ChainSpiritAction.LightningData lightningData = new ChainSpiritAction.LightningData();
			lightningData.lightning = this.CreateChainLightning();
			lightningData.lifeTime = 0f;
			lightningData.status = 0;
			this.missileList.Add(lightningData);
		}
		if (this.missileList[index] != null && this.missileList[index].lightning != null)
		{
			this.missileList[index].lightning.StartPos = headPos;
			this.missileList[index].lightning.EndPos = tailPos;
			this.missileList[index].lifeTime += elapse;
			if (this.missileList[index].lifeTime > this.MissileInterval)
			{
				UnityEngine.Object.Destroy(this.missileList[index].lightning.gameObject);
			}
		}
	}

	private void OnSpawned()
	{
		this.oldActorList.Clear();
		this.missileList.Clear();
		this.nextActor = null;
		this.startTimestamp = 0f;
		this.status = ChainSpiritAction.EStatus.ES_None;
		this.timer = 0f;
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		for (int i = 0; i < this.missileList.Count; i++)
		{
			if (this.missileList[i] != null && this.missileList[i].lightning != null)
			{
				UnityEngine.Object.Destroy(this.missileList[i].lightning.gameObject);
			}
		}
		this.oldActorList.Clear();
		this.missileList.Clear();
		this.nextActor = null;
		this.startTimestamp = 0f;
		this.status = ChainSpiritAction.EStatus.ES_None;
		this.timer = 0f;
	}
}
