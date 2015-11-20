using System;
using UnityEngine;

[ExecuteInEditMode]
public class SetupAdvancedFoliageShader : MonoBehaviour
{
	public Vector4 Wind = new Vector4(0.85f, 0.075f, 0.4f, 1f);

	public float WindFrequency = 0.75f;

	public float WaveSizeFoliageShader = 10f;

	public float WindMultiplierForGrassshader = 4f;

	public float WaveSizeForGrassshader = 1f;

	public float DetailDistanceForGrassShader = 80f;

	private Vector4 TempWind;

	private float TempWindForce;

	private float GrassWind;

	private void Awake()
	{
		this.afsUpdateWind();
	}

	public void Update()
	{
		this.afsUpdateWind();
	}

	private void afsUpdateWind()
	{
		this.TempWind = this.Wind;
		this.TempWindForce = this.Wind.w;
		this.TempWind.x = this.TempWind.x * ((1.25f + Mathf.Sin(Time.time * this.WindFrequency) * Mathf.Sin(Time.time * 0.375f)) * 0.5f);
		this.TempWind.z = this.TempWind.z * ((1.25f + Mathf.Sin(Time.time * this.WindFrequency) * Mathf.Sin(Time.time * 0.193f)) * 0.5f);
		this.TempWind.w = this.TempWindForce;
		Shader.SetGlobalVector("_Wind", this.TempWind);
		this.GrassWind = (this.TempWind.x + this.TempWind.z) / 2f * this.Wind.w;
		Shader.SetGlobalFloat("_AfsWaveSize", 0.5f / this.WaveSizeFoliageShader);
		Shader.SetGlobalVector("_AfsWaveAndDistance", new Vector4(Time.time * (this.WindFrequency * 0.05f), 0.5f / this.WaveSizeForGrassshader, this.GrassWind * this.WindMultiplierForGrassshader, this.DetailDistanceForGrassShader * this.DetailDistanceForGrassShader));
	}
}
