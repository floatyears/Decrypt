using Att;
using NtUniSdk.Unity3d;
using Proto;
using System;
using UnityEngine;

public sealed class GameMessageBox : MessageBox
{
	public static GameMessageBox Instance;

	private static GameMessageBox quitBox;

	public static GameMessageBox ShowMessageBox(string content, MessageBox.Type type, object userData)
	{
		if (string.IsNullOrEmpty(content))
		{
			return null;
		}
		if (GameMessageBox.Instance == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/MessageBox");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/MessageBox error"
				});
				return null;
			}
			GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return null;
			}
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z += 5000f;
			gameObject2.transform.localPosition = localPosition;
			GameMessageBox.Instance = gameObject2.AddComponent<GameMessageBox>();
		}
		if (GameMessageBox.Instance != null)
		{
			GameMessageBox.Instance.Show(content, type, userData);
		}
		return GameMessageBox.Instance;
	}

	public static bool TryClose()
	{
		if (GameMessageBox.Instance != null)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
			GameMessageBox.Instance.Close();
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		GameMessageBox.Instance = null;
	}

	public static GameMessageBox ShowApplicationQuitBox()
	{
		if (GameMessageBox.quitBox != null)
		{
			return GameMessageBox.quitBox;
		}
		GameMessageBox.quitBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("AppQuit"), MessageBox.Type.OKCancel, null);
		if (GameMessageBox.quitBox != null)
		{
			GameMessageBox.quitBox.CanCloseByFadeBGClicked = true;
			GameMessageBox expr_51 = GameMessageBox.quitBox;
			expr_51.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_51.OkClick, new MessageBox.MessageDelegate(GameMessageBox.quitBox.OnApplicationQuitChecked));
			GameMessageBox expr_7B = GameMessageBox.quitBox;
			expr_7B.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_7B.CancelClick, new MessageBox.MessageDelegate(GameMessageBox.quitBox.OnApplicationQuitCancel));
		}
		return GameMessageBox.quitBox;
	}

	private void OnApplicationQuitChecked(object obj)
	{
		GameMessageBox.quitBox = null;
		SdkU3d.exit();
	}

	private void OnApplicationQuitCancel(object obj)
	{
		GameMessageBox.quitBox = null;
	}

	public static GameMessageBox ShowRechargeMessageBox()
	{
		return GameMessageBox.ShowRechargeMessageBox(Singleton<StringManager>.Instance.GetString("PlayerR_22"), null);
	}

	public static GameMessageBox ShowRechargeMessageBox(string content, object userData = null)
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(content, MessageBox.Type.Custom2Btn, null);
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("payLb");
		GameMessageBox expr_1F = gameMessageBox;
		expr_1F.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_1F.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnGoRechargePage));
		gameMessageBox.ContentPivot = UIWidget.Pivot.Center;
		return gameMessageBox;
	}

	private void OnGoRechargePage(object obj)
	{
		if (GameUIManager.mInstance.GetSession<GUISystemSettingPopUp>() != null)
		{
			GameUIManager.mInstance.GetSession<GUISystemSettingPopUp>().Close();
		}
		GameUIVip.OpenRecharge();
	}

	public static GameMessageBox ShowPrivilegeMessageBox(string content)
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(content, MessageBox.Type.Custom2Btn, null);
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("d2mVIP");
		GameMessageBox expr_1F = gameMessageBox;
		expr_1F.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_1F.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnGoPrivilegePage));
		gameMessageBox.WidthOK = 132;
		gameMessageBox.WidthCancel = 132;
		return gameMessageBox;
	}

	private void OnGoPrivilegePage(object obj)
	{
		GUIGuildMinesRecordPopUp.TryClose();
		GameUIVip.OpenVIP(0);
	}

	public static void ShowEnergyTips(GameObject go, bool state, bool isJingli = false)
	{
		UIEnergyTooltip.Show(go, state, isJingli);
	}

	public static GameMessageBox ShowMoneyLackMessageBox(string content)
	{
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(15)))
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", 11);
			return null;
		}
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(content, MessageBox.Type.OKCancel, null);
		GameMessageBox expr_3F = gameMessageBox;
		expr_3F.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_3F.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnMoneyLackChecked));
		return gameMessageBox;
	}

	public static GameMessageBox ShowMoneyLackMessageBox()
	{
		return GameMessageBox.ShowMoneyLackMessageBox(Singleton<StringManager>.Instance.GetString("PlayerR_7"));
	}

	private void OnMoneyLackChecked(object obj)
	{
		GameUIManager.mInstance.CreateSession<GUIAlchemy>(null);
	}

	public static GameMessageBox ShowFarmSceneResetCountMessageBox(string content, int sceneID, int needDiamond)
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(content, MessageBox.Type.OKCancel, sceneID);
		GameMessageBox expr_0F = gameMessageBox;
		expr_0F.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_0F.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnFarmSceneResetCountOkClicked));
		gameMessageBox.name = needDiamond.ToString();
		return gameMessageBox;
	}

	public static GameMessageBox ShowFarmSceneResetCountMessageBox(int sceneID, int resetCount, int needDiamond)
	{
		return GameMessageBox.ShowFarmSceneResetCountMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("FarmResetCount"), needDiamond, resetCount), sceneID, needDiamond);
	}

	private void OnFarmSceneResetCountOkClicked(object obj)
	{
		int sceneID = (int)obj;
		int needDiamond = Convert.ToInt32(GameMessageBox.Instance.name);
		GameUIAdventureReady.SendResetSceneCDMsg(sceneID, needDiamond);
	}

	public static GameMessageBox ShowServerMessageBox(string prefix, int result)
	{
		string key = string.Format("{0}_{1}", prefix, result);
		string text = Singleton<StringManager>.Instance.GetString(key);
		if (string.IsNullOrEmpty(text))
		{
			text = "unknown message";
		}
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(text, MessageBox.Type.OK, null);
		gameMessageBox.CanCloseByFadeBGClicked = false;
		GameMessageBox expr_40 = gameMessageBox;
		expr_40.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_40.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnServerMsgOKClicked));
		return gameMessageBox;
	}

	public void OnServerMsgOKClicked(object obj)
	{
		Globals.Instance.GameMgr.ReturnLogin();
	}

	public static void ShowNeedVipLvlMessageBox(int vipLvl)
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("needPVPLb", new object[]
		{
			string.Format("VIP{0}", vipLvl)
		}), MessageBox.Type.Custom2Btn, vipLvl);
		GameMessageBox expr_36 = gameMessageBox;
		expr_36.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_36.OkClick, new MessageBox.MessageDelegate(GameMessageBox.OnBuyVipClick));
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("d2mVIP");
		gameMessageBox.WidthOK = 132;
		gameMessageBox.WidthCancel = 132;
	}

	private static void OnBuyVipClick(object obj)
	{
		int vipLevel = (int)obj;
		GameUIVip.OpenVIP(vipLevel);
	}

	public static GameMessageBox ShowPayMessageBox(PayInfo payInfo)
	{
		float price = payInfo.Price;
		string @string = Singleton<StringManager>.Instance.GetString("PayConfirm", new object[]
		{
			price,
			payInfo.Name
		});
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, payInfo);
		if (gameMessageBox == null)
		{
			return null;
		}
		GameMessageBox expr_47 = gameMessageBox;
		expr_47.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_47.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnPayOKClicked));
		gameMessageBox.CanCloseByFadeBGClicked = false;
		return gameMessageBox;
	}

	private void OnPayOKClicked(object obj)
	{
		MC2S_CreateOrder mC2S_CreateOrder = new MC2S_CreateOrder();
		mC2S_CreateOrder.ProductID = ((PayInfo)obj).ID;
		Globals.Instance.CliSession.Send(255, mC2S_CreateOrder);
	}
}
