using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIGuildSetPopUp : GameUIBasePopup
{
	private UILabel mGuildName;

	private UILabel mGuildType;

	private UIGuildSliderNumber mGuildLimitLvl;

	private UIGuildSliderNumber mGuildPowerLvl;

	private GameObject mChangeNameBtn;

	private bool mNeedVerify;

	private bool NeedVerify
	{
		get
		{
			return this.mNeedVerify;
		}
		set
		{
			if (this.mNeedVerify != value)
			{
				this.mNeedVerify = value;
				this.RefreshGuildType();
			}
		}
	}

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mGuildName = transform.Find("txt0/name").GetComponent<UILabel>();
		this.mChangeNameBtn = transform.Find("changeNameBtn").gameObject;
		UIEventListener expr_80 = UIEventListener.Get(this.mChangeNameBtn);
		expr_80.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_80.onClick, new UIEventListener.VoidDelegate(this.OnChangeNameBtnClick));
		Transform transform2 = transform.Find("guildType");
		this.mGuildType = transform2.Find("Background/typeTxt").GetComponent<UILabel>();
		GameObject gameObject2 = transform2.Find("BtnMinus").gameObject;
		UIEventListener expr_DA = UIEventListener.Get(gameObject2);
		expr_DA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DA.onClick, new UIEventListener.VoidDelegate(this.OnGuildTypeClick));
		GameObject gameObject3 = transform2.Find("BtnPlus").gameObject;
		UIEventListener expr_114 = UIEventListener.Get(gameObject3);
		expr_114.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_114.onClick, new UIEventListener.VoidDelegate(this.OnGuildTypeClick));
		this.mNeedVerify = Globals.Instance.Player.GuildSystem.Guild.Verify;
		Transform transform3 = transform.Find("changeApplyLvl");
		this.mGuildLimitLvl = transform3.gameObject.AddComponent<UIGuildSliderNumber>();
		this.mGuildLimitLvl.InitWithBaseScene();
		this.mGuildLimitLvl.Minimum = 0;
		this.mGuildLimitLvl.Maximum = 80;
		this.mGuildLimitLvl.CharLimit = 3;
		this.mGuildLimitLvl.InvalidateNum = 38;
		transform.Find("txt3").GetComponent<UILabel>().text = Singleton<StringManager>.Instance.GetString("guild40");
		Transform transform4 = transform.Find("needPower");
		this.mGuildPowerLvl = transform4.gameObject.AddComponent<UIGuildSliderNumber>();
		this.mGuildPowerLvl.InitWithBaseScene();
		this.mGuildPowerLvl.Minimum = 0;
		this.mGuildPowerLvl.Maximum = 99999999;
		this.mGuildPowerLvl.CharLimit = 8;
		this.mGuildPowerLvl.InvalidateNum = 1;
		GameObject gameObject4 = transform.Find("sureBtn").gameObject;
		UIEventListener expr_24B = UIEventListener.Get(gameObject4);
		expr_24B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_24B.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		Globals.Instance.CliSession.Register(955, new ClientSession.MsgHandler(this.OnMsgSetApplyCondition));
	}

	private void OnDestroy()
	{
		if (Globals.Instance != null)
		{
			Globals.Instance.CliSession.Unregister(955, new ClientSession.MsgHandler(this.OnMsgSetApplyCondition));
		}
	}

	private void RefreshGuildType()
	{
		this.mGuildType.text = ((!this.mNeedVerify) ? Singleton<StringManager>.Instance.GetString("guild9") : Singleton<StringManager>.Instance.GetString("guild10"));
	}

	private void Refresh()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			int selfGuildJob = Tools.GetSelfGuildJob();
			this.mChangeNameBtn.SetActive(selfGuildJob == 1);
			this.mGuildName.text = Globals.Instance.Player.GuildSystem.Guild.Name;
			this.RefreshGuildType();
			this.mGuildLimitLvl.Number = Globals.Instance.Player.GuildSystem.Guild.ApplyLevel;
			this.mGuildPowerLvl.Number = Globals.Instance.Player.GuildSystem.Guild.CombatValue;
		}
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnChangeNameBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int selfGuildJob = Tools.GetSelfGuildJob();
		if (selfGuildJob != 1)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", 2);
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildChangeNamePopUp, false, null, null);
	}

	private void OnGuildTypeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.NeedVerify = !this.NeedVerify;
	}

	private void OnSureBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GameConst.GetInt32(4) <= this.mGuildLimitLvl.Number && this.mGuildLimitLvl.Number <= this.mGuildLimitLvl.Maximum)
		{
			MC2S_SetApplyCondition mC2S_SetApplyCondition = new MC2S_SetApplyCondition();
			mC2S_SetApplyCondition.Level = this.mGuildLimitLvl.Number;
			mC2S_SetApplyCondition.NeedVerify = this.NeedVerify;
			mC2S_SetApplyCondition.CombatValue = this.mGuildPowerLvl.Number;
			Globals.Instance.CliSession.Send(954, mC2S_SetApplyCondition);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild34", new object[]
			{
				GameConst.GetInt32(4),
				this.mGuildLimitLvl.Maximum
			}), 0f, 0f);
		}
	}

	private void OnMsgSetApplyCondition(MemoryStream stream)
	{
		MS2C_SetApplyCondition mS2C_SetApplyCondition = Serializer.NonGeneric.Deserialize(typeof(MS2C_SetApplyCondition), stream) as MS2C_SetApplyCondition;
		if (mS2C_SetApplyCondition.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_SetApplyCondition.Result);
			return;
		}
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
