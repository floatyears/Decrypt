using System;
using UnityEngine;

[ExecuteInEditMode]
public class EffectSoundClip : MonoBehaviour
{
	public AudioClip clip;

	private void OnEnable()
	{
		Globals.Instance.EffectSoundMgr.Play(this.clip, 1f, Vector3.zero);
	}
}
