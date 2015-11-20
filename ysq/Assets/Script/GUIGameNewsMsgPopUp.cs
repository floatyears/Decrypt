using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameNewsMsgPopUp : MonoBehaviour
{
	private bool mIsInit;

	private bool mIsDestroy;

	private Queue<KeyValuePair<string, float>> mNeedShowNews = new Queue<KeyValuePair<string, float>>();

	private bool mIsShowNew;

	private UIPanel mPanel;

	private UISprite mMsgBg;

	private UILabel mMsgTxt;

	private float mDestroyTimer;

	private float delayTime = 1f;

	private KeyValuePair<string, float> tempMsg;

	private float mAnimTime;

	public void ShowNew(string content, float delay = 1f, float animTime = 0.25f)
	{
		this.mAnimTime = animTime;
		if (this.mMsgTxt == null)
		{
			this.CreateObjects();
		}
		this.mNeedShowNews.Enqueue(new KeyValuePair<string, float>(content, (delay <= 0f) ? 1f : delay));
	}

	public void ShowNews(List<string> contents, float delay = 1f)
	{
		if (contents.Count != 0)
		{
			if (this.mMsgTxt == null)
			{
				this.CreateObjects();
			}
			for (int i = 0; i < contents.Count; i++)
			{
				this.mNeedShowNews.Enqueue(new KeyValuePair<string, float>(contents[i], (delay <= 0f) ? 1f : delay));
			}
		}
	}

	public void ClearNews()
	{
		this.mNeedShowNews.Clear();
	}

	public int GetNewsCount()
	{
		if (this.mNeedShowNews == null)
		{
			return 0;
		}
		return this.mNeedShowNews.Count;
	}

	private void CreateObjects()
	{
		this.mPanel = base.gameObject.GetComponent<UIPanel>();
		this.mMsgBg = base.transform.Find("bg").GetComponent<UISprite>();
		this.mMsgTxt = base.transform.Find("msg").GetComponent<UILabel>();
		this.mIsInit = true;
		this.mIsDestroy = false;
	}

	private void LateUpdate()
	{
		if (this.mIsInit && !this.mIsDestroy)
		{
			if (this.mNeedShowNews.Count == 0)
			{
				if (!this.mIsShowNew)
				{
					if (Time.time - this.mDestroyTimer > 1f)
					{
						this.mMsgBg.gameObject.SetActive(false);
					}
					if (Time.time - this.mDestroyTimer > 5f)
					{
						this.mIsDestroy = true;
						GameUIManager.mInstance.DestroyGameNewsMsgPopUp();
					}
				}
			}
			else
			{
				if (!this.mMsgBg.gameObject.activeInHierarchy)
				{
					this.mMsgBg.gameObject.SetActive(true);
				}
				if (!this.mIsShowNew)
				{
					this.mIsShowNew = true;
					this.tempMsg = this.mNeedShowNews.Dequeue();
					this.mMsgTxt.text = this.tempMsg.Key;
					this.delayTime = this.tempMsg.Value;
					this.mPanel.alpha = 1f;
					base.transform.localScale = Vector3.zero;
					HOTween.To(base.transform, this.mAnimTime, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack).OnComplete(new TweenDelegate.TweenCallback(this.OnOpenAnimEnd)));
				}
			}
		}
	}

	private void OnOpenAnimEnd()
	{
		HOTween.To(this.mPanel, this.mAnimTime, new TweenParms().Prop("alpha", 0).Delay(this.delayTime).UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
	}

	private void OnAnimEnd()
	{
		this.mIsShowNew = false;
		if (this.mNeedShowNews.Count == 0)
		{
			this.mDestroyTimer = Time.time;
		}
	}
}
