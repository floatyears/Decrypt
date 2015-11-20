using NtUniSdk.Unity3d;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAgreementInfoPopUp : GameUIBasePopup
{
	private Dictionary<string, string> agreementDict;

	private int maxPage;

	private int index;

	private UILabel mInfo;

	private UIScrollBar mBar;

	private UILabel mPages;

	private bool needAgree;

	public static void Show(bool needAgree = false)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIAgreementInfoPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(needAgree);
	}

	private bool LoadAgreementTable()
	{
		string path = string.Format("Globalization/{0}/{1}", GameSetting.Language, "Agreement");
		TextAsset textAsset = Res.Load<TextAsset>(path, false);
		return !(textAsset == null) && StringManager.ParseString(ref this.agreementDict, textAsset.text);
	}

	public int GetAgreementCount()
	{
		if (this.agreementDict == null)
		{
			return 0;
		}
		return this.agreementDict.Count;
	}

	public string GetAgreement(string key)
	{
		if (this.agreementDict == null)
		{
			return string.Empty;
		}
		string result;
		if (!this.agreementDict.TryGetValue(key, out result))
		{
			return string.Empty;
		}
		return result;
	}

	public override void InitPopUp(bool value = false)
	{
		this.needAgree = value;
		this.LoadAgreementTable();
		GameObject parent = GameUITools.FindGameObject("winBG", base.gameObject);
		GameUITools.RegisterClickEvent("closeBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), parent).SetActive(!this.needAgree);
		GameUITools.RegisterClickEvent("Previous", new UIEventListener.VoidDelegate(this.OnPreviousClick), parent);
		GameUITools.RegisterClickEvent("Next", new UIEventListener.VoidDelegate(this.OnNextClick), parent);
		GameUITools.RegisterClickEvent("Disagree", new UIEventListener.VoidDelegate(this.OnDisagreeClick), parent).SetActive(this.needAgree);
		GameUITools.RegisterClickEvent("Agree", new UIEventListener.VoidDelegate(this.OnAgreeClick), parent).SetActive(this.needAgree);
		GameUITools.FindUILabel("title", parent).text = this.GetAgreement("Title");
		this.mInfo = GameUITools.FindUILabel("contentsPanel/contents/info", parent);
		this.mBar = GameUITools.FindGameObject("contentsScrollBar", parent).GetComponent<UIScrollBar>();
		this.maxPage = this.GetAgreementCount() - 1;
		this.mPages = GameUITools.FindUILabel("Pages", parent);
		this.Refresh();
	}

	private void OnPreviousClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.index--;
		this.Refresh();
	}

	private void OnNextClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.index++;
		this.Refresh();
	}

	private void Refresh()
	{
		this.index = Mathf.Clamp(this.index, 0, this.maxPage - 1);
		this.mInfo.text = this.GetAgreement(this.index.ToString());
		this.mBar.value = 0f;
		this.mPages.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
		{
			this.index + 1,
			this.maxPage
		});
	}

	private void OnDisagreeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		SdkU3d.exit();
	}

	private void OnAgreeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIAgreementInfoPopUp.SetUserAgreement(true);
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public static void SetUserAgreement(bool value)
	{
		GameSetting.Data.UserAgreement = value;
		GameSetting.UpdateNow = true;
	}

	private void OnCloseClick(GameObject go)
	{
		if (this.needAgree)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public override void OnButtonBlockerClick()
	{
		if (this.needAgree)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
