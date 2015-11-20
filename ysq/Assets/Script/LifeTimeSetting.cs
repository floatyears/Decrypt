using System;
using UnityEngine;

[AddComponentMenu("Game/LifeTimeSetting")]
public class LifeTimeSetting : MonoBehaviour
{
	public float liftTime = 1f;

	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, this.liftTime);
		if (PoolMgr.spawnPoolByName != null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("should not use LifeTimeSetting in {0}", base.name)
			});
		}
	}

	private void OnSpawned()
	{
		global::Debug.LogError(new object[]
		{
			string.Format("should not use LifeTimeSetting in {0}", base.name)
		});
	}
}
