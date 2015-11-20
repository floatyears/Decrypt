using System;
using UnityEngine;

public class MysteryBtn : MonoBehaviour
{
	public delegate void VoidCallBack();

	public MysteryBtn.VoidCallBack OnClickEvent;

	private UISprite mBG;

	private UISprite mLock;

	private UILabel mTip;

	private UILabel mName;

	private UISprite mNameBG;

	private UISprite mRed;

	private UIButton[] btns;

	private bool isOpen;

	public bool IsOpen
	{
		get
		{
			return this.isOpen;
		}
		set
		{
			this.isOpen = value;
			if (this.isOpen)
			{
				this.mLock.enabled = false;
				this.mBG.color = Color.white;
				this.mNameBG.color = Color.white;
				this.mName.color = Color.white;
			}
			else
			{
				this.SetRed(false);
				this.mBG.color = Color.black;
				this.mNameBG.color = Color.black;
				this.mName.color = Tools.GetDisabledTextColor(96);
				this.mName.effectStyle = UILabel.Effect.None;
				UIButton[] array = this.btns;
				for (int i = 0; i < array.Length; i++)
				{
					UIButton uIButton = array[i];
					uIButton.enabled = false;
				}
			}
		}
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBG = base.gameObject.GetComponent<UISprite>();
		this.mLock = GameUITools.FindUISprite("Lock", base.gameObject);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mTip = GameUITools.FindUILabel("Tip", this.mName.gameObject);
		this.mNameBG = GameUITools.FindUISprite("BG", this.mName.gameObject);
		this.mRed = GameUITools.FindUISprite("Red", this.mName.gameObject);
		this.btns = base.gameObject.GetComponentsInChildren<UIButton>();
	}

	public void SetTip(string str)
	{
		this.mTip.text = str;
	}

	public void SetRed(bool visible)
	{
		this.mRed.enabled = visible;
	}

	private void OnClick()
	{
		if (this.isOpen)
		{
			if (this.OnClickEvent != null)
			{
				this.OnClickEvent();
			}
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(this.mTip.text, 0f, 0f);
		}
	}
}
