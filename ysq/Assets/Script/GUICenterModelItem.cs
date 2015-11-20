using System;
using System.Collections.Generic;
using UnityEngine;

public class GUICenterModelItem : MonoBehaviour
{
	public delegate void OnCenterCallback(GameObject centeredObject);

	public float springStrength = 8f;

	public float nextPageThreshold;

	public SpringPanel.OnFinished onFinished;

	public GUICenterModelItem.OnCenterCallback onCenter;

	private UIScrollView mScrollView;

	private GameObject mCenteredObject;

	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	public void Init()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				global::Debug.LogWarning(new object[]
				{
					string.Concat(new object[]
					{
						base.GetType(),
						" requires ",
						typeof(UIScrollView),
						" on a parent object in order to work"
					}),
					this
				});
				base.enabled = false;
				return;
			}
			if (this.mScrollView)
			{
				UIScrollView expr_94 = this.mScrollView;
				expr_94.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(expr_94.onDragFinished, new UIScrollView.OnDragNotification(this.OnDragFinished));
			}
		}
	}

	public void Recenter()
	{
		this.Init();
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 a = NGUIMath.SpringDampen(ref vector2, 9f, 2f);
		Vector3 b = vector - a * 0.01f;
		float num = 3.40282347E+38f;
		Transform target = null;
		int index = 0;
		int num2 = 0;
		int i = 0;
		int childCount = transform.childCount;
		int num3 = 0;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.activeInHierarchy)
			{
				float num4 = Vector3.SqrMagnitude(child.position - b);
				if (num4 < num)
				{
					num = num4;
					target = child;
					index = i;
					num2 = num3;
				}
				num3++;
			}
			i++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == transform.GetChild(index))
		{
			Vector3 point = UICamera.currentTouch.totalDelta;
			point = base.transform.rotation * point;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num5;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num5 = point.magnitude;
				}
				else
				{
					num5 = point.y;
				}
			}
			else
			{
				num5 = point.x;
			}
			if (Mathf.Abs(num5) > this.nextPageThreshold)
			{
				UIGrid component = base.GetComponent<UIGrid>();
				if (component != null && component.sorting != UIGrid.Sorting.None)
				{
					List<Transform> childList = component.GetChildList();
					if (num5 > this.nextPageThreshold)
					{
						if (num2 > 0)
						{
							target = childList[num2 - 1];
						}
						else
						{
							target = childList[0];
						}
					}
					else if (num5 < -this.nextPageThreshold)
					{
						if (num2 < childList.Count - 1)
						{
							target = childList[num2 + 1];
						}
						else
						{
							target = childList[childList.Count - 1];
						}
					}
				}
				else
				{
					global::Debug.LogWarning(new object[]
					{
						"Next Page Threshold requires a sorted UIGrid in order to work properly",
						this
					});
				}
			}
		}
		this.CenterOn(target, vector);
	}

	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 a = cachedTransform.InverseTransformPoint(target.position);
			Vector3 b = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 b2 = a - b;
			if (!this.mScrollView.canMoveHorizontally)
			{
				b2.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				b2.y = 0f;
			}
			b2.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - b2, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}
}
