using Proto;
using System;
using UnityEngine;

public class GameUItakeName : GameUIBasePopup
{
	private UIInput mInputName;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("personInfoLayer");
		this.mInputName = transform.Find("nameInput").GetComponent<UIInput>();
		GameObject gameObject = transform.Find("randomBtn").gameObject;
		UIEventListener expr_3E = UIEventListener.Get(gameObject);
		expr_3E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3E.onClick, new UIEventListener.VoidDelegate(this.OnRandomNameClick));
		GameObject gameObject2 = transform.Find("okBtn").gameObject;
		UIEventListener expr_76 = UIEventListener.Get(gameObject2);
		expr_76.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_76.onClick, new UIEventListener.VoidDelegate(this.OnOkBtnClick));
		GameObject gameObject3 = transform.Find("cancelBtn").gameObject;
		UIEventListener expr_AE = UIEventListener.Get(gameObject3);
		expr_AE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AE.onClick, new UIEventListener.VoidDelegate(this.OnNoBtnClick));
		this.mInputName.value = Singleton<StringManager>.Instance.GetRecommendName(Globals.Instance.Player.Data.Gender == 0);
		UILabel component = transform.Find("txt2/num").GetComponent<UILabel>();
		component.color = ((Globals.Instance.Player.Data.Diamond >= 100) ? Color.white : Color.red);
		LocalPlayer expr_148 = Globals.Instance.Player;
		expr_148.NameChangeEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_148.NameChangeEvent, new LocalPlayer.VoidCallback(this.OnTakeNameSuccess));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		LocalPlayer expr_1B = Globals.Instance.Player;
		expr_1B.NameChangeEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_1B.NameChangeEvent, new LocalPlayer.VoidCallback(this.OnTakeNameSuccess));
	}

	private void OnRandomNameClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mInputName.value = Singleton<StringManager>.Instance.GetRecommendName(Globals.Instance.Player.Data.Gender == 0);
	}

	private void OnOkBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, 100, 0))
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
			return;
		}
		if (string.IsNullOrEmpty(this.mInputName.value))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("CreateCharacterNullNameError"), 0f, 0f);
			return;
		}
		if (Tools.GetLength(this.mInputName.value) > 12)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("CreateCharacterMaxNameLengthError"), 0f, 0f);
			return;
		}
		MC2S_ChangeName mC2S_ChangeName = new MC2S_ChangeName();
		mC2S_ChangeName.Name = this.mInputName.value;
		Globals.Instance.CliSession.Send(238, mC2S_ChangeName);
	}

	private void OnTakeNameSuccess()
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnNoBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
