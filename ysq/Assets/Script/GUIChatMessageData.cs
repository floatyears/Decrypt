using Proto;
using System;

public class GUIChatMessageData : BaseData
{
	public WorldMessageExtend mWorldMessage
	{
		get;
		private set;
	}

	public ChatMessage mChatMessage
	{
		get;
		private set;
	}

	public bool mIsSelfChat
	{
		get;
		private set;
	}

	public bool mIsVoice
	{
		get;
		private set;
	}

	public bool mIsAlreadyPlayed
	{
		get;
		private set;
	}

	public GUIVoiceChatData mGUIVoiceChatData
	{
		get;
		private set;
	}

	public GUIChatMessageData(WorldMessageExtend wm, ChatMessage cm)
	{
		if (wm != null && cm == null)
		{
			this.mWorldMessage = wm;
		}
		if (cm != null && wm == null)
		{
			this.mChatMessage = cm;
		}
		this.Refresh();
	}

	public bool IsVoicePlaying()
	{
		return this.mIsVoice && this.mGUIVoiceChatData != null && Globals.Instance.VoiceMgr.IsPlaying() && !string.IsNullOrEmpty(Globals.Instance.VoiceMgr.mPlayingVoiceParam) && this.mGUIVoiceChatData.VoiceTranslateParam.Equals(Globals.Instance.VoiceMgr.mPlayingVoiceParam);
	}

	public void Refresh()
	{
		this.mIsAlreadyPlayed = true;
		if (this.mWorldMessage != null)
		{
			this.mIsSelfChat = (this.mWorldMessage.mWM.Msg != null && this.mWorldMessage.mWM.Msg.PlayerID == Globals.Instance.Player.Data.ID);
			this.mIsVoice = (this.mWorldMessage.mWM.Msg != null && this.mWorldMessage.mWM.Msg.Voice);
			if (this.mIsVoice)
			{
				this.mGUIVoiceChatData = new GUIVoiceChatData();
				this.mGUIVoiceChatData.FromJsonStr(this.mWorldMessage.mWM.Msg.Message);
				this.mIsAlreadyPlayed = Globals.Instance.VoiceMgr.IsAlreadyPlayed(this.mGUIVoiceChatData.VoiceTranslateParam);
			}
			else
			{
				this.mGUIVoiceChatData = null;
			}
		}
		else if (this.mChatMessage != null)
		{
			this.mIsSelfChat = (this.mChatMessage.PlayerID == Globals.Instance.Player.Data.ID);
			this.mIsVoice = this.mChatMessage.Voice;
			if (this.mIsVoice)
			{
				this.mGUIVoiceChatData = new GUIVoiceChatData();
				this.mGUIVoiceChatData.FromJsonStr(this.mChatMessage.Message);
				this.mIsAlreadyPlayed = Globals.Instance.VoiceMgr.IsAlreadyPlayed(this.mGUIVoiceChatData.VoiceTranslateParam);
			}
			else
			{
				this.mGUIVoiceChatData = null;
			}
		}
		else
		{
			this.mIsSelfChat = false;
			this.mIsVoice = false;
			this.mGUIVoiceChatData = null;
		}
	}

	public override ulong GetID()
	{
		if (this.mWorldMessage != null)
		{
			return this.mWorldMessage.mWM.Msg.PlayerID;
		}
		if (this.mChatMessage != null)
		{
			return this.mChatMessage.PlayerID;
		}
		return 0uL;
	}
}
