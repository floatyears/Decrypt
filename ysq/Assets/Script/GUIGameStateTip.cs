using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using System;
using UnityEngine;

public sealed class GUIGameStateTip : MonoBehaviour
{
	public enum EGAMEING_STATE
	{
		NONE,
		START,
		BattleEnd,
		TimeUp,
		WaveNum,
		MAX
	}

	private GameObject mBeginGameTip;

	private GameObject mBeginGameTipBg;

	private GameObject mBeginGameTipEffect;

	private GameObject mBeginGameTipLabel;

	private GameObject mBattleEndTip;

	private GameObject mBattleEndTipBg;

	private GameObject mBattleEndLabel;

	private GameObject mTimeUpTip;

	private GameObject mTimeUpTipBg;

	private GameObject mTimeUpLabel;

	private GameObject mWaveTip;

	private GameObject mWaveTipBg;

	private GameObject mWaveTipNumPanel;

	private GameObject mWaveTxt0;

	private GameObject mWaveTxt1;

	private UISprite mWaveNum0;

	private UISprite mWaveNum1;

	private UISprite mWaveNum2;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBeginGameTip = base.transform.Find("beginBackground").gameObject;
		this.mBeginGameTipBg = this.mBeginGameTip.transform.Find("BackGround").gameObject;
		this.mBeginGameTipLabel = this.mBeginGameTip.transform.Find("Sprite").gameObject;
		this.mBeginGameTipEffect = this.mBeginGameTip.transform.Find("ui05").gameObject;
		NGUITools.SetActive(this.mBeginGameTipEffect, false);
		Tools.SetParticleRenderQueue(this.mBeginGameTipEffect, 4000, 1f);
		this.mBeginGameTip.gameObject.SetActive(false);
		this.mBattleEndTip = base.transform.Find("battleEndStateTip").gameObject;
		this.mBattleEndTipBg = this.mBattleEndTip.transform.Find("BackGround").gameObject;
		this.mBattleEndLabel = this.mBattleEndTip.transform.Find("Sprite").gameObject;
		this.mBattleEndTip.gameObject.SetActive(false);
		this.mTimeUpTip = base.transform.Find("TimeUpStateTip").gameObject;
		this.mTimeUpTipBg = this.mTimeUpTip.transform.Find("BackGround").gameObject;
		this.mTimeUpLabel = this.mTimeUpTip.transform.Find("Sprite").gameObject;
		this.mTimeUpTip.gameObject.SetActive(false);
		this.mWaveTip = base.transform.Find("WaveStateTip").gameObject;
		this.mWaveTipBg = this.mWaveTip.transform.Find("BackGround").gameObject;
		this.mWaveTipNumPanel = this.mWaveTip.transform.Find("numPanel").gameObject;
		this.mWaveNum0 = this.mWaveTipNumPanel.transform.Find("num0").GetComponent<UISprite>();
		this.mWaveNum1 = this.mWaveTipNumPanel.transform.Find("num1").GetComponent<UISprite>();
		this.mWaveNum2 = this.mWaveTipNumPanel.transform.Find("num2").GetComponent<UISprite>();
		this.mWaveTxt0 = this.mWaveTip.transform.Find("waveTxt0").gameObject;
		this.mWaveTxt1 = this.mWaveTip.transform.Find("waveTxt1").gameObject;
		this.mWaveTip.gameObject.SetActive(false);
	}

	public void ChangeState(GUIGameStateTip.EGAMEING_STATE game_state, int waveNum)
	{
		switch (game_state)
		{
		case GUIGameStateTip.EGAMEING_STATE.START:
			this.ShowBeginGameTip();
			break;
		case GUIGameStateTip.EGAMEING_STATE.BattleEnd:
			this.ShowBattleEndGameTip();
			break;
		case GUIGameStateTip.EGAMEING_STATE.TimeUp:
			this.ShowTimeUpGameTip();
			break;
		case GUIGameStateTip.EGAMEING_STATE.WaveNum:
			this.ShowWaveNumGameTip(waveNum);
			break;
		}
	}

	private void ShowBeginGameTip()
	{
		this.mBeginGameTip.gameObject.SetActive(true);
		HOTween.To(this.mBeginGameTipBg.transform, 0f, new TweenParms().Prop("localScale", Vector3.one));
		HOTween.To(this.mBeginGameTipLabel.transform, 0f, new TweenParms().Prop("localPosition", new Vector3((float)(-(float)Screen.width) * 0.5f, -25f, 0f)));
        HOTween.To(this.mBeginGameTipLabel.transform, 1f, new TweenParms().Prop("localPosition", new Vector3(0f, -25f, 0f)).Ease(EaseType.EaseOutBounce).OnStart(() =>
		{
			NGUITools.SetActive(this.mBeginGameTipEffect, true);
		}).OnComplete(new TweenDelegate.TweenCallback(this.OnLabelAnimEnd)).UpdateType(UpdateType.TimeScaleIndependentUpdate));
	}

	private void OnLabelAnimEnd()
	{
		HOTween.To(this.mBeginGameTipBg.transform, 0.25f, new TweenParms().Prop("localScale", new PlugVector3Y(0f)).OnComplete(new TweenDelegate.TweenCallback(this.OnBgAnimEnd)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(1f));
	}

	private void OnBgAnimEnd()
	{
		this.mBeginGameTip.gameObject.SetActive(false);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void ShowBattleEndGameTip()
	{
		this.mBattleEndTip.gameObject.SetActive(true);
		HOTween.To(this.mBattleEndTipBg.transform, 0f, new TweenParms().Prop("localScale", Vector3.one));
		HOTween.To(this.mBattleEndLabel.transform, 0f, new TweenParms().Prop("localPosition", new Vector3((float)(-(float)Screen.width) * 0.5f, -25f, 0f)));
		HOTween.To(this.mBattleEndLabel.transform, 1f, new TweenParms().Prop("localPosition", new Vector3(0f, -25f, 0f)).Ease(EaseType.EaseOutBounce).OnComplete(new TweenDelegate.TweenCallback(this.OnBattleEndLabelAnimEnd)).UpdateType(UpdateType.TimeScaleIndependentUpdate));
	}

	private void OnBattleEndLabelAnimEnd()
	{
		HOTween.To(this.mBattleEndTipBg.transform, 0.25f, new TweenParms().Prop("localScale", new PlugVector3Y(0f)).OnComplete(()=>
		{
			this.mBattleEndTip.gameObject.SetActive(false);
		}).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(1f));
	}

	private void ShowTimeUpGameTip()
	{
		this.mTimeUpTip.gameObject.SetActive(true);
		HOTween.To(this.mTimeUpTipBg.transform, 0f, new TweenParms().Prop("localScale", Vector3.one));
		HOTween.To(this.mTimeUpLabel.transform, 0f, new TweenParms().Prop("localPosition", new Vector3((float)(-(float)Screen.width) * 0.5f, -25f, 0f)));
		HOTween.To(this.mTimeUpLabel.transform, 1f, new TweenParms().Prop("localPosition", new Vector3(0f, -25f, 0f)).Ease(EaseType.EaseOutBounce).OnComplete(new TweenDelegate.TweenCallback(this.OnTimeUpLabelAnimEnd)).UpdateType(UpdateType.TimeScaleIndependentUpdate));
	}

	private void OnTimeUpLabelAnimEnd()
	{
        HOTween.To(this.mTimeUpTipBg.transform, 0.25f, new TweenParms().Prop("localScale", new PlugVector3Y(0f)).OnComplete(() =>
		{
			this.mTimeUpTip.gameObject.SetActive(false);
		}).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(1f));
	}

	private void ShowWaveNumGameTip(int waveNum)
	{
		this.mWaveTip.gameObject.SetActive(true);
		int num = waveNum / 100;
		int num2 = waveNum % 100 / 10;
		int num3 = waveNum % 100 % 10;
		if (num != 0)
		{
			this.mWaveNum1.gameObject.SetActive(true);
			this.mWaveNum2.gameObject.SetActive(true);
			this.mWaveNum0.spriteName = string.Format("num{0}", num);
			if (num == 1)
			{
				this.mWaveNum0.transform.localPosition = new Vector3(-105f, -94f, 0f);
			}
			else
			{
				this.mWaveNum0.transform.localPosition = new Vector3(-120f, -94f, 0f);
			}
			this.mWaveNum1.spriteName = string.Format("num{0}", num2);
			this.mWaveNum1.transform.localPosition = new Vector3(-20f, -94f, 0f);
			this.mWaveNum2.spriteName = string.Format("num{0}", num3);
		}
		else if (num2 != 0)
		{
			this.mWaveNum1.gameObject.SetActive(true);
			this.mWaveNum2.gameObject.SetActive(false);
			this.mWaveNum0.spriteName = string.Format("num{0}", num2);
			if (num2 == 1)
			{
				this.mWaveNum0.transform.localPosition = new Vector3(-58f, -97f, 0f);
			}
			else
			{
				this.mWaveNum0.transform.localPosition = new Vector3(-72f, -97f, 0f);
			}
			this.mWaveNum1.spriteName = string.Format("num{0}", num3);
			this.mWaveNum1.transform.localPosition = new Vector3(25f, -94f, 0f);
		}
		else
		{
			this.mWaveNum1.gameObject.SetActive(false);
			this.mWaveNum2.gameObject.SetActive(false);
			this.mWaveNum0.transform.localPosition = new Vector3(-20f, -94f, 0f);
			this.mWaveNum0.spriteName = string.Format("num{0}", num3);
		}
		this.mWaveTipNumPanel.transform.localScale = Vector3.zero;
		this.mWaveTxt0.transform.localPosition = new Vector3(-650f, 50f, 0f);
		this.mWaveTxt1.transform.localPosition = new Vector3(650f, 50f, 0f);
		HOTween.To(this.mWaveTipBg.transform, 0.001f, new TweenParms().Prop("localScale", Vector3.one));
		TweenPosition.Begin(this.mWaveTxt0, 0.15f, new Vector3(10f, 50f, 0f)).ignoreTimeScale = true;
		TweenPosition.Begin(this.mWaveTxt1, 0.15f, new Vector3(10f, 50f, 0f)).ignoreTimeScale = true;
		HOTween.To(this.mWaveTipNumPanel.transform, 0.001f, new TweenParms().Prop("localScale", new Vector3(10f, 10f, 10f)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(0.5f));
		HOTween.To(this.mWaveTipNumPanel.transform, 0.12f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutSine).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(0.51f));
        HOTween.To(this.mWaveTipBg.transform, 0.12f, new TweenParms().Prop("localScale", new PlugVector3Y(0f)).OnComplete(() =>
		{
			this.mWaveTip.gameObject.SetActive(false);
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(1.4f));
	}
}
