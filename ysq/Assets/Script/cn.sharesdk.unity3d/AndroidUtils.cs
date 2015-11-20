using System;
using System.Collections;
using UnityEngine;
using XUPorterJSON;

namespace cn.sharesdk.unity3d
{
	public class AndroidUtils
	{
		private static AndroidUtils instance;

		private AndroidJavaClass ssdk;

		private AuthResultEvent authHandler;

		private GetUserInfoResultEvent showUserHandler;

		private ShareResultEvent shareHandler;

		private AndroidUtils()
		{
			this.ssdk = new AndroidJavaClass("cn.sharesdk.unity3d.ShareSDKUtils");
		}

		public static AndroidUtils getInstance()
		{
			if (AndroidUtils.instance == null)
			{
				AndroidUtils.instance = new AndroidUtils();
			}
			return AndroidUtils.instance;
		}

		public void setGameObject(string gameObject)
		{
			this.ssdk.CallStatic("setGameObject", new object[]
			{
				gameObject,
				"_callback"
			});
		}

		public void initSDK()
		{
			this.initSDK(null);
		}

		public void initSDK(string appKey)
		{
			this.ssdk.CallStatic("initSDK", new object[]
			{
				appKey
			});
		}

		public void stopSDK()
		{
			this.ssdk.CallStatic("stopSDK", new object[0]);
		}

		public void setPlatformConfig(int platform, Hashtable configs)
		{
			string text = MiniJSON.jsonEncode(configs);
			this.ssdk.CallStatic("setPlatformConfig", new object[]
			{
				platform,
				text
			});
		}

		public void authorize(int platform, AuthResultEvent resultHandler)
		{
			this.authHandler = resultHandler;
			this.ssdk.CallStatic("authorize", new object[]
			{
				platform
			});
		}

		public void removeAccount(int platform)
		{
			this.ssdk.CallStatic("removeAccount", new object[]
			{
				platform
			});
		}

		public bool isValid(int platform)
		{
			return this.ssdk.CallStatic<bool>("isValid", new object[]
			{
				platform
			});
		}

		public void showUser(int platform, GetUserInfoResultEvent resultHandler)
		{
			this.showUserHandler = resultHandler;
			this.ssdk.CallStatic("showUser", new object[]
			{
				platform
			});
		}

		public void share(int platform, Hashtable content, ShareResultEvent resultHandler)
		{
			this.shareHandler = resultHandler;
			string text = MiniJSON.jsonEncode(content);
			this.ssdk.CallStatic("share", new object[]
			{
				platform,
				text
			});
		}

		public void onekeyShare(Hashtable content, ShareResultEvent resultHandler)
		{
			this.onekeyShare(0, content, resultHandler);
		}

		public void onekeyShare(int platform, Hashtable content, ShareResultEvent resultHandler)
		{
			this.shareHandler = resultHandler;
			string text = MiniJSON.jsonEncode(content);
			this.ssdk.CallStatic("onekeyShare", new object[]
			{
				platform,
				text
			});
		}

		public void onActionCallback(string message)
		{
			if (message == null)
			{
				return;
			}
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(message);
			if (hashtable == null || hashtable.Count <= 0)
			{
				return;
			}
			int num = Convert.ToInt32(hashtable["status"]);
			int platform = Convert.ToInt32(hashtable["platform"]);
			int action = Convert.ToInt32(hashtable["action"]);
			switch (num)
			{
			case 1:
			{
				Console.WriteLine(message);
				Hashtable res = (Hashtable)hashtable["res"];
				this.OnComplete(platform, action, res);
				break;
			}
			case 2:
			{
				Console.WriteLine(message);
				Hashtable throwable = (Hashtable)hashtable["res"];
				this.OnError(platform, action, throwable);
				break;
			}
			case 3:
				this.OnCancel(platform, action);
				break;
			}
		}

		private void OnError(int platform, int action, Hashtable throwable)
		{
			if (action != 8)
			{
				if (action != 9)
				{
					if (action == 1)
					{
						if (this.authHandler != null)
						{
							this.authHandler(ResponseState.Fail, (PlatformType)platform, throwable);
						}
					}
				}
				else if (this.shareHandler != null)
				{
					this.shareHandler(ResponseState.Fail, (PlatformType)platform, null, throwable, false);
				}
			}
			else if (this.showUserHandler != null)
			{
				this.showUserHandler(ResponseState.Fail, (PlatformType)platform, null, throwable);
			}
		}

		private void OnComplete(int platform, int action, Hashtable res)
		{
			if (action != 8)
			{
				if (action != 9)
				{
					if (action == 1)
					{
						if (this.authHandler != null)
						{
							this.authHandler(ResponseState.Success, (PlatformType)platform, null);
						}
					}
				}
				else if (this.shareHandler != null)
				{
					this.shareHandler(ResponseState.Success, (PlatformType)platform, res, null, false);
				}
			}
			else if (this.showUserHandler != null)
			{
				this.showUserHandler(ResponseState.Success, (PlatformType)platform, res, null);
			}
		}

		private void OnCancel(int platform, int action)
		{
			if (action != 8)
			{
				if (action != 9)
				{
					if (action == 1)
					{
						if (this.authHandler != null)
						{
							this.authHandler(ResponseState.Cancel, (PlatformType)platform, null);
						}
					}
				}
				else if (this.shareHandler != null)
				{
					this.shareHandler(ResponseState.Cancel, (PlatformType)platform, null, null, false);
				}
			}
			else if (this.showUserHandler != null)
			{
				this.showUserHandler(ResponseState.Cancel, (PlatformType)platform, null, null);
			}
		}
	}
}
