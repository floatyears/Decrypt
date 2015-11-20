using System;
using UnityEngine;

[AddComponentMenu("Game/FX/SMTrailFade")]
public class SMTrailFade : MonoBehaviour
{
	public float fadeInTime = 0.1f;

	public float stayTime = 1f;

	public float fadeOutTime = 0.7f;

	public TrailRenderer thisTrail;

	private float timeElapsed;

	private float timeElapsedLast;

	private float percent;

	private void Start()
	{
		if (this.thisTrail == null)
		{
			return;
		}
		this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		if (this.fadeInTime < 0.01f)
		{
			this.fadeInTime = 0.01f;
		}
		this.percent = this.timeElapsed / this.fadeInTime;
	}

	private void OnSpawned()
	{
		if (this.thisTrail == null)
		{
			return;
		}
		this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		if (this.fadeInTime < 0.01f)
		{
			this.fadeInTime = 0.01f;
		}
		this.timeElapsed = 0f;
		this.timeElapsedLast = 0f;
		this.percent = this.timeElapsed / this.fadeInTime;
	}

	private void Update()
	{
		if (this.thisTrail == null)
		{
			return;
		}
		this.timeElapsed += Time.deltaTime;
		if (this.timeElapsed <= this.fadeInTime)
		{
			this.percent = this.timeElapsed / this.fadeInTime;
			this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, this.percent));
		}
		if (this.timeElapsed > this.fadeInTime && this.timeElapsed < this.fadeInTime + this.stayTime)
		{
			this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		}
		if (this.timeElapsed >= this.fadeInTime + this.stayTime && this.timeElapsed < this.fadeInTime + this.stayTime + this.fadeOutTime)
		{
			this.timeElapsedLast += Time.deltaTime;
			this.percent = 1f - this.timeElapsedLast / this.fadeOutTime;
			this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, this.percent));
		}
	}
}
