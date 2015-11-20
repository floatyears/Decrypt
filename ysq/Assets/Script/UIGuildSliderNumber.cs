using System;
using UnityEngine;

public class UIGuildSliderNumber : MonoBehaviour
{
	private int minimum = 1;

	private int maximum = 2147483647;

	private int invalidateNum;

	private int mCharLimit;

	private UIInput numberInput;

	private UIButton btnMinus;

	private UIButton btnPlus;

	public int InvalidateNum
	{
		get
		{
			return this.invalidateNum;
		}
		set
		{
			this.invalidateNum = value;
		}
	}

	public int Number
	{
		get
		{
			int result = this.minimum;
			int.TryParse(this.numberInput.value, out result);
			return result;
		}
		set
		{
			this.ChangeValue(value);
		}
	}

	public int CharLimit
	{
		get
		{
			return this.mCharLimit;
		}
		set
		{
			this.mCharLimit = value;
			if (this.numberInput != null)
			{
				this.numberInput.characterLimit = value;
			}
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

	public void InitWithBaseScene()
	{
		this.btnMinus = base.transform.Find("BtnMinus").GetComponent<UIButton>();
		if (this.btnMinus != null)
		{
			UIEventListener expr_3C = UIEventListener.Get(this.btnMinus.gameObject);
			expr_3C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3C.onClick, new UIEventListener.VoidDelegate(this.OnClickMinus));
		}
		this.btnPlus = base.transform.Find("BtnPlus").GetComponent<UIButton>();
		if (this.btnPlus != null)
		{
			UIEventListener expr_99 = UIEventListener.Get(this.btnPlus.gameObject);
			expr_99.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_99.onClick, new UIEventListener.VoidDelegate(this.OnClickPlus));
		}
		if (this.numberInput == null)
		{
			this.numberInput = base.transform.Find("Input").GetComponent<UIInput>();
		}
		if (this.numberInput != null)
		{
			this.numberInput.validation = UIInput.Validation.Integer;
			this.numberInput.value = this.minimum.ToString();
			this.numberInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputValueSubmit)));
		}
	}

	private void OnInputValueSubmit()
	{
		if (this.numberInput == null)
		{
			return;
		}
		int value = this.minimum;
		int.TryParse(this.numberInput.value, out value);
		this.ChangeValue(value);
	}

	private void OnClickPlus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.numberInput == null)
		{
			return;
		}
		int num = this.invalidateNum;
		if (int.TryParse(this.numberInput.value, out num))
		{
			this.ChangeValue(++num);
		}
		else
		{
			this.ChangeValue(num);
		}
	}

	private void OnClickMinus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = this.invalidateNum;
		if (int.TryParse(this.numberInput.value, out num))
		{
			this.ChangeValue(--num);
		}
		else
		{
			this.ChangeValue(num);
		}
	}

	private void ChangeValue(int value)
	{
		int num = Mathf.Clamp(value, this.minimum, this.maximum);
		this.numberInput.value = num.ToString();
	}
}
