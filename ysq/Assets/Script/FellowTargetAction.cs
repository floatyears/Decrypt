using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Fellow Target Action")]
public sealed class FellowTargetAction : InstantiateBase
{
	private void Awake()
	{
		this.checkInterrupt = false;
	}

	protected override void DoAction()
	{
		if (base.variables.skillTarget != null)
		{
			base.DoInstantiate(base.variables.skillTarget);
		}
	}

	protected override void UpdateAction(float elapse)
	{
		base.UpdateAction(elapse);
		if (this.go != null && base.variables != null && base.variables.skillTarget != null)
		{
			Vector3 vector = base.variables.skillTarget.transform.position;
			if (!this.BaseOnFeet)
			{
				vector.y += ((BoxCollider)base.variables.skillTarget.collider).size.y;
			}
			vector.y += this.YOffset;
			vector += base.variables.skillTarget.transform.forward * this.ForwardOffset;
			this.go.transform.position = vector;
		}
	}
}
