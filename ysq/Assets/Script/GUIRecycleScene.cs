using Proto;
using System;
using UnityEngine;

public class GUIRecycleScene : GameUISession
{
	public enum ERecycleT
	{
		ERecycleT_Null,
		ERecycleT_PetBreak,
		ERecycleT_EquipBreak,
		ERecycleT_PetReborn,
		ERecycleT_TrinketReborn,
		ERecycleT_LopetBreak,
		ERecycleT_LopetReborn
	}

	public AnimationCurve AnimCurve;

	[NonSerialized]
	public float AnimDuration = 0.25f;

	[NonSerialized]
	public GUIRecycleScene.ERecycleT mCurType;

	[NonSerialized]
	public RecycleLayer mRecycleLayer;

	[NonSerialized]
	public UIToggle mPetBreakTab;

	private UIToggle mEquipBreakTab;

	private UIToggle mPetRebornTab;

	private UIToggle mTrinketRebornTab;

	public UIToggle mLopetBreakTab;

	private UIToggle mLopetRebornTab;

	private UISprite mPetBreakRed;

	private UISprite mEquipBreakRed;

	private bool isInit = true;

	public static void Change2This(GUIRecycleScene.ERecycleT type = GUIRecycleScene.ERecycleT.ERecycleT_PetBreak)
	{
		GameUIManager.mInstance.uiState.RecycleType = type;
		GameUIManager.mInstance.ChangeSession<GUIRecycleScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("recycle1");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		this.CreateObjects();
		Globals.Instance.CliSession.Register(531, new ClientSession.MsgHandler(this.mRecycleLayer.OnMsgEquipBreakUp));
		Globals.Instance.CliSession.Register(533, new ClientSession.MsgHandler(this.mRecycleLayer.OnMsgTrinketReborn));
		Globals.Instance.CliSession.Register(416, new ClientSession.MsgHandler(this.mRecycleLayer.OnMsgPetReborn));
		LocalPlayer expr_A7 = Globals.Instance.Player;
		expr_A7.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_A7.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.mRecycleLayer.OnPlayerUpdateEvent));
		PetSubSystem expr_DC = Globals.Instance.Player.PetSystem;
		expr_DC.BreakUpPetEvent = (PetSubSystem.VoidCallback)Delegate.Combine(expr_DC.BreakUpPetEvent, new PetSubSystem.VoidCallback(this.mRecycleLayer.OnPetBreakUpEvent));
		LopetSubSystem expr_111 = Globals.Instance.Player.LopetSystem;
		expr_111.BreakupLopetEvent = (LopetSubSystem.VoidCallback)Delegate.Combine(expr_111.BreakupLopetEvent, new LopetSubSystem.VoidCallback(this.mRecycleLayer.OnLopetBreakUpEvent));
		LopetSubSystem expr_146 = Globals.Instance.Player.LopetSystem;
		expr_146.RebornLopetEvent = (LopetSubSystem.VoidCallback)Delegate.Combine(expr_146.RebornLopetEvent, new LopetSubSystem.VoidCallback(this.mRecycleLayer.OnLopetRebornEvent));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		this.mRecycleLayer.PreDestroy();
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
		Globals.Instance.CliSession.Unregister(531, new ClientSession.MsgHandler(this.mRecycleLayer.OnMsgEquipBreakUp));
		Globals.Instance.CliSession.Unregister(533, new ClientSession.MsgHandler(this.mRecycleLayer.OnMsgTrinketReborn));
		Globals.Instance.CliSession.Unregister(416, new ClientSession.MsgHandler(this.mRecycleLayer.OnMsgPetReborn));
		LocalPlayer expr_98 = Globals.Instance.Player;
		expr_98.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_98.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.mRecycleLayer.OnPlayerUpdateEvent));
		PetSubSystem expr_CD = Globals.Instance.Player.PetSystem;
		expr_CD.BreakUpPetEvent = (PetSubSystem.VoidCallback)Delegate.Remove(expr_CD.BreakUpPetEvent, new PetSubSystem.VoidCallback(this.mRecycleLayer.OnPetBreakUpEvent));
		LopetSubSystem expr_102 = Globals.Instance.Player.LopetSystem;
		expr_102.BreakupLopetEvent = (LopetSubSystem.VoidCallback)Delegate.Remove(expr_102.BreakupLopetEvent, new LopetSubSystem.VoidCallback(this.mRecycleLayer.OnLopetBreakUpEvent));
		LopetSubSystem expr_137 = Globals.Instance.Player.LopetSystem;
		expr_137.RebornLopetEvent = (LopetSubSystem.VoidCallback)Delegate.Remove(expr_137.RebornLopetEvent, new LopetSubSystem.VoidCallback(this.mRecycleLayer.OnLopetRebornEvent));
	}

	private void OnBackClick(GameObject go)
	{
		GameUIManager.mInstance.uiState.EquipBreakUpData = null;
		GameUIManager.mInstance.uiState.TrinketRebornData = null;
		GameUIManager.mInstance.uiState.PetBreakUpData = null;
		GameUIManager.mInstance.uiState.PetRebornData = null;
		GameUIManager.mInstance.uiState.LopetBreakData = null;
		GameUIManager.mInstance.uiState.LopetRebornData = null;
		GameUIManager.mInstance.GobackSession();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mRecycleLayer = GameUITools.FindGameObject("RecycleLayer", parent).AddComponent<RecycleLayer>();
		this.mRecycleLayer.InitWithBaseScene(this);
		parent = GameUITools.FindGameObject("Tabs", parent);
		this.mPetBreakTab = base.FindGameObject("PetBreakTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mPetBreakTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_83 = UIEventListener.Get(this.mPetBreakTab.gameObject);
		expr_83.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_83.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mPetBreakRed = GameUITools.FindUISprite("Red", this.mPetBreakTab.gameObject);
		this.mEquipBreakTab = base.FindGameObject("EquipBreakTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mEquipBreakTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_103 = UIEventListener.Get(this.mEquipBreakTab.gameObject);
		expr_103.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_103.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mEquipBreakRed = GameUITools.FindUISprite("Red", this.mEquipBreakTab.gameObject);
		this.mPetRebornTab = base.FindGameObject("PetRebornTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mPetRebornTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_183 = UIEventListener.Get(this.mPetRebornTab.gameObject);
		expr_183.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_183.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mTrinketRebornTab = base.FindGameObject("TrinketRebornTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mTrinketRebornTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_1E8 = UIEventListener.Get(this.mTrinketRebornTab.gameObject);
		expr_1E8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1E8.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mLopetBreakTab = base.FindGameObject("LopetBreakTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mLopetBreakTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_24D = UIEventListener.Get(this.mLopetBreakTab.gameObject);
		expr_24D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_24D.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mLopetRebornTab = base.FindGameObject("LopetRebornTab", parent).GetComponent<UIToggle>();
		EventDelegate.Add(this.mLopetRebornTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_2B2 = UIEventListener.Get(this.mLopetRebornTab.gameObject);
		expr_2B2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2B2.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		if (!Tools.CanPlay(GameConst.GetInt32(201), true))
		{
			this.mLopetBreakTab.gameObject.SetActive(false);
			this.mLopetRebornTab.gameObject.SetActive(false);
		}
		if (GameUIManager.mInstance.uiState.PetBreakUpData == null)
		{
			GameUIManager.mInstance.uiState.PetBreakUpData = new MC2S_PetBreakUp();
		}
		if (GameUIManager.mInstance.uiState.EquipBreakUpData == null)
		{
			GameUIManager.mInstance.uiState.EquipBreakUpData = new MC2S_EquipBreakUp();
		}
		if (GameUIManager.mInstance.uiState.PetRebornData == null)
		{
			GameUIManager.mInstance.uiState.PetRebornData = new MC2S_PetReborn();
		}
		if (GameUIManager.mInstance.uiState.TrinketRebornData == null)
		{
			GameUIManager.mInstance.uiState.TrinketRebornData = new MC2S_TrinketReborn();
		}
		if (GameUIManager.mInstance.uiState.LopetBreakData == null)
		{
			GameUIManager.mInstance.uiState.LopetBreakData = new MC2S_LopetBreakUp();
		}
		if (GameUIManager.mInstance.uiState.LopetRebornData == null)
		{
			GameUIManager.mInstance.uiState.LopetRebornData = new MC2S_LopetReborn();
		}
		this.RefreshRed();
		this.Init((GameUIManager.mInstance.uiState.RecycleType != GUIRecycleScene.ERecycleT.ERecycleT_Null) ? GameUIManager.mInstance.uiState.RecycleType : GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			switch (name)
			{
			case "PetBreakTab":
				this.mCurType = GUIRecycleScene.ERecycleT.ERecycleT_PetBreak;
				this.mRecycleLayer.Refresh();
				break;
			case "EquipBreakTab":
				this.mCurType = GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak;
				this.mRecycleLayer.Refresh();
				break;
			case "PetRebornTab":
				this.mCurType = GUIRecycleScene.ERecycleT.ERecycleT_PetReborn;
				this.mRecycleLayer.Refresh();
				break;
			case "TrinketRebornTab":
				this.mCurType = GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn;
				this.mRecycleLayer.Refresh();
				break;
			case "LopetBreakTab":
				this.mCurType = GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak;
				this.mRecycleLayer.Refresh();
				break;
			case "LopetRebornTab":
				this.mCurType = GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn;
				this.mRecycleLayer.Refresh();
				break;
			}
			if (!this.isInit)
			{
				this.mRecycleLayer.ClearData();
			}
			else
			{
				this.mRecycleLayer.InitData();
				this.isInit = false;
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void Init(GUIRecycleScene.ERecycleT type = GUIRecycleScene.ERecycleT.ERecycleT_PetBreak)
	{
		this.mCurType = type;
		switch (this.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			this.mPetBreakTab.value = true;
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			this.mEquipBreakTab.value = true;
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			this.mPetRebornTab.value = true;
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
			this.mTrinketRebornTab.value = true;
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			this.mLopetBreakTab.value = true;
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			this.mLopetRebornTab.value = true;
			break;
		}
	}

	public static bool ShowPetBreakRed()
	{
		int num = 0;
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting())
			{
				if (current.Info.Quality <= 1 && ++num >= 5)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool ShowEquipBreakRed()
	{
		int num = 0;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (!current.IsEquiped())
			{
				if (current.Info.Type == 0 && current.Info.Quality <= 1 && ++num >= 5)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RefreshRed()
	{
		this.RefreshPetBreakRed();
		this.RefreshEquipBreakRed();
	}

	public void RefreshPetBreakRed()
	{
		this.mPetBreakRed.enabled = GUIRecycleScene.ShowPetBreakRed();
	}

	public void RefreshEquipBreakRed()
	{
		this.mEquipBreakRed.enabled = GUIRecycleScene.ShowEquipBreakRed();
	}
}
