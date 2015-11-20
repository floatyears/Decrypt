using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace NJG
{
	public abstract class UIMapBase : MonoBehaviour
	{
		public enum Pivot
		{
			BottomLeft,
			Left,
			TopLeft,
			Top,
			TopRight,
			Right,
			BottomRight,
			Bottom,
			Center
		}

		public NJGMapBase.ShaderType shaderType;

		public Texture maskTexture;

		public float mapBorderRadius;

		public Vector2 margin = Vector2.zero;

		public UIMapBase.Pivot pivot = UIMapBase.Pivot.Center;

		[SerializeField]
		public float zoom = 1f;

		[SerializeField]
		public float zoomAmount = 0.5f;

		public Transform target;

		public string targetTag = "Player";

		[SerializeField]
		public float minZoom = 1f;

		[SerializeField]
		public float maxZoom = 30f;

		[SerializeField]
		public float zoomSpeed = 1f;

		[SerializeField]
		public KeyCode zoomInKey = KeyCode.KeypadPlus;

		[SerializeField]
		public KeyCode zoomOutKey = KeyCode.KeypadMinus;

		[SerializeField]
		public bool limitBounds = true;

		[SerializeField]
		public bool rotateWithPlayer;

		[SerializeField]
		public bool mouseWheelEnabled = true;

		[SerializeField]
		public bool panning = true;

		[SerializeField]
		public EaseType panningEasing = EaseType.EaseOutCirc;

		[SerializeField]
		public float panningSpeed = 1f;

		[SerializeField]
		public float panningSensitivity = 5f;

		[SerializeField]
		public bool panningMoveBack = true;

		[SerializeField]
		public KeyCode[] keysInUse = new KeyCode[3];

		[SerializeField]
		public Vector2 panningPosition = Vector2.zero;

		public float mapAngle;

		public Vector2 scrollPosition = Vector2.zero;

		public bool drawDirectionalLines;

		public Shader linesShader;

		public int linePoints = 20;

		public Color lineColor = Color.red;

		public float lineWidth = 0.1f;

		public List<Transform> controlPoints = new List<Transform>();

		[SerializeField]
		public bool calculateBorder = true;

		[HideInInspector, SerializeField]
		protected Transform mMapTrans;

		[HideInInspector, SerializeField]
		protected Vector3 mZoom = Vector3.zero;

		[HideInInspector, SerializeField]
		protected List<UIMapIconBase> mList = new List<UIMapIconBase>();

		[HideInInspector, SerializeField]
		protected Transform mIconRoot;

		[HideInInspector, SerializeField]
		protected Transform mTrans;

		[HideInInspector, SerializeField]
		protected Vector2 mMapScale;

		[HideInInspector, SerializeField]
		protected Vector2 mMapHalfScale;

		[HideInInspector, SerializeField]
		protected Vector2 mIconScale;

		[HideInInspector, SerializeField]
		protected Matrix4x4 mMatrix;

		[HideInInspector, SerializeField]
		protected Matrix4x4 rMatrix;

		[HideInInspector, SerializeField]
		protected bool mMapScaleChanged = true;

		[HideInInspector, SerializeField]
		protected int mDepth;

		protected Vector3 mLastScale = Vector3.zero;

		protected Vector3 mMapPos = Vector3.zero;

		protected int mLastHeight;

		private float mNextUpdate;

		protected Texture mMask;

		protected List<UIMapIconBase> mUnused = new List<UIMapIconBase>();

		protected int mCount;

		protected NJGMapBase map;

		protected bool isZooming;

		[SerializeField]
		protected Renderer mRenderer;

		[SerializeField]
		protected Color mColor = Color.white;

		protected Quaternion mapRotation;

		protected Vector3 rotationPivot = new Vector3(0.5f, 0.5f);

		[SerializeField]
		private EaseType mZoomEasing = EaseType.EaseOutExpo;

		private Vector2 mPanningMousePosLast = Vector2.zero;

		private bool mTargetWarning;

		protected Vector3 mIconEulers = Vector3.zero;

		private int mArrowSize;

		[SerializeField]
		private Vector3 mArrScale = Vector3.one;

		private Camera mUICam;

		private bool mIsPanning;

		private TweenParms mResetPan;

		private Transform mLinesRoot;

		private LineRenderer mLineRenderer;

		[SerializeField]
		protected Vector2 mMapScaleSize;

		private Vector3 mExt;

		public float mMod = 1f;

		private GameObject mFrustum;

		private Mesh mFrustumMesh;

		private Material mFrustumMat;

		private int mVertextCount;

		private int mLastVertextCount;

		private Color mLastColor;

		private float mLastWidth;

		protected Vector3 mClickPos;

		private Vector2 mWTM;

		private Vector3 mScrollPos;

		protected Transform mChild;

		private Animation mAnim;

		private bool mAnimCheck;

		private bool mVisible;

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

		public virtual Color mapColor
		{
			get
			{
				return this.mColor;
			}
			set
			{
				this.mColor = value;
				this.material.color = value;
			}
		}

		[SerializeField]
		public EaseType zoomEasing
		{
			get
			{
				return this.mZoomEasing;
			}
			set
			{
				this.mZoomEasing = value;
			}
		}

		public Transform iconRoot
		{
			get
			{
				if (this.mIconRoot == null && Application.isPlaying)
				{
					this.mIconRoot = NJGTools.AddChild(base.gameObject).transform;
					if (this.rendererTransform != null)
					{
						this.mIconRoot.parent = this.rendererTransform.parent;
						this.mIconRoot.localPosition = new Vector3(this.rendererTransform.localPosition.x, this.rendererTransform.localPosition.y, 1f);
						this.mIconRoot.localEulerAngles = this.rendererTransform.localEulerAngles;
					}
					this.mIconRoot.name = "_MapIcons";
					UIPanel uIPanel = this.mIconRoot.gameObject.AddComponent<UIPanel>();
					uIPanel.depth = 20;
				}
				return this.mIconRoot;
			}
		}

		public virtual bool isMouseOver
		{
			get;
			set;
		}

		public virtual Transform rendererTransform
		{
			get
			{
				if (this.planeRenderer != null && this.mMapTrans == null)
				{
					this.mMapTrans = this.planeRenderer.transform;
				}
				return (!(this.planeRenderer == null)) ? this.mMapTrans : base.transform;
			}
		}

		public virtual Renderer planeRenderer
		{
			get
			{
				if (this.mRenderer == null)
				{
					this.mRenderer = base.gameObject.renderer;
				}
				if (this.mRenderer == null)
				{
					this.mRenderer = base.gameObject.GetComponentInChildren<Renderer>();
				}
				return this.mRenderer;
			}
			set
			{
				this.mRenderer = value;
			}
		}

		public virtual Vector2 mapScale
		{
			get
			{
				return this.rendererTransform.localScale;
			}
			set
			{
				this.mMapScaleChanged = true;
				this.rendererTransform.localScale = value;
			}
		}

		public virtual Vector2 mapHalfScale
		{
			get
			{
				if (this.mMapScaleChanged)
				{
					this.mMapScaleChanged = false;
					this.mMapHalfScale = this.mapScale * 0.5f;
				}
				return this.mMapHalfScale;
			}
		}

		[SerializeField]
		public virtual Material material
		{
			get
			{
				return (!Application.isPlaying) ? this.planeRenderer.sharedMaterial : this.planeRenderer.material;
			}
			set
			{
				this.planeRenderer.material = value;
			}
		}

		public int depth
		{
			get
			{
				this.material.renderQueue = 3000 + this.mDepth;
				return this.mDepth;
			}
			set
			{
				if (this.mDepth != value)
				{
					this.mDepth = value;
					this.material.renderQueue = 3000 + this.mDepth;
				}
			}
		}

		public virtual Vector3 arrowScale
		{
			get
			{
				if (this.mArrowSize != this.map.arrowSize)
				{
					this.mArrowSize = this.map.arrowSize;
					this.mArrScale.x = (this.mArrScale.y = (float)this.map.arrowSize);
				}
				return this.mArrScale;
			}
		}

		public bool isVisible
		{
			get
			{
				return this.mChild.gameObject.activeInHierarchy;
			}
		}

		public bool isMouseOut
		{
			get
			{
				Vector3 mousePosition = Input.mousePosition;
				return mousePosition.x > (float)Screen.width || mousePosition.y > (float)Screen.height || mousePosition.x < 0f || mousePosition.y < 0f;
			}
		}

		public bool isPanning
		{
			get
			{
				return this.panning && this.mIsPanning;
			}
		}

		protected virtual void CleanIcons()
		{
			if (Application.isPlaying)
			{
				int i = this.mList.Count;
				while (i > 0)
				{
					UIMapIconBase uIMapIconBase = this.mList[--i];
					if (!uIMapIconBase.isValid)
					{
						this.Delete(uIMapIconBase);
					}
				}
			}
		}

		protected virtual UIMapIconBase GetEntry(NJGMapItem marker)
		{
			return null;
		}

		protected virtual void Delete(UIMapIconBase ent)
		{
			this.mList.Remove(ent);
			this.mUnused.Add(ent);
			NJGTools.SetActive(ent.gameObject, false);
		}

		protected virtual void OnEnable()
		{
			if (Application.isPlaying)
			{
				this.material.renderQueue = 3000 + this.depth;
				if (!this.map.queue.Contains(new Action(this.UpdateCode)))
				{
					this.map.queue.Add(new Action(this.UpdateCode));
				}
			}
		}

		protected virtual void OnDisable()
		{
			if (Application.isPlaying && this.map.queue.Contains(new Action(this.UpdateCode)))
			{
				this.map.queue.Remove(new Action(this.UpdateCode));
			}
		}

		protected virtual void OnDestroy()
		{
			if (Application.isPlaying && this.map.queue.Contains(new Action(this.UpdateCode)))
			{
				this.map.queue.Remove(new Action(this.UpdateCode));
			}
		}

		protected virtual void UpdateIcon(NJGMapItem item, float x, float y)
		{
		}

		protected virtual void Awake()
		{
			this.mMapScaleChanged = true;
			this.rendererTransform.hasChanged = true;
			this.map = NJGMapBase.instance;
			this.mTrans = base.transform;
			this.mUICam = NJGTools.FindCameraForLayer(NJGMapBase.instance.layer);
			if (this.material != null)
			{
				if (this.shaderType == NJGMapBase.ShaderType.FOW)
				{
					this.material.shader = Shader.Find("NinjutsuGames/Map FOW");
				}
				else if (this.shaderType == NJGMapBase.ShaderType.TextureMask)
				{
					this.material.shader = Shader.Find("NinjutsuGames/Map TextureMask");
				}
				else if (this.shaderType == NJGMapBase.ShaderType.ColorMask)
				{
					this.material.shader = Shader.Find("NinjutsuGames/Map ColorMask");
				}
			}
			if (this.maskTexture == null && this.material != null)
			{
				this.maskTexture = this.material.GetTexture("_Mask");
			}
			if (this.drawDirectionalLines && Application.isPlaying)
			{
				if (this.linesShader == null)
				{
					this.linesShader = Shader.Find("Particles/Additive");
				}
				GameObject gameObject = NJGTools.AddChild(base.gameObject);
				this.mLinesRoot = gameObject.transform;
				this.mLinesRoot.parent = this.iconRoot;
				this.mLinesRoot.localPosition = Vector3.zero;
				this.mLinesRoot.localEulerAngles = Vector3.zero;
				this.mLineRenderer = gameObject.GetComponent<LineRenderer>();
				if (this.mLineRenderer == null)
				{
					gameObject.AddComponent<LineRenderer>();
				}
				this.mLineRenderer = gameObject.GetComponent<LineRenderer>();
				this.mLineRenderer.useWorldSpace = true;
				this.mLineRenderer.material = new Material(this.linesShader);
				this.mLinesRoot.name = "_Lines";
			}
			this.mapAngle = 180f;
			this.mapRotation = Quaternion.Euler(0f, 0f, this.mapAngle);
		}

		protected virtual void Start()
		{
			this.map = NJGMapBase.instance;
			if (this.material == null)
			{
				if (Application.isPlaying)
				{
					global::Debug.LogWarning(new object[]
					{
						"The UITexture does not have a material assigned",
						this
					});
				}
			}
			else
			{
				if (this.map.generateMapTexture)
				{
					this.material.mainTexture = this.map.mapTexture;
				}
				else
				{
					this.material.mainTexture = NJGMapBase.instance.userMapTexture;
				}
				if (this.maskTexture != null)
				{
					this.material.SetTexture("_Mask", this.maskTexture);
				}
				this.material.color = this.mapColor;
			}
			if (Application.isPlaying && this.mChild == null)
			{
				if (this.cachedTransform.childCount > 0)
				{
					this.mChild = this.cachedTransform;
				}
				else
				{
					this.mChild = this.cachedTransform.GetChild(0);
				}
			}
			this.OnStart();
			this.Update();
		}

		protected virtual void UpdateCode()
		{
			global::Debug.Log(new object[]
			{
				"UpdateCode "
			});
		}

		private void UpdateScrollPosition()
		{
			Bounds bounds = this.map.bounds;
			Vector3 extents = bounds.extents;
			float num = 1f / this.zoom;
			if (this.target == null)
			{
				return;
			}
			this.scrollPosition = Vector3.zero;
			Vector3 vector = this.target.position - bounds.center;
			this.mExt.x = 0.5f / extents.x;
			this.mExt.y = 0.5f / extents.y;
			this.mExt.z = 0.5f / extents.z;
			if (this.map.mapResolution == NJGMapBase.Resolution.Double)
			{
				this.mExt.x = this.mExt.x * this.mMod;
				this.mExt.y = this.mExt.y * this.mMod;
				this.mExt.z = this.mExt.z * this.mMod;
			}
			this.scrollPosition.x = vector.x * this.mExt.x;
			if (this.map.orientation == NJGMapBase.Orientation.XZDefault)
			{
				this.scrollPosition.y = vector.z * this.mExt.z;
			}
			else
			{
				this.scrollPosition.y = vector.y * this.mExt.y;
			}
			if (this.panning)
			{
				this.scrollPosition += this.panningPosition;
			}
			if (this.limitBounds)
			{
				this.scrollPosition.x = Mathf.Max(-((1f - num) * 0.5f), this.scrollPosition.x);
				this.scrollPosition.x = Mathf.Min((1f - num) * 0.5f, this.scrollPosition.x);
				this.scrollPosition.y = Mathf.Max(-((1f - num) * 0.5f), this.scrollPosition.y);
				this.scrollPosition.y = Mathf.Min((1f - num) * 0.5f, this.scrollPosition.y);
			}
			Vector3 vector2 = new Vector3((1f - num) * 0.5f + this.scrollPosition.x, (1f - num) * 0.5f + this.scrollPosition.y, 0f);
			this.mZoom.x = (this.mZoom.y = (this.mZoom.z = num));
			if (!this.mMapPos.Equals(vector2) || this.rotateWithPlayer)
			{
				this.UpdateMatrix(vector2);
			}
		}

		protected virtual void UpdateMatrix(Vector3 pos)
		{
			this.mMapPos = pos;
			Matrix4x4 lhs = Matrix4x4.TRS(this.mMapPos, Quaternion.identity, this.mZoom);
			if (this.rotateWithPlayer)
			{
				Vector3 forward = this.target.forward;
				forward.Normalize();
				this.mapAngle = ((Vector3.Dot(forward, Vector3.Cross(Vector3.up, Vector3.forward)) > 0f) ? -1f : 1f) * Vector3.Angle(forward, Vector3.forward);
				this.mapRotation = Quaternion.Euler(0f, 0f, this.mapAngle);
			}
			Matrix4x4 rhs = Matrix4x4.TRS(-this.rotationPivot, Quaternion.identity, Vector3.one);
			Matrix4x4 rhs2 = Matrix4x4.TRS(Vector3.zero, this.mapRotation, Vector3.one);
			Matrix4x4 rhs3 = Matrix4x4.TRS(this.rotationPivot, Quaternion.identity, Vector3.one);
			this.rMatrix = lhs * rhs3 * rhs2 * rhs;
			if (!this.mMatrix.Equals(this.rMatrix))
			{
				this.mMatrix = this.rMatrix;
				this.material.SetMatrix("_Matrix", this.rMatrix);
			}
			if (this.iconRoot != null)
			{
				this.mIconEulers.z = -this.mapAngle;
				if (this.iconRoot.localEulerAngles != this.mIconEulers)
				{
					this.iconRoot.localEulerAngles = this.mIconEulers;
				}
			}
		}

		private void OnApplicationFocus(bool focusStatus)
		{
			if (focusStatus)
			{
				this.material.SetMatrix("_Matrix", this.mMatrix);
			}
		}

		protected virtual void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			if (this.target != null && this.controlPoints.Count == 0 && !this.controlPoints.Contains(this.target))
			{
				this.controlPoints.Add(this.target);
			}
			if (this.target == null && !this.mTargetWarning)
			{
				this.mTargetWarning = true;
			}
			int height = Screen.height;
			bool flag = this.mLastHeight != height;
			if (this.mLastScale != this.rendererTransform.localScale)
			{
				this.mMapScaleChanged = true;
				this.mLastScale = this.rendererTransform.localScale;
				if (this.calculateBorder)
				{
					this.mapBorderRadius = this.rendererTransform.localScale.x / 2f / 4f;
				}
			}
			if (this.mNextUpdate < Time.time)
			{
				this.mLastHeight = height;
				this.mNextUpdate = Time.time + this.map.updateFrequency;
				this.UpdateIcons();
				this.CleanIcons();
				this.UpdateScrollPosition();
				if ((this.OnUpdate() || flag) && NJGMapBase.instance.renderMode == NJGMapBase.RenderMode.ScreenChange && this is UIMiniMapBase)
				{
					NJGMapBase.instance.GenerateMap();
				}
			}
		}

		protected virtual void UpdateZoomKeys()
		{
			if (Input.GetKeyDown(this.zoomInKey))
			{
				this.ZoomIn(this.zoomAmount);
			}
			if (Input.GetKeyDown(this.zoomOutKey))
			{
				this.ZoomOut(this.zoomAmount);
			}
		}

		protected virtual void OnStart()
		{
		}

		protected virtual bool OnUpdate()
		{
			return false;
		}

		protected void UpdateIcons()
		{
			int i = this.mList.Count;
			while (i > 0)
			{
				UIMapIconBase uIMapIconBase = this.mList[--i];
				uIMapIconBase.isValid = false;
				if (this.drawDirectionalLines)
				{
					if (uIMapIconBase.item.cachedTransform != this.target)
					{
						if (uIMapIconBase.item.drawDirection)
						{
							if (!this.controlPoints.Contains(uIMapIconBase.cachedTransform))
							{
								this.controlPoints.Add(uIMapIconBase.cachedTransform);
							}
						}
						else if (this.controlPoints.Contains(uIMapIconBase.cachedTransform))
						{
							this.controlPoints.Remove(uIMapIconBase.cachedTransform);
						}
					}
					else if (this.controlPoints[0] != uIMapIconBase.cachedTransform)
					{
						this.controlPoints[0] = uIMapIconBase.cachedTransform;
					}
				}
			}
			for (int j = 0; j < NJGMapItem.list.Count; j++)
			{
				NJGMapItem nJGMapItem = NJGMapItem.list[j];
				if (nJGMapItem.type >= 1)
				{
					Vector2 pos = this.WorldToMap(nJGMapItem.cachedTransform.position);
					if (this.map.fow.enabled)
					{
						if (!nJGMapItem.isRevealed)
						{
							nJGMapItem.isRevealed = ((nJGMapItem.revealFOW || NJGFOW.instance.IsVisible(pos)) && (nJGMapItem.revealFOW || NJGFOW.instance.IsExplored(pos)));
							bool flag = nJGMapItem.cachedTransform == this.target;
							if (flag)
							{
								nJGMapItem.isRevealed = true;
							}
						}
					}
					else if (!nJGMapItem.isRevealed)
					{
						nJGMapItem.isRevealed = true;
					}
					if (nJGMapItem.isRevealed)
					{
						this.UpdateIcon(nJGMapItem, pos.x, pos.y);
					}
				}
			}
		}

		protected virtual void UpdateFrustum()
		{
			if (this.map.cameraFrustum == null)
			{
				return;
			}
			if (this.map.orientation == NJGMapBase.Orientation.XYSideScroller)
			{
				return;
			}
			if (this.mFrustumMesh == null)
			{
				this.mFrustum = new GameObject();
				this.mFrustumMat = new Material(Shader.Find("NinjutsuGames/Map TextureMask"));
				this.mFrustum.AddComponent<MeshRenderer>().material = this.mFrustumMat;
				this.mFrustum.name = "_Frustum";
				this.mFrustum.transform.parent = this.iconRoot;
				this.mFrustum.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
				this.mFrustum.transform.localPosition = Vector3.zero;
				this.mFrustum.transform.localScale = Vector3.one;
				this.mFrustum.layer = base.gameObject.layer;
				Mesh mesh = NJGTools.CreatePlane();
				this.mFrustum.AddComponent<MeshFilter>().mesh = mesh;
				this.mFrustumMesh = mesh;
			}
			Vector3[] vertices = this.mFrustumMesh.vertices;
			vertices[1] = this.map.cameraFrustum.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)(Screen.height / 2), this.map.cameraFrustum.farClipPlane));
			vertices[2] = this.map.cameraFrustum.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)(Screen.height / 2), this.map.cameraFrustum.nearClipPlane));
			vertices[3] = this.map.cameraFrustum.ScreenToWorldPoint(new Vector3(0f, (float)(Screen.height / 2), this.map.cameraFrustum.nearClipPlane));
			vertices[0] = this.map.cameraFrustum.ScreenToWorldPoint(new Vector3(0f, (float)(Screen.height / 2), this.map.cameraFrustum.farClipPlane));
			float y = (this.map.orientation != NJGMapBase.Orientation.XZDefault) ? (this.map.bounds.max.z + 1f + 0.1f) : (this.map.bounds.min.y - 1f + 0.1f);
			for (int i = 0; i < 4; i++)
			{
				vertices[i].y = y;
			}
			this.mFrustumMesh.vertices = vertices;
			this.mFrustumMesh.RecalculateBounds();
			this.mFrustumMat.SetColor("_Color", this.map.cameraFrustumColor);
		}

		private void UpdatePanning()
		{
			if (!this.panning)
			{
				this.panningPosition = Vector2.zero;
				return;
			}
			if (this.isMouseOver)
			{
				if (Input.GetMouseButtonDown(0))
				{
					this.mPanningMousePosLast = this.mUICam.ScreenToViewportPoint(Input.mousePosition);
					if (HOTween.IsTweening(this))
					{
						HOTween.Kill(this);
					}
				}
				if (!this.mIsPanning && Input.GetMouseButton(0) && Vector2.Distance(this.mUICam.ScreenToViewportPoint(Input.mousePosition), this.mPanningMousePosLast) > 0.01f)
				{
					this.mIsPanning = true;
				}
			}
			if (this.isPanning)
			{
				if (Input.GetMouseButton(0))
				{
                    Vector2 position = Vector2.zero;// this.mUICam.ScreenToViewportPoint(Input.mousePosition) - this.mPanningMousePosLast;
					Vector2 a = this.GetDirection(position) * this.panningSensitivity;
					this.panningPosition -= a / this.zoom;
					this.mPanningMousePosLast = this.mUICam.ScreenToViewportPoint(Input.mousePosition);
				}
				if (Input.GetMouseButtonUp(0))
				{
					if (this.panningMoveBack)
					{
						this.ResetPanning();
					}
					else
					{
						this.mIsPanning = false;
					}
				}
			}
		}

		public void ResetPanning()
		{
			if (this.panningPosition == Vector2.zero)
			{
				this.mIsPanning = false;
				return;
			}
			if (this.mResetPan == null)
			{
				this.mResetPan = new TweenParms().Prop("panningPosition", Vector2.zero).OnComplete(new TweenDelegate.TweenCallback(this.OnPanningComplete));
			}
			HOTween.To(this, this.panningSpeed, this.mResetPan).easeType = this.panningEasing;
		}

		private void OnPanningComplete()
		{
			this.panningPosition = Vector2.zero;
			this.mIsPanning = false;
		}

		private void DrawLines()
		{
			if (null == this.mLineRenderer || this.controlPoints == null || this.controlPoints.Count < 2)
			{
				return;
			}
			if (this.mLastColor != this.lineColor)
			{
				this.mLastColor = this.lineColor;
				this.mLineRenderer.SetColors(this.lineColor, this.lineColor);
			}
			if (this.mLastWidth != this.lineWidth)
			{
				this.mLastWidth = this.lineWidth;
				this.mLineRenderer.SetWidth(this.lineWidth * 0.1f, this.lineWidth * 0.1f);
			}
			if (this.linePoints < 2)
			{
				this.linePoints = 2;
			}
			this.mVertextCount = this.linePoints * (this.controlPoints.Count - 1);
			if (this.mLastVertextCount != this.mVertextCount)
			{
				this.mLastVertextCount = this.mVertextCount;
				this.mLineRenderer.SetVertexCount(this.mVertextCount);
			}
			int i = 0;
			int num = this.controlPoints.Count - 1;
			while (i < num)
			{
				if (this.controlPoints[i] == null || this.controlPoints[i + 1] == null || (i > 0 && this.controlPoints[i - 1] == null) || (i < this.controlPoints.Count - 2 && this.controlPoints[i + 2] == null))
				{
					return;
				}
				Vector3 position = this.controlPoints[i].position;
				Vector3 position2 = this.controlPoints[i + 1].position;
				Vector3 a;
				if (i > 0)
				{
					a = 0.5f * (this.controlPoints[i + 1].position - this.controlPoints[i - 1].position);
				}
				else
				{
					a = this.controlPoints[i + 1].position - this.controlPoints[i].position;
				}
				Vector3 a2;
				if (i < this.controlPoints.Count - 2)
				{
					a2 = 0.5f * (this.controlPoints[i + 2].position - this.controlPoints[i].position);
				}
				else
				{
					a2 = this.controlPoints[i + 1].position - this.controlPoints[i].position;
				}
				float num2 = 1f / (float)this.linePoints;
				if (i == this.controlPoints.Count - 2)
				{
					num2 = 1f / ((float)this.linePoints - 1f);
				}
				for (int j = 0; j < this.linePoints; j++)
				{
					float num3 = (float)j * num2;
					Vector3 position3 = (2f * num3 * num3 * num3 - 3f * num3 * num3 + 1f) * position + (num3 * num3 * num3 - 2f * num3 * num3 + num3) * a + (-2f * num3 * num3 * num3 + 3f * num3 * num3) * position2 + (num3 * num3 * num3 - num3 * num3) * a2;
					this.mLineRenderer.SetPosition(j + i * this.linePoints, position3);
				}
				i++;
			}
		}

		public void ZoomIn(float amount)
		{
			if (this.zoom == this.maxZoom)
			{
				return;
			}
			if (HOTween.IsTweening(this))
			{
				HOTween.Complete(this);
			}
			HOTween.To(this, this.zoomSpeed, "zoom", Mathf.Clamp(this.zoom + amount, (float)((int)this.minZoom), (float)((int)this.maxZoom))).easeType = this.zoomEasing;
		}

		public void ZoomOut(float amount)
		{
			if (this.zoom == this.minZoom)
			{
				return;
			}
			if (HOTween.IsTweening(this))
			{
				HOTween.Complete(this);
			}
			HOTween.To(this, this.zoomSpeed, "zoom", Mathf.Clamp(this.zoom - amount, (float)((int)this.minZoom), (float)((int)this.maxZoom))).easeType = this.zoomEasing;
		}

		public Vector3 MapToWorld(Vector2 pos)
		{
			Bounds bounds = this.map.bounds;
			Vector3 extents = bounds.extents;
            Vector3 vector = new Vector3(bounds.center.x + pos.x, bounds.center.y + pos.x, bounds.center.z);
			float num = this.mapHalfScale.x / extents.x;
			float num2 = this.mapHalfScale.y / ((this.map.orientation != NJGMapBase.Orientation.XZDefault) ? extents.y : extents.z);
			Vector3 vector2 = this.WorldScrollPosition();
			num *= this.zoom;
			num2 *= this.zoom;
			this.mClickPos.x = (vector.x + vector2.x) * num;
			this.mClickPos.z = ((this.map.orientation != NJGMapBase.Orientation.XZDefault) ? ((vector.y + vector2.y) * num2) : ((vector.z + vector2.z) * num2));
			this.mClickPos.y = this.target.position.y;
			return this.mClickPos;
		}

		public Vector2 WorldToMap(Vector3 worldPos)
		{
			return this.WorldToMap(worldPos, true);
		}

		public Vector2 WorldToMap(Vector3 worldPos, bool calculateZoom)
		{
			if (this.map == null)
			{
				this.map = NJGMapBase.instance;
			}
			Bounds bounds = this.map.bounds;
			Vector3 extents = bounds.extents;
			Vector3 vector = worldPos - bounds.center;
			float num = this.mapHalfScale.x / extents.x;
			float num2 = this.mapHalfScale.y / ((this.map.orientation != NJGMapBase.Orientation.XZDefault) ? extents.y : extents.z);
			Vector3 vector2 = this.WorldScrollPosition();
			if (calculateZoom)
			{
				num *= this.zoom;
				num2 *= this.zoom;
			}
			else
			{
				num *= 1f;
				num2 *= 1f;
				vector2 = Vector3.zero;
			}
			this.mWTM.x = (vector.x - vector2.x) * num;
			this.mWTM.y = ((this.map.orientation != NJGMapBase.Orientation.XZDefault) ? ((vector.y - vector2.y) * num2) : ((vector.z - vector2.z) * num2));
			if (this.map.mapResolution == NJGMapBase.Resolution.Double)
			{
				num *= this.mMod;
				num2 *= this.mMod;
			}
			return this.mWTM;
		}

		public Vector3 WorldScrollPosition()
		{
			Vector3 size = this.map.bounds.size;
			this.mScrollPos.x = this.scrollPosition.x * size.x;
			this.mScrollPos.y = this.scrollPosition.y * size.y;
			this.mScrollPos.z = this.scrollPosition.y * size.z;
			return this.mScrollPos;
		}

		public void Toggle()
		{
			if (this.mVisible)
			{
				this.Hide();
			}
			else
			{
				this.Show();
			}
		}

		public virtual void Show()
		{
			if (!this.mVisible)
			{
				if (this.mChild == null)
				{
					this.mChild = this.cachedTransform.GetChild(0);
				}
				if (this.mChild == null)
				{
					this.mChild = base.transform;
				}
				if (this.mAnim == null && !this.mAnimCheck)
				{
					this.mAnim = base.gameObject.GetComponentInChildren<Animation>();
					this.mAnimCheck = true;
				}
				if (this.mAnim != null)
				{
					NJGTools.SetActive(this.mChild.gameObject, true);
					this.mAnim[this.mAnim.clip.name].speed = 1f;
					this.mAnim[this.mAnim.clip.name].time = 0f;
					if (this.mAnim.clip != null)
					{
						this.mAnim.Play();
					}
				}
				else
				{
					NJGTools.SetActive(this.mChild.gameObject, true);
				}
				this.mVisible = true;
				base.enabled = true;
			}
		}

		public virtual void Hide()
		{
			if (this.mVisible)
			{
				if (this.mChild == null)
				{
					this.mChild = this.cachedTransform.GetChild(0);
				}
				if (this.mChild == null)
				{
					this.mChild = base.transform;
				}
				if (this.mAnim == null && !this.mAnimCheck)
				{
					this.mAnim = base.gameObject.GetComponentInChildren<Animation>();
					this.mAnimCheck = true;
				}
				if (this.mAnim != null)
				{
					if (this.mAnim.clip != null)
					{
						this.mAnim[this.mAnim.clip.name].speed = -1f;
						this.mAnim[this.mAnim.clip.name].time = this.mAnim[this.mAnim.clip.name].length;
						this.mAnim.Play();
						base.StartCoroutine(this.DisableOnFinish());
					}
				}
				else
				{
					NJGTools.SetActive(this.mChild.gameObject, false);
				}
				this.mVisible = false;
				base.enabled = false;
			}
		}

		[DebuggerHidden]
		private IEnumerator DisableOnFinish()
		{
            return null;
            //UIMapBase.<DisableOnFinish>c__IteratorAB <DisableOnFinish>c__IteratorAB = new UIMapBase.<DisableOnFinish>c__IteratorAB();
            //<DisableOnFinish>c__IteratorAB.<>f__this = this;
            //return <DisableOnFinish>c__IteratorAB;
		}

		public Vector2 GetDirection(Vector2 position)
		{
			return this.mapRotation * position;
		}

		public Vector3 GetDirection(Vector3 position)
		{
			return this.mapRotation * position;
		}
	}
}
