using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace NJG
{
	[ExecuteInEditMode]
	public class NJGMapBase : MonoBehaviour
	{
		[Serializable]
		public class FOW
		{
			public enum FOWSystem
			{
				BuiltInFOW,
				TasharenFOW
			}

			public bool enabled;

			public NJGMapBase.FOW.FOWSystem fowSystem;

			public bool trailEffect;

			public float textureBlendTime = 0.5f;

			public float updateFrequency = 0.15f;

			public Color fogColor = Color.black;

			public int revealDistance = 10;

			public int textureSize = 200;

			public bool debug;

			public int blurIterations = 2;
		}

		[Serializable]
		public class MapItemType
		{
			public bool enableInteraction = true;

			public string type = "New Marker";

			public string sprite;

			public bool useCustomSize;

			public bool useCustomBorderSize;

			public int size = 32;

			public int borderSize = 32;

			public Color color = Color.white;

			public bool animateOnVisible = true;

			public bool showOnAction;

			public bool loopAnimation;

			public float fadeOutAfterDelay;

			public bool rotate = true;

			public bool updatePosition = true;

			public bool haveArrow;

			public string arrowSprite;

			public bool folded = true;

			public int depth;

			public bool deleteRequest;

			public int arrowOffset = 20;

			public int arrowDepth = 5;

			public bool arrowRotate = true;

			public void OnSelectSprite(string spriteName)
			{
				this.sprite = spriteName;
			}

			public void OnSelectArrowSprite(string spriteName)
			{
				this.arrowSprite = spriteName;
			}
		}

		[Serializable]
		public class MapLevel
		{
			public string level = "Level";

			public List<NJGMapBase.MapZone> zones = new List<NJGMapBase.MapZone>();

			public bool folded = true;

			public bool itemsFolded = true;

			public bool deleteRequest;
		}

		[Serializable]
		public class MapZone
		{
			public string type = "New Zone";

			public Color color = Color.white;

			public float fadeOutAfterDelay = 3f;

			public bool folded = true;

			public int depth;

			public bool deleteRequest;
		}

		public enum SettingsScreen
		{
			General,
			Icons,
			FOW,
			Zones,
			_LastDoNotUse
		}

		public enum Resolution
		{
			Normal,
			Double
		}

		public enum RenderMode
		{
			Once,
			ScreenChange,
			Dynamic
		}

		public enum NJGTextureFormat
		{
			ARGB32 = 5,
			RGB24 = 3
		}

		public enum NJGCameraClearFlags
		{
			Skybox = 1,
			Depth = 3,
			Color = 2,
			Nothing = 4
		}

		public enum ShaderType
		{
			TextureMask,
			ColorMask,
			FOW
		}

		[SerializeField]
		public enum Orientation
		{
			XZDefault,
			XYSideScroller
		}

		public const string VERSION = "1.5";

		[SerializeField]
		public UIMiniMapBase miniMap;

		private static NJGMapBase mInst;

		public Action<string> onWorldNameChanged;

		[SerializeField]
		public NJGMapBase.FOW fow;

		[SerializeField]
		public bool showBounds = true;

		[SerializeField]
		public Color zoneColor = Color.white;

		public List<NJGMapBase.MapItemType> mapItems = new List<NJGMapBase.MapItemType>(new NJGMapBase.MapItemType[]
		{
			new NJGMapBase.MapItemType
			{
				type = "None"
			}
		});

		public List<NJGMapBase.MapLevel> levels = new List<NJGMapBase.MapLevel>();

		[SerializeField]
		public NJGMapBase.RenderMode renderMode;

		[SerializeField]
		public NJGMapBase.Resolution mapResolution;

		[SerializeField]
		public float dynamicRenderTime = 1f;

		public NJGMapBase.Orientation orientation;

		public NJGMapBase.SettingsScreen screen;

		[SerializeField]
		public LayerMask renderLayers = 1;

		[SerializeField]
		public LayerMask boundLayers = 1;

		public int iconSize = 16;

		public int borderSize = 16;

		public int arrowSize = 16;

		public float updateFrequency = 0.01f;

		public bool setBoundsManually;

		[SerializeField]
		public Vector3 manualBounds = new Vector3(10f, 10f, 10f);

		[SerializeField]
		public Bounds bounds;

		public bool typesFolded;

		public bool zonesFolded;

		public Texture2D mapTexture;

		public Texture2D userMapTexture;

		public bool generateMapTexture;

		public bool generateAtStart = true;

		public Camera cameraFrustum;

		public Color cameraFrustumColor = new Color(255f, 255f, 255f, 50f);

		public bool useTextureGenerated;

		[SerializeField]
		public FilterMode mapFilterMode = FilterMode.Bilinear;

		[SerializeField]
		public TextureWrapMode mapWrapMode = TextureWrapMode.Clamp;

		[SerializeField]
		public NJGMapBase.NJGTextureFormat textureFormat = NJGMapBase.NJGTextureFormat.ARGB32;

		[SerializeField]
		public NJGMapBase.NJGCameraClearFlags cameraClearFlags = NJGMapBase.NJGCameraClearFlags.Skybox;

		public Color cameraBackgroundColor = Color.red;

		public bool transparentTexture;

		public bool optimize;

		public int renderOffset = 10;

		public int layer;

		public List<Action> queue = new List<Action>();

		protected Camera mCam;

		private Vector2 mSize = new Vector2(1024f, 1024f);

		private Bounds mBounds;

		[SerializeField]
		private string mWorldName = "My Epic World";

		private string mLastWorldName;

		private Vector3 mMapOrigin = Vector2.zero;

		private Vector3 mMapEulers = Vector2.zero;

		private float mOrtoSize;

		private float mAspect;

		private float mElapsed;

		private Terrain[] mTerrains;

		public static NJGMapBase instance
		{
			get
			{
				return NJGMapBase.mInst;
			}
		}

		[SerializeField]
		public string worldName
		{
			get
			{
				return this.mWorldName;
			}
			set
			{
				this.mWorldName = value;
				if (this.mLastWorldName != this.mWorldName)
				{
					this.mLastWorldName = this.mWorldName;
					if (this.onWorldNameChanged != null)
					{
						this.onWorldNameChanged(this.mWorldName);
					}
				}
			}
		}

		public virtual bool isMouseOver
		{
			get
			{
				return UIMiniMapBase.inst == null || UIMiniMapBase.inst.isMouseOver;
			}
		}

		[SerializeField]
		public Vector3 mapOrigin
		{
			get
			{
				if (NJGMapRenderer.instance != null)
				{
					this.mMapOrigin = NJGMapRenderer.instance.cachedTransform.position;
				}
				return this.mMapOrigin;
			}
		}

		[SerializeField]
		public Vector3 mapEulers
		{
			get
			{
				if (NJGMapRenderer.instance != null)
				{
					this.mMapEulers = NJGMapRenderer.instance.cachedTransform.eulerAngles;
				}
				return this.mMapEulers;
			}
		}

		[SerializeField]
		public float ortoSize
		{
			get
			{
				if (NJGMapRenderer.instance != null)
				{
					this.mOrtoSize = NJGMapRenderer.instance.camera.orthographicSize;
				}
				return this.mOrtoSize;
			}
		}

		[SerializeField]
		public float aspect
		{
			get
			{
				if (NJGMapRenderer.instance != null)
				{
					this.mAspect = NJGMapRenderer.instance.camera.aspect;
				}
				return this.mAspect;
			}
		}

		[SerializeField]
		public Vector2 mapSize
		{
			get
			{
				if (Application.isPlaying)
				{
					this.mSize.x = (float)Screen.width;
					this.mSize.y = (float)Screen.height;
				}
				return this.mSize;
			}
			set
			{
				this.mSize = value;
			}
		}

		public float elapsed
		{
			get
			{
				return this.mElapsed;
			}
		}

		[SerializeField]
		public string[] mapItemTypes
		{
			get
			{
				List<string> list = new List<string>();
				int i = 0;
				int count = this.mapItems.Count;
				while (i < count)
				{
					list.Add(this.mapItems[i].type);
					i++;
				}
				string[] arg_5F_0;
				if (list.Count == 0)
				{
					(arg_5F_0 = new string[1])[0] = "No types defined";
				}
				else
				{
					arg_5F_0 = list.ToArray();
				}
				return arg_5F_0;
			}
		}

		protected virtual void Awake()
		{
			NJGMapBase.mInst = this;
			if (this.fow.textureSize < 200)
			{
				this.fow.textureSize = 200;
			}
			if (this.miniMap == null)
			{
				this.miniMap = base.transform.GetComponentInChildren<UIMiniMapBase>();
			}
			if (Application.isPlaying)
			{
				if (this.mapTexture != null)
				{
					NJGTools.Destroy(this.mapTexture);
				}
				if (this.generateAtStart)
				{
					this.GenerateMap();
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (this.showBounds)
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
			}
		}

		public void GenerateMap()
		{
			if ((Application.isPlaying && this.generateMapTexture) || (!Application.isPlaying && !this.generateMapTexture))
			{
				NJGMapRenderer.instance.Render();
			}
		}

		private void Start()
		{
			if (this.onWorldNameChanged != null)
			{
				this.onWorldNameChanged(this.worldName);
			}
			this.UpdateBounds();
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.fow.enabled)
			{
				NJGFOW.instance.Init();
			}
		}

		private void ThreadUpdate()
		{
			Stopwatch stopwatch = new Stopwatch();
			while (true)
			{
				stopwatch.Reset();
				stopwatch.Start();
				this.queue.ForEach(delegate(Action a)
				{
					a();
				});
				stopwatch.Stop();
				this.mElapsed = 0.001f * (float)stopwatch.ElapsedMilliseconds;
				Thread.Sleep(1);
			}
		}

		public void SetTexture(Texture2D tex)
		{
			if (UIMiniMapBase.inst != null)
			{
				UIMiniMapBase.inst.material.mainTexture = tex;
			}
		}

		public static bool IsInRenderLayers(GameObject obj, LayerMask mask)
		{
			return (mask.value & 1 << obj.layer) > 0;
		}

		public void UpdateBounds()
		{
			if (this.setBoundsManually)
			{
				this.mBounds = new Bounds(this.manualBounds * 0.5f, this.manualBounds);
				this.bounds = this.mBounds;
				return;
			}
			bool flag = false;
			this.mTerrains = (UnityEngine.Object.FindObjectsOfType(typeof(Terrain)) as Terrain[]);
			bool flag2 = this.mTerrains != null;
			if (flag2)
			{
				flag2 = (this.mTerrains.Length > 1);
			}
			if (flag2)
			{
				int i = 0;
				int num = this.mTerrains.Length;
				while (i < num)
				{
					Terrain terrain = this.mTerrains[i];
					MeshRenderer component = terrain.GetComponent<MeshRenderer>();
					if (!flag)
					{
						this.mBounds = default(Bounds);
						flag = true;
					}
					if (component != null)
					{
						this.mBounds.Encapsulate(component.bounds);
					}
					else
					{
						TerrainCollider component2 = terrain.GetComponent<TerrainCollider>();
						if (!(component2 != null))
						{
							global::Debug.LogError(new object[]
							{
								"Could not get measure bounds of terrain.",
								this
							});
							return;
						}
						this.mBounds.Encapsulate(component2.bounds);
					}
					i++;
				}
			}
			else if (Terrain.activeTerrain != null)
			{
				Terrain activeTerrain = Terrain.activeTerrain;
				MeshRenderer component3 = activeTerrain.GetComponent<MeshRenderer>();
				if (!flag)
				{
					this.mBounds = default(Bounds);
					flag = true;
				}
				if (component3 != null)
				{
					this.mBounds.Encapsulate(component3.bounds);
				}
				else
				{
					TerrainCollider component4 = activeTerrain.GetComponent<TerrainCollider>();
					if (!(component4 != null))
					{
						global::Debug.LogError(new object[]
						{
							"Could not get measure bounds of terrain.",
							this
						});
						return;
					}
					this.mBounds.Encapsulate(component4.bounds);
				}
			}
			GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
			if (array != null)
			{
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					GameObject gameObject = array[i];
					if (gameObject.layer != base.gameObject.layer)
					{
						if (NJGMapBase.IsInRenderLayers(gameObject, this.boundLayers))
						{
							if (!flag)
							{
								this.mBounds = new Bounds(gameObject.transform.position, new Vector3(1f, 1f, 1f));
								flag = true;
							}
							Renderer renderer = gameObject.renderer;
							if (renderer != null)
							{
								this.mBounds.Encapsulate(renderer.bounds);
							}
							else
							{
								Collider collider = gameObject.collider;
								if (collider != null)
								{
									this.mBounds.Encapsulate(collider.bounds);
								}
							}
						}
					}
					i++;
				}
			}
			if (!flag)
			{
				global::Debug.Log(new object[]
				{
					"Could not find terrain nor any other bounds in scene",
					this
				});
				this.mBounds = new Bounds(base.gameObject.transform.position, new Vector3(1f, 1f, 1f));
			}
			this.mBounds.Expand(new Vector3((float)this.renderOffset, 0f, (float)this.renderOffset));
			if (this.mapResolution == NJGMapBase.Resolution.Double)
			{
			}
			this.bounds = this.mBounds;
		}

		public string[] GetZones(string level)
		{
			List<string> list = new List<string>();
			if (this.levels != null)
			{
				int i = 0;
				int count = this.levels.Count;
				while (i < count)
				{
					if (this.levels[i].level == level)
					{
						int j = 0;
						int count2 = this.levels[i].zones.Count;
						while (j < count2)
						{
							list.Add(this.levels[i].zones[j].type);
							j++;
						}
					}
					i++;
				}
			}
			string[] arg_BC_0;
			if (list.Count == 0)
			{
				(arg_BC_0 = new string[1])[0] = "No Zones defined";
			}
			else
			{
				arg_BC_0 = list.ToArray();
			}
			return arg_BC_0;
		}

		public string[] GetLevels()
		{
			List<string> list = new List<string>();
			if (this.levels != null)
			{
				int i = 0;
				int count = this.levels.Count;
				while (i < count)
				{
					list.Add(this.levels[i].level);
					i++;
				}
			}
			string[] arg_6A_0;
			if (list.Count == 0)
			{
				(arg_6A_0 = new string[1])[0] = "No Levels defined";
			}
			else
			{
				arg_6A_0 = list.ToArray();
			}
			return arg_6A_0;
		}

		public Color GetZoneColor(string level, string zone)
		{
			Color white = Color.white;
			int i = 0;
			int count = this.levels.Count;
			while (i < count)
			{
				if (this.levels[i].level == level)
				{
					int j = 0;
					int count2 = this.levels[i].zones.Count;
					while (j < count2)
					{
						if (this.levels[i].zones[j].type.Equals(zone))
						{
							return this.levels[i].zones[j].color;
						}
						j++;
					}
				}
				i++;
			}
			return white;
		}

		public bool GetInteraction(int type)
		{
			return this.Get(type) != null && this.Get(type).enableInteraction;
		}

		public Color GetColor(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).color : Color.white;
		}

		public bool GetAnimateOnVisible(int type)
		{
			return this.Get(type) != null && this.Get(type).animateOnVisible;
		}

		public bool GetAnimateOnAction(int type)
		{
			return this.Get(type) != null && this.Get(type).showOnAction;
		}

		public bool GetLoopAnimation(int type)
		{
			return this.Get(type) != null && this.Get(type).loopAnimation;
		}

		public bool GetHaveArrow(int type)
		{
			return this.Get(type) != null && this.Get(type).haveArrow;
		}

		public float GetFadeOutAfter(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).fadeOutAfterDelay : 0f;
		}

		public bool GetRotate(int type)
		{
			return this.Get(type) != null && this.Get(type).rotate;
		}

		public bool GetArrowRotate(int type)
		{
			return this.Get(type) != null && this.Get(type).arrowRotate;
		}

		public bool GetUpdatePosition(int type)
		{
			return this.Get(type) != null && this.Get(type).updatePosition;
		}

		public int GetSize(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).size : 0;
		}

		public int GetBorderSize(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).borderSize : 0;
		}

		public bool GetCustom(int type)
		{
			return this.Get(type) != null && this.Get(type).useCustomSize;
		}

		public bool GetCustomBorder(int type)
		{
			return this.Get(type) != null && this.Get(type).useCustomBorderSize;
		}

		public int GetDepth(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).depth : 0;
		}

		public int GetArrowDepth(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).arrowDepth : 0;
		}

		public int GetArrowOffset(int type)
		{
			return (this.Get(type) != null) ? this.Get(type).arrowOffset : 0;
		}

		public NJGMapBase.MapItemType Get(int type)
		{
			if (type == -1)
			{
				return null;
			}
			if (type > this.mapItems.Count)
			{
				return null;
			}
			NJGMapBase.MapItemType mapItemType = this.mapItems[type];
			return (mapItemType != null) ? mapItemType : null;
		}
	}
}
