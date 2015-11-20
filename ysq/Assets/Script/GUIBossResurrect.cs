using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIBossResurrect : MonoBehaviour
{
	private float timerRefresh;

	private UILabel tiemrLb;

	private bool autoResurrect;

	private bool update;

	private void Awake()
	{
		this.tiemrLb = GameUITools.FindUILabel("counter", base.gameObject);
		GameUITools.RegisterClickEvent("desertBtn", new UIEventListener.VoidDelegate(this.OnDesertClick), base.gameObject);
		GameUITools.RegisterClickEvent("resurrect1Btn", new UIEventListener.VoidDelegate(this.OnResurrect1Click), base.gameObject);
		GameUITools.RegisterClickEvent("resurrect2Btn", new UIEventListener.VoidDelegate(this.OnResurrect2Click), base.gameObject);
		this.update = false;
		MC2S_WorldBossResurrect mC2S_WorldBossResurrect = new MC2S_WorldBossResurrect();
		mC2S_WorldBossResurrect.Type = 0;
		Globals.Instance.CliSession.Send(622, mC2S_WorldBossResurrect);
		Globals.Instance.CliSession.Register(623, new ClientSession.MsgHandler(this.OnMsgWorldBossResurrect));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(623, new ClientSession.MsgHandler(this.OnMsgWorldBossResurrect));
	}

	private void Update()
	{
		if (!this.update)
		{
			return;
		}
		if (Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.Refresh();
			if (!this.autoResurrect && Globals.Instance.Player.GetTimeStamp() > Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp)
			{
				this.autoResurrect = true;
				this.Resurrect();
			}
		}
	}

	private void Refresh()
	{
		int num = Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp - Globals.Instance.Player.GetTimeStamp();
		if (num < 0)
		{
			num = 0;
		}
		this.tiemrLb.text = Singleton<StringManager>.Instance.GetString("wsResurrectTimer", new object[]
		{
			num
		});
	}

	private void OnDesertClick(GameObject go)
	{
		UnityEngine.Object.Destroy(base.gameObject);
		GameAnalytics.OnFailScene(Globals.Instance.SenceMgr.sceneInfo, GameAnalytics.ESceneFailed.CombatEffectiveness);
		Globals.Instance.SenceMgr.CloseScene();
		GameUIManager.mInstance.ChangeSession<GUIWorldBossVictoryScene>(null, false, false);
	}

	private void OnResurrect1Click(GameObject go)
	{
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(47), 0))
		{
			return;
		}
		MC2S_WorldBossResurrect mC2S_WorldBossResurrect = new MC2S_WorldBossResurrect();
		mC2S_WorldBossResurrect.Type = 1;
		Globals.Instance.CliSession.Send(622, mC2S_WorldBossResurrect);
	}

	private void OnResurrect2Click(GameObject go)
	{
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(48), 0))
		{
			return;
		}
		MC2S_WorldBossResurrect mC2S_WorldBossResurrect = new MC2S_WorldBossResurrect();
		mC2S_WorldBossResurrect.Type = 2;
		Globals.Instance.CliSession.Send(622, mC2S_WorldBossResurrect);
	}

	private void Resurrect()
	{
		Globals.Instance.ActorMgr.Resurrect(true);
		UnityEngine.Object.Destroy(base.gameObject);
		GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
		if (session != null)
		{
			NGUITools.SetActive(session.gameObject, true);
			session.mHeroSummonLayer.OnResurrect();
		}
	}

	public void OnMsgWorldBossResurrect(MemoryStream stream)
	{
		MS2C_WorldBossResurrect mS2C_WorldBossResurrect = Serializer.NonGeneric.Deserialize(typeof(MS2C_WorldBossResurrect), stream) as MS2C_WorldBossResurrect;
		if (mS2C_WorldBossResurrect.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_WorldBossResurrect.Result);
			return;
		}
		if (mS2C_WorldBossResurrect.Type != 0)
		{
			GameAnalytics.OnWorldBossResurrect(mS2C_WorldBossResurrect.Type);
			Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp = 0;
			this.Resurrect();
		}
		else
		{
			Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp = Globals.Instance.Player.GetTimeStamp() + 60;
			this.update = true;
		}
	}
}
