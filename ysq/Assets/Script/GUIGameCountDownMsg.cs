using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIGameCountDownMsg : MonoBehaviour
{
	private UISprite mCDNumSprite;

	private Sequence mSeqForCD;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void OnDestroy()
	{
		this.DestroySequence();
	}

	private void CreateObjects()
	{
		this.mCDNumSprite = base.transform.Find("numSprite").GetComponent<UISprite>();
		base.gameObject.SetActive(false);
	}

	public void DestroySequence()
	{
		if (this.mSeqForCD != null)
		{
			this.mSeqForCD.Kill();
			this.mSeqForCD = null;
		}
	}

	public void ShowCountDownMsg(int num)
	{
		this.DestroySequence();
		base.gameObject.SetActive(true);
		this.mSeqForCD = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		for (int i = num; i > 0; i--)
		{
			this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite.transform, 0.001f, new TweenParms().Prop("localScale", new Vector3(5f, 5f, 5f)).OnStart(new TweenDelegate.TweenCallbackWParms(this.OnNumAnimStart), new object[]
			{
				i
			})));
			this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite, 0.001f, new TweenParms().Prop("color", new Color(this.mCDNumSprite.color.r, this.mCDNumSprite.color.g, this.mCDNumSprite.color.b, 1f))));
			this.mSeqForCD.AppendInterval(0.2f);
			this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite.transform, 0.3f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInQuart)));
			this.mSeqForCD.AppendInterval(0.2f);
			this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite, 0.2f, new TweenParms().Prop("color", new Color(this.mCDNumSprite.color.r, this.mCDNumSprite.color.g, this.mCDNumSprite.color.b, 0f))));
			if (i == 1)
			{
				this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite.transform, 0.001f, new TweenParms().Prop("localScale", new Vector3(5f, 5f, 5f)).OnStart(new TweenDelegate.TweenCallbackWParms(this.OnBeginAnimStart), new object[0])));
				this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite, 0.001f, new TweenParms().Prop("color", new Color(this.mCDNumSprite.color.r, this.mCDNumSprite.color.g, this.mCDNumSprite.color.b, 1f))));
				this.mSeqForCD.AppendInterval(0.2f);
				this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite.transform, 0.3f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInQuart)));
				this.mSeqForCD.AppendInterval(0.2f);
				this.mSeqForCD.Append(HOTween.To(this.mCDNumSprite, 0.2f, new TweenParms().Prop("color", new Color(this.mCDNumSprite.color.r, this.mCDNumSprite.color.g, this.mCDNumSprite.color.b, 0f))));
				this.mSeqForCD.AppendCallback(()=>
				{
					GameUIManager.mInstance.CloseGameCDMsg();
				});
			}
		}
		this.mSeqForCD.Play();
	}

	private void OnNumAnimStart(TweenEvent e)
	{
		if (e.parms != null)
		{
			int num = (int)e.parms[0];
			this.mCDNumSprite.spriteName = string.Format("num{0}", num);
			this.mCDNumSprite.MakePixelPerfect();
		}
	}

	private void OnBeginAnimStart(TweenEvent e)
	{
		this.mCDNumSprite.spriteName = "Begin";
		this.mCDNumSprite.MakePixelPerfect();
	}
}
