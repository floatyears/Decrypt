using LitJson;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace cn.sharesdk.unity3d
{
	public class ShareSDK
	{
		public class ShareConfigData
		{
			public string swKey;

			public string swSecret;

			public string swUrl;

			public string wxID;

			public string wxSecret;

			public string yxID;

			public string url;

			public string from;
		}

		public static ShareSDK.ShareConfigData ShareConfig = new ShareSDK.ShareConfigData();

		public static string IconPath = Application.persistentDataPath + "/app_icon.png";

		public static ShareSDKCallback shareSDK;

		public static void setCallbackModule(ShareSDKCallback callback)
		{
			ShareSDK.shareSDK = callback;
			ShareUtils.setCallbackObjectName(callback.name);
		}

		public static void InitShareParam(string shareParam)
		{
			try
			{
				ShareSDK.ShareConfig = JsonMapper.ToObject<ShareSDK.ShareConfigData>(shareParam);
				ShareUtils.setPlatformConfig(PlatformType.SinaWeibo, new Hashtable
				{
					{
						"AppKey",
						ShareSDK.ShareConfig.swKey
					},
					{
						"AppSecret",
						ShareSDK.ShareConfig.swSecret
					},
					{
						"RedirectUrl",
						ShareSDK.ShareConfig.swUrl
					}
				});
				Hashtable hashtable = new Hashtable();
				hashtable.Add("AppId", ShareSDK.ShareConfig.wxID);
				hashtable.Add("AppSecret", ShareSDK.ShareConfig.wxSecret);
				ShareUtils.setPlatformConfig(PlatformType.WeChatSession, hashtable);
				ShareUtils.setPlatformConfig(PlatformType.WeChatTimeline, hashtable);
				ShareUtils.setPlatformConfig(PlatformType.YiXinTimeline, new Hashtable
				{
					{
						"AppId",
						ShareSDK.ShareConfig.yxID
					}
				});
			}
			catch (Exception ex)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Parse ShareConfigData Json Error, {0}", ex.Message)
				});
			}
		}

		public static void Init()
		{
			ShareUtils.open("67686ad0a6ce");
			ShareSDK.shareSDK.StartCoroutine(ShareSDK.CopyIconFileForAndroidShare());
		}

		[DebuggerHidden]
		private static IEnumerator CopyIconFileForAndroidShare()
		{
            return null;
            //return new ShareSDK.<CopyIconFileForAndroidShare>c__Iterator8();
		}

		private static void CopyIconFileForIOSShare()
		{
			string iconPath = Application.dataPath + "/../AppIcon57x57@2x.png";
			ShareSDK.IconPath = iconPath;
		}

		public static void Share(PlatformType platform, string title, string text, string image, ContentType type, ShareResultEvent evt)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["title"] = ((platform != PlatformType.WeChatTimeline) ? title : text);
			hashtable["content"] = ((type != ContentType.News) ? (text + ShareSDK.ShareConfig.url) : text);
			hashtable["image"] = ((type != ContentType.Image) ? ((type != ContentType.News) ? string.Empty : ShareSDK.IconPath) : image);
			hashtable["url"] = ((type != ContentType.News) ? string.Empty : ShareSDK.ShareConfig.url);
			hashtable["type"] = Convert.ToString((int)type);
			ShareUtils.shareContent(platform, hashtable, evt);
		}
	}
}
