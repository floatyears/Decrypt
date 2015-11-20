using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Play Sound Action")]
public class PlaySoundAction : ActionBase
{
	public string soundName;

	public bool Gender;

	public float Volume = 1f;

	protected override void DoAction()
	{
		if (!string.IsNullOrEmpty(this.soundName))
		{
			float num = this.Volume;
			switch (base.variables.skillCaster.ActorType)
			{
			case ActorController.EActorType.EPet:
				num *= 0.75f;
				break;
			case ActorController.EActorType.EMonster:
				num *= 0.5f;
				break;
			}
			base.variables.skillCaster.PlaySound(this.soundName, num, true, this.Gender);
		}
		base.Finish();
	}
}
