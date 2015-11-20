using System;
using UnityEngine;

public class EquipMasterTab : MonoBehaviour
{
	public delegate void CheckChangeCallback(bool isCheck);

	public EquipMasterTab.CheckChangeCallback CheckChangeCallbackEvent;

	private UISprite mTab0;

	private GameObject mTabCheck;

	private UILabel mtabTxt0;

	public static EquipMasterTab mCurrent;

	public static bool HasInitFlag;

	private bool isCheck;

	private bool isEnabled = true;

	public bool value
	{
		get
		{
			return this.isCheck;
		}
		set
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (this.isCheck != value)
			{
				this.isCheck = value;
				if (this.mTabCheck != null)
				{
					this.mTabCheck.gameObject.SetActive(this.isCheck);
				}
				if (this.mtabTxt0 != null)
				{
					this.mtabTxt0.enabled = !this.isCheck;
				}
				if (this.isCheck)
				{
					if (EquipMasterTab.HasInitFlag)
					{
						Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					}
					else
					{
						EquipMasterTab.HasInitFlag = true;
					}
					if (EquipMasterTab.mCurrent != null && EquipMasterTab.mCurrent != this)
					{
						EquipMasterTab.mCurrent.value = false;
					}
					EquipMasterTab.mCurrent = this;
				}
				if (this.CheckChangeCallbackEvent != null)
				{
					this.CheckChangeCallbackEvent(this.isCheck);
				}
			}
		}
	}

	public bool IsEnabled
	{
		get
		{
			return this.isEnabled;
		}
		set
		{
			this.isEnabled = value;
			if (this.isEnabled)
			{
				this.mtabTxt0.applyGradient = true;
				this.mTab0.spriteName = "tab1";
			}
			else
			{
				this.mtabTxt0.applyGradient = false;
				this.mTab0.spriteName = "tab2";
			}
		}
	}

	public void Init(EquipMasterTab.CheckChangeCallback cb)
	{
		this.CreateObjects();
		this.CheckChangeCallbackEvent = cb;
	}

	private void CreateObjects()
	{
		this.mTab0 = base.gameObject.GetComponent<UISprite>();
		this.mTabCheck = GameUITools.FindGameObject("tabCheck0", base.gameObject);
		this.mTabCheck.gameObject.SetActive(false);
		this.mtabTxt0 = GameUITools.FindUILabel("tabTxt0", base.gameObject);
		this.mtabTxt0.enabled = true;
	}

	private void OnClick()
	{
		if (!this.isCheck)
		{
			this.value = true;
		}
	}
}
