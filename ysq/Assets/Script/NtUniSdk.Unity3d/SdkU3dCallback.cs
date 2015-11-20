using LitJson;
using System;
using UnityEngine;

namespace NtUniSdk.Unity3d
{
	public class SdkU3dCallback : MonoBehaviour
	{
		public delegate void SDKCallback(int code, JsonData data);

		public static SdkU3dCallback.SDKCallback FinishInitEvent;

		public static SdkU3dCallback.SDKCallback LoginDoneEvent;

		public static SdkU3dCallback.SDKCallback LogoutDoneEvent;

		public static SdkU3dCallback.SDKCallback OrderCheckEvent;

		public static SdkU3dCallback.SDKCallback DarenUpdatedEvent;

		public static SdkU3dCallback.SDKCallback ReceivedNotificationEvent;

		public void OnSdkMsgCallback(string jsonstr)
		{
			global::Debug.Log(new object[]
			{
				"OnGameSdkCallback, JsonStr = " + jsonstr
			});
			JsonData jsonData = JsonMapper.ToObject(jsonstr);
			string text = (string)jsonData["callbackType"];
			int code = (int)jsonData["code"];
			JsonData data = jsonData["data"];
			string text2 = text;
			switch (text2)
			{
			case "OnFinishInit":
				if (SdkU3dCallback.FinishInitEvent != null)
				{
					SdkU3dCallback.FinishInitEvent(code, data);
				}
				break;
			case "OnLoginDone":
				if (SdkU3dCallback.LoginDoneEvent != null)
				{
					SdkU3dCallback.LoginDoneEvent(code, data);
				}
				break;
			case "onLogoutDone":
				if (SdkU3dCallback.LogoutDoneEvent != null)
				{
					SdkU3dCallback.LogoutDoneEvent(code, data);
				}
				break;
			case "OnOrderCheck":
				if (SdkU3dCallback.OrderCheckEvent != null)
				{
					SdkU3dCallback.OrderCheckEvent(code, data);
				}
				break;
			case "OnExitView":
				SdkU3d.exit();
				break;
			case "OnIsDarenUpdated":
				if (SdkU3dCallback.DarenUpdatedEvent != null)
				{
					SdkU3dCallback.DarenUpdatedEvent(code, data);
				}
				break;
			case "OnReceivedNotification":
				if (SdkU3dCallback.ReceivedNotificationEvent != null)
				{
					SdkU3dCallback.ReceivedNotificationEvent(code, data);
				}
				break;
			}
		}
	}
}
