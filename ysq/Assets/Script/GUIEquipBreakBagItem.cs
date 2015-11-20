using Att;
using System;
using UnityEngine;

public class GUIEquipBreakBagItem : GUICommonBagItem
{
	private GUISelectItemBagScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUISelectItemBagScene);
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

	protected override bool ShowSelect()
	{
		return true;
	}

	protected override bool OnPreSelectToggleChange(bool isCheck)
	{
		return !isCheck || this.mBaseScene.mSelectItemLayer.CanAddItem(this.mData);
	}

	protected override void OnSelectToggleChange(bool isCheck)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mData.IsSelected = isCheck;
		if (isCheck)
		{
			this.mBaseScene.mSelectItemLayer.AddItem(this.mData);
		}
		else
		{
			this.mBaseScene.mSelectItemLayer.DeleteItem(this.mData);
		}
	}

	protected override void OnIconClick(GameObject go)
	{
	}
}
