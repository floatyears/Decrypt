using System;
using UnityEngine;

[AddComponentMenu("Game/Action/UIInstantiateAction")]
public sealed class UIInstantiateAction : MonoBehaviour
{
	private UIActorController uiActor;

	private GameObject go;

	public float DelayTime;

	public GameObject Prefab;

	public float LifeTime = -1f;

	public float YOffset;

	public float ForwardOffset;

	public bool BaseOnFeet = true;

	public bool AttachActor;

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
		if (this.Prefab == null || this.uiActor == null || !this.uiActor.gameObject.activeInHierarchy)
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
		if (!this.AttachActor)
		{
			this.go.transform.parent = this.uiActor.transform.parent;
			this.go.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
		}
		NGUITools.SetLayer(this.go, this.go.layer);
		Vector3 vector = this.go.transform.localPosition;
		if (!this.BaseOnFeet)
		{
			vector.y += ((BoxCollider)this.uiActor.collider).size.y;
		}
		vector.y += this.YOffset;
		vector += this.uiActor.transform.forward * this.ForwardOffset * this.uiActor.transform.localScale.z;
		this.go.transform.localPosition = vector;
		int value = 4210;
		Transform transform = this.uiActor.transform.FindChild("Empty");
		if (transform != null)
		{
			UIWidget component = transform.GetComponent<UIWidget>();
			if (component != null && component.panel)
			{
				value = component.panel.startingRenderQueue + 10;
			}
		}
		Tools.SetParticleRenderQueue(this.go, value, 1f);
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
