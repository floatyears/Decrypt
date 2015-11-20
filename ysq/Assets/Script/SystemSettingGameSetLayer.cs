using NtUniSdk.Unity3d;
using System;
using UnityEngine;

public class SystemSettingGameSetLayer : MonoBehaviour
{
	private UIToggle mJoyStick;

	private UIToggle mTouch;

	private UIToggle[] mGraphQuality = new UIToggle[4];

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GUICommonSwitchBtn gUICommonSwitchBtn = base.transform.Find("set0/bgSoundSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn.InitSwithBtn(GameSetting.Data.Music);
		GUICommonSwitchBtn expr_2C = gUICommonSwitchBtn;
		expr_2C.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_2C.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnBgSoundSwithBtnChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn2 = base.transform.Find("set0/effectSoundSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn2.InitSwithBtn(GameSetting.Data.Sound);
		GUICommonSwitchBtn expr_79 = gUICommonSwitchBtn2;
		expr_79.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_79.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnEffectSoundSwithBtnChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn3 = base.transform.Find("set0/charSoundSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn3.InitSwithBtn(GameSetting.Data.Voice);
		GUICommonSwitchBtn expr_C6 = gUICommonSwitchBtn3;
		expr_C6.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_C6.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnCharSoundSwithBtnChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn4 = base.transform.Find("set4/antiLockSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn4.InitSwithBtn(GameSetting.Data.NoScreenLock);
		GUICommonSwitchBtn expr_113 = gUICommonSwitchBtn4;
		expr_113.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_113.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnAntiLockSwithBtnChanged));
		GameObject gameObject = base.transform.Find("set2").gameObject;
		this.mJoyStick = gameObject.transform.Find("joyStick").GetComponent<UIToggle>();
		this.mTouch = gameObject.transform.Find("touch").GetComponent<UIToggle>();
		if (GameCache.Data.Joystick)
		{
			this.mJoyStick.value = true;
		}
		else
		{
			this.mTouch.value = true;
		}
		EventDelegate.Add(this.mJoyStick.onChange, new EventDelegate.Callback(this.OnOptionTypeChanged));
		EventDelegate.Add(this.mTouch.onChange, new EventDelegate.Callback(this.OnOptionTypeChanged));
		UIEventListener expr_1F9 = UIEventListener.Get(this.mJoyStick.gameObject);
		expr_1F9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1F9.onClick, new UIEventListener.VoidDelegate(this.OnOptionTypeClicked));
		UIEventListener expr_22A = UIEventListener.Get(this.mTouch.gameObject);
		expr_22A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_22A.onClick, new UIEventListener.VoidDelegate(this.OnOptionTypeClicked));
		GameObject gameObject2 = base.transform.Find("set3").gameObject;
		this.mGraphQuality[3] = gameObject2.transform.Find("lower").GetComponent<UIToggle>();
		this.mGraphQuality[2] = gameObject2.transform.Find("middle").GetComponent<UIToggle>();
		this.mGraphQuality[1] = gameObject2.transform.Find("high").GetComponent<UIToggle>();
		this.mGraphQuality[GameSetting.Data.GraphQuality].value = true;
		for (int i = 0; i < 4; i++)
		{
			if (this.mGraphQuality[i] != null)
			{
				EventDelegate.Add(this.mGraphQuality[i].onChange, new EventDelegate.Callback(this.OnEffectTypeChanged));
				UIEventListener expr_322 = UIEventListener.Get(this.mGraphQuality[i].gameObject);
				expr_322.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_322.onClick, new UIEventListener.VoidDelegate(this.OnEffectTypeClicked));
			}
		}
		GUICommonSwitchBtn gUICommonSwitchBtn5 = base.transform.Find("set1/showHpSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn5.InitSwithBtn(GameSetting.Data.ShowHPBar);
		GUICommonSwitchBtn expr_380 = gUICommonSwitchBtn5;
		expr_380.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_380.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnShowHpSwithBtnChanged));
		GameObject gameObject3 = base.transform.Find("set4/showCCSwitch").gameObject;
		if (SdkU3d.getCCWindowState() == -1 || !Globals.Instance.Player.IsFunctionEnable(32))
		{
			gameObject3.SetActive(false);
		}
		else
		{
			GUICommonSwitchBtn gUICommonSwitchBtn6 = gameObject3.AddComponent<GUICommonSwitchBtn>();
			gUICommonSwitchBtn6.InitSwithBtn(SdkU3d.getCCWindowState() != 0);
			GUICommonSwitchBtn expr_403 = gUICommonSwitchBtn6;
			expr_403.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_403.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnShowCCSwithBtnChanged));
			gameObject3.SetActive(true);
		}
	}

	private void OnEffectTypeChanged()
	{
		if (UIToggle.current.value)
		{
			for (int i = 0; i < 4; i++)
			{
				if (UIToggle.current == this.mGraphQuality[i])
				{
					Tools.SetQualityLevel((Tools.CustomQualityLevel)i);
					GameSetting.UpdateNow = true;
					break;
				}
			}
		}
	}

	private void OnEffectTypeClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
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
		}
	}

	private void OnBgSoundSwithBtnChanged(bool isOpen)
	{
		if (GameSetting.Data.Music == isOpen)
		{
			return;
		}
		GameSetting.Data.Music = isOpen;
		GameSetting.UpdateNow = true;
		if (isOpen)
		{
			Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		}
		else
		{
			Globals.Instance.BackgroundMusicMgr.StopLobbySound();
		}
	}

	private void OnEffectSoundSwithBtnChanged(bool isOpen)
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

	private void OnAntiLockSwithBtnChanged(bool isOpen)
	{
		GameSetting.Data.NoScreenLock = isOpen;
		GameSetting.UpdateNow = true;
		if (isOpen)
		{
			Screen.sleepTimeout = -1;
		}
		else
		{
			Screen.sleepTimeout = -2;
		}
	}

	private void OnShowHpSwithBtnChanged(bool isOpen)
	{
		if (GameSetting.Data.ShowHPBar != isOpen)
		{
			GameSetting.Data.ShowHPBar = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnShowCCSwithBtnChanged(bool isOpen)
	{
		if (isOpen)
		{
			SdkU3d.ntCCStartService();
		}
		else
		{
			SdkU3d.ntCCStopService();
		}
	}
}
