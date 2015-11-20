using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIKingRewardScene : GameUISession
{
	private enum EUIPopupState
	{
		EPS_NewGame,
		EPS_Max
	}

	private UILabel mCounter;

	private UILabel mCostValue;

	private KingRewardQuestItem[] mQuests = new KingRewardQuestItem[3];

	public ActivityValueData mActivityValueData;

	private int curPopupState;

	public static void TryOpen()
	{
		if (Globals.Instance.Player.PetSystem.Values.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt18", 0f, 0f);
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(2)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(2)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUIKingRewardScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("activityKingRewardTitle");
		topGoods.SetGoodsSlot(new TopGoods.EGoodsUIType[]
		{
			TopGoods.EGoodsUIType.EGT_UIDiamond,
			TopGoods.EGoodsUIType.EGT_UIEnergy,
			TopGoods.EGoodsUIType.EGT_UIKingRewardMedal
		});
		this.CreateObjects();
		Globals.Instance.CliSession.Register(642, new ClientSession.MsgHandler(this.OnMsgGetKRData));
		Globals.Instance.CliSession.Register(644, new ClientSession.MsgHandler(this.OnMsgOneKeyKR));
		LocalPlayer player = Globals.Instance.Player;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		this.Refresh(false);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		topGoods.DefaultGoodsSlot();
		Globals.Instance.CliSession.Unregister(642, new ClientSession.MsgHandler(this.OnMsgGetKRData));
		Globals.Instance.CliSession.Unregister(644, new ClientSession.MsgHandler(this.OnMsgOneKeyKR));
	}

	protected void CreateObjects()
	{
		this.mActivityValueData = Globals.Instance.Player.ActivitySystem.GetValueMod(3);
		GameObject parent = GameUITools.FindGameObject("Window", base.gameObject);
		this.mCounter = GameUITools.FindUILabel("Counter", parent);
		this.mCostValue = GameUITools.FindUILabel("Cost/Value", parent);
		Transform transform = GameUITools.FindGameObject("Quests", parent).transform;
		if (transform.childCount != 3)
		{
			global::Debug.LogErrorFormat("Activity KingReward QuestItem Num Error ,NUM :{0}", new object[]
			{
				transform.childCount
			});
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			this.mQuests[i] = transform.GetChild(i).gameObject.AddComponent<KingRewardQuestItem>();
			this.mQuests[i].Init(this);
		}
		GameUITools.RegisterClickEvent("ShopBtn", new UIEventListener.VoidDelegate(this.OnShopBtnClick), parent);
		GameUITools.RegisterClickEvent("RefreshBtn", new UIEventListener.VoidDelegate(this.OnRefreshBtnClick), parent);
	}

	public static bool CanTakePartIn()
	{
		return Tools.CanPlay(GameConst.GetInt32(2), true) && (Globals.Instance.Player.Data.RedFlag & 4096) != 0;
	}

	public void Refresh(bool isRefresh)
	{
		MC2S_GetKRData mC2S_GetKRData = new MC2S_GetKRData();
		mC2S_GetKRData.Refresh = isRefresh;
		Globals.Instance.CliSession.Send(641, mC2S_GetKRData);
	}

	private void OnShopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShopScene.TryOpen(EShopType.EShop_KR);
	}

	private void OnRefreshBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!GUIKingRewardScene.CanTakePartIn())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityKingReward0", 0f, 0f);
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(145), 0))
		{
			bool flag = false;
			for (int i = 0; i < this.mQuests.Length; i++)
			{
				if (this.mQuests[i].questInfo.Star == 5)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("activityKingReward1"), MessageBox.Type.OKCancel, null);
				GameMessageBox expr_A7 = gameMessageBox;
				expr_A7.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_A7.OkClick, new MessageBox.MessageDelegate(this.RefreshQuests));
				GameMessageBox expr_C9 = gameMessageBox;
				expr_C9.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_C9.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
			}
			else
			{
				this.Refresh(true);
			}
		}
	}

	private void RefreshQuests(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Refresh(true);
	}

	private void OnCancelClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
	}

	private void OnMsgOneKeyKR(MemoryStream stream)
	{
		MS2C_OneKeyKR mS2C_OneKeyKR = Serializer.NonGeneric.Deserialize(typeof(MS2C_OneKeyKR), stream) as MS2C_OneKeyKR;
		if (mS2C_OneKeyKR.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_OneKeyKR.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_OneKeyKR.Result);
			return;
		}
		GUIKingRewardResultScene.ShowKingRewardResult(mS2C_OneKeyKR.Data);
	}

	public void OnMsgGetKRData(MemoryStream stream)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		MS2C_GetKRData mS2C_GetKRData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetKRData), stream) as MS2C_GetKRData;
		if (mS2C_GetKRData.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_GetKRData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_GetKRData.Result);
			return;
		}
		this.mCostValue.text = GameConst.GetInt32(146).ToString();
		this.mCounter.text = Singleton<StringManager>.Instance.GetString("activityKingRewardCounter", new object[]
		{
			mS2C_GetKRData.Count,
			GameConst.GetInt32(142)
		});
		this.mCounter.enabled = true;
		if (mS2C_GetKRData.QuestID.Count == 3 || mS2C_GetKRData.RewardID.Count == 3)
		{
			for (int i = 0; i < this.mQuests.Length; i++)
			{
				this.mQuests[i].Refresh(mS2C_GetKRData.QuestID[i], mS2C_GetKRData.RewardID[i]);
			}
		}
		else
		{
			global::Debug.LogErrorFormat("GetKRData Error , questCount:{0},rewardCount:{1}", new object[]
			{
				mS2C_GetKRData.QuestID.Count,
				mS2C_GetKRData.RewardID.Count
			});
		}
		this.OpenPopup();
	}

	public void PopupFinishEvent()
	{
		this.OpenPopup();
	}

	private void OpenPopup()
	{
		int num = this.curPopupState++;
		int num2 = num;
		if (num2 == 0)
		{
			if (GameUIManager.mInstance.uiState.UnlockNewGameLevel > 0)
			{
				GUIUnlockPopUp.Show(GameUIManager.mInstance.uiState.UnlockNewGameLevel, null, new GameUIPopupManager.PopClosedCallback(this.PopupFinishEvent));
			}
		}
	}
}
