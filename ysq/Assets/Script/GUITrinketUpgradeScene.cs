using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUITrinketUpgradeScene : GameUISession
{
	public enum EUpgradeType
	{
		EUT_Enhance,
		EUT_Refine
	}

	private static GUITrinketUpgradeScene.EUpgradeType mType;

	private static ItemDataEx mData;

	private static int mCurSelectIndex = -1;

	public SocketDataEx mSocketData;

	public TrinketEnhanceLayer mTrinketEnhanceLayer;

	private TrinketRefineLayer mTrinketRefineLayer;

	private UISprite mFade;

	public ItemDataEx mEquipData
	{
		get
		{
			return GUITrinketUpgradeScene.mData;
		}
	}

	public static void Change2This(ItemDataEx data, GUITrinketUpgradeScene.EUpgradeType type = GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance, int selectIndex = -1)
	{
		if (data == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDataEx is null"
			});
			return;
		}
		GUITrinketUpgradeScene.mData = data;
		GUITrinketUpgradeScene.mType = type;
		if (selectIndex != -2)
		{
			GUITrinketUpgradeScene.mCurSelectIndex = selectIndex;
		}
		GameUIManager.mInstance.ChangeSession<GUITrinketUpgradeScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("equipImprove30");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		this.CreateObjects();
		Globals.Instance.CliSession.Register(525, new ClientSession.MsgHandler(this.mTrinketEnhanceLayer.OnMsgTrinketEnhance));
		Globals.Instance.CliSession.Register(527, new ClientSession.MsgHandler(this.mTrinketRefineLayer.OnMsgTrinketRefine));
		LocalPlayer expr_82 = Globals.Instance.Player;
		expr_82.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_82.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GUIUpgradeTipPopUp.TryClose();
		Globals.Instance.CliSession.Unregister(525, new ClientSession.MsgHandler(this.mTrinketEnhanceLayer.OnMsgTrinketEnhance));
		Globals.Instance.CliSession.Unregister(527, new ClientSession.MsgHandler(this.mTrinketRefineLayer.OnMsgTrinketRefine));
		LocalPlayer expr_5E = Globals.Instance.Player;
		expr_5E.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_5E.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void CreateObjects()
	{
		this.mFade = GameUITools.FindUISprite("Fade", base.gameObject);
		this.mSocketData = Globals.Instance.Player.TeamSystem.GetSocket(this.mEquipData.GetSocketSlot());
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mTrinketEnhanceLayer = GameUITools.FindGameObject("EnhanceLayer", parent).AddComponent<TrinketEnhanceLayer>();
		this.mTrinketEnhanceLayer.InitWithBaseScene(this);
		this.mTrinketRefineLayer = GameUITools.FindGameObject("RefineLayer", parent).AddComponent<TrinketRefineLayer>();
		this.mTrinketRefineLayer.InitWithBaseScene(this);
		UIToggle component = GameUITools.FindGameObject("EnhanceTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_C4 = UIEventListener.Get(component.gameObject);
		expr_C4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C4.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		UIToggle component2 = base.FindGameObject("RefineTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component2.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_11A = UIEventListener.Get(component2.gameObject);
		expr_11A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_11A.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		if (GUITrinketUpgradeScene.mType == GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance)
		{
			component.value = true;
			MC2S_TrinketEnhance trinketEnhanceData = GameUIManager.mInstance.uiState.TrinketEnhanceData;
			if (trinketEnhanceData != null && trinketEnhanceData.TrinketID == this.mEquipData.GetID())
			{
				List<ItemDataEx> list = new List<ItemDataEx>();
				foreach (ulong current in trinketEnhanceData.ItemID)
				{
					list.Add(Globals.Instance.Player.ItemSystem.GetItem(current));
				}
				this.mTrinketEnhanceLayer.Refresh(list);
				list = null;
			}
			else
			{
				GameUIManager.mInstance.uiState.TrinketEnhanceData = new MC2S_TrinketEnhance();
				GameUIManager.mInstance.uiState.TrinketEnhanceData.TrinketID = this.mEquipData.GetID();
				this.mTrinketEnhanceLayer.Refresh(null);
			}
		}
		else if (Globals.Instance.Player.ItemSystem.CanTrinketRefine())
		{
			if (GameUIManager.mInstance.uiState.TrinketEnhanceData == null)
			{
				GameUIManager.mInstance.uiState.TrinketEnhanceData = new MC2S_TrinketEnhance();
			}
			GameUIManager.mInstance.uiState.TrinketEnhanceData.TrinketID = this.mEquipData.GetID();
			component2.value = true;
			this.mTrinketRefineLayer.Refresh(false);
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				"trinket upgrade refine error"
			});
		}
		if (!Globals.Instance.Player.ItemSystem.CanTrinketRefine())
		{
			this.mTrinketRefineLayer.gameObject.SetActive(false);
			component2.activeSprite.alpha = 0f;
			component2.enabled = false;
			UIEventListener expr_305 = UIEventListener.Get(component2.gameObject);
			expr_305.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_305.onClick, new UIEventListener.VoidDelegate(this.OnRefineTabClick));
		}
	}

	private void OnBackClick(GameObject go)
	{
		GUITrinketUpgradeScene.mData = null;
		if (GUITrinketUpgradeScene.mCurSelectIndex != -1)
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = true;
			GameUIManager.mInstance.uiState.CombatPetSlot = GUITrinketUpgradeScene.mCurSelectIndex;
		}
		GameUIManager.mInstance.uiState.TrinketEnhanceData = null;
		GameUIManager.mInstance.GobackSession();
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                //if (GUITrinketUpgradeScene.<>f__switch$mapA == null)
                //{
                //    GUITrinketUpgradeScene.<>f__switch$mapA = new Dictionary<string, int>(2)
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
                //if (GUITrinketUpgradeScene.<>f__switch$mapA.TryGetValue(name, out num))
                //{
                //    if (num != 0)
                //    {
                //        if (num == 1)
                //        {
                //            GUITrinketUpgradeScene.mType = GUITrinketUpgradeScene.EUpgradeType.EUT_Refine;
                //            this.mTrinketRefineLayer.Refresh(false);
                //        }
                //    }
                //    else
                //    {
                //        GUITrinketUpgradeScene.mType = GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance;
                //        this.mTrinketEnhanceLayer.Refresh(null);
                //    }
                //}
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnRefineTabClick(GameObject go)
	{
		GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove44", new object[]
		{
			GameConst.GetInt32(13)
		}), 0f, 0f);
	}

	private void OnPlayerUpdateEvent()
	{
		base.Invoke("RefreshMoney", 0.01f);
		this.mTrinketRefineLayer.RefreshMoney();
	}

	private void RefreshMoney()
	{
		this.mTrinketEnhanceLayer.RefreshMoney();
	}

	public void PlayAnim()
	{
		this.mFade.enabled = true;
	}

	public void EndAnim()
	{
		this.mFade.enabled = false;
	}
}
