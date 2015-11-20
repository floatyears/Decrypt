using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Delay Instantiate On Target")]
public sealed class DelayInstantiateOnTarget : InstantiateBase
{
	public string AttachSocketName = string.Empty;

	public bool OnlyUseSocketPosition;

	private void Awake()
	{
		this.checkInterrupt = false;
	}

	protected override void DoAction()
	{
		if (base.variables.skillInfo.CastTargetType == 3)
		{
			Vector3 targetPosition = base.variables.targetPosition;
			targetPosition.y += this.YOffset;
			base.DoInstantiate(targetPosition, Quaternion.identity);
		}
		else if (base.variables.skillTarget != null)
		{
			base.DoInstantiate(base.variables.skillTarget, this.AttachSocketName, this.OnlyUseSocketPosition);
		}
	}
}
