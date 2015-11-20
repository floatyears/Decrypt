using System;
using UnityEngine;

[AddComponentMenu("Game UI/Slider Number Input")]
public class UISliderNumber : MonoBehaviour
{
	public delegate void NumberChangedCallback();

	public UISliderNumber.NumberChangedCallback NumberChangedEvent;

	public int minimum = 1;

	public int maximum = 2147483647;

	public UIInput numberInput;

	public UIButton btnMinus;

	public UIButton btnPlus;

	public UIButton btnMaximum;

	public int Number
	{
		get
		{
			return int.Parse(this.numberInput.value);
		}
		set
		{
			this.ChangeValue(value);
		}
	}

	public int Minimum
	{
		get
		{
			return this.minimum;
		}
		set
		{
			if (this.minimum != value)
			{
				this.minimum = value;
				int value2 = int.Parse(this.numberInput.value);
				this.ChangeValue(value2);
			}
		}
	}

	public int Maximum
	{
		get
		{
			return this.maximum;
		}
		set
		{
			if (this.maximum != value)
			{
				this.maximum = value;
				int value2 = int.Parse(this.numberInput.value);
				this.ChangeValue(value2);
			}
		}
	}

	private void Awake()
	{
		if (this.btnMinus != null)
		{
			UIEventListener expr_21 = UIEventListener.Get(this.btnMinus.gameObject);
			expr_21.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_21.onClick, new UIEventListener.VoidDelegate(this.OnClickMinus));
		}
		if (this.btnPlus != null)
		{
			UIEventListener expr_63 = UIEventListener.Get(this.btnPlus.gameObject);
			expr_63.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_63.onClick, new UIEventListener.VoidDelegate(this.OnClickPlus));
		}
		if (this.btnMaximum != null)
		{
			UIEventListener expr_A5 = UIEventListener.Get(this.btnMaximum.gameObject);
			expr_A5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A5.onClick, new UIEventListener.VoidDelegate(this.OnClickMaximum));
		}
		if (this.numberInput == null)
		{
			this.numberInput = base.GetComponent<UIInput>();
		}
		if (this.numberInput != null)
		{
			this.numberInput.validation = UIInput.Validation.Integer;
			this.numberInput.value = this.minimum.ToString();
			this.numberInput.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputValueChange)));
			this.numberInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputValueChange)));
		}
	}

	private void OnInputValueChange()
	{
		if (this.numberInput == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.numberInput.value))
		{
			this.ChangeValue(this.minimum);
		}
		else
		{
			int value = int.Parse(this.numberInput.value);
			this.ChangeValue(value);
		}
		if (this.NumberChangedEvent != null)
		{
			this.NumberChangedEvent();
		}
	}

	private void OnClickPlus(GameObject go)
	{
		if (this.numberInput == null)
		{
			return;
		}
		int num = int.Parse(this.numberInput.value);
		this.ChangeValue(num + 1);
	}

	private void OnClickMinus(GameObject go)
	{
		int num = int.Parse(this.numberInput.value);
		this.ChangeValue(num - 1);
	}

	private void OnClickMaximum(GameObject go)
	{
		this.ChangeValue(this.maximum);
	}

	private void ChangeValue(int value)
	{
		int num = Mathf.Clamp(value, this.minimum, this.maximum);
		this.numberInput.value = num.ToString();
	}
}
