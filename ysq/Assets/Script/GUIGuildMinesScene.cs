using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIGuildMinesScene : GameUISession
{
	private class Target : MonoBehaviour
	{
		private UITexture mTex;

		private UILabel mName;

		private UILabel mCombatValue;

		private UILabel mMines;

		private UILabel mGuild;

		private UISprite mAvatarIcon;

		private UISprite mQualityMask;

		private ulong playerID;

		private int amount;

		private int combatValue;

		public void Init()
		{
			this.mTex = base.GetComponent<UITexture>();
			this.mName = GameUITools.FindUILabel("Name", base.gameObject);
			this.mCombatValue = GameUITools.FindUILabel("CombatValue", base.gameObject);
			this.mMines = GameUITools.FindUILabel("Mines", base.gameObject);
			this.mGuild = GameUITools.FindUILabel("Guild", base.gameObject);
			this.mAvatarIcon = GameUITools.FindUISprite("Icon/AvatarIcon", base.gameObject);
			this.mQualityMask = GameUITools.FindUISprite("Icon/QualityMark", base.gameObject);
		}

		public void Refresh(OrePillageTarget data)
		{
			this.playerID = data.GUID;
			this.amount = data.Amount;
			this.combatValue = data.CombatValue;
			this.mName.text = Tools.GetItemQualityColorHex(LocalPlayer.GetQuality(data.ConLevel)) + data.Name;
			this.mCombatValue.text = Singleton<StringManager>.Instance.GetString("guildMines26") + data.CombatValue;
			this.mMines.text = Singleton<StringManager>.Instance.GetString("guildMines17", new object[]
			{
				data.Amount
			});
			if (string.IsNullOrEmpty(data.GuildName))
			{
				this.mGuild.text = Tools.GetColorHex(Color.gray, Singleton<StringManager>.Instance.GetString("chatTxt10"));
			}
			else
			{
				this.mGuild.text = Tools.GetDefaultColorHex(data.GuildName);
			}
			this.mAvatarIcon.spriteName = Tools.GetPlayerIcon(data.FashionID);
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(data.ConLevel));
			int num = 0;
			foreach (OreInfo current in Globals.Instance.AttDB.OreDict.Values)
			{
				if (current.Section <= 0)
				{
					break;
				}
				if ((float)data.Amount <= (float)current.Section / 100f * (float)Globals.Instance.Player.GuildSystem.GuildMines.LostOre)
				{
					break;
				}
				num = current.ID;
			}
			this.mTex.mainTexture = Res.Load<Texture>(string.Format("Guild/Mines{0}", num), false);
		}

		private void OnClick()
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			if (!Globals.Instance.Player.GuildSystem.HasGuild())
			{
				GameUIManager.mInstance.ShowMessageTipByKey("EGR_36", 0f, 0f);
				return;
			}
			if (Globals.Instance.Player.GuildSystem.GuildMines.PillageCount <= 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guildMines15", 0f, 0f);
				return;
			}
			if ((float)Globals.Instance.Player.TeamSystem.GetCombatValue() / (float)this.combatValue > 1.5f)
			{
				GUIPassCombatPopUp.Show(new GUIPassCombatPopUp.VoidCallback(this.SendFarmMsg), new GUIPassCombatPopUp.VoidCallback(this.SendPillageMsg));
				return;
			}
			this.SendPillageMsg();
		}

		private void SendFarmMsg()
		{
			MC2S_OrepillageFarm mC2S_OrepillageFarm = new MC2S_OrepillageFarm();
			mC2S_OrepillageFarm.TargetID = this.playerID;
			mC2S_OrepillageFarm.Amount = this.amount;
			Globals.Instance.CliSession.Send(1041, mC2S_OrepillageFarm);
			GameUIState uiState = GameUIManager.mInstance.uiState;
			LocalPlayer player = Globals.Instance.Player;
			uiState.PlayerLevel = player.Data.Level;
			uiState.PlayerEnergy = player.Data.Energy;
			uiState.PlayerExp = player.Data.Exp;
			uiState.PlayerMoney = player.Data.Money;
		}

		private void SendPillageMsg()
		{
			MC2S_OrePillageStart mC2S_OrePillageStart = new MC2S_OrePillageStart();
			mC2S_OrePillageStart.TargetID = this.playerID;
			mC2S_OrePillageStart.Amount = this.amount;
			mC2S_OrePillageStart.Flag = false;
			Globals.Instance.CliSession.Send(1026, mC2S_OrePillageStart);
			GameUIState uiState = GameUIManager.mInstance.uiState;
			LocalPlayer player = Globals.Instance.Player;
			uiState.PlayerLevel = player.Data.Level;
			uiState.PlayerEnergy = player.Data.Energy;
			uiState.PlayerExp = player.Data.Exp;
			uiState.PlayerMoney = player.Data.Money;
		}
	}

	private GameObject mOpenState;

	private GameObject mCloseState;

	private GameObject mRecordBtnRed;

	private GameObject mBillboardBtn;

	private GameObject mResultBtn;

	private UILabel mTimer0;

	private UILabel mRank;

	private UILabel mMines;

	private UILabel mMayLostMines;

	private UIButton mChangeBtn;

	private UIButton[] mChangeBtns;

	private UILabel mChangeTxt;

	private GUIGuildMinesScene.Target[] targets = new GUIGuildMinesScene.Target[4];

	private UILabel mTimer1;

	private UILabel mTimes;

	private UILabel mRefreshTimer;

	private bool querying;

	private float timerRefresh;

	private bool isInit = true;

	private float BtnRefreshTextTimer;

	private bool queryRank;

	public static void Show(bool showLoading = false)
	{
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(4)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild28", new object[]
			{
				GameConst.GetInt32(4)
			}), 0f, 0f);
			return;
		}
		if (!Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EGR_36", 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUIGuildMinesScene>(null, showLoading, true);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		GuildSubSystem expr_1E = Globals.Instance.Player.GuildSystem;
		expr_1E.QueryOrePillageDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_1E.QueryOrePillageDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryOrePillageDataEvent));
		GuildSubSystem expr_4E = Globals.Instance.Player.GuildSystem;
		expr_4E.QueryMyOreDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_4E.QueryMyOreDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryMyOreDataEvent));
		GuildSubSystem expr_7E = Globals.Instance.Player.GuildSystem;
		expr_7E.UpdateOrePillageDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_7E.UpdateOrePillageDataEvent, new GuildSubSystem.VoidCallback(this.OnUpdateOrePillageDataEvent));
		BillboardSubSystem expr_AE = Globals.Instance.Player.BillboardSystem;
		expr_AE.GetOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_AE.GetOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetOreRankListEvent));
		Globals.Instance.CliSession.Unregister(1033, new ClientSession.MsgHandler(this.OnMsgBuyOrePillageCount));
		Globals.Instance.CliSession.Unregister(1042, new ClientSession.MsgHandler(this.OnMsgOrepillageFarm));
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		GameUIManager.mInstance.GetTopGoods().Show("guildMines0");
		this.CreateObjects();
		GuildSubSystem expr_3E = Globals.Instance.Player.GuildSystem;
		expr_3E.QueryOrePillageDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_3E.QueryOrePillageDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryOrePillageDataEvent));
		GuildSubSystem expr_6E = Globals.Instance.Player.GuildSystem;
		expr_6E.QueryMyOreDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_6E.QueryMyOreDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryMyOreDataEvent));
		GuildSubSystem expr_9E = Globals.Instance.Player.GuildSystem;
		expr_9E.UpdateOrePillageDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_9E.UpdateOrePillageDataEvent, new GuildSubSystem.VoidCallback(this.OnUpdateOrePillageDataEvent));
		BillboardSubSystem expr_CE = Globals.Instance.Player.BillboardSystem;
		expr_CE.GetOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_CE.GetOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetOreRankListEvent));
		Globals.Instance.CliSession.Register(1033, new ClientSession.MsgHandler(this.OnMsgBuyOrePillageCount));
		Globals.Instance.CliSession.Register(1042, new ClientSession.MsgHandler(this.OnMsgOrepillageFarm));
	}

	private void CreateObjects()
	{
		this.mOpenState = GameUITools.FindGameObject("OpenState", base.gameObject);
		this.mOpenState.SetActive(false);
		this.mCloseState = GameUITools.FindGameObject("CloseState", base.gameObject);
		this.mCloseState.SetActive(false);
		this.mTimer0 = GameUITools.FindUILabel("Timer", this.mOpenState);
		GameObject gameObject = GameUITools.FindGameObject("BottomLeft", this.mOpenState);
		this.mRank = GameUITools.FindUILabel("Rank/Value", gameObject);
		this.mMines = GameUITools.FindUILabel("Mines/Value", gameObject);
		this.mMayLostMines = GameUITools.FindUILabel("MayLostMines/Value", gameObject);
		gameObject = GameUITools.FindGameObject("BottomRight", this.mOpenState);
		this.mRefreshTimer = GameUITools.FindUILabel("RefreshTimer/Value", gameObject);
		this.mChangeBtn = GameUITools.RegisterClickEvent("Change", new UIEventListener.VoidDelegate(this.OnChangeClick), gameObject).GetComponent<UIButton>();
		this.mChangeBtns = this.mChangeBtn.GetComponents<UIButton>();
		this.mChangeTxt = GameUITools.FindUILabel("Label", this.mChangeBtn.gameObject);
		this.mTimes = GameUITools.FindUILabel("Times", GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnAddClick), gameObject));
		gameObject = GameUITools.FindGameObject("Targets", this.mOpenState);
		int num = 0;
		while (num < gameObject.transform.childCount && num < this.targets.Length)
		{
			this.targets[num] = gameObject.transform.GetChild(num).gameObject.AddComponent<GUIGuildMinesScene.Target>();
			this.targets[num].Init();
			num++;
		}
		int num2 = Screen.width;
		int num3 = Screen.height;
		float pixelSizeAdjustment = GameUIManager.mInstance.uiRoot.GetPixelSizeAdjustment(num3);
		num2 = Mathf.CeilToInt((float)num2 * pixelSizeAdjustment);
		num3 = Mathf.CeilToInt((float)num3 * pixelSizeAdjustment);
		if ((float)num3 / (float)num2 > 0.5633803f)
		{
			this.targets[0].transform.localPosition = new Vector3(-352f, -6f, 0f);
			this.targets[1].transform.localPosition = new Vector3(-116f, -6f, 0f);
			this.targets[2].transform.localPosition = new Vector3(116f, -6f, 0f);
			this.targets[3].transform.localPosition = new Vector3(352f, -6f, 0f);
		}
		this.mTimer1 = GameUITools.FindUILabel("Time", this.mCloseState);
		gameObject = GameUITools.FindGameObject("TopLeft", base.gameObject);
		GameUITools.RegisterClickEvent("RulesBtn", new UIEventListener.VoidDelegate(this.OnRulesClick), gameObject);
		GameUITools.RegisterClickEvent("RewardDescBtn", new UIEventListener.VoidDelegate(this.OnRewardDescClick), gameObject);
		this.mRecordBtnRed = GameUITools.FindGameObject("Red", GameUITools.RegisterClickEvent("RecordBtn", new UIEventListener.VoidDelegate(this.OnRecordClick), gameObject));
		this.mBillboardBtn = GameUITools.RegisterClickEvent("BillboardBtn", new UIEventListener.VoidDelegate(this.OnBillboardClick), gameObject);
		this.mResultBtn = GameUITools.RegisterClickEvent("ResultBtn", new UIEventListener.VoidDelegate(this.OnResultClick), gameObject);
		this.mBillboardBtn.gameObject.SetActive(false);
		this.mResultBtn.gameObject.SetActive(false);
	}

	protected override void OnLoadedFinished()
	{
		this.SendQueryOrePillageDataMsg();
	}

	private void SendQueryOrePillageDataMsg()
	{
		this.querying = true;
		MC2S_QueryOrePillageData ojb = new MC2S_QueryOrePillageData();
		Globals.Instance.CliSession.Send(1020, ojb);
	}

	private void OnQueryOrePillageDataEvent()
	{
		if (this.isInit && !Globals.Instance.Player.GuildSystem.IsGuildMinesOpen())
		{
			this.isInit = false;
			this.OnResultClick(null);
		}
		this.querying = false;
		this.Refresh();
		this.RefreshTargets();
		this.timerRefresh = Time.time;
		this.RefreshTimer();
		if (Globals.Instance.Player.GuildSystem.IsGuildMinesOpen())
		{
			this.mBillboardBtn.gameObject.SetActive(true);
			this.mResultBtn.gameObject.SetActive(false);
		}
		else
		{
			this.mBillboardBtn.gameObject.SetActive(false);
			this.mResultBtn.gameObject.SetActive(true);
		}
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_OreFirst, 0f);
	}

	private void OnUpdateOrePillageDataEvent()
	{
		this.Refresh();
	}

	private void Refresh()
	{
		MS2C_QueryOrePillageData guildMines = Globals.Instance.Player.GuildSystem.GuildMines;
		if (guildMines == null)
		{
			return;
		}
		if (guildMines.Rank > 0)
		{
			this.mRank.text = guildMines.Rank.ToString();
		}
		else
		{
			this.mRank.text = Singleton<StringManager>.Instance.GetString("Billboard0");
		}
		this.mMines.text = guildMines.OreAmount.ToString();
		this.mMayLostMines.text = guildMines.LostOre.ToString();
		this.mTimes.text = guildMines.PillageCount.ToString();
		if (Globals.Instance.Player.GuildSystem.IsMinesRecordRed())
		{
			this.mRecordBtnRed.gameObject.SetActive(true);
		}
		else
		{
			this.mRecordBtnRed.gameObject.SetActive(false);
		}
	}

	private void RefreshTargets()
	{
		List<OrePillageTarget> data = Globals.Instance.Player.GuildSystem.GuildMines.Data;
		int num = 0;
		while (num < this.targets.Length && num < data.Count)
		{
			if (this.targets[num] != null)
			{
				this.targets[num].Refresh(data[num]);
			}
			num++;
		}
	}

	private void RefreshTimer()
	{
		if (!Globals.Instance.Player.GuildSystem.IsGuildMinesOpen())
		{
			if (!this.mCloseState.activeInHierarchy)
			{
				this.mCloseState.SetActive(true);
			}
			if (this.mOpenState.activeInHierarchy)
			{
				this.mOpenState.SetActive(false);
			}
			this.mTimer1.text = Tools.FormatTimeStr2(this.GetTime2Open(), false, false);
		}
		else
		{
			if (!this.mOpenState.activeInHierarchy)
			{
				this.mOpenState.SetActive(true);
			}
			if (this.mCloseState.activeInHierarchy)
			{
				this.mCloseState.SetActive(false);
			}
			this.mTimer0.text = Tools.FormatTimeStr2(this.GetTime2Close(), false, false);
			this.RefreshRenewTime();
		}
	}

	private int GetTime2Close()
	{
		int num = Globals.Instance.Player.GuildSystem.GuildMines.Timestamp - Globals.Instance.Player.GetTimeStamp();
		if (num < 0)
		{
			this.SendQueryOrePillageDataMsg();
			num = 0;
		}
		return num;
	}

	private int GetTime2Open()
	{
		int num = Globals.Instance.Player.GuildSystem.GuildMines.Timestamp - Globals.Instance.Player.GetTimeStamp();
		if (num < 0)
		{
			this.SendQueryOrePillageDataMsg();
			num = 0;
		}
		return num;
	}

	private void RefreshRenewTime()
	{
		int num = Globals.Instance.Player.GuildSystem.GuildMines.PillageCountTimestamp - Globals.Instance.Player.GetTimeStamp();
		if (num > 0)
		{
			if (!this.mRefreshTimer.gameObject.activeInHierarchy)
			{
				this.mRefreshTimer.transform.parent.gameObject.SetActive(true);
			}
			this.mRefreshTimer.text = Tools.FormatTime(num);
		}
		else if (this.mRefreshTimer.gameObject.activeInHierarchy)
		{
			this.mRefreshTimer.transform.parent.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (base.PostLoadGUIDone && !this.querying && Time.time - this.timerRefresh >= 1f)
		{
			this.RefreshTimer();
		}
		this.RefreshBtnRefreshText();
	}

	private void RefreshBtnRefreshText()
	{
		if (!this.mChangeBtn.isEnabled)
		{
			this.BtnRefreshTextTimer -= Time.deltaTime;
			if (this.BtnRefreshTextTimer < 0f)
			{
				this.BtnRefreshTextTimer = 3.40282347E+38f;
				this.mChangeTxt.text = Singleton<StringManager>.Instance.GetString("Pillage17");
				this.mChangeBtn.isEnabled = true;
				for (int i = 0; i < this.mChangeBtns.Length; i++)
				{
					this.mChangeBtns[i].SetState(UIButtonColor.State.Normal, true);
				}
			}
			else
			{
				this.mChangeTxt.text = string.Format("{0}({1})", Singleton<StringManager>.Instance.GetString("Pillage17"), (int)(this.BtnRefreshTextTimer + 1f));
			}
		}
	}

	private void OnChangeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.SendQueryOrePillageDataMsg();
		this.mChangeBtn.isEnabled = false;
		this.BtnRefreshTextTimer = 10f;
		for (int i = 0; i < this.mChangeBtns.Length; i++)
		{
			this.mChangeBtns[i].SetState(UIButtonColor.State.Disabled, true);
		}
	}

	private void OnAddClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
		if (Globals.Instance.Player.GuildSystem.GuildMines.BuyPillageCount >= vipLevelInfo.BuyPillageCount)
		{
			if (Globals.Instance.Player.Data.VipLevel >= 15u)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guildMines20", 0f, 0f);
			}
			else
			{
				VipLevelInfo vipLevelInfo2 = null;
				for (VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(Globals.Instance.Player.Data.VipLevel + 1u)); info != null; info = Globals.Instance.AttDB.VipLevelDict.GetInfo(info.ID + 1))
				{
					if (info.ID > 15)
					{
						break;
					}
					if (info.BuyPillageCount > vipLevelInfo.BuyPillageCount)
					{
						vipLevelInfo2 = info;
						break;
					}
				}
				if (vipLevelInfo2 != null)
				{
					GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("guildMines21"), vipLevelInfo2.ID, vipLevelInfo2.BuyPillageCount));
				}
				else
				{
					GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildMines20"), MessageBox.Type.OK, null);
				}
			}
			return;
		}
		int num = Globals.Instance.Player.GuildSystem.GuildMines.BuyPillageCount + 1;
		if (num > MiscTable.MaxBuyOrePillageCountCostID)
		{
			num = MiscTable.MaxBuyOrePillageCountCostID;
		}
		MiscInfo info2 = Globals.Instance.AttDB.MiscDict.GetInfo(num);
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("MiscDict get info error , ID : {0}", new object[]
			{
				num
			});
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, info2.BuyOrePillageCountCost, 0))
		{
			string @string = Singleton<StringManager>.Instance.GetString("guildMines7", new object[]
			{
				(Globals.Instance.Player.Data.Diamond >= info2.BuyOrePillageCountCost) ? "[00ff00]" : "[ff0000]",
				info2.BuyOrePillageCountCost
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.Custom2Btn, null);
			gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("OKBuy");
			GameMessageBox expr_247 = gameMessageBox;
			expr_247.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_247.OkClick, new MessageBox.MessageDelegate(this.OnOkBuyPillageCount));
		}
	}

	private void OnOkBuyPillageCount(object obj)
	{
		MC2S_BuyOrePillageCount ojb = new MC2S_BuyOrePillageCount();
		Globals.Instance.CliSession.Send(1032, ojb);
	}

	private void OnMsgBuyOrePillageCount(MemoryStream stream)
	{
		MS2C_BuyOrePillageCount mS2C_BuyOrePillageCount = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyOrePillageCount), stream) as MS2C_BuyOrePillageCount;
		if (mS2C_BuyOrePillageCount.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_BuyOrePillageCount.Result);
			return;
		}
	}

	private void OnMsgOrepillageFarm(MemoryStream stream)
	{
		MS2C_OrepillageFarm mS2C_OrepillageFarm = Serializer.NonGeneric.Deserialize(typeof(MS2C_OrepillageFarm), stream) as MS2C_OrepillageFarm;
		if (mS2C_OrepillageFarm.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_OrepillageFarm.Result);
			return;
		}
		GUIGuildMinesResultScene.Show(true, new MS2C_OrePillageResult
		{
			Amount1 = mS2C_OrepillageFarm.Amount1,
			Amount2 = mS2C_OrepillageFarm.Amount2,
			ElapsedTime = 0
		});
	}

	private void OnRulesClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("guildMines0", "guildMines24");
	}

	private void OnRewardDescClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuildMinesRewardDescPopUp.Show();
	}

	private void OnRecordClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_QueryMyOreData ojb = new MC2S_QueryMyOreData();
		Globals.Instance.CliSession.Send(1022, ojb);
	}

	private void OnQueryMyOreDataEvent()
	{
		MS2C_QueryMyOreData myOreData = Globals.Instance.Player.GuildSystem.MyOreData;
		this.mMines.text = (myOreData.Amount1 + myOreData.Amount2).ToString();
		GUIGuildMinesRecordPopUp.Show();
		this.Refresh();
	}

	private void OnBillboardClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.queryRank = true;
		Globals.Instance.Player.BillboardSystem.GetOreRankList();
	}

	private void OnGetOreRankListEvent()
	{
		if (this.queryRank)
		{
			this.queryRank = false;
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp, false, null, null);
			GameUICommonBillboardPopUp gameUICommonBillboardPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUICommonBillboardPopUp;
			gameUICommonBillboardPopUp.InitBillboard("PersonalMinesRankItem");
			BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
			List<object> list = new List<object>();
			for (int i = 0; i < billboardSystem.OreRankData.Count; i++)
			{
				RankData rankData = billboardSystem.OreRankData[i];
				if (rankData != null)
				{
					list.Add(rankData);
				}
			}
			gameUICommonBillboardPopUp.InitItems(list, 3, 0);
			string selfRankTxt = string.Empty;
			if (billboardSystem.OreRank > 0u)
			{
				selfRankTxt = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
				{
					billboardSystem.OreRank
				});
			}
			else
			{
				selfRankTxt = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
				{
					Singleton<StringManager>.Instance.GetString("trailTower13")
				});
			}
			int num = (Globals.Instance.Player.GuildSystem.GuildMines != null) ? Globals.Instance.Player.GuildSystem.GuildMines.OreAmount : 0;
			gameUICommonBillboardPopUp.Refresh(Singleton<StringManager>.Instance.GetString("guildMines0"), string.Empty, selfRankTxt, null, (num <= 0) ? string.Empty : string.Format("[9e865a]{0}[-]{1}", Singleton<StringManager>.Instance.GetString("guildMines14"), num));
			return;
		}
	}

	private void OnResultClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuildMinesResultPopUp.Show();
	}
}
