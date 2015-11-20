using System;
using UnityEngine;

public class UIFx074 : MonoBehaviour
{
	public void Init()
	{
		base.Invoke("DestroySelf", 8f);
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
