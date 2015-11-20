using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class CriticalPanel : MonoBehaviour
{
	private UIPanel mPanel;

	private GameObject mEffectBg;

	private GameObject mCriticalBg;

	private UILabel mAttTxt;

	private GameObject mImproveBg;

	private UISprite mImproveNum;

	private UISprite mSingle;

	private GameObject mCriticalBgEndEffect;

	private Sequence mSequenceForImprove;

	private Sequence mSequenceForCritical;

	private int mEnhanceNum;

	private int mCriticalCount;

	public bool mFirstOpen = true;

	public void InitCriticalLayer()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		base.gameObject.SetActive(true);
		this.mPanel = base.transform.GetComponent<UIPanel>();
		this.mPanel.enabled = false;
		this.mEffectBg = base.transform.Find("effectBg").gameObject;
		this.mCriticalBg = base.transform.Find("criticalBg").gameObject;
		this.mImproveBg = base.transform.Find("improveBg").gameObject;
		this.mImproveNum = base.transform.Find("improveBg/improveTxt").GetComponent<UISprite>();
		this.mSingle = this.mImproveNum.transform.Find("single").GetComponent<UISprite>();
		this.mAttTxt = GameUITools.FindUILabel("AttTxt", this.mImproveBg.gameObject);
		this.mCriticalBgEndEffect = base.transform.Find("ui12").gameObject;
		Tools.SetParticleRQWithUIScale(this.mCriticalBgEndEffect, 4700);
		this.mCriticalBgEndEffect.SetActive(false);
		this.mSequenceForCritical = new Sequence(new SequenceParms().AutoKill(false));
		this.mSequenceForCritical.AppendInterval(0.4f);
		this.mSequenceForCritical.Append(HOTween.To(this.mEffectBg.transform, 0.001f, new TweenParms().Prop("localScale", Vector3.zero).OnComplete(new TweenDelegate.TweenCallback(this.OnEffectAnimEnd)).OnStart(new TweenDelegate.TweenCallback(this.OnEffectBgS))));
		this.mSequenceForCritical.Append(HOTween.To(this.mCriticalBg.transform, 0.001f, new TweenParms().Prop("localScale", new Vector3(4f, 4f, 0f)).OnStart(new TweenDelegate.TweenCallback(this.OnCriticalBgS))));
		this.mSequenceForCritical.Append(HOTween.To(this.mImproveBg.transform, 0.001f, new TweenParms().Prop("localPosition", new Vector3(1132f, -10f, 0f)).Prop("localScale", Vector3.one).OnStart(new TweenDelegate.TweenCallback(this.OnImproveBgS))));
		this.mSequenceForCritical.Append(HOTween.To(this.mImproveNum.transform, 0.001f, new TweenParms().Prop("localScale", Vector3.one)));
		this.mSequenceForCritical.Append(HOTween.To(this.mCriticalBg.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 0f)).Ease(EaseType.EaseInQuint).OnComplete(new TweenDelegate.TweenCallback(this.OnCriticalBgAnimEnd))));
		this.mSequenceForCritical.Append(HOTween.To(this.mEffectBg.transform, 0.01f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 0f))));
		this.mSequenceForCritical.Append(HOTween.To(this.mImproveBg.transform, 0.25f, new TweenParms().Prop("localPosition", new Vector3(85f, -10f, 0f)).Ease(EaseType.EaseOutBounce)));
		this.mSequenceForCritical.Append(HOTween.To(this.mImproveNum.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(3f, 3f, 0f)).Ease(EaseType.EaseOutExpo)));
		this.mSequenceForCritical.Append(HOTween.To(this.mImproveNum.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 0f)).Ease(EaseType.EaseInExpo)));
		this.mSequenceForImprove = new Sequence(new SequenceParms().AutoKill(false));
		this.mSequenceForImprove.AppendInterval(0.4f);
		this.mSequenceForImprove.Append(HOTween.To(this.mImproveBg.transform, 0.001f, new TweenParms().Prop("localPosition", new Vector3(-40f, -10f, 0f)).Prop("localScale", new Vector3(3f, 3f, 0f)).OnStart(new TweenDelegate.TweenCallback(this.OnImproveBgS))));
		this.mSequenceForImprove.Append(HOTween.To(this.mImproveNum.transform, 0.001f, new TweenParms().Prop("localScale", Vector3.zero)));
		this.mSequenceForImprove.Append(HOTween.To(this.mImproveBg.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 0f)).Ease(EaseType.EaseInQuint)));
		this.mSequenceForImprove.Append(HOTween.To(this.mImproveNum.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(3f, 3f, 0f)).Ease(EaseType.EaseOutExpo)));
		this.mSequenceForImprove.Append(HOTween.To(this.mImproveNum.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInExpo)));
	}

	private void OnEffectAnimEnd()
	{
		NGUITools.SetActive(this.mCriticalBgEndEffect, false);
	}

	private void OnCriticalBgAnimEnd()
	{
		NGUITools.SetActive(this.mCriticalBgEndEffect, true);
	}

	private void OnEffectBgS()
	{
		this.mEffectBg.transform.localScale = Vector3.one;
	}

	private void OnCriticalBgS()
	{
		this.mCriticalBg.transform.localScale = Vector3.one;
	}

	private void OnImproveBgS()
	{
		this.mImproveBg.transform.localScale = Vector3.one;
	}

	public void PlayImproveEffectAnimation()
	{
		this.mAttTxt.transform.localPosition = new Vector3(20f, -98f, 0f);
		this.mEffectBg.transform.localScale = Vector3.zero;
		this.mCriticalBg.transform.localScale = Vector3.zero;
		this.mImproveBg.transform.localScale = Vector3.zero;
		Globals.Instance.EffectSoundMgr.Play("ui/ui_008");
		this.mSequenceForImprove.Restart();
	}

	public void PlayCriticalEffectAnimation()
	{
		this.mAttTxt.transform.localPosition = new Vector3(-100f, -98f, 0f);
		this.mEffectBg.transform.localScale = Vector3.zero;
		this.mCriticalBg.transform.localScale = Vector3.zero;
		this.mImproveBg.transform.localScale = Vector3.zero;
		Globals.Instance.EffectSoundMgr.Play("ui/ui_008");
		this.mSequenceForCritical.Restart();
	}

	private void OnDisable()
	{
		this.mFirstOpen = true;
		NGUITools.SetActive(this.mCriticalBgEndEffect, false);
	}

	public void EnableCriticalLayer(bool isEnable)
	{
		this.mPanel.enabled = isEnable;
		if (isEnable)
		{
			if (this.mCriticalCount > 0)
			{
				if (!this.mFirstOpen)
				{
					this.PlayCriticalEffectAnimation();
				}
			}
			else if (!this.mFirstOpen)
			{
				this.PlayImproveEffectAnimation();
			}
		}
	}

	public void DestroyAnimSequence()
	{
		if (this.mSequenceForCritical != null)
		{
			this.mSequenceForCritical.Kill();
			this.mSequenceForCritical = null;
		}
		if (this.mSequenceForImprove != null)
		{
			this.mSequenceForImprove.Kill();
			this.mSequenceForImprove = null;
		}
	}

	public void SetImproveNum(int enhanceNum, int criticalCount)
	{
		this.mEnhanceNum = enhanceNum;
		this.mCriticalCount = criticalCount;
		if (enhanceNum > 10)
		{
			this.mSingle.enabled = true;
			this.mSingle.spriteName = string.Format("num{0}", this.mEnhanceNum % 10);
			this.mImproveNum.spriteName = string.Format("num{0}", this.mEnhanceNum / 10);
			this.mImproveNum.transform.localScale = Vector3.zero;
		}
		else
		{
			if (this.mEnhanceNum <= 0)
			{
				this.mEnhanceNum = 1 + this.mCriticalCount;
			}
			this.mSingle.enabled = false;
			this.mImproveNum.spriteName = string.Format("num{0}", this.mEnhanceNum);
			this.mImproveNum.transform.localScale = Vector3.zero;
		}
	}

	public void SetImproveAttTxt(string attTxt)
	{
		this.mAttTxt.text = attTxt;
	}
}
