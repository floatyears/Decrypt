using System;
using UnityEngine;

public class ParticleSetting : MonoBehaviour
{
	public float LightIntensityMult = -0.5f;

	public float liftTime = 1f;

	public bool randomRotation;

	public Vector3 posOffest = Vector3.zero;

	private void Start()
	{
		base.gameObject.transform.position += this.posOffest;
		if (this.randomRotation)
		{
			base.gameObject.transform.rotation = UnityEngine.Random.rotation;
		}
		UnityEngine.Object.Destroy(base.gameObject, this.liftTime);
	}

	private void Update()
	{
		if (base.gameObject.light != null)
		{
			base.light.intensity += this.LightIntensityMult * Time.deltaTime;
		}
	}
}
