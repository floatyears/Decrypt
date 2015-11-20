using Holoville.HOTween.Core;
using LitJson;
using NtUniSdk.Unity3d;
using System;
using UnityEngine;

public class GUISystemSettingPopUp : GameUISession
{
	public enum ESettingLayer
	{
		ESL_Null,
		ESL_GameSet,
		ESL_Notify,
		ESL_PlayerInfo
	}

	private SystemSettingGameSetLayer mSystemSettingGameSetLayer;

	private SystemSettingNotifyLayer mSystemSettingNotifyLayer;

	private SystemSettingPlayerInfoLayer mSystemSettingPlayerInfoLayer;

	private UIToggle mPlayerInfoToggle;

	private GameObject mPlayerInfoToggleNew;

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		GameUITools.PlayOpenWindowAnim(base.transform, null, true);
		LocalPlayer expr_1D = Globals.Instance.Player;
		expr_1D.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_1D.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.mSystemSettingPlayerInfoLayer.OnPlayerUpdateEvent));
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("UIMiddle").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("BG").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject2);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnBGClick));
		GameObject gameObject3 = gameObject.transform.Find("WindowBg").gameObject;
		GameObject gameObject4 = gameObject3.transform.Find("closeBtn").gameObject;
		UIEventListener expr_85 = UIEventListener.Get(gameObject4);
		expr_85.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_85.onClick, new UIEventListener.VoidDelegate(this.OnBGClick));
		this.mSystemSettingGameSetLayer = gameObject3.transform.Find("gameSetLayer").gameObject.AddComponent<SystemSettingGameSetLayer>();
		this.mSystemSettingGameSetLayer.InitWithBaseScene();
		this.mSystemSettingNotifyLayer = gameObject3.transform.Find("notifyLayer").gameObject.AddComponent<SystemSettingNotifyLayer>();
		this.mSystemSettingNotifyLayer.InitWithBaseScene();
		this.mSystemSettingPlayerInfoLayer = gameObject3.transform.Find("PlayerInfoLayer").gameObject.AddComponent<SystemSettingPlayerInfoLayer>();
		this.mSystemSettingPlayerInfoLayer.InitWithBaseScene(this);
		base.RegisterClickEvent("tab0", new UIEventListener.VoidDelegate(this.OnTabClick), gameObject3);
		base.RegisterClickEvent("tab1", new UIEventListener.VoidDelegate(this.OnTabClick), gameObject3);
		this.mPlayerInfoToggle = base.RegisterClickEvent("tab2", new UIEventListener.VoidDelegate(this.OnTabClick), gameObject3).GetComponent<UIToggle>();
		this.mPlayerInfoToggleNew = GameUITools.FindGameObject("new", this.mPlayerInfoToggle.gameObject);
		this.RefreshPlayerNew();
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	public void SelectTab(GUISystemSettingPopUp.ESettingLayer layer)
	{
		switch (layer)
		{
		case GUISystemSettingPopUp.ESettingLayer.ESL_PlayerInfo:
			if (this.mPlayerInfoToggle != null)
			{
				this.mPlayerInfoToggle.value = true;
			}
			break;
		}
	}

	protected override void OnPreDestroyGUI()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_0A.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.mSystemSettingPlayerInfoLayer.OnPlayerUpdateEvent));
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Remove(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Remove(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
	}

	private void OnBGClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		base.Close();
	}

	private void SDKEvent(int code, JsonData data)
	{
		this.RefreshPlayerNew();
	}

	public void RefreshPlayerNew()
	{
		if (this.mPlayerInfoToggleNew != null && this.mSystemSettingPlayerInfoLayer.mQuitNew != null)
		{
			this.mSystemSettingPlayerInfoLayer.mQuitNew.SetActive(GameUIManager.mInstance.uiState.SDKPlayerManagerNew);
			this.mPlayerInfoToggleNew.gameObject.SetActive(GameUIManager.mInstance.uiState.SDKPlayerManagerNew);
		}
	}
}
