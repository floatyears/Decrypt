using System;
using UnityEngine;

[AddComponentMenu("Game/Action/UILockingAction")]
public sealed class UILockingAction : MonoBehaviour
{
	private UIActorController uiActor;

	public float LockTime;

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
	}

	private void Start()
	{
		if (this.uiActor == null)
		{
			global::Debug.Log(new object[]
			{
				"uiActor == null"
			});
			return;
		}
		if (this.LockTime > 0f)
		{
			this.Lock();
			base.Invoke("Unlock", this.LockTime);
		}
	}

	private void Lock()
	{
		if (this.uiActor != null)
		{
			this.uiActor.LockAction();
		}
	}

	private void Unlock()
	{
		if (this.uiActor != null)
		{
			this.uiActor.UnlockAction();
		}
	}
}
