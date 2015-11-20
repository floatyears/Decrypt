using System;
using UnityEngine;

public class Delay : MonoBehaviour
{
	public float delayTime = 1f;

	private bool flag;

	private void Start()
	{
		if (!this.flag)
		{
			base.gameObject.SetActive(false);
			base.Invoke("DelayFunc", this.delayTime);
		}
	}

	private void OnSpawned()
	{
		base.gameObject.SetActive(false);
		base.Invoke("DelayFunc", this.delayTime);
		this.flag = true;
	}

	private void DelayFunc()
	{
		base.gameObject.SetActive(true);
	}

	private void OnDespawned()
	{
		base.CancelInvoke();
	}
}
