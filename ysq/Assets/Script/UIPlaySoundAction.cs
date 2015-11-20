using System;
using UnityEngine;

[AddComponentMenu("Game/Action/UIPlaySoundAction")]
public sealed class UIPlaySoundAction : MonoBehaviour
{
	private UIActorController uiActor;

	public float DelayTime;

	public string SoundName;

	public float Volume = 1f;

	public void OnInit(UIActorController actor)
	{
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"actor == null"
			});
			return;
		}
		this.uiActor = actor;
		if (!string.IsNullOrEmpty(this.SoundName))
		{
			if (this.DelayTime > 0f)
			{
				base.Invoke("DoPlaySound", this.DelayTime);
			}
			else
			{
				this.DoPlaySound();
			}
		}
	}

	private void DoPlaySound()
	{
		if (this.uiActor != null && !string.IsNullOrEmpty(this.SoundName))
		{
			Globals.Instance.EffectSoundMgr.PlayVoice(this.SoundName, this.Volume, this.uiActor.transform.position);
		}
	}
}
