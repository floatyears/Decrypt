using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIEquipUpgradeScene : GameUISession
{
	public enum EUpgradeType
	{
		EUT_Enhance,
		EUT_Refine
	}

	private static GUIEquipUpgradeScene.EUpgradeType mType;

	private static ItemDataEx mData;

	private static int mCurSelectIndex = -1;

	public SocketDataEx mSocketData;

	public EquipEnhanceLayer mEquipEnhanceLayer;

	private EquipRefineLayer mEquipRefineLayer;

	public ItemDataEx mEquipData
	{
		get
		{
			return GUIEquipUpgradeScene.mData;
		}
	}

	public static void Change2This(ItemDataEx data, GUIEquipUpgradeScene.EUpgradeType type = GUIEquipUpgradeScene.EUpgradeType.EUT_Enhance, int selectIndex = -1)
	{
		if (data == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDataEx is null"
			});
			return;
		}
		GUIEquipUpgradeScene.mData = data;
		GUIEquipUpgradeScene.mType = type;
		GUIEquipUpgradeScene.mCurSelectIndex = selectIndex;
		GameUIManager.mInstance.ChangeSession<GUIEquipUpgradeScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("equipImprove19");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		this.CreateObjects();
		Globals.Instance.CliSession.Register(521, new ClientSession.MsgHandler(this.mEquipEnhanceLayer.OnMsgEquipEnhance));
		Globals.Instance.CliSession.Register(523, new ClientSession.MsgHandler(this.mEquipRefineLayer.OnMsgEquipRefine));
		LocalPlayer expr_82 = Globals.Instance.Player;
		expr_82.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_82.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GUIUpgradeTipPopUp.TryClose();
		Globals.Instance.CliSession.Unregister(521, new ClientSession.MsgHandler(this.mEquipEnhanceLayer.OnMsgEquipEnhance));
		Globals.Instance.CliSession.Unregister(523, new ClientSession.MsgHandler(this.mEquipRefineLayer.OnMsgEquipRefine));
		LocalPlayer expr_5E = Globals.Instance.Player;
		expr_5E.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_5E.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void OnBackClick(GameObject go)
	{
		GUIEquipUpgradeScene.mData = null;
		if (GUIEquipUpgradeScene.mCurSelectIndex != -1)
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = true;
			GameUIManager.mInstance.uiState.CombatPetSlot = GUIEquipUpgradeScene.mCurSelectIndex;
		}
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		if (this.mEquipData == null)
		{
			this.OnBackClick(null);
			return;
		}
		this.mSocketData = Globals.Instance.Player.TeamSystem.GetSocket(this.mEquipData.GetSocketSlot());
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mEquipEnhanceLayer = GameUITools.FindGameObject("EnhanceLayer", parent).AddComponent<EquipEnhanceLayer>();
		this.mEquipEnhanceLayer.InitWithBaseScene(this);
		this.mEquipRefineLayer = GameUITools.FindGameObject("RefineLayer", parent).AddComponent<EquipRefineLayer>();
		this.mEquipRefineLayer.InitWithBaseScene(this);
		UIToggle component = GameUITools.FindGameObject("EnhanceTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_C1 = UIEventListener.Get(component.gameObject);
		expr_C1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C1.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		UIToggle component2 = base.FindGameObject("RefineTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component2.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_117 = UIEventListener.Get(component2.gameObject);
		expr_117.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_117.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		if (GUIEquipUpgradeScene.mType == GUIEquipUpgradeScene.EUpgradeType.EUT_Enhance)
		{
			component.value = true;
			this.mEquipEnhanceLayer.Refresh();
		}
		else if (Globals.Instance.Player.ItemSystem.CanEquipRefine())
		{
			component2.value = true;
			this.mEquipRefineLayer.Refresh(true);
		}
		if (!Globals.Instance.Player.ItemSystem.CanEquipRefine())
		{
			this.mEquipRefineLayer.gameObject.SetActive(false);
			component2.activeSprite.alpha = 0f;
			component2.enabled = false;
			UIEventListener expr_1D1 = UIEventListener.Get(component2.gameObject);
			expr_1D1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1D1.onClick, new UIEventListener.VoidDelegate(this.OnRefineTabClick));
		}
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                //if (GUIEquipUpgradeScene.<>f__switch$map7 == null)
                //{
                //    GUIEquipUpgradeScene.<>f__switch$map7 = new Dictionary<string, int>(2)
                //    {
                //        {
                //            "EnhanceTab",
                //            0
                //        },
                //        {
                //            "RefineTab",
                //            1
                //        }
                //    };
                //}
                //int num;
                //if (GUIEquipUpgradeScene.<>f__switch$map7.TryGetValue(name, out num))
                //{
                //    if (num != 0)
                //    {
                //        if (num == 1)
                //        {
                //            if (Globals.Instance.Player.ItemSystem.CanEquipRefine())
                //            {
                //                GUIEquipUpgradeScene.mType = GUIEquipUpgradeScene.EUpgradeType.EUT_Refine;
                //                this.mEquipRefineLayer.Refresh(true);
                //            }
                //            else
                //            {
                //                GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove43", new object[]
                //                {
                //                    GameConst.GetInt32(11)
                //                }), 0f, 0f);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        GUIEquipUpgradeScene.mType = GUIEquipUpgradeScene.EUpgradeType.EUT_Enhance;
                //        this.mEquipEnhanceLayer.Refresh();
                //    }
                //}
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnRefineTabClick(GameObject go)
	{
		GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove43", new object[]
		{
			GameConst.GetInt32(11)
		}), 0f, 0f);
	}

	private void OnPlayerUpdateEvent()
	{
		this.mEquipEnhanceLayer.RefreshMoney();
	}
}
