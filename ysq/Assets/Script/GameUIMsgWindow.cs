using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class GameUIMsgWindow : MonoBehaviour
{
	public AnimationCurve PushTxtCurve;

	public AnimationCurve ExpandCurve;

	[NonSerialized]
	public float PushTxtDuration = 0.3f;

	[NonSerialized]
	public float ExpandDuration = 0.4f;

	private UISprite mBG;

	private bool expand;

	private UILabel mMsgLabel;

	private UISprite mArrowBtn;

	private UIPanel mPanel;

	private UITable mContent;

	private UIScrollBar mScrollBar;

	private Queue<string> txt = new Queue<string>();

	private StringBuilder mStringBuilder = new StringBuilder();

	private int top = 301;

	private int bottom = 64;

	private bool hasInit;

	private float oldBarValue;

	private int maxMsgLabelVisibleSizeY = 280;

	public void Init()
	{
		if (this.PushTxtDuration <= 0f)
		{
			this.PushTxtDuration = 1f;
		}
		if (this.ExpandDuration <= 0f)
		{
			this.ExpandDuration = 1f;
		}
		this.mBG = GameUITools.FindUISprite("BG", base.gameObject);
		this.mArrowBtn = GameUITools.RegisterClickEvent("ArrowBtn", new UIEventListener.VoidDelegate(this.OnMsgArrowBtnClick), base.gameObject).GetComponent<UISprite>();
		this.mScrollBar = GameUITools.FindGameObject("BgPanelScrollBar", base.gameObject).GetComponent<UIScrollBar>();
		this.mPanel = GameUITools.FindGameObject("Panel", base.gameObject).GetComponent<UIPanel>();
		this.mContent = GameUITools.FindGameObject("Content", this.mPanel.gameObject).GetComponent<UITable>();
		this.mMsgLabel = GameUITools.FindUILabel("MsgLabel", this.mContent.gameObject);
		this.mMsgLabel.text = string.Empty;
	}

	public void SetAnchor(Vector4 anchor)
	{
		this.mBG.leftAnchor.absolute = (int)anchor.x;
		this.mBG.rightAnchor.absolute = (int)anchor.y;
		this.mBG.bottomAnchor.absolute = (int)anchor.z;
		this.mBG.topAnchor.absolute = (int)anchor.w;
		this.top = this.mBG.topAnchor.absolute + 237;
		this.bottom = this.mBG.topAnchor.absolute;
	}

	public void SetBGColor(Color32 col)
	{
		this.mBG.color = col;
	}

	private void OnMsgArrowBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.expand = !this.expand;
		this.SwitchMode(true);
	}

	private void RefreshMsgs(bool playAnim = true)
	{
		if (this.txt.Count > 40)
		{
			for (int i = 0; i < this.txt.Count - 30 - 10; i++)
			{
				this.txt.Dequeue();
			}
		}
		if (!playAnim)
		{
			for (int j = 0; j < this.txt.Count - 30; j++)
			{
				this.txt.Dequeue();
			}
		}
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		foreach (string current in this.txt)
		{
			this.mStringBuilder.AppendLine(current);
		}
		this.oldBarValue = this.mScrollBar.value;
		this.mMsgLabel.text = this.mStringBuilder.ToString().TrimEnd(new char[]
		{
			'\n'
		});
		base.StartCoroutine(this.Reposition());
		if (playAnim)
		{
			base.StartCoroutine(this.PlayPushMsgAnim());
		}
	}

	[DebuggerHidden]
	private IEnumerator Reposition()
	{
        return null;
        //GameUIMsgWindow.<Reposition>c__Iterator37 <Reposition>c__Iterator = new GameUIMsgWindow.<Reposition>c__Iterator37();
        //<Reposition>c__Iterator.<>f__this = this;
        //return <Reposition>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator PlayPushMsgAnim()
	{
        return null;
        //GameUIMsgWindow.<PlayPushMsgAnim>c__Iterator38 <PlayPushMsgAnim>c__Iterator = new GameUIMsgWindow.<PlayPushMsgAnim>c__Iterator38();
        //<PlayPushMsgAnim>c__Iterator.<>f__this = this;
        //return <PlayPushMsgAnim>c__Iterator;
	}

	private void DequeueTxt()
	{
		for (int i = 0; i < this.txt.Count - 30; i++)
		{
			this.txt.Dequeue();
		}
		this.RefreshMsgs(false);
	}

	private void SwitchMode(bool isPlayAnim = true)
	{
		if (this.expand)
		{
			this.mArrowBtn.transform.localRotation = Quaternion.identity;
			if (isPlayAnim)
			{
				this.oldBarValue = this.mScrollBar.value;
				HOTween.To(this.mBG.topAnchor, this.ExpandDuration, new TweenParms().Prop("absolute", this.top).Ease((this.ExpandCurve.keys.Length <= 0) ? null : this.ExpandCurve).OnUpdate(new TweenDelegate.TweenCallbackWParms(this.OnTweenUpdate), new object[0]));
			}
			else
			{
				this.mBG.topAnchor.absolute = this.top;
			}
		}
		else
		{
			this.mArrowBtn.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
			if (isPlayAnim)
			{
				this.oldBarValue = this.mScrollBar.value;
				HOTween.To(this.mBG.topAnchor, this.ExpandDuration, new TweenParms().Prop("absolute", this.bottom).Ease((this.ExpandCurve.keys.Length <= 0) ? null : this.ExpandCurve));
				HOTween.To(this.mScrollBar, this.ExpandDuration, new TweenParms().Prop("value", (this.mMsgLabel.printedSize.y >= (float)this.maxMsgLabelVisibleSizeY) ? this.oldBarValue : 1f).Ease((this.ExpandCurve.keys.Length <= 0) ? null : this.ExpandCurve));
			}
			else
			{
				this.mBG.topAnchor.absolute = this.bottom;
			}
		}
	}

	private void OnTweenUpdate(TweenEvent e)
	{
		if (this.mMsgLabel.printedSize.y < (float)this.maxMsgLabelVisibleSizeY)
		{
			this.mScrollBar.value = 0f;
		}
		else
		{
			this.mScrollBar.value = this.oldBarValue;
		}
	}

	private void OnTweenUpdate2(TweenEvent e)
	{
		if (this.mMsgLabel.printedSize.y < (float)this.maxMsgLabelVisibleSizeY)
		{
			this.mScrollBar.value = 1f;
		}
		else
		{
			this.mScrollBar.value = this.oldBarValue;
		}
	}

	public void PushMsg(string msg, bool playAnim = true)
	{
		if (!base.gameObject.activeInHierarchy || string.IsNullOrEmpty(msg))
		{
			return;
		}
		this.txt.Enqueue(msg);
		this.RefreshMsgs(playAnim);
	}

	public void PushMsgs(List<string> msgs, bool playAnim = true)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		for (int i = 0; i < msgs.Count; i++)
		{
			if (!string.IsNullOrEmpty(msgs[i]))
			{
				this.txt.Enqueue(msgs[i]);
			}
		}
		this.RefreshMsgs(playAnim);
	}

	public void Clear()
	{
		this.txt.Clear();
		this.RefreshMsgs(false);
	}

	public void InitMsgs(List<string> msgs)
	{
		this.PushMsgs(msgs, false);
		base.Invoke("SetBarDown", 0.01f);
	}

	private void SetBarDown()
	{
		if (this.expand)
		{
			this.mScrollBar.value = 0f;
		}
		else
		{
			this.mScrollBar.value = 1f;
		}
	}

	public void Close()
	{
		this.mMsgLabel.text = string.Empty;
		this.txt.Clear();
		this.expand = false;
		this.SwitchMode(false);
		this.mScrollBar.value = 0f;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
