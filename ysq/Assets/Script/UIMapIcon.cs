using NJG;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIMapIcon : UIMapIconBase
{
	public UISprite sprite;

	public UISprite border;

	private Color mColor;

	private TweenColor mTweenColor;

	private TweenScale mLoop;

	protected override void Start()
	{
		this.UnSelect();
		if (this.item.fadeOutAfterDelay == 0f)
		{
			this.sprite.alpha = 1f;
		}
		base.Start();
	}

	protected virtual void OnTooltip(bool show)
	{
		if (!string.IsNullOrEmpty(this.item.content))
		{
		}
	}

	protected virtual void OnHover(bool isOver)
	{
		if (isOver)
		{
			if (!this.isLooping)
			{
				TweenScale.Begin(this.sprite.cachedGameObject, 0.1f, this.onHoverScale);
			}
		}
		else if (!this.isLooping)
		{
			TweenScale.Begin(this.sprite.cachedGameObject, 0.3f, Vector3.one);
		}
	}

	public override void Select()
	{
		base.Select();
		if (this.border != null)
		{
			this.border.enabled = true;
		}
	}

	public override void UnSelect()
	{
		base.UnSelect();
		if (this.border != null)
		{
			this.border.enabled = false;
		}
	}

	private void OnClick()
	{
		this.Select();
	}

	private void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			this.Select();
		}
		else if (!Input.GetKey(KeyCode.LeftShift) && !this.item.forceSelection)
		{
			base.UnSelectAll();
		}
	}

	private void OnKey(KeyCode key)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && key == KeyCode.Escape)
		{
			this.OnSelect(false);
		}
	}

	protected override void OnVisible()
	{
		if (!this.isVisible)
		{
			if (this.item.fadeOutAfterDelay > 0f && !this.mFadingOut)
			{
				this.mFadingOut = true;
				base.StartCoroutine(base.DelayedFadeOut());
			}
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this.sprite.cachedGameObject, 1f, 1f);
			tweenAlpha.from = 0f;
			tweenAlpha.method = UITweener.Method.Linear;
			tweenAlpha.ResetToBeginning();
			if (!this.item.loopAnimation)
			{
				TweenScale tweenScale = TweenScale.Begin(this.sprite.cachedGameObject, 1f, Vector3.one);
				tweenScale.from = new Vector3(0.01f, 0.01f, 0.01f);
				tweenScale.method = UITweener.Method.BounceOut;
				tweenScale.ResetToBeginning();
			}
			this.isVisible = true;
		}
	}

	protected override void OnLoop()
	{
		if (this.item.loopAnimation)
		{
			this.isLooping = true;
			if (this.mLoop == null)
			{
				this.mLoop = TweenScale.Begin(this.sprite.cachedGameObject, 1f, Vector3.one);
				this.mLoop.from = Vector3.one * 1.5f;
				this.mLoop.style = UITweener.Style.PingPong;
				this.mLoop.method = UITweener.Method.Linear;
			}
		}
	}

	protected override void OnFadeOut()
	{
		if (this.mTweenColor == null)
		{
			this.mColor.a = 0f;
			this.mTweenColor = TweenColor.Begin(this.sprite.cachedGameObject, 1f, this.mColor);
			this.mColor.a = 1f;
			this.mTweenColor.from = this.mColor;
			this.mTweenColor.method = UITweener.Method.Linear;
		}
		else
		{
			this.mTweenColor.Play(true);
		}
		this.mFadingOut = false;
	}

	protected override void Update()
	{
		if (this.item == null)
		{
			return;
		}
		if (this.mSelected != this.item.isSelected)
		{
			this.mSelected = this.item.isSelected;
			if (this.mSelected)
			{
				this.Select();
			}
			else
			{
				this.UnSelect();
			}
		}
		if (this.item.showIcon && this.item.showOnAction)
		{
			this.OnVisible();
			this.item.showIcon = false;
		}
	}
}
