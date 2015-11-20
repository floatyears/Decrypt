using Holoville.HOTween;
using NtUniSdk.Unity3d;
using Proto;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SystemSettingPlayerInfoLayer : MonoBehaviour
{
	private GUISystemSettingPopUp mBaseScene;

	private StringBuilder content = new StringBuilder();

	private float timerRefresh;

	private int costTime;

	private int count;

	private int CountTime;

	private UILabel mName;

	private UILabel mKeysValue;

	private UILabel mJingliValue;

	private UILabel mZhanDouli;

	private int mOldStaminaNum;

	private int mOldJingliNum;

	private UILabel mTimer;

	private UILabel mTimerJingli;

	private GameObject mJieRiMoneyGo;

	private UILabel mJieRiMoneyNum;

	[NonSerialized]
	public GameObject mQuitNew;

	public void InitWithBaseScene(GUISystemSettingPopUp basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
		this.OnPlayerUpdateEvent();
	}

	private void CreateObjects()
	{
		ObscuredStats data = Globals.Instance.Player.Data;
		GameObject gameObject = GameUITools.FindGameObject("CharInfo", base.gameObject);
		this.mName = GameUITools.FindUILabel("Name", gameObject);
		GameObject gameObject2 = GameUITools.FindGameObject("CharIcon", gameObject);
		gameObject2.GetComponent<UISprite>().spriteName = Globals.Instance.Player.GetPlayerIcon();
		GameUITools.FindUISprite("Frame", gameObject2).spriteName = Tools.GetItemQualityIcon(Globals.Instance.Player.GetQuality());
		this.mZhanDouli = gameObject.transform.Find("zhanDouLi/num").GetComponent<UILabel>();
		this.mZhanDouli.text = Globals.Instance.Player.TeamSystem.GetCombatValue().ToString();
		gameObject2 = GameUITools.FindGameObject("CharInfo/VIP", base.gameObject);
		UISprite uISprite = GameUITools.FindUISprite("Single", gameObject2);
		UISprite uISprite2 = GameUITools.FindUISprite("Tens", gameObject2);
		if (data.VipLevel > 0u)
		{
			gameObject2.SetActive(true);
			if (data.VipLevel >= 10u)
			{
				uISprite.enabled = true;
				uISprite.spriteName = (data.VipLevel % 10u).ToString();
				uISprite2.spriteName = (data.VipLevel / 10u).ToString();
			}
			else
			{
				uISprite.enabled = false;
				uISprite2.spriteName = data.VipLevel.ToString();
			}
		}
		else
		{
			gameObject2.SetActive(false);
		}
		gameObject2 = GameUITools.FindGameObject("Level", gameObject);
		gameObject2.GetComponent<UILabel>().text = string.Format("Lv{0}", data.Level);
		gameObject2 = GameUITools.FindGameObject("Exp", gameObject2);
		uint pExp = Globals.Instance.AttDB.LevelDict.GetInfo((int)data.Level).PExp;
		gameObject2.GetComponent<UISlider>().value = ((pExp != 0u) ? (data.Exp / pExp) : 1f);
		GameUITools.FindUILabel("ExpText", gameObject2).text = string.Format("{0}/{1}", data.Exp, pExp);
		GameObject gameObject3 = base.transform.Find("RenameBtn").gameObject;
		UIEventListener expr_23B = UIEventListener.Get(gameObject3);
		expr_23B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_23B.onClick, new UIEventListener.VoidDelegate(this.OnRenameBtnClick));
		gameObject2 = GameUITools.RegisterClickEvent("QuitBtn", new UIEventListener.VoidDelegate(this.QuitBtnClick), base.gameObject);
		this.mQuitNew = GameUITools.FindGameObject("new", gameObject2);
		if (SdkU3d.getChannel() == "netease")
		{
			gameObject2.transform.Find("Label").GetComponent<UILabel>().text = Singleton<StringManager>.Instance.GetString("systemSettingPlayerInfoQuit");
		}
		else
		{
			gameObject2.transform.Find("Label").GetComponent<UILabel>().text = Singleton<StringManager>.Instance.GetString("systemSettingPlayerInfoQuit2");
		}
		gameObject2 = GameUITools.FindGameObject("Keys", base.gameObject);
		this.mKeysValue = GameUITools.FindUILabel("KeysValue", gameObject2);
		this.mTimer = GameUITools.FindUILabel("Timer", gameObject2);
		gameObject2 = GameUITools.FindGameObject("jingLi", base.gameObject);
		this.mJingliValue = GameUITools.FindUILabel("KeysValue", gameObject2);
		this.mTimerJingli = GameUITools.FindUILabel("Timer", gameObject2);
		this.mJieRiMoneyGo = GameUITools.FindGameObject("jieRiMoney", base.gameObject);
		this.mJieRiMoneyNum = this.mJieRiMoneyGo.transform.Find("KeysValue").GetComponent<UILabel>();
		if (Globals.Instance.Player.Data.FestivalVoucher > 0)
		{
			this.mJieRiMoneyNum.text = Globals.Instance.Player.Data.FestivalVoucher.ToString();
			this.mJieRiMoneyGo.SetActive(true);
		}
		else
		{
			this.mJieRiMoneyGo.SetActive(false);
		}
		GameObject gameObject4 = base.transform.Find("exchangeBtn").gameObject;
		UILabel component = gameObject4.transform.Find("Label").GetComponent<UILabel>();
		component.overflowMethod = UILabel.Overflow.ShrinkContent;
		component.pivot = UIWidget.Pivot.Left;
		component.width = 120;
		component.transform.localPosition = new Vector3(-40f, -1f, 0f);
		GameObject gameObject5 = base.transform.Find("bangDingBtn").gameObject;
		GameObject gameObject6 = base.transform.Find("elfBtn").gameObject;
		GameObject gameObject7 = base.transform.Find("serviceBtn").gameObject;
		UIEventListener expr_4A4 = UIEventListener.Get(gameObject4);
		expr_4A4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4A4.onClick, new UIEventListener.VoidDelegate(this.OnExchangeBtnClick));
		if (Globals.Instance.Player.IsFunctionEnable(2))
		{
			gameObject4.SetActive(true);
		}
		else
		{
			gameObject4.SetActive(false);
		}
		UIEventListener expr_4F6 = UIEventListener.Get(gameObject5);
		expr_4F6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4F6.onClick, new UIEventListener.VoidDelegate(this.OnBindBtnClick));
		if (Globals.Instance.Player.IsFunctionEnable(16))
		{
			gameObject5.SetActive(true);
		}
		else
		{
			gameObject5.SetActive(false);
		}
		UIEventListener expr_549 = UIEventListener.Get(gameObject6);
		expr_549.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_549.onClick, new UIEventListener.VoidDelegate(this.ElfBtnClick));
		if (Globals.Instance.Player.IsFunctionEnable(4))
		{
			gameObject6.SetActive(true);
		}
		else
		{
			gameObject6.SetActive(false);
		}
		UIEventListener expr_59B = UIEventListener.Get(gameObject7);
		expr_59B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_59B.onClick, new UIEventListener.VoidDelegate(this.ServiceBtnClick));
		if (Globals.Instance.Player.IsFunctionEnable(8))
		{
			gameObject7.SetActive(true);
		}
		else
		{
			gameObject7.SetActive(false);
		}
		this.Refresh();
		Globals.Instance.CliSession.Register(1508, new ClientSession.MsgHandler(this.OnMsgElfInit));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(1508, new ClientSession.MsgHandler(this.OnMsgElfInit));
	}

	private void OnRenameBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUItakeName, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
	}

	private void OnExchangeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIExchangeAcNumPopUp, false, null, null);
	}

	private void OnBindBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((Globals.Instance.Player.Data.DataFlag & 8) != 0)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt9"), 0f, 0f);
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIPhoneBindPopUp, false, null, null);
	}

	private void QuitBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (SdkU3d.getChannel() == "netease")
		{
			SdkU3d.ntOpenManager();
		}
		else
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("systemSettingPlayerInfoQuitDialog"), MessageBox.Type.OKCancel, null);
			gameMessageBox.CanCloseByFadeBGClicked = false;
			GameMessageBox expr_52 = gameMessageBox;
			expr_52.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_52.OkClick, new MessageBox.MessageDelegate(this.ReturnLogin));
			GameMessageBox expr_74 = gameMessageBox;
			expr_74.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_74.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
		}
		if (GameUIManager.mInstance.uiState.SDKPlayerManagerNew)
		{
			GameUIManager.mInstance.uiState.SDKPlayerManagerNew = false;
			this.mBaseScene.RefreshPlayerNew();
			GUIMainMenuScene session = GameUIManager.mInstance.GetSession<GUIMainMenuScene>();
			if (session != null)
			{
				session.RefreshOptionBtn();
			}
		}
	}

	private void ServiceBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_ApplyCSAServerToken ojb = new MC2S_ApplyCSAServerToken();
		Globals.Instance.CliSession.Send(727, ojb);
	}

	private void OnCancelClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
	}

	private void ReturnLogin(object obj)
	{
		SdkU3d.setPropStr("SESSION", string.Empty);
		if (SdkU3d.getChannel() != "caohua")
		{
			SdkU3d.ntLogout();
		}
		if (SdkU3d.getChannel() == "scgames")
		{
			SdkU3d.setLoginStat(0);
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (SdkU3d.getChannel() != "kuaifa")
		{
			Globals.Instance.GameMgr.ReturnLogin();
		}
	}

	public void OnPlayerUpdateEvent()
	{
		int maxEnergy = Globals.Instance.Player.GetMaxEnergy();
		if (this.mOldStaminaNum != Globals.Instance.Player.Data.Energy || Globals.Instance.Player.Data.Energy == 0)
		{
			this.mKeysValue.text = string.Format("{0}/{1}", Globals.Instance.Player.Data.Energy, maxEnergy);
			if (this.mOldStaminaNum != 0)
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(this.mKeysValue.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence.Append(HOTween.To(this.mKeysValue.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.mOldStaminaNum = Globals.Instance.Player.Data.Energy;
		}
		int maxStamina = Globals.Instance.Player.GetMaxStamina();
		if (this.mOldJingliNum != Globals.Instance.Player.Data.Stamina || Globals.Instance.Player.Data.Stamina == 0)
		{
			this.mJingliValue.text = string.Format("{0}/{1}", Globals.Instance.Player.Data.Stamina, maxStamina);
			if (this.mOldJingliNum != 0)
			{
				Sequence sequence2 = new Sequence();
				sequence2.Append(HOTween.To(this.mJingliValue.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence2.Append(HOTween.To(this.mJingliValue.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence2.Play();
			}
			this.mOldJingliNum = Globals.Instance.Player.Data.Stamina;
		}
		this.Refresh();
	}

	private void Refresh()
	{
		this.mName.text = this.content.Remove(0, this.content.Length).Append(Globals.Instance.Player.Data.Name).Append(" (ID:").Append(Globals.Instance.Player.Data.AccountID).Append(")").ToString();
		this.mName.color = Tools.GetItemQualityColor(Globals.Instance.Player.GetQuality());
	}

	private void RefreshKeys()
	{
		this.content.Remove(0, this.content.Length);
		int maxEnergy = Globals.Instance.Player.GetMaxEnergy();
		if (Globals.Instance.Player.Data.Energy >= maxEnergy)
		{
			this.content.Append(Singleton<StringManager>.Instance.GetString("Energy6"));
		}
		else
		{
			this.costTime = Globals.Instance.Player.Data.EnergyTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (this.costTime < 0)
			{
				this.costTime = 0;
			}
			this.count = maxEnergy - Globals.Instance.Player.Data.Energy - 1;
			this.CountTime = this.count * GameConst.GetInt32(134) + this.costTime;
			this.content.Append(Singleton<StringManager>.Instance.GetString("Energy4"));
			this.content.Append(UIEnergyTooltip.FormatTime(this.CountTime));
		}
		this.mTimer.text = this.content.ToString();
	}

	private void RefreshJingli()
	{
		this.content.Remove(0, this.content.Length);
		int maxStamina = Globals.Instance.Player.GetMaxStamina();
		if (Globals.Instance.Player.Data.Stamina >= maxStamina)
		{
			this.content.Append(Singleton<StringManager>.Instance.GetString("Energy7"));
		}
		else
		{
			this.costTime = Globals.Instance.Player.Data.StaminaTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (this.costTime < 0)
			{
				this.costTime = 0;
			}
			this.count = maxStamina - Globals.Instance.Player.Data.Stamina - 1;
			this.CountTime = this.count * GameConst.GetInt32(136) + this.costTime;
			this.content.Append(Singleton<StringManager>.Instance.GetString("Energy8"));
			this.content.Append(UIEnergyTooltip.FormatTime(this.CountTime));
		}
		this.mTimerJingli.text = this.content.ToString();
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 1f)
		{
			this.RefreshKeys();
			this.RefreshJingli();
			this.timerRefresh = Time.time;
		}
	}

	private void ElfBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIFairyTalePopUp.HttpGetElfQueryUrl(1508, null);
	}

	private void OnMsgElfInit(MemoryStream stream)
	{
		GameUIFairyTalePopUp.TryOpenElf(stream);
	}
}
