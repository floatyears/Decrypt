using System;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
	public float LineTilingWeight = 3f;

	public int uvAnimationTileY = 2;

	public float framesPerSecond = 10f;

	public bool loop = true;

	public bool play = true;

	public Vector3 StartPos;

	public Vector3 EndPos;

	private LineRenderer LineRenderer;

	private Material LineRendererMatrial;

	private float offestTime;

	private void Awake()
	{
		this.LineRenderer = base.GetComponent<LineRenderer>();
		if (this.LineRenderer != null)
		{
			this.LineRendererMatrial = new Material(this.LineRenderer.material);
			this.LineRenderer.material = this.LineRendererMatrial;
			this.LineRenderer.SetVertexCount(2);
			this.LineRenderer.useWorldSpace = true;
		}
	}

	private void Start()
	{
		this.offestTime = Time.time;
	}

	private void Update()
	{
		if (this.LineRenderer == null || !this.LineRenderer.enabled || this.LineRendererMatrial == null)
		{
			return;
		}
		this.LineRenderer.SetPosition(0, this.StartPos);
		this.LineRenderer.SetPosition(1, this.EndPos);
		int num = Mathf.RoundToInt((Time.time - this.offestTime) * this.framesPerSecond);
		if (this.play)
		{
			float num2 = Vector3.Distance(this.StartPos, this.EndPos);
			Vector2 scale = new Vector2(num2 / this.LineTilingWeight, 1f / (float)this.uvAnimationTileY);
			num %= this.uvAnimationTileY;
			Vector2 offset = new Vector2(0f, (float)num * scale.y);
			this.LineRenderer.material.SetTextureOffset("_MainTex", offset);
			this.LineRenderer.material.SetTextureScale("_MainTex", scale);
		}
		if (!this.loop && num >= this.uvAnimationTileY - 1)
		{
			this.play = false;
		}
	}
}
