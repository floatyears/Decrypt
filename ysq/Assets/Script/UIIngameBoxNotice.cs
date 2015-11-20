using System;
using UnityEngine;

public class UIIngameBoxNotice : MonoBehaviour
{
	private UILabel mInfo;

	private Vector3 initPosition;

	private void Awake()
	{
		TweenAlpha component = base.GetComponent<TweenAlpha>();
		EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinished), true);
		TweenScale component2 = base.GetComponent<TweenScale>();
		component2.method = UITweener.Method.BounceEaseOut;
		this.mInfo = base.transform.FindChild("Info").GetComponent<UILabel>();
	}

	private void OnFinished()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetInfoLabel(ActorController actor, string strBoxInfo)
	{
		if (actor != null)
		{
			float num = (!(actor.gameObject.collider == null)) ? ((BoxCollider)actor.gameObject.collider).size.y : 0f;
			float y = actor.transform.position.y + num * actor.transform.localScale.y + 0.4f;
			this.initPosition = actor.transform.position;
			this.initPosition.y = y;
			Vector3 position = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(this.initPosition));
			position.z = 0f;
			base.transform.position = position;
			this.mInfo.text = strBoxInfo;
		}
	}

	private void Update()
	{
		Vector3 position = GameUIManager.mInstance.uiCamera.camera.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(this.initPosition));
		position.z = 0f;
		base.transform.position = position;
	}
}
