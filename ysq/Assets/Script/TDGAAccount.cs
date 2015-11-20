using System;
using UnityEngine;

public class TDGAAccount
{
	private static string JAVA_CLASS = "com.tendcloud.tenddata.TDGAAccount";

	private static AndroidJavaClass agent = new AndroidJavaClass(TDGAAccount.JAVA_CLASS);

	private AndroidJavaObject mAccount;

	public void setAccountObject(AndroidJavaObject account)
	{
		this.mAccount = account;
	}

	public static TDGAAccount SetAccount(string accountId)
	{
		TDGAAccount tDGAAccount = new TDGAAccount();
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			AndroidJavaObject accountObject = TDGAAccount.agent.CallStatic<AndroidJavaObject>("setAccount", new object[]
			{
				accountId
			});
			tDGAAccount.setAccountObject(accountObject);
		}
		return tDGAAccount;
	}

	public void SetAccountName(string accountName)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && this.mAccount != null)
		{
			this.mAccount.Call("setAccountName", new object[]
			{
				accountName
			});
		}
	}

	public void SetAccountType(AccountType type)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && this.mAccount != null)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount$AccountType");
			AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("valueOf", new object[]
			{
				type.ToString()
			});
			this.mAccount.Call("setAccountType", new object[]
			{
				androidJavaObject
			});
		}
	}

	public void SetLevel(int level)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && this.mAccount != null)
		{
			this.mAccount.Call("setLevel", new object[]
			{
				level
			});
		}
	}

	public void SetAge(int age)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && this.mAccount != null)
		{
			this.mAccount.Call("setAge", new object[]
			{
				age
			});
		}
	}

	public void SetGender(Gender type)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && this.mAccount != null)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount$Gender");
			AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("valueOf", new object[]
			{
				type.ToString()
			});
			this.mAccount.Call("setGender", new object[]
			{
				androidJavaObject
			});
		}
	}

	public void SetGameServer(string gameServer)
	{
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && this.mAccount != null)
		{
			this.mAccount.Call("setGameServer", new object[]
			{
				gameServer
			});
		}
	}
}
