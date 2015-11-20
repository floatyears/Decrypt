using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIStick : MonoBehaviour
{
	public Transform target;

	private Vector3 mLastPosition;

	private Transform mTrans;

	private void Awake()
	{
		this.mTrans = base.transform;
	}

	private void Update()
	{
		if (this.target == null)
		{
			return;
		}
		if (this.mLastPosition != this.target.localPosition)
		{
			this.mLastPosition = this.target.localPosition;
			this.mTrans.localPosition = this.mLastPosition;
		}
	}
}
