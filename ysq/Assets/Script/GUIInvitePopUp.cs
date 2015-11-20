using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIInvitePopUp : GameUIBasePopup
{
	private static float CDTimer = -15f;

	private static int CD = 15;

	private EInviteType type;

	private UILabel mTitle;

	private UIInput mContent;

	private GUIChannelToggle mCostumePartyToggle;

	private GUIChannelToggle mGuildToggle;

	private UILabel mSendTxt;

	private int value;

	private Queue<EChannelType> channelQueue = new Queue<EChannelType>();

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mContent = GameUITools.FindGameObject("Content", base.gameObject).GetComponent<UIInput>();
		this.mContent.characterLimit = 33;
		this.mCostumePartyToggle = GameUITools.FindGameObject("CostumeParty", base.gameObject).AddComponent<GUIChannelToggle>();
		this.mGuildToggle = GameUITools.FindGameObject("Guild", base.gameObject).AddComponent<GUIChannelToggle>();
		this.mCostumePartyToggle.IsChecked = true;
		this.mGuildToggle.IsChecked = true;
		this.mGuildToggle.SetCheckInfo(Globals.Instance.Player.GuildSystem.HasGuild(), Singleton<StringManager>.Instance.GetString("chatTxt13"));
		GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelClick), base.gameObject);
		this.mSendTxt = GameUITools.FindUILabel("Label", GameUITools.RegisterClickEvent("Send", new UIEventListener.VoidDelegate(this.OnSendClick), base.gameObject));
		LocalPlayer expr_10D = Globals.Instance.Player;
		expr_10D.MsgChatEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_10D.MsgChatEvent, new LocalPlayer.VoidCallback(this.OnMsgChatEvent));
		this.RefreshCD();
	}

	private void RefreshCD()
	{
		this.value = (int)(GUIInvitePopUp.CDTimer + (float)GUIInvitePopUp.CD - Time.time);
		if (this.value > 0)
		{
			this.mSendTxt.text = Singleton<StringManager>.Instance.GetString("InvitePopUpSendTxt1", new object[]
			{
				this.value
			});
		}
		else
		{
			this.mSendTxt.text = Singleton<StringManager>.Instance.GetString("InvitePopUpSendTxt0");
		}
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && this.value > -1)
		{
			this.RefreshCD();
		}
	}

	private void OnDestroy()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.MsgChatEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_0A.MsgChatEvent, new LocalPlayer.VoidCallback(this.OnMsgChatEvent));
	}

	public override void InitPopUp(EInviteType type)
	{
		this.type = type;
		if (type == EInviteType.EInviteType_CostumeParty)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("InvitePopUpTitleCostumeParty");
		}
	}

	private void OnSendClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.value > 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("InvitePopUpSendTxt2", 0f, 0f);
			return;
		}
		EInviteType eInviteType = this.type;
		if (eInviteType == EInviteType.EInviteType_CostumeParty)
		{
			if (Globals.Instance.Player.CostumePartySystem.IsPartyFull())
			{
				GameUIManager.mInstance.ShowMessageTipByKey("costumePartyFull", 0f, 0f);
				return;
			}
			if (this.mCostumePartyToggle.IsChecked)
			{
				this.channelQueue.Enqueue(EChannelType.EChannel_World);
			}
			if (this.mGuildToggle.IsChecked)
			{
				this.channelQueue.Enqueue(EChannelType.EChannel_Guild);
			}
			this.SendInviteChat2Server();
		}
	}

	private void SendInviteChat2Server()
	{
		if (this.channelQueue.Count <= 0)
		{
			GUIInvitePopUp.CDTimer = Time.time;
			GameUIPopupManager.GetInstance().PopState(false, null);
			return;
		}
		MC2S_Chat mC2S_Chat = new MC2S_Chat();
		mC2S_Chat.Message = this.mContent.value;
		mC2S_Chat.Channel = (int)this.channelQueue.Dequeue();
		mC2S_Chat.PlayerID = Globals.Instance.Player.Data.ID;
		mC2S_Chat.Type = (uint)this.type;
		mC2S_Chat.Voice = false;
		Globals.Instance.CliSession.Send(216, mC2S_Chat);
	}

	private void OnMsgChatEvent()
	{
		base.Invoke("SendInviteChat2Server", 0.01f);
	}

	private void OnCancelClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}
}
