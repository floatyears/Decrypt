using NJG;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class NJGMapRenderer : MonoBehaviour
{
	private static NJGMapRenderer mInst;

	public int mapImageIndex;

	private Vector2 lastSize;

	private Vector2 mSize;

	private Transform mTrans;

	private bool canRender = true;

	private bool mGenerated;

	private bool mWarning;

	private bool mReaded;

	private bool mApplied;

	private float lastRender;

	private NJGMapBase map;

	public static NJGMapRenderer instance
	{
		get
		{
			if (NJGMapRenderer.mInst == null)
			{
				NJGMapRenderer.mInst = new GameObject("_NJGMapRenderer")
				{
					transform = 
					{
						parent = NJGMapBase.instance.transform
					},
					layer = LayerMask.NameToLayer("TransparentFX")
				}.AddComponent<NJGMapRenderer>();
			}
			return NJGMapRenderer.mInst;
		}
	}

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

	private void Awake()
	{
		this.map = NJGMapBase.instance;
		if (this.map == null)
		{
			global::Debug.LogWarning(new object[]
			{
				"Can't render map photo. NJGMiniMap instance not found."
			});
			NJGTools.Destroy(base.gameObject);
			return;
		}
		if (base.gameObject.GetComponent<Camera>() == null)
		{
			base.gameObject.AddComponent<Camera>();
		}
		base.camera.enabled = false;
	}

	private void Start()
	{
		if (this.map.boundLayers.value == 0)
		{
			global::Debug.LogWarning(new object[]
			{
				"Can't render map photo. You have not choosen any layer for bounds calculation. Go to the NJGMiniMap inspector.",
				this.map
			});
			NJGTools.DestroyImmediate(base.gameObject);
			return;
		}
		if (this.map.renderLayers.value == 0)
		{
			global::Debug.LogWarning(new object[]
			{
				"Can't render map photo. You have not choosen any layer for rendering. Go to the NJGMiniMap inspector.",
				this.map
			});
			NJGTools.DestroyImmediate(base.gameObject);
			return;
		}
		if (!Application.isPlaying)
		{
			this.Render();
		}
	}

	private void ConfigCamera()
	{
		this.map.UpdateBounds();
		Bounds bounds = this.map.bounds;
		base.camera.depth = -100f;
		base.camera.backgroundColor = this.map.cameraBackgroundColor;
		base.camera.cullingMask = this.map.renderLayers;
		base.camera.clearFlags = (CameraClearFlags)this.map.cameraClearFlags;
		base.camera.isOrthoGraphic = true;
		float orthographicSize = 0f;
		if (this.map.orientation == NJGMapBase.Orientation.XYSideScroller)
		{
			base.camera.farClipPlane = bounds.size.z * 1.1f;
			orthographicSize = bounds.extents.y;
			base.camera.aspect = bounds.size.x / bounds.size.y;
		}
		else if (this.map.orientation == NJGMapBase.Orientation.XZDefault)
		{
			base.camera.farClipPlane = bounds.size.y * 1.1f;
			orthographicSize = bounds.extents.z;
			base.camera.aspect = bounds.size.x / bounds.size.z;
		}
		base.camera.farClipPlane = base.camera.farClipPlane * 5f;
		base.camera.nearClipPlane = -base.camera.farClipPlane;
		base.camera.orthographicSize = orthographicSize;
		if (this.map.orientation == NJGMapBase.Orientation.XZDefault)
		{
			this.cachedTransform.eulerAngles = new Vector3(90f, 0f, 0f);
			if (this.map.mapResolution == NJGMapBase.Resolution.Double)
			{
				for (int i = 0; i < 4; i++)
				{
					switch (i)
					{
					case 0:
						this.cachedTransform.position = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y + 1f, bounds.center.z - bounds.extents.z);
						break;
					case 1:
						this.cachedTransform.position = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y + 1f, bounds.center.z - bounds.extents.z);
						break;
					case 2:
						this.cachedTransform.position = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y + 1f, bounds.center.z + bounds.extents.z);
						break;
					case 3:
						this.cachedTransform.position = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y + 1f, bounds.center.z + bounds.extents.z);
						break;
					}
					global::Debug.Log(new object[]
					{
						string.Concat(new object[]
						{
							"cachedTransform.position ",
							this.cachedTransform.position,
							" / mapImageIndex ",
							this.mapImageIndex
						})
					});
					base.camera.enabled = true;
					this.mapImageIndex = i;
				}
			}
			else
			{
				this.cachedTransform.position = new Vector3(bounds.max.x - bounds.extents.x, bounds.size.y * 2f, bounds.center.z);
				base.camera.enabled = true;
			}
		}
		else
		{
			this.cachedTransform.eulerAngles = new Vector3(0f, 0f, 0f);
			this.cachedTransform.position = new Vector3(bounds.max.x - bounds.extents.x, bounds.center.y, -(Mathf.Abs(bounds.min.z) + Mathf.Abs(bounds.max.z) + 10f));
		}
	}

	[DebuggerHidden]
	private IEnumerator OnPostRender()
	{
        return null;
        //NJGMapRenderer.<OnPostRender>c__IteratorAA <OnPostRender>c__IteratorAA = new NJGMapRenderer.<OnPostRender>c__IteratorAA();
        //<OnPostRender>c__IteratorAA.<>f__this = this;
        //return <OnPostRender>c__IteratorAA;
	}

	public void Render()
	{
		if (Time.time >= this.lastRender)
		{
			if (Application.isPlaying)
			{
				this.lastRender = Time.time + 1f;
			}
			this.mReaded = false;
			this.mApplied = false;
			this.mGenerated = false;
			this.mWarning = false;
			if (!this.map.optimize)
			{
				this.canRender = true;
			}
			if (this.map.mapSize.x == 0f || this.map.mapSize.y == 0f)
			{
				this.map.mapSize = new Vector2((float)Screen.width, (float)Screen.height);
			}
			if (this.map.generateMapTexture)
			{
				if (this.map.userMapTexture != null)
				{
					NJGTools.Destroy(this.map.userMapTexture);
					this.map.userMapTexture = null;
				}
				if (this.map.mapTexture == null)
				{
					this.mSize = this.map.mapSize;
					if (this.map.mapResolution == NJGMapBase.Resolution.Double)
					{
						this.mSize = this.map.mapSize * 2f;
					}
					this.map.mapTexture = new Texture2D((int)this.mSize.x, (int)this.mSize.y, (TextureFormat)this.map.textureFormat, true);
					this.map.mapTexture.name = "_NJGMapTexture";
					this.map.mapTexture.filterMode = this.map.mapFilterMode;
					this.map.mapTexture.wrapMode = this.map.mapWrapMode;
					this.lastSize = this.mSize;
				}
			}
			else if (!Application.isPlaying)
			{
				if (this.map.mapTexture != null)
				{
					NJGTools.DestroyImmediate(this.map.mapTexture);
					this.map.mapTexture = null;
				}
				this.map.userMapTexture = new Texture2D((int)this.map.mapSize.x, (int)this.map.mapSize.y, (TextureFormat)this.map.textureFormat, true);
				this.map.userMapTexture.name = "_NJGTempTexture";
				this.map.userMapTexture.filterMode = this.map.mapFilterMode;
				this.map.userMapTexture.wrapMode = this.map.mapWrapMode;
			}
			this.ConfigCamera();
			base.camera.enabled = true;
		}
	}
}
