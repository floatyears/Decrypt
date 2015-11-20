using System;
using UnityEngine;

public class GUIElfCheckBtn : MonoBehaviour
{
	public static GUIElfCheckBtn mCurrent;

	private ElfBtnItem btnData;

	private UISprite mCheckSp;

	private UILabel mTabName;

	private bool mIsChecked;

	public bool IsChecked
	{
		get
		{
			return this.mIsChecked;
		}
		set
		{
			if (this.mIsChecked != value)
			{
				this.mIsChecked = value;
				this.mCheckSp.spriteName = ((!this.mIsChecked) ? "Price_bg" : "gold_bg");
				if (this.mIsChecked)
				{
					if (GUIElfCheckBtn.mCurrent != null && GUIElfCheckBtn.mCurrent != this)
					{
						GUIElfCheckBtn.mCurrent.IsChecked = false;
					}
					GUIElfCheckBtn.mCurrent = this;
				}
			}
		}
	}

	public string Text
	{
		get
		{
			return this.mTabName.text;
		}
		set
		{
			this.mTabName.text = value;
		}
	}

	public void InitWithBaseScene(bool isCheck, ElfBtnItem data)
	{
		this.mIsChecked = isCheck;
		this.btnData = data;
		this.CreateObjects();
		this.Text = this.btnData.strName;
	}

	private void CreateObjects()
	{
		this.mCheckSp = base.transform.GetComponent<UISprite>();
		this.mCheckSp.spriteName = ((!this.mIsChecked) ? "Price_bg" : "gold_bg");
		this.mTabName = this.mCheckSp.transform.Find("tabTxt").GetComponent<UILabel>();
		UIEventListener expr_61 = UIEventListener.Get(base.gameObject);
		expr_61.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_61.onClick, new UIEventListener.VoidDelegate(this.OnTabBtnClick));
		GameUITools.UpdateUIBoxCollider(base.transform, 16f, false);
	}

	private void OnTabBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.IsChecked = true;
		GameUIFairyTalePopUp.HttpGetElfQueryUrl(1509, this.btnData.strQuest);
	}
}
