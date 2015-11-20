using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayActorControllerDead : MonoBehaviour
{
	public enum DeadEffectType
	{
		Dissolve,
		ReplacePrefab
	}

	public PlayActorControllerDead.DeadEffectType type;

	public float delayPlayTime;

	public Material dissolveTemplate;

	public float dissolveDuration = 1.5f;

	public GameObject prefab;

	private SkinnedMeshRenderer[] meshRenders;

	private float dissolveTimestamp;

	private void Update()
	{
		if (this.type == PlayActorControllerDead.DeadEffectType.Dissolve && this.dissolveTimestamp > 1f && this.meshRenders != null)
		{
			float num = Time.time - this.dissolveTimestamp;
			if (num > this.dissolveDuration)
			{
				this.dissolveTimestamp = 0f;
			}
			else
			{
				float value = Mathf.Clamp01(num / this.dissolveDuration);
				for (int i = 0; i < this.meshRenders.Length; i++)
				{
					if (!(this.meshRenders[i].renderer == null))
					{
						for (int j = 0; j < this.meshRenders[i].sharedMaterials.Length; j++)
						{
							Material material = this.meshRenders[i].renderer.sharedMaterials[j];
							if (!(material == null))
							{
								material.SetFloat("_Amount", value);
							}
						}
					}
				}
			}
		}
	}

	public float StartDeadEffect()
	{
		if (this.delayPlayTime <= 0f)
		{
			return this.DoDeadEffect();
		}
		base.Invoke("DoDeadEffect", this.delayPlayTime);
		return this.delayPlayTime + ((this.type != PlayActorControllerDead.DeadEffectType.Dissolve) ? 0f : this.dissolveDuration);
	}

	private float DoDeadEffect()
	{
		if (this.type == PlayActorControllerDead.DeadEffectType.Dissolve)
		{
			this.StartDissolve();
			return this.dissolveDuration;
		}
		this.SpawnPrefab();
		return 0f;
	}

	private void SpawnPrefab()
	{
		if (this.prefab != null)
		{
			UnityEngine.Object.Instantiate(this.prefab, base.transform.position, base.transform.rotation);
		}
	}

	private void StartDissolve()
	{
		this.dissolveTimestamp = Time.time;
		global::Debug.Assert(this.dissolveTemplate != null, "can not find dissolve material template.");
		global::Debug.Assert(this.dissolveTemplate.name != "Game/Dissolve/Dissolve_TexturCoords", "dissolve material shader error.");
		this.meshRenders = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < this.meshRenders.Length; i++)
		{
			if (!(this.meshRenders[i].renderer == null))
			{
				List<Material> list = new List<Material>();
				for (int j = 0; j < this.meshRenders[i].sharedMaterials.Length; j++)
				{
					Material material = this.meshRenders[i].renderer.sharedMaterials[j];
					if (material == null)
					{
						list.Add(material);
					}
					else
					{
						list.Add(new Material(this.dissolveTemplate)
						{
							mainTexture = this.meshRenders[i].sharedMaterial.mainTexture
						});
					}
				}
				this.meshRenders[i].renderer.sharedMaterials = list.ToArray();
			}
		}
	}

	private void OnDespawned()
	{
		base.CancelInvoke();
	}
}
