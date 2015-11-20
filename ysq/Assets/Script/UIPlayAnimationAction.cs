using System;
using UnityEngine;

[AddComponentMenu("Game/Action/UIPlayAnimationAction")]
public sealed class UIPlayAnimationAction : MonoBehaviour
{
	private UIActorController uiActor;

	public string Anim = string.Empty;

	public float DelayPlayAnimtion;

	public float LifeTime = -1f;

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
		if (!string.IsNullOrEmpty(this.Anim))
		{
			if (this.DelayPlayAnimtion > 0f)
			{
				base.Invoke("PlayAnimation", this.DelayPlayAnimtion);
			}
			else
			{
				this.PlayAnimation();
			}
		}
	}

	private void PlayAnimation()
	{
		if (this.uiActor == null || this.uiActor.AnimCtrl == null || string.IsNullOrEmpty(this.Anim))
		{
			return;
		}
		if (this.uiActor.AnimCtrl[this.Anim] == null)
		{
			return;
		}
		this.uiActor.AnimCtrl.CrossFade(this.Anim);
		if (this.LifeTime > 0f)
		{
			base.Invoke("StopAnimation", this.LifeTime);
		}
		else
		{
			this.uiActor.AnimCtrl.CrossFadeQueued("std");
		}
	}

	private void StopAnimation()
	{
		if (this.uiActor == null || this.uiActor.AnimCtrl == null)
		{
			return;
		}
		this.uiActor.AnimCtrl.CrossFade("std");
	}
}
