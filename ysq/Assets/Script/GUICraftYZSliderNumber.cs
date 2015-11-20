using System;
using UnityEngine;

public class GUICraftYZSliderNumber : MonoBehaviour
{
	private int minimum = 1;

	private int maximum = 2147483647;

	private int mCharLimit;

	private BoxCollider numberInputCollider;

	private UIInput numberInput;

	private UIButton btnMinus;

	private UIButton btnPlus;

	private bool mEditable = true;

	public bool Editable
	{
		get
		{
			return this.mEditable;
		}
		set
		{
			this.mEditable = value;
			this.numberInputCollider.enabled = value;
			this.btnMinus.gameObject.SetActive(this.mEditable);
			this.btnPlus.gameObject.SetActive(this.mEditable);
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

	public int Number
	{
		get
		{
			int result = this.minimum;
			if (int.TryParse(this.numberInput.value, out result))
			{
				return result;
			}
			return this.minimum;
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

	public void InitWithBaseScene()
	{
		this.btnMinus = base.transform.Find("leftBtn").GetComponent<UIButton>();
		if (this.btnMinus != null)
		{
			UIEventListener expr_3C = UIEventListener.Get(this.btnMinus.gameObject);
			expr_3C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3C.onClick, new UIEventListener.VoidDelegate(this.OnClickMinus));
		}
		this.btnPlus = base.transform.Find("rightBtn").GetComponent<UIButton>();
		if (this.btnPlus != null)
		{
			UIEventListener expr_99 = UIEventListener.Get(this.btnPlus.gameObject);
			expr_99.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_99.onClick, new UIEventListener.VoidDelegate(this.OnClickPlus));
		}
		if (this.numberInput == null)
		{
			this.numberInput = base.transform.GetComponent<UIInput>();
		}
		if (this.numberInput != null)
		{
			this.numberInputCollider = this.numberInput.GetComponent<BoxCollider>();
			this.numberInput.validation = UIInput.Validation.Integer;
			this.numberInput.value = this.minimum.ToString();
			this.numberInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.OnInputValueSubmit)));
		}
	}

	private void ChangeValue(int value)
	{
		int num = Mathf.Clamp(value, this.minimum, this.maximum);
		this.numberInput.value = num.ToString();
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

	private void OnClickMinus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.numberInput == null)
		{
			return;
		}
		int num = this.Minimum;
		if (int.TryParse(this.numberInput.value, out num))
		{
			this.ChangeValue(--num);
		}
		else
		{
			this.ChangeValue(num);
		}
	}

	private void OnClickPlus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.numberInput == null)
		{
			return;
		}
		int num = this.Maximum;
		if (int.TryParse(this.numberInput.value, out num))
		{
			this.ChangeValue(++num);
		}
		else
		{
			this.ChangeValue(num);
		}
	}
}
