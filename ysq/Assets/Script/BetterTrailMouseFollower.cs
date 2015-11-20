using PigeonCoopToolkit.Effects.Trails;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pigeon Coop Toolkit/Controller/Better Trail Mouse Follower")]
public class BetterTrailMouseFollower : MonoBehaviour
{
	public List<TrailRenderer_Base> Trails;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			this.Trails.ForEach(delegate(TrailRenderer_Base a)
			{
				if (a != null)
				{
					a.Emit = true;
				}
			});
			base.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 0.01f));
		}
		else
		{
			this.Trails.ForEach(delegate(TrailRenderer_Base a)
			{
				if (a != null)
				{
					a.Emit = false;
				}
			});
		}
	}
}
