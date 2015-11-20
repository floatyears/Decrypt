using System;
using UnityEngine;

public class ParticleScaleAnimByTween : MonoBehaviour
{
	private TweenScale twScale;

	private GameObject[] pObjs;

	private ParticleSystem[][] ps;

	private float effectScale = -1f;

	private float[][] psInitSize;

	private float[][] psInitSpeed;

	private void Awake()
	{
		this.twScale = base.transform.GetComponent<TweenScale>();
		EventDelegate.Add(this.twScale.onFinished, new EventDelegate.Callback(this.ParticleScaleAnimEnd));
	}

	private void ParticleScaleAnimEnd()
	{
		this.UpdateParticleScaleAnim();
		this.effectScale = -1f;
	}

	public void Init(GameObject[] pObjs, ParticleSystem[][] ps)
	{
		this.pObjs = pObjs;
		this.ps = ps;
		float num = ParticleScaler.GetsRootScaleFactor();
		this.psInitSize = new float[pObjs.Length][];
		this.psInitSpeed = new float[pObjs.Length][];
		for (int i = 0; i < pObjs.Length; i++)
		{
			this.psInitSize[i] = new float[ps[i].Length];
			this.psInitSpeed[i] = new float[ps[i].Length];
			for (int j = 0; j < ps[i].Length; j++)
			{
				this.psInitSize[i][j] = ps[i][j].startSize * num;
				this.psInitSpeed[i][j] = ps[i][j].startSpeed * num;
			}
		}
	}

	private void LateUpdate()
	{
		if (this.twScale.enabled && this.effectScale != this.twScale.value.x)
		{
			this.UpdateParticleScaleAnim();
		}
	}

	private void UpdateParticleScaleAnim()
	{
		this.effectScale = this.twScale.value.x;
		for (int i = 0; i < this.pObjs.Length; i++)
		{
			for (int j = 0; j < this.ps[i].Length; j++)
			{
				this.ps[i][j].startSize = this.psInitSize[i][j] * this.effectScale;
				this.ps[i][j].startSpeed = this.psInitSpeed[i][j] * this.effectScale;
				this.ps[i][j].Simulate(0f, false, true);
				this.ps[i][j].Play();
			}
		}
	}
}
