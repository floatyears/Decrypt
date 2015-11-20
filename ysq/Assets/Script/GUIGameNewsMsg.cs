using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameNewsMsg : MonoBehaviour
{
	public const int DEFAULT_TOP_ANCHOR = -78;

	private static int topAnchor = -78;

	private bool mIsInit;

	public bool mIsDestroy;

	private List<GUIGameNewData> mNeedShowNews = new List<GUIGameNewData>();

	private bool mIsShowNew;

	private UISprite mMsgBg;

	private UIWidget mMsgCon;

	private UILabel mMsgTxt;

	private Transform mMsgTrans;

	private UIPanel mBasePanel;

	private UIPanel mContentsPanel;

	private TweenPosition tp;

	private float mDestroyTimer;

	public static void SetAnchors(int offestTop)
	{
		GUIGameNewsMsg.topAnchor = offestTop;
		GUIGameNewsMsg systemNoticeMsg = GameUIManager.mInstance.GetSystemNoticeMsg();
		if (systemNoticeMsg != null)
		{
			systemNoticeMsg.UpdateAchors();
		}
	}

	private void AddNewByPriority(string content, int speed, int priority)
	{
		bool flag = false;
		for (int i = this.mNeedShowNews.Count - 1; i >= 0; i--)
		{
			GUIGameNewData gUIGameNewData = this.mNeedShowNews[i];
			if (priority >= gUIGameNewData.Priority)
			{
				this.mNeedShowNews.Insert(i + 1, new GUIGameNewData(content, speed, priority));
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.mNeedShowNews.Add(new GUIGameNewData(content, speed, priority));
		}
	}

	public void ShowNew(string content, int speed, int priority, bool isNotice = false)
	{
		if (this.mMsgTxt == null)
		{
			this.CreateObjects();
		}
		this.SetPanelLayer(isNotice);
		this.AddNewByPriority(content, speed, priority);
	}

	private void SetPanelLayer(bool isNotice)
	{
		if (isNotice)
		{
			this.mBasePanel.depth = 5000;
			this.mBasePanel.renderQueue = UIPanel.RenderQueue.StartAt;
			this.mBasePanel.startingRenderQueue = 8000;
			this.mContentsPanel.depth = 5100;
			this.mContentsPanel.renderQueue = UIPanel.RenderQueue.StartAt;
			this.mContentsPanel.startingRenderQueue = 8100;
		}
		else
		{
			this.mBasePanel.depth = 550;
			this.mBasePanel.renderQueue = UIPanel.RenderQueue.Automatic;
			this.mContentsPanel.depth = 560;
			this.mContentsPanel.renderQueue = UIPanel.RenderQueue.Automatic;
		}
	}

	public void ShowNews(List<string> contents, int speed, int priority, bool isNotice = false)
	{
		if (contents.Count != 0)
		{
			if (this.mMsgTxt == null)
			{
				this.CreateObjects();
			}
			this.SetPanelLayer(isNotice);
			for (int i = 0; i < contents.Count; i++)
			{
				this.AddNewByPriority(contents[i], speed, priority);
			}
		}
	}

	public void ClearNews()
	{
		this.mNeedShowNews.Clear();
	}

	private void CreateObjects()
	{
		this.mMsgCon = base.transform.Find("msg").GetComponent<UIWidget>();
		this.mMsgBg = this.mMsgCon.transform.Find("bg").GetComponent<UISprite>();
		this.mMsgTrans = this.mMsgCon.transform.Find("contents/msg");
		this.mMsgTxt = this.mMsgTrans.GetComponent<UILabel>();
		this.tp = this.mMsgTrans.GetComponent<TweenPosition>();
		this.tp.enabled = false;
		EventDelegate.Add(this.tp.onFinished, new EventDelegate.Callback(this.OnAnimEnd));
		this.mIsInit = true;
		this.mIsDestroy = false;
		this.mBasePanel = base.gameObject.GetComponent<UIPanel>();
		this.mContentsPanel = this.mMsgTrans.transform.parent.gameObject.GetComponent<UIPanel>();
		this.UpdateAchors();
	}

	private void UpdateAchors()
	{
		if (this.mMsgCon != null)
		{
			this.mMsgCon.topAnchor.absolute = GUIGameNewsMsg.topAnchor;
			this.mMsgCon.bottomAnchor.absolute = GUIGameNewsMsg.topAnchor - 42;
		}
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
						this.mMsgCon.gameObject.SetActive(false);
					}
					if (Time.time - this.mDestroyTimer > 5f)
					{
						this.mIsDestroy = true;
						GameUIManager.mInstance.DestroyGameNewsMsg();
					}
				}
			}
			else
			{
				if (!this.mMsgCon.gameObject.activeInHierarchy)
				{
					this.mMsgCon.gameObject.SetActive(true);
				}
				if (!this.mIsShowNew)
				{
					this.mIsShowNew = true;
					this.mMsgTrans.localPosition = new Vector3(1024f, 0f, 0f);
					GUIGameNewData gUIGameNewData = this.mNeedShowNews[0];
					this.mMsgTxt.text = gUIGameNewData.MsgContent;
					float num = (float)this.mMsgBg.width / 2f;
					float x = -(num + this.mMsgTxt.printedSize.x);
					this.tp.duration = (this.mMsgTxt.printedSize.x + (float)this.mMsgBg.width) / (float)gUIGameNewData.MoveSpeed;
					this.tp.from = new Vector3(num, 0f, 0f);
					this.tp.to = new Vector3(x, 0f, 0f);
					this.tp.tweenFactor = 0f;
					this.tp.enabled = true;
					this.mNeedShowNews.RemoveAt(0);
				}
			}
		}
	}

	private void OnAnimEnd()
	{
		this.tp.enabled = false;
		this.mIsShowNew = false;
		if (this.mNeedShowNews.Count == 0)
		{
			this.mDestroyTimer = Time.time;
		}
	}
}
