using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIWebViewPopUp : MonoBehaviour
{
	private GUIWebViewPopUpCallback mCallback;

	private GameObject mWinBg;

	private WebViewBehavior mWebViewBehavior;

	private UISprite mCloseBtnSp;

	private UISprite mBottomBar;

	private UIRoot mUIRoot;

	private UIToggle mCheckBtn;

	private GameObject mGoBackBtn;

	private UILabel mTitle;

	private string mUrl;

	private float timerRefresh;

	private void Awake()
	{
		this.CreateObjects();
	}

	public void ShowWebPage(string url, string title)
	{
		this.mUrl = url;
		if (!string.IsNullOrEmpty(title))
		{
			this.mTitle.text = title;
		}
		base.StartCoroutine(this.DoShowWebPage());
	}

	private void CreateObjects()
	{
		this.mWinBg = base.transform.Find("winBG").gameObject;
		this.mUIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		this.mTitle = this.mWinBg.transform.Find("title").GetComponent<UILabel>();
		this.mWebViewBehavior = this.mWinBg.AddComponent<WebViewBehavior>();
		this.mCheckBtn = this.mWinBg.transform.Find("bottomBar/checkBtn").GetComponent<UIToggle>();
		this.mCheckBtn.value = false;
		GameObject gameObject = this.mWinBg.transform.Find("closeBtn").gameObject;
		UIEventListener expr_AA = UIEventListener.Get(gameObject);
		expr_AA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AA.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		GameObject gameObject2 = this.mWinBg.transform.Find("reloadBtn").gameObject;
		UIEventListener expr_EC = UIEventListener.Get(gameObject2);
		expr_EC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_EC.onClick, new UIEventListener.VoidDelegate(this.OnReloadClick));
		gameObject2.SetActive(false);
		this.mGoBackBtn = this.mWinBg.transform.Find("backBtn").gameObject;
		UIEventListener expr_13F = UIEventListener.Get(this.mGoBackBtn);
		expr_13F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13F.onClick, new UIEventListener.VoidDelegate(this.OnGoBackClick));
		this.mCloseBtnSp = gameObject.GetComponent<UISprite>();
		this.mBottomBar = this.mWinBg.transform.Find("bottomBar").GetComponent<UISprite>();
		this.mBottomBar.gameObject.SetActive(false);
	}

	[DebuggerHidden]
	public IEnumerator DoShowWebPage()
	{
        return null;
        //GUIWebViewPopUp.<DoShowWebPage>c__Iterator8D <DoShowWebPage>c__Iterator8D = new GUIWebViewPopUp.<DoShowWebPage>c__Iterator8D();
        //<DoShowWebPage>c__Iterator8D.<>f__this = this;
        //return <DoShowWebPage>c__Iterator8D;
	}

	private void OnCloseClick(GameObject go)
	{
		if (this.mCheckBtn.value)
		{
			GameSetting.Data.WebViewDontShow = true;
			GameSetting.Data.WebViewDontShowTimeStamp = DateTime.Now.Ticks;
			GameSetting.UpdateNow = true;
		}
		this.mCallback = null;
		this.mWebViewBehavior = null;
		GameUIManager.mInstance.DestroyGUIWebViewPopUp();
	}

	private void OnReloadClick(GameObject go)
	{
		if (this.mWebViewBehavior != null)
		{
			this.mWebViewBehavior.ReLoad();
		}
	}

	private void OnGoBackClick(GameObject go)
	{
		if (this.mWebViewBehavior != null)
		{
			this.mWebViewBehavior.GoBack();
		}
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 0.1f)
		{
			this.timerRefresh = Time.time;
			this.Refresh();
		}
	}

	public void Refresh()
	{
		if (this.mWebViewBehavior != null)
		{
			this.mGoBackBtn.SetActive(this.mWebViewBehavior.CanGoBack());
		}
	}
}
