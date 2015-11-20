using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITweener : MonoBehaviour
{
	public enum Method
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut,
		BounceIn,
		BounceOut,
		QuadEaseOut,
		QuadEaseIn,
		QuadEaseInOut,
		QuadEaseOutIn,
		ExpoEaseOut,
		ExpoEaseIn,
		ExpoEaseInOut,
		ExpoEaseOutIn,
		CubicEaseOut,
		CubicEaseIn,
		CubicEaseInOut,
		CubicEaseOutIn,
		QuartEaseOut,
		QuartEaseIn,
		QuartEaseInOut,
		QuartEaseOutIn,
		QuintEaseOut,
		QuintEaseIn,
		QuintEaseInOut,
		QuintEaseOutIn,
		CircEaseOut,
		CircEaseIn,
		CircEaseInOut,
		CircEaseOutIn,
		SineEaseOut,
		SineEaseIn,
		SineEaseInOut,
		SineEaseOutIn,
		ElasticEaseOut,
		ElasticEaseIn,
		ElasticEaseInOut,
		ElasticEaseOutIn,
		BounceEaseOut,
		BounceEaseIn,
		BounceEaseInOut,
		BounceEaseOutIn,
		BackEaseOut,
		BackEaseIn,
		BackEaseInOut,
		BackEaseOutIn
	}

	public enum Style
	{
		Once,
		Loop,
		PingPong
	}

	public static UITweener current;

	[HideInInspector]
	public UITweener.Method method;

	[HideInInspector]
	public UITweener.Style style;

	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	[HideInInspector]
	public bool ignoreTimeScale = true;

	[HideInInspector]
	public bool fixedTime;

	public static float frameRateDelta = 0.0333333351f;

	[HideInInspector]
	public float delay;

	[HideInInspector]
	public float duration = 1f;

	[HideInInspector]
	public bool steeperCurves;

	[HideInInspector]
	public int tweenGroup;

	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private bool mStarted;

	private float mStartTime;

	private float mDuration;

	private float mAmountPerDelta = 1000f;

	private float mFactor;

	private float curTime;

	private List<EventDelegate> mTemp;

	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration)) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	public Direction direction
	{
		get
		{
			return (this.amountPerDelta >= 0f) ? Direction.Forward : Direction.Reverse;
		}
	}

	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	protected virtual void Start()
	{
		this.Update();
	}

	private void Update()
	{
		float num = (!this.fixedTime) ? ((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime) : UITweener.frameRateDelta;
		this.curTime = ((!this.fixedTime) ? ((!this.ignoreTimeScale) ? Time.time : RealTime.time) : (this.curTime + num));
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = this.curTime + this.delay;
		}
		if (this.curTime < this.mStartTime)
		{
			return;
		}
		this.mFactor += this.amountPerDelta * num;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			if (this.duration == 0f || (this.mFactor == 1f && this.mAmountPerDelta > 0f) || (this.mFactor == 0f && this.mAmountPerDelta < 0f))
			{
				base.enabled = false;
			}
			if (UITweener.current == null)
			{
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate eventDelegate = this.mTemp[i];
						if (eventDelegate != null && !eventDelegate.oneShot)
						{
							EventDelegate.Add(this.onFinished, eventDelegate, eventDelegate.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				UITweener.current = null;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	private void OnDisable()
	{
		this.mStarted = false;
	}

	private float BackEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
	}

	private float BackEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * (2.70158f * t + 1.70158f) + 1f) + b;
	}

	private float BackEaseInOut(float t, float b, float c, float d)
	{
		float num = 1.70158f;
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
		}
		return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
	}

	private float BackEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.BackEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.BackEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float BounceEaseInOut(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.BounceEaseIn(t * 2f, 0f, c, d) * 0.5f + b;
		}
		return this.BounceEaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
	}

	private float BounceEaseIn(float t, float b, float c, float d)
	{
		return c - this.BounceEaseOut(d - t, 0f, c, d) + b;
	}

	private float BounceEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.BounceEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.BounceEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float BounceEaseOut(float t, float b, float c, float d)
	{
		if ((t /= d) < 0.363636374f)
		{
			return c * (7.5625f * t * t) + b;
		}
		if (t < 0.727272749f)
		{
			return c * (7.5625f * (t -= 0.545454562f) * t + 0.75f) + b;
		}
		if (t < 0.909090936f)
		{
			return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
		}
		return c * (7.5625f * (t -= 0.954545438f) * t + 0.984375f) + b;
	}

	private float CircEaseOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
	}

	private float CircEaseIn(float t, float b, float c, float d)
	{
		return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
	}

	private float CircEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
		}
		return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
	}

	private float CircEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.CircEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.CircEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float CubicEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t + b;
	}

	private float CubicEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * t + 1f) + b;
	}

	private float CubicEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t + 2f) + b;
	}

	private float CubicEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.CubicEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.CubicEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float ElasticEaseOut(float t, float b, float c, float d)
	{
		if ((t /= d) == 1f)
		{
			return b + c;
		}
		float num = d * 0.3f;
		float num2 = num / 4f;
		return c * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num2) * 6.28318548f / num) + c + b;
	}

	private float ElasticEaseIn(float t, float b, float c, float d)
	{
		if ((t /= d) == 1f)
		{
			return b + c;
		}
		float num = d * 0.3f;
		float num2 = num / 4f;
		return -(c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num)) + b;
	}

	private float ElasticEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) == 2f)
		{
			return b + c;
		}
		float num = d * 0.450000018f;
		float num2 = num / 4f;
		if (t < 1f)
		{
			return -0.5f * (c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num)) + b;
		}
		return c * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num) * 0.5f + c + b;
	}

	private float ElasticEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.ElasticEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.ElasticEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float ExpoEaseOut(float t, float b, float c, float d)
	{
		return (t != d) ? (c * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b) : (b + c);
	}

	private float ExpoEaseIn(float t, float b, float c, float d)
	{
		return (t != 0f) ? (c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b) : b;
	}

	private float ExpoEaseInOut(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if (t == d)
		{
			return b + c;
		}
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
		}
		return c / 2f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
	}

	private float ExpoEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.ExpoEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.ExpoEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float QuadEaseOut(float t, float b, float c, float d)
	{
		return -c * (t /= d) * (t - 2f) + b;
	}

	private float QuadEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t + b;
	}

	private float QuadEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t + b;
		}
		return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
	}

	private float QuadEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.QuadEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.QuadEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float QuartEaseOut(float t, float b, float c, float d)
	{
		return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
	}

	private float QuartEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t + b;
	}

	private float QuartEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t * t + b;
		}
		return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
	}

	private float QuartEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.QuartEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.QuartEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float QuintEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
	}

	private float QuintEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t * t + b;
	}

	private float QuintEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
	}

	private float QuintEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.QuintEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.QuintEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float SineEaseOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sin(t / d * 1.57079637f) + b;
	}

	private float SineEaseIn(float t, float b, float c, float d)
	{
		return -c * Mathf.Cos(t / d * 1.57079637f) + c + b;
	}

	private float SineEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * Mathf.Sin(3.14159274f * t / 2f) + b;
		}
		return -c / 2f * (Mathf.Cos(3.14159274f * (t -= 1f) / 2f) - 2f) + b;
	}

	private float SineEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return this.SineEaseOut(t * 2f, b, c / 2f, d);
		}
		return this.SineEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	private float GetEaseProgress(float linear_progress)
	{
		switch (this.method)
		{
		case UITweener.Method.Linear:
			return linear_progress;
		case UITweener.Method.QuadEaseOut:
			return this.QuadEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuadEaseIn:
			return this.QuadEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuadEaseInOut:
			return this.QuadEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuadEaseOutIn:
			return this.QuadEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ExpoEaseOut:
			return this.ExpoEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ExpoEaseIn:
			return this.ExpoEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ExpoEaseInOut:
			return this.ExpoEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ExpoEaseOutIn:
			return this.ExpoEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CubicEaseOut:
			return this.CubicEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CubicEaseIn:
			return this.CubicEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CubicEaseInOut:
			return this.CubicEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CubicEaseOutIn:
			return this.CubicEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuartEaseOut:
			return this.QuartEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuartEaseIn:
			return this.QuartEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuartEaseInOut:
			return this.QuartEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuartEaseOutIn:
			return this.QuartEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuintEaseOut:
			return this.QuintEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuintEaseIn:
			return this.QuintEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuintEaseInOut:
			return this.QuintEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.QuintEaseOutIn:
			return this.QuintEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CircEaseOut:
			return this.CircEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CircEaseIn:
			return this.CircEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CircEaseInOut:
			return this.CircEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.CircEaseOutIn:
			return this.CircEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.SineEaseOut:
			return this.SineEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.SineEaseIn:
			return this.SineEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.SineEaseInOut:
			return this.SineEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.SineEaseOutIn:
			return this.SineEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ElasticEaseOut:
			return this.ElasticEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ElasticEaseIn:
			return this.ElasticEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ElasticEaseInOut:
			return this.ElasticEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.ElasticEaseOutIn:
			return this.ElasticEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BounceEaseOut:
			return this.BounceEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BounceEaseIn:
			return this.BounceEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BounceEaseInOut:
			return this.BounceEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BounceEaseOutIn:
			return this.BounceEaseOutIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BackEaseOut:
			return this.BackEaseOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BackEaseIn:
			return this.BackEaseIn(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BackEaseInOut:
			return this.BackEaseInOut(linear_progress, 0f, 1f, 1f);
		case UITweener.Method.BackEaseOutIn:
			return this.BackEaseOutIn(linear_progress, 0f, 1f, 1f);
		}
		return linear_progress;
	}

	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.57079637f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.57079637f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.28318548f) / 6.28318548f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		else
		{
			num = this.GetEaseProgress(num);
		}
		this.OnUpdate((this.animationCurve == null) ? num : this.animationCurve.Evaluate(num), isFinished);
	}

	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	public void PlayForward()
	{
		this.Play(true);
	}

	public void PlayReverse()
	{
		this.Play(false);
	}

	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = ((this.amountPerDelta >= 0f) ? 0f : 1f);
		this.Sample(this.mFactor, false);
	}

	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	protected abstract void OnUpdate(float factor, bool isFinished);

	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t != null && t.tweenGroup != 0)
		{
			t = (T)((object)null);
			T[] components = go.GetComponents<T>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				t = components[i];
				if (t != null && t.tweenGroup == 0)
				{
					break;
				}
				t = (T)((object)null);
				i++;
			}
		}
		if (t == null)
		{
			t = go.AddComponent<T>();
			if (t == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Concat(new object[]
					{
						"Unable to add ",
						typeof(T),
						" to ",
						NGUITools.GetHierarchy(go)
					}),
					go
				});
				return (T)((object)null);
			}
		}
		t.mStarted = false;
		t.duration = duration;
		t.mFactor = 0f;
		t.mAmountPerDelta = Mathf.Abs(t.amountPerDelta);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.enabled = true;
		return t;
	}

	public virtual void SetStartToCurrentValue()
	{
	}

	public virtual void SetEndToCurrentValue()
	{
	}
}
