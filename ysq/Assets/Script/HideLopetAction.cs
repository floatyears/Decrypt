using System;
using UnityEngine;

[AddComponentMenu("Game/Action/HideLopetAction")]
public class HideLopetAction : ActionBase
{
	public float Duration = 1f;

	protected override void DoAction()
	{
		float timer = this.Duration / base.variables.skillCaster.AttackSpeed;
		Globals.Instance.ActorMgr.HideLopet(base.variables.skillCaster.FactionType == ActorController.EFactionType.EBlue, timer);
		base.Finish();
	}
}
