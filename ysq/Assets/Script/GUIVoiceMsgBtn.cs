using System;
using UnityEngine;

public class GUIVoiceMsgBtn : MonoBehaviour
{
	protected const float mPressedValidTime = 0.5f;

	protected GUIChatWindowV2 mBaseScene;

	protected bool mWasPressed;

	protected float mPressedStartTime;

	protected bool mCanVoiceMsg;

	public virtual void OnMsgBtnPressed(bool isPressed)
	{
	}

	public void InitWithBaseScene(GUIChatWindowV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		UIEventListener expr_0B = UIEventListener.Get(base.gameObject);
		expr_0B.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_0B.onPress, new UIEventListener.BoolDelegate(this.OnVoiceMsgPressed));
		UIEventListener expr_37 = UIEventListener.Get(base.gameObject);
		expr_37.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_37.onDrag, new UIEventListener.VectorDelegate(this.OnVoiceMsgDrag));
	}

	private void OnVoiceMsgPressed(GameObject go, bool isPressed)
	{
		this.OnMsgBtnPressed(isPressed);
	}

	private void OnVoiceMsgDrag(GameObject go, Vector2 delta)
	{
		if (this.mBaseScene.mIsRecording && this.mCanVoiceMsg)
		{
			if (UICamera.hoveredObject != base.gameObject)
			{
				this.mBaseScene.SetVoiceTipState(1);
			}
			else
			{
				this.mBaseScene.SetVoiceTipState(0);
			}
		}
		else
		{
			this.mBaseScene.SetVoiceTipState(-1);
		}
	}

	private void Update()
	{
		if (this.mBaseScene.mIsRecording && Time.time - this.mPressedStartTime >= 30f)
		{
			this.mBaseScene.StopVoiceRecord(false);
			GUIChatVoiceTipPanel.TryDestroyMe();
		}
	}
}
