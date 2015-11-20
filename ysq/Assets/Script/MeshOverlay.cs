using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshOverlay : MonoBehaviour
{
	public Material overlayMaterial;

	public Vector2 scrollUVSpeed = new Vector2(0.3f, 0.3f);

	public Vector3 meshScaleOffest = Vector3.zero;

	private GameObject overlayObject;

	private List<MeshRenderer> overlayRenders = new List<MeshRenderer>();

	private void Start()
	{
		this.overlayObject = new GameObject(base.gameObject.name + "(Overlay)");
		this.overlayObject.transform.parent = base.gameObject.transform;
		this.overlayObject.layer = base.gameObject.layer;
		this.overlayObject.transform.localPosition = Vector3.zero;
		this.overlayObject.transform.localRotation = Quaternion.identity;
		this.overlayObject.transform.localScale = Vector3.one + this.meshScaleOffest * 0.001f;
		MeshFilter[] components = base.GetComponents<MeshFilter>();
		for (int i = 0; i < components.Length; i++)
		{
			MeshFilter meshFilter = components[i];
			MeshFilter meshFilter2 = this.overlayObject.AddComponent<MeshFilter>();
			meshFilter2.mesh = meshFilter.sharedMesh;
			MeshRenderer meshRenderer = this.overlayObject.AddComponent<MeshRenderer>();
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;
			meshRenderer.material = this.overlayMaterial;
			this.overlayRenders.Add(meshRenderer);
		}
	}

	private void Update()
	{
		for (int i = 0; i < this.overlayRenders.Count; i++)
		{
			if (!(this.overlayRenders[i] == null) && !(this.overlayRenders[i].material == null))
			{
				Vector2 vector = this.overlayRenders[i].material.mainTextureOffset;
				vector += this.scrollUVSpeed * Time.deltaTime;
				if (vector.x > 1f)
				{
					vector.x = 0f;
				}
				else if (vector.x < 0f)
				{
					vector.x = 1f;
				}
				if (vector.y > 1f)
				{
					vector.y = 0f;
				}
				else if (vector.x < 0f)
				{
					vector.y = 1f;
				}
				this.overlayRenders[i].material.mainTextureOffset = vector;
			}
		}
	}
}
