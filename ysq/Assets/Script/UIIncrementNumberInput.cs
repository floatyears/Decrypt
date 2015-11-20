using System;
using UnityEngine;

[AddComponentMenu("Game UI/Increment Number Input")]
public class UIIncrementNumberInput : MonoBehaviour
{
	public UIEventListener.VoidDelegate NumberChangedEvent;

	private int number = 1;

	private int minimum = 1;

	private int maximum = 1000;

	private int maxStep = 10;

	public UIInput numberInput;

	public UIButton btnMinus;

	public UIButton btnPlus;

	public UIButton btnMaxPlus;

	public UILabel labelMaxPlus;

	public UIButton btnMaxMinus;

	public UILabel labelMaxMinus;

	private bool init;

	public int Number
	{
		get
		{
			return this.number;
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
				this.ChangeValue(this.number);
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
				if (value > 9999)
				{
					value = 9999;
				}
				this.maximum = value;
				this.ChangeValue(this.number);
			}
		}
	}

	public int MaxStep
	{
		get
		{
			return this.maxStep;
		}
		set
		{
			if (this.maxStep != value)
			{
				this.maxStep = value;
				this.labelMaxPlus.text = string.Format("+{0}", this.MaxStep);
				this.labelMaxMinus.text = string.Format("-{0}", this.MaxStep);
			}
		}
	}

	public void Init()
	{
		if (this.init)
		{
			return;
		}
		this.init = true;
		if (this.numberInput == null)
		{
			this.numberInput = base.GetComponent<UIInput>();
		}
		if (this.numberInput != null)
		{
			this.numberInput.validation = UIInput.Validation.Integer;
			this.numberInput.value = this.number.ToString();
			this.numberInput.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputValueChange)));
			this.numberInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputValueChange)));
		}
		if (this.btnMinus != null)
		{
			UIEventListener expr_C6 = UIEventListener.Get(this.btnMinus.gameObject);
			expr_C6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C6.onClick, new UIEventListener.VoidDelegate(this.OnClickMinus));
		}
		if (this.btnPlus != null)
		{
			UIEventListener expr_108 = UIEventListener.Get(this.btnPlus.gameObject);
			expr_108.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_108.onClick, new UIEventListener.VoidDelegate(this.OnClickPlus));
		}
		if (this.btnMaxPlus != null)
		{
			UIEventListener expr_14A = UIEventListener.Get(this.btnMaxPlus.gameObject);
			expr_14A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_14A.onClick, new UIEventListener.VoidDelegate(this.OnClickMaxPlus));
		}
		if (this.btnMaxMinus != null)
		{
			UIEventListener expr_18C = UIEventListener.Get(this.btnMaxMinus.gameObject);
			expr_18C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18C.onClick, new UIEventListener.VoidDelegate(this.OnClickMaxMinus));
		}
		if (this.labelMaxPlus != null)
		{
			this.labelMaxPlus.text = string.Format("+{0}", this.MaxStep);
		}
		if (this.labelMaxMinus != null)
		{
			this.labelMaxMinus.text = string.Format("-{0}", this.MaxStep);
		}
	}

	private void Awake()
	{
		this.Init();
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
			this.NumberChangedEvent(base.gameObject);
		}
	}

	private void OnClickPlus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.numberInput == null)
		{
			return;
		}
		this.ChangeValue(this.number + 1);
	}

	private void OnClickMinus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.ChangeValue(this.number - 1);
	}

	private void OnClickMaxPlus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.number == this.minimum)
		{
			this.ChangeValue(this.MaxStep);
		}
		else
		{
			this.ChangeValue(this.Number + this.MaxStep);
		}
	}

	private void OnClickMaxMinus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.number == this.minimum)
		{
			this.ChangeValue(-this.MaxStep);
		}
		else
		{
			this.ChangeValue(this.Number - this.MaxStep);
		}
	}

	private void ChangeValue(int value)
	{
		this.number = Mathf.Clamp(value, this.minimum, this.maximum);
		this.numberInput.value = this.number.ToString();
	}
}
