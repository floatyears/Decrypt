using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Set Material")]
public sealed class SetMaterial : ActionBase
{
	public Material material;

	public int materialIndex;

	private GameObject activeObj;

	protected override void DoAction()
	{
		this.activeObj = base.variables.skillCaster.gameObject;
		if (this.activeObj == null)
		{
			base.Finish();
			return;
		}
		if (this.activeObj.renderer == null)
		{
			base.Finish();
			return;
		}
		if (this.materialIndex == 0)
		{
			this.activeObj.renderer.material = this.material;
		}
		else if (this.activeObj.renderer.materials.Length > this.materialIndex)
		{
			Material[] materials = this.activeObj.renderer.materials;
			materials[this.materialIndex] = this.material;
			this.activeObj.renderer.materials = materials;
		}
		base.Finish();
	}
}
