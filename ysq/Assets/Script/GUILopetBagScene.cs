using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUILopetBagScene : GameUISession
{
	private static int tabIndex;

	private LopetLayer mPetLayer;

	private LopetPropsLayer mPropsLayer;

	private LopetFragmentLayer mFragmentLayer;

	private LopetSetLayer mSetLayer;

	public UIToggle mPetTab;

	private UIToggle mPropsTab;

	private UIToggle mFragmentTab;

	private UIToggle mSetTab;

	private UISprite mFragmentNew;

	private static bool FragmentRed
	{
		get
		{
			return Globals.Instance.Player.ItemSystem.HasLopet2Create();
		}
	}

	public static void TryOpen()
	{
		if (!Tools.CanPlay(GameConst.GetInt32(201), true))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(201)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUILopetBagScene>(null, false, true);
	}

	public static bool ShowRed()
	{
		return GUILopetBagScene.FragmentRed;
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		TopGoods expr_0C = topGoods;
		expr_0C.BackClickListener = (UIEventListener.VoidDelegate)Delegate.Combine(expr_0C.BackClickListener, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		topGoods.Show("LopetLb");
		this.CreateObjects();
		Globals.Instance.CliSession.Register(1077, new ClientSession.MsgHandler(this.OnMsgLopetAdd));
		LopetSubSystem expr_6D = Globals.Instance.Player.LopetSystem;
		expr_6D.AddLopetEvent = (LopetSubSystem.AddLopetCallback)Delegate.Combine(expr_6D.AddLopetEvent, new LopetSubSystem.AddLopetCallback(this.OnAddLopetEvent));
		LopetSubSystem expr_9D = Globals.Instance.Player.LopetSystem;
		expr_9D.RemoveLopetEvent = (LopetSubSystem.RemoveLopetCallback)Delegate.Combine(expr_9D.RemoveLopetEvent, new LopetSubSystem.RemoveLopetCallback(this.OnRemoveLopetDataEvent));
		ItemSubSystem expr_CD = Globals.Instance.Player.ItemSystem;
		expr_CD.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_CD.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_FD = Globals.Instance.Player.ItemSystem;
		expr_FD.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_FD.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_12D = Globals.Instance.Player.ItemSystem;
		expr_12D.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_12D.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		Globals.Instance.CliSession.Unregister(1077, new ClientSession.MsgHandler(this.OnMsgLopetAdd));
		LopetSubSystem expr_2F = Globals.Instance.Player.LopetSystem;
		expr_2F.AddLopetEvent = (LopetSubSystem.AddLopetCallback)Delegate.Remove(expr_2F.AddLopetEvent, new LopetSubSystem.AddLopetCallback(this.OnAddLopetEvent));
		LopetSubSystem expr_5F = Globals.Instance.Player.LopetSystem;
		expr_5F.RemoveLopetEvent = (LopetSubSystem.RemoveLopetCallback)Delegate.Remove(expr_5F.RemoveLopetEvent, new LopetSubSystem.RemoveLopetCallback(this.OnRemoveLopetDataEvent));
		ItemSubSystem expr_8F = Globals.Instance.Player.ItemSystem;
		expr_8F.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_8F.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_BF = Globals.Instance.Player.ItemSystem;
		expr_BF.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_BF.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_EF = Globals.Instance.Player.ItemSystem;
		expr_EF.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_EF.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdataItemEvent));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void OnBackBtnClick(GameObject go)
	{
		GUILopetBagScene.tabIndex = 0;
		GameUIManager.mInstance.uiState.CacheLopetBag = false;
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mPetLayer = GameUITools.FindGameObject("PetLayer", parent).AddComponent<LopetLayer>();
		this.mPropsLayer = GameUITools.FindGameObject("PropsLayer", parent).AddComponent<LopetPropsLayer>();
		this.mFragmentLayer = GameUITools.FindGameObject("FragmentLayer", parent).AddComponent<LopetFragmentLayer>();
		this.mSetLayer = GameUITools.FindGameObject("SetLayer", parent).AddComponent<LopetSetLayer>();
		this.mPetLayer.Init(this);
		this.mPropsLayer.Init(this);
		this.mFragmentLayer.Init(this);
		this.mSetLayer.Init(this);
		this.mPetTab = GameUITools.FindGameObject("PetTab", parent).GetComponent<UIToggle>();
		this.mPropsTab = GameUITools.FindGameObject("PropsTab", parent).GetComponent<UIToggle>();
		this.mFragmentTab = GameUITools.FindGameObject("FragmentTab", parent).GetComponent<UIToggle>();
		this.mFragmentNew = GameUITools.FindUISprite("Red", this.mFragmentTab.gameObject);
		this.mSetTab = GameUITools.FindGameObject("SetTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mPetTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_139 = UIEventListener.Get(this.mPetTab.gameObject);
		expr_139.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_139.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		EventDelegate.Add(this.mPropsTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_187 = UIEventListener.Get(this.mPropsTab.gameObject);
		expr_187.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_187.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		EventDelegate.Add(this.mFragmentTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_1D5 = UIEventListener.Get(this.mFragmentTab.gameObject);
		expr_1D5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1D5.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		EventDelegate.Add(this.mSetTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_223 = UIEventListener.Get(this.mSetTab.gameObject);
		expr_223.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_223.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		switch (GUILopetBagScene.tabIndex)
		{
		case 0:
			this.mPetTab.value = true;
			break;
		case 1:
			this.mPropsTab.value = true;
			break;
		case 2:
			this.mFragmentTab.value = true;
			break;
		}
		this.mSetTab.gameObject.SetActive(false);
		this.mSetLayer.gameObject.SetActive(false);
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			switch (name)
			{
			case "PetTab":
				this.mPetLayer.Refresh();
				GUILopetBagScene.tabIndex = 0;
				break;
			case "PropsTab":
				this.mPropsLayer.Refresh();
				GUILopetBagScene.tabIndex = 1;
				break;
			case "FragmentTab":
				this.mFragmentLayer.Refresh();
				GUILopetBagScene.tabIndex = 2;
				break;
			case "SetTab":
				this.mSetLayer.Refresh();
				GUILopetBagScene.tabIndex = 3;
				break;
			}
			this.RefreshNewMark();
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void RefreshNewMark()
	{
		this.mFragmentNew.enabled = GUILopetBagScene.FragmentRed;
	}

	private void OnAddLopetEvent(LopetDataEx data)
	{
		this.mPetLayer.AddItem(data);
	}

	private void OnRemoveLopetDataEvent(ulong id)
	{
		this.mPetLayer.RemoveItem(id);
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		this.mPropsLayer.AddItem(data);
		this.mFragmentLayer.AddItem(data);
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.mPropsLayer.RemoveItem(id);
		this.mFragmentLayer.RemoveItem(id);
	}

	private void OnUpdataItemEvent(ItemDataEx data)
	{
		this.mPropsLayer.UpdataItem(data);
		this.mFragmentLayer.UpdataItem(data);
	}

	public void SendLopetCreateMsg(ItemDataEx data)
	{
		if (Tools.IsLopetBagFull())
		{
			return;
		}
		MC2S_LopetSummon mC2S_LopetSummon = new MC2S_LopetSummon();
		mC2S_LopetSummon.ItemID = data.GetID();
		Globals.Instance.CliSession.Send(1076, mC2S_LopetSummon);
	}

	private void OnMsgLopetAdd(MemoryStream stream)
	{
		MS2C_LopetSummon mS2C_LopetSummon = Serializer.NonGeneric.Deserialize(typeof(MS2C_LopetSummon), stream) as MS2C_LopetSummon;
		if (mS2C_LopetSummon.Result != ELopetResult.ELR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("LopetR", (int)mS2C_LopetSummon.Result);
			return;
		}
		this.RefreshNewMark();
		GUISummonLopetSuccess.Show(Globals.Instance.Player.LopetSystem.GetLopet(mS2C_LopetSummon.LopetID));
	}
}
