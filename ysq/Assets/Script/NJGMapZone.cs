using NJG;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NJG MiniMap/Map Zone"), ExecuteInEditMode, RequireComponent(typeof(SphereCollider))]
public class NJGMapZone : MonoBehaviour
{
	public static List<NJGMapZone> list = new List<NJGMapZone>();

	public static int id = 0;

	public string triggerTag = "Player";

	public string zone;

	public string level;

	public int colliderRadius = 10;

	public int mId;

	private SphereCollider mCollider;

	private NJGMapBase map;

	public Color color
	{
		get
		{
			return (!(this.map == null)) ? this.map.GetZoneColor(this.level, this.zone) : Color.white;
		}
	}

	private void Awake()
	{
		this.map = NJGMapBase.instance;
		NJGMapZone.id++;
		this.mId = NJGMapZone.id;
		this.mCollider = base.GetComponent<SphereCollider>();
		this.mCollider.isTrigger = true;
		this.mCollider.radius = (float)this.colliderRadius;
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag(this.triggerTag) && this.map != null)
		{
			this.map.zoneColor = this.color;
			this.map.worldName = this.zone;
		}
	}

	private void OnEnable()
	{
		NJGMapZone.list.Add(this);
	}

	private void OnDisable()
	{
		NJGMapZone.list.Remove(this);
	}

	private void OnDestroy()
	{
		NJGMapZone.id--;
	}
}
