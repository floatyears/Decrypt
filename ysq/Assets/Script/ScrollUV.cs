using System;
using UnityEngine;

[AddComponentMenu("Game/FX/ScrollUV")]
public class ScrollUV : MonoBehaviour
{
	public Vector2 scrollSpeed = Vector2.one;

	private void Update()
	{
		if (base.renderer == null || base.renderer.material == null)
		{
			return;
		}
		Vector2 vector = base.renderer.material.mainTextureOffset;
		vector += this.scrollSpeed * Time.deltaTime;
		if (vector.x > 1f)
		{
			vector.x = 0f;
		}
		else if (vector.x < 0f)
		{
			vector.x = 1f;
		}
		if (vector.y > 1f)
		{
			vector.y = 0f;
		}
		else if (vector.x < 0f)
		{
			vector.y = 1f;
		}
		base.renderer.material.mainTextureOffset = vector;
	}
}
