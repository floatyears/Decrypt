using System;
using UnityEngine;

public class TDGAItem
{
	private static string JAVA_CLASS = "com.tendcloud.tenddata.TDGAItem";

	private static AndroidJavaClass agent = new AndroidJavaClass(TDGAItem.JAVA_CLASS);

	public static void OnPurchase(string item, int itemNumber, double priceInVirtualCurrency)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAItem.agent.CallStatic("onPurchase", new object[]
			{
				item,
				itemNumber,
				priceInVirtualCurrency
			});
		}
	}

	public static void OnUse(string item, int itemNumber)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TDGAItem.agent.CallStatic("onUse", new object[]
			{
				item,
				itemNumber
			});
		}
	}
}
