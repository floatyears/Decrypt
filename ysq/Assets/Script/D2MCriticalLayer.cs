using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class D2MCriticalLayer : MonoBehaviour
{
	private GameObject mBg;

	private UISprite mCritiTxt;

	private Transform mCritiGo;

	private UISprite mCritiNum;

	private UISprite mCritiNum1;

	private Sequence mAnimSequence;

	public void InitWithBaseLayer()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBg = base.transform.Find("bg").gameObject;
		this.mCritiTxt = base.transform.Find("critiTxt").GetComponent<UISprite>();
		this.mCritiGo = base.transform.Find("numObj");
		this.mCritiNum = this.mCritiGo.Find("critiNum").GetComponent<UISprite>();
		this.mCritiNum1 = this.mCritiGo.Find("critiNum1").GetComponent<UISprite>();
		if (this.mAnimSequence == null)
		{
			this.mAnimSequence = new Sequence(new SequenceParms().AutoKill(false).OnComplete(new TweenDelegate.TweenCallback(this.OnSeqEnd)));
		}
		this.mAnimSequence.Append(HOTween.To(this.mBg.transform, 0.01f, new TweenParms().Prop("localScale", Vector3.zero)));
		this.mAnimSequence.Append(HOTween.To(this.mCritiTxt.transform, 0.01f, new TweenParms().Prop("localScale", new Vector3(4f, 4f, 1f))));
		this.mAnimSequence.Append(HOTween.To(this.mCritiGo, 0.01f, new TweenParms().Prop("localPosition", new Vector3(700f, 0f, 0f))));
		this.mAnimSequence.Append(HOTween.To(this.mCritiTxt.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInQuint)));
		this.mAnimSequence.Append(HOTween.To(this.mBg.transform, 0.1f, new TweenParms().Prop("localScale", Vector3.one)));
		this.mAnimSequence.Append(HOTween.To(this.mCritiGo, 0.25f, new TweenParms().Prop("localPosition", new Vector3(10f, 0f, 0f)).Ease(EaseType.EaseOutBounce)));
		this.mAnimSequence.AppendInterval(1f);
	}

	private void OnSeqEnd()
	{
		base.gameObject.SetActive(false);
	}

	public void Refresh(int critiNum)
	{
		int num = critiNum / 10;
		int num2 = critiNum % 10;
		if (num == 0)
		{
			this.mCritiNum.spriteName = string.Format("num{0}", num2);
			this.mCritiNum1.gameObject.SetActive(false);
		}
		else
		{
			this.mCritiNum.spriteName = string.Format("num{0}", num);
			this.mCritiNum1.gameObject.SetActive(true);
			this.mCritiNum1.spriteName = string.Format("num{0}", num2);
		}
	}

	public void PlayD2MCriticalAnim()
	{
		base.gameObject.SetActive(true);
		this.mAnimSequence.Restart();
	}

	public void DestroyEffect()
	{
		if (this.mAnimSequence != null)
		{
			this.mAnimSequence.Kill();
			this.mAnimSequence = null;
		}
	}
}
