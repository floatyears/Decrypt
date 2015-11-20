using Proto;
using System;
using UnityEngine;

public class GUISelectLopetBagScene : GameUISession
{
	private int lopetSlot = 4;

	private SelectLopetLayer mSelectLopetLayer;

	private UIToggle mFilter;

	public static void Show()
	{
		GameUIManager.mInstance.ChangeSession<GUISelectLopetBagScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("Lopet2");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		this.CreateObjects();
		LopetSubSystem expr_3D = Globals.Instance.Player.LopetSystem;
		expr_3D.SetCombatLopetEvent = (LopetSubSystem.VoidCallback)Delegate.Combine(expr_3D.SetCombatLopetEvent, new LopetSubSystem.VoidCallback(this.OnMsgSetCombatLopetEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		LopetSubSystem expr_14 = Globals.Instance.Player.LopetSystem;
		expr_14.SetCombatLopetEvent = (LopetSubSystem.VoidCallback)Delegate.Remove(expr_14.SetCombatLopetEvent, new LopetSubSystem.VoidCallback(this.OnMsgSetCombatLopetEvent));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void OnBackClick(GameObject go)
	{
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = this.lopetSlot;
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mSelectLopetLayer = GameUITools.FindGameObject("LopetLayer", parent).AddComponent<SelectLopetLayer>();
		this.mSelectLopetLayer.InitWithBaseScene(this);
		this.mFilter = GameUITools.RegisterClickEvent("Filter", new UIEventListener.VoidDelegate(this.OnFilterClick), parent).GetComponent<UIToggle>();
		this.mSelectLopetLayer.Refresh();
	}

	public void SendEquipItemMsg(LopetDataEx data)
	{
		if (data == null)
		{
			return;
		}
		MC2S_LopetSetCombat mC2S_LopetSetCombat = new MC2S_LopetSetCombat();
		mC2S_LopetSetCombat.LopetID = data.GetID();
		Globals.Instance.CliSession.Send(1060, mC2S_LopetSetCombat);
	}

	private void OnMsgSetCombatLopetEvent()
	{
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = this.lopetSlot;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	private void OnFilterClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.mSelectLopetLayer.ReInit(this.mFilter.value);
	}
}
