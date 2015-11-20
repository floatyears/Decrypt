using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NtUniSdk.Unity3d
{
	public class SdkU3d : MonoBehaviour
	{
		private const string SDK_JAVA_CLASS = "com.netease.ntunisdk.base.SdkU3d";

		private const string SDK_JAVA_CLASS_ORDERINFO = "com.netease.ntunisdk.base.OrderInfo";

		public static void init()
		{
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "init", new object[0]);
		}

		public static void ntLogin()
		{
			SdkU3d.callSdkApi("ntLogin", new object[0]);
		}

		public static void regProduct(string pId, string pName, float pPrice, int eRatio)
		{
			NtOrderInfo.regProduct(pId, pName, pPrice, eRatio);
		}

		public static void regProduct(string pId, string pName, float pPrice, int eRatio, Dictionary<string, string> sdkPids)
		{
			NtOrderInfo.regProduct(pId, pName, pPrice, eRatio, sdkPids);
		}

		public static void ntCheckOrder(string pid, string orderId, string orderEtc, string userData)
		{
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "ntCheckOrder", new object[]
			{
				pid,
				orderId,
				orderEtc
			});
		}

		public static void ntCheckOrder(NtOrderInfo order)
		{
			JsonData jsonData = order.ToJsonData();
			SdkU3d.log(jsonData.ToJson());
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "ntCheckOrder", new object[]
			{
				jsonData.ToJson()
			});
		}

		public static void setDebugMode(bool v)
		{
			SdkU3d.callSdkApi("setDebugMode", new object[]
			{
				v
			});
		}

		public static void ntLogout()
		{
			SdkU3d.callSdkApi("ntLogout", new object[0]);
		}

		public static void exit()
		{
			AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new object[]
			{
				new AndroidJavaRunnable(delegate
				{
					activity.Call("finish", new object[0]);
					SdkU3d.callSdkApi("exit", new object[0]);
					new AndroidJavaClass("java.lang.System").CallStatic("exit", new object[]
					{
						0
					});
				})
			});
		}

		public static void ntOpenManager()
		{
			SdkU3d.callSdkApi("ntOpenManager", new object[0]);
		}

		public static void ntOpenPauseView()
		{
			SdkU3d.callSdkApi("ntOpenPauseView", new object[0]);
		}

		public static void ntOpenExitView()
		{
			SdkU3d.callSdkApi("ntOpenExitView", new object[0]);
		}

		public static string getPropStr(string prop)
		{
			return SdkU3d.callSdkApiReturnString("getPropStr", new object[]
			{
				prop
			});
		}

		public static void setPropStr(string prop, string val)
		{
			SdkU3d.callSdkApi("setPropStr", new object[]
			{
				prop,
				val
			});
		}

		public static int getPropInt(string prop, int defaultVal)
		{
			return SdkU3d.callSdkApiReturnInt("getPropInt", new object[]
			{
				prop,
				defaultVal
			});
		}

		public static void setPropInt(string prop, int val)
		{
			SdkU3d.callSdkApi("setPropInt", new object[]
			{
				prop,
				val
			});
		}

		public static void resetCommonProp()
		{
			SdkU3d.callSdkApi("resetCommonProp", new object[0]);
		}

		public static bool hasLogin()
		{
			return SdkU3d.callSdkApiReturnBool("hasLogin", new object[0]);
		}

		public static string getChannel()
		{
			return SdkU3d.callSdkApiReturnString("getChannel", new object[0]);
		}

		public static string getPlatform()
		{
			return SdkU3d.callSdkApiReturnString("getPlatform", new object[0]);
		}

		public static string getAppChannel()
		{
			return SdkU3d.callSdkApiReturnString("getAppChannel", new object[0]);
		}

		public static string getUdid()
		{
			return SdkU3d.callSdkApiReturnString("getUdid", new object[0]);
		}

		public static string getSDKVersion(string channel)
		{
			return SdkU3d.callSdkApiReturnString("getSDKVersion", new object[]
			{
				channel
			});
		}

		private static string callSdkApiReturnString(string apiName, params object[] args)
		{
			SdkU3d.log("callSdkApiReturnString Unity3D " + apiName + " calling...");
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.netease.ntunisdk.base.SdkU3d"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInst", new object[0]))
				{
					string text = androidJavaObject.Call<string>(apiName, args);
					result = text;
				}
			}
			return result;
		}

		private static int callSdkApiReturnInt(string apiName, params object[] args)
		{
			SdkU3d.log("callSdkApiReturnInt Unity3D " + apiName + " calling...");
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.netease.ntunisdk.base.SdkU3d"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInst", new object[0]))
				{
					int num = androidJavaObject.Call<int>(apiName, args);
					result = num;
				}
			}
			return result;
		}

		private static bool callSdkApiReturnBool(string apiName, params object[] args)
		{
			SdkU3d.log("callSdkApiReturnBool Unity3D " + apiName + " calling...");
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.netease.ntunisdk.base.SdkU3d"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInst", new object[0]))
				{
					bool flag = androidJavaObject.Call<bool>(apiName, args);
					result = flag;
				}
			}
			return result;
		}

		private static void callSdkApi(string apiName, params object[] args)
		{
			SdkU3d.log("callSdkApi Unity3D " + apiName + " calling...");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.netease.ntunisdk.base.SdkU3d"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInst", new object[0]))
				{
					androidJavaObject.Call(apiName, args);
				}
			}
		}

		private static void callRawSdkApi(string className, string apiName, params object[] args)
		{
			SdkU3d.log("Unity3D callRawSdkApi" + className + apiName + " calling...");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(className))
			{
				androidJavaClass.CallStatic(apiName, args);
			}
		}

		private static void log(string msg)
		{
			global::Debug.Log(new object[]
			{
				msg
			});
		}

		public static void setCallbackModule(string module)
		{
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "setCallbackModule", new object[]
			{
				module
			});
		}

		public static void setSdk(string sdk)
		{
		}

		public static void ntSetFloatBtnVisible(bool v)
		{
			SdkU3d.callSdkApi("ntSetFloatBtnVisible", new object[]
			{
				v
			});
		}

		public static bool hasFeature(string feature)
		{
			return SdkU3d.callSdkApiReturnBool("hasFeature", new object[]
			{
				feature
			});
		}

		public static NtOrderInfo[] getCheckedOrders()
		{
			List<NtOrderInfo> list = new List<NtOrderInfo>();
			return list.ToArray();
		}

		public static void removeCheckedOrders(string OrderID)
		{
		}

		public static void ntUpLoadUserInfo()
		{
			SdkU3d.callSdkApi("ntUpLoadUserInfo", new object[0]);
		}

		public static void setUserInfo(string prop, string val)
		{
			SdkU3d.callSdkApi("setUserInfo", new object[]
			{
				prop,
				val
			});
		}

		public static string getUserInfo(string prop)
		{
			return SdkU3d.callSdkApiReturnString("getUserInfo", new object[]
			{
				prop
			});
		}

		public static void ntAntiAddiction(string accessToken)
		{
			SdkU3d.callSdkApi("ntAntiAddiction", new object[]
			{
				accessToken
			});
		}

		public static void ntDoSdkRealNameRegister()
		{
			SdkU3d.callSdkApi("ntDoSdkRealNameRegister", new object[0]);
		}

		public static void ntGuestBind()
		{
			SdkU3d.callSdkApi("ntGuestBind", new object[0]);
		}

		public static void ntApplyFriend(string accountId)
		{
			SdkU3d.callSdkApi("ntApplyFriend", new object[]
			{
				accountId
			});
		}

		public static void ntQueryFriendList()
		{
			SdkU3d.callSdkApi("ntQueryFriendList", new object[0]);
		}

		public static void ntQueryAvailablesInvitees()
		{
			SdkU3d.callSdkApi("ntQueryAvailablesInvitees", new object[0]);
		}

		public static void ntQueryMyAccount()
		{
			SdkU3d.callSdkApi("ntQueryMyAccount", new object[0]);
		}

		public static void ntQueryRank(QueryRankInfo queryRank)
		{
			JsonData jsonData = queryRank.ToJsonData();
			SdkU3d.log(jsonData.ToJson());
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "ntQueryRank", new object[]
			{
				jsonData.ToJson()
			});
		}

		public static void ntUpdateRank(string rankType, double score)
		{
			SdkU3d.callSdkApi("ntUpdateRank", new object[]
			{
				rankType,
				score
			});
		}

		public static void ntShare(ShareInfo shareInfo)
		{
			JsonData jsonData = shareInfo.ToJsonData();
			SdkU3d.log(jsonData.ToJson());
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "ntShare", new object[]
			{
				jsonData.ToJson()
			});
		}

		public static void ntGameLoginSuccess()
		{
			SdkU3d.callSdkApi("ntGameLoginSuccess", new object[0]);
		}

		public static void ntConsume(NtOrderInfo order)
		{
			JsonData jsonData = order.ToJsonData();
			SdkU3d.log(jsonData.ToJson());
			SdkU3d.callRawSdkApi("com.netease.ntunisdk.base.SdkU3d", "ntConsume", new object[]
			{
				jsonData.ToJson()
			});
		}

		public static string getPayChannel()
		{
			return SdkU3d.callSdkApiReturnString("getPayChannel", new object[0]);
		}

		public static string getPayChannelByPid(string pid)
		{
			return SdkU3d.callSdkApiReturnString("getPayChannelByPid", new object[]
			{
				pid
			});
		}

		public static void ntShowDaren()
		{
			SdkU3d.callSdkApi("ntShowDaren", new object[0]);
		}

		public static void ntIsDarenUpdated()
		{
			SdkU3d.callSdkApi("ntIsDarenUpdated", new object[0]);
		}

		public static int getAuthType()
		{
			return SdkU3d.callSdkApiReturnInt("getAuthType", new object[0]);
		}

		public static string getChannelByImsi()
		{
			return SdkU3d.callSdkApiReturnString("getChannelByImsi", new object[0]);
		}

		public static int getCCPerformance()
		{
			return SdkU3d.callSdkApiReturnInt("getCCPerformance", new object[0]);
		}

		public static void ntCCStartService()
		{
			SdkU3d.callSdkApi("ntCCStartService", new object[0]);
		}

		public static void ntCCStopService()
		{
			SdkU3d.callSdkApi("ntCCStopService", new object[0]);
		}

		public static int getCCWindowState()
		{
			return SdkU3d.callSdkApiReturnInt("getCCWindowState", new object[0]);
		}

		public static int DRPF(string logInfo)
		{
			return SdkU3d.callSdkApiReturnInt("DRPF", new object[]
			{
				logInfo
			});
		}

		public static void setLoginFlag(bool value)
		{
		}

		public static void setLoginStat(int value)
		{
			SdkU3d.callSdkApi("setLoginStat", new object[]
			{
				value
			});
		}

		public static void ntGetAnnouncementInfo()
		{
			SdkU3d.callSdkApi("ntGetAnnouncementInfo", new object[0]);
		}
	}
}
