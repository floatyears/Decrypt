using System;
using UnityEngine;

[AddComponentMenu("Game/Action/KillSelfAction")]
public class KillSelfAction : ActionBase
{
	protected override void DoAction()
	{
		if (base.variables.skillCaster == null || base.variables.skillCaster.IsDead)
		{
			base.Finish();
			return;
		}
		base.variables.skillCaster.DoDamage(base.variables.skillCaster.MaxHP * 2L, null, false);
	}
}
