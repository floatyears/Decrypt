using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIEquipBagScene : GameUISession
{
	private static bool IsFragmentLayer;

	public AnimationCurve Anim;

	[NonSerialized]
	public float Dura = 0.13f;

	[NonSerialized]
	public EquipLayer mEquipLayer;

	[NonSerialized]
	public EquipFragmentLayer mEquipFragmentLayer;

	[NonSerialized]
	public UIToggle mEquipTab;

	private UIToggle mFragmentTab;

	private UISprite mFragmentTabNewMark;

	public static bool ShowRed()
	{
		return Globals.Instance.Player.ItemSystem.HasEquip2Create();
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		TopGoods expr_0C = topGoods;
		expr_0C.BackClickListener = (UIEventListener.VoidDelegate)Delegate.Combine(expr_0C.BackClickListener, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		topGoods.Show("equipLb");
		this.CreateObjects();
		Globals.Instance.CliSession.Register(503, new ClientSession.MsgHandler(this.OnMsgEquipCreate));
		ItemSubSystem expr_6D = Globals.Instance.Player.ItemSystem;
		expr_6D.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_6D.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_9D = Globals.Instance.Player.ItemSystem;
		expr_9D.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_9D.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_CD = Globals.Instance.Player.ItemSystem;
		expr_CD.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_CD.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		Globals.Instance.CliSession.Unregister(503, new ClientSession.MsgHandler(this.OnMsgEquipCreate));
		GameUIManager.mInstance.GetTopGoods().Hide();
		ItemSubSystem expr_43 = Globals.Instance.Player.ItemSystem;
		expr_43.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_43.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_73 = Globals.Instance.Player.ItemSystem;
		expr_73.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_73.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_A3 = Globals.Instance.Player.ItemSystem;
		expr_A3.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_A3.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
	}

	private void OnBackBtnClick(GameObject go)
	{
		GUIEquipBagScene.IsFragmentLayer = false;
		GameUIManager.mInstance.uiState.CacheEquipBag = false;
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mEquipLayer = GameUITools.FindGameObject("EquipLayer", parent).AddComponent<EquipLayer>();
		this.mEquipFragmentLayer = GameUITools.FindGameObject("EquipFragmentLayer", parent).AddComponent<EquipFragmentLayer>();
		this.mEquipLayer.InitWithBaseScene(this);
		this.mEquipFragmentLayer.InitWithBaseScene(this);
		this.mEquipTab = base.FindGameObject("EquipTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mEquipTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_99 = UIEventListener.Get(this.mEquipTab.gameObject);
		expr_99.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_99.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mFragmentTab = base.FindGameObject("EquipFragmentTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mFragmentTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_FE = UIEventListener.Get(this.mFragmentTab.gameObject);
		expr_FE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_FE.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mFragmentTabNewMark = GameUITools.FindUISprite("Red", this.mFragmentTab.gameObject);
		GameUITools.RegisterClickEvent("SaleBtn", new UIEventListener.VoidDelegate(this.OnSaleBtnClick), parent);
		UISprite uISprite = GameUITools.FindUISprite("Red", GameUITools.RegisterClickEvent("BreakBtn", new UIEventListener.VoidDelegate(this.OnBreakBtnClick), parent));
		uISprite.enabled = false;
		if (GUIEquipBagScene.IsFragmentLayer)
		{
			this.mFragmentTab.value = true;
		}
		else
		{
			this.mEquipTab.value = true;
		}
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                //if (GUIEquipBagScene.<>f__switch$map5 == null)
                //{
                //    GUIEquipBagScene.<>f__switch$map5 = new Dictionary<string, int>(2)
                //    {
                //        {
                //            "EquipTab",
                //            0
                //        },
                //        {
                //            "EquipFragmentTab",
                //            1
                //        }
                //    };
                //}
                //int num;
                //if (GUIEquipBagScene.<>f__switch$map5.TryGetValue(name, out num))
                //{
                //    if (num != 0)
                //    {
                //        if (num == 1)
                //        {
                //            this.mEquipFragmentLayer.Refresh();
                //            GUIEquipBagScene.IsFragmentLayer = true;
                //        }
                //    }
                //    else
                //    {
                //        this.mEquipLayer.Refresh();
                //        GUIEquipBagScene.IsFragmentLayer = false;
                //    }
                //}
			}
			this.RefreshFragmentNewMark();
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void RefreshFragmentNewMark()
	{
		this.mFragmentTabNewMark.enabled = Globals.Instance.Player.ItemSystem.HasEquip2Create();
	}

	public void SendEquipCreateMsg(ItemDataEx data)
	{
		if (Tools.IsEquipBagFull())
		{
			return;
		}
		MC2S_EquipCreate mC2S_EquipCreate = new MC2S_EquipCreate();
		mC2S_EquipCreate.ItemID = data.GetID();
		Globals.Instance.CliSession.Send(502, mC2S_EquipCreate);
	}

	private void OnMsgEquipCreate(MemoryStream stream)
	{
		MS2C_EquipCreate mS2C_EquipCreate = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipCreate), stream) as MS2C_EquipCreate;
		if (mS2C_EquipCreate.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_EquipCreate.Result);
			return;
		}
		this.RefreshFragmentNewMark();
		GUIEquipInfoPopUp.ShowThis(Globals.Instance.Player.ItemSystem.GetItem(mS2C_EquipCreate.ItemID), GUIEquipInfoPopUp.EIPT.EIPT_View, -1, true, true);
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		this.mEquipLayer.AddItem(data);
		this.mEquipFragmentLayer.AddItem(data);
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.mEquipLayer.RemoveItem(id);
		this.mEquipFragmentLayer.RemoveItem(id);
	}

	private void OnUpdataItemEvent(ItemDataEx data)
	{
		this.mEquipLayer.UpdataItem(data);
		this.mEquipFragmentLayer.UpdataItem(data);
	}

	private void OnSaleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIEquipSaleScene>(null, false, false);
	}

	private void OnBreakBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak);
	}
}
