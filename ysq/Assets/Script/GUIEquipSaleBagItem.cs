using Att;
using System;
using UnityEngine;

public class GUIEquipSaleBagItem : GUICommonBagItem
{
	private GUIEquipSaleScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUIEquipSaleScene);
	}

	protected override bool IsShowPanel()
	{
		return false;
	}

	protected override string GetLevel()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
		{
			this.mData.GetEquipEnhanceLevel()
		});
	}

	protected override string GetName()
	{
		return this.mData.Info.Name;
	}

	protected override string GetPoint0()
	{
		return Tools.GetEquipAEStr((ESubTypeEquip)this.mData.Info.SubType) + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint0Value()
	{
		return this.mData.GetEquipEnhanceAttValue().ToString();
	}

	protected override int GetRefineLevel()
	{
		return this.mData.GetEquipRefineLevel();
	}

	protected override void OnSelectToggleChange(bool isCheck)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mData.IsSelected = isCheck;
		if (isCheck)
		{
			this.mBaseScene.mEquipSaleLayer.AddItem(this.mData);
		}
		else
		{
			this.mBaseScene.mEquipSaleLayer.DeleteItem(this.mData);
		}
	}

	protected override bool ShowSelect()
	{
		return true;
	}

	protected override int GetPrice(out string name, out bool showIcon)
	{
		name = Singleton<StringManager>.Instance.GetString("equipImprove32");
		showIcon = true;
		return this.mData.GetPrice();
	}

	protected override void OnIconClick(GameObject go)
	{
		GUIEquipInfoPopUp.ShowThis(this.mData, GUIEquipInfoPopUp.EIPT.EIPT_View, -1, false, true);
	}
}
