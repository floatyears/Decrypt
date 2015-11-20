using Holoville.HOTween.Core;
using LitJson;
using NtUniSdk.Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIGameLoginScene : GameUISession
{
	public enum EState
	{
		StepNativeLogin,
		StepEnter
	}

	private class RegisterAccountData
	{
		public int ret = -1;

		public int account;

		public int password;
	}

	private class LoginAccountData
	{
		public int ret = -1;

		public string key = string.Empty;

		public string msg = string.Empty;
	}

	public class ServerList
	{
		public int id
		{
			get;
			set;
		}

		public int show_id
		{
			get;
			set;
		}

		public string name
		{
			get;
			set;
		}

		public string ip
		{
			get;
			set;
		}

		public int status
		{
			get;
			set;
		}

		public int is_new
		{
			get;
			set;
		}

		public int env
		{
			get;
			set;
		}

		public string platform
		{
			get;
			set;
		}
	}

	public class ServerListData
	{
		public List<GUIGameLoginScene.ServerList> server_list
		{
			get;
			set;
		}
	}

	private GameObject mLoginLayer;

	private GameObject mEnterLayer;

	private GameObject mManagerView;

	private GameObject mZonesWindow;

	private GameObject mManagerViewNew;

	private UISprite mFadeBG;

	private UILabel mZoneListTitle;

	private UIPanel mZonesPanel;

	public ZoneItemInfoData selectedZoneData;

	private GUILoginZoneItem mLastTimeZone;

	private LoginZoneTable mZonesTable;

	private LoginGroupTable mGroupsTable;

	public GameObject mSelectZone;

	public UILabel mNum;

	public UILabel mName;

	public UISprite mNew;

	public UISprite mState;

	private UISprite mShadow;

	private UIInput mAccountInput;

	private UIInput mSecretInput;

	private string key;

	public static bool first = true;

	private bool fullServerList;

	private float serverListCD;

	private float timerRefresh;

	private bool autoRequesting;

	private List<GUIGameLoginScene.ServerList> mServerListData;

	private void CreateObjects()
	{
		this.mLoginLayer = base.FindGameObject("LoginState", null);
		this.mEnterLayer = base.FindGameObject("EnterState", null);
		this.mZonesWindow = base.FindGameObject("ZonesWindow", this.mEnterLayer);
		this.mFadeBG = GameUITools.FindUISprite("FadeBG", this.mEnterLayer);
		this.mLoginLayer.SetActive(false);
		this.mEnterLayer.SetActive(false);
		GameObject gameObject = base.RegisterClickEvent("Register", new UIEventListener.VoidDelegate(this.OnRegisterClick), this.mLoginLayer);
		base.SetLabelLocalText("Label", "Register", gameObject);
		gameObject = base.RegisterClickEvent("Enter", new UIEventListener.VoidDelegate(this.OnEnterClick), this.mLoginLayer);
		base.SetLabelLocalText("Label", "Enter", gameObject);
		gameObject = base.FindGameObject("AccountInput", this.mLoginLayer);
		this.mAccountInput = gameObject.GetComponent<UIInput>();
		gameObject = base.FindGameObject("mimaInput", this.mLoginLayer);
		this.mSecretInput = gameObject.GetComponent<UIInput>();
		gameObject = base.RegisterClickEvent("EnterGame", new UIEventListener.VoidDelegate(this.OnEnterGameClick), this.mEnterLayer);
		GameUITools.FindGameObject("ui39", gameObject).gameObject.SetActive(false);
		this.mManagerView = GameUITools.FindGameObject("ManagerView", base.gameObject);
		UIEventListener expr_15A = UIEventListener.Get(this.mManagerView);
		expr_15A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_15A.onClick, new UIEventListener.VoidDelegate(this.OnManagerViewClick));
		GameUITools.RegisterClickEvent("Agreement", new UIEventListener.VoidDelegate(this.OnAgreementClick), base.gameObject);
		this.mManagerViewNew = GameUITools.FindGameObject("new", this.mManagerView);
		if (this.mManagerViewNew != null)
		{
			this.mManagerViewNew.gameObject.SetActive(false);
		}
		base.RegisterClickEvent("Billboard", new UIEventListener.VoidDelegate(this.OnBillboardClick), null);
		if (!SdkU3d.hasFeature("FEATURE_HAS_MANAGER"))
		{
			this.mManagerView.SetActive(false);
		}
		this.mSelectZone = GameUITools.FindGameObject("SelectZone", this.mEnterLayer);
		this.mNum = GameUITools.FindUILabel("Num", this.mSelectZone);
		this.mName = GameUITools.FindUILabel("Name", this.mSelectZone);
		this.mNew = GameUITools.FindUISprite("New", this.mSelectZone);
		this.mState = GameUITools.FindUISprite("State", this.mSelectZone);
		this.mShadow = GameUITools.FindUISprite("Shadow", this.mSelectZone);
		this.mZoneListTitle = GameUITools.FindUILabel("Zones/ZoneListTitle", this.mZonesWindow);
		this.mZonesPanel = base.FindGameObject("Zones/ZonesPanel", this.mZonesWindow).GetComponent<UIPanel>();
		this.mZonesTable = base.FindGameObject("ZonesContents", this.mZonesPanel.gameObject).AddComponent<LoginZoneTable>();
		this.mZonesTable.maxPerLine = 2;
		this.mZonesTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mZonesTable.cellWidth = 334f;
		this.mZonesTable.cellHeight = 62f;
		this.mZonesTable.gapHeight = 5f;
		this.mZonesTable.gapWidth = 14f;
		this.mZonesTable.InitWithBaseScene(this);
		this.mLastTimeZone = base.FindGameObject("LastTime/ZoneItem", this.mZonesWindow).AddComponent<GUILoginZoneItem>();
		this.mLastTimeZone.InitWithBaseScene(this);
		GameUITools.IncreaseObjectsDepth(this.mLastTimeZone.gameObject, 110);
		this.mGroupsTable = base.FindGameObject("Groups/Panel/Contents", this.mZonesWindow).AddComponent<LoginGroupTable>();
		this.mGroupsTable.maxPerLine = 1;
		this.mGroupsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGroupsTable.cellWidth = 210f;
		this.mGroupsTable.cellHeight = 66f;
		this.mGroupsTable.gapWidth = 0f;
		this.mGroupsTable.gapHeight = -1f;
		this.mGroupsTable.Init(this);
		this.RefreshPlayerManagerNew();
		base.RegisterClickEvent("SelectZone", new UIEventListener.VoidDelegate(this.OnSelectZoneClick), this.mEnterLayer);
		base.RegisterPressEvent("SelectZone", new UIEventListener.BoolDelegate(this.OnSelectZonePress), this.mEnterLayer);
		UIEventListener expr_44B = UIEventListener.Get(this.mFadeBG.gameObject);
		expr_44B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_44B.onClick, new UIEventListener.VoidDelegate(this.OnCloseZonesWindow));
	}

	protected override void OnPostLoadGUI()
	{
		if (GameUIPopupManager.GetInstance().GetCurrentPopup() != null)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
		GameUIManager.mInstance.CreateLoginBG();
		GameUIManager.mInstance.HideLoginText();
		this.CreateObjects();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		GameUIManager.mInstance.CloseTopGoods();
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		Globals.Instance.CliSession.Register(1504, new ClientSession.MsgHandler(this.OnMsgRegister));
		Globals.Instance.CliSession.Register(1505, new ClientSession.MsgHandler(this.OnMsgLogin));
		Globals.Instance.CliSession.Register(1506, new ClientSession.MsgHandler(this.OnMsgServerList));
		this.OpenUIIngameScene();
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		this.SetState(GUIGameLoginScene.EState.StepEnter);
		string text = SdkU3d.getPropStr("SESSION");
		if (("3k_sdk" == SdkU3d.getChannel() || "scgames" == SdkU3d.getChannel()) && (text == null || text.Equals(string.Empty)))
		{
			text = "null";
		}
		if (!SdkU3d.hasLogin() || text == null || text.Equals(string.Empty))
		{
			SdkU3d.ntLogin();
		}
		this.RequestServerList(false);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.DestroyLoginBG();
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Remove(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Remove(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		Globals.Instance.CliSession.Unregister(1504, new ClientSession.MsgHandler(this.OnMsgRegister));
		Globals.Instance.CliSession.Unregister(1505, new ClientSession.MsgHandler(this.OnMsgLogin));
		Globals.Instance.CliSession.Unregister(1506, new ClientSession.MsgHandler(this.OnMsgServerList));
	}

	private void RequestServerList(bool fullList = false)
	{
		this.fullServerList = fullList;
		string url = string.Format("{0}{1}", GameSetting.ServerListURL, GameSetting.GameVersion);
		if (!Globals.Instance.CliSession.HttpGet(url, 1506, false, null))
		{
			GameUIManager.mInstance.HideIndicate();
		}
	}

	private void RetryGetServerList()
	{
		this.RequestServerList(this.fullServerList);
	}

	public void OnSelectZoneClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.serverListCD <= 0f || Time.time >= this.serverListCD)
		{
			this.RequestServerList(true);
		}
		else
		{
			base.StartCoroutine(this.PopServerList());
		}
	}

	private void RepositionZonesWindow()
	{
		this.mGroupsTable.repositionNow = true;
		this.mZonesTable.repositionNow = true;
	}

	private void OnSelectZonePress(GameObject obj, bool isPressed)
	{
		if (isPressed)
		{
			this.mShadow.enabled = true;
		}
		else
		{
			this.mShadow.enabled = false;
		}
	}

	private void OnBillboardClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIBillboardPopUp, false, null, null);
	}

	public void OnManagerViewClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GameUIManager.mInstance.uiState.SDKPlayerManagerNew)
		{
			GameUIManager.mInstance.uiState.SDKPlayerManagerNew = false;
			this.RefreshPlayerManagerNew();
		}
		global::Debug.Log(new object[]
		{
			"ntOpenManager"
		});
		SdkU3d.ntOpenManager();
	}

	private void OnAgreementClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIAgreementInfoPopUp.Show(false);
	}

	public void OnRegisterClick(GameObject go)
	{
		if (Globals.Instance.CliSession.HttpGet(string.Format("{0}one_key_register.php", GameSetting.LoginURL), 1504, false, null))
		{
			GameUIManager.mInstance.ShowIndicate();
		}
	}

	public void OnEnterClick(GameObject go)
	{
		if (Globals.Instance.CliSession.HttpGet(string.Format("{0}login.php?account={1}&password={2}", GameSetting.LoginURL, this.mAccountInput.value, this.mSecretInput.value), 1505, false, null))
		{
			GameUIManager.mInstance.ShowIndicate();
		}
	}

	public void OnEnterGameClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		string text = SdkU3d.getPropStr("SESSION");
		if (("3k_sdk" == SdkU3d.getChannel() || "scgames" == SdkU3d.getChannel()) && (text == null || text.Equals(string.Empty)))
		{
			text = "null";
		}
		if (!SdkU3d.hasLogin() || text == null || text.Equals(string.Empty))
		{
			SdkU3d.ntLogin();
			return;
		}
		string text2 = SdkU3d.getPropStr("UIN");
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "0";
		}
		string text3 = text;
		string platform = SdkU3d.getPlatform();
		string channel = SdkU3d.getChannel();
		int pub = 1;
		GameSetting.Data.Account = text2;
		if (!GameSetting.Data.LastGMLogin && this.selectedZoneData != null && this.selectedZoneData.mState == 5)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("loginZoneStatusMaintainTips", 0f, 0f);
			return;
		}
		if (this.selectedZoneData != null)
		{
			GameSetting.ServerIP = this.selectedZoneData.mServerIP;
			GameSetting.ServerPort = this.selectedZoneData.mServerPort;
			GameSetting.ServerID = this.selectedZoneData.mShowNum;
			if (GameSetting.Data.ServerID != this.selectedZoneData.mServerID || GameSetting.Data.ServerName != this.selectedZoneData.mName)
			{
				GameSetting.Data.ServerName = this.selectedZoneData.mName;
				GameSetting.Data.ServerID = this.selectedZoneData.mServerID;
				GameSetting.UpdateNow = true;
			}
			Globals.Instance.CliSession.Connect(GameSetting.ServerIP, GameSetting.ServerPort, text2, text3, platform, channel, pub);
			return;
		}
		GameUIManager.mInstance.ShowMessageTipByKey("loginZonePleaseSelectZone", 0f, 0f);
	}

	public void OnMsgRegister(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int num = binaryReader.ReadInt32();
		if (num != 200)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("LoginR_0", 0f, 0f);
		}
		else
		{
			try
			{
				GUIGameLoginScene.RegisterAccountData registerAccountData = JsonMapper.ToObject<GUIGameLoginScene.RegisterAccountData>(binaryReader.ReadString());
				if (registerAccountData.ret == 0)
				{
					this.mAccountInput.value = registerAccountData.account.ToString();
					this.mSecretInput.value = registerAccountData.password.ToString();
				}
				else if (registerAccountData.ret == 6 || registerAccountData.ret == 10)
				{
					GameUIManager.mInstance.ShowMessageTip("LoginR", registerAccountData.ret);
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("LoginR_1", 0f, 0f);
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Parse Login Json Error, {0}", ex.Message)
				});
			}
		}
		GameUIManager.mInstance.HideIndicate();
	}

	public void OnMsgLogin(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int num = binaryReader.ReadInt32();
		if (num != 200)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("LoginR_20", 0f, 0f);
			this.SetState(GUIGameLoginScene.EState.StepNativeLogin);
		}
		else
		{
			try
			{
				GUIGameLoginScene.LoginAccountData loginAccountData = JsonMapper.ToObject<GUIGameLoginScene.LoginAccountData>(binaryReader.ReadString());
				if (loginAccountData.ret == 0)
				{
					global::Debug.Log(new object[]
					{
						"Login Success!"
					});
					this.key = loginAccountData.key;
					if (!string.IsNullOrEmpty(this.mAccountInput.value) && GameSetting.Data.Account != this.mAccountInput.value)
					{
						GameSetting.Data.Account = this.mAccountInput.value;
						GameSetting.Data.Password = this.mSecretInput.value;
						GameSetting.UpdateNow = true;
					}
					GameUIManager.mInstance.HideIndicate();
					this.SetState(GUIGameLoginScene.EState.StepEnter);
				}
				else if (loginAccountData.ret == 6 || loginAccountData.ret == 7)
				{
					GameUIManager.mInstance.ShowMessageTip("LoginR", loginAccountData.ret);
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("LoginR_1", 0f, 0f);
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Parse Login Json Error, {0}", ex.Message)
				});
			}
		}
		GameUIManager.mInstance.HideIndicate();
	}

	public void OnMsgServerList(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int num = binaryReader.ReadInt32();
		if (num != 200)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("LoginR_21", 0f, 0f);
			if (!this.fullServerList)
			{
				base.Invoke("RetryGetServerList", 3f);
			}
			return;
		}
		try
		{
			GUIGameLoginScene.ServerListData serverListData = JsonMapper.ToObject<GUIGameLoginScene.ServerListData>(binaryReader.ReadString());
			this.InitZonesWindow(serverListData.server_list);
			if (this.fullServerList)
			{
				if (!this.autoRequesting)
				{
					base.StartCoroutine(this.PopServerList());
				}
				this.serverListCD = Time.time + 15f;
			}
			this.timerRefresh = Time.time;
			if (this.autoRequesting)
			{
				this.autoRequesting = false;
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Parse ServerList Json Error, {0}", ex.Message)
			});
		}
		GameUIManager.mInstance.HideIndicate();
	}

	[DebuggerHidden]
	private IEnumerator PopServerList()
	{
        return null;
        //GUIGameLoginScene.<PopServerList>c__Iterator62 <PopServerList>c__Iterator = new GUIGameLoginScene.<PopServerList>c__Iterator62();
        //<PopServerList>c__Iterator.<>f__this = this;
        //return <PopServerList>c__Iterator;
	}

	private void TryLogin()
	{
		if (!string.IsNullOrEmpty(GameSetting.Data.Account))
		{
			this.mAccountInput.value = GameSetting.Data.Account;
			this.mSecretInput.value = GameSetting.Data.Password;
			if (Globals.Instance.CliSession.HttpGet(string.Format("{0}login.php?account={1}&password={2}", GameSetting.LoginURL, GameSetting.Data.Account, GameSetting.Data.Password), 1505, false, null))
			{
				GameUIManager.mInstance.ShowIndicate();
			}
		}
		else
		{
			this.SetState(GUIGameLoginScene.EState.StepNativeLogin);
		}
	}

	private void OnCloseZonesWindow(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.CloseZonesWindow();
	}

	public void CloseZonesWindow()
	{
		GameUITools.PlayCloseWindowAnim(this.mZonesWindow.transform, new TweenDelegate.TweenCallback(this.OnCloseZonesWindowAnimEnd), true);
	}

	private void OnCloseZonesWindowAnimEnd()
	{
		this.mZonesWindow.SetActive(false);
		this.mFadeBG.enabled = false;
		GameUITools.PlayOpenWindowAnim(this.mZonesWindow.transform, new TweenDelegate.TweenCallback(this.RepositionZonesWindow), true);
	}

	private int SortByShowID(GUIGameLoginScene.ServerList a, GUIGameLoginScene.ServerList b)
	{
		if (a != null && b != null)
		{
			if (a.show_id > b.show_id)
			{
				return 1;
			}
			if (a.show_id < b.show_id)
			{
				return -1;
			}
		}
		return 0;
	}

	private void InitZonesWindow(List<GUIGameLoginScene.ServerList> datas)
	{
		if (datas != null && datas.Count > 0)
		{
			datas.Sort(new Comparison<GUIGameLoginScene.ServerList>(this.SortByShowID));
			this.mServerListData = datas;
			if (this.fullServerList || this.selectedZoneData == null)
			{
				int num = 0;
				ZoneItemInfoData zoneItemInfoData = null;
				int num2 = -1;
				int num3 = 0;
				int num4 = -1;
				this.mGroupsTable.SetDragAmount(0f, 0f);
				this.mGroupsTable.ClearData();
				int i = 0;
				while (i < datas.Count)
				{
					GUIGameLoginScene.ServerList serverList = datas[i];
					if (GameSetting.Data.LastGMLogin)
					{
						goto IL_112;
					}
					if (serverList.env != 0)
					{
						if (string.IsNullOrEmpty(serverList.platform))
						{
							goto IL_112;
						}
						string[] array = serverList.platform.Split(new char[]
						{
							'|'
						});
						string platform = SdkU3d.getPlatform();
						bool flag = false;
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j] == platform)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							goto IL_112;
						}
					}
					IL_234:
					i++;
					continue;
					IL_112:
					ZoneItemInfoData data = this.GetData(serverList, i);
					if (data == null)
					{
						goto IL_234;
					}
					if (num2 < 0)
					{
						num2 = data.mShowNum;
					}
					num3++;
					if (num3 >= 10)
					{
						this.mGroupsTable.AddData(new GroupItemInfoData(num2, data.mShowNum));
						num2 = -1;
						num3 = 0;
					}
					if (GameSetting.Data.ServerName == serverList.name)
					{
						this.mLastTimeZone.Refresh(data);
						if (this.selectedZoneData == null)
						{
							this.selectedZoneData = data;
						}
						else if (this.selectedZoneData.mName == GameSetting.Data.ServerName)
						{
							this.selectedZoneData = data;
						}
					}
					else if (this.selectedZoneData != null && this.selectedZoneData.mName == serverList.name)
					{
						this.selectedZoneData = data;
					}
					else if (this.selectedZoneData == null && serverList.show_id > num && (zoneItemInfoData == null || serverList.status != 5))
					{
						num = serverList.show_id;
						zoneItemInfoData = data;
					}
					num4 = i;
					goto IL_234;
				}
				if (num2 > 0 && num4 >= 0 && num4 < datas.Count)
				{
					this.mGroupsTable.AddData(new GroupItemInfoData(num2, datas[num4].show_id));
				}
				if (this.selectedZoneData == null)
				{
					this.selectedZoneData = zoneItemInfoData;
				}
			}
			if (this.selectedZoneData != null)
			{
				this.mNum.text = Singleton<StringManager>.Instance.GetString("loginZoneNum", new object[]
				{
					this.selectedZoneData.mShowNum
				});
				this.mName.text = this.selectedZoneData.mName;
				this.mName.color = Color.white;
				this.mNum.color = Color.white;
				switch (this.selectedZoneData.mState)
				{
				case 1:
					this.mState.spriteName = "green";
					break;
				case 2:
					this.mState.spriteName = "green";
					break;
				case 3:
					this.mState.spriteName = "yellow";
					break;
				case 4:
					this.mState.spriteName = "red";
					break;
				case 5:
					this.mState.spriteName = "gray";
					this.mName.color = Color.gray;
					this.mNum.color = Color.gray;
					break;
				}
				int num5 = this.selectedZoneData.mNew;
				if (num5 != 0)
				{
					if (num5 == 1)
					{
						this.mNew.enabled = true;
					}
				}
				else
				{
					this.mNew.enabled = false;
				}
				foreach (BaseData current in this.mGroupsTable.mDatas)
				{
					GroupItemInfoData groupItemInfoData = (GroupItemInfoData)current;
					if (groupItemInfoData.StartNum <= this.selectedZoneData.mShowNum && groupItemInfoData.EndNum >= this.selectedZoneData.mShowNum)
					{
						this.RefreshZones(groupItemInfoData.StartNum, groupItemInfoData.EndNum);
						break;
					}
				}
			}
		}
		this.mZonesTable.repositionNow = true;
	}

	public void RefreshZones(int startNum, int endNum)
	{
		this.mZonesTable.ClearData();
		for (int i = 0; i < this.mServerListData.Count; i++)
		{
			if (GameSetting.Data.LastGMLogin || this.mServerListData[i].env != 0)
			{
				ZoneItemInfoData data = this.GetData(this.mServerListData[i], i);
				if (data != null && data.mShowNum >= startNum && data.mShowNum <= endNum)
				{
					this.mZonesTable.AddData(data);
				}
			}
		}
		this.mZoneListTitle.text = Singleton<StringManager>.Instance.GetString("loginZoneZonesTitle", new object[]
		{
			startNum,
			endNum
		});
	}

	private ZoneItemInfoData GetData(GUIGameLoginScene.ServerList data, int index)
	{
		string[] array = null;
		array = data.ip.Split(new char[]
		{
			':'
		});
		if (array == null || array.Length != 2)
		{
			global::Debug.LogError(new object[]
			{
				"Parse IP string Error, ip: {0}",
				data.ip
			});
			return null;
		}
		int serverPort;
		try
		{
			serverPort = Convert.ToInt32(array[1]);
		}
		catch
		{
			global::Debug.LogError(new object[]
			{
				"Parse ServerPort int Error, ServerPort: {0}",
				array[1]
			});
			return null;
		}
		return new ZoneItemInfoData(data.show_id, data.name, data.id, data.status, data.is_new, array[0], serverPort, index, 2);
	}

	public void SetState(GUIGameLoginScene.EState state)
	{
		if (state != GUIGameLoginScene.EState.StepNativeLogin)
		{
			if (state == GUIGameLoginScene.EState.StepEnter)
			{
				this.mLoginLayer.SetActive(false);
				this.mEnterLayer.SetActive(true);
			}
		}
		else
		{
			this.mLoginLayer.SetActive(true);
			this.mEnterLayer.SetActive(false);
		}
	}

	private void OpenUIIngameScene()
	{
		if (GUIGameLoginScene.first)
		{
			GUIGameLoginScene.first = false;
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIBillboardPopUp, false, null, null);
		}
		if (!GameSetting.Data.UserAgreement)
		{
			GUIAgreementInfoPopUp.Show(true);
		}
		GameUIManager.mInstance.ShowIndicate();
	}

	private void Update()
	{
		if (base.PostLoadGUIDone && this.mEnterLayer.activeInHierarchy && !this.autoRequesting && Time.time - this.timerRefresh >= 120f)
		{
			this.autoRequesting = true;
			this.RetryGetServerList();
		}
	}

	private void SDKEvent(int code, JsonData data)
	{
		this.RefreshPlayerManagerNew();
	}

	private void RefreshPlayerManagerNew()
	{
		if (this.mManagerViewNew != null)
		{
			this.mManagerViewNew.gameObject.SetActive(GameUIManager.mInstance.uiState.SDKPlayerManagerNew);
		}
	}
}
