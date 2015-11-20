using System;
using System.Collections.Generic;
using UnityEngine;

public class GameRenderQueue : MonoBehaviour
{
	public int renderQueue = 3100;

	[HideInInspector]
	public Renderer[] rds;

	private void Start()
	{
		this.Init();
	}

	public void Init()
	{
		this.rds = base.gameObject.GetComponentsInChildren<Renderer>();
		if (this.rds == null)
		{
			return;
		}
		List<Material> list = new List<Material>();
		for (int i = 0; i < this.rds.Length; i++)
		{
			for (int j = 0; j < this.rds[i].sharedMaterials.Length; j++)
			{
				Material material = this.rds[i].sharedMaterials[j];
				if (material == null)
				{
					list.Add(material);
				}
				else
				{
					list.Add(new Material(material)
					{
						renderQueue = this.renderQueue
					});
				}
			}
			this.rds[i].sharedMaterials = list.ToArray();
			list.Clear();
		}
	}
}
