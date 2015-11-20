using System;
using UnityEngine;

public class GUIVoiceMsgBtnCommon : GUIVoiceMsgBtn
{
	public override void OnMsgBtnPressed(bool isPressed)
	{
		if (isPressed)
		{
			if (!this.mWasPressed)
			{
				this.mWasPressed = true;
				this.mCanVoiceMsg = this.mBaseScene.CanVoiceRecordCommon();
				if (this.mCanVoiceMsg)
				{
					this.mPressedStartTime = Time.time;
					this.mBaseScene.SetVoiceTipState(0);
					this.mBaseScene.StartVoiceRecord();
				}
			}
		}
		else
		{
			this.mCanVoiceMsg = false;
			this.mWasPressed = false;
			if (Time.time - this.mPressedStartTime <= 0.5f)
			{
				if (this.mBaseScene.mIsRecording)
				{
					this.mBaseScene.SetVoiceTipState(2);
					this.mBaseScene.StopVoiceRecord(true);
				}
			}
			else if (this.mBaseScene.mIsRecording)
			{
				int state = GUIChatVoiceTipPanel.GetState();
				this.mBaseScene.StopVoiceRecord(state != 0);
				this.mBaseScene.SetVoiceTipState(-1);
			}
		}
	}
}
