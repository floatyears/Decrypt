using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquipSaleLayer : MonoBehaviour
{
	private GUIEquipSaleScene mBaseScene;

	private EquipSaleBagUITable mContentsTable;

	private bool IsInit = true;

	private UILabel mCount;

	private UILabel mWorth;

	private int count;

	private int worth;

	private uint breakMoneyWorth;

	public List<ItemDataEx> mCurSelectItems = new List<ItemDataEx>();

	public void InitWithBaseScene(GUIEquipSaleScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<EquipSaleBagUITable>();
		this.mContentsTable.maxPerLine = 2;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 442f;
		this.mContentsTable.cellHeight = 130f;
		this.mContentsTable.gapHeight = 8f;
		this.mContentsTable.gapWidth = 8f;
		this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUIEquipSaleBagItem");
		this.mCount = GameUITools.FindUILabel("Count/Value", base.gameObject);
		this.mWorth = GameUITools.FindUILabel("Worth/Value", base.gameObject);
		GameUITools.RegisterClickEvent("SaleBtn", new UIEventListener.VoidDelegate(this.OnSaleBtnClick), base.gameObject);
	}

	private void OnSaleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurSelectItems.Count < 1)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove40", 0f, 0f);
			return;
		}
		string @string = Singleton<StringManager>.Instance.GetString("equipImprove4");
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, null);
		GameMessageBox expr_5A = gameMessageBox;
		expr_5A.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_5A.OkClick, new MessageBox.MessageDelegate(this.OnOkClick));
		GameMessageBox expr_7C = gameMessageBox;
		expr_7C.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_7C.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
	}

	private void OnOkClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_SellItem mC2S_SellItem = new MC2S_SellItem();
		foreach (ItemDataEx current in this.mCurSelectItems)
		{
			mC2S_SellItem.AutoSellItemID.Add(current.GetID());
		}
		Globals.Instance.CliSession.Send(510, mC2S_SellItem);
	}

	private void OnCancelClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
	}

	public void OnMsgSellItem(MemoryStream stream)
	{
		MS2C_SellItem mS2C_SellItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_SellItem), stream) as MS2C_SellItem;
		if (mS2C_SellItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_SellItem.Result);
			return;
		}
		GUIRewardPanel.Show(new List<RewardData>
		{
			new RewardData
			{
				RewardType = 1,
				RewardValue1 = this.worth
			}
		}, Singleton<StringManager>.Instance.GetString("sellGet"), false, true, null, false);
		this.mCurSelectItems.Clear();
		this.count = 0;
		this.worth = 0;
		this.RefreshTxt();
	}

	public void Refresh()
	{
		if (this.IsInit)
		{
			this.IsInit = false;
			this.InitEquipSaleBagItems();
		}
		this.RefreshTxt();
	}

	public void AddItem(ItemDataEx data)
	{
		if (this.mCurSelectItems.Contains(data))
		{
			global::Debug.LogErrorFormat("already selected this item , ID : {0}", new object[]
			{
				data.GetID()
			});
			return;
		}
		this.mCurSelectItems.Add(data);
		this.count++;
		this.worth += data.GetPrice();
		uint[] array = null;
		uint num;
		uint num2;
		uint num3;
		data.GetEquipBreakValue(out num, out num2, out num3, out array);
		this.breakMoneyWorth += num3;
		this.RefreshTxt();
	}

	public void DeleteItem(ItemDataEx data)
	{
		if (!this.mCurSelectItems.Contains(data))
		{
			global::Debug.LogErrorFormat("do not contain this item , ID : {0}", new object[]
			{
				data.GetID()
			});
			return;
		}
		this.mCurSelectItems.Remove(data);
		this.count--;
		this.worth -= data.GetPrice();
		uint[] array = null;
		uint num;
		uint num2;
		uint num3;
		data.GetEquipBreakValue(out num, out num2, out num3, out array);
		this.breakMoneyWorth -= num3;
		this.RefreshTxt();
	}

	private void RefreshTxt()
	{
		this.mCount.text = this.count.ToString();
		this.mWorth.text = this.worth.ToString();
	}

	public void RemoveItem(ulong id)
	{
		this.mContentsTable.RemoveData(id);
	}

	private void InitEquipSaleBagItems()
	{
		this.mContentsTable.ClearData();
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 0 && !current.IsEquiped())
			{
				current.IsSelected = false;
				this.mContentsTable.AddData(current);
			}
		}
	}
}
