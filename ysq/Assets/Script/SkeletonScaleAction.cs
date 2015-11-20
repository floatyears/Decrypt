using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Skeleton Scale Action")]
public sealed class SkeletonScaleAction : ActionBase
{
	public string BoneName;

	public float scale = 2f;

	public float duration = 0.5f;

	public SkeletonScale.ByType type;

	private SkeletonScale scaleCmp;

	protected override void DoAction()
	{
		if (this.scaleCmp != null)
		{
			GameObject gameObject = base.variables.skillCaster.gameObject;
			SkeletonScale x = null;
			SkeletonScale[] components = gameObject.GetComponents<SkeletonScale>();
			for (int i = 0; i < components.Length; i++)
			{
				SkeletonScale skeletonScale = components[i];
				if (skeletonScale.BoneName == this.BoneName)
				{
					x = skeletonScale;
					break;
				}
			}
			if (x == null)
			{
				x = gameObject.AddComponent<SkeletonScale>();
				this.scaleCmp.BoneName = this.BoneName;
			}
			this.scaleCmp = x;
			this.scaleCmp = x;
		}
		this.scaleCmp.scale = this.scale * Vector3.one;
		this.scaleCmp.duration = this.duration;
		this.scaleCmp.type = this.type;
		base.Finish();
	}
}
