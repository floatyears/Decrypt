using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIPropsBagScene : GameUISession
{
	private PropsLayer mPropsLayer;

	private AwakeItemsLayer mAwakeItemsLayer;

	private GUIAttributeTip mGUIAttributeTip;

	private UIToggle tab0;

	private UIToggle tab1;

	private ItemDataEx mCurData;

	private int mCurCount;

	public static void TryOpenAwakeItemsLayer()
	{
		GameUIManager.mInstance.ChangeSession<GUIPropsBagScene>(delegate(GUIPropsBagScene session)
		{
			session.tab1.value = true;
		}, false, true);
	}

	public static void TryOpenmPropsLayer()
	{
		GameUIManager.mInstance.ChangeSession<GUIPropsBagScene>(delegate(GUIPropsBagScene session)
		{
			session.tab0.value = true;
		}, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex = 0;
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		TopGoods expr_1C = topGoods;
		expr_1C.BackClickListener = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.BackClickListener, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		topGoods.Show("bagLb");
		this.CreateObjects();
		Globals.Instance.CliSession.Register(509, new ClientSession.MsgHandler(this.OnMsgOpenItem));
		Globals.Instance.CliSession.Register(539, new ClientSession.MsgHandler(this.OnMsgAwakeItemBreakUp));
		Globals.Instance.CliSession.Register(541, new ClientSession.MsgHandler(this.OnMsgOpenSelectBox));
		Globals.Instance.CliSession.Register(545, new ClientSession.MsgHandler(this.OnMsgOpenRewardBox));
		ItemSubSystem expr_DD = Globals.Instance.Player.ItemSystem;
		expr_DD.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_DD.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_10D = Globals.Instance.Player.ItemSystem;
		expr_10D.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_10D.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_13D = Globals.Instance.Player.ItemSystem;
		expr_13D.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_13D.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
		LocalPlayer expr_168 = Globals.Instance.Player;
		expr_168.UseItemEvent = (LocalPlayer.UseItemCallback)Delegate.Combine(expr_168.UseItemEvent, new LocalPlayer.UseItemCallback(this.OnUseItemEvent));
		LocalPlayer player = Globals.Instance.Player;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		if (this.mGUIAttributeTip != null)
		{
			this.mGUIAttributeTip.DestroySelf();
		}
		Globals.Instance.CliSession.Unregister(509, new ClientSession.MsgHandler(this.OnMsgOpenItem));
		Globals.Instance.CliSession.Unregister(539, new ClientSession.MsgHandler(this.OnMsgAwakeItemBreakUp));
		Globals.Instance.CliSession.Unregister(541, new ClientSession.MsgHandler(this.OnMsgOpenSelectBox));
		Globals.Instance.CliSession.Unregister(545, new ClientSession.MsgHandler(this.OnMsgOpenRewardBox));
		ItemSubSystem expr_B0 = Globals.Instance.Player.ItemSystem;
		expr_B0.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_B0.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_E0 = Globals.Instance.Player.ItemSystem;
		expr_E0.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_E0.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_110 = Globals.Instance.Player.ItemSystem;
		expr_110.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_110.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
		LocalPlayer expr_13B = Globals.Instance.Player;
		expr_13B.UseItemEvent = (LocalPlayer.UseItemCallback)Delegate.Remove(expr_13B.UseItemEvent, new LocalPlayer.UseItemCallback(this.OnUseItemEvent));
		GameUIManager.mInstance.GetTopGoods().Hide();
		GameUIManager.mInstance.uiState.CachePropsBag = false;
	}

	private void OnBackBtnClick(GameObject go)
	{
		GameUIManager.mInstance.uiState.CachePropsBag = false;
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mPropsLayer = GameUITools.FindGameObject("PropsLayer", parent).AddComponent<PropsLayer>();
		this.mPropsLayer.InitWithBaseScene(this);
		this.mAwakeItemsLayer = GameUITools.FindGameObject("AwakeItemsLayer", parent).AddComponent<AwakeItemsLayer>();
		this.mAwakeItemsLayer.InitWithBaseScene(this);
		this.tab0 = GameUITools.FindGameObject("PropsTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.tab0.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_98 = UIEventListener.Get(this.tab0.gameObject);
		expr_98.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_98.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.tab1 = GameUITools.FindGameObject("AwakeItemsTab", parent).GetComponent<UIToggle>();
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(26)))
		{
			this.tab1.gameObject.SetActive(false);
		}
		else
		{
			this.tab1.gameObject.SetActive(true);
			EventDelegate.Add(this.tab1.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_145 = UIEventListener.Get(this.tab1.gameObject);
			expr_145.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_145.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		}
	}

	private void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			if (UIToggle.current == this.tab0)
			{
				this.mPropsLayer.Refresh();
			}
			else if (UIToggle.current == this.tab1)
			{
				this.mAwakeItemsLayer.Refresh();
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		this.mPropsLayer.AddItem(data);
		this.mAwakeItemsLayer.AddItem(data);
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.mPropsLayer.RemoveItem(id);
		this.mAwakeItemsLayer.RemoveItem(id);
	}

	private void OnUpdataItemEvent(ItemDataEx data)
	{
		this.mPropsLayer.UpdataItem(data);
		this.mAwakeItemsLayer.UpdataItem(data);
	}

	private void OnUseItemEvent(int infoID, int value)
	{
		this.mGUIAttributeTip = GUIPropsBagScene.ShowItemUseTips(infoID, value, this.mCurCount);
		GUIMultiUsePopUp.TryClose();
	}

	public static GUIAttributeTip ShowItemUseTips(int infoID, int value, int count)
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemDict get info error , ID : {0}", new object[]
			{
				infoID
			});
			return null;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
		switch (info.Type)
		{
		case 2:
		{
			GUIAttributeTip result = null;
			List<string> list = new List<string>();
			switch (info.SubType)
			{
			case 0:
				list.Add(Tools.GetItemQualityColorHex(info.Quality) + Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
				{
					Singleton<StringManager>.Instance.GetString("energy"),
					info.Value1 * count
				}));
				result = GameUIManager.mInstance.ShowAttributeTip(list, 2f, 0.4f, 0f, 200f);
				break;
			case 1:
				list.Add(Tools.GetItemQualityColorHex(info.Quality) + Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
				{
					Singleton<StringManager>.Instance.GetString("stamina"),
					info.Value1 * count
				}));
				result = GameUIManager.mInstance.ShowAttributeTip(list, 2f, 0.4f, 0f, 200f);
				break;
			case 3:
				if (GameUIManager.mInstance.uiState.PlayerLevel < Globals.Instance.Player.Data.Level)
				{
					GameUILevelupPanel.GetInstance().Init(GameUIManager.mInstance.uiCamera.transform, null);
					LocalPlayer player = Globals.Instance.Player;
					GameUIState uiState = GameUIManager.mInstance.uiState;
					uiState.PlayerLevel = player.Data.Level;
					uiState.PlayerExp = player.Data.Exp;
					uiState.PlayerEnergy = player.Data.Energy;
					uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
				}
				break;
			case 4:
				GUIRewardPanel.Show(new RewardData
				{
					RewardType = 8,
					RewardValue1 = value
				}, null, false, true, null, false);
				break;
			case 5:
				list.Add(Tools.GetItemQualityColorHex(info.Quality) + Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
				{
					Singleton<StringManager>.Instance.GetString("noBattle"),
					Singleton<StringManager>.Instance.GetString("timeTxt2", new object[]
					{
						info.Value1 * count / 3600
					})
				}));
				result = GameUIManager.mInstance.ShowAttributeTip(list, 2f, 0.4f, 0f, 200f);
				break;
			case 6:
				GUIRewardPanel.Show(new RewardData
				{
					RewardType = 13,
					RewardValue1 = value
				}, null, false, true, null, false);
				break;
			}
			return result;
		}
		case 4:
			GUIRewardPanel.Show(new RewardData
			{
				RewardType = 3,
				RewardValue1 = info.ID,
				RewardValue2 = value
			}, null, false, true, null, false);
			break;
		}
		return null;
	}

	private void OnMsgOpenItem(MemoryStream stream)
	{
		MS2C_OpenItem mS2C_OpenItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_OpenItem), stream) as MS2C_OpenItem;
		if (mS2C_OpenItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_OpenItem.Result);
			return;
		}
		base.StartCoroutine(this.ShowRewards(mS2C_OpenItem));
		GUIMultiUsePopUp.TryClose();
	}

	[DebuggerHidden]
	private IEnumerator ShowRewards(MS2C_OpenItem reply)
	{
        return null;
        //GUIPropsBagScene.<ShowRewards>c__Iterator46 <ShowRewards>c__Iterator = new GUIPropsBagScene.<ShowRewards>c__Iterator46();
        //<ShowRewards>c__Iterator.reply = reply;
        //<ShowRewards>c__Iterator.<$>reply = reply;
        //return <ShowRewards>c__Iterator;
	}

	private void OnMsgAwakeItemBreakUp(MemoryStream stream)
	{
		MS2C_AwakeItemBreakUp mS2C_AwakeItemBreakUp = Serializer.NonGeneric.Deserialize(typeof(MS2C_AwakeItemBreakUp), stream) as MS2C_AwakeItemBreakUp;
		if (mS2C_AwakeItemBreakUp.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_AwakeItemBreakUp.Result);
			return;
		}
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.mCurData.Info.Quality + 1);
		if (info == null)
		{
			global::Debug.LogErrorFormat("QualitDict get info error , ID : {0}", new object[]
			{
				this.mCurData.Info.Quality + 1
			});
			return;
		}
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = 13,
			RewardValue1 = info.AwakeBreakupValue * this.mCurCount
		}, Singleton<StringManager>.Instance.GetString("openitem"), false, true, null, false);
		GUIMultiUsePopUp.TryClose();
	}

	private void OnMsgOpenSelectBox(MemoryStream stream)
	{
		MS2C_OpenSelectBox mS2C_OpenSelectBox = Serializer.NonGeneric.Deserialize(typeof(MS2C_OpenSelectBox), stream) as MS2C_OpenSelectBox;
		if (mS2C_OpenSelectBox.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_OpenSelectBox.Result);
			return;
		}
		int item = -1;
		switch (this.mCurCount)
		{
		case 0:
			item = this.mCurData.Info.Value1;
			break;
		case 1:
			item = this.mCurData.Info.Value2;
			break;
		case 2:
			item = this.mCurData.Info.Value3;
			break;
		case 3:
			item = this.mCurData.Info.Value4;
			break;
		}
		base.StartCoroutine(this.ShowRewards(new MS2C_OpenItem
		{
			PetInfoIDs = 
			{
				item
			}
		}));
		GUISelectBoxPopUp.TryClose();
	}

	private void OnMsgOpenRewardBox(MemoryStream stream)
	{
		MS2C_OpenRewardBox mS2C_OpenRewardBox = Serializer.NonGeneric.Deserialize(typeof(MS2C_OpenRewardBox), stream) as MS2C_OpenRewardBox;
		if (mS2C_OpenRewardBox.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_OpenRewardBox.Result);
			return;
		}
		GUIMultiUsePopUp.TryClose();
		GUIRewardPanel.Show(mS2C_OpenRewardBox.Data, Singleton<StringManager>.Instance.GetString("openitem"), false, true, null, true);
	}

	public void UseItem(ItemDataEx data)
	{
		if (data.Info.Type == 2)
		{
			switch (data.Info.SubType)
			{
			case 3:
			case 5:
				this.UseItem(data, 1);
				return;
			case 7:
				GUISelectBoxPopUp.Show(data, new GUIMultiUsePopUp.UseItemCallBack(this.UseItem));
				return;
			}
		}
		if (data.GetCount() > 1 || (data.Info.Type == 5 && data.Info.Quality >= 2))
		{
			GUIMultiUsePopUp.Show(data, new GUIMultiUsePopUp.UseItemCallBack(this.UseItem));
		}
		else
		{
			this.UseItem(data, 1);
		}
	}

	public void UseItem(ItemDataEx data, int count)
	{
		this.mCurData = data;
		this.mCurCount = count;
		switch (data.Info.Type)
		{
		case 2:
			switch (data.Info.SubType)
			{
			case 0:
			case 1:
			case 3:
			case 4:
			case 5:
			case 6:
			case 9:
			{
				MC2S_UseItem mC2S_UseItem = new MC2S_UseItem();
				mC2S_UseItem.ItemID = data.GetID();
				mC2S_UseItem.Count = count;
				Globals.Instance.CliSession.Send(516, mC2S_UseItem);
				break;
			}
			case 2:
			{
				MC2S_OpenItem mC2S_OpenItem = new MC2S_OpenItem();
				mC2S_OpenItem.ItemID = data.GetID();
				mC2S_OpenItem.Count = count;
				Globals.Instance.CliSession.Send(508, mC2S_OpenItem);
				break;
			}
			case 7:
			{
				MC2S_OpenSelectBox mC2S_OpenSelectBox = new MC2S_OpenSelectBox();
				mC2S_OpenSelectBox.ItemID = data.GetID();
				mC2S_OpenSelectBox.Index = count;
				Globals.Instance.CliSession.Send(540, mC2S_OpenSelectBox);
				break;
			}
			case 8:
			{
				MC2S_OpenRewardBox mC2S_OpenRewardBox = new MC2S_OpenRewardBox();
				mC2S_OpenRewardBox.ItemID = data.GetID();
				mC2S_OpenRewardBox.Count = count;
				Globals.Instance.CliSession.Send(544, mC2S_OpenRewardBox);
				break;
			}
			}
			break;
		case 5:
		{
			MC2S_AwakeItemBreakUp mC2S_AwakeItemBreakUp = new MC2S_AwakeItemBreakUp();
			mC2S_AwakeItemBreakUp.ID = data.GetID();
			mC2S_AwakeItemBreakUp.Count = count;
			Globals.Instance.CliSession.Send(538, mC2S_AwakeItemBreakUp);
			break;
		}
		}
	}
}
