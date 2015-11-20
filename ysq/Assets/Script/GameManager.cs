using Att;
using NtUniSdk.Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public enum EGameStatus
	{
		EGS_None,
		EGS_StartMovie,
		EGS_Login,
		EGS_Updating,
		EGS_Loading,
		EGS_CreateChar,
		EGS_Gaming
	}

	public GameManager.EGameStatus Status;

	private bool pause;

	private float speedDown;

	private float speedUp;

	private void Start()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Combine(expr_0A.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInit));
		GameUIManager.mInstance.CheckVerion(new DynamicUpdate.VoidCallback(this.OnCheckVersionEvent));
	}

	public void OnCheckVersionEvent()
	{
		base.StartCoroutine(this.InitGame(true));
	}

	[DebuggerHidden]
	private IEnumerator InitGame(bool flag)
	{
        return null;
        //GameManager.<InitGame>c__Iterator13 <InitGame>c__Iterator = new GameManager.<InitGame>c__Iterator13();
        //<InitGame>c__Iterator.flag = flag;
        //<InitGame>c__Iterator.<$>flag = flag;
        //<InitGame>c__Iterator.<>f__this = this;
        //return <InitGame>c__Iterator;
	}

	public void ReturnLogin()
	{
		Globals.Instance.CliSession.ReturnLogin();
		if (this.Status == GameManager.EGameStatus.EGS_Login)
		{
			return;
		}
		this.Status = GameManager.EGameStatus.EGS_Login;
		Globals.Instance.Player.ReturnLogin();
		Globals.Instance.SenceMgr.CloseScene();
		GameUIManager.mInstance.DestroyAll();
		GameUIManager.mInstance.CloseAllSession();
		GameUIManager.mInstance.ChangeSession<GUIGameLoginScene>(null, false, true);
	}

	public void ReCheckVersion(Action callBack)
	{
		base.StartCoroutine(this.DoReCheckVerion(callBack));
	}

	[DebuggerHidden]
	private IEnumerator DoReCheckVerion(Action callBack)
	{
        return null;
        //GameManager.<DoReCheckVerion>c__Iterator14 <DoReCheckVerion>c__Iterator = new GameManager.<DoReCheckVerion>c__Iterator14();
        //<DoReCheckVerion>c__Iterator.callBack = callBack;
        //<DoReCheckVerion>c__Iterator.<$>callBack = callBack;
        //<DoReCheckVerion>c__Iterator.<>f__this = this;
        //return <DoReCheckVerion>c__Iterator;
	}

	public void OnReCheckVersionEvent()
	{
		base.StartCoroutine(this.InitGame(false));
	}

	public void OnDataInit(bool versionInit, bool newPlayer)
	{
		global::Debug.Log(new object[]
		{
			"Load player data success!"
		});
		this.Status = GameManager.EGameStatus.EGS_Gaming;
		if (versionInit || newPlayer)
		{
			string channel = SdkU3d.getChannel();
			string text = channel;
			switch (text)
			{
			case "netease":
			case "uc_platform":
			case "huawei":
				this.UpLoadUserInfo(string.Empty);
				break;
			case "oppo":
				if (newPlayer)
				{
					this.UpLoadUserInfo(string.Empty);
				}
				break;
			case "iaround":
			case "ljsdk":
				this.UpLoadUserInfo((!newPlayer) ? "1" : "2");
				break;
			case "kuaifa":
				this.UpLoadUserInfo((!newPlayer) ? "2" : "5");
				break;
			case "meizu_sdk":
			case "37yyb":
				this.SetUserInfo(string.Empty);
				SdkU3d.setUserInfo("USERINFO_HOSTID", "S" + GameSetting.Data.ServerID.ToString());
				SdkU3d.ntUpLoadUserInfo();
				break;
			case "pps":
				this.SetUserInfo(string.Empty);
				SdkU3d.setUserInfo("USERINFO_HOSTID", "ppsmobile_s" + GameSetting.Data.ServerID.ToString());
				SdkU3d.ntUpLoadUserInfo();
				break;
			case "caohua":
				this.SetUserInfo(string.Empty);
				SdkU3d.ntGameLoginSuccess();
				break;
			case "3k_sdk":
				this.SetUserInfo(string.Empty);
				SdkU3d.setPropInt("SERVER_ID", GameSetting.Data.ServerID);
				SdkU3d.ntUpLoadUserInfo();
				break;
			}
			if (newPlayer)
			{
				GameUIManager.mInstance.LoadScene(GameConst.GetInt32(110));
			}
			else
			{
				GameUIManager.mInstance.uiState.MaskTutorial = true;
				GameUIManager.mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
				GameUIManager.mInstance.ClearGobackSession();
			}
		}
	}

	public void Play()
	{
		Globals.Instance.ActorMgr.OnPauseStop();
		this.pause = false;
		this.UpdateSpeed();
	}

	public bool IsPause()
	{
		return this.pause;
	}

	public void Pause()
	{
		Globals.Instance.ActorMgr.OnPauseStart();
		this.pause = true;
		this.UpdateSpeed();
	}

	public void SpeedDown(float value, bool apply)
	{
		this.speedDown += ((!apply) ? (-value) : value);
		this.UpdateSpeed();
	}

	public void SpeedUp(float value, bool apply)
	{
		this.speedUp += ((!apply) ? (-value) : value);
		this.UpdateSpeed();
	}

	public void ClearSpeedMod()
	{
		this.pause = false;
		this.speedDown = 0f;
		this.speedUp = 0f;
		this.UpdateSpeed();
	}

	private void UpdateSpeed()
	{
		if (this.pause)
		{
			Time.timeScale = 0f;
			return;
		}
		float num = 1f;
		if (this.speedDown < 0f)
		{
			num += this.speedDown;
		}
		if (this.speedUp > 0f)
		{
			num += this.speedUp;
		}
		Time.timeScale = num;
	}

	private void RegisterPayInfo()
	{
		string channel = SdkU3d.getChannel();
		switch (channel)
		{
		case "oppo":
			foreach (PayInfo current in Globals.Instance.AttDB.PayDict.Values)
			{
				SdkU3d.regProduct(current.ProductID, Singleton<StringManager>.Instance.GetString("oppoProductName", new object[]
				{
					current.Name
				}), current.Price, (current.Type != 0) ? 1 : ((int)((float)current.Diamond / current.Price)));
			}
			return;
		case "coolpad_sdk":
			foreach (PayInfo current2 in Globals.Instance.AttDB.PayDict.Values)
			{
				SdkU3d.regProduct(current2.CoolpadPID.ToString(), current2.Name, current2.Price, (current2.Type != 0) ? 1 : ((int)((float)current2.Diamond / current2.Price)));
			}
			return;
		case "lenovo_open":
			foreach (PayInfo current3 in Globals.Instance.AttDB.PayDict.Values)
			{
				SdkU3d.regProduct(current3.LenovoPID.ToString(), current3.Name, current3.Price, (current3.Type != 0) ? 1 : ((int)((float)current3.Diamond / current3.Price)));
			}
			return;
		case "kuaifa":
		case "appchina":
			foreach (PayInfo current4 in Globals.Instance.AttDB.PayDict.Values)
			{
				SdkU3d.regProduct(current4.KuaiFaPID.ToString(), current4.Name, current4.Price, (current4.Type != 0) ? 1 : ((int)((float)current4.Diamond / current4.Price)));
			}
			return;
		case "yixin":
			foreach (PayInfo current5 in Globals.Instance.AttDB.PayDict.Values)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("yixin", current5.KuaiFaPID.ToString());
				SdkU3d.regProduct(current5.KuaiFaPID.ToString(), current5.Name, current5.Price, (current5.Type != 0) ? 1 : ((int)((float)current5.Diamond / current5.Price)), dictionary);
			}
			return;
		}
		foreach (PayInfo current6 in Globals.Instance.AttDB.PayDict.Values)
		{
			SdkU3d.regProduct(current6.ProductID, current6.Name, current6.Price, (current6.Type != 0) ? 1 : ((int)((float)current6.Diamond / current6.Price)));
		}
	}

	public void SetUserInfo(string dataType)
	{
		LocalPlayer player = Globals.Instance.Player;
		SdkU3d.setUserInfo("USERINFO_UID", player.Data.ID.ToString());
		SdkU3d.setUserInfo("USERINFO_NAME", player.Data.Name);
		SdkU3d.setUserInfo("USERINFO_GRADE", player.Data.Level.ToString());
		SdkU3d.setUserInfo("USERINFO_HOSTID", GameSetting.Data.ServerID.ToString());
		SdkU3d.setUserInfo("USERINFO_HOSTNAME", GameSetting.Data.ServerName);
		SdkU3d.setUserInfo("USERINFO_BALANCE", player.Data.Diamond.ToString());
		SdkU3d.setUserInfo("USERINFO_VIP", player.Data.VipLevel.ToString());
		SdkU3d.setUserInfo("USERINFO_ORG", string.Empty);
		if (SdkU3d.getChannel() == "netease")
		{
			SdkU3d.setUserInfo("USERINFO_ROLE_TYPE_ID", string.Empty);
			SdkU3d.setUserInfo("USERINFO_ROLE_TYPE_NAME", string.Empty);
			SdkU3d.setUserInfo("USERINFO_MENPAI_ID", string.Empty);
			SdkU3d.setUserInfo("USERINFO_MENPAI_NAME", string.Empty);
			SdkU3d.setUserInfo("USERINFO_GANG_ID", string.Empty);
			SdkU3d.setUserInfo("USERINFO_REGION_ID", string.Empty);
			SdkU3d.setUserInfo("USERINFO_REGION_NAME", string.Empty);
			SdkU3d.setUserInfo("USERINFO_CAPABILITY", player.TeamSystem.GetCombatValue().ToString());
		}
		if (!string.IsNullOrEmpty(dataType))
		{
			SdkU3d.setUserInfo("USERINFO_DATATYPE", dataType);
		}
	}

	public void UpLoadUserInfo(string dataType)
	{
		this.SetUserInfo(dataType);
		SdkU3d.ntUpLoadUserInfo();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && this.Status == GameManager.EGameStatus.EGS_Gaming && SdkU3d.getChannel() == "netease")
		{
			this.UpLoadUserInfo(string.Empty);
		}
	}
}
