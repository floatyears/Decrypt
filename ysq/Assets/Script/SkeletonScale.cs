using Att;
using System;
using UnityEngine;

public class SkeletonScale : MonoBehaviour
{
	public enum ByType
	{
		None,
		HP
	}

	public string BoneName;

	public Vector3 scale = Vector3.one;

	public float duration = 1f;

	public SkeletonScale.ByType type;

	private Transform boneTransform;

	private Vector3 from;

	private Vector3 to;

	private float timestamp;

	private ActorController acc;

	private void Start()
	{
		if (string.IsNullOrEmpty(this.BoneName))
		{
			return;
		}
		GameObject gameObject = ObjectUtil.FindChildObject(base.gameObject, this.BoneName);
		if (gameObject != null)
		{
			this.boneTransform = gameObject.transform;
			this.from = this.boneTransform.localScale;
			this.to = this.scale;
			this.timestamp = Time.time;
		}
	}

	private void OnSpawned()
	{
		if (string.IsNullOrEmpty(this.BoneName))
		{
			return;
		}
		GameObject gameObject = ObjectUtil.FindChildObject(base.gameObject, this.BoneName);
		if (gameObject != null)
		{
			this.boneTransform = gameObject.transform;
			this.from = this.boneTransform.localScale;
			this.to = this.scale;
			this.timestamp = Time.time;
		}
		this.acc = null;
	}

	public void LateUpdate()
	{
		if (this.boneTransform == null)
		{
			base.enabled = false;
			return;
		}
		if (this.type == SkeletonScale.ByType.HP)
		{
			if (this.acc == null)
			{
				this.acc = base.gameObject.GetComponent<ActorController>();
			}
			if (this.acc != null)
			{
				float t = Mathf.Clamp01(1f - (float)this.acc.CurHP / (float)this.acc.GetAtt(EAttID.EAID_MaxHP));
				this.boneTransform.localScale = Vector3.Lerp(this.from, this.to, t);
			}
		}
		else
		{
			if (this.to != this.scale)
			{
				this.from = this.boneTransform.localScale;
				this.to = this.scale;
				this.timestamp = Time.time;
			}
			float num = Time.time - this.timestamp;
			if (num <= this.duration)
			{
				this.boneTransform.localScale = Vector3.Lerp(this.from, this.to, num / this.duration);
			}
			else
			{
				this.boneTransform.localScale = this.to;
			}
		}
	}
}
