using Proto;
using System;
using UnityEngine;

public class GUIGuildImpeachPopUp : GameUIBasePopup
{
	private GameObject mCantTip;

	private UIButton mSureBtn;

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		this.mCantTip = transform.Find("cantTip").gameObject;
		GameObject gameObject = transform.Find("cancelBtn").gameObject;
		UIEventListener expr_3E = UIEventListener.Get(gameObject);
		expr_3E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3E.onClick, new UIEventListener.VoidDelegate(this.OnCancelBtnClick));
		this.mSureBtn = transform.Find("sureBtn").GetComponent<UIButton>();
		UIEventListener expr_85 = UIEventListener.Get(this.mSureBtn.gameObject);
		expr_85.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_85.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		GuildSubSystem expr_B5 = Globals.Instance.Player.GuildSystem;
		expr_B5.ImpeachGuildMasterEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_B5.ImpeachGuildMasterEvent, new GuildSubSystem.VoidCallback(this.OnImpeachGuildMasterEvent));
	}

	private void OnDestroy()
	{
		GuildSubSystem expr_0F = Globals.Instance.Player.GuildSystem;
		expr_0F.ImpeachGuildMasterEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_0F.ImpeachGuildMasterEvent, new GuildSubSystem.VoidCallback(this.OnImpeachGuildMasterEvent));
	}

	private void Refresh()
	{
		this.mCantTip.SetActive(!Tools.GetSelfCanImpectMaster());
		this.mSureBtn.isEnabled = Tools.GetSelfCanImpectMaster();
	}

	private void OnCancelBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnSureBtnClick(GameObject go)
	{
		MC2S_ImpeachGuildMaster ojb = new MC2S_ImpeachGuildMaster();
		Globals.Instance.CliSession.Send(923, ojb);
	}

	private void OnImpeachGuildMasterEvent()
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
