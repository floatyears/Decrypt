using System;
using System.Collections.Generic;
using UnityEngine;

public class GUITrinketBagScene : GameUISession
{
	public TrinketLayer mTrinketLayer;

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		TopGoods expr_0C = topGoods;
		expr_0C.BackClickListener = (UIEventListener.VoidDelegate)Delegate.Combine(expr_0C.BackClickListener, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		topGoods.Show("shengQiLb");
		this.CreateObjects();
		ItemSubSystem expr_4D = Globals.Instance.Player.ItemSystem;
		expr_4D.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_4D.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_7D = Globals.Instance.Player.ItemSystem;
		expr_7D.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_7D.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_AD = Globals.Instance.Player.ItemSystem;
		expr_AD.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_AD.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
	}

	private void OnBackBtnClick(GameObject go)
	{
		GameUIManager.mInstance.uiState.CacheTrinketBag = false;
		GameUIManager.mInstance.GobackSession();
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
		ItemSubSystem expr_23 = Globals.Instance.Player.ItemSystem;
		expr_23.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_23.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_53 = Globals.Instance.Player.ItemSystem;
		expr_53.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_53.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_83 = Globals.Instance.Player.ItemSystem;
		expr_83.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_83.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mTrinketLayer = GameUITools.FindGameObject("TrinketLayer", parent).AddComponent<TrinketLayer>();
		this.mTrinketLayer.InitWithBaseScene(this);
		UIToggle component = GameUITools.FindGameObject("TrinketTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_67 = UIEventListener.Get(component.gameObject);
		expr_67.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_67.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		GameUITools.RegisterClickEvent("RebornBtn", new UIEventListener.VoidDelegate(this.OnRebornClick), parent);
		GameUITools.RegisterClickEvent("CastBtn", new UIEventListener.VoidDelegate(this.OnCastBtnClick), parent).SetActive(Tools.CanPlay(GameConst.GetInt32(186), true));
		this.mTrinketLayer.Refresh();
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                //if (GUITrinketBagScene.<>f__switch$map9 == null)
                //{
                //    GUITrinketBagScene.<>f__switch$map9 = new Dictionary<string, int>(1)
                //    {
                //        {
                //            "TrinketTab",
                //            0
                //        }
                //    };
                //}
                //int num;
                //if (GUITrinketBagScene.<>f__switch$map9.TryGetValue(name, out num))
                //{
                //    if (num == 0)
                //    {
                //        this.mTrinketLayer.Refresh();
                //    }
                //}
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnCastBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPillageScene.TryOpen(true);
	}

	private void OnRebornClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn);
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		this.mTrinketLayer.AddItem(data);
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.mTrinketLayer.RemoveItem(id);
	}

	private void OnUpdataItemEvent(ItemDataEx data)
	{
		this.mTrinketLayer.UpdataItem(data);
	}
}
