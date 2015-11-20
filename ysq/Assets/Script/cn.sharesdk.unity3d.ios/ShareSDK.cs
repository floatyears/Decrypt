using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using XUPorterJSON;

namespace cn.sharesdk.unity3d.ios
{
	public static class ShareSDK
	{
		private static AuthResultEvent _authResultEvent;

		private static GetUserInfoResultEvent _getUserInfoResultEvent;

		private static ShareResultEvent _shareResultEvent;

		private static GetFriendsResultEvent _getFriendsResultEvent;

		private static GetCredentialResultEvent _getCredentialResultEvent;

		private static string _callbackObjectName = "Main Camera";

		[DllImport("__Internal")]
		private static extern void __iosShareSDKOpen(string appKey);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKSetPlatformConfig(int platType, string configInfo);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKAuthorize(int platType, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKCancelAuthorize(int platType);

		[DllImport("__Internal")]
		private static extern bool __iosShareSDKHasAuthorized(int platType);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKGetUserInfo(int platType, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKShare(int platType, string content, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKOneKeyShare(string platTypes, string content, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKShowShareMenu(string platTypes, string content, int x, int y, int direction, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKShowShareView(int platType, string content, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKGetFriendsList(int platType, string page, string observer);

		[DllImport("__Internal")]
		private static extern void __iosShareSDKGetCredential(int platType, string observer);

		[DllImport("__Internal")]
		private static extern bool __iosShareSDKIsClientInstalled(int platType);

		private static void _authorizeResultHandler(Hashtable data)
		{
			ResponseState state = ResponseState.Cancel;
			PlatformType type = PlatformType.Any;
			Hashtable error = null;
			if (data.ContainsKey("state"))
			{
				state = (ResponseState)Convert.ToInt32(data["state"]);
			}
			if (data.ContainsKey("type"))
			{
				type = (PlatformType)Convert.ToInt32(data["type"]);
			}
			if (data.ContainsKey("error"))
			{
				error = (Hashtable)data["error"];
			}
			ShareSDK._authResultEvent(state, type, error);
		}

		private static void _getUserInfoResultHandler(Hashtable data)
		{
			ResponseState state = ResponseState.Cancel;
			PlatformType type = PlatformType.Any;
			Hashtable userInfo = null;
			Hashtable error = null;
			if (data.ContainsKey("state"))
			{
				state = (ResponseState)Convert.ToInt32(data["state"]);
			}
			if (data.ContainsKey("type"))
			{
				type = (PlatformType)Convert.ToInt32(data["type"]);
			}
			if (data.ContainsKey("user"))
			{
				userInfo = (Hashtable)data["user"];
			}
			if (data.ContainsKey("error"))
			{
				error = (Hashtable)data["error"];
			}
			ShareSDK._getUserInfoResultEvent(state, type, userInfo, error);
		}

		private static void _shareResultHandler(Hashtable data)
		{
			ResponseState state = ResponseState.Cancel;
			PlatformType type = PlatformType.Any;
			bool end = true;
			Hashtable shareInfo = null;
			Hashtable error = null;
			if (data.ContainsKey("state"))
			{
				state = (ResponseState)Convert.ToInt32(data["state"]);
			}
			if (data.ContainsKey("type"))
			{
				type = (PlatformType)Convert.ToInt32(data["type"]);
			}
			if (data.ContainsKey("end"))
			{
				end = Convert.ToBoolean(data["end"]);
			}
			if (data.ContainsKey("share_info"))
			{
				shareInfo = (Hashtable)data["share_info"];
			}
			if (data.ContainsKey("error"))
			{
				error = (Hashtable)data["error"];
			}
			ShareSDK._shareResultEvent(state, type, shareInfo, error, end);
		}

		private static void _getFriendsResultHandler(Hashtable data)
		{
			ResponseState state = ResponseState.Cancel;
			PlatformType type = PlatformType.Any;
			ArrayList users = null;
			Hashtable error = null;
			if (data.ContainsKey("state"))
			{
				state = (ResponseState)Convert.ToInt32(data["state"]);
			}
			if (data.ContainsKey("type"))
			{
				type = (PlatformType)Convert.ToInt32(data["type"]);
			}
			if (data.ContainsKey("users"))
			{
				users = (ArrayList)data["users"];
			}
			if (data.ContainsKey("error"))
			{
				error = (Hashtable)data["error"];
			}
			ShareSDK._getFriendsResultEvent(state, type, users, error);
		}

		private static void _getCredentialResultHandler(Hashtable data)
		{
			ResponseState state = ResponseState.Cancel;
			PlatformType type = PlatformType.Any;
			Hashtable credential = null;
			Hashtable error = null;
			if (data.ContainsKey("state"))
			{
				state = (ResponseState)Convert.ToInt32(data["state"]);
			}
			if (data.ContainsKey("type"))
			{
				type = (PlatformType)Convert.ToInt32(data["type"]);
			}
			if (data.ContainsKey("credential"))
			{
				credential = (Hashtable)data["credential"];
			}
			if (data.ContainsKey("error"))
			{
				error = (Hashtable)data["error"];
			}
			ShareSDK._getCredentialResultEvent(state, type, credential, error);
		}

		public static void callback(string data)
		{
			object obj = MiniJSON.jsonDecode(data);
			if (obj is Hashtable)
			{
				Hashtable hashtable = obj as Hashtable;
				if (hashtable != null && hashtable.ContainsKey("action"))
				{
					switch (Convert.ToInt32(hashtable["action"]))
					{
					case 1:
						ShareSDK._authorizeResultHandler(hashtable);
						break;
					case 2:
						ShareSDK._getUserInfoResultHandler(hashtable);
						break;
					case 3:
						ShareSDK._shareResultHandler(hashtable);
						break;
					case 4:
						ShareSDK._getFriendsResultHandler(hashtable);
						break;
					case 5:
						ShareSDK._getCredentialResultHandler(hashtable);
						break;
					}
				}
			}
		}

		public static void setCallbackObjectName(string objectName)
		{
			ShareSDK._callbackObjectName = objectName;
		}

		public static void open(string appKey)
		{
			ShareSDK.__iosShareSDKOpen(appKey);
		}

		public static void close()
		{
		}

		public static void setPlatformConfig(PlatformType type, Hashtable configInfo)
		{
			string configInfo2 = MiniJSON.jsonEncode(configInfo);
			ShareSDK.__iosShareSDKSetPlatformConfig((int)type, configInfo2);
		}

		public static void authorize(PlatformType type, AuthResultEvent resultHandler)
		{
			ShareSDK._authResultEvent = resultHandler;
			ShareSDK.__iosShareSDKAuthorize((int)type, ShareSDK._callbackObjectName);
		}

		public static void cancelAuthorie(PlatformType type)
		{
			ShareSDK.__iosShareSDKCancelAuthorize((int)type);
		}

		public static bool hasAuthorized(PlatformType type)
		{
			return ShareSDK.__iosShareSDKHasAuthorized((int)type);
		}

		public static bool isClientInstalled(PlatformType type)
		{
			return ShareSDK.__iosShareSDKIsClientInstalled((int)type);
		}

		public static void getUserInfo(PlatformType type, GetUserInfoResultEvent resultHandler)
		{
			ShareSDK._getUserInfoResultEvent = resultHandler;
			ShareSDK.__iosShareSDKGetUserInfo((int)type, ShareSDK._callbackObjectName);
		}

		public static void shareContent(PlatformType type, Hashtable content, ShareResultEvent resultHandler)
		{
			ShareSDK._shareResultEvent = resultHandler;
			string content2 = MiniJSON.jsonEncode(content);
			ShareSDK.__iosShareSDKShare((int)type, content2, ShareSDK._callbackObjectName);
		}

		public static void oneKeyShareContent(PlatformType[] types, Hashtable content, ShareResultEvent resultHandler)
		{
			ShareSDK._shareResultEvent = resultHandler;
			List<PlatformType> json = new List<PlatformType>(types);
			string platTypes = MiniJSON.jsonEncode(json);
			string content2 = MiniJSON.jsonEncode(content);
			ShareSDK.__iosShareSDKOneKeyShare(platTypes, content2, ShareSDK._callbackObjectName);
		}

		public static void showShareMenu(PlatformType[] types, Hashtable content, int x, int y, MenuArrowDirection direction, ShareResultEvent resultHandler)
		{
			ShareSDK._shareResultEvent = resultHandler;
			List<PlatformType> json = new List<PlatformType>(types);
			string platTypes = MiniJSON.jsonEncode(json);
			string content2 = MiniJSON.jsonEncode(content);
			ShareSDK.__iosShareSDKShowShareMenu(platTypes, content2, x, y, (int)direction, ShareSDK._callbackObjectName);
		}

		public static void showShareView(PlatformType type, Hashtable content, ShareResultEvent resultHandler)
		{
			ShareSDK._shareResultEvent = resultHandler;
			string content2 = MiniJSON.jsonEncode(content);
			ShareSDK.__iosShareSDKShowShareView((int)type, content2, ShareSDK._callbackObjectName);
		}

		public static void getFriends(PlatformType type, Hashtable page, GetFriendsResultEvent resultHandler)
		{
			ShareSDK._getFriendsResultEvent = resultHandler;
			string page2 = MiniJSON.jsonEncode(page);
			ShareSDK.__iosShareSDKGetFriendsList((int)type, page2, ShareSDK._callbackObjectName);
		}

		public static void getCredential(PlatformType type, GetCredentialResultEvent resultHandler)
		{
			ShareSDK._getCredentialResultEvent = resultHandler;
			ShareSDK.__iosShareSDKGetCredential((int)type, ShareSDK._callbackObjectName);
		}
	}
}
