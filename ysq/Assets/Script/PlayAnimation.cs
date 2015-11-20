using System;
using UnityEngine;

[Serializable]
public class PlayAnimation
{
	public string AnimName;

	public PlayMode PlayMode;

	public WrapMode WrapMode = WrapMode.Once;

	public float BlendTime;

	public bool replay = true;

	public int priority;
}
