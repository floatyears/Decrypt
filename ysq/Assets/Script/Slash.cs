using System;
using UnityEngine;

[AddComponentMenu("Game/FX/Slash")]
public class Slash : MonoBehaviour
{
	public Vector2 speed = Vector2.zero;

	private void Update()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x += this.speed.x * Time.deltaTime;
		localScale.y += this.speed.y * Time.deltaTime;
		if (localScale.x < 0f)
		{
			localScale.x = 0f;
		}
		if (localScale.y < 0f)
		{
			localScale.y = 0f;
		}
		base.transform.localScale = localScale;
	}
}
