using System;
using UnityEngine;

public class UISliderNumberInput : MonoBehaviour
{
	public EventDelegate.Callback ValueChangeEvent;

	public int Number;

	private int max;

	private UISlider mSlider;

	private UILabel mValue;

	public int Max
	{
		get
		{
			return this.max;
		}
		set
		{
			if (value > 9999)
			{
				this.max = 9999;
			}
			else
			{
				this.max = value;
			}
		}
	}

	public void Init(EventDelegate.Callback cb)
	{
		this.ValueChangeEvent = cb;
		this.mSlider = GameUITools.FindGameObject("ValueBar", base.gameObject).GetComponent<UISlider>();
		EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.OnValueChange));
		this.mValue = GameUITools.FindUILabel("Thumb/Tips/Value", this.mSlider.gameObject);
		GameUITools.RegisterClickEvent("Min", new UIEventListener.VoidDelegate(this.OnMinClick), base.gameObject);
		GameUITools.RegisterClickEvent("Max", new UIEventListener.VoidDelegate(this.OnMaxClick), base.gameObject);
	}

	public void SetValue(float value)
	{
		this.mSlider.value = Mathf.Clamp01(value);
	}

	private void OnValueChange()
	{
		this.Refresh();
		if (this.ValueChangeEvent != null)
		{
			this.ValueChangeEvent();
		}
	}

	private void Refresh()
	{
		this.Max = Mathf.Clamp(this.Max, 1, this.Max);
		this.Number = Mathf.Clamp(Mathf.CeilToInt(this.mSlider.value * (float)this.Max), 1, this.Max);
		this.mValue.text = this.Number.ToString();
	}

	private void OnMinClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mSlider.value = 0f;
	}

	private void OnMaxClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mSlider.value = 1f;
	}
}
