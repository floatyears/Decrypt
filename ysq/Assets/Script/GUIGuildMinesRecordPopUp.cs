using Att;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIGuildMinesRecordPopUp : GameUIBasePopup
{
	private UILabel mInvariable;

	private UILabel mVariable;

	private UILabel mTimes;

	private GUIGuildMinesRecordTable mContentTable;

	private MS2C_QueryMyOreData mData;

	public static void Show()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildMinesRecordPopUp, false, null, null);
	}

	public static void TryClose()
	{
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIGuildMinesRecordPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(false, null);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(1031, new ClientSession.MsgHandler(this.OnMsgBuyOreRevengeCount));
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), parent);
		this.mInvariable = GameUITools.FindUILabel("Invariable/Value", parent);
		this.mVariable = GameUITools.FindUILabel("Variable/Value", parent);
		this.mTimes = GameUITools.FindUILabel("Times", GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnAddClick), parent));
		this.mContentTable = GameUITools.FindGameObject("Panel/Content", parent).AddComponent<GUIGuildMinesRecordTable>();
		this.mContentTable.maxPerLine = 1;
		this.mContentTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentTable.cellWidth = 610f;
		this.mContentTable.cellHeight = 90f;
		this.mContentTable.Init(this);
		this.Init();
	}

	private void OnDestroy()
	{
		Globals.Instance.CliSession.Unregister(1031, new ClientSession.MsgHandler(this.OnMsgBuyOreRevengeCount));
	}

	public void Init()
	{
		this.mData = Globals.Instance.Player.GuildSystem.MyOreData;
		if (this.mData == null)
		{
			this.Close();
		}
		this.mInvariable.text = this.mData.Amount1.ToString();
		this.mVariable.text = this.mData.Amount2.ToString();
		this.mTimes.text = this.mData.RevengeCount.ToString();
		foreach (OrePillageRecord current in this.mData.Data)
		{
			this.mContentTable.AddData(new GUIGuildMinesRecordData(current));
		}
	}

	private void Close()
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnMsgBuyRevenge()
	{
		this.mTimes.text = this.mData.RevengeCount.ToString();
	}

	private void OnAddClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
		if (this.mData.BuyRevengeCount >= vipLevelInfo.BuyRevengeCount)
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
					if (info.BuyRevengeCount > vipLevelInfo.BuyRevengeCount)
					{
						vipLevelInfo2 = info;
						break;
					}
				}
				if (vipLevelInfo2 != null)
				{
					GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("guildMines23"), vipLevelInfo2.ID, vipLevelInfo2.BuyRevengeCount));
				}
				else
				{
					GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildMines20"), MessageBox.Type.OK, null);
				}
			}
			return;
		}
		int num = this.mData.BuyRevengeCount + 1;
		if (num > MiscTable.MaxBuyOreRevengeCountCostID)
		{
			num = MiscTable.MaxBuyOreRevengeCountCostID;
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
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, info2.BuyOreRevengeCountCost, 0))
		{
			string @string = Singleton<StringManager>.Instance.GetString("guildMines12", new object[]
			{
				(Globals.Instance.Player.Data.Diamond >= info2.BuyOreRevengeCountCost) ? "[00ff00]" : "[ff0000]",
				info2.BuyOreRevengeCountCost
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.Custom2Btn, null);
			gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("OKBuy");
			GameMessageBox expr_22B = gameMessageBox;
			expr_22B.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_22B.OkClick, new MessageBox.MessageDelegate(this.OnOkBuyRevengeCount));
		}
	}

	private void OnOkBuyRevengeCount(object obj)
	{
		MC2S_BuyOreRevengeCount ojb = new MC2S_BuyOreRevengeCount();
		Globals.Instance.CliSession.Send(1030, ojb);
	}

	public void OnOrePillageClick(GUIGuildMinesRecordData data)
	{
		if (this.mData.RevengeCount <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildMines16", 0f, 0f);
			return;
		}
		MC2S_OrePillageStart mC2S_OrePillageStart = new MC2S_OrePillageStart();
		mC2S_OrePillageStart.TargetID = data.mRecord.GUID;
		mC2S_OrePillageStart.Amount = data.mRecord.PillageCount;
		mC2S_OrePillageStart.Flag = true;
		Globals.Instance.CliSession.Send(1026, mC2S_OrePillageStart);
		GameUIState uiState = GameUIManager.mInstance.uiState;
		LocalPlayer player = Globals.Instance.Player;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.SetOldFurtherData(player.TeamSystem.GetPet(0));
		GameUIPopupManager.GetInstance().PopState(true, null);
	}

	private void OnCloseClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}

	private void OnMsgBuyOreRevengeCount(MemoryStream stream)
	{
		MS2C_BuyOreRevengeCount mS2C_BuyOreRevengeCount = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyOreRevengeCount), stream) as MS2C_BuyOreRevengeCount;
		if (mS2C_BuyOreRevengeCount.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_BuyOreRevengeCount.Result);
			return;
		}
		this.mData.RevengeCount++;
		this.mData.BuyRevengeCount++;
		this.mTimes.text = this.mData.RevengeCount.ToString();
	}
}
