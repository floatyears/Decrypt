using System;
using UnityEngine;

public class BillboardChildBtn : MonoBehaviour
{
	public delegate void VoidCallback();

	public BillboardChildBtn.VoidCallback SendMessageToServer;

	private GUIBillboard mBaseScene;

	private UIButton mBtn;

	private UILabel mName;

	private UIDragScrollView scrollView;

	public void Init(GUIBillboard baseScene, string name, BillboardChildBtn.VoidCallback cb)
	{
		this.mBaseScene = baseScene;
		this.SendMessageToServer = cb;
		this.CreateObjects();
		this.mName.text = name;
	}

	private void CreateObjects()
	{
		this.mBtn = base.gameObject.GetComponent<UIButton>();
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mBtn.normalSprite = "btn_dark3";
		EventDelegate.Add(this.mBtn.onClick, new EventDelegate.Callback(this.OnBtnClick));
	}

	public void OnBtnClick()
	{
		if (this.mBaseScene.mHasStarted)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		}
		else
		{
			this.mBaseScene.mHasStarted = true;
		}
		if (this.mBaseScene.isWaitingMessageReply)
		{
			return;
		}
		if (this.mBaseScene.mCurChildBtn != this)
		{
			if (this.SendMessageToServer != null)
			{
				this.SendMessageToServer();
				this.mBaseScene.isWaitingMessageReply = true;
			}
			if (this.mBaseScene.mCurChildBtn != null)
			{
				this.mBaseScene.mCurChildBtn.SetActiveState(false);
			}
			this.mBaseScene.mCurChildBtn = this;
			this.SetActiveState(true);
		}
	}

	public void SetActiveState(bool isChecked)
	{
		if (isChecked)
		{
			this.mBtn.normalSprite = "btn_check3";
		}
		else
		{
			this.mBtn.normalSprite = "btn_dark3";
		}
	}
}
