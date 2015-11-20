using Proto;
using System;
using UnityEngine;

public class GUIGuildChangeNamePopUp : GameUIBasePopup
{
	private UIInput mNameInput;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		this.mNameInput = transform.Find("nameInput").GetComponent<UIInput>();
		GameObject gameObject = transform.Find("cancelBtn").gameObject;
		UIEventListener expr_3E = UIEventListener.Get(gameObject);
		expr_3E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3E.onClick, new UIEventListener.VoidDelegate(this.OnCancelBtnClick));
		GameObject gameObject2 = transform.Find("sureBtn").gameObject;
		UIEventListener expr_76 = UIEventListener.Get(gameObject2);
		expr_76.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_76.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		GuildSubSystem expr_A6 = Globals.Instance.Player.GuildSystem;
		expr_A6.GuildNameChangedEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_A6.GuildNameChangedEvent, new GuildSubSystem.VoidCallback(this.OnGuildNameChangedEvent));
	}

	private void OnDestroy()
	{
		GuildSubSystem expr_0F = Globals.Instance.Player.GuildSystem;
		expr_0F.GuildNameChangedEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_0F.GuildNameChangedEvent, new GuildSubSystem.VoidCallback(this.OnGuildNameChangedEvent));
	}

	private void OnCancelBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnSureBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!string.IsNullOrEmpty(this.mNameInput.value))
		{
			if (this.mNameInput.value == Globals.Instance.Player.GuildSystem.Guild.Name)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guild11", 0f, 0f);
				return;
			}
			if (string.IsNullOrEmpty(this.mNameInput.value))
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guild1", 0f, 0f);
				return;
			}
			if (Tools.GetLength(this.mNameInput.value) > 12)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("EGR_11", 0f, 0f);
				return;
			}
			MC2S_ChangeGuildName mC2S_ChangeGuildName = new MC2S_ChangeGuildName();
			mC2S_ChangeGuildName.Name = this.mNameInput.value;
			Globals.Instance.CliSession.Send(952, mC2S_ChangeGuildName);
		}
	}

	private void OnGuildNameChangedEvent()
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
