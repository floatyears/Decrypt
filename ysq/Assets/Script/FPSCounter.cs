using System;
using System.Text;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	public float frequency = 0.5f;

	public UILabel lbText;

	private StringBuilder sb = new StringBuilder();

	private float timeLeft;

	private float lastTime;

	private int lastFrameCount;

	public int FramesPerSec
	{
		get;
		protected set;
	}

	private void Start()
	{
		this.timeLeft = this.frequency;
		this.lastTime = Time.realtimeSinceStartup;
		this.lastFrameCount = Time.frameCount;
	}

	private void Update()
	{
		this.timeLeft -= Time.deltaTime;
		if (this.timeLeft <= 0f)
		{
			float num = Time.realtimeSinceStartup - this.lastTime;
			int num2 = Time.frameCount - this.lastFrameCount;
			this.FramesPerSec = Mathf.RoundToInt((float)num2 / num);
			this.sb.Remove(0, this.sb.Length);
			this.sb.Append(this.FramesPerSec).Append(" FPS");
			this.lbText.text = this.sb.ToString();
			this.timeLeft = this.frequency;
			this.lastTime = Time.realtimeSinceStartup;
			this.lastFrameCount = Time.frameCount;
		}
	}
}
