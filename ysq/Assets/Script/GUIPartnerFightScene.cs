using System;
using UnityEngine;

public class GUIPartnerFightScene : GameUISession
{
	public PartnerFightLayer mPartnerFightLayer;

	public PartnerFightBagItem mPartnerFightBagItem;

	private GameObject mSelect;

	private GameObject mSelectIcon;

	public PetDataEx mPetData;

	public int mSlot;

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("WindowBg");
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mPartnerFightLayer = GameUITools.FindGameObject("PartnerFightLayer", parent).AddComponent<PartnerFightLayer>();
		this.mSelect = transform.transform.FindChild("selectBtn").gameObject;
		this.mSelectIcon = this.mSelect.transform.Find("icon").gameObject;
		this.mPartnerFightLayer.InitWithBaseScene(this);
		if (this.mSlot == -1)
		{
			this.mSelect.gameObject.SetActive(false);
		}
	}

	protected override void OnPostLoadGUI()
	{
		this.mSlot = GameUIManager.mInstance.uiState.CombatPetSlot;
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(this.mSlot);
		if (socket != null)
		{
			this.mPetData = socket.GetPet();
		}
		else
		{
			int slot = this.mSlot - 4;
			this.mPetData = Globals.Instance.Player.TeamSystem.GetAssist(slot);
		}
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("PetFurther7");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		this.CreateObjects();
		UIEventListener expr_B8 = UIEventListener.Get(this.mSelect.gameObject);
		expr_B8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B8.onClick, new UIEventListener.VoidDelegate(this.OnSlectClick));
		TeamSubSystem expr_E8 = Globals.Instance.Player.TeamSystem;
		expr_E8.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Combine(expr_E8.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		UIEventListener expr_15 = UIEventListener.Get(this.mSelect.gameObject);
		expr_15.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(expr_15.onClick, new UIEventListener.VoidDelegate(this.OnSlectClick));
		GameUIManager.mInstance.GetTopGoods().Hide();
		TeamSubSystem expr_54 = Globals.Instance.Player.TeamSystem;
		expr_54.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Remove(expr_54.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
	}

	private void OnEquipPetEvent(int slot)
	{
		if (slot == this.mSlot)
		{
			this.OnBackClick(null);
		}
	}

	public void OnBackClick(GameObject go)
	{
		if (this.mSlot == -1)
		{
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetReborn);
		}
		else
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = true;
			GameUIManager.mInstance.uiState.CombatPetSlot = ((this.mSlot < 4) ? this.mSlot : 5);
			GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
		}
	}

	public void OnSlectClick(GameObject go)
	{
		this.mSelectIcon.SetActive(!this.mSelectIcon.activeInHierarchy);
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.mPartnerFightLayer.IsShowFightOrHide(!this.mSelectIcon.activeInHierarchy);
	}
}
