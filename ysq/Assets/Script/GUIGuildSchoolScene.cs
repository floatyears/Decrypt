using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIGuildSchoolScene : GameUISession
{
	private GameObject mMonsterModelPos;

	private GameObject mMonsterModel;

	private GameObject mKilledFlag;

	private GameObject mBattleBtn;

	private GameObject mBattleBtnEffect;

	private UIButton mTakeRewardBtn;

	private UILabel mTakeRewardBtnLb;

	private GameObject mTakeRewardEffect;

	private UILabel mTiaoZhanRewardTip;

	private UILabel mTakeNum;

	private UILabel mShoolTitle;

	private UILabel mJiShaTipTxt;

	private UILabel mMonsterName;

	private UISprite mMonsterFlag;

	private UISlider mMonsterHpBar;

	private GUIGuildLootItemTable mLootItemTable;

	private int mSchoolId;

	private ResourceEntity asyncEntiry;

	public int SchoolId
	{
		get
		{
			return this.mSchoolId;
		}
		set
		{
			this.mSchoolId = value;
			GuildSubSystem guildSystem = Globals.Instance.Player.GuildSystem;
			MS2C_GetLootReward mS2C_GetLootReward;
			if (!guildSystem.mSchoolLootDataCaches.TryGetValue(this.mSchoolId, out mS2C_GetLootReward))
			{
				guildSystem.DoSendGetLootRequest(this.mSchoolId);
			}
			this.Refresh(true);
		}
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		this.CreateObjects();
		Globals.Instance.Player.GuildSystem.ClearSchoolLootDataCache();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("guildSchool50");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		GuildSubSystem expr_66 = Globals.Instance.Player.GuildSystem;
		expr_66.SchoolLootDatasInitEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_66.SchoolLootDatasInitEvent, new GuildSubSystem.VoidCallback(this.OnSchoolLootDatasUpdateEvent));
		BillboardSubSystem expr_96 = Globals.Instance.Player.BillboardSystem;
		expr_96.GuildBossDamageRankDataEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_96.GuildBossDamageRankDataEvent, new BillboardSubSystem.VoidCallback(this.OnGuildBossDamageRankDataEvent));
		Globals.Instance.CliSession.Register(941, new ClientSession.MsgHandler(this.OnMsgBuyGuildBossCount));
		Globals.Instance.CliSession.Register(963, new ClientSession.MsgHandler(this.OnMsgGuildBossStart));
		Globals.Instance.CliSession.Register(943, new ClientSession.MsgHandler(this.OnMsgTakeGuildBossReward));
	}

	protected override void OnLoadedFinished()
	{
		this.SchoolId = Globals.Instance.Player.GuildSystem.Guild.AttackAcademyID1;
	}

	protected override void OnPreDestroyGUI()
	{
		this.DestroyModel();
		GameUIManager.mInstance.GetTopGoods().Hide();
		GuildSubSystem expr_24 = Globals.Instance.Player.GuildSystem;
		expr_24.SchoolLootDatasInitEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_24.SchoolLootDatasInitEvent, new GuildSubSystem.VoidCallback(this.OnSchoolLootDatasUpdateEvent));
		BillboardSubSystem expr_54 = Globals.Instance.Player.BillboardSystem;
		expr_54.GuildBossDamageRankDataEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_54.GuildBossDamageRankDataEvent, new BillboardSubSystem.VoidCallback(this.OnGuildBossDamageRankDataEvent));
		Globals.Instance.CliSession.Unregister(941, new ClientSession.MsgHandler(this.OnMsgBuyGuildBossCount));
		Globals.Instance.CliSession.Unregister(963, new ClientSession.MsgHandler(this.OnMsgGuildBossStart));
		Globals.Instance.CliSession.Unregister(943, new ClientSession.MsgHandler(this.OnMsgTakeGuildBossReward));
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		this.mMonsterModelPos = transform.Find("monsterModel").gameObject;
		Transform transform2 = transform.Find("infoBg");
		this.mMonsterName = transform2.Find("monsterName").GetComponent<UILabel>();
		this.mMonsterFlag = transform2.Find("flagBg/flag").GetComponent<UISprite>();
		this.mMonsterHpBar = transform2.Find("hpBar").GetComponent<UISlider>();
		this.mKilledFlag = transform2.Find("killFlag").gameObject;
		this.mKilledFlag.SetActive(false);
		this.mJiShaTipTxt = transform2.Find("tipTxt").GetComponent<UILabel>();
		this.mShoolTitle = transform.Find("titleBg/title").GetComponent<UILabel>();
		this.mTiaoZhanRewardTip = transform.Find("rewardTxt").GetComponent<UILabel>();
		this.mTakeNum = transform.Find("takeNum").GetComponent<UILabel>();
		this.mBattleBtn = transform.Find("battleBtn").gameObject;
		UIEventListener expr_110 = UIEventListener.Get(this.mBattleBtn);
		expr_110.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_110.onClick, new UIEventListener.VoidDelegate(this.OnBattleBtnClick));
		this.mBattleBtnEffect = this.mBattleBtn.transform.Find("Effect").gameObject;
		this.mBattleBtnEffect.SetActive(false);
		this.mTakeRewardBtn = transform.Find("rewardBtn").GetComponent<UIButton>();
		UIEventListener expr_183 = UIEventListener.Get(this.mTakeRewardBtn.gameObject);
		expr_183.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_183.onClick, new UIEventListener.VoidDelegate(this.OnTakeRewardBtnClick));
		this.mTakeRewardBtnLb = this.mTakeRewardBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mTakeRewardEffect = this.mTakeRewardBtn.transform.Find("Effect").gameObject;
		this.mTakeRewardEffect.SetActive(false);
		GameObject gameObject = transform.Find("btnTeam").gameObject;
		UIEventListener expr_207 = UIEventListener.Get(gameObject);
		expr_207.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_207.onClick, new UIEventListener.VoidDelegate(this.OnTeamBtnClick));
		GameUITools.RegisterClickEvent("billboardBtn", new UIEventListener.VoidDelegate(this.OnBillboardBtnClick), transform.gameObject);
		this.mLootItemTable = transform.Find("lootBg/contentsPanel/contents").gameObject.AddComponent<GUIGuildLootItemTable>();
		this.mLootItemTable.maxPerLine = 1;
		this.mLootItemTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mLootItemTable.cellWidth = 280f;
		this.mLootItemTable.cellHeight = 74f;
	}

	private void OnBackClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		Type type = GameUIManager.mInstance.GobackSession();
		if (type == typeof(GUIGuildManageScene))
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildSchoolPopUp, false, null, null);
		}
	}

	private void DestroyModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mMonsterModel != null)
		{
			UnityEngine.Object.DestroyImmediate(this.mMonsterModel);
			this.mMonsterModel = null;
		}
	}

	private void CreateModel(MonsterInfo monsterInfo)
	{
		this.DestroyModel();
		if (monsterInfo != null)
		{
			this.asyncEntiry = ActorManager.CreateUIMonster(monsterInfo, 0, false, false, this.mMonsterModelPos, 1f, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mMonsterModel = go;
			});
		}
	}

	private void Refresh(bool isCreateModel = false)
	{
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(this.mSchoolId);
		GuildBossData guildBossData = Globals.Instance.Player.GuildSystem.GetGuildBossData(this.mSchoolId);
		MonsterInfo monsterInfo = null;
		if (guildBossData != null && info != null)
		{
			monsterInfo = Globals.Instance.AttDB.MonsterDict.GetInfo(guildBossData.InfoID);
			if (monsterInfo != null)
			{
				this.mKilledFlag.SetActive(guildBossData.HealthPct == 0f);
				this.mMonsterName.text = monsterInfo.Name;
				this.mMonsterFlag.spriteName = Tools.GetPropertyIconWithBorder((EElementType)monsterInfo.ElementType);
				this.mMonsterHpBar.value = Mathf.Clamp01(guildBossData.HealthPct);
			}
			this.mJiShaTipTxt.text = Singleton<StringManager>.Instance.GetString("guildSchool10", new object[]
			{
				info.RewardExp
			});
			this.mTiaoZhanRewardTip.text = Singleton<StringManager>.Instance.GetString("guildSchool11", new object[]
			{
				info.BossReputation
			});
			if (guildBossData.HealthPct == 0f)
			{
				this.mTakeNum.text = Singleton<StringManager>.Instance.GetString("guildSchool7");
				this.mBattleBtn.SetActive(false);
				this.mTakeRewardBtn.gameObject.SetActive(true);
				this.RefreshTakenBtn();
			}
			else
			{
				int num = GameConst.GetInt32(150) - Globals.Instance.Player.Data.GuildBossCount;
				this.mTakeNum.text = Singleton<StringManager>.Instance.GetString("guildSchool5", new object[]
				{
					(num <= 0) ? "[ff0000]" : "[ffffff]",
					num,
					GameConst.GetInt32(150)
				});
				this.mBattleBtn.SetActive(true);
				this.mBattleBtnEffect.SetActive(Tools.IsInGuildBossTime() && Tools.IsGuildBossHasNum());
				this.mTakeRewardBtn.gameObject.SetActive(false);
			}
		}
		else if (info != null)
		{
			if (info.BossID != 0)
			{
				monsterInfo = Globals.Instance.AttDB.MonsterDict.GetInfo(info.BossID);
				if (monsterInfo != null)
				{
					this.mKilledFlag.SetActive(false);
					this.mMonsterName.text = monsterInfo.Name;
					this.mMonsterFlag.spriteName = Tools.GetPropertyIconWithBorder((EElementType)monsterInfo.ElementType);
					this.mMonsterHpBar.value = 1f;
				}
			}
			this.mTakeNum.gameObject.SetActive(false);
			this.mBattleBtn.gameObject.SetActive(false);
			this.mTakeRewardBtn.gameObject.SetActive(false);
		}
		if (isCreateModel && monsterInfo != null)
		{
			this.CreateModel(monsterInfo);
		}
		if (info != null && !string.IsNullOrEmpty(info.Academy))
		{
			this.mShoolTitle.text = Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
			{
				this.mSchoolId,
				info.Academy
			});
		}
		this.InitLootItems();
	}

	private void InitLootItems()
	{
		this.mLootItemTable.ClearData();
		MS2C_GetLootReward mS2C_GetLootReward;
		if (Globals.Instance.Player.GuildSystem.mSchoolLootDataCaches.TryGetValue(this.mSchoolId, out mS2C_GetLootReward) && mS2C_GetLootReward.LootReward != null)
		{
			for (int i = 0; i < mS2C_GetLootReward.LootReward.Count; i++)
			{
				RewardData rewardData = mS2C_GetLootReward.LootReward[i];
				if (rewardData != null)
				{
					this.mLootItemTable.AddData(new GUIGuildLootItemData(rewardData));
				}
			}
		}
	}

	private void OnOkResetBoss(object obj)
	{
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(153), 0))
		{
			MC2S_BuyGuildBossCount ojb = new MC2S_BuyGuildBossCount();
			Globals.Instance.CliSession.Send(940, ojb);
		}
	}

	public void StartPveGame()
	{
		base.StartCoroutine(this.SendPveStart());
	}

	[DebuggerHidden]
	private IEnumerator SendPveStart()
	{
        return null;
        //GUIGuildSchoolScene.<SendPveStart>c__Iterator60 <SendPveStart>c__Iterator = new GUIGuildSchoolScene.<SendPveStart>c__Iterator60();
        //<SendPveStart>c__Iterator.<>f__this = this;
        //return <SendPveStart>c__Iterator;
	}

	private void OnBattleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.GuildBossCount == GameConst.GetInt32(150))
		{
			string @string = Singleton<StringManager>.Instance.GetString("guildSchool14", new object[]
			{
				((long)Globals.Instance.Player.Data.Diamond >= (long)((ulong)GameConst.GetInt32(153))) ? "[00ff00]" : "[ff0000]",
				GameConst.GetInt32(153)
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.Custom2Btn, null);
			gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("OKBuy");
			GameMessageBox expr_B6 = gameMessageBox;
			expr_B6.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_B6.OkClick, new MessageBox.MessageDelegate(this.OnOkResetBoss));
			return;
		}
		if (Globals.Instance.Player.GuildSystem.IsBossDead(this.SchoolId))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EGR_29", 0f, 0f);
			return;
		}
		if (Globals.Instance.Player.GetTimeStamp() >= Globals.Instance.Player.GuildSystem.GuildBossGMOpenTime && !Tools.IsInGuildBossTime())
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", 70);
			return;
		}
		this.StartPveGame();
	}

	private void OnSchoolLootDatasUpdateEvent()
	{
		this.Refresh(false);
	}

	private void OnTakeRewardBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(this.mSchoolId);
		GuildBossData guildBossData = Globals.Instance.Player.GuildSystem.GetGuildBossData(this.mSchoolId);
		if (guildBossData != null && info != null && guildBossData.HealthPct == 0f)
		{
			this.mTakeNum.text = Singleton<StringManager>.Instance.GetString("guildSchool7");
			this.mBattleBtn.SetActive(false);
			this.mTakeRewardBtn.gameObject.SetActive(true);
			if ((Globals.Instance.Player.Data.DataFlag & 16) == 0)
			{
				MC2S_TakeGuildBossReward mC2S_TakeGuildBossReward = new MC2S_TakeGuildBossReward();
				mC2S_TakeGuildBossReward.id = this.SchoolId;
				Globals.Instance.CliSession.Send(942, mC2S_TakeGuildBossReward);
			}
		}
	}

	private void OnTeamBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = 0;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	private void OnBillboardBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.Player.BillboardSystem.QueryGuildBossDamageRank();
	}

	private void OnGuildBossDamageRankDataEvent()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp, false, null, null);
		GameUICommonBillboardPopUp gameUICommonBillboardPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUICommonBillboardPopUp;
		gameUICommonBillboardPopUp.InitBillboard("GuildBossDamageRankItem");
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.GuildBossDamageRankData.Count; i++)
		{
			RankData rankData = billboardSystem.GuildBossDamageRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		gameUICommonBillboardPopUp.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.GuildBossDamageRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.GuildBossDamageRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("trailTower13")
			});
		}
		long num = 0L;
		if (billboardSystem.GuildBossDamageRank >= 0u && (ulong)billboardSystem.GuildBossDamageRank <= (ulong)((long)billboardSystem.GuildBossDamageRankData.Count))
		{
			for (int j = 0; j < billboardSystem.GuildBossDamageRankData.Count; j++)
			{
				RankData rankData2 = billboardSystem.GuildBossDamageRankData[j];
				if (rankData2 != null && rankData2.Data.GUID == Globals.Instance.Player.Data.ID)
				{
					num = rankData2.Value;
				}
			}
		}
		else
		{
			num = 0L;
		}
		gameUICommonBillboardPopUp.Refresh(Singleton<StringManager>.Instance.GetString("Billboard1"), string.Empty, @string, null, string.Format("[9e865a]{0}[-]{1}", Singleton<StringManager>.Instance.GetString("guildSchool28"), num));
	}

	private void OnMsgBuyGuildBossCount(MemoryStream stream)
	{
		MS2C_BuyGuildBossCount mS2C_BuyGuildBossCount = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyGuildBossCount), stream) as MS2C_BuyGuildBossCount;
		if (mS2C_BuyGuildBossCount.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_BuyGuildBossCount.Result);
			return;
		}
		this.Refresh(false);
	}

	private void OnMsgGuildBossStart(MemoryStream stream)
	{
		MS2C_GuildBossStart mS2C_GuildBossStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildBossStart), stream) as MS2C_GuildBossStart;
		if (mS2C_GuildBossStart.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildBossStart.Result);
			return;
		}
		GameUIManager.mInstance.uiState.GuildBossHp = mS2C_GuildBossStart.HealthPct;
		Globals.Instance.Player.GuildSystem.SetCurBossID(mS2C_GuildBossStart.ID);
		Globals.Instance.Player.GuildSystem.UpdateCurBossData(mS2C_GuildBossStart.InfoID, mS2C_GuildBossStart.HealthPct);
		Globals.Instance.ActorMgr.SetServerData(mS2C_GuildBossStart.Key, mS2C_GuildBossStart.Data);
		GameUIManager.mInstance.LoadScene(mS2C_GuildBossStart.SceneID);
	}

	private void RefreshTakenBtn()
	{
		if ((Globals.Instance.Player.Data.DataFlag & 16) != 0)
		{
			this.mTakeRewardBtn.isEnabled = false;
			Tools.SetButtonState(this.mTakeRewardBtn.gameObject, false);
			this.mTakeRewardBtnLb.text = Singleton<StringManager>.Instance.GetString("guildSchool13");
			this.mTakeRewardEffect.SetActive(false);
		}
		else
		{
			this.mTakeRewardBtn.isEnabled = true;
			Tools.SetButtonState(this.mTakeRewardBtn.gameObject, true);
			this.mTakeRewardBtnLb.text = Singleton<StringManager>.Instance.GetString("guildSchool12");
			this.mTakeRewardEffect.SetActive(true);
		}
	}

	private void OnMsgTakeGuildBossReward(MemoryStream stream)
	{
		MS2C_TakeGuildBossReward mS2C_TakeGuildBossReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeGuildBossReward), stream) as MS2C_TakeGuildBossReward;
		if (mS2C_TakeGuildBossReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_TakeGuildBossReward.Result);
			return;
		}
		this.RefreshTakenBtn();
		GUIRewardPanel.Show(new List<RewardData>
		{
			new RewardData
			{
				RewardType = mS2C_TakeGuildBossReward.Reward.RewardType,
				RewardValue1 = mS2C_TakeGuildBossReward.Reward.RewardValue1,
				RewardValue2 = mS2C_TakeGuildBossReward.Reward.RewardValue2
			},
			new RewardData
			{
				RewardType = 7,
				RewardValue1 = mS2C_TakeGuildBossReward.Reputation,
				RewardValue2 = 0
			}
		}, null, false, true, null, false);
	}
}
