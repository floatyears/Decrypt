using System;
using UnityEngine;

public class GUIUnlockPopUp : GameUIBasePopup
{
	private UISprite mPic1;

	private UISprite mPic2;

	private GameObject mGo;

	private UILabel mName;

	public int unlockLevel;

	public static bool Show(int level, GameUIPopupManager.PopClosedCallback cb = null, GameUIPopupManager.PopClosedCallback cb2 = null)
	{
		if (string.IsNullOrEmpty(Singleton<StringManager>.Instance.GetUnlockImage(level)))
		{
			return false;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIUnlockPopUp, false, null, cb2);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(level);
		return true;
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPic1 = GameUITools.FindUISprite("Pic1", base.gameObject);
		this.mPic2 = GameUITools.FindUISprite("Pic2", base.gameObject);
		this.mGo = GameUITools.FindGameObject("Go", base.gameObject);
		UIEventListener expr_4D = UIEventListener.Get(this.mGo);
		expr_4D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4D.onClick, new UIEventListener.VoidDelegate(this.OnGoClick));
		this.mName = GameUITools.FindUILabel("Name/Label", base.gameObject);
	}

	public override void InitPopUp(int level)
	{
		this.mName.text = Singleton<StringManager>.Instance.GetUnlockFunc(level, true);
		this.unlockLevel = level;
		if (this.ShowPic2(level))
		{
			this.mPic1.enabled = false;
			this.mPic2.enabled = true;
			this.mPic2.spriteName = Singleton<StringManager>.Instance.GetUnlockImage(level);
			this.mPic2.MakePixelPerfect();
		}
		else
		{
			this.mPic1.enabled = true;
			this.mPic2.enabled = false;
			this.mPic1.spriteName = Singleton<StringManager>.Instance.GetUnlockImage(level);
			this.mPic1.MakePixelPerfect();
			if (level == GameConst.GetInt32(122))
			{
				this.mPic1.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
				this.mPic1.width = 220;
			}
		}
		GameUIManager.mInstance.uiState.UnlockNewGameLevel = 0;
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private bool ShowPic2(int level)
	{
		return level == GameConst.GetInt32(246) || level == GameConst.GetInt32(201);
	}

	public void OnGoClick(GameObject go)
	{
		if (this.unlockLevel == GameConst.GetInt32(6))
		{
			GUIPVP4ReadyScene.TryOpen();
		}
		else if (this.unlockLevel == GameConst.GetInt32(8))
		{
			GUIPillageScene.TryOpen(false);
		}
		else if (this.unlockLevel == GameConst.GetInt32(7))
		{
			GameUIManager.mInstance.ChangeSession<GUIConstellationScene>(null, false, true);
		}
		else if (this.unlockLevel == GameConst.GetInt32(5))
		{
			GUITrailTowerSceneV2.TryOpen();
		}
		else if (this.unlockLevel == GameConst.GetInt32(2))
		{
			GUIKingRewardScene.TryOpen();
		}
		else if (this.unlockLevel == GameConst.GetInt32(10))
		{
			GUICostumePartyScene.TryOpen();
		}
		else if (this.unlockLevel == GameConst.GetInt32(1))
		{
			GUIBossReadyScene.TryOpen();
		}
		else if (this.unlockLevel == GameConst.GetInt32(3) || this.unlockLevel == GameConst.GetInt32(4))
		{
			GUIGuildManageScene.TryOpen();
		}
		else if (this.unlockLevel == GameConst.GetInt32(24))
		{
			GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, false, true);
		}
		else if (this.unlockLevel == GameConst.GetInt32(122))
		{
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = Globals.Instance.Player.TeamSystem.GetPet(0);
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 4;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
		else if (this.unlockLevel == GameConst.GetInt32(32))
		{
			GUIGuardScene.Show(false);
		}
		else if (this.unlockLevel == GameConst.GetInt32(246))
		{
			GameUIManager.mInstance.ChangeSession<GUIMagicLoveScene>(null, false, true);
		}
		else if (this.unlockLevel == GameConst.GetInt32(201))
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = true;
			GameUIManager.mInstance.uiState.CombatPetSlot = 0;
			GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
		}
		GameUIPopupManager.GetInstance().PopState(true, null);
	}

	private void Close()
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
