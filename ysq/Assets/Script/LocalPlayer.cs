using Att;
using cn.sharesdk.unity3d;
using LitJson;
using NtUniSdk.Unity3d;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public sealed class LocalPlayer : MonoBehaviour
{
	public delegate void DataInitCallback(bool versionInit, bool newPlayer);

	public delegate void VoidCallback();

	public delegate void TakeMailAffixCallback(uint mailID);

	public delegate void ChatMessageCallback(ChatMessage chatMsg);

	public delegate void WorldMessageCallback(WorldMessage worldMsg);

	public delegate void OldWorldMessageCallback(WorldMessageExtend oldworldMsg);

	public delegate void SysEventMessageCallback(WorldMessage worldMsg);

	public delegate void ShopDataCallback(int shopType);

	public delegate void ShopBuyItemCallback(int shopType, int id);

	public delegate void TakeRewardCallback(int id);

	public delegate void UseItemCallback(int infoID, int value);

	public LocalPlayer.DataInitCallback DataInitEvent;

	public LocalPlayer.VoidCallback DataVersionEvent;

	public LocalPlayer.VoidCallback PlayerUpdateEvent;

	public LocalPlayer.VoidCallback NewMailEvent;

	public LocalPlayer.VoidCallback GetMailEvent;

	public LocalPlayer.TakeMailAffixCallback TakeMailAffixEvent;

	public LocalPlayer.ChatMessageCallback ChatMessageEvent;

	public LocalPlayer.WorldMessageCallback WorldMessageEvent;

	public LocalPlayer.OldWorldMessageCallback OldWorldMessageEvent;

	public LocalPlayer.SysEventMessageCallback SysEventMessageEvent;

	public LocalPlayer.VoidCallback RecyleMessageEvent;

	public LocalPlayer.ShopDataCallback ShopDataEvent;

	public LocalPlayer.ShopBuyItemCallback ShopBuyItemEvent;

	public LocalPlayer.VoidCallback NameChangeEvent;

	public LocalPlayer.VoidCallback SoulReliquaryUpdateEvent;

	public LocalPlayer.VoidCallback TotalPayUpdateEvent;

	public LocalPlayer.VoidCallback MsgChatEvent;

	public LocalPlayer.VoidCallback RemotePlayerDataEvent;

	public LocalPlayer.TakeRewardCallback QuestTakeRewardEvent;

	public LocalPlayer.UseItemCallback UseItemEvent;

	private Dictionary<int, SceneData> scenes = new Dictionary<int, SceneData>();

	private Dictionary<int, int> mapRewards = new Dictionary<int, int>();

	public List<MailData> Mails = new List<MailData>();

	private bool mailUpdate;

	public List<WorldMessage> SysEventMsgs = new List<WorldMessage>();

	public List<WorldMessageExtend> WorldMsgs = new List<WorldMessageExtend>();

	public List<ChatMessage> GuildMsgs = new List<ChatMessage>();

	public List<ChatMessage> WhisperMsgs = new List<ChatMessage>();

	public List<ChatMessage> CostumePartyMsgs = new List<ChatMessage>();

	private GUIVoiceChatData mVoiceChatData = new GUIVoiceChatData();

	private StringBuilder mSb = new StringBuilder(42);

	public int mCommitTimer;

	public int mCommitTimerPrivate;

	private int frozenFunction;

	private int frozenTimestamp;

	public bool ShowChatWorldNewMark;

	public bool ShowChatGuildNewMark;

	public bool ShowChatWisperNewMark;

	public bool ShowChatPartyNewMark;

	public bool ShowGuildWarNewMark;

	public uint BuyRecordVersion;

	private Dictionary<int, BuyRecord> buyRecords = new Dictionary<int, BuyRecord>();

	public uint PetShopVersion;

	public List<ShopItemData> PetShopData = new List<ShopItemData>();

	public uint AwakeShopVersion;

	public List<ShopItemData> AwakeShopData = new List<ShopItemData>();

	public uint LopetShopVersion;

	public List<ShopItemData> LopetShopData = new List<ShopItemData>();

	private List<PayData> pays;

	public int PayOrderTimer;

	private float localTimeStamp;

	private bool isAppStore;

	private string orderID;

	private int setting;

	private int curSceneID;

	public bool IsFirst = true;

	public uint Version
	{
		get;
		private set;
	}

	public ObscuredStats Data
	{
		get;
		private set;
	}

	public PetSubSystem PetSystem
	{
		get;
		private set;
	}

	public ItemSubSystem ItemSystem
	{
		get;
		private set;
	}

	public TeamSubSystem TeamSystem
	{
		get;
		private set;
	}

	public AchievementSubSystem AchievementSystem
	{
		get;
		private set;
	}

	public WorldBossSubSystem WorldBossSystem
	{
		get;
		private set;
	}

	public PvpSubSystem PvpSystem
	{
		get;
		private set;
	}

	public GuildSubSystem GuildSystem
	{
		get;
		private set;
	}

	public CostumePartySubSystem CostumePartySystem
	{
		get;
		private set;
	}

	public BillboardSubSystem BillboardSystem
	{
		get;
		private set;
	}

	public ActivitySubSystem ActivitySystem
	{
		get;
		private set;
	}

	public FriendSubSystem FriendSystem
	{
		get;
		private set;
	}

	public LopetSubSystem LopetSystem
	{
		get;
		private set;
	}

	public MagicLoveSubSystem MagicLoveSystem
	{
		get;
		private set;
	}

	public uint SceneVersion
	{
		get;
		private set;
	}

	public uint MapRewardVersion
	{
		get;
		private set;
	}

	public QuestInfo MainQuest
	{
		get;
		private set;
	}

	public QuestInfo BranchQuest
	{
		get;
		private set;
	}

	public uint MailVersion
	{
		get;
		private set;
	}

	public uint MaxMailID
	{
		get;
		private set;
	}

	public bool ShowChatBtnAnim
	{
		get
		{
			return this.ShowChatWorldNewMark || this.ShowChatGuildNewMark || this.ShowChatWisperNewMark || this.ShowChatPartyNewMark;
		}
	}

	public bool ShowChatWorldMsg
	{
		get
		{
			return this.ShowChatWorldNewMark;
		}
	}

	private void Awake()
	{
		this.Version = 0u;
		this.SceneVersion = 0u;
		this.MapRewardVersion = 0u;
		this.Data = new ObscuredStats();
		this.PetSystem = new PetSubSystem();
		this.ItemSystem = new ItemSubSystem();
		this.TeamSystem = new TeamSubSystem();
		this.AchievementSystem = new AchievementSubSystem();
		this.WorldBossSystem = new WorldBossSubSystem();
		this.PvpSystem = new PvpSubSystem();
		this.GuildSystem = new GuildSubSystem();
		this.CostumePartySystem = new CostumePartySubSystem();
		this.BillboardSystem = new BillboardSubSystem();
		this.ActivitySystem = new ActivitySubSystem();
		this.FriendSystem = new FriendSubSystem();
		this.LopetSystem = new LopetSubSystem();
		this.MagicLoveSystem = new MagicLoveSubSystem();
		this.ShowChatWorldNewMark = false;
		this.ShowChatGuildNewMark = false;
		this.ShowChatWisperNewMark = false;
		this.ShowChatPartyNewMark = false;
		this.ShowGuildWarNewMark = false;
	}

	private void Start()
	{
		Globals.Instance.CliSession.Register(104, new ClientSession.MsgHandler(this.OnMsgCreatePlayer));
		Globals.Instance.CliSession.Register(106, new ClientSession.MsgHandler(this.OnMsgGetPlayerData));
		Globals.Instance.CliSession.Register(192, new ClientSession.MsgHandler(this.OnMsgUpdatePlayer));
		Globals.Instance.CliSession.Register(608, new ClientSession.MsgHandler(this.OnMsgSceneScore));
		Globals.Instance.CliSession.Register(611, new ClientSession.MsgHandler(this.OnMsgMapReward));
		Globals.Instance.CliSession.Register(211, new ClientSession.MsgHandler(this.OnMsgMailVersionUpdate));
		Globals.Instance.CliSession.Register(213, new ClientSession.MsgHandler(this.OnMsgGetMailData));
		Globals.Instance.CliSession.Register(215, new ClientSession.MsgHandler(this.OnMsgTakeMailAffix));
		Globals.Instance.CliSession.Register(217, new ClientSession.MsgHandler(this.OnMsgChat));
		Globals.Instance.CliSession.Register(218, new ClientSession.MsgHandler(this.OnMsgPlayerChat));
		Globals.Instance.CliSession.Register(219, new ClientSession.MsgHandler(this.OnMsgSystemEvent));
		Globals.Instance.CliSession.Register(513, new ClientSession.MsgHandler(this.OnMsgGetShopData));
		Globals.Instance.CliSession.Register(515, new ClientSession.MsgHandler(this.OnMsgShopBuyItem));
		Globals.Instance.CliSession.Register(239, new ClientSession.MsgHandler(this.OnMsgChangeName));
		Globals.Instance.CliSession.Register(223, new ClientSession.MsgHandler(this.OnMsgBuyEnergy));
		Globals.Instance.CliSession.Register(254, new ClientSession.MsgHandler(this.OnMsgSystemNotice));
		Globals.Instance.CliSession.Register(256, new ClientSession.MsgHandler(this.OnMsgCreateOrder));
		Globals.Instance.CliSession.Register(257, new ClientSession.MsgHandler(this.OnMsgOrderInfo));
		Globals.Instance.CliSession.Register(259, new ClientSession.MsgHandler(this.OnMsgCheckPayResult));
		Globals.Instance.CliSession.Register(260, new ClientSession.MsgHandler(this.OnMsgUpdatePay));
		Globals.Instance.CliSession.Register(399, new ClientSession.MsgHandler(this.OnMsgIAPCheckPayResult));
		Globals.Instance.CliSession.Register(728, new ClientSession.MsgHandler(this.OnMsgApplyCSAServerToken));
		Globals.Instance.CliSession.Register(283, new ClientSession.MsgHandler(this.OnMsgRecycleChat));
		Globals.Instance.CliSession.Register(287, new ClientSession.MsgHandler(this.OnMsgQueryRemotePlayer));
		Globals.Instance.CliSession.Register(629, new ClientSession.MsgHandler(this.OnMsgTrialStart));
		Globals.Instance.CliSession.Register(210, new ClientSession.MsgHandler(this.OnMsgTakeQuestReward));
		Globals.Instance.CliSession.Register(517, new ClientSession.MsgHandler(this.OnMsgUseItem));
		Globals.Instance.CliSession.Register(601, new ClientSession.MsgHandler(this.OnMsgPveStart));
		Globals.Instance.CliSession.Register(603, new ClientSession.MsgHandler(this.OnMsgPveResult));
		Globals.Instance.CliSession.Register(296, new ClientSession.MsgHandler(this.OnMsgComment));
		Globals.Instance.CliSession.Register(208, new ClientSession.MsgHandler(this.OnMsgFrozenPlay));
		SdkU3dCallback.LoginDoneEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.LoginDoneEvent, new SdkU3dCallback.SDKCallback(this.OnSDKLogin));
		SdkU3dCallback.LogoutDoneEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.LogoutDoneEvent, new SdkU3dCallback.SDKCallback(this.OnSDKLogout));
		SdkU3dCallback.OrderCheckEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.OrderCheckEvent, new SdkU3dCallback.SDKCallback(this.OnSDKOrderCheck));
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.OnSDKPlayerManagerNewEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.OnSDKPlayerManagerNewEvent));
		this.PetSystem.Init();
		this.ItemSystem.Init();
		this.TeamSystem.Init();
		this.AchievementSystem.Init();
		this.WorldBossSystem.Init();
		this.PvpSystem.Init();
		this.GuildSystem.Init();
		this.CostumePartySystem.Init();
		this.BillboardSystem.Init();
		this.ActivitySystem.Init();
		this.FriendSystem.Init();
		this.LopetSystem.Init();
		this.MagicLoveSystem.Init();
		base.StartCoroutine(this.StartCheckCacheData());
		base.StartCoroutine(this.CheckVoiceAutoPlayed());
	}

	[DebuggerHidden]
	private IEnumerator StartCheckCacheData()
	{
        return null;
        //LocalPlayer.<StartCheckCacheData>c__IteratorF <StartCheckCacheData>c__IteratorF = new LocalPlayer.<StartCheckCacheData>c__IteratorF();
        //<StartCheckCacheData>c__IteratorF.<>f__this = this;
        //return <StartCheckCacheData>c__IteratorF;
	}

	[DebuggerHidden]
	private IEnumerator UpdateIAPCheck()
	{
        return null;
        //LocalPlayer.<UpdateIAPCheck>c__Iterator10 <UpdateIAPCheck>c__Iterator = new LocalPlayer.<UpdateIAPCheck>c__Iterator10();
        //<UpdateIAPCheck>c__Iterator.<>f__this = this;
        //return <UpdateIAPCheck>c__Iterator;
	}

	private void OnDestroy()
	{
		this.Version = 0u;
		this.SceneVersion = 0u;
		this.scenes.Clear();
		this.MapRewardVersion = 0u;
		this.mapRewards.Clear();
		this.MainQuest = null;
		this.BranchQuest = null;
		this.MailVersion = 0u;
		this.MaxMailID = 0u;
		this.Mails.Clear();
		this.mailUpdate = false;
		this.SysEventMsgs.Clear();
		this.WorldMsgs.Clear();
		this.GuildMsgs.Clear();
		this.WhisperMsgs.Clear();
		this.CostumePartyMsgs.Clear();
		this.BuyRecordVersion = 0u;
		this.buyRecords.Clear();
		this.PetShopVersion = 0u;
		this.PetShopData.Clear();
		this.AwakeShopVersion = 0u;
		this.AwakeShopData.Clear();
		this.LopetShopVersion = 0u;
		this.LopetShopData.Clear();
		this.pays = null;
		this.PayOrderTimer = 0;
		this.isAppStore = false;
		this.curSceneID = 0;
		this.LopetSystem.Destroy();
		this.FriendSystem.Destroy();
		this.ActivitySystem.Destroy();
		this.BillboardSystem.Destroy();
		this.CostumePartySystem.Destroy();
		this.GuildSystem.Destroy();
		this.PvpSystem.Destroy();
		this.WorldBossSystem.Destroy();
		this.AchievementSystem.Destroy();
		this.TeamSystem.Destroy();
		this.ItemSystem.Destroy();
		this.PetSystem.Destroy();
		this.MagicLoveSystem.Destroy();
		this.ShowChatWorldNewMark = false;
		this.ShowChatGuildNewMark = false;
		this.ShowChatWisperNewMark = false;
		this.ShowChatPartyNewMark = false;
		this.ShowGuildWarNewMark = false;
		if (Globals.Instance != null)
		{
			Globals.Instance.TutorialMgr.Destroy();
		}
	}

	private void LoadData(MS2C_GetPlayerData data)
	{
		if (data.StatsVersion != 0u && this.Version != data.StatsVersion)
		{
			this.Version = data.StatsVersion;
			this.Data.Stats = data.StatsData;
			this.localTimeStamp = Time.realtimeSinceStartup;
			PlayerPetInfo.Info.Name = this.Data.Name;
			PlayerPetInfo.Info.Quality = this.GetQuality();
			PlayerPetInfo.Info.Type = this.Data.Gender;
			PetDataEx pet = this.PetSystem.GetPet(100uL);
			if (pet != null)
			{
				pet.Data.Level = this.Data.Level;
				pet.Data.Further = (uint)this.Data.FurtherLevel;
				pet.Data.Awake = (uint)this.Data.AwakeLevel;
				pet.Data.ItemFlag = (uint)this.Data.AwakeItemFlag;
				pet.Data.CultivateCount = this.Data.CultivateCount;
				pet.Data.Attack = this.Data.Attack;
				pet.Data.PhysicDefense = this.Data.PhysicDefense;
				pet.Data.MagicDefense = this.Data.MagicDefense;
				pet.Data.MaxHP = this.Data.MaxHP;
				pet.Data.AttackPreview = this.Data.AttackPreview;
				pet.Data.PhysicDefensePreview = this.Data.PhysicDefensePreview;
				pet.Data.MagicDefensePreview = this.Data.MagicDefensePreview;
				pet.Data.MaxHPPreview = this.Data.MaxHPPreview;
			}
		}
		if (data.SceneVersion != 0u && this.SceneVersion != data.SceneVersion)
		{
			this.SceneVersion = data.SceneVersion;
			this.scenes.Clear();
			for (int i = 0; i < data.Scenes.Count; i++)
			{
				this.scenes.Add(data.Scenes[i].SceneID, data.Scenes[i]);
			}
			this.InitQuest();
		}
		if (data.MapRewardVersion != 0u && this.MapRewardVersion != data.MapRewardVersion)
		{
			this.MapRewardVersion = data.MapRewardVersion;
			this.mapRewards.Clear();
			for (int j = 0; j < data.MapRewards.Count; j++)
			{
				this.mapRewards.Add(data.MapRewards[j].MapID, data.MapRewards[j].RewardMask);
			}
		}
		Globals.Instance.TutorialMgr.Init(data.GuideSteps);
		if (data.MailVersion != 0u && this.MailVersion != data.MailVersion)
		{
			this.MailVersion = data.MailVersion;
			this.MaxMailID = 0u;
			this.LoadMailCacheData();
			for (int k = 0; k < this.Mails.Count; k++)
			{
				if (this.Mails[k].AffixType.Count > 0)
				{
					bool flag = false;
					for (int l = 0; l < data.MailIDs.Count; l++)
					{
						if (this.Mails[k].MailID == data.MailIDs[l])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.Mails[k].MailID = 0u;
						this.mailUpdate = true;
					}
				}
				else if (this.GetTimeStamp() > this.Mails[k].TimeStamp + 259200)
				{
					this.Mails[k].MailID = 0u;
					this.mailUpdate = true;
				}
			}
			this.Mails.RemoveAll(new Predicate<MailData>(LocalPlayer.IsDeletedMail));
			for (int m = 0; m < data.MailIDs.Count; m++)
			{
				bool flag = false;
				for (int n = 0; n < this.Mails.Count; n++)
				{
					if (data.MailIDs[m] == this.Mails[n].MailID)
					{
						flag = true;
						break;
					}
				}
				if (!flag && (this.MaxMailID == 0u || this.MaxMailID > data.MailIDs[m]))
				{
					this.MaxMailID = data.MailIDs[m];
				}
			}
			if (this.MaxMailID == 0u)
			{
				this.MaxMailID = this.MailVersion;
			}
			else if (this.MaxMailID > 0u)
			{
				this.MaxMailID -= 1u;
			}
			else
			{
				this.MaxMailID = 0u;
			}
		}
		if (data.BuyDataVersion != 0u && this.BuyRecordVersion != data.BuyDataVersion)
		{
			this.BuyRecordVersion = data.BuyDataVersion;
			this.buyRecords.Clear();
			for (int num = 0; num < data.BuyData.Count; num++)
			{
				this.buyRecords.Add(data.BuyData[num].ID, data.BuyData[num]);
			}
		}
		this.pays = data.Pays;
		if (SdkU3d.getPlatform() == "ios")
		{
			this.isAppStore = true;
			base.StartCoroutine(this.UpdateIAPCheck());
		}
		else
		{
			this.isAppStore = false;
		}
		this.setting = data.Setting;
		if (data.hasShareParam)
		{
			ShareSDK.InitShareParam(data.ShareParam);
		}
	}

	public int GetTimeStamp()
	{
		return this.Data.TimeStamp + (int)(Time.realtimeSinceStartup - this.localTimeStamp);
	}

	public SceneData GetSceneData(int sceneID)
	{
		SceneData result = null;
		this.scenes.TryGetValue(sceneID, out result);
		return result;
	}

	public int GetSceneScore(int sceneID)
	{
		SceneData sceneData = this.GetSceneData(sceneID);
		if (sceneData == null)
		{
			return 0;
		}
		return sceneData.Score;
	}

	public int GetSceneTimes(int sceneID)
	{
		SceneData sceneData = this.GetSceneData(sceneID);
		if (sceneData == null)
		{
			return 0;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneID);
		if (info == null)
		{
			return 0;
		}
		if (info.DayReset && this.GetTimeStamp() >= sceneData.CoolDown)
		{
			return 0;
		}
		return sceneData.Times;
	}

	public int GetSceneResetCount(int sceneID)
	{
		SceneData sceneData = this.GetSceneData(sceneID);
		if (sceneData == null)
		{
			return 0;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneID);
		if (info == null)
		{
			return 0;
		}
		if (info.DayReset && this.GetTimeStamp() >= sceneData.CoolDown)
		{
			return 0;
		}
		return sceneData.ResetCount;
	}

	public int GetQuestState(int questID)
	{
		SceneData sceneData = this.GetSceneData(questID);
		if (sceneData == null)
		{
			return 0;
		}
		if (!sceneData.QuestReward)
		{
			return 1;
		}
		return 2;
	}

	public int GetMapRewardMask(int mapID)
	{
		int result = 0;
		this.mapRewards.TryGetValue(mapID, out result);
		return result;
	}

	public void SetMapRewardMask(int mapID, int mask)
	{
		if (mask == 0)
		{
			return;
		}
		if (this.mapRewards.ContainsKey(mapID))
		{
			this.mapRewards[mapID] = mask;
		}
		else
		{
			this.mapRewards.Add(mapID, mask);
		}
	}

	public bool HasRedFlag(int mask)
	{
		return 0 != (this.Data.RedFlag & mask);
	}

	public void OnMsgCreatePlayer(MemoryStream stream)
	{
		MS2C_CreatePlayer mS2C_CreatePlayer = Serializer.NonGeneric.Deserialize(typeof(MS2C_CreatePlayer), stream) as MS2C_CreatePlayer;
		if (mS2C_CreatePlayer.Result != 0)
		{
			Globals.Instance.CliSession.OnCreatePlayerError();
			GameUIManager.mInstance.ShowMessageTip("EnterR", mS2C_CreatePlayer.Result);
			return;
		}
		GameCache.LoadCacheData(mS2C_CreatePlayer.Data.StatsData.ID, mS2C_CreatePlayer.Data.StatsData.CreateTime, true);
		this.LoadData(mS2C_CreatePlayer.Data);
		this.PetSystem.LoadData(mS2C_CreatePlayer.Data.PetVersion, mS2C_CreatePlayer.Data.Pets);
		this.ItemSystem.LoadData(mS2C_CreatePlayer.Data.ItemVersion, mS2C_CreatePlayer.Data.Items, mS2C_CreatePlayer.Data.FashionVersion, mS2C_CreatePlayer.Data.Fashions, mS2C_CreatePlayer.Data.FashionTimes);
		this.LopetSystem.LoadData(mS2C_CreatePlayer.Data.LopetVersion, mS2C_CreatePlayer.Data.AssistLopetID, mS2C_CreatePlayer.Data.Lopets);
		this.TeamSystem.LoadData(mS2C_CreatePlayer.Data.SocketVersion, mS2C_CreatePlayer.Data.Sockets, mS2C_CreatePlayer.Data.AssistPetID);
		this.AchievementSystem.LoadData(mS2C_CreatePlayer.Data.AchievementVersion, mS2C_CreatePlayer.Data.Achievements);
		this.CostumePartySystem.LoadData(mS2C_CreatePlayer.Data.RoomID, mS2C_CreatePlayer.Data.CostumePartyTimestamp, mS2C_CreatePlayer.Data.CostumePartyCount, mS2C_CreatePlayer.Data.HasReward);
		this.WorldBossSystem.LoadData(mS2C_CreatePlayer.Data.FDSReward);
		this.ActivitySystem.LoadData(mS2C_CreatePlayer.Data.BuyFundNum, mS2C_CreatePlayer.Data.WorldOpenTimeStamp, mS2C_CreatePlayer.Data.SevenDayVersion, mS2C_CreatePlayer.Data.SDRData, mS2C_CreatePlayer.Data.ShareVersion, mS2C_CreatePlayer.Data.ShardData, mS2C_CreatePlayer.Data.ActivityAchievementVersion, mS2C_CreatePlayer.Data.AAData, mS2C_CreatePlayer.Data.ActivityValueVersion, mS2C_CreatePlayer.Data.AVData, mS2C_CreatePlayer.Data.ActivityShopVersion, mS2C_CreatePlayer.Data.ASData, mS2C_CreatePlayer.Data.APData, mS2C_CreatePlayer.Data.APSData, mS2C_CreatePlayer.Data.REData, mS2C_CreatePlayer.Data.SPData, mS2C_CreatePlayer.Data.GroupBuyingData, mS2C_CreatePlayer.Data.NationalDayData, mS2C_CreatePlayer.Data.HalloweenData);
		this.FriendSystem.LoadData(mS2C_CreatePlayer.Data.FriendList);
		Globals.Instance.CliSession.ResetSerialID((ushort)mS2C_CreatePlayer.Data.MsgSerialID);
		GameAnalytics.UpdatePlayerEvent();
		Hashtable hashtable = new Hashtable();
		string label = string.Empty;
		label = "-kdiCJ7on1kQ18DCxgM";
		if (SdkU3d.getChannel() != "4399com")
		{
			hashtable.Add("admob", AdvertMgr.createRoleAdmob("953196631", label, "0.00", "true"));
			hashtable.Add("inmobi", "{}");
			AdvertMgr.trackEvent("CREATEROLE", hashtable);
		}
		if (this.DataInitEvent != null)
		{
			this.DataInitEvent(true, true);
		}
	}

	public void OnMsgGetPlayerData(MemoryStream stream)
	{
		MS2C_GetPlayerData mS2C_GetPlayerData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetPlayerData), stream) as MS2C_GetPlayerData;
		bool versionInit = false;
		if (this.Version == 0u)
		{
			versionInit = true;
		}
		if (mS2C_GetPlayerData.StatsData != null && this.Data.ID != mS2C_GetPlayerData.StatsData.ID)
		{
			GameCache.LoadCacheData(mS2C_GetPlayerData.StatsData.ID, mS2C_GetPlayerData.StatsData.CreateTime, false);
		}
		this.LoadData(mS2C_GetPlayerData);
		this.PetSystem.LoadData(mS2C_GetPlayerData.PetVersion, mS2C_GetPlayerData.Pets);
		this.ItemSystem.LoadData(mS2C_GetPlayerData.ItemVersion, mS2C_GetPlayerData.Items, mS2C_GetPlayerData.FashionVersion, mS2C_GetPlayerData.Fashions, mS2C_GetPlayerData.FashionTimes);
		this.LopetSystem.LoadData(mS2C_GetPlayerData.LopetVersion, mS2C_GetPlayerData.AssistLopetID, mS2C_GetPlayerData.Lopets);
		this.TeamSystem.LoadData(mS2C_GetPlayerData.SocketVersion, mS2C_GetPlayerData.Sockets, mS2C_GetPlayerData.AssistPetID);
		this.AchievementSystem.LoadData(mS2C_GetPlayerData.AchievementVersion, mS2C_GetPlayerData.Achievements);
		this.CostumePartySystem.LoadData(mS2C_GetPlayerData.RoomID, mS2C_GetPlayerData.CostumePartyTimestamp, mS2C_GetPlayerData.CostumePartyCount, mS2C_GetPlayerData.HasReward);
		this.WorldBossSystem.LoadData(mS2C_GetPlayerData.FDSReward);
		this.ActivitySystem.LoadData(mS2C_GetPlayerData.BuyFundNum, mS2C_GetPlayerData.WorldOpenTimeStamp, mS2C_GetPlayerData.SevenDayVersion, mS2C_GetPlayerData.SDRData, mS2C_GetPlayerData.ShareVersion, mS2C_GetPlayerData.ShardData, mS2C_GetPlayerData.ActivityAchievementVersion, mS2C_GetPlayerData.AAData, mS2C_GetPlayerData.ActivityValueVersion, mS2C_GetPlayerData.AVData, mS2C_GetPlayerData.ActivityShopVersion, mS2C_GetPlayerData.ASData, mS2C_GetPlayerData.APData, mS2C_GetPlayerData.APSData, mS2C_GetPlayerData.REData, mS2C_GetPlayerData.SPData, mS2C_GetPlayerData.GroupBuyingData, mS2C_GetPlayerData.NationalDayData, mS2C_GetPlayerData.HalloweenData);
		this.FriendSystem.LoadData(mS2C_GetPlayerData.FriendList);
		this.GuildSystem.LoadData(mS2C_GetPlayerData.GuildWarData);
		Globals.Instance.CliSession.ResetSerialID((ushort)mS2C_GetPlayerData.MsgSerialID);
		GameAnalytics.UpdatePlayerEvent();
		if (this.DataInitEvent != null)
		{
			this.DataInitEvent(versionInit, false);
		}
	}

	public void OnMsgUpdatePlayer(MemoryStream stream)
	{
		MS2C_UpdatePlayer mS2C_UpdatePlayer = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdatePlayer), stream) as MS2C_UpdatePlayer;
		if (mS2C_UpdatePlayer.Exp != 0u)
		{
			if (mS2C_UpdatePlayer.Exp == 4294967295u)
			{
				this.Data.Exp = 0u;
			}
			else
			{
				this.Data.Exp = mS2C_UpdatePlayer.Exp;
			}
		}
		if (mS2C_UpdatePlayer.Level != 0u)
		{
			this.Data.Level = mS2C_UpdatePlayer.Level;
			this.TeamSystem.OnPlayerLevelup(this.Data.Level);
			GameAnalytics.PlayerLevelupEvent();
			this.OnPlayerLevelup();
		}
		if (mS2C_UpdatePlayer.Energy != 0)
		{
			if (mS2C_UpdatePlayer.Energy == 2147483647)
			{
				this.Data.Energy = 0;
			}
			else
			{
				this.Data.Energy = mS2C_UpdatePlayer.Energy;
			}
		}
		if (mS2C_UpdatePlayer.Stamina != 0)
		{
			if (mS2C_UpdatePlayer.Stamina == 2147483647)
			{
				this.Data.Stamina = 0;
			}
			else
			{
				this.Data.Stamina = mS2C_UpdatePlayer.Stamina;
			}
		}
		if (mS2C_UpdatePlayer.FestivalVoucher != 0)
		{
			if (mS2C_UpdatePlayer.FestivalVoucher == 2147483647)
			{
				this.Data.FestivalVoucher = 0;
			}
			else
			{
				this.Data.FestivalVoucher = mS2C_UpdatePlayer.FestivalVoucher;
			}
		}
		if (mS2C_UpdatePlayer.Money != 0)
		{
			if (mS2C_UpdatePlayer.Money == 2147483647)
			{
				this.Data.Money = 0;
			}
			else
			{
				this.Data.Money = mS2C_UpdatePlayer.Money;
			}
		}
		if (mS2C_UpdatePlayer.Diamond != 0)
		{
			if (mS2C_UpdatePlayer.Diamond == 2147483647)
			{
				this.Data.Diamond = 0;
			}
			else
			{
				this.Data.Diamond = mS2C_UpdatePlayer.Diamond;
			}
		}
		if (mS2C_UpdatePlayer.Honor != 0)
		{
			if (mS2C_UpdatePlayer.Honor == 2147483647)
			{
				this.Data.Honor = 0;
			}
			else
			{
				this.Data.Honor = mS2C_UpdatePlayer.Honor;
			}
		}
		if (mS2C_UpdatePlayer.Reputation != 0)
		{
			if (mS2C_UpdatePlayer.Reputation == 2147483647)
			{
				this.Data.Reputation = 0;
			}
			else
			{
				this.Data.Reputation = mS2C_UpdatePlayer.Reputation;
			}
		}
		if (mS2C_UpdatePlayer.EnergyTimeStamp != 0)
		{
			this.Data.EnergyTimeStamp = mS2C_UpdatePlayer.EnergyTimeStamp;
		}
		if (mS2C_UpdatePlayer.DayLevel != 0u)
		{
			if (mS2C_UpdatePlayer.DayLevel == 4294967295u)
			{
				this.Data.DayLevel = 0u;
			}
			else
			{
				this.Data.DayLevel = mS2C_UpdatePlayer.DayLevel;
			}
		}
		if (mS2C_UpdatePlayer.DayTimeStamp != 0)
		{
			this.Data.DayTimeStamp = mS2C_UpdatePlayer.DayTimeStamp;
		}
		if (mS2C_UpdatePlayer.DataFlag != 0)
		{
			if (mS2C_UpdatePlayer.DataFlag == -1)
			{
				this.Data.DataFlag = 0;
			}
			else
			{
				this.Data.DataFlag = mS2C_UpdatePlayer.DataFlag;
			}
		}
		if (mS2C_UpdatePlayer.RedFlag != 0)
		{
			if (mS2C_UpdatePlayer.RedFlag == -1)
			{
				this.Data.RedFlag = 0;
			}
			else
			{
				this.Data.RedFlag = mS2C_UpdatePlayer.RedFlag;
			}
		}
		if (mS2C_UpdatePlayer.FreeLuckyRollCD1 != 0)
		{
			this.Data.FreeLuckyRollCD1 = mS2C_UpdatePlayer.FreeLuckyRollCD1;
		}
		if (mS2C_UpdatePlayer.FreeLuckyRollCD2 != 0)
		{
			this.Data.FreeLuckyRollCD2 = mS2C_UpdatePlayer.FreeLuckyRollCD2;
		}
		if (mS2C_UpdatePlayer.SignIn != 0)
		{
			if (mS2C_UpdatePlayer.SignIn <= 0)
			{
				this.Data.SignIn = 0;
			}
			else
			{
				this.Data.SignIn = mS2C_UpdatePlayer.SignIn;
			}
		}
		if (mS2C_UpdatePlayer.SignInTimeStamp != 0)
		{
			this.Data.SignInTimeStamp = mS2C_UpdatePlayer.SignInTimeStamp;
		}
		if (mS2C_UpdatePlayer.LevelReward != 0)
		{
			this.Data.LevelReward = mS2C_UpdatePlayer.LevelReward;
		}
		if (mS2C_UpdatePlayer.VipWeekReward != 0)
		{
			if (mS2C_UpdatePlayer.VipWeekReward == -1)
			{
				this.Data.VipWeekReward = 0;
			}
			else
			{
				this.Data.VipWeekReward = mS2C_UpdatePlayer.VipWeekReward;
			}
		}
		if (mS2C_UpdatePlayer.VipPayReward != 0)
		{
			this.Data.VipPayReward = mS2C_UpdatePlayer.VipPayReward;
		}
		if (mS2C_UpdatePlayer.CardTimeStamp != 0)
		{
			this.Data.CardTimeStamp = mS2C_UpdatePlayer.CardTimeStamp;
		}
		if (mS2C_UpdatePlayer.CardFlag != 0)
		{
			if (mS2C_UpdatePlayer.CardFlag == -1)
			{
				this.Data.CardFlag = 0;
			}
			else
			{
				this.Data.CardFlag = mS2C_UpdatePlayer.CardFlag;
			}
		}
		if (mS2C_UpdatePlayer.CardRenew != 0)
		{
			if (mS2C_UpdatePlayer.CardRenew == -1)
			{
				this.Data.CardRenew = 0;
			}
			else
			{
				this.Data.CardRenew = mS2C_UpdatePlayer.CardRenew;
			}
		}
		if (mS2C_UpdatePlayer.SuperCardTimeStamp != 0)
		{
			if (mS2C_UpdatePlayer.SuperCardTimeStamp == -1)
			{
				this.Data.SuperCardTimeStamp = 0;
			}
			else
			{
				this.Data.SuperCardTimeStamp = mS2C_UpdatePlayer.SuperCardTimeStamp;
			}
		}
		if (mS2C_UpdatePlayer.D2MCount != 0)
		{
			if (mS2C_UpdatePlayer.D2MCount == -1)
			{
				this.Data.D2MCount = 0;
			}
			else
			{
				this.Data.D2MCount = mS2C_UpdatePlayer.D2MCount;
			}
		}
		if (mS2C_UpdatePlayer.FreeLuckyRollCount != 0)
		{
			if (mS2C_UpdatePlayer.FreeLuckyRollCount == -1)
			{
				this.Data.FreeLuckyRollCount = 0;
			}
			else
			{
				this.Data.FreeLuckyRollCount = mS2C_UpdatePlayer.FreeLuckyRollCount;
			}
		}
		if (mS2C_UpdatePlayer.VipLevel != 0u)
		{
			if (mS2C_UpdatePlayer.VipLevel == 4294967295u)
			{
				this.Data.VipLevel = 0u;
			}
			else
			{
				this.Data.VipLevel = mS2C_UpdatePlayer.VipLevel;
				GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIVIPLevelUpPopUp, false, null, null);
				GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp((int)mS2C_UpdatePlayer.VipLevel);
			}
		}
		if (mS2C_UpdatePlayer.TotalPay != 0u)
		{
			if (mS2C_UpdatePlayer.TotalPay == 4294967295u)
			{
				this.Data.TotalPay = 0u;
			}
			else
			{
				this.Data.TotalPay = mS2C_UpdatePlayer.TotalPay;
			}
			if (this.TotalPayUpdateEvent != null)
			{
				this.TotalPayUpdateEvent();
			}
		}
		if (mS2C_UpdatePlayer.Common2ShopRefresh != 0u)
		{
			if (mS2C_UpdatePlayer.Common2ShopRefresh == 4294967295u)
			{
				this.Data.Common2ShopRefresh = 0u;
			}
			else
			{
				this.Data.Common2ShopRefresh = mS2C_UpdatePlayer.Common2ShopRefresh;
			}
		}
		if (mS2C_UpdatePlayer.AwakenShopRefresh != 0u)
		{
			if (mS2C_UpdatePlayer.AwakenShopRefresh == 4294967295u)
			{
				this.Data.AwakenShopRefresh = 0u;
			}
			else
			{
				this.Data.AwakenShopRefresh = mS2C_UpdatePlayer.AwakenShopRefresh;
			}
		}
		if (mS2C_UpdatePlayer.LopetShopRefresh != 0)
		{
			if (mS2C_UpdatePlayer.LopetShopRefresh == -1)
			{
				this.Data.LopetShopRefresh = 0;
			}
			else
			{
				this.Data.LopetShopRefresh = mS2C_UpdatePlayer.LopetShopRefresh;
			}
		}
		if (mS2C_UpdatePlayer.MagicCrystal != 0)
		{
			if (mS2C_UpdatePlayer.MagicCrystal == 2147483647)
			{
				this.Data.MagicCrystal = 0;
			}
			else
			{
				this.Data.MagicCrystal = mS2C_UpdatePlayer.MagicCrystal;
			}
		}
		if (mS2C_UpdatePlayer.LRTimeStamp != 0)
		{
			this.Data.LRTimeStamp = mS2C_UpdatePlayer.LRTimeStamp;
		}
		if (mS2C_UpdatePlayer.OnlineDays != 0u)
		{
			if (mS2C_UpdatePlayer.OnlineDays == 4294967295u)
			{
				this.Data.OnlineDays = 0u;
			}
			else
			{
				this.Data.OnlineDays = mS2C_UpdatePlayer.OnlineDays;
			}
		}
		if (mS2C_UpdatePlayer.Day7Flag != 0)
		{
			if (mS2C_UpdatePlayer.Day7Flag == -1)
			{
				this.Data.Day7Flag = 0;
			}
			else
			{
				this.Data.Day7Flag = mS2C_UpdatePlayer.Day7Flag;
			}
		}
		if (mS2C_UpdatePlayer.KingMedal != 0)
		{
			if (mS2C_UpdatePlayer.KingMedal == 2147483647)
			{
				this.Data.KingMedal = 0;
			}
			else
			{
				this.Data.KingMedal = mS2C_UpdatePlayer.KingMedal;
			}
		}
		if (mS2C_UpdatePlayer.LRTimeStamp != 0)
		{
			this.Data.LRTimeStamp = mS2C_UpdatePlayer.LRTimeStamp;
			this.Data.LRPetID = mS2C_UpdatePlayer.LRPetID;
			this.Data.LRPetID1 = mS2C_UpdatePlayer.LRPetID1;
			this.Data.LRPetID2 = mS2C_UpdatePlayer.LRPetID2;
			this.Data.LRPetID3 = mS2C_UpdatePlayer.LRPetID3;
			if (this.SoulReliquaryUpdateEvent != null)
			{
				this.SoulReliquaryUpdateEvent();
			}
		}
		if (mS2C_UpdatePlayer.LuckyRoll2Count != 0)
		{
			if (mS2C_UpdatePlayer.LuckyRoll2Count == -1)
			{
				this.Data.LuckyRoll2Count = 0;
			}
			else
			{
				this.Data.LuckyRoll2Count = mS2C_UpdatePlayer.LuckyRoll2Count;
			}
		}
		if (mS2C_UpdatePlayer.HasGuild != 0)
		{
			if (mS2C_UpdatePlayer.HasGuild == -1)
			{
				this.Data.HasGuild = 0;
			}
			else
			{
				this.Data.HasGuild = mS2C_UpdatePlayer.HasGuild;
			}
		}
		if (mS2C_UpdatePlayer.GuildBossCount != 0)
		{
			if (mS2C_UpdatePlayer.GuildBossCount == -1)
			{
				this.Data.GuildBossCount = 0;
			}
			else
			{
				this.Data.GuildBossCount = mS2C_UpdatePlayer.GuildBossCount;
			}
		}
		if (mS2C_UpdatePlayer.ConstellationLevel != 0)
		{
			if (mS2C_UpdatePlayer.ConstellationLevel == -1)
			{
				this.Data.ConstellationLevel = 0;
			}
			else
			{
				this.Data.ConstellationLevel = mS2C_UpdatePlayer.ConstellationLevel;
			}
			PlayerPetInfo.Info.Quality = this.GetQuality();
			this.TeamSystem.OnConLevelup(this.Data.ConstellationLevel);
		}
		if (mS2C_UpdatePlayer.FurtherLevel != 0)
		{
			if (mS2C_UpdatePlayer.FurtherLevel == -1)
			{
				this.Data.FurtherLevel = 0;
			}
			else
			{
				this.Data.FurtherLevel = mS2C_UpdatePlayer.FurtherLevel;
			}
			this.TeamSystem.OnPlayerFurther(this.Data.FurtherLevel);
		}
		if (mS2C_UpdatePlayer.StaminaTimeStamp != 0)
		{
			if (mS2C_UpdatePlayer.StaminaTimeStamp == -1)
			{
				this.Data.StaminaTimeStamp = 0;
			}
			else
			{
				this.Data.StaminaTimeStamp = mS2C_UpdatePlayer.StaminaTimeStamp;
			}
		}
		if (mS2C_UpdatePlayer.MagicSoul != 0)
		{
			if (mS2C_UpdatePlayer.MagicSoul == 2147483647)
			{
				this.Data.MagicSoul = 0;
			}
			else
			{
				this.Data.MagicSoul = mS2C_UpdatePlayer.MagicSoul;
			}
		}
		if (mS2C_UpdatePlayer.LopetSoul != 0)
		{
			if (mS2C_UpdatePlayer.LopetSoul == 2147483647)
			{
				this.Data.LopetSoul = 0;
			}
			else
			{
				this.Data.LopetSoul = mS2C_UpdatePlayer.LopetSoul;
			}
		}
		if (mS2C_UpdatePlayer.FestivalVoucher != 0)
		{
			if (mS2C_UpdatePlayer.FestivalVoucher == 2147483647)
			{
				this.Data.FestivalVoucher = 0;
			}
			else
			{
				this.Data.FestivalVoucher = mS2C_UpdatePlayer.FestivalVoucher;
			}
		}
		if (mS2C_UpdatePlayer.FireDragonScale != 0)
		{
			if (mS2C_UpdatePlayer.FireDragonScale == 2147483647)
			{
				this.Data.FireDragonScale = 0;
			}
			else
			{
				this.Data.FireDragonScale = mS2C_UpdatePlayer.FireDragonScale;
			}
		}
		if (mS2C_UpdatePlayer.WarFreeTime != 0)
		{
			if (mS2C_UpdatePlayer.WarFreeTime == -1)
			{
				this.Data.WarFreeTime = 0;
			}
			else
			{
				this.Data.WarFreeTime = mS2C_UpdatePlayer.WarFreeTime;
			}
		}
		if (mS2C_UpdatePlayer.ShopCommon2TimeStamp != 0)
		{
			if (mS2C_UpdatePlayer.ShopCommon2TimeStamp == -1)
			{
				this.Data.ShopCommon2TimeStamp = 0;
			}
			else
			{
				this.Data.ShopCommon2TimeStamp = mS2C_UpdatePlayer.ShopCommon2TimeStamp;
			}
		}
		if (mS2C_UpdatePlayer.ShopAwakenTimeStamp != 0)
		{
			if (mS2C_UpdatePlayer.ShopAwakenTimeStamp == -1)
			{
				this.Data.ShopAwakenTimeStamp = 0;
			}
			else
			{
				this.Data.ShopAwakenTimeStamp = mS2C_UpdatePlayer.ShopAwakenTimeStamp;
			}
		}
		if (mS2C_UpdatePlayer.TrialMaxWave != 0)
		{
			if (mS2C_UpdatePlayer.TrialMaxWave == -1)
			{
				this.Data.TrialMaxWave = 0;
			}
			else
			{
				this.Data.TrialMaxWave = mS2C_UpdatePlayer.TrialMaxWave;
			}
		}
		if (mS2C_UpdatePlayer.TrialWave != 0)
		{
			if (mS2C_UpdatePlayer.TrialWave == -1)
			{
				this.Data.TrialWave = 0;
			}
			else
			{
				this.Data.TrialWave = mS2C_UpdatePlayer.TrialWave;
			}
		}
		if (mS2C_UpdatePlayer.TrialResetCount != 0)
		{
			if (mS2C_UpdatePlayer.TrialResetCount == -1)
			{
				this.Data.TrialResetCount = 0;
			}
			else
			{
				this.Data.TrialResetCount = mS2C_UpdatePlayer.TrialResetCount;
			}
		}
		if (mS2C_UpdatePlayer.TrialFarmTimeStamp != 0)
		{
			if (mS2C_UpdatePlayer.TrialFarmTimeStamp == -1)
			{
				this.Data.TrialFarmTimeStamp = 0;
			}
			else
			{
				this.Data.TrialFarmTimeStamp = mS2C_UpdatePlayer.TrialFarmTimeStamp;
			}
		}
		if (mS2C_UpdatePlayer.TrialOver != 0)
		{
			if (mS2C_UpdatePlayer.TrialOver == -1)
			{
				this.Data.TrialOver = 0;
			}
			else
			{
				this.Data.TrialOver = mS2C_UpdatePlayer.TrialOver;
			}
		}
		if (mS2C_UpdatePlayer.DailyScore != 0)
		{
			if (mS2C_UpdatePlayer.DailyScore == -1)
			{
				this.Data.DailyScore = 0;
			}
			else
			{
				this.Data.DailyScore = mS2C_UpdatePlayer.DailyScore;
			}
		}
		if (mS2C_UpdatePlayer.DailyRewardFlag != 0)
		{
			if (mS2C_UpdatePlayer.DailyRewardFlag == -1)
			{
				this.Data.DailyRewardFlag = 0;
			}
			else
			{
				this.Data.DailyRewardFlag = mS2C_UpdatePlayer.DailyRewardFlag;
			}
		}
		if (mS2C_UpdatePlayer.GuildScoreRewardFlag != 0)
		{
			if (mS2C_UpdatePlayer.GuildScoreRewardFlag == -1)
			{
				this.Data.GuildScoreRewardFlag = 0;
			}
			else
			{
				this.Data.GuildScoreRewardFlag = mS2C_UpdatePlayer.GuildScoreRewardFlag;
			}
		}
		if (mS2C_UpdatePlayer.FundFlag != 0)
		{
			if (mS2C_UpdatePlayer.FundFlag == -1)
			{
				this.Data.FundFlag = 0;
			}
			else
			{
				this.Data.FundFlag = mS2C_UpdatePlayer.FundFlag;
			}
		}
		if (mS2C_UpdatePlayer.WelfareFlag != 0)
		{
			if (mS2C_UpdatePlayer.WelfareFlag == -1)
			{
				this.Data.WelfareFlag = 0;
			}
			else
			{
				this.Data.WelfareFlag = mS2C_UpdatePlayer.WelfareFlag;
			}
		}
		if (mS2C_UpdatePlayer.DayHotFlag != 0)
		{
			if (mS2C_UpdatePlayer.DayHotFlag == -1)
			{
				this.Data.DayHotFlag = 0;
			}
			else
			{
				this.Data.DayHotFlag = mS2C_UpdatePlayer.DayHotFlag;
			}
		}
		if (mS2C_UpdatePlayer.AwakeLevel != 0)
		{
			if (mS2C_UpdatePlayer.AwakeLevel == -1)
			{
				this.Data.AwakeLevel = 0;
			}
			else
			{
				this.Data.AwakeLevel = mS2C_UpdatePlayer.AwakeLevel;
			}
			this.TeamSystem.OnPlayerAwakeLevelup(this.Data.AwakeLevel);
		}
		if (mS2C_UpdatePlayer.AwakeItemFlag != 0)
		{
			if (mS2C_UpdatePlayer.AwakeItemFlag == -1)
			{
				this.Data.AwakeItemFlag = 0;
			}
			else
			{
				this.Data.AwakeItemFlag = mS2C_UpdatePlayer.AwakeItemFlag;
			}
			this.TeamSystem.OnPlayerAwakeEquipItem(this.Data.AwakeItemFlag);
		}
		if (mS2C_UpdatePlayer.StarSoul != 0)
		{
			if (mS2C_UpdatePlayer.StarSoul == 2147483647)
			{
				this.Data.StarSoul = 0;
			}
			else
			{
				this.Data.StarSoul = mS2C_UpdatePlayer.StarSoul;
			}
		}
		if (mS2C_UpdatePlayer.WeekTimestamp != 0)
		{
			if (mS2C_UpdatePlayer.WeekTimestamp == -1)
			{
				this.Data.WeekTimestamp = 0;
			}
			else
			{
				this.Data.WeekTimestamp = mS2C_UpdatePlayer.WeekTimestamp;
			}
		}
		if (mS2C_UpdatePlayer.Praise != 0)
		{
			if (mS2C_UpdatePlayer.Praise == -1)
			{
				this.Data.Praise = 0;
			}
			else
			{
				this.Data.Praise = mS2C_UpdatePlayer.Praise;
			}
		}
		if (mS2C_UpdatePlayer.Emblem != 0)
		{
			if (mS2C_UpdatePlayer.Emblem == 2147483647)
			{
				this.Data.Emblem = 0;
			}
			else
			{
				this.Data.Emblem = mS2C_UpdatePlayer.Emblem;
			}
		}
		if (mS2C_UpdatePlayer.TakeFriendEnergy != 0)
		{
			if (mS2C_UpdatePlayer.TakeFriendEnergy == -1)
			{
				this.Data.TakeFriendEnergy = 0;
			}
			else
			{
				this.Data.TakeFriendEnergy = mS2C_UpdatePlayer.TakeFriendEnergy;
			}
		}
		if (mS2C_UpdatePlayer.TakeGuildGift != 0)
		{
			if (mS2C_UpdatePlayer.TakeGuildGift == -1)
			{
				this.Data.TakeGuildGift = 0;
			}
			else
			{
				this.Data.TakeGuildGift = mS2C_UpdatePlayer.TakeGuildGift;
			}
		}
		if (mS2C_UpdatePlayer.MGCount != 0)
		{
			if (mS2C_UpdatePlayer.MGCount == -1)
			{
				this.Data.MGCount = 0;
			}
			else
			{
				this.Data.MGCount = mS2C_UpdatePlayer.MGCount;
			}
		}
		if (mS2C_UpdatePlayer.NightmareCount != 0)
		{
			if (mS2C_UpdatePlayer.NightmareCount == -1)
			{
				this.Data.NightmareCount = 0;
			}
			else
			{
				this.Data.NightmareCount = mS2C_UpdatePlayer.NightmareCount;
			}
		}
		this.UpdatePlayerCultivate(mS2C_UpdatePlayer);
		if (mS2C_UpdatePlayer.StatsVersion != 0u)
		{
			this.Version = mS2C_UpdatePlayer.StatsVersion;
		}
		if (this.PlayerUpdateEvent != null)
		{
			this.PlayerUpdateEvent();
		}
	}

	private void UpdatePlayerCultivate(MS2C_UpdatePlayer reply)
	{
		bool flag = false;
		bool flag2 = false;
		if (reply.Attack != 0)
		{
			flag = true;
			if (reply.Attack == -1)
			{
				this.Data.Attack = 0;
			}
			else
			{
				this.Data.Attack = reply.Attack;
			}
		}
		if (reply.PhysicDefense != 0)
		{
			flag = true;
			if (reply.PhysicDefense == -1)
			{
				this.Data.PhysicDefense = 0;
			}
			else
			{
				this.Data.PhysicDefense = reply.PhysicDefense;
			}
		}
		if (reply.MagicDefense != 0)
		{
			flag = true;
			if (reply.MagicDefense == -1)
			{
				this.Data.MagicDefense = 0;
			}
			else
			{
				this.Data.MagicDefense = reply.MagicDefense;
			}
		}
		if (reply.MaxHP != 0)
		{
			flag = true;
			if (reply.MaxHP == -1)
			{
				this.Data.MaxHP = 0;
			}
			else
			{
				this.Data.MaxHP = reply.MaxHP;
			}
		}
		if (reply.AttackPreview != 0)
		{
			flag2 = true;
			if (reply.AttackPreview == 2147483647)
			{
				this.Data.AttackPreview = 0;
			}
			else
			{
				this.Data.AttackPreview = reply.AttackPreview;
			}
		}
		if (reply.PhysicDefensePreview != 0)
		{
			flag2 = true;
			if (reply.PhysicDefensePreview == 2147483647)
			{
				this.Data.PhysicDefensePreview = 0;
			}
			else
			{
				this.Data.PhysicDefensePreview = reply.PhysicDefensePreview;
			}
		}
		if (reply.MagicDefensePreview != 0)
		{
			flag2 = true;
			if (reply.MagicDefensePreview == 2147483647)
			{
				this.Data.MagicDefensePreview = 0;
			}
			else
			{
				this.Data.MagicDefensePreview = reply.MagicDefensePreview;
			}
		}
		if (reply.MaxHPPreview != 0)
		{
			flag2 = true;
			if (reply.MaxHPPreview == 2147483647)
			{
				this.Data.MaxHPPreview = 0;
			}
			else
			{
				this.Data.MaxHPPreview = reply.MaxHPPreview;
			}
		}
		if (reply.CultivateCount != 0)
		{
			if (reply.CultivateCount == -1)
			{
				this.Data.CultivateCount = 0;
			}
			else
			{
				this.Data.CultivateCount = reply.CultivateCount;
			}
		}
		if (reply.MGFlag != 0)
		{
			if (reply.MGFlag == -1)
			{
				this.Data.MGFlag = 0;
			}
			else
			{
				this.Data.MGFlag = reply.MGFlag;
			}
		}
		if (flag)
		{
			this.TeamSystem.OnPlayerCultivate(this.Data.Attack, this.Data.PhysicDefense, this.Data.MagicDefense, this.Data.MaxHP);
		}
		if (flag2)
		{
			PetDataEx pet = this.TeamSystem.GetPet(0);
			if (pet != null && pet.Data.ID == 100uL && pet.Data.InfoID == 90000)
			{
				pet.Data.AttackPreview = this.Data.AttackPreview;
				pet.Data.PhysicDefensePreview = this.Data.PhysicDefensePreview;
				pet.Data.MagicDefensePreview = this.Data.MagicDefensePreview;
				pet.Data.MaxHPPreview = this.Data.MaxHPPreview;
			}
		}
	}

	public void OnMsgSceneScore(MemoryStream stream)
	{
		MS2C_SceneScore mS2C_SceneScore = Serializer.NonGeneric.Deserialize(typeof(MS2C_SceneScore), stream) as MS2C_SceneScore;
		SceneData sceneData = this.GetSceneData(mS2C_SceneScore.SceneID);
		if (sceneData != null)
		{
			if (sceneData.Score < mS2C_SceneScore.Score)
			{
				sceneData.Score = mS2C_SceneScore.Score;
			}
			sceneData.Times = mS2C_SceneScore.Times;
			sceneData.ResetCount = mS2C_SceneScore.ResetCount;
			if (sceneData.CoolDown < mS2C_SceneScore.CoolDown)
			{
				sceneData.CoolDown = mS2C_SceneScore.CoolDown;
			}
		}
		else
		{
			sceneData = new SceneData();
			sceneData.SceneID = mS2C_SceneScore.SceneID;
			sceneData.Score = mS2C_SceneScore.Score;
			sceneData.Times = mS2C_SceneScore.Times;
			sceneData.ResetCount = mS2C_SceneScore.ResetCount;
			sceneData.CoolDown = mS2C_SceneScore.CoolDown;
			this.scenes.Add(sceneData.SceneID, sceneData);
		}
		if (mS2C_SceneScore.SceneVersion != 0u)
		{
			this.SceneVersion = mS2C_SceneScore.SceneVersion;
		}
	}

	public void OnMsgMapReward(MemoryStream stream)
	{
		MS2C_MapReward mS2C_MapReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_MapReward), stream) as MS2C_MapReward;
		this.SetMapRewardMask(mS2C_MapReward.MapID, mS2C_MapReward.Mask);
		if (mS2C_MapReward.MapRewardVersion != 0u)
		{
			this.MapRewardVersion = mS2C_MapReward.MapRewardVersion;
		}
	}

	public void OnMsgMailVersionUpdate(MemoryStream stream)
	{
		MS2C_MailVersionUpdate mS2C_MailVersionUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_MailVersionUpdate), stream) as MS2C_MailVersionUpdate;
		this.MailVersion = mS2C_MailVersionUpdate.MailVersion;
		if (this.NewMailEvent != null)
		{
			this.NewMailEvent();
		}
	}

	public void OnMsgGetMailData(MemoryStream stream)
	{
		MS2C_GetMailData mS2C_GetMailData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetMailData), stream) as MS2C_GetMailData;
		for (int i = 0; i < mS2C_GetMailData.Data.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < this.Mails.Count; j++)
			{
				if (mS2C_GetMailData.Data[i].MailID == this.Mails[j].MailID)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (mS2C_GetMailData.Data[i].MailID > this.MaxMailID)
				{
					this.MaxMailID = mS2C_GetMailData.Data[i].MailID;
				}
				this.Mails.Add(mS2C_GetMailData.Data[i]);
			}
		}
		this.mailUpdate = true;
		if (this.GetMailEvent != null)
		{
			this.GetMailEvent();
		}
	}

	public void OnMsgTakeMailAffix(MemoryStream stream)
	{
		MS2C_TakeMailAffix mS2C_TakeMailAffix = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeMailAffix), stream) as MS2C_TakeMailAffix;
		if (mS2C_TakeMailAffix.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeMailAffix.Result);
			return;
		}
		for (int i = 0; i < this.Mails.Count; i++)
		{
			if (this.Mails[i].MailID == mS2C_TakeMailAffix.MailID)
			{
				List<RewardData> list = new List<RewardData>();
				for (int j = 0; j < this.Mails[i].AffixType.Count; j++)
				{
					if (this.Mails[i].AffixType[j] == 1)
					{
						list.Add(new RewardData
						{
							RewardType = 2,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 17)
					{
						list.Add(new RewardData
						{
							RewardType = 18,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 0)
					{
						list.Add(new RewardData
						{
							RewardType = 1,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 4)
					{
						list.Add(new RewardData
						{
							RewardType = 14,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 5)
					{
						list.Add(new RewardData
						{
							RewardType = 7,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 2)
					{
						list.Add(new RewardData
						{
							RewardType = 3,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = this.Mails[i].AffixValue2[j]
						});
					}
					else if (this.Mails[i].AffixType[j] == 3)
					{
						list.Add(new RewardData
						{
							RewardType = 4,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 15)
					{
						list.Add(new RewardData
						{
							RewardType = 16,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 6)
					{
						list.Add(new RewardData
						{
							RewardType = 5,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 7)
					{
						list.Add(new RewardData
						{
							RewardType = 6,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 8)
					{
						list.Add(new RewardData
						{
							RewardType = 8,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 9)
					{
						list.Add(new RewardData
						{
							RewardType = 9,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 6)
					{
						list.Add(new RewardData
						{
							RewardType = 5,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 10)
					{
						list.Add(new RewardData
						{
							RewardType = 10,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 11)
					{
						list.Add(new RewardData
						{
							RewardType = 11,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 13)
					{
						list.Add(new RewardData
						{
							RewardType = 13,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
					else if (this.Mails[i].AffixType[j] == 16)
					{
						list.Add(new RewardData
						{
							RewardType = 17,
							RewardValue1 = this.Mails[i].AffixValue1[j],
							RewardValue2 = 0
						});
					}
				}
				if (list.Count > 0)
				{
					GUIRewardPanel.Show(list, null, false, true, null, false);
				}
				GameAnalytics.OnTakeMailAffixReward(this.Mails[i]);
				this.Mails.RemoveAt(i);
				break;
			}
		}
		this.mailUpdate = true;
		if (this.TakeMailAffixEvent != null)
		{
			this.TakeMailAffixEvent(mS2C_TakeMailAffix.MailID);
		}
	}

	public void OnMsgChat(MemoryStream stream)
	{
		MS2C_Chat mS2C_Chat = Serializer.NonGeneric.Deserialize(typeof(MS2C_Chat), stream) as MS2C_Chat;
		if (mS2C_Chat.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_Chat.Result);
			return;
		}
		if (this.MsgChatEvent != null)
		{
			this.MsgChatEvent();
		}
	}

	public void OnMsgPlayerChat(MemoryStream stream)
	{
		MS2C_PlayerChat mS2C_PlayerChat = Serializer.NonGeneric.Deserialize(typeof(MS2C_PlayerChat), stream) as MS2C_PlayerChat;
		for (int i = 0; i < mS2C_PlayerChat.Data.Count; i++)
		{
			if (Globals.Instance.Player.FriendSystem.GetBlack(mS2C_PlayerChat.Data[i].PlayerID) == null)
			{
				if (mS2C_PlayerChat.Data[i].Channel == 0 && mS2C_PlayerChat.Data[i].Type == 1u)
				{
					mS2C_PlayerChat.Data[i].Channel = 3;
				}
				this.PushChatMessage(mS2C_PlayerChat.Data[i]);
			}
		}
		this.IsFirst = false;
	}

	public void OnMsgSystemEvent(MemoryStream stream)
	{
		MS2C_SystemEvent mS2C_SystemEvent = Serializer.NonGeneric.Deserialize(typeof(MS2C_SystemEvent), stream) as MS2C_SystemEvent;
		if (0 < mS2C_SystemEvent.EventType && mS2C_SystemEvent.EventType < 14)
		{
			if (this.SysEventMsgs.Count >= 20)
			{
				this.SysEventMsgs.RemoveAt(0);
			}
			WorldMessage worldMessage = new WorldMessage();
			worldMessage.SysEvent = mS2C_SystemEvent;
			this.SysEventMsgs.Add(worldMessage);
			this.OnSysMessageEvent(worldMessage);
			if (this.SysEventMessageEvent != null)
			{
				this.SysEventMessageEvent(worldMessage);
			}
		}
	}

	public void OnMsgGetShopData(MemoryStream stream)
	{
		MS2C_GetShopData mS2C_GetShopData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetShopData), stream) as MS2C_GetShopData;
		if (mS2C_GetShopData.Result == 10)
		{
			GameMessageBox.ShowRechargeMessageBox();
			return;
		}
		if (mS2C_GetShopData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_GetShopData.Result);
			return;
		}
		if (mS2C_GetShopData.DiamondRefresh)
		{
			GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.ShopRefresh, 20.0);
		}
		if (mS2C_GetShopData.ShopType == 0)
		{
			if (this.PetShopVersion != mS2C_GetShopData.ShopVersion)
			{
				this.PetShopVersion = mS2C_GetShopData.ShopVersion;
				this.PetShopData = mS2C_GetShopData.Data;
			}
		}
		else if (mS2C_GetShopData.ShopType == 1)
		{
			if (this.AwakeShopVersion != mS2C_GetShopData.ShopVersion)
			{
				this.AwakeShopVersion = mS2C_GetShopData.ShopVersion;
				this.AwakeShopData = mS2C_GetShopData.Data;
			}
		}
		else
		{
			if (mS2C_GetShopData.ShopType != 9)
			{
				global::Debug.LogError(new object[]
				{
					"error shop type"
				});
				return;
			}
			if (this.LopetShopVersion != mS2C_GetShopData.ShopVersion)
			{
				this.LopetShopVersion = mS2C_GetShopData.ShopVersion;
				this.LopetShopData = mS2C_GetShopData.Data;
			}
		}
		if (this.ShopDataEvent != null)
		{
			this.ShopDataEvent(mS2C_GetShopData.ShopType);
		}
	}

	public void OnMsgShopBuyItem(MemoryStream stream)
	{
		MS2C_ShopBuyItem mS2C_ShopBuyItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_ShopBuyItem), stream) as MS2C_ShopBuyItem;
		if (mS2C_ShopBuyItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_ShopBuyItem.Result);
			return;
		}
		if (mS2C_ShopBuyItem.ShopType == 0)
		{
			ShopItemData petShopItem = this.GetPetShopItem(mS2C_ShopBuyItem.ID);
			if (petShopItem != null)
			{
				petShopItem.BuyCount = mS2C_ShopBuyItem.BuyCount;
			}
			this.PetShopVersion = mS2C_ShopBuyItem.ShopVersion;
			GameAnalytics.ShopBuyItemEvent(petShopItem);
		}
		else if (mS2C_ShopBuyItem.ShopType == 1)
		{
			ShopItemData awakeShopItem = this.GetAwakeShopItem(mS2C_ShopBuyItem.ID);
			if (awakeShopItem != null)
			{
				awakeShopItem.BuyCount = mS2C_ShopBuyItem.BuyCount;
			}
			this.AwakeShopVersion = mS2C_ShopBuyItem.ShopVersion;
			GameAnalytics.ShopBuyItemEvent(awakeShopItem);
		}
		else if (mS2C_ShopBuyItem.ShopType == 9)
		{
			ShopItemData lopetShopItem = this.GetLopetShopItem(mS2C_ShopBuyItem.ID);
			if (lopetShopItem != null)
			{
				lopetShopItem.BuyCount = mS2C_ShopBuyItem.BuyCount;
			}
			this.LopetShopVersion = mS2C_ShopBuyItem.ShopVersion;
			GameAnalytics.ShopBuyItemEvent(lopetShopItem);
		}
		else
		{
			this.UpdateBuyRecord(mS2C_ShopBuyItem.ID, (int)mS2C_ShopBuyItem.BuyCount, mS2C_ShopBuyItem.TimeStamp, mS2C_ShopBuyItem.ShopVersion);
		}
		if (this.ShopBuyItemEvent != null)
		{
			this.ShopBuyItemEvent(mS2C_ShopBuyItem.ShopType, mS2C_ShopBuyItem.ID);
		}
	}

	public void OnMsgChangeName(MemoryStream stream)
	{
		MS2C_ChangeName mS2C_ChangeName = Serializer.NonGeneric.Deserialize(typeof(MS2C_ChangeName), stream) as MS2C_ChangeName;
		if (mS2C_ChangeName.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_ChangeName.Result);
			return;
		}
		this.Version = mS2C_ChangeName.StatsVersion;
		this.Data.Name = mS2C_ChangeName.Name;
		PlayerPetInfo.Info.Name = this.Data.Name;
		if (this.NameChangeEvent != null)
		{
			this.NameChangeEvent();
		}
	}

	public void OnMsgBuyEnergy(MemoryStream stream)
	{
		MS2C_BuyEnergy mS2C_BuyEnergy = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyEnergy), stream) as MS2C_BuyEnergy;
		if (mS2C_BuyEnergy.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_BuyEnergy.Result);
			return;
		}
	}

	public void OnMsgSystemNotice(MemoryStream stream)
	{
		MS2C_SystemNotice mS2C_SystemNotice = Serializer.NonGeneric.Deserialize(typeof(MS2C_SystemNotice), stream) as MS2C_SystemNotice;
		GameUIManager.mInstance.ShowGameNew(mS2C_SystemNotice.Content, mS2C_SystemNotice.Priority, 110, true);
	}

	public void OnMsgCreateOrder(MemoryStream stream)
	{
		MS2C_CreateOrder mS2C_CreateOrder = Serializer.NonGeneric.Deserialize(typeof(MS2C_CreateOrder), stream) as MS2C_CreateOrder;
		if (mS2C_CreateOrder.Result == 0)
		{
			return;
		}
		if (mS2C_CreateOrder.Result == 57)
		{
			string @string = Singleton<StringManager>.Instance.GetString("OrderProcess", new object[]
			{
				mS2C_CreateOrder.CDTime
			});
			GameUIManager.mInstance.ShowMessageTip(@string, 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_CreateOrder.Result);
	}

	public void OnMsgOrderInfo(MemoryStream stream)
	{
		MS2C_OrderInfo mS2C_OrderInfo = Serializer.NonGeneric.Deserialize(typeof(MS2C_OrderInfo), stream) as MS2C_OrderInfo;
		if (mS2C_OrderInfo.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_OrderInfo.Result);
			return;
		}
		PayInfo info = Globals.Instance.AttDB.PayDict.GetInfo(mS2C_OrderInfo.ProductID);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("PayDict.GetInfo error, ID = {0}", mS2C_OrderInfo.ProductID)
			});
			return;
		}
		NtOrderInfo ntOrderInfo = new NtOrderInfo();
		ntOrderInfo.productId = info.ProductID;
		ntOrderInfo.orderId = mS2C_OrderInfo.OrderID;
		ntOrderInfo.productCount = 1;
		ntOrderInfo.productCurrentPrice = info.Price;
		if (this.isAppStore)
		{
			ntOrderInfo.userName = this.Data.AccountID.ToString();
		}
		if (mS2C_OrderInfo.AccessToken != string.Empty && mS2C_OrderInfo.AccessToken != SdkU3d.getPropStr("SESSION"))
		{
			Globals.Instance.CliSession.RefreshToken(mS2C_OrderInfo.AccessToken);
		}
		if (SdkU3d.getPlatform() == "ad")
		{
			string channel = SdkU3d.getChannel();
			switch (channel)
			{
			case "360_assistant":
				ntOrderInfo.orderEtc = SdkU3d.getPropStr("SESSION");
				break;
			case "lenovo_open":
				ntOrderInfo.productId = info.LenovoPID.ToString();
				ntOrderInfo.orderEtc = Globals.Instance.Player.Data.AccountID.ToString();
				break;
			case "coolpad_sdk":
				ntOrderInfo.productId = info.CoolpadPID.ToString();
				break;
			case "nearme_vivo":
			{
				ntOrderInfo.orderDesc = mS2C_OrderInfo.OrderID;
				ntOrderInfo.orderEtc = mS2C_OrderInfo.Etc;
				int num2 = mS2C_OrderInfo.Etc.IndexOf("|");
				if (num2 > 0)
				{
					ntOrderInfo.orderId = mS2C_OrderInfo.Etc.Substring(0, num2);
				}
				break;
			}
			case "xiaomi_app":
				Globals.Instance.GameMgr.SetUserInfo(string.Empty);
				break;
			case "pps":
				Globals.Instance.GameMgr.SetUserInfo(string.Empty);
				SdkU3d.setUserInfo("USERINFO_HOSTID", "ppsmobile_s" + GameSetting.Data.ServerID.ToString());
				break;
			case "3k_sdk":
				Globals.Instance.GameMgr.SetUserInfo(string.Empty);
				SdkU3d.setPropInt("SERVER_ID", GameSetting.Data.ServerID);
				break;
			case "anzhi":
			case "sina_sdk":
				ntOrderInfo.orderDesc = info.Desc;
				break;
			case "scgames":
				ntOrderInfo.orderDesc = info.Name;
				break;
			case "gionee":
				ntOrderInfo.orderEtc = mS2C_OrderInfo.Etc;
				break;
			case "kuaifa":
				ntOrderInfo.productId = info.KuaiFaPID.ToString();
				ntOrderInfo.orderEtc = info.Desc;
				break;
			case "appchina":
				ntOrderInfo.productId = info.KuaiFaPID.ToString();
				break;
			case "yixin":
			{
				ntOrderInfo.productId = info.KuaiFaPID.ToString();
				int num3 = mS2C_OrderInfo.Etc.IndexOf(" ");
				if (num3 > 0)
				{
					ntOrderInfo.orderEtc = mS2C_OrderInfo.Etc.Substring(0, num3);
				}
				if (num3 + 1 < mS2C_OrderInfo.Etc.Length)
				{
					ntOrderInfo.sdkOrderId = mS2C_OrderInfo.Etc.Substring(num3 + 1, mS2C_OrderInfo.Etc.Length - num3 - 1);
				}
				break;
			}
			}
		}
		SdkU3d.ntCheckOrder(ntOrderInfo);
		GameAnalytics.OnChargeRequest(mS2C_OrderInfo.OrderID, info.ProductID, (double)info.Price, "CNY", (double)info.Diamond, SdkU3d.getPayChannel());
	}

	private void OnSDKLogin(int code, JsonData data)
	{
		if (code == 1)
		{
			return;
		}
		if (code == 0 || code == 11 || code == 12 || code == 10)
		{
			GameManager gameMgr = Globals.Instance.GameMgr;
			if (gameMgr.Status == GameManager.EGameStatus.EGS_Loading || gameMgr.Status == GameManager.EGameStatus.EGS_CreateChar || gameMgr.Status == GameManager.EGameStatus.EGS_Gaming)
			{
				Globals.Instance.GameMgr.ReturnLogin();
			}
		}
		if (code == 0)
		{
			if (SdkU3d.getChannel() == "youku" || SdkU3d.getChannel() == "dangle" || SdkU3d.getChannel() == "huawei" || SdkU3d.getChannel() == "uc_platform")
			{
				SdkU3d.ntSetFloatBtnVisible(true);
			}
			if (SdkU3d.getChannel() == "kuaifa")
			{
				SdkU3d.setUserInfo("USERINFO_DATATYPE", "1");
				SdkU3d.setUserInfo("USERINFO_HOSTID", GameSetting.Data.ServerID.ToString());
				SdkU3d.ntUpLoadUserInfo();
			}
			SdkU3d.setLoginFlag(true);
		}
	}

	private void OnSDKLogout(int code, JsonData data)
	{
		if (code == 0 || code == 3)
		{
			if (code == 3)
			{
				SdkU3d.setPropStr("SESSION", string.Empty);
			}
			GameManager gameMgr = Globals.Instance.GameMgr;
			if (gameMgr.Status == GameManager.EGameStatus.EGS_Loading || gameMgr.Status == GameManager.EGameStatus.EGS_CreateChar || gameMgr.Status == GameManager.EGameStatus.EGS_Gaming)
			{
				if (SdkU3d.getChannel() != "youku")
				{
					Globals.Instance.GameMgr.ReturnLogin();
				}
			}
			else if (SdkU3d.getChannel() == "dangle" || SdkU3d.getChannel() == "huawei")
			{
				SdkU3d.ntLogin();
			}
		}
	}

	private void OnSDKOrderCheck(int code, JsonData data)
	{
		try
		{
			NtOrderInfo ntOrderInfo = NtOrderInfo.FromJsonData(data);
			if (ntOrderInfo.orderStatus == OrderStatus.OS_SDK_CHECK_OK)
			{
				GameAnalytics.OnChargeSuccess(ntOrderInfo.orderId);
				if (SdkU3d.getChannel() == "kuaifa")
				{
					Globals.Instance.GameMgr.UpLoadUserInfo("3");
				}
			}
			if (this.isAppStore && ntOrderInfo.orderStatus != OrderStatus.OS_SDK_CHECK_OK && string.IsNullOrEmpty(ntOrderInfo.transactionReceipt))
			{
				SdkU3d.removeCheckedOrders(ntOrderInfo.orderId);
				if (!string.IsNullOrEmpty(ntOrderInfo.orderErrReason))
				{
					GameUIManager.mInstance.ShowMessageTip(ntOrderInfo.orderErrReason, 0f, 0f);
				}
			}
			else
			{
				MC2S_CheckPayResult mC2S_CheckPayResult = new MC2S_CheckPayResult();
				mC2S_CheckPayResult.OrderID = ntOrderInfo.orderId;
				mC2S_CheckPayResult.ReceiptData = ntOrderInfo.transactionReceipt;
				mC2S_CheckPayResult.OrderStatus = (int)ntOrderInfo.orderStatus;
				mC2S_CheckPayResult.Currency = ntOrderInfo.userPriceLocaleId;
				Globals.Instance.CliSession.Send(258, mC2S_CheckPayResult);
			}
		}
		catch (Exception ex)
		{
			GameUIManager.mInstance.ShowMessageTip(string.Format("Parse Error, {0}", ex.Message), 0f, 0f);
		}
	}

	private void OnSDKPlayerManagerNewEvent(int code, JsonData data)
	{
		if (!GameUIManager.mInstance.uiState.SDKPlayerManagerNew)
		{
			GameUIManager.mInstance.uiState.SDKPlayerManagerNew = true;
		}
	}

	public void OnMsgCheckPayResult(MemoryStream stream)
	{
		MS2C_CheckPayResult mS2C_CheckPayResult = Serializer.NonGeneric.Deserialize(typeof(MS2C_CheckPayResult), stream) as MS2C_CheckPayResult;
		if (mS2C_CheckPayResult.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_CheckPayResult.Result);
			return;
		}
	}

	public void OnMsgUpdatePay(MemoryStream stream)
	{
		MS2C_UpdatePay mS2C_UpdatePay = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdatePay), stream) as MS2C_UpdatePay;
		if (mS2C_UpdatePay.Result == 70)
		{
			string @string = Singleton<StringManager>.Instance.GetString("PayInvaild", new object[]
			{
				mS2C_UpdatePay.Value
			});
			GameUIManager.mInstance.ShowMessageTip(@string, 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ShowMessageTipByKey("PaySucess", 0f, 0f);
		if (this.pays == null)
		{
			this.pays = new List<PayData>();
		}
		for (int i = 0; i < this.pays.Count; i++)
		{
			if (this.pays[i].ID == mS2C_UpdatePay.Pay.ID)
			{
				this.pays[i].Count = mS2C_UpdatePay.Pay.Count;
				return;
			}
		}
		this.pays.Add(mS2C_UpdatePay.Pay);
	}

	public void OnMsgIAPCheckPayResult(MemoryStream stream)
	{
		MS2C_IAPCheckPayResult mS2C_IAPCheckPayResult = Serializer.NonGeneric.Deserialize(typeof(MS2C_IAPCheckPayResult), stream) as MS2C_IAPCheckPayResult;
		SdkU3d.removeCheckedOrders(mS2C_IAPCheckPayResult.OrderID);
	}

	public void OnMsgQueryRemotePlayer(MemoryStream stream)
	{
		MS2C_QueryRemotePlayer mS2C_QueryRemotePlayer = Serializer.NonGeneric.Deserialize(typeof(MS2C_QueryRemotePlayer), stream) as MS2C_QueryRemotePlayer;
		if (mS2C_QueryRemotePlayer.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_QueryRemotePlayer.Result);
			return;
		}
		this.TeamSystem.SetRemotePlayerData(mS2C_QueryRemotePlayer.Data1, mS2C_QueryRemotePlayer.Data2);
		if (this.RemotePlayerDataEvent != null)
		{
			this.RemotePlayerDataEvent();
		}
		GUITeamManageSceneV2 session = GameUIManager.mInstance.GetSession<GUITeamManageSceneV2>();
		if (session == null)
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = false;
			GameUIManager.mInstance.uiState.CombatPetSlot = 0;
			GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = false;
			session.IsLocalPlayer = false;
			GameUIManager.mInstance.uiState.CombatPetSlot = 0;
			session.SetUIState();
		}
	}

	public void OnMsgApplyCSAServerToken(MemoryStream stream)
	{
		MS2C_ApplyCSAServerToken mS2C_ApplyCSAServerToken = Serializer.NonGeneric.Deserialize(typeof(MS2C_ApplyCSAServerToken), stream) as MS2C_ApplyCSAServerToken;
		if (mS2C_ApplyCSAServerToken.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_ApplyCSAServerToken.Result);
			return;
		}
		GameUIManager.mInstance.ShowGUIWebViewPopUp("http://sq.gm.163.com/cgi-bin/csa/guide_csa.py?act=do_login_with_token&token=" + mS2C_ApplyCSAServerToken.Token + "&refer=/sprite.html?sq", Singleton<StringManager>.Instance.GetString("keFuZhuanQu"));
	}

	private void LoadMailCacheData()
	{
		if (this.Data == null)
		{
			return;
		}
		FileStream fileStream = null;
		try
		{
			string path = string.Format("{0}/m_{1}_{2}.data", Application.persistentDataPath, this.Data.ID, this.Data.CreateTime);
			if (File.Exists(path))
			{
				fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
				if (fileStream != null)
				{
					MS2C_GetMailData mS2C_GetMailData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetMailData), fileStream) as MS2C_GetMailData;
					this.Mails = mS2C_GetMailData.Data;
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning(new object[]
			{
				"Load MailCache failed, Exception:" + ex.Message
			});
		}
		if (fileStream != null)
		{
			fileStream.Close();
		}
		if (this.Mails == null)
		{
			this.Mails = new List<MailData>();
			if (this.Mails == null)
			{
				global::Debug.LogError(new object[]
				{
					"allocate List<MailData> error"
				});
			}
		}
	}

	private void SaveMailCacheData()
	{
		if (this.Data == null)
		{
			return;
		}
		FileStream fileStream = null;
		try
		{
			string path = string.Format("{0}/m_{1}_{2}.data", Application.persistentDataPath, this.Data.ID, this.Data.CreateTime);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			if (fileStream != null)
			{
				MS2C_GetMailData mS2C_GetMailData = new MS2C_GetMailData();
				for (int i = 0; i < this.Mails.Count; i++)
				{
					if (this.Mails[i].AffixType.Count != 0 || this.GetTimeStamp() <= this.Mails[i].TimeStamp + 259200)
					{
						mS2C_GetMailData.Data.Add(this.Mails[i]);
					}
				}
				Serializer.NonGeneric.Serialize(fileStream, mS2C_GetMailData);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Save MailCache failed, Exception:" + ex.Message
			});
		}
		if (fileStream != null)
		{
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public bool HasNewMail()
	{
		return this.MailVersion > this.MaxMailID;
	}

	public bool HasUnreadMail()
	{
		if (this.HasNewMail())
		{
			return true;
		}
		for (int i = 0; i < this.Mails.Count; i++)
		{
			if (!this.Mails[i].Read)
			{
				return true;
			}
			if (this.Mails[i].AffixType.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void SetMailRead(MailData mail)
	{
		if (mail == null || mail.Read)
		{
			return;
		}
		mail.Read = true;
		this.mailUpdate = true;
	}

	private static bool IsDeletedMail(MailData data)
	{
		return data.MailID == 0u;
	}

	public void PushGuildMessage(ChatMessage chatMsg)
	{
		if (this.WorldMsgs.Count >= 50)
		{
			this.WorldMsgs.RemoveAt(0);
		}
		WorldMessage worldMessage = new WorldMessage();
		worldMessage.Msg = chatMsg;
		this.WorldMsgs.Add(new WorldMessageExtend(worldMessage, true));
		if (!this.ShowChatWorldNewMark && chatMsg.PlayerID != this.Data.ID && chatMsg.TimeStamp > GameCache.Data.HasReadedWorldMsgTimeStamp)
		{
			this.ShowChatWorldNewMark = true;
		}
		if (this.OldWorldMessageEvent != null)
		{
			this.OldWorldMessageEvent(new WorldMessageExtend(worldMessage, false));
		}
	}

	private string RemoveEmotion(string text)
	{
		int length = text.Length;
		for (int i = 0; i < length; i++)
		{
			if (text[i] == '<' && i + 3 < length && text[i + 3] == '>')
			{
				string s = text.Substring(i + 1, 2);
				int num = 0;
				if (int.TryParse(s, out num) && num < 49 && num > 0)
				{
					text = text.Remove(i, 4);
					return this.RemoveEmotion(text);
				}
			}
		}
		return text;
	}

	private bool IsAllEmotion(ChatMessage msg)
	{
		string value = this.RemoveEmotion(msg.Message);
		return string.IsNullOrEmpty(value);
	}

	public void SetWorldReadedMsgTimestamp()
	{
		for (int i = 0; i < this.WorldMsgs.Count; i++)
		{
			WorldMessageExtend worldMessageExtend = this.WorldMsgs[i];
			if (worldMessageExtend != null)
			{
				if (worldMessageExtend.mWM != null)
				{
					if (worldMessageExtend.mWM.Msg != null)
					{
						if (worldMessageExtend.mWM.Msg.TimeStamp > GameCache.Data.HasReadedWorldMsgTimeStamp)
						{
							GameCache.Data.HasReadedWorldMsgTimeStamp = worldMessageExtend.mWM.Msg.TimeStamp;
						}
					}
				}
			}
		}
		GameCache.UpdateNow = true;
	}

	public void SetGuildReadedMsgTimestamp()
	{
		for (int i = 0; i < this.GuildMsgs.Count; i++)
		{
			ChatMessage chatMessage = this.GuildMsgs[i];
			if (chatMessage != null)
			{
				if (chatMessage.TimeStamp > GameCache.Data.HasReadedGuildMsgTimeStamp)
				{
					GameCache.Data.HasReadedGuildMsgTimeStamp = chatMessage.TimeStamp;
				}
			}
		}
		GameCache.UpdateNow = true;
	}

	public void SetSiLiaoReadedMsgTimestamp()
	{
		for (int i = 0; i < this.WhisperMsgs.Count; i++)
		{
			ChatMessage chatMessage = this.WhisperMsgs[i];
			if (chatMessage != null)
			{
				if (chatMessage.TimeStamp > GameCache.Data.HasReadedSiLiaoMsgTimeStamp)
				{
					GameCache.Data.HasReadedSiLiaoMsgTimeStamp = chatMessage.TimeStamp;
				}
			}
		}
		GameCache.UpdateNow = true;
	}

	public void SetWuHuiReadedMsgTimestamp()
	{
		for (int i = 0; i < this.CostumePartyMsgs.Count; i++)
		{
			ChatMessage chatMessage = this.CostumePartyMsgs[i];
			if (chatMessage != null)
			{
				if (chatMessage.TimeStamp > GameCache.Data.HasReadedWuHuiMsgTimeStamp)
				{
					GameCache.Data.HasReadedWuHuiMsgTimeStamp = chatMessage.TimeStamp;
				}
			}
		}
		GameCache.UpdateNow = true;
	}

	private void PushChatMessage(ChatMessage chatMsg)
	{
		switch (chatMsg.Channel)
		{
		case 0:
		{
			if (this.WorldMsgs.Count >= 50)
			{
				this.WorldMsgs.RemoveAt(0);
			}
			WorldMessage worldMessage = new WorldMessage();
			worldMessage.Msg = chatMsg;
			this.WorldMsgs.Add(new WorldMessageExtend(worldMessage, false));
			if (!this.ShowChatWorldNewMark && chatMsg.PlayerID != this.Data.ID && chatMsg.TimeStamp > GameCache.Data.HasReadedWorldMsgTimeStamp)
			{
				this.ShowChatWorldNewMark = true;
			}
			if (this.WorldMessageEvent != null)
			{
				this.WorldMessageEvent(worldMessage);
			}
			break;
		}
		case 1:
			if (this.GuildMsgs.Count >= 50)
			{
				this.GuildMsgs.RemoveAt(0);
			}
			this.GuildMsgs.Add(chatMsg);
			if (this.GuildSystem != null && this.GuildSystem.HasGuild() && !this.ShowChatGuildNewMark && chatMsg.PlayerID != this.Data.ID && chatMsg.TimeStamp > GameCache.Data.HasReadedGuildMsgTimeStamp)
			{
				this.ShowChatGuildNewMark = true;
				if (!chatMsg.Voice && !this.IsAllEmotion(chatMsg))
				{
					this.ShowGuildWarNewMark = true;
				}
			}
			if (this.ChatMessageEvent != null)
			{
				this.ChatMessageEvent(chatMsg);
			}
			break;
		case 2:
			if (this.WhisperMsgs.Count >= 50)
			{
				this.WhisperMsgs.RemoveAt(0);
			}
			this.WhisperMsgs.Add(chatMsg);
			if (!this.ShowChatWisperNewMark && chatMsg.PlayerID != this.Data.ID && chatMsg.TimeStamp > GameCache.Data.HasReadedSiLiaoMsgTimeStamp)
			{
				this.ShowChatWisperNewMark = true;
			}
			if (this.ChatMessageEvent != null)
			{
				this.ChatMessageEvent(chatMsg);
			}
			break;
		case 3:
			if (this.CostumePartyMsgs.Count >= 50)
			{
				this.CostumePartyMsgs.RemoveAt(0);
			}
			this.CostumePartyMsgs.Add(chatMsg);
			if (!this.ShowChatPartyNewMark && chatMsg.PlayerID != this.Data.ID && (!GameSetting.Data.ShieldPartyInvite || chatMsg.Type != 1u) && (!GameSetting.Data.ShieldPartyInteraction || chatMsg.Type != 2u) && chatMsg.TimeStamp > GameCache.Data.HasReadedWuHuiMsgTimeStamp)
			{
				this.ShowChatPartyNewMark = true;
			}
			if (this.ChatMessageEvent != null)
			{
				this.ChatMessageEvent(chatMsg);
			}
			break;
		default:
			global::Debug.LogError(new object[]
			{
				"chat message channel error!"
			});
			break;
		}
	}

	public bool IsLevelRewardTaken(int index)
	{
		if (index < 0 || index >= 32)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("LevelReward overflow, index = {0}", index)
			});
			return false;
		}
		return (this.Data.LevelReward & 1 << index) != 0;
	}

	public bool IsVipWeekRewardBuy(VipLevelInfo info)
	{
		if (info == null)
		{
			return false;
		}
		if (info.ID > 16)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("VipReward overflow, vipLevel = {0}", info.ID)
			});
			return false;
		}
		return (this.Data.VipWeekReward & 1 << info.ID) != 0;
	}

	public bool IsPayVipRewardTaken(VipLevelInfo info)
	{
		if (info == null)
		{
			return false;
		}
		if (info.ID > 16)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("VipReward overflow, vipLevel = {0}", info.ID)
			});
			return false;
		}
		return (this.Data.VipPayReward & 1 << info.ID) != 0;
	}

	public bool IsCardExpire()
	{
		return this.GetTimeStamp() > this.Data.CardTimeStamp;
	}

	public int GetCardRemainDays()
	{
		int num = this.Data.CardTimeStamp - this.GetTimeStamp();
		if (num <= 0)
		{
			return 0;
		}
		num /= 86400;
		if (this.IsTodayCardDiamondTaken())
		{
			return num;
		}
		return num + 1;
	}

	public bool IsRenew()
	{
		if (this.Data.CardRenew != 0)
		{
			return false;
		}
		int num = this.Data.CardTimeStamp - this.GetTimeStamp();
		return num > 0 && num < 259200;
	}

	public bool IsTodayCardDiamondTaken()
	{
		int i = this.Data.CardTimeStamp - this.GetTimeStamp();
		if (i <= 0)
		{
			return false;
		}
		for (i /= 86400; i > 30; i -= 30)
		{
		}
		i = 30 - i;
		if (i <= 0)
		{
			i = 1;
		}
		return (this.Data.CardFlag & 1 << i) != 0;
	}

	public bool IsBuySuperCard()
	{
		return 0 != this.Data.SuperCardTimeStamp;
	}

	public bool IsTodaySuperCardDiamondTaken()
	{
		return this.Data.SuperCardTimeStamp > this.GetTimeStamp();
	}

	public BuyRecord GetBuyRecord(int id)
	{
		BuyRecord result = null;
		this.buyRecords.TryGetValue(id, out result);
		return result;
	}

	public void UpdateBuyRecord(int id, int count, int timeStamp, uint dataVersion)
	{
		BuyRecord buyRecord = this.GetBuyRecord(id);
		if (buyRecord == null)
		{
			buyRecord = new BuyRecord();
			buyRecord.ID = id;
			buyRecord.Count = count;
			buyRecord.TimeStamp = timeStamp;
			this.buyRecords.Add(buyRecord.ID, buyRecord);
		}
		else
		{
			buyRecord.Count = count;
			buyRecord.TimeStamp = timeStamp;
		}
		this.BuyRecordVersion = dataVersion;
	}

	public int GetBuyCount(ShopInfo shopInfo)
	{
		BuyRecord buyRecord = this.GetBuyRecord(shopInfo.ID);
		if (buyRecord == null)
		{
			return 0;
		}
		if (shopInfo.ResetType == 1)
		{
			return buyRecord.Count;
		}
		if (this.GetTimeStamp() >= buyRecord.TimeStamp)
		{
			return 0;
		}
		return buyRecord.Count;
	}

	public ShopItemData GetPetShopItem(int id)
	{
		if (this.PetShopData == null)
		{
			return null;
		}
		for (int i = 0; i < this.PetShopData.Count; i++)
		{
			if (this.PetShopData[i].ID == id)
			{
				return this.PetShopData[i];
			}
		}
		return null;
	}

	public ShopItemData GetAwakeShopItem(int id)
	{
		if (this.AwakeShopData == null)
		{
			return null;
		}
		for (int i = 0; i < this.AwakeShopData.Count; i++)
		{
			if (this.AwakeShopData[i].ID == id)
			{
				return this.AwakeShopData[i];
			}
		}
		return null;
	}

	public ShopItemData GetLopetShopItem(int id)
	{
		if (this.LopetShopData == null)
		{
			return null;
		}
		for (int i = 0; i < this.LopetShopData.Count; i++)
		{
			if (this.LopetShopData[i].ID == id)
			{
				return this.LopetShopData[i];
			}
		}
		return null;
	}

	public ShopItemData GetShopItem(int shopType, int id)
	{
		if (shopType == 0)
		{
			return this.GetPetShopItem(id);
		}
		if (shopType == 1)
		{
			return this.GetAwakeShopItem(id);
		}
		if (shopType == 9)
		{
			return this.GetLopetShopItem(id);
		}
		return null;
	}

	public float GetShopActivityDiscount(int shopType)
	{
		return 0f;
	}

	public uint GetShopRefreshCount(int shopType)
	{
		if (shopType == 0)
		{
			return this.Data.Common2ShopRefresh;
		}
		if (shopType == 1)
		{
			return this.Data.AwakenShopRefresh;
		}
		if (shopType == 9)
		{
			return (uint)this.Data.LopetShopRefresh;
		}
		return 0u;
	}

	public bool IsFirstPayCompleted()
	{
		return (this.Data.DataFlag & 4194304) != 0;
	}

	public bool IsFirstPayRewardTaken()
	{
		return (this.Data.DataFlag & 4) != 0;
	}

	public bool IsDay7RewardTaken(int index)
	{
		if (index <= 0 || index > 7)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Day7Reward overflow, index = {0}", index)
			});
			return false;
		}
		return (this.Data.Day7Flag & 1 << index) != 0;
	}

	public void ReturnLogin()
	{
		this.OnDestroy();
	}

	public int GetQuality()
	{
		return LocalPlayer.GetQuality(this.Data.ConstellationLevel);
	}

	public static int GetQuality(int ConstellationLevel)
	{
		if (ConstellationLevel < 10)
		{
			return 1;
		}
		if (ConstellationLevel < 30)
		{
			return 2;
		}
		return 3;
	}

	public string GetPlayerIcon()
	{
		string result = string.Empty;
		PetDataEx pet = this.TeamSystem.GetPet(0);
		if (pet != null)
		{
			result = pet.Info.Icon;
		}
		return result;
	}

	public VipLevelInfo GetVipLevelInfo()
	{
		if (this.Data.VipLevel == 0u)
		{
			return Globals.Instance.AttDB.VipLevelDict.GetInfo(16);
		}
		return Globals.Instance.AttDB.VipLevelDict.GetInfo((int)this.Data.VipLevel);
	}

	public VipLevelInfo GetVipLevelInfo(int vipLevel)
	{
		if (vipLevel == 0)
		{
			return Globals.Instance.AttDB.VipLevelDict.GetInfo(16);
		}
		return Globals.Instance.AttDB.VipLevelDict.GetInfo(vipLevel);
	}

	public bool CanSignIn()
	{
		return Globals.Instance.Player.GetTimeStamp() >= Globals.Instance.Player.Data.SignInTimeStamp;
	}

	public int GetPayCount(int id)
	{
		if (this.pays == null)
		{
			return 0;
		}
		for (int i = 0; i < this.pays.Count; i++)
		{
			if (this.pays[i].ID == id)
			{
				return this.pays[i].Count;
			}
		}
		return 0;
	}

	private void OnMsgRecycleChat(MemoryStream stream)
	{
		MS2C_RecycleChat mS2C_RecycleChat = Serializer.NonGeneric.Deserialize(typeof(MS2C_RecycleChat), stream) as MS2C_RecycleChat;
		for (int i = this.WorldMsgs.Count - 1; i >= 0; i--)
		{
			if (this.WorldMsgs[i].mWM.Msg.PlayerID == mS2C_RecycleChat.PlayerID)
			{
				this.WorldMsgs.RemoveAt(i);
			}
		}
		for (int j = this.GuildMsgs.Count - 1; j >= 0; j--)
		{
			if (this.GuildMsgs[j].PlayerID == mS2C_RecycleChat.PlayerID)
			{
				this.GuildMsgs.RemoveAt(j);
			}
		}
		for (int k = this.WhisperMsgs.Count - 1; k >= 0; k--)
		{
			if (this.WhisperMsgs[k].PlayerID == mS2C_RecycleChat.PlayerID)
			{
				this.WhisperMsgs.RemoveAt(k);
			}
		}
		for (int l = this.CostumePartyMsgs.Count - 1; l >= 0; l--)
		{
			if (this.CostumePartyMsgs[l].PlayerID == mS2C_RecycleChat.PlayerID)
			{
				this.CostumePartyMsgs.RemoveAt(l);
			}
		}
		if (this.RecyleMessageEvent != null)
		{
			this.RecyleMessageEvent();
		}
	}

	private QuestInfo GetNextQuest(int sceneID)
	{
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("SceneDict.GetInfo error, id = {0}", new object[]
			{
				sceneID
			});
			return null;
		}
		while (info != null)
		{
			QuestInfo info2 = Globals.Instance.AttDB.QuestDict.GetInfo(info.ID);
			if (info2 != null)
			{
				int questState = this.GetQuestState(info2.ID);
				if (questState != 2)
				{
					return info2;
				}
			}
			info = Globals.Instance.AttDB.SceneDict.GetInfo(info.NextID);
		}
		return null;
	}

	private void InitQuest()
	{
		this.MainQuest = this.GetNextQuest(GameConst.GetInt32(107));
		this.BranchQuest = this.GetNextQuest(GameConst.GetInt32(108));
	}

	public bool HasQuestReward()
	{
		return (this.MainQuest != null && this.GetQuestState(this.MainQuest.ID) == 1) || (this.BranchQuest != null && this.GetQuestState(this.BranchQuest.ID) == 1);
	}

	public int GetCurFashionID()
	{
		SocketDataEx socket = this.TeamSystem.GetSocket(0);
		if (socket == null)
		{
			return 0;
		}
		return socket.GetFashionID();
	}

	private void OnMsgTrialStart(MemoryStream stream)
	{
		MS2C_TrialStart mS2C_TrialStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrialStart), stream) as MS2C_TrialStart;
		if (mS2C_TrialStart.Result == 51)
		{
			this.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TrialStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_TrialStart.Result);
			return;
		}
		GameUIManager.mInstance.uiState.TrailCurLvl = Globals.Instance.Player.Data.TrialWave;
		GameUIManager.mInstance.uiState.TrailMaxRecord = Globals.Instance.Player.Data.TrialMaxWave;
		Globals.Instance.ActorMgr.SetServerData(mS2C_TrialStart.Key, mS2C_TrialStart.Data);
		GameUIManager.mInstance.LoadScene(GameConst.GetInt32(104));
	}

	private void OnMsgTakeQuestReward(MemoryStream stream)
	{
		MS2C_TakeQuestReward mS2C_TakeQuestReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeQuestReward), stream) as MS2C_TakeQuestReward;
		if (mS2C_TakeQuestReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeQuestReward.Result);
			return;
		}
		SceneData sceneData = this.GetSceneData(mS2C_TakeQuestReward.QuestID);
		if (sceneData == null)
		{
			global::Debug.LogErrorFormat("GetSceneData error, id = {0}", new object[]
			{
				mS2C_TakeQuestReward.QuestID
			});
			return;
		}
		sceneData.QuestReward = true;
		this.SceneVersion = mS2C_TakeQuestReward.SceneVersion;
		if (this.MainQuest != null && this.MainQuest.ID == mS2C_TakeQuestReward.QuestID)
		{
			this.MainQuest = this.GetNextQuest(mS2C_TakeQuestReward.QuestID);
		}
		if (this.BranchQuest != null && this.BranchQuest.ID == mS2C_TakeQuestReward.QuestID)
		{
			this.BranchQuest = this.GetNextQuest(mS2C_TakeQuestReward.QuestID);
		}
		if (this.QuestTakeRewardEvent != null)
		{
			this.QuestTakeRewardEvent(mS2C_TakeQuestReward.QuestID);
		}
	}

	private void OnMsgUseItem(MemoryStream stream)
	{
		MS2C_UseItem mS2C_UseItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_UseItem), stream) as MS2C_UseItem;
		if (mS2C_UseItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_UseItem.Result);
			return;
		}
		if (this.UseItemEvent != null)
		{
			this.UseItemEvent(mS2C_UseItem.ItemInfoID, mS2C_UseItem.Value);
		}
	}

	public void OnMsgPveStart(MemoryStream stream)
	{
		MS2C_PveStart mS2C_PveStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_PveStart), stream) as MS2C_PveStart;
		if (mS2C_PveStart.Result == 51)
		{
			this.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_PveStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_PveStart.Result);
			return;
		}
		Globals.Instance.ActorMgr.SetServerData(mS2C_PveStart.Key, mS2C_PveStart.Data);
		GameUIManager.mInstance.LoadScene(mS2C_PveStart.SceneID);
		GameAnalytics.OnStartScene(Globals.Instance.AttDB.SceneDict.GetInfo(mS2C_PveStart.SceneID));
	}

	public void OnMsgPveResult(MemoryStream stream)
	{
		MS2C_PveResult mS2C_PveResult = Serializer.NonGeneric.Deserialize(typeof(MS2C_PveResult), stream) as MS2C_PveResult;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.CurSceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		if (mS2C_PveResult.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_PveResult.Result);
			GameAnalytics.OnFailScene(uiState.CurSceneInfo, GameAnalytics.ESceneFailed.ServerFaild);
			Globals.Instance.SenceMgr.CloseScene();
			GameUIManager.mInstance.ChangeSession<GUIGameResultFailureScene>(null, false, false);
			return;
		}
		uiState.IsPvp = false;
		uiState.PveResult = mS2C_PveResult;
		GameAnalytics.OnFinishScene(uiState.CurSceneInfo);
		GameUIManager.mInstance.ChangeSession<GUIGameResultReadyScene>(null, false, false);
	}

	public void OnMsgComment(MemoryStream stream)
	{
		MS2C_CommentMsg commentData = Serializer.NonGeneric.Deserialize(typeof(MS2C_CommentMsg), stream) as MS2C_CommentMsg;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.CommentData = commentData;
	}

	public void OnMsgFrozenPlay(MemoryStream stream)
	{
		MS2C_UpdateFrozenPlay mS2C_UpdateFrozenPlay = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateFrozenPlay), stream) as MS2C_UpdateFrozenPlay;
		this.frozenFunction = mS2C_UpdateFrozenPlay.Play;
		this.frozenTimestamp = mS2C_UpdateFrozenPlay.Timestamp;
	}

	public void ShowFrozenFunctionMsg()
	{
		int num = this.frozenTimestamp - this.GetTimeStamp();
		if (num <= 0)
		{
			num = 1;
		}
		string @string = Singleton<StringManager>.Instance.GetString(string.Format("FrozenF_{0}", this.frozenFunction), new object[]
		{
			num
		});
		GameUIManager.mInstance.ShowMessageTip(@string, 0f, 0f);
	}

	private void OnPlayerLevelup()
	{
		string channel = SdkU3d.getChannel();
		string text = channel;
		switch (text)
		{
		case "ljsdk":
			Globals.Instance.GameMgr.UpLoadUserInfo("3");
			break;
		case "kuaifa":
			Globals.Instance.GameMgr.UpLoadUserInfo("4");
			break;
		case "caohua":
			Globals.Instance.GameMgr.UpLoadUserInfo(string.Empty);
			break;
		}
	}

	public int GetMaxEnergy()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo((int)this.Data.Level);
		if (info == null)
		{
			return 0;
		}
		VipLevelInfo vipLevelInfo = this.GetVipLevelInfo();
		if (vipLevelInfo == null)
		{
			return 0;
		}
		return (int)(info.MaxEnergy + (uint)vipLevelInfo.MaxEnergy);
	}

	public int GetMaxStamina()
	{
		int @int = GameConst.GetInt32(35);
		VipLevelInfo vipLevelInfo = this.GetVipLevelInfo();
		if (vipLevelInfo == null)
		{
			return @int;
		}
		return @int + vipLevelInfo.MaxStamina;
	}

	private void QuitGame()
	{
		SdkU3d.setPropStr("SESSION", string.Empty);
		SdkU3d.ntLogout();
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		Globals.Instance.GameMgr.ReturnLogin();
	}

	private void OnSysMessageEvent(WorldMessage worldMsg)
	{
		if (worldMsg.SysEvent != null)
		{
			if (worldMsg.SysEvent.EventType == 1)
			{
				List<string> list = new List<string>();
				string item = this.mSb.Remove(0, this.mSb.Length).Append("[ce9f00]").Append(Singleton<StringManager>.Instance.GetString("chatTxt15")).Append("[-]").ToString();
				list.Add(item);
				list.Add(item);
				list.Add(item);
				GameUIManager.mInstance.ShowGameNewsMsg(list, worldMsg.SysEvent.Priority, 110, true);
				base.Invoke("QuitGame", 35f);
			}
			else if (worldMsg.SysEvent.EventType == 5)
			{
				string @string = Singleton<StringManager>.Instance.GetString("chatTxt18", new object[]
				{
					worldMsg.SysEvent.StrValue,
					worldMsg.SysEvent.IntValue[0]
				});
				GameUIManager.mInstance.ShowGameNew(@string, worldMsg.SysEvent.Priority, 110, true);
			}
			else if (worldMsg.SysEvent.EventType == 4)
			{
				string string2 = Singleton<StringManager>.Instance.GetString("chatTxt19", new object[]
				{
					worldMsg.SysEvent.StrValue,
					worldMsg.SysEvent.IntValue[0]
				});
				GameUIManager.mInstance.ShowGameNew(string2, worldMsg.SysEvent.Priority, 110, true);
			}
			else if (worldMsg.SysEvent.EventType == 2)
			{
				PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info != null)
				{
					string itemQualityColorHex = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[2]));
					string itemQualityColorHex2 = Tools.GetItemQualityColorHex(info.Quality);
					string string3 = Singleton<StringManager>.Instance.GetString("chatTxt21", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex2,
						Tools.GetPetName(info),
						worldMsg.SysEvent.IntValue[1],
						itemQualityColorHex
					});
					GameUIManager.mInstance.ShowGameNew(string3, worldMsg.SysEvent.Priority, 110, true);
				}
			}
			else if (worldMsg.SysEvent.EventType == 3)
			{
				PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info2 != null)
				{
					string itemQualityColorHex3 = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[1]));
					string itemQualityColorHex4 = Tools.GetItemQualityColorHex(info2.Quality);
					string string4 = Singleton<StringManager>.Instance.GetString("chatTxt20", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex4,
						Tools.GetPetName(info2),
						itemQualityColorHex3
					});
					if (!worldMsg.SysEvent.StrValue.Equals(Globals.Instance.Player.Data.Name))
					{
						GameUIManager.mInstance.ShowGameNew(string4, worldMsg.SysEvent.Priority, 110, true);
					}
				}
			}
			else if (worldMsg.SysEvent.EventType == 7)
			{
				PetInfo info3 = Globals.Instance.AttDB.PetDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info3 != null)
				{
					string itemQualityColorHex5 = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[1]));
					string itemQualityColorHex6 = Tools.GetItemQualityColorHex(info3.Quality);
					string string5 = Singleton<StringManager>.Instance.GetString("chatTxt23", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex6,
						Tools.GetPetName(info3),
						itemQualityColorHex5
					});
					if (!worldMsg.SysEvent.StrValue.Equals(Globals.Instance.Player.Data.Name))
					{
						GameUIManager.mInstance.ShowGameNew(string5, worldMsg.SysEvent.Priority, 110, true);
					}
				}
			}
			else if (worldMsg.SysEvent.EventType == 8)
			{
				PetInfo info4 = Globals.Instance.AttDB.PetDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info4 != null)
				{
					string itemQualityColorHex7 = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[1]));
					string itemQualityColorHex8 = Tools.GetItemQualityColorHex(info4.Quality);
					string string6 = Singleton<StringManager>.Instance.GetString("chatTxt22", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex8,
						Tools.GetPetName(info4),
						itemQualityColorHex7
					});
					if (!worldMsg.SysEvent.StrValue.Equals(Globals.Instance.Player.Data.Name))
					{
						GameUIManager.mInstance.ShowGameNew(string6, worldMsg.SysEvent.Priority, 110, true);
					}
				}
			}
			else if (worldMsg.SysEvent.EventType == 11)
			{
				LopetInfo info5 = Globals.Instance.AttDB.LopetDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info5 != null)
				{
					string itemQualityColorHex9 = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[1]));
					string itemQualityColorHex10 = Tools.GetItemQualityColorHex(info5.Quality);
					string string7 = Singleton<StringManager>.Instance.GetString("chatTxt37", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex10,
						info5.Name,
						itemQualityColorHex9
					});
					GameUIManager.mInstance.ShowGameNew(string7, worldMsg.SysEvent.Priority, 110, true);
				}
			}
			else if (worldMsg.SysEvent.EventType == 12)
			{
				LopetInfo info6 = Globals.Instance.AttDB.LopetDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info6 != null)
				{
					string itemQualityColorHex11 = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[2]));
					string itemQualityColorHex12 = Tools.GetItemQualityColorHex(info6.Quality);
					string string8 = Singleton<StringManager>.Instance.GetString("chatTxt38", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex12,
						info6.Name,
						worldMsg.SysEvent.IntValue[1],
						itemQualityColorHex11
					});
					GameUIManager.mInstance.ShowGameNew(string8, worldMsg.SysEvent.Priority, 110, true);
				}
			}
			else if (worldMsg.SysEvent.EventType == 6)
			{
				if (worldMsg.SysEvent.IntValue[0] == 0)
				{
					string string9 = Singleton<StringManager>.Instance.GetString("worldBossTxt23");
					GameUIManager.mInstance.ShowGameNew(string9, worldMsg.SysEvent.Priority, 110, true);
				}
				else
				{
					MonsterInfo info7 = Globals.Instance.AttDB.MonsterDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
					if (info7 != null)
					{
						string string10 = Singleton<StringManager>.Instance.GetString("worldBossTxt24", new object[]
						{
							info7.Name
						});
						GameUIManager.mInstance.ShowGameNew(string10, worldMsg.SysEvent.Priority, 110, true);
					}
				}
			}
			else if (worldMsg.SysEvent.EventType == 9)
			{
				ItemInfo info8 = Globals.Instance.AttDB.ItemDict.GetInfo(worldMsg.SysEvent.IntValue[0]);
				if (info8 != null)
				{
					string itemQualityColorHex13 = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(worldMsg.SysEvent.IntValue[1]));
					string itemQualityColorHex14 = Tools.GetItemQualityColorHex(info8.Quality);
					string string11 = Singleton<StringManager>.Instance.GetString("chatTxt29", new object[]
					{
						worldMsg.SysEvent.StrValue,
						itemQualityColorHex14,
						info8.Name,
						itemQualityColorHex13
					});
					GameUIManager.mInstance.ShowGameNew(string11, worldMsg.SysEvent.Priority, 110, true);
				}
			}
		}
	}

	public bool IsFunctionEnable(int flag)
	{
		return (this.setting & flag) != 0;
	}

	public int GetCurSceneID()
	{
		int num;
		if (this.curSceneID == 0)
		{
			num = GameConst.GetInt32(107);
		}
		else
		{
			num = this.curSceneID;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(num);
		if (info == null)
		{
			global::Debug.LogErrorFormat("SceneDict.GetInfo error, id = {0}", new object[]
			{
				num
			});
			return this.curSceneID;
		}
		while (info != null)
		{
			SceneData sceneData = this.GetSceneData(num);
			if (sceneData == null)
			{
				break;
			}
			this.curSceneID = num;
			num = info.NextID;
			info = Globals.Instance.AttDB.SceneDict.GetInfo(num);
		}
		return this.curSceneID;
	}

	private bool IsVoiceChatMessagePlayed(ChatMessage ctMessage)
	{
		this.mVoiceChatData.FromJsonStr(ctMessage.Message);
		return Globals.Instance.VoiceMgr.IsAlreadyPlayed(this.mVoiceChatData.VoiceTranslateParam);
	}

	[DebuggerHidden]
	private IEnumerator CheckVoiceAutoPlayed()
	{
        return null;
        //LocalPlayer.<CheckVoiceAutoPlayed>c__Iterator11 <CheckVoiceAutoPlayed>c__Iterator = new LocalPlayer.<CheckVoiceAutoPlayed>c__Iterator11();
        //<CheckVoiceAutoPlayed>c__Iterator.<>f__this = this;
        //return <CheckVoiceAutoPlayed>c__Iterator;
	}
}
