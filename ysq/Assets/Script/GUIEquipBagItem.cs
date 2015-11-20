using Att;
using System;
using UnityEngine;

public class GUIEquipBagItem : GUICommonBagItem
{
	protected override void InitObjects()
	{
		this.mRefineLock.enabled = !Globals.Instance.Player.ItemSystem.CanEquipRefine();
	}

	protected override bool IsShowPanel()
	{
		return true;
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

	protected override string GetEnhanceMax()
	{
		if (this.mData.CanEnhance())
		{
			return string.Empty;
		}
		if (this.mData.IsEnhanceMax())
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove26");
		}
		return Singleton<StringManager>.Instance.GetString("equipImprove29");
	}

	protected override string GetRefineMax()
	{
		if (this.mData.CanRefine())
		{
			return string.Empty;
		}
		if (this.mData.IsRefineMax())
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove26");
		}
		return Singleton<StringManager>.Instance.GetString("equipImprove29");
	}

	protected override void OnEnhanceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData.CanEnhance())
		{
			GameUIManager.mInstance.uiState.SelectItemID = this.mData.Data.ID;
			GUIEquipUpgradeScene.Change2This(this.mData, GUIEquipUpgradeScene.EUpgradeType.EUT_Enhance, -1);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
		}
	}

	public override void OnRefineClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.ItemSystem.CanEquipRefine())
		{
			if (this.mData.CanRefine())
			{
				GameUIManager.mInstance.uiState.SelectItemID = this.mData.Data.ID;
				GUIEquipUpgradeScene.Change2This(this.mData, GUIEquipUpgradeScene.EUpgradeType.EUT_Refine, -1);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
			}
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove43", new object[]
			{
				GameConst.GetInt32(11)
			}), 0f, 0f);
		}
	}

	protected override void OnIconClick(GameObject go)
	{
		GUIEquipInfoPopUp.ShowThis(this.mData, GUIEquipInfoPopUp.EIPT.EIPT_Bag, -1, false, true);
	}
}
