using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Delay Instantiate")]
public sealed class DelayInstantiate : InstantiateBase
{
	public string AttachSocketName = string.Empty;

	public bool OnlyUseSocketPosition;

	protected override void DoAction()
	{
		base.DoInstantiate(base.variables.skillCaster, this.AttachSocketName, this.OnlyUseSocketPosition);
	}
}
