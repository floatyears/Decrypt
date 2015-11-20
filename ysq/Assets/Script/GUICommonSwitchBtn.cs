using System;
using UnityEngine;

public class GUICommonSwitchBtn : MonoBehaviour
{
	public delegate void BtnSwithCallback(bool isOpen);

	public GUICommonSwitchBtn.BtnSwithCallback BtnSwithCallbackEvent;

	private bool mIsOpen;

	private GameObject mSwithBtn;

	private GameObject mOpenGo;

	private GameObject mCloseGo;

	public bool IsOpen
	{
		get
		{
			return this.mIsOpen;
		}
		set
		{
			this.mIsOpen = value;
			this.Refresh();
			if (this.BtnSwithCallbackEvent != null)
			{
				this.BtnSwithCallbackEvent(this.mIsOpen);
			}
		}
	}

	public void InitSwithBtn(bool isOpen)
	{
		this.CreateObjects();
		this.IsOpen = isOpen;
	}

	private void CreateObjects()
	{
		this.mSwithBtn = base.transform.Find("btn").gameObject;
		this.mOpenGo = base.transform.Find("open").gameObject;
		UIEventListener expr_41 = UIEventListener.Get(this.mOpenGo);
		expr_41.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_41.onClick, new UIEventListener.VoidDelegate(this.OnSwithBtnClick));
		this.mCloseGo = base.transform.Find("close").gameObject;
		UIEventListener expr_88 = UIEventListener.Get(this.mCloseGo);
		expr_88.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_88.onClick, new UIEventListener.VoidDelegate(this.OnSwithBtnClick));
	}

	private void OnSwithBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.IsOpen = !this.IsOpen;
	}

	private void Refresh()
	{
		if (this.IsOpen)
		{
			this.mSwithBtn.transform.localPosition = new Vector3(-41f, 0f, 0f);
		}
		else
		{
			this.mSwithBtn.transform.localPosition = new Vector3(-130f, 0f, 0f);
		}
		this.mOpenGo.SetActive(this.mIsOpen);
		this.mCloseGo.SetActive(!this.mIsOpen);
	}
}
