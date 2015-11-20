using System;
using UnityEngine;

[AddComponentMenu("Game/Action/ChangeModelAction")]
public sealed class ChangeModelAction : ActionBase
{
	public GameObject ModelPrefab;

	public float MaxDuration = 10f;

	private float timer;

	private GameObject model;

	protected override void DoAction()
	{
		if (base.variables.skillCaster != null && this.ModelPrefab != null)
		{
			this.model = (UnityEngine.Object.Instantiate(this.ModelPrefab) as GameObject);
			this.model.layer = base.variables.skillCaster.gameObject.layer;
			this.model.transform.position = base.variables.skillCaster.transform.position;
			this.model.transform.rotation = base.variables.skillCaster.transform.rotation;
			base.variables.skillCaster.ActionScale = base.variables.skillCaster.ActionScale * 0.0001f;
		}
		this.timer = this.MaxDuration;
	}

	protected override void UpdateAction(float elapse)
	{
		this.timer -= elapse;
		if (base.variables.skillCaster != null && this.model != null)
		{
			this.model.transform.position = base.variables.skillCaster.transform.position;
			this.model.transform.rotation = base.variables.skillCaster.transform.rotation;
		}
		if (this.timer <= 0f)
		{
			if (base.variables.skillCaster != null && this.model != null)
			{
				base.variables.skillCaster.ActionScale = base.variables.skillCaster.ActionScale * 10000f;
				UnityEngine.Object.Destroy(this.model);
				this.model = null;
			}
			base.Finish();
		}
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		if (base.variables != null && base.variables.skillCaster != null && this.model != null)
		{
			base.variables.skillCaster.ActionScale = base.variables.skillCaster.ActionScale * 10000f;
			UnityEngine.Object.Destroy(this.model);
			this.model = null;
		}
	}
}
