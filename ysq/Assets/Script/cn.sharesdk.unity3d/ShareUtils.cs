using System;
using System.Collections;
using UnityEngine;

namespace cn.sharesdk.unity3d
{
	public class ShareUtils
	{
		private static readonly string INHERITED_VALUE = "{inherited}";

		public static void setCallbackObjectName(string objectName)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().setGameObject(objectName);
				}
			}
		}

		public static void open(string appKey)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().initSDK(appKey);
				}
			}
		}

		public static void close()
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().stopSDK();
				}
			}
		}

		public static void setPlatformConfig(PlatformType type, Hashtable configInfo)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().setPlatformConfig((int)type, configInfo);
				}
			}
		}

		public static void authorize(PlatformType type, AuthResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().authorize((int)type, resultHandler);
				}
			}
		}

		public static void cancelAuthorie(PlatformType type)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().removeAccount((int)type);
				}
			}
		}

		public static bool hasAuthorized(PlatformType type)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					return AndroidUtils.getInstance().isValid((int)type);
				}
			}
			return false;
		}

		public static bool isClientInstalled(PlatformType type)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					return false;
				}
			}
			return false;
		}

		public static void getUserInfo(PlatformType type, GetUserInfoResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().showUser((int)type, resultHandler);
				}
			}
		}

		public static void shareContent(PlatformType type, Hashtable content, ShareResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().share((int)type, content, resultHandler);
				}
			}
		}

		public static void oneKeyShareContent(PlatformType[] types, Hashtable content, ShareResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					for (int i = 0; i < types.Length; i++)
					{
						PlatformType platform = types[i];
						AndroidUtils.getInstance().share((int)platform, content, resultHandler);
					}
				}
			}
		}

		public static void showShareMenu(PlatformType[] types, Hashtable content, int x, int y, MenuArrowDirection direction, ShareResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().onekeyShare(content, resultHandler);
				}
			}
		}

		public static void showShareView(PlatformType type, Hashtable content, ShareResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().onekeyShare(content, resultHandler);
				}
			}
		}

		public static void getFriends(PlatformType type, Hashtable page, GetFriendsResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
				}
			}
		}

		public static void getCredential(PlatformType type, GetCredentialResultEvent resultHandler)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
				}
			}
		}

		public static void customSinaWeiboShareContent(Hashtable content, object message, object image, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						hashtable.Add("location", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("location", location);
					}
				}
				content.Add(PlatformType.SinaWeibo, hashtable);
			}
		}

		public static void customTencentWeiboShareContent(Hashtable content, object message, object image, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						hashtable.Add("location", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("location", location);
					}
				}
				content.Add(PlatformType.TencentWeibo, hashtable);
			}
		}

		public static void customDouBanShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.DouBan, hashtable);
			}
		}

		public static void customQZoneShareContent(Hashtable content, object title, object url, object site, object fromUrl, object comment, object summary, object image, object type, object playUrl, object nswb)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (site != null)
				{
					if (site is InheritedValue)
					{
						hashtable.Add("site", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("site", site);
					}
				}
				if (fromUrl != null)
				{
					if (fromUrl is InheritedValue)
					{
						hashtable.Add("fromUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("fromUrl", fromUrl);
					}
				}
				if (comment != null)
				{
					if (fromUrl is InheritedValue)
					{
						hashtable.Add("comment", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("comment", comment);
					}
				}
				if (summary != null)
				{
					if (fromUrl is InheritedValue)
					{
						hashtable.Add("summary", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("summary", summary);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (playUrl != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("playUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("playUrl", playUrl);
					}
				}
				if (nswb != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("nswb", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("nswb", nswb);
					}
				}
				content.Add(PlatformType.QZone, hashtable);
			}
		}

		public static void customRenrenShareContent(Hashtable content, object name, object description, object url, object message, object image, object caption)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (name != null)
				{
					if (name is InheritedValue)
					{
						hashtable.Add("name", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("name", name);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						hashtable.Add("description", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("description", description);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (caption != null)
				{
					if (caption is InheritedValue)
					{
						hashtable.Add("caption", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("caption", caption);
					}
				}
				content.Add(PlatformType.Renren, hashtable);
			}
		}

		public static void customKaixinShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.Kaixin, hashtable);
			}
		}

		public static void customFacebookShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.Facebook, hashtable);
			}
		}

		public static void customTwitterShareContent(Hashtable content, object message, object image, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						hashtable.Add("location", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("location", location);
					}
				}
				content.Add(PlatformType.Twitter, hashtable);
			}
		}

		public static void customEvernoteShareContent(Hashtable content, object message, object title, object resource, object notebookGuid, object tagsGuid)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (resource != null)
				{
					if (resource is InheritedValue)
					{
						hashtable.Add("resource", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("resource", resource);
					}
				}
				if (notebookGuid != null)
				{
					if (notebookGuid is InheritedValue)
					{
						hashtable.Add("notebookGuid", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("notebookGuid", notebookGuid);
					}
				}
				if (tagsGuid != null)
				{
					if (tagsGuid is InheritedValue)
					{
						hashtable.Add("tagsGuid", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("tagsGuid", tagsGuid);
					}
				}
				content.Add(PlatformType.Evernote, hashtable);
			}
		}

		public static void customGooglePlusShareContent(Hashtable content, object message, object image, object url, object deepLinkId, object title, object description, object thumbnail)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (deepLinkId != null)
				{
					if (deepLinkId is InheritedValue)
					{
						hashtable.Add("deepLinkId", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("deepLinkId", deepLinkId);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						hashtable.Add("description", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("description", description);
					}
				}
				if (thumbnail != null)
				{
					if (thumbnail is InheritedValue)
					{
						hashtable.Add("thumbnail", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("thumbnail", thumbnail);
					}
				}
				content.Add(PlatformType.GooglePlus, hashtable);
			}
		}

		public static void customInstagramShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.Instagram, hashtable);
			}
		}

		public static void customLinkedInShareContent(Hashtable content, object comment, object title, object description, object url, object image, object visibility)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (comment != null)
				{
					if (comment is InheritedValue)
					{
						hashtable.Add("comment", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("comment", comment);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						hashtable.Add("description", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("description", description);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (visibility != null)
				{
					if (visibility is InheritedValue)
					{
						hashtable.Add("visibility", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("visibility", visibility);
					}
				}
				content.Add(PlatformType.LinkedIn, hashtable);
			}
		}

		public static void customTumblrShareContent(Hashtable content, object text, object title, object image, object url, object blogName)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (text != null)
				{
					if (text is InheritedValue)
					{
						hashtable.Add("text", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("text", text);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (blogName != null)
				{
					if (blogName is InheritedValue)
					{
						hashtable.Add("blogName", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("blogName", blogName);
					}
				}
				content.Add(PlatformType.Tumblr, hashtable);
			}
		}

		public static void customMailShareContent(Hashtable content, object subject, object message, object isHTML, object attachments, object to, object cc, object bcc)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (subject != null)
				{
					if (subject is InheritedValue)
					{
						hashtable.Add("subject", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("subject", subject);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (isHTML != null)
				{
					if (isHTML is InheritedValue)
					{
						hashtable.Add("isHTML", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("isHTML", isHTML);
					}
				}
				if (attachments != null)
				{
					if (attachments is InheritedValue)
					{
						hashtable.Add("attachments", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("attachments", attachments);
					}
				}
				if (to != null)
				{
					if (to is InheritedValue)
					{
						hashtable.Add("to", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("to", to);
					}
				}
				if (cc != null)
				{
					if (cc is InheritedValue)
					{
						hashtable.Add("cc", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("cc", cc);
					}
				}
				if (bcc != null)
				{
					if (bcc is InheritedValue)
					{
						hashtable.Add("bcc", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("bcc", bcc);
					}
				}
				content.Add(PlatformType.Mail, hashtable);
			}
		}

		public static void customPrintShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.Print, hashtable);
			}
		}

		public static void customSMSShareContent(Hashtable content, object message)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				content.Add(PlatformType.SMS, hashtable);
			}
		}

		public static void customCopyShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.Copy, hashtable);
			}
		}

		public static void customWeChatSessionShareContent(Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData, object emoticonData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						hashtable.Add("thumbImage", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						hashtable.Add("musicFileUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						hashtable.Add("extInfo", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						hashtable.Add("fileData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("fileData", fileData);
					}
				}
				if (emoticonData != null)
				{
					if (emoticonData is InheritedValue)
					{
						hashtable.Add("emoticonData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("emoticonData", emoticonData);
					}
				}
				content.Add(PlatformType.WeChatSession, hashtable);
			}
		}

		public static void customWeChatTimelineShareContent(Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData, object emoticonData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						hashtable.Add("thumbImage", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						hashtable.Add("musicFileUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						hashtable.Add("extInfo", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						hashtable.Add("fileData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("fileData", fileData);
					}
				}
				if (emoticonData != null)
				{
					if (emoticonData is InheritedValue)
					{
						hashtable.Add("emoticonData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("emoticonData", emoticonData);
					}
				}
				content.Add(PlatformType.WeChatTimeline, hashtable);
			}
		}

		public static void customQQShareContent(Hashtable content, object type, object message, object title, object url, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.QQ, hashtable);
			}
		}

		public static void customInstapaperShareContent(Hashtable content, object url, object title, object description)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						hashtable.Add("description", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("description", description);
					}
				}
				content.Add(PlatformType.Instapaper, hashtable);
			}
		}

		public static void customPocketShareContent(Hashtable content, object url, object title, object tags, object tweetId)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (tags != null)
				{
					if (tags is InheritedValue)
					{
						hashtable.Add("tags", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("tags", tags);
					}
				}
				if (tweetId != null)
				{
					if (tweetId is InheritedValue)
					{
						hashtable.Add("tweetId", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("tweetId", tweetId);
					}
				}
				content.Add(PlatformType.Pocket, hashtable);
			}
		}

		public static void customYouDaoNoteShareContent(Hashtable content, object message, object title, object author, object source, object attachments)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (author != null)
				{
					if (author is InheritedValue)
					{
						hashtable.Add("author", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("author", author);
					}
				}
				if (source != null)
				{
					if (source is InheritedValue)
					{
						hashtable.Add("source", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("source", source);
					}
				}
				if (attachments != null)
				{
					if (attachments is InheritedValue)
					{
						hashtable.Add("attachments", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("attachments", attachments);
					}
				}
				content.Add(PlatformType.YouDaoNote, hashtable);
			}
		}

		public static void customSohuKanShareContent(Hashtable content, object url)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				content.Add(PlatformType.SohuKan, hashtable);
			}
		}

		public static void customPinterestShareContent(Hashtable content, object image, object url, object description)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						hashtable.Add("description", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("description", description);
					}
				}
				content.Add(PlatformType.Pinterest, hashtable);
			}
		}

		public static void customFlickrShareContent(Hashtable content, object photo, object title, object description, object tags, object isPublic, object isFriend, object isFamily, object safetyLevel, object contentType, object hidden)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (photo != null)
				{
					if (photo is InheritedValue)
					{
						hashtable.Add("photo", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("photo", photo);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (description != null)
				{
					if (description is InheritedValue)
					{
						hashtable.Add("description", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("description", description);
					}
				}
				if (tags != null)
				{
					if (tags is InheritedValue)
					{
						hashtable.Add("tags", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("tags", tags);
					}
				}
				if (isPublic != null)
				{
					if (isPublic is InheritedValue)
					{
						hashtable.Add("isPublic", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("isPublic", isPublic);
					}
				}
				if (isFriend != null)
				{
					if (isFriend is InheritedValue)
					{
						hashtable.Add("isFriend", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("isFriend", isFriend);
					}
				}
				if (isFamily != null)
				{
					if (isFamily is InheritedValue)
					{
						hashtable.Add("isFamily", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("isFamily", isFamily);
					}
				}
				if (safetyLevel != null)
				{
					if (safetyLevel is InheritedValue)
					{
						hashtable.Add("safetyLevel", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("safetyLevel", safetyLevel);
					}
				}
				if (contentType != null)
				{
					if (contentType is InheritedValue)
					{
						hashtable.Add("contentType", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("contentType", contentType);
					}
				}
				if (hidden != null)
				{
					if (hidden is InheritedValue)
					{
						hashtable.Add("hidden", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("hidden", hidden);
					}
				}
				content.Add(PlatformType.Flickr, hashtable);
			}
		}

		public static void customDropboxShareContent(Hashtable content, object file)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (file != null)
				{
					if (file is InheritedValue)
					{
						hashtable.Add("file", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("file", file);
					}
				}
				content.Add(PlatformType.Dropbox, hashtable);
			}
		}

		public static void customVKShareContent(Hashtable content, object message, object attachments, object url, object groupId, object friendsOnly, object location)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (attachments != null)
				{
					if (attachments is InheritedValue)
					{
						hashtable.Add("attachments", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("attachments", attachments);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (groupId != null)
				{
					if (groupId is InheritedValue)
					{
						hashtable.Add("groupId", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("groupId", groupId);
					}
				}
				if (friendsOnly != null)
				{
					if (friendsOnly is InheritedValue)
					{
						hashtable.Add("friendsOnly", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("friendsOnly", friendsOnly);
					}
				}
				if (location != null)
				{
					if (location is InheritedValue)
					{
						hashtable.Add("location", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("location", location);
					}
				}
				content.Add(PlatformType.VKontakte, hashtable);
			}
		}

		public static void customWeChatFavShareContent(Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData, object emoticonData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						hashtable.Add("thumbImage", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						hashtable.Add("musicFileUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						hashtable.Add("extInfo", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						hashtable.Add("fileData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("fileData", fileData);
					}
				}
				if (emoticonData != null)
				{
					if (emoticonData is InheritedValue)
					{
						hashtable.Add("emoticonData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("emoticonData", emoticonData);
					}
				}
				content.Add(PlatformType.WeChatFav, hashtable);
			}
		}

		public static void customYiXinSessionShareContent(Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						hashtable.Add("thumbImage", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						hashtable.Add("musicFileUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						hashtable.Add("extInfo", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						hashtable.Add("fileData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("fileData", fileData);
					}
				}
				content.Add(PlatformType.YiXinSession, hashtable);
			}
		}

		public static void customYiXinTimelineShareContent(Hashtable content, object type, object message, object title, object url, object thumbImage, object image, object musicFileUrl, object extInfo, object fileData)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (type != null)
				{
					if (type is InheritedValue)
					{
						hashtable.Add("type", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("type", type);
					}
				}
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				if (thumbImage != null)
				{
					if (thumbImage is InheritedValue)
					{
						hashtable.Add("thumbImage", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("thumbImage", thumbImage);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (musicFileUrl != null)
				{
					if (musicFileUrl is InheritedValue)
					{
						hashtable.Add("musicFileUrl", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("musicFileUrl", musicFileUrl);
					}
				}
				if (extInfo != null)
				{
					if (extInfo is InheritedValue)
					{
						hashtable.Add("extInfo", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("extInfo", extInfo);
					}
				}
				if (fileData != null)
				{
					if (fileData is InheritedValue)
					{
						hashtable.Add("fileData", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("fileData", fileData);
					}
				}
				content.Add(PlatformType.YiXinTimeline, hashtable);
			}
		}

		public static void customMingDaoShareContent(Hashtable content, object message, object image, object title, object url)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (title != null)
				{
					if (title is InheritedValue)
					{
						hashtable.Add("title", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("title", title);
					}
				}
				if (url != null)
				{
					if (url is InheritedValue)
					{
						hashtable.Add("url", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("url", url);
					}
				}
				content.Add(PlatformType.MingDao, hashtable);
			}
		}

		public static void customLineShareContent(Hashtable content, object message, object image)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				content.Add(PlatformType.Line, hashtable);
			}
		}

		public static void customWhatsAppShareContent(Hashtable content, object message, object image, object music, object video)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Hashtable hashtable = new Hashtable();
				if (message != null)
				{
					if (message is InheritedValue)
					{
						hashtable.Add("message", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("message", message);
					}
				}
				if (image != null)
				{
					if (image is InheritedValue)
					{
						hashtable.Add("image", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("image", image);
					}
				}
				if (music != null)
				{
					if (music is InheritedValue)
					{
						hashtable.Add("music", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("music", music);
					}
				}
				if (video != null)
				{
					if (video is InheritedValue)
					{
						hashtable.Add("video", ShareUtils.INHERITED_VALUE);
					}
					else
					{
						hashtable.Add("video", video);
					}
				}
				content.Add(PlatformType.WhatsApp, hashtable);
			}
		}
	}
}
