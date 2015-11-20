using Att;
using NtUniSdk.Unity3d;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Game/GUI Manager")]
public sealed class GameUIManager : MonoBehaviour
{
	public delegate void GUICallback<T>(T session) where T : GameUISession;

	public const int UI_DESIGN_WIDTH = 1136;

	public const int UI_DESIGN_HEIGHT = 640;

	public const int UI_MAX_DESIGN_HEIGHT = 720;

	public const int UI_Z_OFFEST = 5000;

	public static GameUIManager mInstance;

	private List<GameUISession> guiSessions = new List<GameUISession>();

	private GameUISession curUISession;

	private int loadingSceneID;

	private GUISceneLoading loadingUISession;

	private Stack<Type> BackSessionTypes = new Stack<Type>();

	public Type PreSessionType;

	public GameUIState uiState = new GameUIState();

	public bool CanEscape = true;

	private GUIGameStateTip mGameState;

	private UIVirtualPad m_virtualPad;

	private GameObject fps;

	private GUIMiniMap m_minimap;

	private TopGoods m_TopGoods;

	private GUIIndicator indicator;

	private GUIFadeBG fadeBG;

	private int showTimes;

	private int tempTimes;

	private HUDTextManager hudTextManager;

	private MessageTip messageTip;

	private GameUIMsgWindow mGameUIMsgWindow;

	private GUIPlotDialog uiDialog;

	private GUIGameCountDownMsg mGameCDMsg;

	private GUIBattleCountDown mBattleCountDown;

	private GUIGameNewsMsg mGUIGameNewsMsg;

	private GUIGameNewsMsg mGUISystemNoticeMsg;

	private GUIGameNewsMsgPopUp mGUIGameNewsMsgPopUp;

	private DynamicUpdate loginBG;

	private GUIBattleWarnning mBattleWarnning;

	private GameUIOptionPopUp mGameUIOptionPopUp;

	private GUIWebViewPopUp mGUIWebViewPopUp;

	private GUIPetInfoSceneV2 mGUIPetInfoSceneV2;

	private GUIPetFurtherSucV2 mGUIPetFurtherSucV2;

	private GUIPetQualityUp mGUIPetQualityUp;

	private GUITrialInGamePopUp mGUITrialInGamePopUp;

	public UICamera uiCamera
	{
		get;
		private set;
	}

	public UIRoot uiRoot
	{
		get;
		private set;
	}

	public GameUISession CurUISession
	{
		get
		{
			return this.curUISession;
		}
	}

	public HUDTextManager HUDTextMgr
	{
		get
		{
			if (this.hudTextManager == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/HUDTextManager");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/HUDTextManager error"
					});
					return null;
				}
				GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
				if (gameObject2 == null)
				{
					global::Debug.LogError(new object[]
					{
						"AddChild HUDTextManager error"
					});
					return null;
				}
				this.hudTextManager = gameObject2.AddComponent<HUDTextManager>();
				Vector3 localPosition = gameObject2.transform.localPosition;
				localPosition.z += 5000f;
				gameObject2.transform.localPosition = localPosition;
			}
			return this.hudTextManager;
		}
	}

	private void Awake()
	{
		GameUIManager.mInstance = this;
		this.uiCamera = base.gameObject.GetComponentInChildren<UICamera>();
		if (this.uiCamera == null)
		{
			global::Debug.LogError(new object[]
			{
				"Can not find UICamera component!"
			});
			return;
		}
		this.CreateLoginBG();
	}

	private void OnDestroy()
	{
		this.DestroyAll();
		GameUIManager.mInstance = null;
	}

	public void DestroyAll()
	{
		this.BackSessionTypes.Clear();
		GameMessageBox.Instance = null;
		NetworkMessageBox.Instance = null;
		HackerMessageBox.Instance = null;
		this.DestroyHUDTextManager();
		this.DestroyMessageTip();
		this.CloseDPad();
		this.CloseTopGoods();
		this.CloseGameStateTip();
		this.CloseGameCDMsg();
		this.CloseBattleCDMsg();
		this.DestroyGameNewsMsg();
		this.DestroyGameNewsMsgPopUp();
		this.CloseDialog();
		this.DestroyPetInfoSceneV2();
		this.DestroyGameUIOptionPopUp();
		GetPetLayer.TryDestroy();
		GUIGuildCraftTeamInfoPop.CloseMe();
	}

	private void Start()
	{
		UIRoot[] array = NGUITools.FindActive<UIRoot>();
		for (int i = 0; i < array.Length; i++)
		{
			UIRoot uIRoot = array[i];
			this.uiRoot = uIRoot;
			this.uiRoot.manualHeight = 640;
			this.uiRoot.maximumHeight = 720;
			UnityEngine.Object.DontDestroyOnLoad(uIRoot.gameObject);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && this.CanEscape)
		{
			if (Globals.Instance.SenceMgr.sceneInfo != null && Globals.Instance.TutorialMgr.Tutorial != null)
			{
				if (Globals.Instance.SenceMgr.sceneInfo.ID != GameConst.GetInt32(110))
				{
					GameUIManager.mInstance.ShowGameUIOptionPopUp();
				}
			}
			else if (Globals.Instance.TutorialMgr.Tutorial == null)
			{
				if (GameMessageBox.TryClose())
				{
					return;
				}
				if (GetPetLayer.TryDestroy())
				{
					return;
				}
				if (GUIPlayerInfoPopUp.TryClose())
				{
					return;
				}
				if (GUIFriendInfoPopUp.TryClose())
				{
					return;
				}
				if (GUIGuildSignLogPopUp.TryClose())
				{
					return;
				}
				if (this.GetPetInfoSceneV2() != null)
				{
					this.DestroyPetInfoSceneV2();
				}
				else
				{
					if (GUILopetInfoScene.TryClose())
					{
						return;
					}
					if (this.DestroyPetFurtherSucV2())
					{
						return;
					}
					if (this.DestroyPetQualityUp())
					{
						return;
					}
					if (GUIMagicMirrorExchangeSuccess.TryClose())
					{
						return;
					}
					if (GUISummonLopetSuccess.TryClose())
					{
						return;
					}
					if (GUILopetAwakeSuccess.TryClose())
					{
						return;
					}
					if (GUIPassCombatPopUp.TryClose())
					{
						return;
					}
					if (GUIBagFullPopUp.TryClose())
					{
						return;
					}
					if (GameUILevelupPanel.TryClose())
					{
						return;
					}
					if (GameUIPopupManager.GetInstance().GetStackSize() > 0)
					{
						Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
						if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIAgreementInfoPopUp && !GameSetting.Data.UserAgreement)
						{
							return;
						}
						GameUIPopupManager.eSTATE stateByIndex = GameUIPopupManager.GetInstance().GetStateByIndex(1);
						GameUIPopupManager.eSTATE eSTATE = stateByIndex;
						if (eSTATE != GameUIPopupManager.eSTATE.GUIEquipInfoPopUp)
						{
							GameUIPopupManager.GetInstance().PopState(false, null);
						}
						else
						{
							GameUIPopupManager.GetInstance().PopState(true, null);
							GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
						}
					}
					else
					{
						if (GUIPetViewPopUp.TryClose())
						{
							return;
						}
						if (ItemsBox.TryClose())
						{
							return;
						}
						if (this.curUISession != null && this.guiSessions != null && this.guiSessions.Count > 0 && this.curUISession != this.guiSessions[this.guiSessions.Count - 1])
						{
							Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
							this.guiSessions[this.guiSessions.Count - 1].Close();
							return;
						}
						if (GUIRollingSceneV2.TryClose())
						{
							return;
						}
						if (this.m_TopGoods != null && this.m_TopGoods.gameObject.activeInHierarchy)
						{
							this.m_TopGoods.OnBackClicked(null);
						}
						else if (SdkU3d.hasFeature("FEATURE_EXIT_VIEW"))
						{
							SdkU3d.ntOpenExitView();
						}
						else if (Globals.Instance.GameMgr.Status != GameManager.EGameStatus.EGS_None)
						{
							GameMessageBox.ShowApplicationQuitBox();
						}
					}
				}
			}
		}
	}

	public T GetSession<T>() where T : GameUISession
	{
		Type typeFromHandle = typeof(T);
		for (int i = 0; i < this.guiSessions.Count; i++)
		{
			if (this.guiSessions[i] != null && (this.guiSessions[i].GetType() == typeFromHandle || this.guiSessions[i].GetType().IsSubclassOf(typeFromHandle)))
			{
				return (T)((object)this.guiSessions[i]);
			}
		}
		return (T)((object)null);
	}

	public bool IsSessionExisted(Type[] sessions)
	{
		for (int i = 0; i < this.guiSessions.Count; i++)
		{
			if (!(this.guiSessions[i] == null))
			{
				for (int j = 0; j < sessions.Length; j++)
				{
					Type type = this.guiSessions[i].GetType();
					Type type2 = sessions[j];
					if (type == type2 || type.IsSubclassOf(type2))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void CloseAllSession()
	{
		for (int i = 0; i < this.guiSessions.Count; i++)
		{
			if (this.guiSessions[i] != null)
			{
				this.guiSessions[i].CloseNotRemoveSession();
			}
		}
		this.curUISession = null;
		this.guiSessions.Clear();
		this.BackSessionTypes.Clear();
	}

	public T CreateSession<T>(GameUIManager.GUICallback<T> callback) where T : GameUISession
	{
		T session = this.GetSession<T>();
		if (session != null)
		{
			if (callback != null)
			{
				callback(session);
			}
			return session;
		}
		return this.DoLoadingGUISession<T>(callback);
	}

	public void RemoveSession(GameUISession session)
	{
		this.guiSessions.Remove(session);
	}

	private T DoLoadingGUISession<T>(GameUIManager.GUICallback<T> cb) where T : GameUISession
	{
		GameObject gameObject = Res.LoadGUI("SceneUI/" + typeof(T).ToString());
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("LoadGUI Error : {0}", new object[]
			{
				typeof(T).ToString()
			});
			return (T)((object)null);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogErrorFormat("GameObject Instantiate Error : {0}", new object[]
			{
				gameObject.name
			});
			return (T)((object)null);
		}
		GameObject gameObject3 = GameUIManager.mInstance.uiCamera.gameObject;
		Transform transform = gameObject2.transform;
		transform.parent = gameObject3.transform;
		gameObject2.layer = gameObject3.layer;
		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.z += 5000f;
		transform.localPosition = localPosition;
		transform.localRotation = gameObject.transform.localRotation;
		transform.localScale = gameObject.transform.localScale;
		T t = gameObject2.GetComponent<T>();
		if (t == null)
		{
			t = gameObject2.AddComponent<T>();
		}
		this.guiSessions.Add(t);
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		GameUIManager.mInstance.SaveFadeBGTimes();
		t._OnLoadedGUI();
		base.StartCoroutine(this.OnLoadedGUI<T>(t, cb));
		return t;
	}

	[DebuggerHidden]
	private IEnumerator OnLoadedGUI<T>(T gui, GameUIManager.GUICallback<T> cb) where T : GameUISession
	{
        return null;
        //GameUIManager.<OnLoadedGUI>c__Iterator51<T> <OnLoadedGUI>c__Iterator = new GameUIManager.<OnLoadedGUI>c__Iterator51<T>();
        //<OnLoadedGUI>c__Iterator.cb = cb;
        //<OnLoadedGUI>c__Iterator.gui = gui;
        //<OnLoadedGUI>c__Iterator.<$>cb = cb;
        //<OnLoadedGUI>c__Iterator.<$>gui = gui;
        //return <OnLoadedGUI>c__Iterator;
	}

	public void ClearGobackSession()
	{
		this.BackSessionTypes.Clear();
	}

	public Type GetPeekSessionType()
	{
		if (this.BackSessionTypes.Count == 0)
		{
			return null;
		}
		return this.BackSessionTypes.Peek();
	}

	public Type PopBackSessionType()
	{
		if (this.BackSessionTypes.Count == 0)
		{
			return null;
		}
		return this.BackSessionTypes.Pop();
	}

	public Type GobackSession()
	{
        System.Type type = null;
        do
        {
            if (this.BackSessionTypes.Count == 0)
            {
                break;
            }
            type = this.BackSessionTypes.Pop();
            if (type == this.curUISession.GetType())
            {
                type = null;
            }
        }
        while (type == null);
        if (type != null)
        {
            System.Type[] typeArray1 = new System.Type[] { type };
            object[] objArray1 = new object[3];
            objArray1[1] = false;
            objArray1[2] = true;
            base.GetType().GetMethod("ChangeSession").MakeGenericMethod(typeArray1).Invoke(this, objArray1);
            return type;
        }
        mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
        return typeof(GUIMainMenuScene);
	}

	public void ChangeSession<T>(GameUIManager.GUICallback<T> callback = null, bool loadingUI = false, bool backUISession = true) where T : GameUISession
	{
		Type typeFromHandle = typeof(T);
		if (this.curUISession != null && this.curUISession.GetType() == typeFromHandle)
		{
			global::Debug.LogWarning(new object[]
			{
				string.Format("Target UISession is the curUISession Type[{0}]!", this.curUISession.GetType().ToString())
			});
			return;
		}
		if (this.BackSessionTypes.Count > 0 && this.BackSessionTypes.Peek() == typeFromHandle)
		{
			this.BackSessionTypes.Pop();
		}
		if (backUISession)
		{
			this.BackSessionTypes.Push(typeFromHandle);
		}
		this.DoChangingSession<T>(callback, loadingUI);
	}

	private void DoChangingSession<T>(GameUIManager.GUICallback<T> callback, bool loadingUI) where T : GameUISession
	{
		if (this.curUISession != null)
		{
			this.PreSessionType = this.curUISession.GetType();
		}
		this.CloseCurrentSession();
		if (loadingUI)
		{
			base.StartCoroutine(this.OnLoadedSession<T>(callback));
		}
		else
		{
			this.curUISession = this.CreateSession<T>(callback);
		}
	}

	[DebuggerHidden]
	private IEnumerator OnLoadedSession<T>(GameUIManager.GUICallback<T> callback) where T : GameUISession
	{
        return null;
        //GameUIManager.<OnLoadedSession>c__Iterator52<T> <OnLoadedSession>c__Iterator = new GameUIManager.<OnLoadedSession>c__Iterator52<T>();
        //<OnLoadedSession>c__Iterator.callback = callback;
        //<OnLoadedSession>c__Iterator.<$>callback = callback;
        //<OnLoadedSession>c__Iterator.<>f__this = this;
        //return <OnLoadedSession>c__Iterator;
	}

	private void CloseCurrentSession()
	{
		if (this.curUISession != null)
		{
			this.curUISession.CloseImmediate();
			this.curUISession = null;
			GUIRewardPanel.CloseAll();
		}
	}

	public void LoadScene(int sceneID)
	{
		if (sceneID == 0)
		{
			return;
		}
		Globals.Instance.ActorMgr.RecvStartTime = Globals.Instance.Player.GetTimeStamp();
		if (this.loadingSceneID == 0)
		{
			SceneManager expr_3A = Globals.Instance.SenceMgr;
			expr_3A.SenceLoadedEvent = (SceneManager.SenceCallback)Delegate.Combine(expr_3A.SenceLoadedEvent, new SceneManager.SenceCallback(this.OnSenceLoadedEvent));
		}
		this.loadingSceneID = sceneID;
		this.CloseCurrentSession();
		GameObject gameObject = Res.LoadGUI("SceneUI/GUISceneLoading");
		if (gameObject != null)
		{
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			if (gameObject2 != null)
			{
				Vector3 localPosition = gameObject2.transform.localPosition;
				localPosition.z += 4000f;
				gameObject2.transform.localPosition = localPosition;
				this.loadingUISession = gameObject2.AddComponent<GUISceneLoading>();
				this.loadingUISession.SceneLoading = true;
				this.loadingUISession.UpdateFlag = true;
				this.loadingUISession._OnLoadedGUI();
			}
		}
		base.StartCoroutine(this.OnLoadedScene());
	}

	[DebuggerHidden]
	private IEnumerator OnLoadedScene()
	{
        return null;
        //GameUIManager.<OnLoadedScene>c__Iterator53 <OnLoadedScene>c__Iterator = new GameUIManager.<OnLoadedScene>c__Iterator53();
        //<OnLoadedScene>c__Iterator.<>f__this = this;
        //return <OnLoadedScene>c__Iterator;
	}

	private void OnSenceLoadedEvent(SceneInfo senceInfo)
	{
		this.curUISession = this.CreateSession<GUICombatMain>(null);
		if (this.loadingUISession != null)
		{
			this.loadingUISession.Loaded = true;
			this.loadingUISession.MaxProgress = 1f;
		}
	}

	[DebuggerHidden]
	private IEnumerator DoSessionScene<T>() where T : GameUISession
	{
        return null;
        //GameUIManager.<DoSessionScene>c__Iterator54<T> <DoSessionScene>c__Iterator = new GameUIManager.<DoSessionScene>c__Iterator54<T>();
        //<DoSessionScene>c__Iterator.<>f__this = this;
        //return <DoSessionScene>c__Iterator;
	}

	public void LoadSessionScene<T>() where T : GameUISession
	{
		this.CloseCurrentSession();
		base.StartCoroutine(this.DoSessionScene<T>());
	}

	public void GameStateChange(GUIGameStateTip.EGAMEING_STATE _eGameState, int waveNum = 0)
	{
		if (this.mGameState == null && _eGameState != GUIGameStateTip.EGAMEING_STATE.NONE)
		{
			GameObject gameObject = Res.LoadGUI("GUI/GUIGameStateTip");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIGameStateTip error"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return;
			}
			this.mGameState = gameObject2.AddComponent<GUIGameStateTip>();
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
		}
		if (this.mGameState != null)
		{
			this.mGameState.ChangeState(_eGameState, waveNum);
		}
	}

	public void CloseGameStateTip()
	{
		if (this.mGameState != null)
		{
			UnityEngine.Object.Destroy(this.mGameState.gameObject);
			this.mGameState = null;
		}
	}

	public void ShowCombatPaopaoTip(ActorController target, string content, float showTime = 3f)
	{
		GameObject gameObject = Res.LoadGUI("GUI/GUICombatPaopaoTip");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUICombatPaopaoTip error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild error"
			});
			return;
		}
		GUICombatPaopaoTip gUICombatPaopaoTip = gameObject2.AddComponent<GUICombatPaopaoTip>();
		if (gUICombatPaopaoTip != null)
		{
			gUICombatPaopaoTip.InitWithActorController(target, content, showTime);
		}
	}

	public void ShowCombatPaopaoTip(ActorController target, int sayID, float showTime = 3f)
	{
		if (sayID == 0)
		{
			return;
		}
		SayInfo info = Globals.Instance.AttDB.SayDict.GetInfo(sayID);
		if (info == null)
		{
			return;
		}
		this.ShowCombatPaopaoTip(target, info.Content, showTime);
	}

	public void CreateDPad(GameObject parent)
	{
		if (this.m_virtualPad == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/Global/VirtualPad");
			if (gameObject != null)
			{
				GameObject gameObject2 = NGUITools.AddChild(parent, gameObject);
				if (gameObject2 != null)
				{
					this.m_virtualPad = gameObject2.AddComponent<UIVirtualPad>();
				}
			}
		}
	}

	public UIVirtualPad GetDPad()
	{
		return this.m_virtualPad;
	}

	public void CloseDPad()
	{
		if (this.m_virtualPad != null)
		{
			UnityEngine.Object.Destroy(this.m_virtualPad.gameObject);
			this.m_virtualPad = null;
		}
	}

	public void ShowFPS()
	{
		if (this.fps == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/Global/FPS");
			if (gameObject != null)
			{
				this.fps = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			}
			if (this.fps != null)
			{
				Vector3 localPosition = this.fps.transform.localPosition;
				localPosition.z += 5000f;
				this.fps.transform.localPosition = localPosition;
			}
		}
		if (this.fps != null)
		{
			this.fps.SetActive(true);
		}
	}

	public void HideFPS()
	{
		if (this.fps == null)
		{
			return;
		}
		this.fps.SetActive(false);
	}

	public void CreateMiniMap()
	{
		if (this.m_minimap == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/GUIMiniMap");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Load GUI/GUIMiniMap error!"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			this.m_minimap = gameObject2.AddComponent<GUIMiniMap>();
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
		}
		else if (!this.m_minimap.gameObject.activeSelf)
		{
			this.m_minimap.gameObject.SetActive(true);
		}
	}

	public void CloseMiniMap()
	{
		if (this.m_minimap != null)
		{
			this.m_minimap.gameObject.SetActive(false);
		}
	}

	public void DestoryMiniMap()
	{
		if (this.m_minimap != null)
		{
			UnityEngine.Object.Destroy(this.m_minimap.gameObject);
			this.m_minimap = null;
		}
	}

	public GUIMiniMap GetMiniMap()
	{
		return this.m_minimap;
	}

	public TopGoods GetTopGoods()
	{
		if (this.m_TopGoods == null)
		{
			this.CreateTopGoods();
		}
		return this.m_TopGoods;
	}

	public void CreateTopGoods()
	{
		if (this.m_TopGoods == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/Global/TopGoods");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Load GUI/TopGoods error!"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			this.m_TopGoods = gameObject2.AddComponent<TopGoods>();
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
		}
	}

	public void CloseTopGoods()
	{
		if (this.m_TopGoods != null)
		{
			this.m_TopGoods.Hide();
		}
	}

	public void ShowIndicate()
	{
		if (this.indicator == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/Global/Indicator");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Load GUI/Indicator error!"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z -= 1000f;
			gameObject2.transform.localPosition = localPosition;
			this.indicator = gameObject2.AddComponent<GUIIndicator>();
		}
		if (this.indicator != null)
		{
			this.indicator.gameObject.SetActive(true);
		}
	}

	public void HideIndicate()
	{
		if (this.indicator != null)
		{
			this.indicator.gameObject.SetActive(false);
		}
	}

	private void SaveFadeBGTimes()
	{
		this.tempTimes = this.showTimes;
	}

	public void ShowFadeBG(int renderQ = 5900, int z = 3000)
	{
		if (this.fadeBG == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/Global/FadeBG");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Load GUI/Global/FadeBG error!"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			this.fadeBG = gameObject2.AddComponent<GUIFadeBG>();
			this.fadeBG.gameObject.SetActive(false);
		}
		if (this.fadeBG != null)
		{
			if (this.fadeBG.gameObject.activeInHierarchy)
			{
				this.showTimes++;
			}
			else
			{
				this.fadeBG.gameObject.SetActive(true);
			}
			this.fadeBG.SetRQ(renderQ);
			Vector3 localPosition = this.fadeBG.transform.localPosition;
			localPosition.z = (float)z;
			this.fadeBG.transform.localPosition = localPosition;
		}
	}

	public void HideFadeBG(bool hideSave = false)
	{
		if (this.fadeBG != null)
		{
			if (hideSave && this.tempTimes > 0)
			{
				this.showTimes -= this.tempTimes;
			}
			if (this.showTimes > 0)
			{
				this.showTimes--;
			}
			else
			{
				this.fadeBG.gameObject.SetActive(false);
			}
		}
	}

	public void DestroyHUDTextManager()
	{
		if (this.hudTextManager != null)
		{
			UnityEngine.Object.Destroy(this.hudTextManager.gameObject);
			this.hudTextManager = null;
		}
	}

	public void DestroyMessageTip()
	{
		if (this.messageTip != null)
		{
			UnityEngine.Object.Destroy(this.messageTip.gameObject);
			this.messageTip = null;
		}
	}

	public void ShowMessageTip(string text, float localPosX = 0f, float localPosY = 0f)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.messageTip == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/MessageTip");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/MessageTip error"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return;
			}
			this.messageTip = gameObject2.AddComponent<MessageTip>();
			this.messageTip.Init();
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
		}
		Vector3 localPosition2 = this.messageTip.transform.localPosition;
		localPosition2.z = -2000f;
		this.messageTip.transform.localPosition = localPosition2;
		this.messageTip.SetText(text);
	}

	public void ShowMessageTipByKey(string key, float localPosX = 0f, float localPosY = 0f)
	{
		this.ShowMessageTip(Singleton<StringManager>.Instance.GetString(key), localPosX, localPosY);
	}

	public void ShowMessageTip(string prefix, int result)
	{
		string text = string.Format("{0}_{1}", prefix, result);
		string @string = Singleton<StringManager>.Instance.GetString(text);
		if (string.IsNullOrEmpty(@string))
		{
			this.ShowMessageTip(text, 0f, 0f);
			global::Debug.Log(new object[]
			{
				string.Format("string table can't find the key, key = {0}", text)
			});
			return;
		}
		this.ShowMessageTip(@string, 0f, 0f);
	}

	public GameUIMsgWindow ShowMsgWindow()
	{
		if (this.mGameUIMsgWindow == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/Global/GameUIMsgWindow");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GameUIMsgWindow error"
				});
				return null;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return null;
			}
			this.mGameUIMsgWindow = gameObject2.GetComponent<GameUIMsgWindow>();
			Vector3 localPosition = this.mGameUIMsgWindow.transform.localPosition;
			localPosition.z = 4000f;
			this.mGameUIMsgWindow.transform.localPosition = localPosition;
			this.mGameUIMsgWindow.Init();
		}
		this.mGameUIMsgWindow.gameObject.SetActive(true);
		return this.mGameUIMsgWindow;
	}

	public void CloseMsgWindow()
	{
		if (this.mGameUIMsgWindow != null)
		{
			UnityEngine.Object.Destroy(this.mGameUIMsgWindow.gameObject);
			this.mGameUIMsgWindow = null;
		}
	}

	public bool ShowPlotDialog(int dialogID, GUIPlotDialog.FinishCallback callBackEvent, GUIPlotDialog.VoidCallback showNextEvent = null)
	{
		GameObject gameObject = Res.LoadGUI("GUI/GUIPlotDialog");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIPlotDialog error"
			});
			return false;
		}
		GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild error"
			});
			return false;
		}
		this.uiDialog = gameObject2.AddComponent<GUIPlotDialog>();
		if (this.uiDialog == null)
		{
			return false;
		}
		Vector3 localPosition = gameObject2.transform.localPosition;
		localPosition.z += 5000f;
		gameObject2.transform.localPosition = localPosition;
		return this.uiDialog.ShowDialogInfo(dialogID, callBackEvent, showNextEvent);
	}

	public bool ShowMapUIDialog(int sceneID, GUIPlotDialog.FinishCallback callBackEvent)
	{
		QuestInfo info = Globals.Instance.AttDB.QuestDict.GetInfo(sceneID);
		if (info == null || info.MapUIDialogID == 0 || GameCache.HasDialogShowed(info.MapUIDialogID))
		{
			return false;
		}
		if (Globals.Instance.Player.GetSceneData(sceneID) != null)
		{
			return false;
		}
		GameCache.SetDialogShowed(info.MapUIDialogID);
		return this.ShowPlotDialog(info.MapUIDialogID, callBackEvent, null);
	}

	public bool ShowSceneStartDialog(int sceneID, GUIPlotDialog.FinishCallback callBackEvent)
	{
		QuestInfo info = Globals.Instance.AttDB.QuestDict.GetInfo(sceneID);
		if (info == null || info.SceneStartDialogID == 0 || GameCache.HasDialogShowed(info.SceneStartDialogID))
		{
			return false;
		}
		if (Globals.Instance.Player.GetSceneData(sceneID) != null)
		{
			return false;
		}
		GameCache.SetDialogShowed(info.SceneStartDialogID);
		return this.ShowPlotDialog(info.SceneStartDialogID, callBackEvent, null);
	}

	public bool ShowSceneWinDialog(int sceneID, GUIPlotDialog.FinishCallback callBackEvent)
	{
		QuestInfo info = Globals.Instance.AttDB.QuestDict.GetInfo(sceneID);
		if (info == null || info.SceneWinDialogID == 0 || GameCache.HasDialogShowed(info.SceneWinDialogID))
		{
			return false;
		}
		if (Globals.Instance.Player.GetSceneData(sceneID) != null)
		{
			return false;
		}
		GameCache.SetDialogShowed(info.SceneWinDialogID);
		return this.ShowPlotDialog(info.SceneWinDialogID, callBackEvent, null);
	}

	public void CloseDialog()
	{
		if (this.uiDialog != null)
		{
			UnityEngine.Object.Destroy(this.uiDialog.gameObject);
			this.uiDialog = null;
		}
	}

	public void ShowGameCDMsg(int cdnum)
	{
		if (this.mGameCDMsg == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/GUIGameCountDownMsg");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIGameStateTip error"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return;
			}
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
			this.mGameCDMsg = gameObject2.AddComponent<GUIGameCountDownMsg>();
		}
		this.mGameCDMsg.ShowCountDownMsg(cdnum);
	}

	public void CloseGameCDMsg()
	{
		if (this.mGameCDMsg != null)
		{
			UnityEngine.Object.Destroy(this.mGameCDMsg.gameObject);
			this.mGameCDMsg = null;
		}
	}

	public void ShowBattleCDMsg(GUIBattleCountDown.CDEndCallback cb = null)
	{
		if (this.mBattleCountDown == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUIBattleCountDown");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIBattleCountDown error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 5000f);
			gameObject.transform.localScale = Vector3.one;
			this.mBattleCountDown = gameObject.AddComponent<GUIBattleCountDown>();
			if (cb != null)
			{
				GUIBattleCountDown expr_B8 = this.mBattleCountDown;
				expr_B8.CDEndEvent = (GUIBattleCountDown.CDEndCallback)Delegate.Combine(expr_B8.CDEndEvent, cb);
			}
		}
	}

	public void CloseBattleCDMsg()
	{
		if (this.mBattleCountDown != null)
		{
			UnityEngine.Object.Destroy(this.mBattleCountDown.gameObject);
			this.mBattleCountDown = null;
		}
	}

	public GUIAttributeTip ShowAttributeTip(List<string> contents, float time1 = 2f, float time2 = 0.4f, float startY = 0f, float animLength = 200f)
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/Global/GUIAttributeTip");
		if (@object == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIAttributeTip error"
			});
			return null;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = this.uiCamera.gameObject.transform;
		gameObject.transform.localPosition = new Vector3(0f, startY, 3000f);
		gameObject.transform.localScale = Vector3.one;
		GUIAttributeTip gUIAttributeTip = gameObject.AddComponent<GUIAttributeTip>();
		gUIAttributeTip.ShowTipContents(contents, time1, time2, animLength);
		return gUIAttributeTip;
	}

	public GUIGameNewsMsg GetSystemNoticeMsg()
	{
		return this.mGUISystemNoticeMsg;
	}

	private void LoadGameNew(bool isNotice = false)
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUIGameNewsMsg");
		if (@object == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIGameNewsMsg error"
			});
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = this.uiCamera.gameObject.transform;
		gameObject.transform.localScale = Vector3.one;
		if (isNotice)
		{
			gameObject.transform.localPosition = new Vector3(0f, 105f, 3000f);
			this.mGUISystemNoticeMsg = gameObject.AddComponent<GUIGameNewsMsg>();
		}
		else
		{
			gameObject.transform.localPosition = new Vector3(0f, 50f, 3000f);
			this.mGUIGameNewsMsg = gameObject.AddComponent<GUIGameNewsMsg>();
		}
	}

	public GUIGameNewsMsg ShowGameNew(string content, int priority, int speed = 110, bool isNotice = false)
	{
		if (isNotice)
		{
			if (this.mGUISystemNoticeMsg == null)
			{
				this.LoadGameNew(true);
			}
			this.mGUISystemNoticeMsg.ShowNew(content, speed, priority, isNotice);
			return this.mGUISystemNoticeMsg;
		}
		if (this.mGUIGameNewsMsg == null)
		{
			this.LoadGameNew(false);
		}
		this.mGUIGameNewsMsg.ShowNew(content, speed, priority, isNotice);
		return this.mGUIGameNewsMsg;
	}

	public GUIGameNewsMsg ShowGameNewsMsg(List<string> contents, int priority, int speed = 110, bool isNotice = false)
	{
		if (isNotice)
		{
			if (this.mGUISystemNoticeMsg == null)
			{
				this.LoadGameNew(true);
			}
			this.mGUISystemNoticeMsg.ShowNews(contents, speed, priority, isNotice);
			return this.mGUISystemNoticeMsg;
		}
		if (this.mGUIGameNewsMsg == null)
		{
			this.LoadGameNew(false);
		}
		this.mGUIGameNewsMsg.ShowNews(contents, speed, priority, isNotice);
		return this.mGUIGameNewsMsg;
	}

	public void DestroyGameNewsMsg()
	{
		if (this.mGUIGameNewsMsg != null)
		{
			this.mGUIGameNewsMsg.ClearNews();
			UnityEngine.Object.Destroy(this.mGUIGameNewsMsg.gameObject);
			this.mGUIGameNewsMsg = null;
		}
		if (this.mGUISystemNoticeMsg != null)
		{
			this.mGUISystemNoticeMsg.ClearNews();
			UnityEngine.Object.Destroy(this.mGUISystemNoticeMsg.gameObject);
			this.mGUISystemNoticeMsg = null;
		}
	}

	public void DestroySystemNoticeMsg()
	{
		if (this.mGUISystemNoticeMsg != null)
		{
			this.mGUISystemNoticeMsg.ClearNews();
			UnityEngine.Object.Destroy(this.mGUISystemNoticeMsg.gameObject);
			this.mGUISystemNoticeMsg = null;
		}
	}

	public void SetVisibleGameNewsMsg(bool visible)
	{
		if (this.mGUIGameNewsMsg != null)
		{
			this.mGUIGameNewsMsg.gameObject.SetActive(visible);
		}
		if (this.mGUISystemNoticeMsg != null)
		{
			this.mGUISystemNoticeMsg.gameObject.SetActive(visible);
		}
	}

	private void LoadGameNewPopUp(float startY)
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUIGameNewsMsgPopUp");
		if (@object == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIGameNewsMsgPopUp error"
			});
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = this.uiCamera.gameObject.transform;
		gameObject.transform.localPosition = new Vector3(0f, startY, 3000f);
		gameObject.transform.localScale = Vector3.one;
		this.mGUIGameNewsMsgPopUp = gameObject.AddComponent<GUIGameNewsMsgPopUp>();
	}

	public void ShowGameNewPopUp(string content, float delay = 1f, float startY = 50f, float animTime = 0.25f)
	{
		if (this.mGUIGameNewsMsgPopUp == null)
		{
			this.LoadGameNewPopUp(startY);
		}
		this.mGUIGameNewsMsgPopUp.ShowNew(content, delay, animTime);
	}

	public void ShowGameNewsMsgPopUp(List<string> contents, float delay = 1f, float startY = 50f)
	{
		if (this.mGUIGameNewsMsgPopUp == null)
		{
			this.LoadGameNewPopUp(startY);
		}
		this.mGUIGameNewsMsgPopUp.ShowNews(contents, delay);
	}

	public GUIGameNewsMsgPopUp GetGameNewsPopUp()
	{
		return this.mGUIGameNewsMsgPopUp;
	}

	public void DestroyGameNewsMsgPopUp()
	{
		if (this.mGUIGameNewsMsgPopUp != null)
		{
			this.mGUIGameNewsMsgPopUp.ClearNews();
			UnityEngine.Object.Destroy(this.mGUIGameNewsMsgPopUp.gameObject);
			this.mGUIGameNewsMsgPopUp = null;
		}
	}

	public void CreateLoginBG()
	{
		if (this.loginBG != null)
		{
			return;
		}
		Transform transform = this.uiCamera.transform.FindChild("LoginBg");
		GameObject gameObject;
		if (transform != null)
		{
			gameObject = transform.gameObject;
		}
		else
		{
			gameObject = Res.LoadGUI("GUI/LoginBg");
			gameObject = NGUITools.AddChild(this.uiCamera.gameObject, gameObject);
		}
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Load GUI/LoginBg error!"
			});
			return;
		}
		this.loginBG = gameObject.AddComponent<DynamicUpdate>();
		Vector3 localPosition = this.loginBG.transform.localPosition;
		localPosition.z += 5000f;
		this.loginBG.transform.localPosition = localPosition;
	}

	public void DestroyLoginBG()
	{
		if (this.loginBG != null)
		{
			UnityEngine.Object.Destroy(this.loginBG.gameObject);
			this.loginBG = null;
		}
	}

	public void SetLoginText(string str)
	{
		if (this.loginBG != null)
		{
			this.loginBG.SetLoginText(str);
		}
	}

	public void HideLoginText()
	{
		if (this.loginBG != null)
		{
			this.loginBG.HideLoginText();
		}
	}

	public void CheckVerion(DynamicUpdate.VoidCallback callBack)
	{
		if (this.loginBG != null)
		{
			DynamicUpdate expr_17 = this.loginBG;
			expr_17.FinishUpdateEvent = (DynamicUpdate.VoidCallback)Delegate.Combine(expr_17.FinishUpdateEvent, callBack);
			base.StartCoroutine(this.loginBG.CheckUpdateList());
		}
	}

	public void ReCheckVerion(DynamicUpdate.VoidCallback callBack)
	{
		if (this.loginBG != null)
		{
			DynamicUpdate expr_17 = this.loginBG;
			expr_17.FinishUpdateEvent = (DynamicUpdate.VoidCallback)Delegate.Combine(expr_17.FinishUpdateEvent, callBack);
			base.StartCoroutine(this.loginBG.CheckInterUpdateList(false));
		}
	}

	public void ShowBattleWarnning()
	{
		if (this.mBattleWarnning == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUIBattleWarnning");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIBattleWarnning error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 5000f);
			gameObject.transform.localScale = Vector3.one;
			this.mBattleWarnning = gameObject.AddComponent<GUIBattleWarnning>();
		}
	}

	public void CloseBattleWarnning()
	{
		if (this.mBattleWarnning != null)
		{
			UnityEngine.Object.Destroy(this.mBattleWarnning.gameObject);
			this.mBattleWarnning = null;
		}
	}

	public void ShowGameUIOptionPopUp()
	{
		if (this.mGameUIOptionPopUp == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/GameUIOptionPopUp");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GameUIOptionPopUp error"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return;
			}
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
			this.mGameUIOptionPopUp = gameObject2.AddComponent<GameUIOptionPopUp>();
		}
	}

	public GameUIOptionPopUp GetGameUIOptionPopUp()
	{
		return this.mGameUIOptionPopUp;
	}

	public void DestroyGameUIOptionPopUp()
	{
		if (this.mGameUIOptionPopUp != null)
		{
			UnityEngine.Object.Destroy(this.mGameUIOptionPopUp.gameObject);
			this.mGameUIOptionPopUp = null;
		}
	}

	public void ShowGUIWebViewPopUp(string url, string title = null)
	{
		if (this.mGUIWebViewPopUp == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/GUIWebViewPopUp");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIWebViewPopUp error"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return;
			}
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z = 2800f;
			gameObject2.transform.localPosition = localPosition;
			this.mGUIWebViewPopUp = gameObject2.AddComponent<GUIWebViewPopUp>();
		}
		this.mGUIWebViewPopUp.ShowWebPage(url, title);
	}

	public GUIWebViewPopUp GetWebViewPopUp()
	{
		return this.mGUIWebViewPopUp;
	}

	public void DestroyGUIWebViewPopUp()
	{
		if (this.mGUIWebViewPopUp != null)
		{
			UnityEngine.Object.Destroy(this.mGUIWebViewPopUp.gameObject);
			this.mGUIWebViewPopUp = null;
		}
	}

	public GUIPetInfoSceneV2 GetPetInfoSceneV2()
	{
		return this.mGUIPetInfoSceneV2;
	}

	public void ShowPetInfoSceneV2(PetDataEx pdEx, int whichPart = 0, ItemInfo idEx = null, int uiState = 0)
	{
		if (pdEx == null)
		{
			return;
		}
		if (this.mGUIPetInfoSceneV2 == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUIPetInfoSceneV2");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIPetInfoSceneV2 error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 1500f);
			gameObject.transform.localScale = Vector3.one;
			this.mGUIPetInfoSceneV2 = gameObject.GetComponent<GUIPetInfoSceneV2>();
		}
		this.mGUIPetInfoSceneV2.Show(pdEx, whichPart, idEx, uiState);
	}

	public void ShowPetInfo(PetInfo info)
	{
		if (info == null)
		{
			return;
		}
		this.ShowPetInfoSceneV2(new PetDataEx(new PetData
		{
			Level = 1u
		}, info), 0, null, 2);
	}

	public void ShowPetSliceInfo(ItemInfo iDataEx)
	{
		if (iDataEx != null)
		{
			PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(iDataEx.Value2);
			PetDataEx pdEx = new PetDataEx(new PetData
			{
				Level = 1u
			}, info);
			GameUIManager.mInstance.ShowPetInfoSceneV2(pdEx, 0, iDataEx, 1);
		}
	}

	public void ShowPetCollectionInfo(ItemInfo iDataEx)
	{
		if (iDataEx != null)
		{
			PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(iDataEx.Value2);
			PetDataEx pdEx = new PetDataEx(new PetData
			{
				Level = 1u
			}, info);
			GameUIManager.mInstance.ShowPetInfoSceneV2(pdEx, 0, iDataEx, 4);
		}
	}

	public void DestroyPetInfoSceneV2()
	{
		if (this.mGUIPetInfoSceneV2 != null)
		{
			UnityEngine.Object.Destroy(this.mGUIPetInfoSceneV2.gameObject);
			this.mGUIPetInfoSceneV2 = null;
		}
	}

	public void ShowPetFurtherSucV2(PetDataEx pdEx)
	{
		if (this.mGUIPetFurtherSucV2 == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUIPetFurtherSucV2");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIPetFurtherSucV2 error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			this.mGUIPetFurtherSucV2 = gameObject.AddComponent<GUIPetFurtherSucV2>();
		}
		this.mGUIPetFurtherSucV2.CurPetDataEx = pdEx;
	}

	public bool DestroyPetFurtherSucV2()
	{
		if (this.mGUIPetFurtherSucV2 != null)
		{
			UnityEngine.Object.Destroy(this.mGUIPetFurtherSucV2.gameObject);
			this.mGUIPetFurtherSucV2 = null;
			return true;
		}
		return false;
	}

	public void ShowPetQualityUp(int conLv)
	{
		if (this.mGUIPetQualityUp == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUIPetQualityUp");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIPetQualityUp error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			this.mGUIPetQualityUp = gameObject.AddComponent<GUIPetQualityUp>();
		}
		this.mGUIPetQualityUp.ConLv = conLv;
	}

	public bool DestroyPetQualityUp()
	{
		if (this.mGUIPetQualityUp != null)
		{
			UnityEngine.Object.Destroy(this.mGUIPetQualityUp.gameObject);
			this.mGUIPetQualityUp = null;
			return true;
		}
		return false;
	}

	public void ShowItemInfo(ItemInfo info)
	{
		if (info == null)
		{
			return;
		}
		ItemData itemData = new ItemData();
		itemData.Value1 = 1;
		switch (info.Type)
		{
		case 0:
			GUIEquipInfoPopUp.ShowThis(new ItemDataEx(itemData, info), GUIEquipInfoPopUp.EIPT.EIPT_View, -1, false, true);
			break;
		case 1:
			GUIEquipInfoPopUp.ShowThis(new ItemDataEx(itemData, info), GUIEquipInfoPopUp.EIPT.EIPT_View, -1, false, true);
			break;
		case 2:
		case 4:
			GUIPropsInfoPopUp.Show(info);
			break;
		case 3:
			switch (info.SubType)
			{
			case 0:
				this.ShowPetSliceInfo(info);
				break;
			case 1:
				GUIEquipInfoPopUp.ShowThis(new ItemDataEx(itemData, info), GUIEquipInfoPopUp.EIPT.EIPT_Fragment, -1, false, true);
				break;
			case 2:
				GUIEquipInfoPopUp.ShowThis(new ItemDataEx(itemData, info), GUIEquipInfoPopUp.EIPT.EIPT_Fragment, -1, false, true);
				break;
			case 3:
				this.ShowLopetInfo(info);
				break;
			}
			break;
		case 5:
			GUIAwakeItemInfoPopUp.Show(info, GUIAwakeItemInfoPopUp.EOpenType.EOT_View, null);
			break;
		}
	}

	public void ShowTrialInGamePopUp(int curLvl)
	{
		if (this.mGUITrialInGamePopUp == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUITrialInGamePopUp");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUITrialInGamePopUp error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			this.mGUITrialInGamePopUp = gameObject.AddComponent<GUITrialInGamePopUp>();
		}
		this.mGUITrialInGamePopUp.ShowMe(curLvl);
	}

	public void DestroyTrialInGamePopUp()
	{
		if (this.mGUITrialInGamePopUp != null)
		{
			UnityEngine.Object.Destroy(this.mGUITrialInGamePopUp.gameObject);
			this.mGUITrialInGamePopUp = null;
		}
	}

	public void TryCommend(ECommentType type, float waitTime = 0f)
	{
		if (GameUIManager.mInstance.uiState.CommentData == null)
		{
			return;
		}
		if (type == ECommentType.EComment_Level)
		{
			if (GameUIManager.mInstance.uiState.CommentData.Type != 9 && GameUIManager.mInstance.uiState.CommentData.Type != 18)
			{
				return;
			}
		}
		else if (type == ECommentType.EComment_Trial)
		{
			if (GameUIManager.mInstance.uiState.CommentData.Type != 8 && GameUIManager.mInstance.uiState.CommentData.Type != 17)
			{
				return;
			}
		}
		else if (type != ECommentType.EComment_Null && GameUIManager.mInstance.uiState.CommentData.Type != (int)type)
		{
			return;
		}
		if (!Globals.Instance.TutorialMgr.IsNull)
		{
			return;
		}
		base.Invoke("Commend", waitTime);
	}

	private void Commend()
	{
		GUIScorePopUp.ShowScorePopUp();
	}

	public void ShowCountdownDeath(int time)
	{
		GUICountdownDeath.Show(time);
	}

	public void ShowLopetInfo(ItemInfo info)
	{
		if (info == null)
		{
			return;
		}
		LopetInfo info2 = Globals.Instance.AttDB.LopetDict.GetInfo(info.Value2);
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("LopetDict get info error , ID : {0}", new object[]
			{
				info.Value2
			});
			return;
		}
		GUILopetInfoScene.Show(new LopetDataEx(new LopetData
		{
			Level = 1u
		}, info2), GUILopetInfoScene.EType.EType_Fragment);
	}

	public void ShowLopetInfo(LopetInfo info)
	{
		if (info == null)
		{
			return;
		}
		GUILopetInfoScene.Show(new LopetDataEx(new LopetData
		{
			Level = 1u
		}, info), GUILopetInfoScene.EType.EType_Fragment);
	}

	public void ShowLopetInfo(LopetDataEx data)
	{
		if (data == null)
		{
			return;
		}
		if (data.IsLocal())
		{
			GUIPetTrainSceneV2.Show(data, GUIPetTrainSceneV2.EUILopetTabs.E_UIBaseInfo);
		}
		else
		{
			GUILopetInfoScene.Show(data, GUILopetInfoScene.EType.EType_Info);
		}
	}

	public bool CloseLopetInfo()
	{
		return GUILopetInfoScene.TryClose();
	}
}
