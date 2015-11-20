using System;
using UnityEngine;

public class GameUIFadeInOut : MonoBehaviour
{
	public TweenAlpha mAlpha;

	private void Awake()
	{
		this.mAlpha = base.gameObject.transform.FindChild("Sprite").GetComponent<TweenAlpha>();
		EventDelegate.Add(this.mAlpha.onFinished, new EventDelegate.Callback(this.StopEffect));
	}

	public void StartEffect()
	{
		this.StartFadeInEffect();
	}

	public void StopEffect()
	{
		base.gameObject.SetActive(false);
	}

	public void PlayEffect(float time)
	{
		this.mAlpha.duration = time;
		base.gameObject.SetActive(true);
		this.mAlpha.Play(true);
	}

	public void StartFadeInEffect()
	{
		this.mAlpha.from = 1f;
		this.mAlpha.to = 0f;
		this.mAlpha.enabled = false;
		this.mAlpha.ResetToBeginning();
		base.gameObject.SetActive(true);
	}

	public void StartFadeOutEffect()
	{
		this.mAlpha.from = 0f;
		this.mAlpha.to = 1f;
		this.mAlpha.enabled = false;
		this.mAlpha.ResetToBeginning();
		base.gameObject.SetActive(true);
	}
}
