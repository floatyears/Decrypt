using System;
using UnityEngine;

[AddComponentMenu("Game/Action/UIInstantiateSocketAction")]
public sealed class UIInstantiateSocketAction : MonoBehaviour
{
	private UIActorController uiActor;

	private GameObject go;

	public float DelayTime;

	public GameObject Prefab;

	public float LifeTime = -1f;

	public string SocketName = string.Empty;

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
		if (this.Prefab != null)
		{
			if (this.DelayTime > 0f)
			{
				base.Invoke("DoInstantiate", this.DelayTime);
			}
			else
			{
				this.DoInstantiate();
			}
		}
	}

	private void DoInstantiate()
	{
		if (this.Prefab == null || this.uiActor == null)
		{
			return;
		}
		this.go = NGUITools.AddChild(this.uiActor.gameObject, this.Prefab);
		if (this.go == null)
		{
			global::Debug.LogError(new object[]
			{
				"Instantiate game object error : " + this.Prefab.name
			});
			return;
		}
		if (string.IsNullOrEmpty(this.SocketName))
		{
			global::Debug.LogError(new object[]
			{
				"SocketName is NullOrEmpty"
			});
			return;
		}
		GameObject gameObject = ObjectUtil.FindChildObject(this.uiActor.gameObject, this.SocketName);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Can not find socket : {0}", this.SocketName)
			});
			return;
		}
		Transform transform = this.go.transform;
		transform.parent = gameObject.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		NGUITools.SetLayer(this.go, this.go.layer);
		Tools.SetParticleRenderQueue(this.go, 4000, 1f);
		if (this.LifeTime > 0f)
		{
			UnityEngine.Object.Destroy(this.go, this.LifeTime);
		}
	}

	private void OnDestroy()
	{
		if (this.LifeTime <= 0f)
		{
			UnityEngine.Object.Destroy(this.go);
		}
	}
}
