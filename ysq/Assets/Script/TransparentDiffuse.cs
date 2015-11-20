using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Camera/Transparent Diffuse")]
public sealed class TransparentDiffuse : MonoBehaviour
{
	private class TransparentRenderItem
	{
		public List<Shader> shader = new List<Shader>();

		public List<Color> color = new List<Color>();

		public int frame;
	}

	public LayerMask RaycastLayerMask = 1 << LayerDefine.TransparentLayer;

	public float Distance = 100f;

	public float Duration = 0.5f;

	private int frame;

	private float timerStamp;

	private Dictionary<Renderer, TransparentDiffuse.TransparentRenderItem> items = new Dictionary<Renderer, TransparentDiffuse.TransparentRenderItem>();

	private List<Renderer> removes = new List<Renderer>();

	private Shader transparentShader;

	private void OnStart()
	{
		this.timerStamp = Time.realtimeSinceStartup;
	}

	private void OnDestroy()
	{
		this.items.Clear();
		this.removes.Clear();
	}

	private void FixedUpdate()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup - this.timerStamp < this.Duration)
		{
			return;
		}
		this.timerStamp = realtimeSinceStartup;
		this.frame++;
		this.removes.Clear();
		RaycastHit[] array = Physics.RaycastAll(base.transform.position, base.transform.forward, this.Distance, this.RaycastLayerMask);
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit raycastHit = array[i];
			Renderer renderer = raycastHit.collider.renderer;
			if (renderer)
			{
				if (this.items.ContainsKey(renderer))
				{
					TransparentDiffuse.TransparentRenderItem transparentRenderItem = this.items[renderer];
					transparentRenderItem.frame = this.frame;
				}
				else
				{
					TransparentDiffuse.TransparentRenderItem transparentRenderItem2 = new TransparentDiffuse.TransparentRenderItem();
					if (this.transparentShader == null)
					{
						this.transparentShader = Shader.Find("Transparent/Diffuse");
					}
					for (int j = 0; j < renderer.materials.Length; j++)
					{
						Material material = renderer.materials[j];
						transparentRenderItem2.shader.Add(material.shader);
						transparentRenderItem2.color.Add((!material.HasProperty("_Color")) ? Color.white : material.color);
						transparentRenderItem2.frame = this.frame;
						material.shader = this.transparentShader;
						material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.3f);
					}
					this.items[renderer] = transparentRenderItem2;
				}
			}
		}
		foreach (KeyValuePair<Renderer, TransparentDiffuse.TransparentRenderItem> current in this.items)
		{
			if (current.Value.frame != this.frame)
			{
				this.removes.Add(current.Key);
			}
		}
		for (int k = 0; k < this.removes.Count; k++)
		{
			Renderer renderer2 = this.removes[k];
			for (int l = 0; l < renderer2.materials.Length; l++)
			{
				TransparentDiffuse.TransparentRenderItem transparentRenderItem3 = this.items[renderer2];
				Material material2 = renderer2.materials[l];
				material2.shader = transparentRenderItem3.shader[l];
				if (material2.HasProperty("_Color"))
				{
					material2.SetColor("_Color", transparentRenderItem3.color[l]);
				}
			}
			this.items.Remove(renderer2);
		}
		this.removes.Clear();
	}
}
