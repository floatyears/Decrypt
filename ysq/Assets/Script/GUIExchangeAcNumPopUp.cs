using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIExchangeAcNumPopUp : GameUIBasePopup
{
	private UIInput mInputNum;

	private void Awake()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(725, new ClientSession.MsgHandler(this.OnMsgExchangeGiftCode));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(725, new ClientSession.MsgHandler(this.OnMsgExchangeGiftCode));
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("bg");
		this.mInputNum = transform.Find("nameInput").GetComponent<UIInput>();
		GameObject gameObject = transform.Find("okBtn").gameObject;
		UIEventListener expr_3E = UIEventListener.Get(gameObject);
		expr_3E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3E.onClick, new UIEventListener.VoidDelegate(this.OnOkBtnClick));
		GameObject gameObject2 = transform.Find("cancelBtn").gameObject;
		UIEventListener expr_76 = UIEventListener.Get(gameObject2);
		expr_76.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_76.onClick, new UIEventListener.VoidDelegate(this.OnCancelClick));
	}

	private void OnOkBtnClick(GameObject go)
	{
		if (!string.IsNullOrEmpty(this.mInputNum.value))
		{
			MC2S_ExchangeGiftCode mC2S_ExchangeGiftCode = new MC2S_ExchangeGiftCode();
			mC2S_ExchangeGiftCode.Code = this.mInputNum.value;
			Globals.Instance.CliSession.Send(724, mC2S_ExchangeGiftCode);
		}
	}

	private void OnCancelClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnMsgExchangeGiftCode(MemoryStream stream)
	{
		MS2C_ExchangeGiftCode mS2C_ExchangeGiftCode = Serializer.NonGeneric.Deserialize(typeof(MS2C_ExchangeGiftCode), stream) as MS2C_ExchangeGiftCode;
		if (mS2C_ExchangeGiftCode.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ExchangeGiftCode.Result);
			return;
		}
		GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("ActivityR_0"), MessageBox.Type.OK, null);
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
