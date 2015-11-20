using System;
using UnityEngine;

public class TDGAVirtualCurrency
{
	private static string JAVA_CLASS = "com.tendcloud.tenddata.TDGAVirtualCurrency";

	private static AndroidJavaClass agent = new AndroidJavaClass(TDGAVirtualCurrency.JAVA_CLASS);

	public static void OnChargeRequest(string orderId, string iapId, double currencyAmount, string currencyType, double virtualCurrencyAmount, string paymentType)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAVirtualCurrency.agent.CallStatic("onChargeRequest", new object[]
			{
				orderId,
				iapId,
				currencyAmount,
				currencyType,
				virtualCurrencyAmount,
				paymentType
			});
		}
	}

	public static void OnChargeSuccess(string orderId)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAVirtualCurrency.agent.CallStatic("onChargeSuccess", new object[]
			{
				orderId
			});
		}
	}

	public static void OnReward(double virtualCurrencyAmount, string reason)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAVirtualCurrency.agent.CallStatic("onReward", new object[]
			{
				virtualCurrencyAmount,
				reason
			});
		}
	}
}
