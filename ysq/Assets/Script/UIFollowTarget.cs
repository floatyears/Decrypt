using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
	public Transform target;

	public Camera gameCamera;

	public Camera uiCamera;

	public bool disableIfInvisible = true;

	private Transform mTrans;

	private bool mIsVisible;

	private Vector3 offset = Vector3.zero;

	private void Awake()
	{
		this.mTrans = base.transform;
	}

	private void Start()
	{
		if (this.target != null)
		{
			if (this.gameCamera == null)
			{
				this.gameCamera = NGUITools.FindCameraForLayer(this.target.gameObject.layer);
			}
			if (this.uiCamera == null)
			{
				this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			this.SetVisible(false);
			this.offset = new Vector3(0f, ((BoxCollider)this.target.collider).size.y, 0f);
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				"Expected to have 'target' set to a valid transform",
				this
			});
			base.enabled = false;
		}
	}

	private void SetVisible(bool val)
	{
		this.mIsVisible = val;
		int i = 0;
		int childCount = this.mTrans.childCount;
		while (i < childCount)
		{
			if (this.mTrans.GetChild(i).gameObject.activeInHierarchy)
			{
				NGUITools.SetActive(this.mTrans.GetChild(i).gameObject, val);
			}
			i++;
		}
	}

	private void Update()
	{
		Vector3 vector = this.gameCamera.WorldToViewportPoint(this.target.position + this.offset);
		bool flag = vector.z > 0f && vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f;
		if (this.disableIfInvisible && this.mIsVisible != flag)
		{
			this.SetVisible(flag);
		}
		if (flag)
		{
			base.transform.position = this.uiCamera.ViewportToWorldPoint(vector);
			vector = this.mTrans.localPosition;
			vector.x = (float)Mathf.RoundToInt(vector.x);
			vector.y = (float)Mathf.RoundToInt(vector.y);
			vector.z = 0f;
			this.mTrans.localPosition = vector;
		}
		else
		{
			this.SetVisible(false);
		}
		this.OnUpdate(flag);
	}

	protected virtual void OnUpdate(bool isVisible)
	{
	}
}
