using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
	public enum SenceState
	{
		Unknown,
		PrepareLoad,
		Downloading,
		Loading,
		Loaded
	}

	public delegate void SenceCallback(SceneInfo senceInfo);

	public static bool PendingGCCollect;

	public SceneManager.SenceCallback SencePreLoadEvent;

	public SceneManager.SenceCallback SencePreLoadedEvent;

	public SceneManager.SenceCallback SenceLoadedEvent;

	private AsyncOperation AsyncIO;

	private Coroutine LoadSceneCoroutine;

	public SceneInfo sceneInfo
	{
		get;
		private set;
	}

	public SceneManager.SenceState senceState
	{
		get;
		private set;
	}

	public float progress
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.sceneInfo = null;
		this.senceState = SceneManager.SenceState.Unknown;
	}

	private void Start()
	{
		base.StartCoroutine(SceneManager.DoGC());
	}

	public void LoadScene(int sceneID)
	{
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneID);
		if (info == null || this.sceneInfo == info)
		{
			return;
		}
		if (this.LoadSceneCoroutine != null)
		{
			base.StopCoroutine(this.LoadSceneCoroutine);
		}
		this.LoadSceneCoroutine = base.StartCoroutine(this.DoLoadScene(info));
	}

	[DebuggerHidden]
	private IEnumerator DoLoadScene(SceneInfo _senceInfo)
	{
        this.senceState = SenceState.PrepareLoad;
        progress = 0f;
        OnSencePreLoad(_senceInfo);
        Res.LoadScene(_senceInfo.ResLoc);
        AsyncIO = Application.LoadLevelAdditiveAsync(_senceInfo.ResLoc);
        senceState = SenceState.Loading;
        while(!AsyncIO.isDone)
        {
            progress = AsyncIO.progress * 0.96f;
            yield return 0;
        }
        AsyncIO = null;
        sceneInfo = _senceInfo;
        senceState = SceneManager.SenceState.Loaded;
        yield return new WaitForEndOfFrame();
        OnSencePreLoaded();
        while(!Globals.Instance.ResourceCache.IsAllResourceLoaded())
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.DoGCCollect();
        OnSenceLoaded();
        LoadSceneCoroutine = null;
        progress = 1f;
	}

	public void CloseScene()
	{
		Globals.Instance.ActorMgr.ClearActor();
		Globals.Instance.GameMgr.ClearSpeedMod();
		if (this.sceneInfo == null)
		{
			return;
		}
		Application.LoadLevel("Empty");
		Res.ForceUnloadAssets(false);
		GameUIManager.mInstance.DestroyHUDTextManager();
		this.sceneInfo = null;
		this.senceState = SceneManager.SenceState.Unknown;
	}

	public static void DoGCCollect()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		SceneManager.PendingGCCollect = false;
	}

	[DebuggerHidden]
	private static IEnumerator DoGC()
	{
        if(Application.isPlaying)
        {
            while(SceneManager.PendingGCCollect)
            {
                SceneManager.DoGCCollect();
                yield return 0;
            }
        }
	}

	private void OnSencePreLoad(SceneInfo _senceInfo)
	{
		if (this.SencePreLoadEvent != null)
		{
			this.SencePreLoadEvent(_senceInfo);
		}
	}

	private void OnSencePreLoaded()
	{
		if (this.SencePreLoadedEvent != null)
		{
			this.SencePreLoadedEvent(this.sceneInfo);
		}
	}

	private void OnSenceLoaded()
	{
		if (Camera.main != null)
		{
			AudioListener component = Camera.main.gameObject.GetComponent<AudioListener>();
			if (component != null)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
		}
		if (QualitySettings.GetQualityLevel() == 0)
		{
			RenderSettings.fog = false;
		}
		if (this.SenceLoadedEvent != null)
		{
			this.SenceLoadedEvent(this.sceneInfo);
		}
	}
}
