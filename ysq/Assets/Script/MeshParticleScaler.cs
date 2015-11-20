using System;
using UnityEngine;

public sealed class MeshParticleScaler : MonoBehaviour
{
	public class PSInitParam
	{
		public float startSpeed;

		public float startSize;

		public float gravityModifier;
	}

	public float customScale = 1f;

	public int renderQueue;

	private ParticleSystem[] particleSystems;

	private MeshParticleScaler.PSInitParam[] psInitParams;

	private float[] lastPSScales;

	private void Start()
	{
		this.particleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
		this.psInitParams = new MeshParticleScaler.PSInitParam[this.particleSystems.Length];
		this.lastPSScales = new float[this.particleSystems.Length];
		this.ScaleShurikenSystems(true);
	}

	private void Update()
	{
		this.ScaleShurikenSystems(false);
	}

	private void ScaleShurikenSystems(bool init)
	{
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			ParticleSystem particleSystem = this.particleSystems[i];
			if (init)
			{
				this.psInitParams[i] = new MeshParticleScaler.PSInitParam();
				this.psInitParams[i].startSpeed = particleSystem.startSpeed;
				this.psInitParams[i].startSize = particleSystem.startSize;
				this.psInitParams[i].gravityModifier = particleSystem.gravityModifier;
				if (this.renderQueue > 0)
				{
					particleSystem.renderer.material.renderQueue = this.renderQueue;
				}
			}
			float num = (particleSystem.transform.localScale.x + particleSystem.transform.localScale.y + particleSystem.transform.localScale.z) / 3f;
			Transform parent = particleSystem.transform.parent;
			while (parent != null)
			{
				num *= (parent.localScale.x + parent.localScale.y + parent.localScale.z) / 3f;
				parent = parent.parent;
			}
			float num2 = this.customScale * num;
			if (this.lastPSScales[i] != num2)
			{
				this.lastPSScales[i] = num2;
				particleSystem.startSpeed = this.psInitParams[i].startSpeed * num2;
				particleSystem.startSize = this.psInitParams[i].startSize * num2;
				particleSystem.gravityModifier = this.psInitParams[i].gravityModifier * num2;
				particleSystem.Simulate(particleSystem.time);
				particleSystem.Play();
			}
		}
	}
}
