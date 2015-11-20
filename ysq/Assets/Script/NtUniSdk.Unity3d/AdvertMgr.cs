using System;
using System.Collections;
using UnityEngine;
using XUPorterJSON;

namespace NtUniSdk.Unity3d
{
	public class AdvertMgr
	{
		public const string CONVERSION_ID = "conversionId";

		public const string LABEL = "label";

		public const string VALUE = "value";

		public const string IS_REPEATABLE = "isRepeatable";

		private const string SDK_JAVA_CLASS_ADVERT = "com.netease.advertSdk.base.AdvertMgr";

		public static string createRoleAdmob(string conversionId, string label, string value, string isRepeatable)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["conversionId"] = conversionId;
			hashtable["label"] = label;
			hashtable["value"] = value;
			hashtable["isRepeatable"] = isRepeatable;
			return MiniJSON.jsonEncode(hashtable);
		}

		public static void trackEvent(string eventStr, Hashtable chnlDetail)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]))
			{
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				foreach (DictionaryEntry dictionaryEntry in chnlDetail)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
					{
						dictionaryEntry.Key
					}))
					{
						using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
						{
							dictionaryEntry.Value
						}))
						{
							array[0] = androidJavaObject2;
							array[1] = androidJavaObject3;
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
				AdvertMgr.callSdkApi("trackEvent", new object[]
				{
					eventStr,
					androidJavaObject
				});
			}
		}

		private static void callSdkApi(string apiName, params object[] args)
		{
			AdvertMgr.log("callSdkApi Unity3D " + apiName + " calling...");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.netease.advertSdk.base.AdvertMgr"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInst", new object[0]))
				{
					androidJavaObject.Call(apiName, args);
				}
			}
		}

		private static void log(string msg)
		{
			global::Debug.Log(new object[]
			{
				msg
			});
		}
	}
}
