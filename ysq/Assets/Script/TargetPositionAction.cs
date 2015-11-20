using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Target Position Action")]
public sealed class TargetPositionAction : InstantiateBase
{
	private void Awake()
	{
		this.checkInterrupt = false;
	}

	protected override void DoAction()
	{
		if (this.prefab == null)
		{
			return;
		}
		Vector3 targetPosition = base.variables.targetPosition;
		targetPosition.y += this.YOffset;
		base.DoInstantiate(targetPosition, base.transform.rotation);
	}
}
