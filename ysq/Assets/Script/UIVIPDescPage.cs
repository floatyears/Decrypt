using Att;
using System;
using UnityEngine;

public sealed class UIVIPDescPage : MonoBehaviour
{
	private int vipLevel;

	private VipLevelInfo vipLevelInfo;

	private UISprite btnPageLeft;

	private UISprite btnPageRight;

	private Transform groupPage;

	private UIPanel mClip;

	private SpringPanel mSpPanel;

	private UIVIPDesc groupPageDesc;

	private UIVIPGiftPacks groupGiftPacks;

	private void Awake()
	{
		this.btnPageLeft = base.transform.FindChild("button-left").GetComponent<UISprite>();
		UIEventListener expr_2B = UIEventListener.Get(this.btnPageLeft.gameObject);
		expr_2B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2B.onClick, new UIEventListener.VoidDelegate(this.OnPageLeftClicked));
		this.btnPageRight = base.transform.FindChild("button-right").GetComponent<UISprite>();
		UIEventListener expr_77 = UIEventListener.Get(this.btnPageRight.gameObject);
		expr_77.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_77.onClick, new UIEventListener.VoidDelegate(this.OnPageRightClicked));
		this.mClip = base.transform.FindChild("Clip").GetComponent<UIPanel>();
		this.groupPage = this.mClip.transform.FindChild("Group1");
		this.groupPageDesc = this.groupPage.FindChild("VipBg").gameObject.AddComponent<UIVIPDesc>();
		this.groupGiftPacks = this.groupPage.FindChild("Packs").gameObject.AddComponent<UIVIPGiftPacks>();
	}

	public void Refresh(int viplevel)
	{
		if (viplevel <= 0 || this.vipLevel > 15)
		{
			viplevel = (int)Globals.Instance.Player.Data.VipLevel;
		}
		this.vipLevel = Mathf.Clamp(viplevel, 1, 15);
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mSpPanel == null)
		{
			this.mSpPanel = this.mClip.GetComponent<SpringPanel>();
		}
		if (this.mSpPanel != null && this.mSpPanel.enabled)
		{
			Vector3 target = this.mSpPanel.target;
			target.y = -32f;
			this.mSpPanel.target = target;
		}
		else
		{
			Vector3 localPosition = this.mClip.transform.localPosition;
			localPosition.y = -34f;
			this.mClip.transform.localPosition = localPosition;
			this.mClip.clipOffset = Vector2.zero;
		}
		GameUIVip.lastLookVIPDesc = this.vipLevel;
		this.vipLevelInfo = Globals.Instance.AttDB.VipLevelDict.GetInfo(this.vipLevel);
		this.groupPageDesc.Refresh(this.vipLevelInfo);
		this.groupGiftPacks.Refresh(this.vipLevelInfo);
		NGUITools.SetActive(this.btnPageLeft.gameObject, this.vipLevel > 1);
		NGUITools.SetActive(this.btnPageRight.gameObject, this.vipLevel < 15);
	}

	private void OnPageLeftClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.vipLevel == 1)
		{
			return;
		}
		this.vipLevel--;
		float num = 0.2f;
		TweenPosition.Begin(this.groupPage.gameObject, 0f, Vector3.zero);
		TweenPosition.Begin(this.groupPage.gameObject, num, new Vector3(870f, 0f, 0f)).method = UITweener.Method.EaseOut;
		base.Invoke("EndLeftAnimation", num);
	}

	private void EndLeftAnimation()
	{
		this.Refresh();
		float duration = 0.2f;
		TweenPosition.Begin(this.groupPage.gameObject, 0f, new Vector3(-870f, 0f, 0f));
		TweenPosition.Begin(this.groupPage.gameObject, duration, Vector3.zero).method = UITweener.Method.EaseIn;
	}

	private void OnPageRightClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.vipLevel == 15)
		{
			return;
		}
		this.vipLevel++;
		float num = 0.2f;
		TweenPosition.Begin(this.groupPage.gameObject, 0f, Vector3.zero);
		TweenPosition.Begin(this.groupPage.gameObject, num, new Vector3(-870f, 0f, 0f)).method = UITweener.Method.EaseOut;
		base.Invoke("EndRightAnimation", num);
	}

	private void EndRightAnimation()
	{
		this.Refresh();
		float duration = 0.2f;
		TweenPosition.Begin(this.groupPage.gameObject, 0f, new Vector3(870f, 0f, 0f));
		TweenPosition.Begin(this.groupPage.gameObject, duration, Vector3.zero).method = UITweener.Method.EaseIn;
	}

	public void OnBuyVipRewardCallback()
	{
		if (this.vipLevelInfo == null)
		{
			return;
		}
		this.groupGiftPacks.Refresh(this.vipLevelInfo);
	}
}
