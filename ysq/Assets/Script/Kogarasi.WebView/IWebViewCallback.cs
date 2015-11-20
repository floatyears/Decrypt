using System;

namespace Kogarasi.WebView
{
	public interface IWebViewCallback
	{
		void onLoadStart(string url);

		void onLoadFinish(string url);

		void onLoadFail(string url);
	}
}
