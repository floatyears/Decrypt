using System;
using UnityEngine;

public class CommonBagItemToggle : MonoBehaviour
{
	public delegate void ValueChangeCallBack(bool isCheck);

	public delegate bool PreValueChangeCallBack(bool isCheck);

	public CommonBagItemToggle.ValueChangeCallBack ValueChangeEvent;

	public CommonBagItemToggle.PreValueChangeCallBack PreValueChangeEvent;

	private UISprite mCheck;

	private bool isCheck;

	public bool value
	{
		get
		{
			return this.isCheck;
		}
		set
		{
			if (this.isCheck != value)
			{
				this.isCheck = value;
				this.mCheck.enabled = this.isCheck;
				if (this.ValueChangeEvent != null)
				{
					this.ValueChangeEvent(this.isCheck);
				}
			}
		}
	}

	public void SetCheckValue(bool check)
	{
		if (this.isCheck != check)
		{
			this.isCheck = check;
			this.mCheck.enabled = this.isCheck;
		}
	}

	private void OnClick()
	{
		if (this.PreValueChangeEvent != null)
		{
			if (this.PreValueChangeEvent(!this.isCheck))
			{
				this.value = !this.isCheck;
			}
		}
		else
		{
			this.value = !this.isCheck;
		}
	}

	private void Awake()
	{
		this.mCheck = GameUITools.FindUISprite("Check", base.gameObject);
		this.mCheck.enabled = false;
	}
}
