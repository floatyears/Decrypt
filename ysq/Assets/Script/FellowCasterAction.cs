using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Fellow Caster Action")]
public sealed class FellowCasterAction : InstantiateBase
{
	public bool UpdateInterrupt;

	private void Awake()
	{
		this.checkInterrupt = this.UpdateInterrupt;
	}

	protected override void DoAction()
	{
		base.DoInstantiate(base.variables.skillCaster);
	}

	protected override void UpdateAction(float elapse)
	{
		base.UpdateAction(elapse);
		if (this.go != null && base.variables != null && base.variables.skillCaster != null)
		{
			Vector3 vector = base.variables.skillCaster.transform.position;
			if (!this.BaseOnFeet)
			{
				vector.y += ((BoxCollider)base.variables.skillCaster.collider).size.y;
			}
			vector.y += this.YOffset;
			vector += base.variables.skillCaster.transform.forward * this.ForwardOffset;
			this.go.transform.position = vector;
		}
	}
}
