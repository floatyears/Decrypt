using Att;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GameUIOptionPopUp : MonoBehaviour
{
	private UIToggle mJoyStick;

	private UIToggle mTouch;

	private bool mIsGuildBoss;

	private void Awake()
	{
		this.CreateObjects();
		base.Invoke("Pause", 0.15f);
	}

	private void Pause()
	{
		Globals.Instance.GameMgr.Pause();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("windowBg").gameObject;
		GUICommonSwitchBtn gUICommonSwitchBtn = gameObject.transform.Find("bgMusic/bgSoundSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn.InitSwithBtn(GameSetting.Data.Music);
		GUICommonSwitchBtn expr_42 = gUICommonSwitchBtn;
		expr_42.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_42.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnBgMusicChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn2 = gameObject.transform.Find("effectSound/effectSoundSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn2.InitSwithBtn(GameSetting.Data.Sound);
		GUICommonSwitchBtn expr_8F = gUICommonSwitchBtn2;
		expr_8F.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_8F.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnEffectSoundChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn3 = gameObject.transform.Find("charVoice/charSoundSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn3.InitSwithBtn(GameSetting.Data.Voice);
		GUICommonSwitchBtn expr_DC = gUICommonSwitchBtn3;
		expr_DC.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_DC.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnCharSoundSwithBtnChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn4 = gameObject.transform.Find("showHp/showHpSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn4.InitSwithBtn(GameSetting.Data.ShowHPBar);
		GUICommonSwitchBtn expr_12C = gUICommonSwitchBtn4;
		expr_12C.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_12C.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnShowHpSwithBtnChanged));
		GameObject gameObject2 = gameObject.transform.Find("optionType").gameObject;
		this.mJoyStick = gameObject2.transform.Find("joyStick").GetComponent<UIToggle>();
		this.mTouch = gameObject2.transform.Find("touch").GetComponent<UIToggle>();
		EventDelegate.Add(this.mJoyStick.onChange, new EventDelegate.Callback(this.OnOptionTypeChanged));
		EventDelegate.Add(this.mTouch.onChange, new EventDelegate.Callback(this.OnOptionTypeChanged));
		UIEventListener expr_1E6 = UIEventListener.Get(this.mJoyStick.gameObject);
		expr_1E6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1E6.onClick, new UIEventListener.VoidDelegate(this.OnOptionTypeClicked));
		UIEventListener expr_217 = UIEventListener.Get(this.mTouch.gameObject);
		expr_217.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_217.onClick, new UIEventListener.VoidDelegate(this.OnOptionTypeClicked));
		GameObject gameObject3 = gameObject.transform.Find("yesBtn").gameObject;
		UIEventListener expr_256 = UIEventListener.Get(gameObject3);
		expr_256.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_256.onClick, new UIEventListener.VoidDelegate(this.OnOKClick));
		GameObject gameObject4 = gameObject.transform.Find("noBtn").gameObject;
		UIEventListener expr_295 = UIEventListener.Get(gameObject4);
		expr_295.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_295.onClick, new UIEventListener.VoidDelegate(this.OnCancelClick));
		if (GameCache.Data.Joystick)
		{
			this.mJoyStick.value = true;
		}
		else
		{
			this.mTouch.value = true;
		}
		SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		if (sceneInfo.Type == 5)
		{
			this.mIsGuildBoss = true;
			Globals.Instance.CliSession.Register(968, new ClientSession.MsgHandler(this.OnMsgTakeGuildBossDamageReward));
		}
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		if (this.mIsGuildBoss)
		{
			Globals.Instance.CliSession.Unregister(968, new ClientSession.MsgHandler(this.OnMsgTakeGuildBossDamageReward));
		}
	}

	private void OnBgMusicChanged(bool isOpen)
	{
		if (GameSetting.Data.Music == isOpen)
		{
			return;
		}
		GameSetting.Data.Music = isOpen;
		GameSetting.UpdateNow = true;
		if (isOpen)
		{
			if (!Globals.Instance.BackgroundMusicMgr.IsGameBGMPlaying())
			{
				Globals.Instance.BackgroundMusicMgr.PauseGameBGM(0.1f);
				Tools.PlaySceneBGM(Globals.Instance.SenceMgr.sceneInfo);
			}
		}
		else
		{
			Globals.Instance.BackgroundMusicMgr.PauseGameBGM(3.40282347E+38f);
		}
	}

	private void OnEffectSoundChanged(bool isOpen)
	{
		if (GameSetting.Data.Sound != isOpen)
		{
			GameSetting.Data.Sound = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnCharSoundSwithBtnChanged(bool isOpen)
	{
		GameSetting.Data.Voice = isOpen;
		GameSetting.UpdateNow = true;
	}

	private void OnShowHpSwithBtnChanged(bool isOpen)
	{
		if (GameSetting.Data.ShowHPBar != isOpen)
		{
			GameSetting.Data.ShowHPBar = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnOptionTypeClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnOptionTypeChanged()
	{
		if (UIToggle.current.value)
		{
			GameCache.Data.Joystick = (UIToggle.current == this.mJoyStick);
			GameCache.UpdateNow = true;
			GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
			if (session != null)
			{
				session.mGameController.UpdateControlType();
			}
		}
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.GameMgr.Play();
		SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		GameAnalytics.OnFailScene(sceneInfo, GameAnalytics.ESceneFailed.UIClose);
		Globals.Instance.SenceMgr.CloseScene();
		switch (sceneInfo.Type)
		{
		case 0:
			if (sceneInfo.Difficulty == 2)
			{
				GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, true, true);
			}
			else
			{
				GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, true, true);
			}
			break;
		case 1:
			GameUIManager.mInstance.ChangeSession<GUITrailTowerSceneV2>(null, true, true);
			break;
		case 3:
			Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp = Globals.Instance.Player.GetTimeStamp() + 60;
			GUIWorldBossVictoryScene.BackBossScene();
			break;
		case 5:
		{
			MC2S_TakeGuildBossDamageReward mC2S_TakeGuildBossDamageReward = new MC2S_TakeGuildBossDamageReward();
			mC2S_TakeGuildBossDamageReward.ID = Globals.Instance.Player.GuildSystem.Guild.AttackAcademyID1;
			Globals.Instance.CliSession.Send(967, mC2S_TakeGuildBossDamageReward);
			break;
		}
		case 6:
			GameUIManager.mInstance.ChangeSession<GUIKingRewardScene>(null, false, true);
			break;
		case 7:
			GUIGuardScene.Show(true);
			break;
		}
		if (sceneInfo.Type != 5)
		{
			GameUIManager.mInstance.DestroyGameUIOptionPopUp();
		}
		Globals.Instance.ActorMgr.GetCombatTime(true);
	}

	public void OnCancelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.GameMgr.Play();
		GameUITools.PlayCloseWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), false);
	}

	private void OnCloseAnimEnd()
	{
		GameUIManager.mInstance.DestroyGameUIOptionPopUp();
	}

	public void OnMsgTakeGuildBossDamageReward(MemoryStream stream)
	{
		MS2C_TakeGuildBossDamageReward mS2C_TakeGuildBossDamageReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeGuildBossDamageReward), stream) as MS2C_TakeGuildBossDamageReward;
		if (mS2C_TakeGuildBossDamageReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_TakeGuildBossDamageReward.Result);
			return;
		}
		GameUIManager.mInstance.DestroyGameUIOptionPopUp();
		GameUIManager.mInstance.ChangeSession<GUIGuildSchoolScene>(null, true, true);
	}
}
