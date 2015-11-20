using System;
using UnityEngine;

public class GUIEquipSaleScene : GameUISession
{
	public EquipSaleLayer mEquipSaleLayer;

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("equipImprove2");
		this.CreateObjects();
		Globals.Instance.CliSession.Register(511, new ClientSession.MsgHandler(this.mEquipSaleLayer.OnMsgSellItem));
		ItemSubSystem expr_50 = Globals.Instance.Player.ItemSystem;
		expr_50.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_50.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
		Globals.Instance.CliSession.Unregister(511, new ClientSession.MsgHandler(this.mEquipSaleLayer.OnMsgSellItem));
		ItemSubSystem expr_48 = Globals.Instance.Player.ItemSystem;
		expr_48.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_48.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		foreach (ItemDataEx current in this.mEquipSaleLayer.mCurSelectItems)
		{
			current.ClearUIData();
		}
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mEquipSaleLayer = GameUITools.FindGameObject("EquipLayer", parent).AddComponent<EquipSaleLayer>();
		this.mEquipSaleLayer.InitWithBaseScene(this);
		this.mEquipSaleLayer.Refresh();
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.mEquipSaleLayer.RemoveItem(id);
	}
}
