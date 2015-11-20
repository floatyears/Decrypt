using System;
using System.Collections.Generic;
using UnityEngine;

namespace NJG
{
	public abstract class UIMiniMapBase : UIMapBase
	{
		public enum ZoomType
		{
			ZoomIn,
			ZoomOut
		}

		private static UIMiniMapBase mInst;

		public KeyCode lockKey = KeyCode.L;

		public KeyCode mapKey = KeyCode.M;

		public int overlayBorderOffset;

		protected Vector2 mArrowScale;

		protected Transform mArrowRoot;

		protected Transform mMinimapFrame;

		protected List<NJGMapItem> mPingList = new List<NJGMapItem>();

		protected List<NJGMapItem> mPingUnused = new List<NJGMapItem>();

		protected List<UIMapArrowBase> mListArrow = new List<UIMapArrowBase>();

		protected List<UIMapArrowBase> mUnusedArrow = new List<UIMapArrowBase>();

		protected int mArrowCount;

		private UIMapBase.Pivot mPivot;

		private Vector2 mMargin;

		public static bool initialized
		{
			get
			{
				return UIMiniMapBase.mInst != null;
			}
		}

		public static UIMiniMapBase inst
		{
			get
			{
				return UIMiniMapBase.mInst;
			}
		}

		public Transform arrowRoot
		{
			get
			{
				if (this.mArrowRoot == null && Application.isPlaying)
				{
					this.mArrowRoot = NJGTools.AddChild(base.gameObject).transform;
					this.mArrowRoot.parent = base.iconRoot;
					this.mArrowRoot.name = "_MapArrows";
					this.mArrowRoot.localEulerAngles = Vector3.zero;
					this.mArrowRoot.localScale = Vector3.one;
					this.mArrowRoot.localPosition = Vector3.zero;
				}
				return this.mArrowRoot;
			}
		}

		public Transform MinimapFrame
		{
			get
			{
				if (this.mMinimapFrame == null && Application.isPlaying)
				{
					this.mMinimapFrame = base.transform.FindChild("_Content/Frame");
				}
				return this.mMinimapFrame;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			UIMiniMapBase.mInst = this;
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (Application.isPlaying && this.calculateBorder)
			{
				this.mapBorderRadius = this.rendererTransform.localScale.x / 2f / 4f;
			}
			this.UpdateAlignment();
		}

		private void OnClick()
		{
		}

		public virtual void UpdateAlignment()
		{
			if (this.mPivot != this.pivot || this.mMargin != this.margin || this.mMapScale != this.mapScale)
			{
				this.mMapScale = this.mapScale;
				this.mPivot = this.pivot;
				this.mMargin = this.margin;
				Vector3 zero = Vector3.zero;
				zero.z = this.rendererTransform.localPosition.z;
				if (this.pivot != UIMapBase.Pivot.Center)
				{
					switch (this.pivot)
					{
					case UIMapBase.Pivot.BottomLeft:
						zero.x = Mathf.Round(0.5f * this.mapScale.x) + this.margin.x;
						zero.y = Mathf.Round(0.5f * this.mapScale.y) + this.margin.y;
						break;
					case UIMapBase.Pivot.Left:
						zero.x = Mathf.Round(0.5f * this.mapScale.x) + this.margin.x;
						break;
					case UIMapBase.Pivot.TopLeft:
						zero.x = Mathf.Round(0.5f * this.mapScale.x) + this.margin.x;
						zero.y = Mathf.Round(-0.5f * this.mapScale.y) - this.margin.y;
						break;
					case UIMapBase.Pivot.Top:
						zero.y = Mathf.Round(-0.5f * this.mapScale.y) - this.margin.y;
						break;
					case UIMapBase.Pivot.TopRight:
						zero.x = Mathf.Round(-0.5f * this.mapScale.x) - this.margin.x;
						zero.y = Mathf.Round(-0.5f * this.mapScale.y) - this.margin.y;
						break;
					case UIMapBase.Pivot.Right:
						zero.x = Mathf.Round(-0.5f * this.mapScale.x) - this.margin.x;
						break;
					case UIMapBase.Pivot.BottomRight:
						zero.x = Mathf.Round(-0.5f * this.mapScale.x) - this.margin.x;
						zero.y = Mathf.Round(0.5f * this.mapScale.y) + this.margin.y;
						break;
					case UIMapBase.Pivot.Bottom:
						zero.y = Mathf.Round(0.5f * this.mapScale.y) + this.margin.y;
						break;
					}
				}
				this.rendererTransform.localPosition = zero;
				if (base.iconRoot != null)
				{
					base.iconRoot.localPosition = new Vector3(this.rendererTransform.localPosition.x, this.rendererTransform.localPosition.y, 1f);
				}
			}
		}

		protected override void UpdateIcon(NJGMapItem item, float x, float y)
		{
			float num = Mathf.Max(this.mapHalfScale.x, this.mapHalfScale.y) - this.mapBorderRadius;
			bool flag = item.gameObject.activeSelf && x * x + y * y <= num * num;
			if (!base.isPanning)
			{
				if (!flag && item.haveArrow)
				{
					if (item.arrow == null)
					{
						item.arrow = this.GetArrow(item);
					}
					if (item.arrow != null)
					{
						if (!NJGTools.GetActive(item.arrow.gameObject))
						{
							NJGTools.SetActive(item.arrow.gameObject, true);
						}
						item.arrow.UpdateRotation(this.target.position);
						if (this.MinimapFrame != null)
						{
							this.MinimapFrame.localEulerAngles = item.arrow.Rotation;
						}
					}
				}
				else if (flag && item.haveArrow && item.arrow != null && NJGTools.GetActive(item.arrow.gameObject))
				{
					NJGTools.SetActive(item.arrow.gameObject, false);
					if (this.MinimapFrame != null)
					{
						this.MinimapFrame.localEulerAngles = Vector3.zero;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			UIMapIconBase entry = this.GetEntry(item);
			if (entry != null && !entry.isValid)
			{
				entry.isMapIcon = false;
				entry.isValid = true;
				Transform cachedTransform = entry.cachedTransform;
				Vector3 vector = new Vector3(x, y, 0f);
				if (item.updatePosition && cachedTransform.localPosition != vector)
				{
					cachedTransform.localPosition = vector;
				}
				if (item.rotate)
				{
					float z = ((Vector3.Dot(item.cachedTransform.forward, Vector3.Cross(Vector3.up, Vector3.forward)) > 0f) ? -1f : 1f) * Vector3.Angle(item.cachedTransform.forward, Vector3.forward);
					cachedTransform.localEulerAngles = new Vector3(cachedTransform.localEulerAngles.x, cachedTransform.localEulerAngles.y, z);
				}
				else if (!item.rotate && this.rotateWithPlayer)
				{
					Vector3 vector2 = new Vector3(0f, 0f, -base.iconRoot.localEulerAngles.z);
					if (!cachedTransform.localEulerAngles.Equals(vector2))
					{
						cachedTransform.localEulerAngles = vector2;
					}
				}
				else if (!cachedTransform.localEulerAngles.Equals(Vector3.zero))
				{
					cachedTransform.localEulerAngles = Vector3.zero;
				}
			}
		}

		public virtual UIMapArrowBase GetArrow(UnityEngine.Object o)
		{
			return (UIMapArrowBase)o;
		}

		protected override void Update()
		{
			if (this.mPivot != this.pivot || this.mMargin != this.margin || this.mMapScale != this.mapScale)
			{
				this.UpdateAlignment();
			}
			if (this.arrowRoot != null)
			{
				if (base.isPanning && this.arrowRoot.localScale != new Vector3(0.001f, 0.001f, 0.001f))
				{
					this.arrowRoot.localScale = new Vector3(0.001f, 0.001f, 0.001f);
				}
				else if (!base.isPanning && this.arrowRoot.localScale != Vector3.one)
				{
					this.arrowRoot.localScale = Vector3.one;
				}
			}
			base.Update();
		}

		protected virtual void UpdateKeys()
		{
			if (Input.GetKeyDown(this.lockKey))
			{
				this.rotateWithPlayer = !this.rotateWithPlayer;
			}
		}

		public virtual void DeleteArrow(UIMapArrowBase ent)
		{
			if (ent != null)
			{
				this.mListArrow.Remove(ent);
				this.mUnusedArrow.Add(ent);
				NJGTools.SetActive(ent.gameObject, false);
			}
		}
	}
}
