using System;
using UnityEngine;

public class GUIVoiceMsgBtnV2F : MonoBehaviour
{
	private const float mPressedValidTime = 0.5f;

	private GUIChatWindowV2F mBaseScene;

	private bool mWasPressed;

	private float mPressedStartTime;

	public void InitWithBaseScene(GUIChatWindowV2F baseScene)
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
		if (isPressed)
		{
			if (!this.mWasPressed)
			{
				this.mWasPressed = true;
				this.mPressedStartTime = Time.time;
				this.mBaseScene.SetVoiceTipState(0);
			}
		}
		else
		{
			this.mWasPressed = false;
			if (Time.time - this.mPressedStartTime <= 0.5f)
			{
				this.mBaseScene.SetVoiceTipState(2);
			}
			else
			{
				this.mBaseScene.SetVoiceTipState(-1);
				this.mBaseScene.StopVoiceRecord();
			}
		}
	}

	private void OnVoiceMsgDrag(GameObject go, Vector2 delta)
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

	private void Update()
	{
		if (this.mWasPressed && Time.time - this.mPressedStartTime > 0.5f && !this.mBaseScene.mIsRecording)
		{
			this.mBaseScene.StartVoiceRecord();
		}
	}
}
