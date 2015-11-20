using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class GUITrailTowerSceneV2 : GameUISession
{
	public enum ESceneState
	{
		ESS_Easy,
		ESS_Hard
	}

	public delegate void SaoDangDoneCallback(int lvl);

	private const int mMonsterNum = 4;

	public GUITrailTowerSceneV2.SaoDangDoneCallback OnSaoDangDoneEvent;

	private Transform mContentPanelTransform;

	private GUITrailBranchItemTable mGUITrailBranchItemTable;

	private GameObject mFanBeiSp;

	private UILabel mTitle;

	private UILabel mExpNum;

	private UILabel mMoneyNum;

	private UILabel mTakeTimesLb;

	private UILabel mTakeTimesNum;

	private GameObject mJinduResetGo;

	private UILabel mJinDuCostNum;

	private GameObject mJinDuMianFeiReset;

	private UILabel mStateTip;

	private UILabel mSaoDangBtnLb;

	private GameObject mResetBtn;

	private GameObject mSaoDangBtn;

	private GameObject mBattleBtn;

	private GUITrailTowerMonster[] mMonsters = new GUITrailTowerMonster[4];

	private int[] mMonsterInfoId = new int[4];

	private float mTimerRefresh;

	private int mSaoDangBeginLvl;

	private bool mIsOnCDDownIng;

	private StringBuilder mSb = new StringBuilder(42);

	private string mCancelStr;

	private string mTrailTower1Str;

	private string mTrailTower2Str;

	private string mTrailTower3Str;

	private string mTrailTower4Str;

	private string mTrailTower5Str;

	public int mMaxBranchIndex
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
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(5)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(5)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUITrailTowerSceneV2>(null, false, true);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/winBg");
		GameObject gameObject = transform.Find("rulesBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnRulesBtnClick));
		GameObject gameObject2 = transform.Find("paiHangBtn").gameObject;
		UIEventListener expr_60 = UIEventListener.Get(gameObject2);
		expr_60.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_60.onClick, new UIEventListener.VoidDelegate(this.OnPaiHangBtnClick));
		GameObject gameObject3 = transform.Find("shopBtn").gameObject;
		UIEventListener expr_98 = UIEventListener.Get(gameObject3);
		expr_98.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_98.onClick, new UIEventListener.VoidDelegate(this.OnShopBtnClick));
		this.mContentPanelTransform = transform.Find("leftInfo/contentWidget/contentPanel");
		if ((float)Screen.width / (float)Screen.height >= 1.6f)
		{
			this.mContentPanelTransform.localPosition = new Vector3(250f, this.mContentPanelTransform.localPosition.y, this.mContentPanelTransform.localPosition.z);
		}
		this.mGUITrailBranchItemTable = transform.Find("leftInfo/contentWidget/contentPanel/contents").gameObject.AddComponent<GUITrailBranchItemTable>();
		this.mGUITrailBranchItemTable.maxPerLine = 1;
		this.mGUITrailBranchItemTable.arrangement = UICustomGrid.Arrangement.VerticalReverse;
		this.mGUITrailBranchItemTable.cellWidth = 324f;
		this.mGUITrailBranchItemTable.cellHeight = 580f;
		this.mGUITrailBranchItemTable.gapWidth = 0f;
		this.mGUITrailBranchItemTable.gapHeight = 0f;
		this.mGUITrailBranchItemTable.mDragEffect = UIScrollView.DragEffect.Momentum;
		this.mGUITrailBranchItemTable.InitWithBaseScene(this);
		Transform transform2 = transform.Find("rightInfoPanel/rightInfo");
		this.mTitle = transform2.Find("title").GetComponent<UILabel>();
		Transform transform3 = transform2.Find("bg");
		for (int i = 0; i < 4; i++)
		{
			this.mMonsters[i] = transform3.Find(string.Format("pet{0}", i)).gameObject.AddComponent<GUITrailTowerMonster>();
			this.mMonsters[i].InitWithBaseScene();
		}
		GameObject gameObject4 = transform3.Find("teamBtn").gameObject;
		UIEventListener expr_244 = UIEventListener.Get(gameObject4);
		expr_244.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_244.onClick, new UIEventListener.VoidDelegate(this.OnTeamBtnClick));
		this.mExpNum = transform3.Find("expBg/num").GetComponent<UILabel>();
		this.mFanBeiSp = transform3.Find("expBg/fanBei").gameObject;
		this.mMoneyNum = transform3.Find("moneyBg/num").GetComponent<UILabel>();
		this.mTakeTimesLb = transform3.Find("takeTimes").GetComponent<UILabel>();
		this.mTakeTimesNum = this.mTakeTimesLb.transform.Find("num").GetComponent<UILabel>();
		this.mJinduResetGo = transform3.Find("txt3").gameObject;
		this.mJinDuCostNum = this.mJinduResetGo.transform.Find("num").GetComponent<UILabel>();
		this.mJinDuMianFeiReset = transform3.Find("txt4").gameObject;
		this.mResetBtn = transform3.Find("resetBtn").gameObject;
		UIEventListener expr_351 = UIEventListener.Get(this.mResetBtn);
		expr_351.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_351.onClick, new UIEventListener.VoidDelegate(this.OnResetBtnClick));
		this.mSaoDangBtn = transform3.Find("saoDangBtn").gameObject;
		UIEventListener expr_394 = UIEventListener.Get(this.mSaoDangBtn);
		expr_394.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_394.onClick, new UIEventListener.VoidDelegate(this.OnSaoDangBtnClick));
		this.mSaoDangBtnLb = this.mSaoDangBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mBattleBtn = transform2.Find("battleBtn").gameObject;
		UIEventListener expr_3F7 = UIEventListener.Get(this.mBattleBtn);
		expr_3F7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3F7.onClick, new UIEventListener.VoidDelegate(this.OnBattleBtnClick));
		this.mStateTip = transform2.Find("stateTip").GetComponent<UILabel>();
		this.mCancelStr = Singleton<StringManager>.Instance.GetString("Cancel");
		this.mTrailTower1Str = Singleton<StringManager>.Instance.GetString("trailTower1");
		this.mTrailTower2Str = Singleton<StringManager>.Instance.GetString("trailTower2");
		this.mTrailTower3Str = Singleton<StringManager>.Instance.GetString("trailTower3");
		this.mTrailTower4Str = Singleton<StringManager>.Instance.GetString("trailTower4");
		this.mTrailTower5Str = Singleton<StringManager>.Instance.GetString("trailTower5");
	}

	protected override void OnPostLoadGUI()
	{
		GameUIManager.mInstance.uiState.TrailCurLvl = Globals.Instance.Player.Data.TrialWave;
		this.CreateObjects();
		this.Refresh();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("trailTower0");
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		LocalPlayer expr_5E = Globals.Instance.Player;
		expr_5E.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_5E.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		BillboardSubSystem expr_8E = Globals.Instance.Player.BillboardSystem;
		expr_8E.QueryTrialRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_8E.QueryTrialRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryTrialRankEvent));
		Globals.Instance.CliSession.Register(633, new ClientSession.MsgHandler(this.OnMsgTrialReset));
		Globals.Instance.CliSession.Register(635, new ClientSession.MsgHandler(this.OnMsgTrialFarmStart));
		Globals.Instance.CliSession.Register(637, new ClientSession.MsgHandler(this.OnMsgTrialFarmStop));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_Trial, 0f);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		LocalPlayer expr_19 = Globals.Instance.Player;
		expr_19.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_19.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		BillboardSubSystem expr_49 = Globals.Instance.Player.BillboardSystem;
		expr_49.QueryTrialRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_49.QueryTrialRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryTrialRankEvent));
		Globals.Instance.CliSession.Unregister(633, new ClientSession.MsgHandler(this.OnMsgTrialReset));
		Globals.Instance.CliSession.Unregister(635, new ClientSession.MsgHandler(this.OnMsgTrialFarmStart));
		Globals.Instance.CliSession.Unregister(637, new ClientSession.MsgHandler(this.OnMsgTrialFarmStop));
	}

	private void OnRulesBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIRuleInfoPopUp, false, null, null);
		GameUIRuleInfoPopUp gameUIRuleInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUIRuleInfoPopUp;
		gameUIRuleInfoPopUp.Refresh(Singleton<StringManager>.Instance.GetString("trailTower0"), Singleton<StringManager>.Instance.GetString("trailTower22", new object[]
		{
			GameConst.GetInt32(187)
		}));
	}

	private void OnPaiHangBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.Player.BillboardSystem.SendTrialRankRequest();
	}

	private void OnQueryTrialRankEvent()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp, false, null, null);
		GameUICommonBillboardPopUp gameUICommonBillboardPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUICommonBillboardPopUp;
		gameUICommonBillboardPopUp.InitBillboard("TrailRankItem");
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.TrialRankData.Count; i++)
		{
			RankData rankData = billboardSystem.TrialRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		gameUICommonBillboardPopUp.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.TrialRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.TrialRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("trailTower13")
			});
		}
		gameUICommonBillboardPopUp.Refresh(Singleton<StringManager>.Instance.GetString("trailTower18"), string.Empty, @string, null, string.Format("[9e865a]{0}[-]{1}", Singleton<StringManager>.Instance.GetString("BillboardTrialFloors"), Globals.Instance.Player.Data.TrialMaxWave));
	}

	private void OnShopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShopScene.TryOpen(EShopType.EShop_Trial);
	}

	private void OnTeamBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = 0;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	private void OnResetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((Globals.Instance.Player.Data.TrialOver != 0 || Globals.Instance.Player.Data.TrialWave == GameConst.GetInt32(187) || Globals.Instance.Player.Data.TrialWave == Globals.Instance.Player.Data.TrialMaxWave) && Globals.Instance.Player.Data.TrialFarmTimeStamp == 0 && GameConst.GetInt32(242) > Globals.Instance.Player.Data.TrialResetCount)
		{
			int num = Globals.Instance.Player.Data.TrialResetCount * 50;
			if (num == 0 || !Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, num, 0))
			{
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("trailTower8"), MessageBox.Type.OKCancel, null);
				GameMessageBox expr_FD = gameMessageBox;
				expr_FD.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_FD.OkClick, new MessageBox.MessageDelegate(this.OnSureResetClick));
			}
		}
	}

	private void OnSureResetClick(object obj)
	{
		MC2S_TrialReset ojb = new MC2S_TrialReset();
		Globals.Instance.CliSession.Send(632, ojb);
	}

	private void CancelSaoDang(object obj)
	{
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0)
		{
			MC2S_TrialFarmStop ojb = new MC2S_TrialFarmStop();
			Globals.Instance.CliSession.Send(636, ojb);
		}
	}

	public int GetCurSaoDangLvl()
	{
		int result = 0;
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0)
		{
			int num = Globals.Instance.Player.GetTimeStamp() - Globals.Instance.Player.Data.TrialFarmTimeStamp;
			result = Mathf.Min(Globals.Instance.Player.Data.TrialMaxWave, Globals.Instance.Player.Data.TrialWave + Mathf.FloorToInt((float)num / 30f));
		}
		return result;
	}

	private int GetCurSaoDangCD()
	{
		int result = 0;
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0)
		{
			int num = (Globals.Instance.Player.Data.TrialMaxWave - Globals.Instance.Player.Data.TrialWave) * 30;
			result = Mathf.Max(0, num - (Globals.Instance.Player.GetTimeStamp() - Globals.Instance.Player.Data.TrialFarmTimeStamp));
		}
		return result;
	}

	private void OnSaoDangBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp == 0)
		{
			if (Globals.Instance.Player.Data.TrialOver == 0 && Globals.Instance.Player.Data.TrialWave < Globals.Instance.Player.Data.TrialMaxWave)
			{
				GameUIManager.mInstance.uiState.TrailCurLvl = Globals.Instance.Player.Data.TrialWave;
				this.mSaoDangBeginLvl = Globals.Instance.Player.Data.TrialWave;
				if (Globals.Instance.Player.Data.VipLevel >= 9u)
				{
					MC2S_TrialFarmStop ojb = new MC2S_TrialFarmStop();
					Globals.Instance.CliSession.Send(636, ojb);
				}
				else
				{
					MC2S_TrialFarmStart ojb2 = new MC2S_TrialFarmStart();
					Globals.Instance.CliSession.Send(634, ojb2);
				}
			}
		}
		else
		{
			int num = Globals.Instance.Player.GetTimeStamp() - Globals.Instance.Player.Data.TrialFarmTimeStamp;
			int num2 = Mathf.Max(0, Mathf.Min(Globals.Instance.Player.Data.TrialMaxWave, Mathf.FloorToInt((float)num / 30f)));
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("trailTower6", new object[]
			{
				num2
			}), MessageBox.Type.OKCancel, null);
			GameMessageBox expr_189 = gameMessageBox;
			expr_189.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_189.OkClick, new MessageBox.MessageDelegate(this.CancelSaoDang));
		}
	}

	private void OnBattleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.TrialOver == 0 && Globals.Instance.Player.Data.TrialWave < GameConst.GetInt32(187) && Globals.Instance.Player.Data.TrialFarmTimeStamp == 0)
		{
			MC2S_TrialStart ojb = new MC2S_TrialStart();
			Globals.Instance.CliSession.Send(628, ojb);
		}
	}

	private void InitBranchs()
	{
		this.mGUITrailBranchItemTable.ClearData();
		if (Globals.Instance.Player.Data.TrialMaxWave == 0)
		{
			this.mMaxBranchIndex = 1;
			this.mGUITrailBranchItemTable.AddData(new GUITrailBranchItemData(1));
		}
		else
		{
			int num = 0;
			while (num <= Globals.Instance.Player.Data.TrialMaxWave && num < GameConst.GetInt32(187))
			{
				this.mMaxBranchIndex = num + 1;
				this.mGUITrailBranchItemTable.AddData(new GUITrailBranchItemData(num + 1));
				num += 5;
			}
		}
		base.StartCoroutine(this.UpdateBranchPos());
	}

	private void Refresh()
	{
		this.InitBranchs();
		this.RefreshTrailInfo();
	}

	private bool IsMonsterIdExist(int infoId)
	{
		bool result = false;
		for (int i = 0; i < 4; i++)
		{
			if (this.mMonsterInfoId[i] != 0 && this.mMonsterInfoId[i] == infoId)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void RefreshTrailInfo()
	{
		int num = (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0) ? this.GetCurSaoDangLvl() : Globals.Instance.Player.Data.TrialWave;
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp == 0)
		{
			num = Mathf.Min(GameConst.GetInt32(187), Mathf.Max(1, num + 1));
		}
		else
		{
			num = Mathf.Max(Mathf.Min(Globals.Instance.Player.Data.TrialMaxWave, num + 1), 1);
		}
		this.mTitle.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(this.mTrailTower1Str, num).ToString();
		TrialRespawnInfo info = Globals.Instance.AttDB.TrialRespawnDict.GetInfo(num);
		if (info != null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.mMonsterInfoId[i] = 0;
			}
			int j = 0;
			for (int k = 0; k < info.InfoID.Count; k++)
			{
				if (info.InfoID[k] != 0 && !this.IsMonsterIdExist(info.InfoID[k]))
				{
					MonsterInfo info2 = Globals.Instance.AttDB.MonsterDict.GetInfo(info.InfoID[k]);
					if (info2 != null && j < 4)
					{
						this.mMonsters[j].Refresh(info2);
						this.mMonsterInfoId[j] = info.InfoID[k];
						j++;
					}
				}
			}
			while (j < 4)
			{
				this.mMonsters[j].Refresh(null);
				j++;
			}
		}
		TrialInfo info3 = Globals.Instance.AttDB.TrialDict.GetInfo(num);
		if (info3 != null)
		{
			int num2 = info3.Value;
			ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(4);
			if (valueMod != null)
			{
				num2 = info3.Value * (valueMod.Value1 / 100);
				this.mFanBeiSp.SetActive(true);
			}
			else
			{
				this.mFanBeiSp.SetActive(false);
			}
			this.mExpNum.text = num2.ToString();
			this.mMoneyNum.text = info3.Money.ToString();
		}
		this.mStateTip.text = this.mTrailTower4Str;
		this.RefreshTakeTimesInfo();
	}

	private void OnCDDown()
	{
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0 && !this.mIsOnCDDownIng)
		{
			this.mIsOnCDDownIng = true;
			MC2S_TrialFarmStop ojb = new MC2S_TrialFarmStop();
			Globals.Instance.CliSession.Send(636, ojb);
		}
	}

	private void RefreshTakeTimesInfo()
	{
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0)
		{
			int curSaoDangCD = this.GetCurSaoDangCD();
			if (curSaoDangCD < 1)
			{
				this.OnCDDown();
			}
			this.mTakeTimesNum.text = Tools.FormatTime(curSaoDangCD);
			this.mTakeTimesLb.text = this.mTrailTower3Str;
			this.mStateTip.gameObject.SetActive(true);
			this.mBattleBtn.SetActive(false);
			this.mResetBtn.SetActive(false);
			this.mJinduResetGo.SetActive(false);
			this.mJinDuMianFeiReset.SetActive(false);
			this.mSaoDangBtn.SetActive(true);
			this.mSaoDangBtnLb.text = this.mCancelStr;
		}
		else
		{
			this.mTakeTimesLb.text = this.mTrailTower2Str;
			int trialResetCount = Globals.Instance.Player.Data.TrialResetCount;
			int num = GameConst.GetInt32(242) - trialResetCount;
			this.mSb.Remove(0, this.mSb.Length);
			if (num == 0)
			{
				this.mSb.Append("[ff0000]");
			}
			else
			{
				this.mSb.Append("[ffffff]");
			}
			this.mSb.Append(num).Append("[-]/").Append(GameConst.GetInt32(242));
			this.mTakeTimesNum.text = this.mSb.ToString();
			this.mStateTip.gameObject.SetActive(false);
			this.mBattleBtn.SetActive(Globals.Instance.Player.Data.TrialOver == 0 && Globals.Instance.Player.Data.TrialWave < GameConst.GetInt32(187));
			this.mResetBtn.SetActive((Globals.Instance.Player.Data.TrialOver != 0 || Globals.Instance.Player.Data.TrialWave == GameConst.GetInt32(187) || Globals.Instance.Player.Data.TrialWave == Globals.Instance.Player.Data.TrialMaxWave) && (Globals.Instance.Player.Data.TrialMaxWave > 1 || Globals.Instance.Player.Data.TrialOver != 0) && num > 0);
			if (Globals.Instance.Player.Data.TrialOver == 0 && Globals.Instance.Player.Data.TrialWave < Globals.Instance.Player.Data.TrialMaxWave)
			{
				this.mSaoDangBtn.SetActive(true);
				this.mSaoDangBtnLb.text = this.mTrailTower5Str;
			}
			else
			{
				this.mSaoDangBtn.SetActive(false);
			}
			if ((Globals.Instance.Player.Data.TrialOver != 0 || Globals.Instance.Player.Data.TrialWave == GameConst.GetInt32(187) || Globals.Instance.Player.Data.TrialWave == Globals.Instance.Player.Data.TrialMaxWave) && num > 0)
			{
				if (num < GameConst.GetInt32(242))
				{
					this.mJinduResetGo.SetActive(true);
					this.mJinDuMianFeiReset.SetActive(false);
					int diamond = Globals.Instance.Player.Data.Diamond;
					int num2 = trialResetCount * 50;
					this.mJinDuCostNum.text = ((diamond >= num2) ? this.mSb.Remove(0, this.mSb.Length).Append("[ffffff]").Append(num2).Append("[-]").ToString() : this.mSb.Remove(0, this.mSb.Length).Append("[ff0000]").Append(num2).Append("[-]").ToString());
				}
				else
				{
					this.mJinduResetGo.SetActive(false);
					this.mJinDuMianFeiReset.SetActive(true);
				}
			}
			else
			{
				this.mJinduResetGo.SetActive(false);
				this.mJinDuMianFeiReset.SetActive(false);
			}
		}
	}

	private void OnPlayerUpdateEvent()
	{
		this.Refresh();
	}

	private void Update()
	{
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (Time.time - this.mTimerRefresh > 1f)
		{
			this.mTimerRefresh = Time.time;
			this.RefreshTakeTimesInfo();
			int curSaoDangCD = this.GetCurSaoDangCD();
			if (curSaoDangCD > 0 && Globals.Instance.Player.Data.TrialFarmTimeStamp != 0)
			{
				int curSaoDangLvl = this.GetCurSaoDangLvl();
				if (GameUIManager.mInstance.uiState.TrailCurLvl + 1 != curSaoDangLvl)
				{
					GameUIManager.mInstance.uiState.TrailCurLvl = curSaoDangLvl - 1;
					this.RefreshTrailInfo();
					base.StartCoroutine(this.UpdateBranchPos());
					this.mGUITrailBranchItemTable.Refresh();
					if (this.OnSaoDangDoneEvent != null)
					{
						this.OnSaoDangDoneEvent(curSaoDangLvl);
					}
				}
			}
		}
	}

	private void OnMsgTrialReset(MemoryStream stream)
	{
		MS2C_TrialReset mS2C_TrialReset = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrialReset), stream) as MS2C_TrialReset;
		if (mS2C_TrialReset.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TrialReset.Result == 0)
		{
			this.Refresh();
		}
	}

	private void OnMsgTrialFarmStart(MemoryStream stream)
	{
		MS2C_TrialFarmStart mS2C_TrialFarmStart = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrialFarmStart), stream) as MS2C_TrialFarmStart;
		if (mS2C_TrialFarmStart.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
	}

	private void OnMsgTrialFarmStop(MemoryStream stream)
	{
		MS2C_TrialFarmStop mS2C_TrialFarmStop = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrialFarmStop), stream) as MS2C_TrialFarmStop;
		if (mS2C_TrialFarmStop.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TrialFarmStop.Result == 0)
		{
			this.mIsOnCDDownIng = false;
			this.Refresh();
			if (Globals.Instance.Player.Data.TrialWave > this.mSaoDangBeginLvl)
			{
				GUITrialRewardPopUp.ShowThis(this.mSaoDangBeginLvl + 1, Globals.Instance.Player.Data.TrialWave, false);
				this.mSaoDangBeginLvl = 0;
			}
		}
	}

	private int GetCurMaxLeafIndex()
	{
		int result = 5;
		if (this.mGUITrailBranchItemTable.mDatas.Count > 1)
		{
			GUITrailBranchItemData gUITrailBranchItemData = (GUITrailBranchItemData)this.mGUITrailBranchItemTable.mDatas[this.mGUITrailBranchItemTable.mDatas.Count - 1];
			if (gUITrailBranchItemData != null)
			{
				result = gUITrailBranchItemData.StartIndex + 4;
			}
		}
		return result;
	}

	[DebuggerHidden]
	private IEnumerator UpdateBranchPos()
	{
        return null;
        //GUITrailTowerSceneV2.<UpdateBranchPos>c__Iterator9C <UpdateBranchPos>c__Iterator9C = new GUITrailTowerSceneV2.<UpdateBranchPos>c__Iterator9C();
        //<UpdateBranchPos>c__Iterator9C.<>f__this = this;
        //return <UpdateBranchPos>c__Iterator9C;
	}
}
