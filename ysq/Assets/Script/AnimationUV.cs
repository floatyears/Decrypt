using System;
using UnityEngine;

[AddComponentMenu("Game/FX/AnimationUV")]
public class AnimationUV : MonoBehaviour
{
	public int uvAnimationTileX = 24;

	public int uvAnimationTileY = 1;

	public float framesPerSecond = 10f;

	public bool loop = true;

	public bool play = true;

	public bool hideWhenStopPlaying;

	private int indexFrames;

	private float offestTime;

	private void Start()
	{
		this.offestTime = Time.time;
	}

	private void OnSpawned()
	{
		this.indexFrames = 0;
		this.offestTime = Time.time;
	}

	private void Update()
	{
		int num = Mathf.RoundToInt((Time.time - this.offestTime) * this.framesPerSecond);
		if (this.play && this.indexFrames != num)
		{
			this.indexFrames = num % (this.uvAnimationTileX * this.uvAnimationTileY);
			Vector2 scale = new Vector2(1f / (float)this.uvAnimationTileX, 1f / (float)this.uvAnimationTileY);
			int num2 = this.indexFrames % this.uvAnimationTileX;
			int num3 = this.indexFrames / this.uvAnimationTileX;
			Vector2 offset = new Vector2((float)num2 * scale.x, 1f - scale.y - (float)num3 * scale.y);
			base.renderer.material.SetTextureOffset("_MainTex", offset);
			base.renderer.material.SetTextureScale("_MainTex", scale);
		}
		if (!this.loop && this.indexFrames >= this.uvAnimationTileX * this.uvAnimationTileY - 1)
		{
			this.play = false;
			if (this.hideWhenStopPlaying)
			{
				base.renderer.gameObject.SetActive(false);
			}
		}
	}
}
