using System;
using UnityEngine;

public class GUICastTrinketSelectBagItem : GUICommonBagItem
{
	private SelectTrinketPopUp mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as SelectTrinketPopUp);
		base.transform.localScale = new Vector3(0.93f, 0.9f, 1f);
	}

	protected override bool IsShowPanel()
	{
		return false;
	}

	protected override string GetLevel()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
		{
			this.mData.GetTrinketEnhanceLevel()
		});
	}

	protected override string GetName()
	{
		return this.mData.Info.Name;
	}

	protected override string GetPoint0()
	{
		if (this.mData.Info.Type == 4 && this.mData.Info.SubType == 9)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove31") + Singleton<StringManager>.Instance.GetString("Colon0");
		}
		if (this.mData.Info.Type == 1)
		{
			return Tools.GetTrinketAEStr(this.mData, 0) + Singleton<StringManager>.Instance.GetString("Colon0");
		}
		return string.Empty;
	}

	protected override string GetPoint0Value()
	{
		if (this.mData.Info.Type == 4 && this.mData.Info.SubType == 9)
		{
			return this.mData.GetTrinketOrItem2EnhanceExp().ToString();
		}
		if (this.mData.Info.Type == 1)
		{
			return this.mData.GetTrinketEnhanceAttValue0().ToString();
		}
		return string.Empty;
	}

	protected override string GetPoint1()
	{
		if (this.mData.Info.Type == 1)
		{
			return Tools.GetTrinketAEStr(this.mData, 1) + Singleton<StringManager>.Instance.GetString("Colon0");
		}
		return string.Empty;
	}

	protected override string GetPoint1Value()
	{
		if (this.mData.Info.Type == 1)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
			{
				this.mData.GetTrinketEnhanceAttValue1().ToString("0.0")
			});
		}
		return string.Empty;
	}

	protected override bool OnPreSelectToggleChange(bool isCheck)
	{
		return !isCheck || this.mBaseScene.CanAddItem(this.mData);
	}

	protected override void OnSelectToggleChange(bool isCheck)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mData.IsSelected = isCheck;
		if (isCheck)
		{
			this.mBaseScene.AddItem(this.mData);
		}
		else
		{
			this.mBaseScene.DeleteItem(this.mData);
		}
	}

	protected override bool ShowSelect()
	{
		return true;
	}

	protected override void OnIconClick(GameObject go)
	{
	}
}
