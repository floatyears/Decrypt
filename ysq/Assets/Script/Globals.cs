using cn.sharesdk.unity3d;
using Holoville.HOTween;
using NtUniSdk.Unity3d;
using System;
using UnityEngine;
using Util;
using com.tencent.bugly.unity3d;

public sealed class Globals : MonoBehaviour
{
	private int platformFlag;

	public static Globals Instance
	{
		get;
		private set;
	}

	public GameManager GameMgr
	{
		get;
		private set;
	}

	public SceneManager SenceMgr
	{
		get;
		private set;
	}

	public AttDatabase AttDB
	{
		get;
		private set;
	}

	public ClientSession CliSession
	{
		get;
		private set;
	}

	public ActorManager ActorMgr
	{
		get;
		private set; 
	}

	public CameraManager CameraMgr
	{
		get;
		private set;
	}

	public TutorialManager TutorialMgr
	{
		get;
		private set;
	}

	public BackgroundMusicManager BackgroundMusicMgr
	{
		get;
		private set;
	}

	public EffectSoundManager EffectSoundMgr
	{
		get;
		private set;
	}

	public LocalPlayer Player
	{
		get;
		private set;
	}

	public SpeedHackDetector SpeedHackDetector
	{
		get;
		private set;
	}

	public DownloadManager DownloadMgr
	{
		get;
		private set;
	}

	public ResourceLoaderManager ResourceCache
	{
		get;
		private set;
	}

	public NotificationManager NotifyMgr
	{
		get;
		private set;
	}

	public VoiceRecorderManager VoiceMgr
	{
		get;
		private set;
	}

	public static int DeviceWidth
	{
		get;
		private set;
	}

	public static int DeviceHeight
	{
		get;
		private set;
	}

	private void Awake()
	{
		Globals.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(Globals.Instance);
		Application.targetFrameRate = 30;
		Time.fixedDeltaTime = 0.0333333351f;
		UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
		UtilFunc.SetSeed((uint)UnityEngine.Random.seed);
		Globals.DeviceWidth = Screen.width;
		Globals.DeviceHeight = Screen.height;
	}

	private void OnDestroy()
	{
		Globals.Instance = null;
	}

	private void Start()
	{
		NotificationManager.RegisterNotifyTypes();
		this.DownloadMgr = base.gameObject.AddComponent<DownloadManager>();
		this.InitUniSDK();
		this.InitBuglySDK();
		this.InitShareSDK();
		this.InitGamePad();
		base.gameObject.AddComponent<GameAnalytics>();
		GameSetting.ReadConfigFile();
		Screen.sleepTimeout = ((!GameSetting.Data.NoScreenLock) ? -2 : -1);
		Tools.SetQualityLevel((Tools.CustomQualityLevel)GameSetting.Data.GraphQuality);
		this.AttDB = base.gameObject.AddComponent<AttDatabase>();
		this.GameMgr = base.gameObject.AddComponent<GameManager>();
		this.SenceMgr = base.gameObject.AddComponent<SceneManager>();
		this.CliSession = base.gameObject.AddComponent<ClientSession>();
		this.ActorMgr = base.gameObject.AddComponent<ActorManager>();
		this.CameraMgr = base.gameObject.AddComponent<CameraManager>();
		this.TutorialMgr = base.gameObject.AddComponent<TutorialManager>();
		Tools.GetSafeComponent<AudioListener>(base.gameObject);
		Tools.GetSafeComponent<AudioSource>(base.gameObject);
		this.BackgroundMusicMgr = base.gameObject.AddComponent<BackgroundMusicManager>();
		this.EffectSoundMgr = base.gameObject.AddComponent<EffectSoundManager>();
		this.Player = base.gameObject.AddComponent<LocalPlayer>();
		this.NotifyMgr = base.gameObject.AddComponent<NotificationManager>();
		this.VoiceMgr = base.gameObject.AddComponent<VoiceRecorderManager>();
		this.ResourceCache = base.gameObject.AddComponent<ResourceLoaderManager>();
		this.platformFlag |= 1;
		this.platformFlag |= 4;
		this.platformFlag |= 8;
		this.platformFlag |= 32;
		this.platformFlag |= 128;
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(false);
	}

	public void InitSpeedHackDetector()
	{
		if (this.SpeedHackDetector == null)
		{
			this.SpeedHackDetector = base.gameObject.AddComponent<SpeedHackDetector>();
		}
	}

	private void OnApplicationQuit()
	{
		if (GameSetting.UpdateNow)
		{
			GameSetting.UpdateNow = false;
			GameSetting.WriteConfigFile();
		}
		if (GameCache.UpdateNow)
		{
			GameCache.UpdateNow = false;
			GameCache.SaveCacheData();
		}
	}

	public bool IsSimulater()
	{
		return (this.platformFlag & 1 << (int)Application.platform) != 0;
	}

	private void InitUniSDK()
	{
		GameObject gameObject = new GameObject("UniSDK");
		gameObject.AddComponent<SdkU3dCallback>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		SdkU3d.setCallbackModule("UniSDK");
		SdkU3d.setPropStr("unity3d", "true");
	}

	private void InitBuglySDK()
	{
		BuglyAgent.ConfigDebugMode(false);
        BuglyAgent.ConfigAutoReportLogLevel((LogSeverity)5); 
		BuglyAgent.ConfigAutoQuitApplication(false);
		BuglyAgent.ConfigDefault(SdkU3d.getAppChannel(), GameSetting.GameVersion, SystemInfo.deviceUniqueIdentifier, -1L);
		BuglyAgent.InitWithAppId("900002066");
		BuglyAgent.EnableExceptionHandler();
	}

	private void InitShareSDK()
	{
		GameObject gameObject = new GameObject("ShareSDK");
		ShareSDKCallback callbackModule = gameObject.AddComponent<ShareSDKCallback>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		ShareSDK.setCallbackModule(callbackModule);
		ShareSDK.Init();
	}

	private void InitGamePad()
	{
		GameObject gameObject = new GameObject("GamePadSDK");
		gameObject.AddComponent<GamePadMgr>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		GamePadMgr.setCallbackModule("GamePadSDK");
		GamePadMgr.init();
	}
}
