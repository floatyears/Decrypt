using Proto;
using System;
using UnityEngine;

public class GUISelectEquipBagScene : GameUISession
{
	private static int mSocketSlot;

	private static int mEquipSlot;

	public SelectEquipLayer mSelectEquipLayer;

	private UIToggle mFilter;

	public static void Change2This(int socketSlot, int equipSlot)
	{
		GUISelectEquipBagScene.mSocketSlot = socketSlot;
		GUISelectEquipBagScene.mEquipSlot = equipSlot;
		GameUIManager.mInstance.ChangeSession<GUISelectEquipBagScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("equipImprove22");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		this.CreateObjects();
		TeamSubSystem expr_3D = Globals.Instance.Player.TeamSystem;
		expr_3D.EquipItemEvent = (TeamSubSystem.ItemUpdateCallback)Delegate.Combine(expr_3D.EquipItemEvent, new TeamSubSystem.ItemUpdateCallback(this.OnMsgEquipItemEvent));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		TeamSubSystem expr_14 = Globals.Instance.Player.TeamSystem;
		expr_14.EquipItemEvent = (TeamSubSystem.ItemUpdateCallback)Delegate.Remove(expr_14.EquipItemEvent, new TeamSubSystem.ItemUpdateCallback(this.OnMsgEquipItemEvent));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void OnBackClick(GameObject go)
	{
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = GUISelectEquipBagScene.mSocketSlot;
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mSelectEquipLayer = GameUITools.FindGameObject("SelectEquipLayer", parent).AddComponent<SelectEquipLayer>();
		this.mSelectEquipLayer.InitWithBaseScene(this, GUISelectEquipBagScene.mSocketSlot, GUISelectEquipBagScene.mEquipSlot);
		this.mFilter = GameUITools.RegisterClickEvent("Filter", new UIEventListener.VoidDelegate(this.OnFilterClick), parent).GetComponent<UIToggle>();
		this.mSelectEquipLayer.Refresh();
	}

	public void SendEquipItemMsg(ItemDataEx data)
	{
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(GUISelectEquipBagScene.mSocketSlot);
		if (socket != null)
		{
			PetDataEx pet = socket.GetPet();
			if (pet != null)
			{
				pet.GetAttribute(ref GameUIManager.mInstance.uiState.mOldHpNum, ref GameUIManager.mInstance.uiState.mOldAttackNum, ref GameUIManager.mInstance.uiState.mOldWufangNum, ref GameUIManager.mInstance.uiState.mOldFafangNum);
			}
		}
		MC2S_EquipItem mC2S_EquipItem = new MC2S_EquipItem();
		mC2S_EquipItem.SocketSlot = GUISelectEquipBagScene.mSocketSlot;
		mC2S_EquipItem.EquipSlot = GUISelectEquipBagScene.mEquipSlot;
		mC2S_EquipItem.ItemID = data.Data.ID;
		Globals.Instance.CliSession.Send(197, mC2S_EquipItem);
	}

	private void OnMsgEquipItemEvent(int socketSlot, int equipSlot)
	{
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = GUISelectEquipBagScene.mSocketSlot;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	private void OnFilterClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.mSelectEquipLayer.ReInit(this.mFilter.value);
	}
}
