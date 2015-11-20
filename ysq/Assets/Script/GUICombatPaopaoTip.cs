using Holoville.HOTween;
using System;
using UnityEngine;

public class GUICombatPaopaoTip : MonoBehaviour
{
	public float mScale = 1f;

	public float mHeightOffest = 0.4f;

	private Camera m2dCamera;

	private Camera m3dCamera;

	private float mSizeY;

	private float mNpcHeight;

	private ActorController mTargetUnit;

	private UILabel mContentDesc;

	private float mShowTipTimer;

	private void CreateObjects()
	{
		this.m2dCamera = GameUIManager.mInstance.uiCamera.camera;
		this.m3dCamera = Camera.main;
		this.mContentDesc = base.transform.Find("desc").GetComponent<UILabel>();
	}

	public void InitWithActorController(ActorController target, string content, float showTime)
	{
		if (target == null)
		{
			return;
		}
		if (this.mContentDesc == null)
		{
			this.CreateObjects();
		}
		this.mContentDesc.text = content;
		this.mTargetUnit = target;
		this.mSizeY = ((!(this.mTargetUnit.collider == null)) ? this.mTargetUnit.collider.bounds.size.y : 0f);
		this.mShowTipTimer = showTime;
		this.Show();
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
		HOTween.To(base.gameObject.transform, 0f, new TweenParms().Prop("localScale", Vector3.zero));
		HOTween.To(base.gameObject.transform, 0f, new TweenParms().Prop("localRotation", new Vector3(0f, 0f, 60f)));
		HOTween.To(base.gameObject.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutSine));
		HOTween.To(base.gameObject.transform, 0.25f, new TweenParms().Prop("localRotation", Vector3.zero));
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void LateUpdate()
	{
		this.mShowTipTimer -= Time.deltaTime;
		if (this.mTargetUnit == null || !this.mTargetUnit.gameObject.activeInHierarchy || this.mTargetUnit.IsDead || this.mShowTipTimer <= 0f)
		{
			this.DestroySelf();
		}
		else
		{
			this.mNpcHeight = this.mTargetUnit.transform.position.y + this.mSizeY + this.mHeightOffest;
			Vector3 position = new Vector3(this.mTargetUnit.transform.position.x, this.mNpcHeight, this.mTargetUnit.transform.position.z);
			position = this.m2dCamera.ViewportToWorldPoint(this.m3dCamera.WorldToViewportPoint(position));
			position.z = 0f;
			base.transform.position = position;
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z += 5000f;
			base.transform.localPosition = localPosition;
		}
	}
}
