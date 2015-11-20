using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIPVP4ReadyScene : GameUISession
{
	private GameObject rulesBtn;

	private GameObject fightRecordBtn;

	private GameObject fightRecordNew;

	private GameObject pvpBillboardBtn;

	private UILabel selfRank;

	private UILabel Rankreward;

	private UILabel RankrewardDiamond;

	private UILabel RankrewardHonor;

	private UILabel stamina;

	private UILabel honor;

	private GameObject honorShopBtn;

	private GUIPVP4FarmPopUp mPVP4FarmPopUp;

	private bool RefreshPlayerDataFlag;

	public PVPTargetGrid mTargetTable
	{
		get;
		set;
	}

	public static void TryOpen()
	{
		if (Globals.Instance.Player.PetSystem.Values.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt18", 0f, 0f);
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(6)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(6)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUIPVP4ReadyScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("pvp4Txt0");
		topGoods.SetGoodsSlot(new TopGoods.EGoodsUIType[]
		{
			TopGoods.EGoodsUIType.EGT_UIDiamond,
			TopGoods.EGoodsUIType.EGT_UIStamina,
			TopGoods.EGoodsUIType.EGT_UIHonor
		});
		PvpSubSystem expr_58 = Globals.Instance.Player.PvpSystem;
		expr_58.QueryPvpRecordEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_58.QueryPvpRecordEvent, new PvpSubSystem.VoidCallback(this.OnQueryPvpRecordEvent));
		PvpSubSystem expr_88 = Globals.Instance.Player.PvpSystem;
		expr_88.QueryArenaRankEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_88.QueryArenaRankEvent, new PvpSubSystem.VoidCallback(this.OnQueryArenaRankEvent));
		PvpSubSystem expr_B8 = Globals.Instance.Player.PvpSystem;
		expr_B8.QueryArenaDataEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_B8.QueryArenaDataEvent, new PvpSubSystem.VoidCallback(this.OnQueryArenaDataEvent));
		LocalPlayer expr_E3 = Globals.Instance.Player;
		expr_E3.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_E3.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Register(825, new ClientSession.MsgHandler(this.OnMsgPvpFarm));
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		LocalPlayer player = Globals.Instance.Player;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerEnergy = player.Data.Energy;
		this.SendQueryArena();
	}

	protected override void OnPreDestroyGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		topGoods.DefaultGoodsSlot();
		PvpSubSystem expr_26 = Globals.Instance.Player.PvpSystem;
		expr_26.QueryPvpRecordEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_26.QueryPvpRecordEvent, new PvpSubSystem.VoidCallback(this.OnQueryPvpRecordEvent));
		PvpSubSystem expr_56 = Globals.Instance.Player.PvpSystem;
		expr_56.QueryArenaRankEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_56.QueryArenaRankEvent, new PvpSubSystem.VoidCallback(this.OnQueryArenaRankEvent));
		PvpSubSystem expr_86 = Globals.Instance.Player.PvpSystem;
		expr_86.QueryArenaDataEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_86.QueryArenaDataEvent, new PvpSubSystem.VoidCallback(this.OnQueryArenaDataEvent));
		LocalPlayer expr_B1 = Globals.Instance.Player;
		expr_B1.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_B1.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Unregister(825, new ClientSession.MsgHandler(this.OnMsgPvpFarm));
	}

	private void SendQueryArena()
	{
		MC2S_QueryArenaData ojb = new MC2S_QueryArenaData();
		Globals.Instance.CliSession.Send(801, ojb);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.FindChild("LeftTop");
		this.rulesBtn = transform.FindChild("rulesBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(this.rulesBtn);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnRuleInfoBtnClick));
		this.fightRecordBtn = transform.FindChild("Record/fightRecordBtn").gameObject;
		UIEventListener expr_74 = UIEventListener.Get(this.fightRecordBtn);
		expr_74.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_74.onClick, new UIEventListener.VoidDelegate(this.OnQueryPvpRecordClick));
		this.fightRecordNew = this.fightRecordBtn.transform.FindChild("new").gameObject;
		this.fightRecordNew.gameObject.SetActive(false);
		this.pvpBillboardBtn = transform.FindChild("Billboard/pvpBillboardBtn").gameObject;
		UIEventListener expr_E7 = UIEventListener.Get(this.pvpBillboardBtn);
		expr_E7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E7.onClick, new UIEventListener.VoidDelegate(this.OnPvpQueryArenaRankClick));
		this.selfRank = transform.FindChild("selfRank/rankNum").GetComponent<UILabel>();
		this.selfRank.text = string.Empty;
		this.Rankreward = transform.FindChild("selfRank/Rankreward").GetComponent<UILabel>();
		this.Rankreward.text = string.Empty;
		this.RankrewardDiamond = transform.FindChild("selfRank/Gem/num").GetComponent<UILabel>();
		this.RankrewardDiamond.text = string.Empty;
		this.RankrewardHonor = transform.FindChild("selfRank/Honor/num").GetComponent<UILabel>();
		this.RankrewardHonor.text = string.Empty;
		this.stamina = transform.FindChild("jingli/num").GetComponent<UILabel>();
		this.stamina.text = string.Empty;
		Transform transform2 = base.transform.FindChild("LeftBottom");
		this.honorShopBtn = transform2.FindChild("honorShop").gameObject;
		UIEventListener expr_1F8 = UIEventListener.Get(this.honorShopBtn);
		expr_1F8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1F8.onClick, new UIEventListener.VoidDelegate(this.OnHonorShopClick));
		this.honor = transform2.FindChild("honor/num").GetComponent<UILabel>();
		this.honor.text = string.Empty;
		Transform transform3 = base.transform.Find("Right");
		Transform transform4 = transform3.transform.Find("bagPanel");
		this.mTargetTable = transform4.transform.Find("bagContents").gameObject.AddComponent<PVPTargetGrid>();
		this.mTargetTable.maxPerLine = 1;
		this.mTargetTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mTargetTable.cellWidth = 677f;
		this.mTargetTable.cellHeight = 138f;
		this.mPVP4FarmPopUp = GameUITools.FindGameObject("FarmPopUp", base.gameObject).AddComponent<GUIPVP4FarmPopUp>();
		this.mPVP4FarmPopUp.Init();
		this.mPVP4FarmPopUp.gameObject.SetActive(false);
		this.RefreshPlayerDataFlag = true;
	}

	private void Update()
	{
		if (this.RefreshPlayerDataFlag)
		{
			this.RefreshPlayerData();
		}
	}

	private void RefreshPlayerData()
	{
		if (!this.RefreshPlayerDataFlag)
		{
			return;
		}
		this.RefreshPlayerDataFlag = false;
		this.stamina.text = GameConst.GetInt32(36).ToString();
		if (Globals.Instance.Player.Data.Stamina < GameConst.GetInt32(36))
		{
			this.stamina.color = Color.red;
		}
		this.fightRecordNew.SetActive((Globals.Instance.Player.Data.RedFlag & 256) != 0);
		this.honor.text = Globals.Instance.Player.Data.Honor.ToString();
	}

	private void OnRuleInfoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIPVPRuleInfoPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
	}

	private void OnPvpQueryArenaRankClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_QueryArenaRank mC2S_QueryArenaRank = new MC2S_QueryArenaRank();
		mC2S_QueryArenaRank.RankVersion = Globals.Instance.Player.PvpSystem.ArenaRankVersion;
		Globals.Instance.CliSession.Send(808, mC2S_QueryArenaRank);
	}

	private void OnQueryPvpRecordClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_QueryPvpRecord mC2S_QueryPvpRecord = new MC2S_QueryPvpRecord();
		mC2S_QueryPvpRecord.RecordVersion = Globals.Instance.Player.PvpSystem.ArenaRecordVersion;
		Globals.Instance.CliSession.Send(810, mC2S_QueryPvpRecord);
	}

	public void OnHonorShopClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShopScene.TryOpen(EShopType.EShop_Pvp);
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshPlayerDataFlag = true;
	}

	private void OnQueryArenaDataEvent()
	{
		this.RefreshRankData();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_Arena, 0f);
	}

	private void OnQueryPvpRecordEvent()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIPvpRecordPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
	}

	private void OnQueryArenaRankEvent()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp, false, null, null);
		GameUICommonBillboardPopUp gameUICommonBillboardPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUICommonBillboardPopUp;
		if (gameUICommonBillboardPopUp == null)
		{
			return;
		}
		gameUICommonBillboardPopUp.InitBillboard("PVP4RankItem");
		PvpSubSystem pvpSystem = Globals.Instance.Player.PvpSystem;
		List<object> list = new List<object>();
		for (int i = 0; i < pvpSystem.ArenaRank.Count; i++)
		{
			RankData rankData = pvpSystem.ArenaRank[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		gameUICommonBillboardPopUp.InitItems(list, 3, 0);
		string @string;
		if (pvpSystem.Rank <= 15000 && 0 < pvpSystem.Rank)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				pvpSystem.Rank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("trailTower13")
			});
		}
		gameUICommonBillboardPopUp.Refresh(Singleton<StringManager>.Instance.GetString("pvp4Txt9"), Singleton<StringManager>.Instance.GetString("pvp4Txt10", new object[]
		{
			"22:00"
		}), @string, null, null);
	}

	private void RefreshRankReward(PvpInfo info)
	{
		if (info == null)
		{
			return;
		}
		this.Rankreward.text = Singleton<StringManager>.Instance.GetString("pvp4Txt18", new object[]
		{
			info.ArenaLowRank
		});
		this.RankrewardDiamond.text = info.ArenaRewardDiamond.ToString();
		this.RankrewardHonor.text = info.ArenaRewardHonor.ToString();
	}

	private void RefreshRankData()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (!Globals.Instance.Player.PvpSystem.ShowSelfRank())
		{
			this.selfRank.text = string.Empty;
		}
		else
		{
			this.selfRank.text = player.PvpSystem.Rank.ToString();
		}
		bool flag = false;
		PvpInfo pvpInfo = null;
		foreach (PvpInfo current in Globals.Instance.AttDB.PvpDict.Values)
		{
			if (current.ArenaHighRank <= player.PvpSystem.Rank && player.PvpSystem.Rank <= current.ArenaLowRank)
			{
				this.RefreshRankReward(current);
				flag = true;
				break;
			}
			if (pvpInfo == null || pvpInfo.ArenaLowRank < current.ArenaLowRank)
			{
				pvpInfo = current;
			}
		}
		if (!flag)
		{
			this.RefreshRankReward(pvpInfo);
		}
		this.mTargetTable.ClearData();
		List<RankData> arenaTargets = Globals.Instance.Player.PvpSystem.ArenaTargets;
		for (int i = 0; i < arenaTargets.Count; i++)
		{
			if (arenaTargets[i] != null)
			{
				PVPTargetDaata data = new PVPTargetDaata(arenaTargets[i]);
				this.mTargetTable.AddData(data);
				if (player.Data.ID == arenaTargets[i].Data.GUID && i > 2)
				{
					this.mTargetTable.focusID = arenaTargets[i - 2].Data.GUID;
				}
			}
		}
		this.mTargetTable.repositionNow = true;
	}

	private void OnMsgPvpFarm(MemoryStream stream)
	{
		MS2C_PvpFarm mS2C_PvpFarm = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpFarm), stream) as MS2C_PvpFarm;
		if (mS2C_PvpFarm.Result == 34)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_PvpFarm.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpFarm.Result);
			return;
		}
		if (this.mPVP4FarmPopUp != null)
		{
			this.mPVP4FarmPopUp.Show(mS2C_PvpFarm.Data);
		}
	}
}
