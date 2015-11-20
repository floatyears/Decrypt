using System;
using UnityEngine;

public sealed class ParticleScaler : MonoBehaviour
{
	public bool scaleByUIRoot = true;

	public bool scaleByParent;

	public int renderQueue;

	private void Start()
	{
		float scaleFactor = (!this.scaleByUIRoot) ? 1f : ParticleScaler.GetsRootScaleFactor();
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>(true);
		ParticleScaler.ScaleShurikenSystems(componentsInChildren, scaleFactor, this.scaleByParent, this.renderQueue);
	}

	public static void ScaleShurikenSystems(ParticleSystem[] systems, float scaleFactor, bool scaleByParent, int renderQueue)
	{
		for (int i = 0; i < systems.Length; i++)
		{
			ParticleSystem particleSystem = systems[i];
			float num = scaleFactor;
			if (scaleByParent)
			{
				float num2 = (particleSystem.transform.localScale.x + particleSystem.transform.localScale.y + particleSystem.transform.localScale.z) / 3f;
				Transform parent = particleSystem.transform.parent;
				while (parent != null)
				{
					num2 *= (parent.localScale.x + parent.localScale.y + parent.localScale.z) / 3f;
					parent = parent.parent;
				}
				num *= num2;
			}
			particleSystem.startSpeed *= num;
			particleSystem.startSize *= num;
			particleSystem.gravityModifier *= num;
			if (renderQueue > 0)
			{
				particleSystem.renderer.material.renderQueue = renderQueue;
			}
			particleSystem.Simulate(0f, false, true);
			particleSystem.Play();
		}
	}

	public static float GetsRootScaleFactor()
	{
		UIRoot uIRoot = (!(GameUIManager.mInstance == null)) ? NGUITools.FindInParents<UIRoot>(GameUIManager.mInstance.uiCamera.gameObject) : null;
		if (uIRoot == null)
		{
			return 1f;
		}
		return uIRoot.transform.localScale.x / (2f / (float)uIRoot.minimumHeight);
	}
}
