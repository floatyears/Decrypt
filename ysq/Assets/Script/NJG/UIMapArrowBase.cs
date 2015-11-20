using System;
using UnityEngine;

namespace NJG
{
	public class UIMapArrowBase : MonoBehaviour
	{
		[SerializeField]
		public NJGMapItem item;

		public Transform child;

		public bool isValid;

		private Transform mTrans;

		protected float rotationOffset;

		private Vector3 mRot = Vector3.zero;

		private Vector3 mArrowRot = Vector3.zero;

		private Vector3 mFrom = Vector3.zero;

		public Transform cachedTransform
		{
			get
			{
				if (this.mTrans == null)
				{
					this.mTrans = base.transform;
				}
				return this.mTrans;
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return this.mRot;
			}
		}

		public void UpdateRotation(Vector3 fromTarget)
		{
			this.mFrom = fromTarget - this.item.cachedTransform.position;
			float num;
			if (NJGMapBase.instance.orientation == NJGMapBase.Orientation.XZDefault)
			{
				this.mFrom.y = 0f;
				num = Vector3.Angle(Vector3.forward, this.mFrom);
			}
			else
			{
				this.mFrom.z = 0f;
				num = Vector3.Angle(Vector3.up, this.mFrom);
			}
			if (Vector3.Dot(Vector3.right, this.mFrom) < 0f)
			{
				num = 360f - num;
			}
			num += 180f;
			this.mRot = Vector3.zero;
			if (NJGMapBase.instance.orientation == NJGMapBase.Orientation.XZDefault)
			{
				this.mRot.z = num;
				this.mRot.y = 180f;
			}
			else
			{
				this.mRot.z = -num;
				this.mRot.y = (this.mRot.x = 0f);
			}
			if (!this.cachedTransform.localEulerAngles.Equals(this.mRot))
			{
				this.cachedTransform.localEulerAngles = this.mRot;
			}
			if (!this.item.arrowRotate)
			{
				this.mArrowRot.x = 0f;
				this.mArrowRot.y = 180f;
				this.mArrowRot.z = ((!UIMiniMapBase.inst.rotateWithPlayer) ? (-this.cachedTransform.localEulerAngles.z) : (UIMiniMapBase.inst.iconRoot.localEulerAngles.z - this.cachedTransform.localEulerAngles.z));
				if (this.child.localEulerAngles != this.mArrowRot)
				{
					this.child.localEulerAngles = this.mArrowRot;
				}
			}
		}
	}
}
