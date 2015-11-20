using Att;
using System;
using UnityEngine;

public sealed class GUICombatMain : GameUISession
{
	private CombatMainBattleCD mCombatMainBattleCD;

	private CombatMainWavePanel mCombatMainWavePanel;

	private CommonCombatMainBattleCD mCommonCombatMainBattleCD;

	private UILabel mDragSkillTipTxt;

	private CombatMainPvp4TopInfo mCombatMainPvp4TopInfo;

	private CombatMainPvp4TargetInfo mCombatMainPvp4TargetInfo;

	private PlayerController pcc;

	private bool mIsReplay;

	public bool EquipChange
	{
		private get;
		set;
	}

	public CombatMainHeroSummonLayer mHeroSummonLayer
	{
		get;
		private set;
	}

	public CombatMainSkillLayer mSkillLayer
	{
		get;
		private set;
	}

	public CombatMainGameControllerLayer mGameController
	{
		get;
		private set;
	}

	public CombatMainOptionalLayer mOptionalLayer
	{
		get;
		private set;
	}

	public bool IsReplay
	{
		get
		{
			return this.mIsReplay;
		}
		set
		{
			this.mIsReplay = value;
			this.mCombatMainPvp4TopInfo.RefreshUIState();
		}
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.FindGameObject("left-top", null);
		this.mHeroSummonLayer = gameObject.AddComponent<CombatMainHeroSummonLayer>();
		gameObject = base.FindGameObject("right-bottom", null);
		this.mSkillLayer = gameObject.AddComponent<CombatMainSkillLayer>();
		gameObject = base.FindGameObject("left-bottom", null);
		this.mGameController = gameObject.AddComponent<CombatMainGameControllerLayer>();
		this.mGameController.InitWithBaseScene();
		gameObject = base.FindGameObject("right-top", null);
		this.mOptionalLayer = gameObject.AddComponent<CombatMainOptionalLayer>();
		this.mCombatMainBattleCD = base.transform.Find("right-top/battleCD").gameObject.AddComponent<CombatMainBattleCD>();
		this.mCombatMainBattleCD.InitWithBaseScene();
		this.mCombatMainBattleCD.gameObject.SetActive(false);
		this.mCommonCombatMainBattleCD = base.transform.Find("center-top/comBattleCD").gameObject.AddComponent<CommonCombatMainBattleCD>();
		this.mCommonCombatMainBattleCD.InitWithBaseScene();
		this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
		this.mDragSkillTipTxt = base.transform.Find("center-top/dragSkillTip").GetComponent<UILabel>();
		this.mDragSkillTipTxt.text = Singleton<StringManager>.Instance.GetString("dragSkillTipTxt");
		this.mDragSkillTipTxt.gameObject.SetActive(false);
		this.mCombatMainWavePanel = base.transform.Find("right-top/wavePanel").gameObject.AddComponent<CombatMainWavePanel>();
		this.mCombatMainWavePanel.InitWithBaseScene();
		this.mCombatMainWavePanel.gameObject.SetActive(false);
		this.mCombatMainPvp4TopInfo = base.transform.Find("pvp4TopInfo").gameObject.AddComponent<CombatMainPvp4TopInfo>();
		this.mCombatMainPvp4TopInfo.InitWithBaseScene(this);
		this.mCombatMainPvp4TopInfo.gameObject.SetActive(false);
		this.mCombatMainPvp4TargetInfo = base.transform.Find("pvp4TargetInfo").gameObject.AddComponent<CombatMainPvp4TargetInfo>();
		this.mCombatMainPvp4TargetInfo.gameObject.SetActive(false);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		this.OpenUIIngameScene();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		ActorManager expr_27 = Globals.Instance.ActorMgr;
		expr_27.BuffAddEvent = (ActorManager.BuffAddCallback)Delegate.Combine(expr_27.BuffAddEvent, new ActorManager.BuffAddCallback(this.OnBuffAddEvent));
		ActorManager expr_52 = Globals.Instance.ActorMgr;
		expr_52.BuffUpdateEvent = (ActorManager.BuffAddCallback)Delegate.Combine(expr_52.BuffUpdateEvent, new ActorManager.BuffAddCallback(this.OnBuffAddEvent));
		ActorManager expr_7D = Globals.Instance.ActorMgr;
		expr_7D.BuffRemoveEvent = (ActorManager.BuffRemoveCallback)Delegate.Combine(expr_7D.BuffRemoveEvent, new ActorManager.BuffRemoveCallback(this.OnBuffRemoveEvent));
		ActorManager expr_A8 = Globals.Instance.ActorMgr;
		expr_A8.LootMoneyEvent = (ActorManager.LootMoneyCallback)Delegate.Combine(expr_A8.LootMoneyEvent, new ActorManager.LootMoneyCallback(this.mOptionalLayer.OnLootMoneyEvent));
		ActorManager expr_D8 = Globals.Instance.ActorMgr;
		expr_D8.TrialScoreEvent = (ActorManager.TrialScoreCallback)Delegate.Combine(expr_D8.TrialScoreEvent, new ActorManager.TrialScoreCallback(this.mCombatMainWavePanel.OnTrialScoreEvent));
		if (Globals.Instance.ActorMgr.TrialScene != null)
		{
			TrialScene expr_121 = Globals.Instance.ActorMgr.TrialScene;
			expr_121.AllMonsterDeadEvent = (TrialScene.AllMonsterDeadCallback)Delegate.Combine(expr_121.AllMonsterDeadEvent, new TrialScene.AllMonsterDeadCallback(this.OnTrialSceneMonsterDead));
			TrialScene expr_151 = Globals.Instance.ActorMgr.TrialScene;
			expr_151.NextWaveEvent = (TrialScene.NextWaveCallback)Delegate.Combine(expr_151.NextWaveEvent, new TrialScene.NextWaveCallback(this.OnNextWaveEvent));
		}
		if (Globals.Instance.ActorMgr.MemoryGearScene != null)
		{
			MemoryGearScene expr_195 = Globals.Instance.ActorMgr.MemoryGearScene;
			expr_195.PlayerResurrectEvent = (MemoryGearScene.VoidCallback)Delegate.Combine(expr_195.PlayerResurrectEvent, new MemoryGearScene.VoidCallback(this.OnPlayerResurrectEvent));
		}
	}

	protected override void OnPreDestroyGUI()
	{
		ActorManager expr_0A = Globals.Instance.ActorMgr;
		expr_0A.TrialScoreEvent = (ActorManager.TrialScoreCallback)Delegate.Remove(expr_0A.TrialScoreEvent, new ActorManager.TrialScoreCallback(this.mCombatMainWavePanel.OnTrialScoreEvent));
		ActorManager expr_3A = Globals.Instance.ActorMgr;
		expr_3A.BuffAddEvent = (ActorManager.BuffAddCallback)Delegate.Remove(expr_3A.BuffAddEvent, new ActorManager.BuffAddCallback(this.OnBuffAddEvent));
		ActorManager expr_65 = Globals.Instance.ActorMgr;
		expr_65.BuffUpdateEvent = (ActorManager.BuffAddCallback)Delegate.Remove(expr_65.BuffUpdateEvent, new ActorManager.BuffAddCallback(this.OnBuffAddEvent));
		ActorManager expr_90 = Globals.Instance.ActorMgr;
		expr_90.BuffRemoveEvent = (ActorManager.BuffRemoveCallback)Delegate.Remove(expr_90.BuffRemoveEvent, new ActorManager.BuffRemoveCallback(this.OnBuffRemoveEvent));
		ActorManager expr_BB = Globals.Instance.ActorMgr;
		expr_BB.LootMoneyEvent = (ActorManager.LootMoneyCallback)Delegate.Remove(expr_BB.LootMoneyEvent, new ActorManager.LootMoneyCallback(this.mOptionalLayer.OnLootMoneyEvent));
		if (Globals.Instance.ActorMgr.TrialScene != null)
		{
			Globals.Instance.ActorMgr.TrialScene.AllMonsterDeadEvent = null;
			Globals.Instance.ActorMgr.TrialScene.NextWaveEvent = null;
		}
		GameUIManager.mInstance.CloseGameStateTip();
		GameUIManager.mInstance.CloseDPad();
		GameUIManager.mInstance.CloseGameCDMsg();
		GameUIManager.mInstance.CloseBattleCDMsg();
		GameUIManager.mInstance.CloseDialog();
		GameUIManager.mInstance.DestoryMiniMap();
		GameUIManager.mInstance.DestroyGameNewsMsg();
		GameUIManager.mInstance.DestroyGameNewsMsgPopUp();
		GameUIManager.mInstance.DestroyGameUIOptionPopUp();
	}

	public void OnBuffAddEvent(int slot, int serialID, BuffInfo info, float duration, int stackCount)
	{
		if (0 <= slot && slot < 5)
		{
			this.mHeroSummonLayer.OnBuffAddEvent(slot, serialID, info, duration, stackCount);
		}
		else if (5 <= slot && slot < 10)
		{
			this.mCombatMainPvp4TargetInfo.OnBuffAddEvent(slot - 5, serialID, info, duration, stackCount);
		}
	}

	public void OnBuffRemoveEvent(int slot, int serialID)
	{
		if (0 <= slot && slot < 5)
		{
			this.mHeroSummonLayer.OnBuffRemoveEvent(slot, serialID);
		}
		else if (5 <= slot && slot < 10)
		{
			this.mCombatMainPvp4TargetInfo.OnBuffRemoveEvent(slot - 5, serialID);
		}
	}

	private void UpdateSkillBtn()
	{
		if (this.EquipChange)
		{
			this.EquipChange = false;
			this.mSkillLayer.SkillBtn1.InitIngameSkillBtn(1, 4);
			this.mSkillLayer.SkillBtn2.InitIngameSkillBtn(2, 1);
			this.mSkillLayer.SkillBtn3.InitIngameSkillBtn(3, 2);
			this.mSkillLayer.SkillBtn1.IsEquipChanged = true;
			this.mSkillLayer.SkillBtn2.IsEquipChanged = true;
			this.mSkillLayer.SkillBtn3.IsEquipChanged = true;
			LopetDataEx curLopet = Globals.Instance.Player.LopetSystem.GetCurLopet(true);
			if (curLopet != null)
			{
				this.mSkillLayer.SkillBtn4.InitIngameSkillBtn(4, -1);
				this.mSkillLayer.SkillBtn4.IsEquipChanged = true;
			}
		}
	}

	private void Update()
	{
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (this.pcc == null)
		{
			this.pcc = Globals.Instance.ActorMgr.PlayerCtrler;
		}
		if (this.pcc == null)
		{
			return;
		}
		this.UpdateSkillBtn();
	}

	public void SetState(int nState)
	{
		switch (nState)
		{
		case 0:
		case 6:
		{
			this.mHeroSummonLayer.gameObject.SetActive(true);
			this.mHeroSummonLayer.SetState(nState);
			this.mSkillLayer.gameObject.SetActive(true);
			this.mSkillLayer.SetState(nState);
			this.mGameController.gameObject.SetActive(true);
			this.mGameController.SetState(nState);
			GameUIManager.mInstance.CreateMiniMap();
			SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
			if (sceneInfo != null)
			{
				if (sceneInfo.ID == GameConst.GetInt32(110))
				{
					this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
				}
				else
				{
					this.mCommonCombatMainBattleCD.gameObject.SetActive(true);
				}
			}
			this.mCombatMainBattleCD.gameObject.SetActive(false);
			this.mCombatMainWavePanel.gameObject.SetActive(false);
			this.mCombatMainPvp4TopInfo.gameObject.SetActive(false);
			this.mCombatMainPvp4TargetInfo.gameObject.SetActive(false);
			this.mOptionalLayer.SetState(nState);
			break;
		}
		case 1:
			this.mHeroSummonLayer.gameObject.SetActive(true);
			this.mHeroSummonLayer.SetState(nState);
			this.mSkillLayer.gameObject.SetActive(true);
			this.mSkillLayer.SetState(nState);
			this.mGameController.gameObject.SetActive(true);
			this.mGameController.SetState(nState);
			this.mCombatMainBattleCD.gameObject.SetActive(true);
			this.mCombatMainBattleCD.SetState(nState);
			this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
			this.mCombatMainWavePanel.gameObject.SetActive(true);
			this.mCombatMainPvp4TopInfo.gameObject.SetActive(false);
			this.mCombatMainPvp4TargetInfo.gameObject.SetActive(false);
			this.mOptionalLayer.SetState(nState);
			break;
		case 2:
		case 4:
		case 8:
		case 9:
			this.mHeroSummonLayer.gameObject.SetActive(true);
			this.mHeroSummonLayer.SetState(nState);
			this.mSkillLayer.gameObject.SetActive(true);
			this.mSkillLayer.SetState(nState);
			this.mGameController.gameObject.SetActive(true);
			this.mGameController.SetState(nState);
			this.mCombatMainBattleCD.gameObject.SetActive(false);
			this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
			this.mCombatMainWavePanel.gameObject.SetActive(false);
			this.IsReplay = GameUIManager.mInstance.uiState.ArenaIsReplay;
			this.mOptionalLayer.gameObject.SetActive(false);
			this.mOptionalLayer.SetState(nState);
			this.mCombatMainPvp4TopInfo.gameObject.SetActive(true);
			this.mCombatMainPvp4TopInfo.RefreshUIState();
			this.mCombatMainPvp4TargetInfo.gameObject.SetActive(true);
			this.mCombatMainPvp4TargetInfo.InitWithBaseScene();
			if (this.IsReplay)
			{
				this.mHeroSummonLayer.transform.GetComponent<UIWidget>().topAnchor.absolute = -60;
				this.mCombatMainPvp4TargetInfo.transform.GetComponent<UIWidget>().topAnchor.absolute = -60;
			}
			else
			{
				this.mHeroSummonLayer.transform.GetComponent<UIWidget>().topAnchor.absolute = 0;
				this.mCombatMainPvp4TargetInfo.transform.GetComponent<UIWidget>().topAnchor.absolute = 0;
			}
			break;
		case 3:
			this.mHeroSummonLayer.gameObject.SetActive(true);
			this.mHeroSummonLayer.SetState(nState);
			this.mSkillLayer.gameObject.SetActive(true);
			this.mSkillLayer.SetState(nState);
			this.mGameController.gameObject.SetActive(true);
			this.mGameController.SetState(nState);
			this.mCombatMainBattleCD.gameObject.SetActive(true);
			this.mCombatMainBattleCD.SetState(nState);
			this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
			this.mCombatMainWavePanel.gameObject.SetActive(false);
			this.mCombatMainPvp4TopInfo.gameObject.SetActive(false);
			this.mCombatMainPvp4TargetInfo.gameObject.SetActive(false);
			this.mOptionalLayer.SetState(nState);
			WorldBossCombatTip.GetInstance().Init(base.transform, nState);
			break;
		case 5:
			this.mHeroSummonLayer.gameObject.SetActive(true);
			this.mHeroSummonLayer.SetState(nState);
			this.mSkillLayer.gameObject.SetActive(true);
			this.mSkillLayer.SetState(nState);
			this.mGameController.gameObject.SetActive(true);
			this.mGameController.SetState(nState);
			this.mCombatMainBattleCD.gameObject.SetActive(true);
			this.mCombatMainBattleCD.SetState(nState);
			this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
			this.mCombatMainWavePanel.gameObject.SetActive(false);
			this.mCombatMainPvp4TopInfo.gameObject.SetActive(false);
			this.mCombatMainPvp4TargetInfo.gameObject.SetActive(false);
			this.mOptionalLayer.SetState(nState);
			WorldBossCombatTip.GetInstance().Init(base.transform, nState);
			break;
		case 7:
			GameUIManager.mInstance.CreateMiniMap();
			this.mHeroSummonLayer.gameObject.SetActive(true);
			this.mHeroSummonLayer.SetState(nState);
			this.mSkillLayer.gameObject.SetActive(true);
			this.mSkillLayer.SetState(nState);
			this.mGameController.gameObject.SetActive(true);
			this.mGameController.SetState(nState);
			this.mCombatMainBattleCD.gameObject.SetActive(false);
			this.mCommonCombatMainBattleCD.gameObject.SetActive(false);
			this.mCombatMainWavePanel.gameObject.SetActive(false);
			this.mCombatMainPvp4TopInfo.gameObject.SetActive(false);
			this.mCombatMainPvp4TargetInfo.gameObject.SetActive(false);
			this.mOptionalLayer.SetState(nState);
			CombatGuardInfo.GetInstance().Init(base.transform);
			break;
		}
	}

	public void OpenUIIngameScene()
	{
		SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		if (sceneInfo != null)
		{
			this.SetState(sceneInfo.Type);
		}
		if (this.mSkillLayer.gameObject.activeSelf)
		{
			this.mSkillLayer.SkillBtn1.EnableSkillBtn = false;
			this.mSkillLayer.SkillBtn2.EnableSkillBtn = false;
			this.mSkillLayer.SkillBtn3.EnableSkillBtn = false;
			this.mSkillLayer.SkillBtn4.EnableSkillBtn = false;
		}
		this.EquipChange = true;
	}

	private void OnTrialSceneMonsterDead(int curLvl)
	{
		GUITrialInGamePopUp.ShowThis(curLvl);
		if (this.mCombatMainWavePanel != null)
		{
			this.mCombatMainWavePanel.RefreshBoxNum();
		}
	}

	private void OnNextWaveEvent()
	{
		if (this.mHeroSummonLayer != null)
		{
			this.mHeroSummonLayer.OnResurrect();
		}
	}

	private void OnPlayerResurrectEvent()
	{
		if (this.mHeroSummonLayer != null)
		{
			this.mHeroSummonLayer.OnResurrect();
		}
	}

	public void ShowHideSkillDragTip(bool isShow)
	{
		this.mDragSkillTipTxt.gameObject.SetActive(isShow);
	}
}
