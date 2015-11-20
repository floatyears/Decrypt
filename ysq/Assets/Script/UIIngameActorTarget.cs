using System;
using UnityEngine;

public class UIIngameActorTarget : MonoBehaviour
{
	public float mScale = 0.6f;

	public float mHeightOffest = 0.7f;

	private Camera m2dCamera;

	private Camera m3dCamera;

	private float mSizeY;

	private float mNpcHeight;

	private static UIIngameActorTarget mInstance;

	public ActorController mTargetUnit
	{
		get;
		private set;
	}

	public static UIIngameActorTarget GetInstance()
	{
		if (UIIngameActorTarget.mInstance == null)
		{
			UIIngameActorTarget.CreateInstance();
		}
		return UIIngameActorTarget.mInstance;
	}

	private static void CreateInstance()
	{
		if (GameUIManager.mInstance != null)
		{
			GameObject prefab = Res.LoadGUI("GUI/UIIngameActorTarget");
			GameObject gameObject = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
			UIIngameActorTarget.mInstance = gameObject.AddComponent<UIIngameActorTarget>();
			UIIngameActorTarget.mInstance.m2dCamera = GameUIManager.mInstance.uiCamera.camera;
			UIIngameActorTarget.mInstance.m3dCamera = Camera.main;
		}
	}

	public void Init(ActorController target)
	{
		if (target == null)
		{
			return;
		}
		this.mTargetUnit = target;
		this.mSizeY = ((!(this.mTargetUnit.collider == null)) ? this.mTargetUnit.collider.bounds.size.y : 0f);
		this.Show();
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void LateUpdate()
	{
		if (this.mTargetUnit == null || !this.mTargetUnit.gameObject.activeInHierarchy || this.mTargetUnit.IsDead)
		{
			this.DestroySelf();
		}
		else
		{
			this.mNpcHeight = this.mTargetUnit.transform.position.y + this.mSizeY + this.mHeightOffest;
			Vector3 position = new Vector3(this.mTargetUnit.transform.position.x, this.mNpcHeight, this.mTargetUnit.transform.position.z);
			position = this.m2dCamera.ViewportToWorldPoint(this.m3dCamera.WorldToViewportPoint(position));
			position.z = 15f;
			base.transform.localScale = Vector3.one * this.mScale;
			base.transform.position = position;
		}
	}
}
