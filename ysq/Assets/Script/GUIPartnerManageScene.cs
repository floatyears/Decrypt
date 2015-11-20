using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIPartnerManageScene : GameUISession
{
	public UIAtlas mAvatarIcon;

	public UIAtlas mPetIcon;

	private static bool IsFragmentLayer;

	[NonSerialized]
	public UIToggle mTab0;

	[NonSerialized]
	public PartnerManageTabLayer mPartnerManageTabLayer;

	[NonSerialized]
	public PartnerItemSliceTabLayer mPartnerItemSliceTabLayer;

	private UIToggle mTab1;

	private UILabel mFenJie;

	private UILabel mBornTxt;

	private GameObject mTabNew1;

	private GameObject mBornBtn;

	public static bool ShowRed()
	{
		return Globals.Instance.Player.ItemSystem.HasPetCreateItem();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/WindowBg");
		this.mTab0 = transform.Find("tab0").GetComponent<UIToggle>();
		EventDelegate.Add(this.mTab0.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_54 = UIEventListener.Get(this.mTab0.gameObject);
		expr_54.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_54.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mTab1 = transform.Find("tab1").GetComponent<UIToggle>();
		EventDelegate.Add(this.mTab1.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		this.mTabNew1 = this.mTab1.transform.Find("new").gameObject;
		this.mTabNew1.SetActive(Globals.Instance.Player.ItemSystem.HasPetCreateItem());
		UIEventListener expr_F7 = UIEventListener.Get(this.mTab1.gameObject);
		expr_F7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F7.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mFenJie = transform.Find("resolveBtn/fenjie").GetComponent<UILabel>();
		GameUITools.RegisterClickEvent("resolveBtn", new UIEventListener.VoidDelegate(this.OnResolveBtnClick), transform.gameObject);
		this.mBornTxt = transform.Find("bornBtn/born").GetComponent<UILabel>();
		this.mBornBtn = transform.Find("bornBtn").gameObject;
		UIEventListener expr_187 = UIEventListener.Get(this.mBornBtn.gameObject);
		expr_187.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_187.onClick, new UIEventListener.VoidDelegate(this.OnBornBtnClick));
		this.mPartnerManageTabLayer = transform.Find("page0").gameObject.AddComponent<PartnerManageTabLayer>();
		this.mPartnerManageTabLayer.InitWithBaseScene(this);
		this.mPartnerItemSliceTabLayer = transform.Find("page1").gameObject.AddComponent<PartnerItemSliceTabLayer>();
		this.mPartnerItemSliceTabLayer.InitWithBaseScene();
		if (GUIPartnerManageScene.IsFragmentLayer)
		{
			this.mTab1.value = true;
		}
		else
		{
			this.mTab0.value = true;
		}
	}

	private void Refresh()
	{
		this.mFenJie.text = Singleton<StringManager>.Instance.GetString("PetFurther3");
		this.mBornTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther4");
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		TopGoods expr_0C = topGoods;
		expr_0C.BackClickListener = (UIEventListener.VoidDelegate)Delegate.Combine(expr_0C.BackClickListener, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		topGoods.Show("teamate");
		this.CreateObjects();
		this.Refresh();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		PetSubSystem expr_62 = Globals.Instance.Player.PetSystem;
		expr_62.AddPetEvent = (PetSubSystem.AddPetCallback)Delegate.Combine(expr_62.AddPetEvent, new PetSubSystem.AddPetCallback(this.OnAddPetEvent));
		PetSubSystem expr_92 = Globals.Instance.Player.PetSystem;
		expr_92.RemovePetEvent = (PetSubSystem.RemovePetCallback)Delegate.Combine(expr_92.RemovePetEvent, new PetSubSystem.RemovePetCallback(this.OnRemovePetEvent));
		ItemSubSystem expr_C2 = Globals.Instance.Player.ItemSystem;
		expr_C2.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_C2.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_F2 = Globals.Instance.Player.ItemSystem;
		expr_F2.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_F2.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_122 = Globals.Instance.Player.ItemSystem;
		expr_122.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_122.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdateItemEvent));
		Globals.Instance.CliSession.Register(505, new ClientSession.MsgHandler(this.OnMsgSummonPet));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		PetSubSystem expr_1E = Globals.Instance.Player.PetSystem;
		expr_1E.AddPetEvent = (PetSubSystem.AddPetCallback)Delegate.Remove(expr_1E.AddPetEvent, new PetSubSystem.AddPetCallback(this.OnAddPetEvent));
		PetSubSystem expr_4E = Globals.Instance.Player.PetSystem;
		expr_4E.RemovePetEvent = (PetSubSystem.RemovePetCallback)Delegate.Remove(expr_4E.RemovePetEvent, new PetSubSystem.RemovePetCallback(this.OnRemovePetEvent));
		ItemSubSystem expr_7E = Globals.Instance.Player.ItemSystem;
		expr_7E.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_7E.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_AE = Globals.Instance.Player.ItemSystem;
		expr_AE.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_AE.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_DE = Globals.Instance.Player.ItemSystem;
		expr_DE.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_DE.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdateItemEvent));
		Globals.Instance.CliSession.Unregister(505, new ClientSession.MsgHandler(this.OnMsgSummonPet));
	}

	private void OnBackBtnClick(GameObject go)
	{
		GUIPartnerManageScene.IsFragmentLayer = false;
		GameUIManager.mInstance.uiState.CachePetBag = false;
		GameUIManager.mInstance.GobackSession();
	}

	public void OnResolveBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
	}

	private void OnBornBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetReborn);
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			string text = name;
			if (text != null)
			{
                //if (GUIPartnerManageScene.<>f__switch$mapF == null)
                //{
                //    GUIPartnerManageScene.<>f__switch$mapF = new Dictionary<string, int>(2)
                //    {
                //        {
                //            "tab0",
                //            0
                //        },
                //        {
                //            "tab1",
                //            1
                //        }
                //    };
                //}
                //int num;
                //if (GUIPartnerManageScene.<>f__switch$mapF.TryGetValue(text, out num))
                //{
                //    if (num != 0)
                //    {
                //        if (num == 1)
                //        {
                //            GUIPartnerManageScene.IsFragmentLayer = true;
                //        }
                //    }
                //    else
                //    {
                //        GUIPartnerManageScene.IsFragmentLayer = false;
                //    }
                //}
			}
			this.mTabNew1.SetActive(Globals.Instance.Player.ItemSystem.HasPetCreateItem());
		}
	}

	private void OnAddPetEvent(PetDataEx data)
	{
		this.mPartnerManageTabLayer.AddPetItem(data);
	}

	private void OnRemovePetEvent(ulong id)
	{
		this.mPartnerManageTabLayer.RemovePetItem(id);
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		this.mPartnerItemSliceTabLayer.AddItem(data);
		this.mTabNew1.SetActive(Globals.Instance.Player.ItemSystem.HasPetCreateItem());
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.mPartnerItemSliceTabLayer.RemoveItem(id);
		this.mTabNew1.SetActive(Globals.Instance.Player.ItemSystem.HasPetCreateItem());
	}

	private void OnUpdateItemEvent(ItemDataEx data)
	{
		this.mPartnerItemSliceTabLayer.UpdateItem(data);
		this.mTabNew1.SetActive(Globals.Instance.Player.ItemSystem.HasPetCreateItem());
	}

	private void OnMsgSummonPet(MemoryStream stream)
	{
		MS2C_SummonPet mS2C_SummonPet = Serializer.NonGeneric.Deserialize(typeof(MS2C_SummonPet), stream) as MS2C_SummonPet;
		if (mS2C_SummonPet.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_SummonPet.Result);
			return;
		}
		PetDataEx pet = Globals.Instance.Player.PetSystem.GetPet(mS2C_SummonPet.PetID);
		if (pet == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("GetPet error, id = {0}", mS2C_SummonPet.PetID)
			});
			return;
		}
		GetPetLayer.Show(pet, null, GetPetLayer.EGPL_ShowNewsType.Create);
	}
}
