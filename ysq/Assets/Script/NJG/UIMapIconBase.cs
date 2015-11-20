using Holoville.HOTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace NJG
{
	public class UIMapIconBase : MonoBehaviour
	{
		private static List<UIMapIconBase> selected = new List<UIMapIconBase>();

		public NJGMapItem item;

		public bool isValid;

		public bool isMapIcon = true;

		public bool isVisible;

		public new BoxCollider collider;

		private Transform mTrans;

		protected bool isLooping;

		protected bool isScaling;

		protected Vector3 onHoverScale = new Vector3(1.3f, 1.3f, 1.3f);

		protected TweenParms tweenParms = new TweenParms();

		private Tweener mLoop;

		private float mAlpha = 1f;

		protected bool mFadingOut;

		protected bool mSelected;

		public Transform cachedTransform
		{
			get
			{
				if (this.mTrans == null)
				{
					this.mTrans = base.transform;
				}
				return this.mTrans;
			}
		}

		public float alpha
		{
			get
			{
				return this.mAlpha;
			}
			set
			{
				this.mAlpha = value;
			}
		}

		protected virtual void Start()
		{
			if (Application.isPlaying)
			{
				this.CheckAnimations();
			}
		}

		protected virtual void Update()
		{
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

		public virtual void Select()
		{
			if (!Input.GetKey(KeyCode.LeftShift) && !this.item.forceSelection)
			{
				this.UnSelectAll();
			}
			this.item.isSelected = true;
			if (!UIMapIconBase.selected.Contains(this))
			{
				UIMapIconBase.selected.Add(this);
			}
		}

		public virtual void UnSelect()
		{
		}

		protected void UnSelectAll()
		{
			int i = 0;
			int count = UIMapIconBase.selected.Count;
			while (i < count)
			{
				UIMapIconBase uIMapIconBase = UIMapIconBase.selected[i];
				uIMapIconBase.UnSelect();
				i++;
			}
			UIMapIconBase.selected.Clear();
		}

		protected void CheckAnimations()
		{
			this.alpha = 1f;
			if (this.item != null)
			{
				if (this.item.showOnAction)
				{
					this.cachedTransform.localScale = Vector3.zero;
				}
				else if (this.item.fadeOutAfterDelay > 0f)
				{
					if (!this.mFadingOut)
					{
						this.mFadingOut = true;
						base.StartCoroutine(this.DelayedFadeOut());
					}
				}
				else if (this.item.loopAnimation)
				{
					this.OnLoop();
				}
				else if (this.item.animateOnVisible && !this.isMapIcon && this.item.fadeOutAfterDelay == 0f)
				{
					this.OnVisible();
				}
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				if (this.mLoop != null && !this.item.loopAnimation)
				{
					this.mLoop.Kill();
				}
				this.cachedTransform.localScale = Vector3.one;
				this.CheckAnimations();
			}
		}

		private void OnDisable()
		{
			if (this.mFadingOut)
			{
				this.mFadingOut = false;
				base.StopAllCoroutines();
			}
			if (Application.isPlaying && this.item != null)
			{
				if (this.item.loopAnimation)
				{
					if (this.mLoop != null && !this.mLoop.isPaused)
					{
						this.mLoop.Pause();
					}
				}
				else if (this.mLoop != null)
				{
					this.mLoop.Kill();
				}
			}
			if (this.isVisible)
			{
				this.isVisible = false;
			}
		}

		protected virtual void OnVisible()
		{
			if (!this.isVisible)
			{
				if (this.item.fadeOutAfterDelay > 0f && !this.mFadingOut)
				{
					this.mFadingOut = true;
					base.StartCoroutine(this.DelayedFadeOut());
				}
				if (!this.item.loopAnimation)
				{
					this.cachedTransform.localScale = Vector3.one * 0.01f;
					this.tweenParms.Prop("localScale", Vector3.one).Ease(EaseType.EaseInOutElastic);
					HOTween.To(this.cachedTransform, 1f, this.tweenParms);
				}
				this.isVisible = true;
			}
		}

		protected virtual void OnLoop()
		{
			if (this.item.loopAnimation)
			{
				this.isLooping = true;
				if (this.mLoop == null)
				{
					this.cachedTransform.localScale = Vector3.one * 1.5f;
					this.mLoop = HOTween.To(this.cachedTransform, 0.5f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.Linear).Loops(-1, LoopType.Yoyo).IntId(999));
				}
				else if (this.mLoop.isPaused)
				{
					this.mLoop.Play();
				}
			}
		}

		[DebuggerHidden]
		protected IEnumerator DelayedFadeOut()
		{
            return null;
            //UIMapIconBase.<DelayedFadeOut>c__IteratorAC <DelayedFadeOut>c__IteratorAC = new UIMapIconBase.<DelayedFadeOut>c__IteratorAC();
            //<DelayedFadeOut>c__IteratorAC.<>f__this = this;
            //return <DelayedFadeOut>c__IteratorAC;
		}

		protected virtual void OnFadeOut()
		{
			Tweener tweener = HOTween.To(this, 0.9f, "alpha", 0);
			tweener.easeType = EaseType.Linear;
			this.mFadingOut = false;
		}
	}
}
