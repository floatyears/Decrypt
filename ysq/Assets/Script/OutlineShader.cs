using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class OutlineShader : MonoBehaviour
{
	private Dictionary<Renderer, Material[]> defaultMaterial = new Dictionary<Renderer, Material[]>();

	private bool outline;

	private static Shader outlineShader;

	private static Shader outlineRimShader;

	private ActorController controller;

	public bool Outline
	{
		set
		{
			if (this.outline == value)
			{
				return;
			}
			this.outline = value;
			if (this.controller != null)
			{
				List<CharacterMeshInfo> meshInfos = this.controller.MeshInfos;
				for (int i = 0; i < meshInfos.Count; i++)
				{
					CharacterMeshInfo characterMeshInfo = meshInfos[i];
					if (this.outline)
					{
						this.VerifyOutlineShader();
						for (int j = 0; j < characterMeshInfo.Materials.Length; j++)
						{
							Material material = characterMeshInfo.Materials[j];
							if (!material.shader.name.Contains("Particle") && !material.shader.name.Contains("Cutoff"))
							{
								if (material.shader.name.Contains("Rim"))
								{
									if (OutlineShader.outlineRimShader != null && material.shader != OutlineShader.outlineRimShader)
									{
										material.shader = OutlineShader.outlineRimShader;
									}
								}
								else if (OutlineShader.outlineShader != null && material.shader != OutlineShader.outlineShader)
								{
									material.shader = OutlineShader.outlineShader;
								}
							}
						}
						characterMeshInfo.Materials = characterMeshInfo.Materials;
					}
					else
					{
						for (int k = 0; k < characterMeshInfo.Materials.Length; k++)
						{
							if (characterMeshInfo.Materials[k].shader != characterMeshInfo.InitShader[k])
							{
								characterMeshInfo.Materials[k].shader = characterMeshInfo.InitShader[k];
							}
						}
						characterMeshInfo.Materials = characterMeshInfo.Materials;
					}
				}
			}
			else
			{
				Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>(true);
				for (int l = 0; l < componentsInChildren.Length; l++)
				{
					Renderer renderer = componentsInChildren[l];
					if (!(renderer is ParticleRenderer))
					{
						if (value)
						{
							this.VerifyOutlineShader();
							this.defaultMaterial[renderer] = renderer.materials;
							List<Material> list = new List<Material>();
							for (int m = 0; m < renderer.materials.Length; m++)
							{
								Material material2 = renderer.materials[m];
								if (material2.shader.name.Contains("Particle") || material2.shader.name.Contains("Cutoff"))
								{
									list.Add(material2);
								}
								else
								{
									Material material3 = new Material(material2);
									if (material3.shader.name.Contains("Rim"))
									{
										if (OutlineShader.outlineRimShader != null)
										{
											material3.shader = OutlineShader.outlineRimShader;
										}
									}
									else if (OutlineShader.outlineShader != null)
									{
										material3.shader = OutlineShader.outlineShader;
									}
									material3.hideFlags = (HideFlags.DontSave | HideFlags.NotEditable);
									list.Add(material3);
								}
							}
							renderer.materials = list.ToArray();
						}
						else if (this.defaultMaterial.ContainsKey(renderer))
						{
							renderer.materials = this.defaultMaterial[renderer];
							this.defaultMaterial.Remove(renderer);
						}
					}
				}
			}
		}
	}

	private void VerifyOutlineShader()
	{
		if (OutlineShader.outlineShader == null)
		{
			OutlineShader.outlineShader = Shader.Find("Game/Characters/Diffuse Outline");
		}
		if (OutlineShader.outlineRimShader == null)
		{
			OutlineShader.outlineRimShader = Shader.Find("Game/Characters/Diffuse Rim Outline");
		}
	}

	private void Start()
	{
		this.controller = base.gameObject.GetComponent<ActorController>();
	}

	private void OnDestroy()
	{
		this.defaultMaterial.Clear();
	}
}
