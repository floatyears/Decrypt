using System;
using UnityEngine;

public class GUIPetTrainPeiYangTog : MonoBehaviour
{
	public delegate void ToggleChangedCallback(bool isCheck);

	public GUIPetTrainPeiYangTog.ToggleChangedCallback ToggleChangedEvent;

	private GameObject mToggleSp;

	private bool mIsChecked;

	public bool IsChecked
	{
		get
		{
			return this.mIsChecked;
		}
		set
		{
			this.mIsChecked = value;
			this.Refresh();
			if (this.ToggleChangedEvent != null && this.mIsChecked)
			{
				this.ToggleChangedEvent(this.mIsChecked);
			}
		}
	}

	public void InitToggleBtn(bool isChecked = false)
	{
		this.CreateObjects();
		this.IsChecked = isChecked;
	}

	private void CreateObjects()
	{
		this.mToggleSp = base.transform.Find("toggle").gameObject;
		UIEventListener expr_26 = UIEventListener.Get(base.gameObject);
		expr_26.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26.onClick, new UIEventListener.VoidDelegate(this.OnToggleBtnClick));
	}

	private void Refresh()
	{
		this.mToggleSp.SetActive(this.mIsChecked);
	}

	private void OnToggleBtnClick(GameObject go)
	{
		if (!this.IsChecked)
		{
			this.IsChecked = !this.IsChecked;
		}
	}
}
