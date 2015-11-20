using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIUpgradeTipPopUp : GameUIBasePopup
{
	public delegate void VoidCallBack();

	public GUIUpgradeTipPopUp.VoidCallBack OnCloseEvent;

	private static GUIUpgradeTipPopUp mInstance;

	private UIPanel mPanel;

	private Transform mWindowBg;

	private UILabel mMaster;

	private UILabel mMasterTitle;

	private UILabel mUpgrade;

	private UILabel mUpgradeTitle;

	private float mVisibleTime = 1f;

	private float mDestroyTime;

	private float timer;

	private int state;

	public static void ShowThis(string bottomTitle, string bottomContent, string topTitle, string topContent, float destroyTime = 5f, float visibleTime = 1f)
	{
		if ((string.IsNullOrEmpty(bottomTitle) || string.IsNullOrEmpty(bottomContent)) && (string.IsNullOrEmpty(topTitle) || string.IsNullOrEmpty(topContent)))
		{
			global::Debug.LogError(new object[]
			{
				"string is null"
			});
			return;
		}
		if (GUIUpgradeTipPopUp.mInstance == null)
		{
			GUIUpgradeTipPopUp.CreateInstance();
		}
		GUIUpgradeTipPopUp.mInstance.Show(bottomTitle, bottomContent, topTitle, topContent, destroyTime, visibleTime);
	}

	private static void CreateInstance()
	{
		if (GUIUpgradeTipPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIUpgradeTipPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIUpgradeTipPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIUpgradeTipPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 3000f);
		UIPanel uIPanel = gameObject2.transform.GetComponent<UIPanel>();
		if (uIPanel == null)
		{
			uIPanel = gameObject2.gameObject.AddComponent<UIPanel>();
		}
		uIPanel.depth = 500;
		uIPanel.renderQueue = UIPanel.RenderQueue.StartAt;
		uIPanel.startingRenderQueue = 5501;
		GUIUpgradeTipPopUp.mInstance = gameObject2.AddComponent<GUIUpgradeTipPopUp>();
	}

	public static GUIUpgradeTipPopUp GetInstance()
	{
		if (GUIUpgradeTipPopUp.mInstance == null)
		{
			GUIUpgradeTipPopUp.CreateInstance();
		}
		return GUIUpgradeTipPopUp.mInstance;
	}

	public static void TryClose()
	{
		if (GUIUpgradeTipPopUp.mInstance != null)
		{
			GUIUpgradeTipPopUp.mInstance.CloseImmediate();
		}
	}

	public static void TryHide()
	{
		if (GUIUpgradeTipPopUp.mInstance != null)
		{
			GUIUpgradeTipPopUp.mInstance.HideImmediate();
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPanel = base.gameObject.GetComponent<UIPanel>();
		this.mWindowBg = GameUITools.FindGameObject("WindowBg", base.gameObject).transform;
		this.mMaster = GameUITools.FindUILabel("Master", this.mWindowBg.gameObject);
		this.mMasterTitle = GameUITools.FindUILabel("Title", this.mMaster.gameObject);
		this.mUpgrade = GameUITools.FindUILabel("Upgrade", this.mWindowBg.gameObject);
		this.mUpgradeTitle = GameUITools.FindUILabel("Title", this.mUpgrade.gameObject);
	}

	private void Show(string str0, string str1, string str2, string str3, float destroyTime, float visibleTime)
	{
		if (this.mPanel != null && HOTween.IsTweening(this.mPanel))
		{
			HOTween.Complete(this.mPanel);
		}
		this.mUpgradeTitle.text = str0;
		this.mUpgrade.text = str1;
		this.mMasterTitle.text = str2;
		this.mMaster.text = str3;
		this.mDestroyTime = destroyTime;
		this.mVisibleTime = visibleTime;
		this.mMaster.transform.localPosition = new Vector3(0f, 9f, 0f);
		this.mUpgrade.topAnchor.absolute = -85;
		if (string.IsNullOrEmpty(str0) || string.IsNullOrEmpty(str1))
		{
			this.mUpgrade.gameObject.SetActive(false);
		}
		else
		{
			this.mUpgrade.gameObject.SetActive(true);
		}
		if (string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str3))
		{
			this.mMaster.gameObject.SetActive(false);
			this.mUpgrade.transform.localPosition = Vector3.zero;
			this.mUpgrade.topAnchor.absolute = (int)((float)((88 + this.mUpgrade.height) / 2 - 68) - this.mMaster.transform.localPosition.y);
		}
		else
		{
			this.mUpgrade.topAnchor.target = this.mMaster.transform;
			this.mUpgrade.topAnchor.absolute = -85;
			this.mMaster.gameObject.SetActive(true);
		}
		NGUITools.SetActive(base.gameObject, true);
		this.mPanel.alpha = 1f;
		if (HOTween.IsTweening(this.mWindowBg))
		{
			HOTween.Kill(this.mWindowBg);
		}
		TweenAlpha.Begin(this.mWindowBg.gameObject, 0f, 0f);
		TweenAlpha.Begin(this.mWindowBg.gameObject, 0.25f, 1f);
		HOTween.To(this.mWindowBg, 0f, new TweenParms().Prop("localScale", Vector3.zero));
		HOTween.To(this.mWindowBg, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack).OnComplete(new TweenDelegate.TweenCallback(this.OnOpenOver)));
		this.ChangeState(1);
	}

	private void OnOpenOver()
	{
		this.ChangeState(2);
	}

	private void Hide()
	{
		this.ChangeState(3);
		HOTween.To(this.mPanel, 0.1f, new TweenParms().Prop("alpha", 0).OnComplete(new TweenDelegate.TweenCallback(this.OnHideOver)));
	}

	private void OnHideOver()
	{
		this.ChangeState(4);
		if (this.OnCloseEvent != null)
		{
			this.OnCloseEvent();
		}
	}

	private void ChangeState(int value)
	{
		this.state = value;
		this.timer = Time.time;
	}

	private void LateUpdate()
	{
		switch (this.state)
		{
		case 2:
			if (Time.time - this.timer > this.mVisibleTime)
			{
				this.Hide();
			}
			break;
		case 4:
			if (Time.time - this.timer > this.mDestroyTime)
			{
				this.CloseImmediate();
			}
			break;
		}
	}

	public void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIUpgradeTipPopUp.mInstance.gameObject);
	}

	public void HideImmediate()
	{
		this.ChangeState(3);
		this.mPanel.alpha = 0f;
		this.OnHideOver();
	}
}
