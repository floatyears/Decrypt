using System;
using UnityEngine;

namespace Kogarasi.WebView
{
	public class WebViewAndroid : IWebView
	{
		private AndroidJavaObject webView;

		private string inputString = string.Empty;

		public void Init(string name)
		{
			this.webView = new AndroidJavaObject("com.kogarasi.unity.webview.WebViewPlugin", new object[0]);
			this.SafeCall("Init", new object[]
			{
				name
			});
		}

		public void Term()
		{
			this.SafeCall("Destroy", new object[0]);
		}

		public void SetMargins(int left, int top, int right, int bottom)
		{
			this.SafeCall("SetMargins", new object[]
			{
				left,
				top,
				right,
				bottom
			});
		}

		public void SetVisibility(bool state)
		{
			this.SafeCall("SetVisibility", new object[]
			{
				state
			});
		}

		public void LoadURL(string url)
		{
			this.SafeCall("LoadURL", new object[]
			{
				url
			});
		}

		public void EvaluateJS(string js)
		{
			this.SafeCall("LoadURL", new object[]
			{
				"javascript:" + js
			});
		}

		public bool CanGoBack()
		{
			this.SafeCall("CanGoBack", new object[0]);
			return this.SafeCallStatic("CanGoBackStatic", new object[0]);
		}

		public void GoBack()
		{
			this.SafeCall("GoBack", new object[0]);
		}

		public void ReLoad()
		{
			this.SafeCall("ReLoad", new object[0]);
		}

		private void SafeCall(string method, params object[] args)
		{
			if (this.webView != null)
			{
				this.webView.Call(method, args);
			}
			else
			{
				global::Debug.LogError(new object[]
				{
					"webview is not created. you check is a call 'Init' method"
				});
			}
		}

		private bool SafeCallStatic(string method, params object[] args)
		{
			bool result = false;
			if (this.webView != null)
			{
				result = this.webView.CallStatic<bool>(method, args);
			}
			else
			{
				global::Debug.LogError(new object[]
				{
					"webview is not created. you check is a call 'Init' method"
				});
			}
			return result;
		}
	}
}
