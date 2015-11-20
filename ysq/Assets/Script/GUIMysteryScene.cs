using System;
using UnityEngine;

public class GUIMysteryScene : GameUISession
{
	private static bool pillageState;

	private static float timer;

	private UIPanel mPanel;

	private MysteryBtn mKRBtn;

	private MysteryBtn mAwakeBtn;

	public static bool Red
	{
		get
		{
			return GUIMysteryScene.TrialRed() || GUIMysteryScene.PillageRed() || GUIMysteryScene.CostumePartyRed() || GUIMysteryScene.KingRewardRed() || GUIMysteryScene.WorldBossRed() || GUIMysteryScene.AwakeRoadRed() || GUIMysteryScene.GuardRed() || GUIMysteryScene.MagicLoveRed();
		}
	}

	private static bool CanTrialReward()
	{
		bool result = false;
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0)
		{
			int num = (Globals.Instance.Player.Data.TrialMaxWave - Globals.Instance.Player.Data.TrialWave) * 30;
			int num2 = Mathf.Max(0, num - (Globals.Instance.Player.GetTimeStamp() - Globals.Instance.Player.Data.TrialFarmTimeStamp));
			result = (num2 < 1);
		}
		return result;
	}

	public static bool TrialRed()
	{
		return Tools.CanPlay(GameConst.GetInt32(5), true) && (Globals.Instance.Player.Data.TrialResetCount == 0 || GUIMysteryScene.CanTrialReward());
	}

	public static bool WorldBossRed()
	{
		return Tools.CanPlay(GameConst.GetInt32(1), true) && (Globals.Instance.Player.HasRedFlag(64) || Tools.IsFDSCanTaken() || Tools.IsWBRewardCanTaken());
	}

	public static bool CostumePartyRed()
	{
		return Globals.Instance.Player.CostumePartySystem.IsNew();
	}

	private static bool GuardRed()
	{
		return Tools.CanPlay(GameConst.GetInt32(32), true) && GameConst.GetInt32(125) - Globals.Instance.Player.Data.MGCount > 0;
	}

	private static bool MagicLoveRed()
	{
		return Globals.Instance.Player.HasRedFlag(262144);
	}

	public static bool PillageRed()
	{
		if (Time.time - GUIMysteryScene.timer > 3f)
		{
			GUIMysteryScene.timer = Time.time;
			GUIMysteryScene.pillageState = (Tools.CanPlay(GameConst.GetInt32(8), true) && Tools.IsAnyRecipeComposite());
		}
		return GUIMysteryScene.pillageState;
	}

	public static bool KingRewardRed()
	{
		return GUIKingRewardScene.CanTakePartIn();
	}

	public static bool AwakeRoadRed()
	{
		return GUIAwakeRoadSceneV2.CanShowRedTag();
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		GameUIManager.mInstance.GetTopGoods().Show("mijingLb");
		this.CreateObjects();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void CreateObjects()
	{
		GameObject gameObject = GameUITools.FindGameObject("Panel", base.gameObject);
		this.mPanel = gameObject.GetComponent<UIPanel>();
		MysteryBtn mysteryBtn = GameUITools.FindGameObject("PVP", gameObject).AddComponent<MysteryBtn>();
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_35 = mysteryBtn;
		expr_35.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_35.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnPVPClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(6), true);
		if (mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryPVP"));
		}
		else
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(6)
			}));
		}
		mysteryBtn = GameUITools.FindGameObject("Pillage", gameObject).AddComponent<MysteryBtn>();
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_DC = mysteryBtn;
		expr_DC.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_DC.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnPillageClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(8), true);
		if (mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(GUIMysteryScene.PillageRed());
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryPillage"));
		}
		else
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(8)
			}));
		}
		mysteryBtn = GameUITools.FindGameObject("WorldBoss", gameObject).AddComponent<MysteryBtn>();
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_187 = mysteryBtn;
		expr_187.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_187.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnWorldBossClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(1), true);
		if (mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(GUIMysteryScene.WorldBossRed());
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryWorldBoss"));
		}
		else
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(1)
			}));
		}
		mysteryBtn = GameUITools.FindGameObject("KingReward", gameObject).AddComponent<MysteryBtn>();
		this.mKRBtn = mysteryBtn;
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_239 = mysteryBtn;
		expr_239.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_239.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnKingRewardClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(2), true);
		if (mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(GUIMysteryScene.KingRewardRed());
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryKingReward"));
		}
		else
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(2)
			}));
		}
		mysteryBtn = GameUITools.FindGameObject("Trial", gameObject).AddComponent<MysteryBtn>();
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_2E4 = mysteryBtn;
		expr_2E4.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_2E4.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnTrialClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(5), true);
		if (!mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(5)
			}));
		}
		else
		{
			mysteryBtn.SetRed(GUIMysteryScene.TrialRed());
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryTrial"));
		}
		mysteryBtn = GameUITools.FindGameObject("CostumeParty", gameObject).AddComponent<MysteryBtn>();
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_38F = mysteryBtn;
		expr_38F.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_38F.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnCostumepartyClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(10), true);
		if (mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(GUIMysteryScene.CostumePartyRed());
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryCostumeParty"));
		}
		else
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(10)
			}));
		}
		mysteryBtn = GameUITools.FindGameObject("AwakeRoad", gameObject).AddComponent<MysteryBtn>();
		this.mAwakeBtn = mysteryBtn;
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(26)))
		{
			mysteryBtn.gameObject.SetActive(false);
		}
		else
		{
			mysteryBtn.InitWithBaseScene();
			MysteryBtn expr_476 = mysteryBtn;
			expr_476.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_476.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnAwakeRoadClick));
			mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(24), true);
			if (mysteryBtn.IsOpen)
			{
				mysteryBtn.SetRed(GUIMysteryScene.AwakeRoadRed());
				mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryAwakeRoad"));
			}
			else
			{
				mysteryBtn.SetRed(false);
				mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
				{
					GameConst.GetInt32(24)
				}));
			}
		}
		mysteryBtn = GameUITools.FindGameObject("Guard", gameObject).AddComponent<MysteryBtn>();
		mysteryBtn.InitWithBaseScene();
		MysteryBtn expr_523 = mysteryBtn;
		expr_523.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_523.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnGuardClick));
		mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(32), true);
		if (mysteryBtn.IsOpen)
		{
			mysteryBtn.SetRed(GUIMysteryScene.GuardRed());
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryGuard"));
		}
		else
		{
			mysteryBtn.SetRed(false);
			mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(32)
			}));
		}
		mysteryBtn = GameUITools.FindGameObject("MagicLove", gameObject).AddComponent<MysteryBtn>();
		if (Tools.CanPlay(GameConst.GetInt32(245), true))
		{
			mysteryBtn.InitWithBaseScene();
			MysteryBtn expr_5E5 = mysteryBtn;
			expr_5E5.OnClickEvent = (MysteryBtn.VoidCallBack)Delegate.Combine(expr_5E5.OnClickEvent, new MysteryBtn.VoidCallBack(this.OnMagicLoveClick));
			mysteryBtn.IsOpen = Tools.CanPlay(GameConst.GetInt32(246), true);
			if (mysteryBtn.IsOpen)
			{
				mysteryBtn.SetRed(GUIMysteryScene.MagicLoveRed());
				mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryMagicLove"));
			}
			else
			{
				mysteryBtn.SetRed(false);
				mysteryBtn.SetTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
				{
					GameConst.GetInt32(246)
				}));
			}
		}
		else
		{
			mysteryBtn.gameObject.SetActive(false);
		}
	}

	public void CenterOnKingReward()
	{
		this.CenterOnItem(this.mKRBtn);
	}

	public void CenterOnAwakeRoad()
	{
		this.CenterOnItem(this.mAwakeBtn);
	}

	private void CenterOnItem(MysteryBtn btn)
	{
		Vector3[] worldCorners = this.mPanel.worldCorners;
		Vector3 position = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Transform cachedTransform = this.mPanel.cachedTransform;
		UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
		Vector3 a = cachedTransform.InverseTransformPoint(btn.transform.position);
		Vector3 b = cachedTransform.InverseTransformPoint(position);
		Vector3 b2 = a - b;
		if (!component.canMoveHorizontally)
		{
			b2.x = 0f;
		}
		if (!component.canMoveVertically)
		{
			b2.y = 0f;
		}
		b2.z = 0f;
		cachedTransform.localPosition -= b2;
		Vector4 v = this.mPanel.clipOffset;
		v.x += b2.x;
		v.y += b2.y;
		this.mPanel.clipOffset = v;
		component.RestrictWithinBounds(true);
	}

	private void Party()
	{
		GUICostumePartyScene.TryOpen();
	}

	public void OnPillageClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPillageScene.TryOpen(false);
	}

	private void OnWorldBossClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIBossReadyScene>(null, false, true);
	}

	public void OnKingRewardClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIKingRewardScene>(null, false, true);
	}

	public void OnTrialClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUITrailTowerSceneV2>(null, false, true);
	}

	public void OnCostumepartyClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUICostumePartyScene.TryOpen();
	}

	public void OnAwakeRoadClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.AdventureSceneInfo = null;
		GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, false, true);
	}

	public void OnPVPClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPVP4ReadyScene.TryOpen();
	}

	private void OnGuardClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuardScene.Show(false);
	}

	private void OnMagicLoveClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIMagicLoveScene>(null, false, true);
	}
}
