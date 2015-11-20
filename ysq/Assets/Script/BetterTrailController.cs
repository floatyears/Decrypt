using PigeonCoopToolkit.Effects.Trails;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pigeon Coop Toolkit/Controller/Better Trail Controller")]
public class BetterTrailController : MonoBehaviour
{
	public bool EmitBySpeed = true;

	public List<TrailRenderer_Base> Trails;

	private ActorController acc;

	private void Start()
	{
		this.acc = base.GetComponent<ActorController>();
		if (this.acc == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				this.acc = parent.GetComponent<ActorController>();
				if (this.acc != null)
				{
					break;
				}
				parent = parent.parent;
			}
		}
	}

	private void Update()
	{
		if (this.acc == null)
		{
			return;
		}
		if (this.EmitBySpeed)
		{
			if (Mathf.Abs(this.acc.NavAgent.velocity.sqrMagnitude) > 0.1f)
			{
				this.Trails.ForEach(delegate(TrailRenderer_Base trail)
				{
					if (trail != null)
					{
						trail.Emit = true;
					}
				});
			}
			else
			{
				this.Trails.ForEach(delegate(TrailRenderer_Base trail)
				{
					if (trail != null)
					{
						trail.Emit = false;
					}
				});
			}
		}
	}
}
