using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameUIPhoneBindPopUp : GameUIBasePopup
{
	private const int REWARD_COUNT = 3;

	private int[] rewardType = new int[3];

	private int[] rewardValue1 = new int[3];

	private int[] rewardValue2 = new int[3];

	private UIInput inputCode;

	private UIInput inputBind;

	private GameUIToolTip toolTips;

	private ItemInfo itemInfo;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		GameObject gameObject = transform.Find("titleBg/CloseBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		Transform transform2 = transform.Find("panel");
		gameObject = transform2.Find("codeBtn").gameObject;
		UIEventListener expr_6C = UIEventListener.Get(gameObject);
		expr_6C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6C.onClick, new UIEventListener.VoidDelegate(this.OnCodeBtnClick));
		gameObject = transform2.Find("bindBtn").gameObject;
		UIEventListener expr_A4 = UIEventListener.Get(gameObject);
		expr_A4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A4.onClick, new UIEventListener.VoidDelegate(this.OnBindBtnClick));
		this.inputCode = transform2.Find("codeInput").GetComponent<UIInput>();
		this.inputCode.defaultText = Singleton<StringManager>.Instance.GetString("phoneBindTxt1");
		this.inputCode.characterLimit = 11;
		this.inputCode.validation = UIInput.Validation.Integer;
		this.inputCode.keyboardType = UIInput.KeyboardType.NumberPad;
		this.inputBind = transform2.Find("bindInput").GetComponent<UIInput>();
		this.inputBind.defaultText = Singleton<StringManager>.Instance.GetString("phoneBindTxt2");
		this.inputBind.characterLimit = 4;
		this.inputBind.validation = UIInput.Validation.Integer;
		this.inputBind.keyboardType = UIInput.KeyboardType.NumberPad;
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(16);
		int num = 0;
		int num2 = 0;
		while (num < info.Day7RewardType.Count && num < 3)
		{
			if (info.Day7RewardType[num] != 0 && info.Day7RewardType[num] != 20)
			{
				int num3 = info.Day7RewardType[num];
				int num4 = info.Day7RewardValue1[num];
				int num5 = info.Day7RewardValue2[num];
				Transform parent = transform.Find(string.Format("RewardItem{0}", num));
				if (num3 == 3)
				{
					this.itemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(num4);
					GameObject go = GameUITools.CreateReward(num3, num4, num5, parent, true, false, 0f, 0f, 0f, 255f, 255f, 255f, 0);
					UIEventListener expr_25C = UIEventListener.Get(go);
					expr_25C.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_25C.onPress, new UIEventListener.BoolDelegate(this.OnRewardPress));
				}
				else
				{
					GameUITools.CreateReward(num3, num4, num5, parent, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
				}
				this.rewardType[num2] = num3;
				this.rewardValue1[num2] = num4;
				this.rewardValue2[num2] = num5;
				num2++;
			}
			num++;
		}
		Globals.Instance.CliSession.Register(395, new ClientSession.MsgHandler(this.OnMsgRequestSMSCode));
		Globals.Instance.CliSession.Register(397, new ClientSession.MsgHandler(this.OnMsgVerifySMSCode));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(395, new ClientSession.MsgHandler(this.OnMsgRequestSMSCode));
		Globals.Instance.CliSession.Unregister(397, new ClientSession.MsgHandler(this.OnMsgVerifySMSCode));
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnRewardPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			if (this.toolTips == null)
			{
				this.toolTips = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
			}
			this.toolTips.Create(Tools.GetCameraRootParent(go.transform), this.itemInfo.Name, this.itemInfo.Desc, this.itemInfo.Quality);
			this.toolTips.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(0f, this.toolTips.transform.localPosition.y, -5000f));
			this.toolTips.EnableToolTip();
		}
		else if (this.toolTips != null)
		{
			this.toolTips.HideTipAnim();
		}
	}

	private void OnCodeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((Globals.Instance.Player.Data.DataFlag & 8) != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", 94);
			return;
		}
		string value = this.inputCode.value;
		if (string.IsNullOrEmpty(value))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt5"), 0f, 0f);
			return;
		}
		if (!Regex.IsMatch(value, "^13\\d{9}$|^14\\d{9}$|^15\\d{9}$|^18\\d{9}$"))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt6"), 0f, 0f);
			return;
		}
		MC2S_RequestSMSCode mC2S_RequestSMSCode = new MC2S_RequestSMSCode();
		mC2S_RequestSMSCode.PhoneNumber = value;
		Globals.Instance.CliSession.Send(394, mC2S_RequestSMSCode);
	}

	private void OnBindBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((Globals.Instance.Player.Data.DataFlag & 8) != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", 94);
			return;
		}
		string value = this.inputBind.value;
		if (string.IsNullOrEmpty(value))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt7"), 0f, 0f);
			return;
		}
		if (!Regex.IsMatch(value, "^\\d{4}$"))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt8"), 0f, 0f);
			return;
		}
		MC2S_VerifySMSCode mC2S_VerifySMSCode = new MC2S_VerifySMSCode();
		mC2S_VerifySMSCode.Code = value;
		Globals.Instance.CliSession.Send(396, mC2S_VerifySMSCode);
	}

	public void OnMsgRequestSMSCode(MemoryStream stream)
	{
		MS2C_RequestSMSCode mS2C_RequestSMSCode = Serializer.NonGeneric.Deserialize(typeof(MS2C_RequestSMSCode), stream) as MS2C_RequestSMSCode;
		if (mS2C_RequestSMSCode.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_RequestSMSCode.Result);
			return;
		}
		GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt3"), 0f, 0f);
	}

	public void OnMsgVerifySMSCode(MemoryStream stream)
	{
		MS2C_VerifySMSCode mS2C_VerifySMSCode = Serializer.NonGeneric.Deserialize(typeof(MS2C_VerifySMSCode), stream) as MS2C_VerifySMSCode;
		if (mS2C_VerifySMSCode.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_VerifySMSCode.Result);
			return;
		}
		GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("phoneBindTxt4"), 0f, 0f);
		List<RewardMessagebox.RewardData> list = new List<RewardMessagebox.RewardData>(this.rewardType.Length);
		for (int i = 0; i < this.rewardType.Length; i++)
		{
			list.Add(new RewardMessagebox.RewardData
			{
				rewardType = this.rewardType[i],
				rewardValue1 = this.rewardValue1[i],
				rewardValue2 = this.rewardValue2[i]
			});
		}
		RewardMessagebox.GetInstance().ShowRewardMessageBox(Singleton<StringManager>.Instance.GetString("getRewardLb"), string.Empty, list, true);
		this.OnCloseClick(null);
	}
}
