using System;
using UnityEngine;

public class GUIChannelToggle : MonoBehaviour
{
	public static BetterList<GUIChannelToggle> list = new BetterList<GUIChannelToggle>();

	private GameObject mChecked;

	private bool isChecked;

	private bool canCheck = true;

	private string noCheckTips;

	public bool CanCheck
	{
		get
		{
			return this.canCheck;
		}
		private set
		{
			this.canCheck = value;
		}
	}

	public bool IsChecked
	{
		get
		{
			return this.isChecked;
		}
		set
		{
			if (value)
			{
				this.mChecked.SetActive(true);
				this.isChecked = value;
			}
			else
			{
				bool flag = false;
				foreach (GUIChannelToggle current in GUIChannelToggle.list)
				{
					if (current != this && current.CanCheck && current.IsChecked)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.mChecked.SetActive(false);
					this.isChecked = value;
				}
			}
		}
	}

	public void SetCheckInfo(bool i, string tips)
	{
		this.canCheck = i;
		this.noCheckTips = tips;
		if (!this.canCheck)
		{
			this.IsChecked = false;
		}
	}

	private void OnEnable()
	{
		GUIChannelToggle.list.Add(this);
	}

	private void OnDisable()
	{
		GUIChannelToggle.list.Remove(this);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mChecked = GameUITools.FindGameObject("Checked", base.gameObject);
	}

	private void OnClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!this.canCheck)
		{
			GameUIManager.mInstance.ShowMessageTip(this.noCheckTips, 0f, 0f);
			return;
		}
		this.IsChecked = !this.isChecked;
	}
}
