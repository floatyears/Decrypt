using Kogarasi.WebView;
using System;
using UnityEngine;

public class WebViewBehavior : MonoBehaviour
{
	private IWebView webView;

	private IWebViewCallback callback;

	public void Awake()
	{
		this.webView = new WebViewAndroid();
		this.webView.Init(base.name);
		this.callback = null;
	}

	public void OnDestroy()
	{
		this.webView.Term();
	}

	public void SetMargins(int left, int top, int right, int bottom)
	{
		this.webView.SetMargins(left, top, right, bottom);
	}

	public void SetVisibility(bool state)
	{
		this.webView.SetVisibility(state);
	}

	public void LoadURL(string url)
	{
		this.webView.LoadURL(url);
	}

	public void EvaluateJS(string js)
	{
		this.webView.EvaluateJS(js);
	}

	public bool CanGoBack()
	{
		return this.webView.CanGoBack();
	}

	public void GoBack()
	{
		this.webView.GoBack();
	}

	public void ReLoad()
	{
		this.webView.ReLoad();
	}

	public void setCallback(IWebViewCallback _callback)
	{
		this.callback = _callback;
	}

	public void onLoadStart(string url)
	{
		if (this.callback != null)
		{
			this.callback.onLoadStart(url);
		}
	}

	public void onLoadFinish(string url)
	{
		if (this.callback != null)
		{
			this.callback.onLoadFinish(url);
		}
	}

	public void onLoadFail(string url)
	{
		if (this.callback != null)
		{
			this.callback.onLoadFail(url);
		}
	}
}
