using System;
using UnityEngine;

public class GUITrinketBagItem : GUICommonBagItem
{
	protected override void InitObjects()
	{
		this.mRefineLock.enabled = !Globals.Instance.Player.ItemSystem.CanTrinketRefine();
	}

	protected override bool IsShowPanel()
	{
		return true;
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

	protected override PetDataEx GetPetDataEx()
	{
		return this.mData.GetEquipPet();
	}

	protected override string GetPoint0()
	{
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove68");
		}
		return Tools.GetTrinketAEStr(this.mData, 0) + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint0Value()
	{
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return this.mData.GetTrinketOrItem2EnhanceExp().ToString();
		}
		return this.mData.GetTrinketEnhanceAttValue0().ToString();
	}

	protected override string GetPoint1()
	{
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return string.Empty;
		}
		return Tools.GetTrinketAEStr(this.mData, 1) + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint1Value()
	{
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return string.Empty;
		}
		return Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
		{
			this.mData.GetTrinketEnhanceAttValue1().ToString("0.0")
		});
	}

	protected override int GetRefineLevel()
	{
		return this.mData.GetTrinketRefineLevel();
	}

	protected override string GetEnhanceMax()
	{
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove66");
		}
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
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove67");
		}
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
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return;
		}
		if (this.mData.CanEnhance())
		{
			GameUIManager.mInstance.uiState.SelectItemID = this.mData.Data.ID;
			GUITrinketUpgradeScene.Change2This(this.mData, GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance, -1);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
		}
	}

	public override void OnRefineClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(this.mData.Info.ID))
		{
			return;
		}
		if (Globals.Instance.Player.ItemSystem.CanTrinketRefine())
		{
			if (this.mData.CanRefine())
			{
				GameUIManager.mInstance.uiState.SelectItemID = this.mData.Data.ID;
				GUITrinketUpgradeScene.Change2This(this.mData, GUITrinketUpgradeScene.EUpgradeType.EUT_Refine, -1);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
			}
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove44", new object[]
			{
				GameConst.GetInt32(13)
			}), 0f, 0f);
		}
	}

	protected override bool ShowListGoBtn()
	{
		return true;
	}

	protected override void OnListGoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData != null)
		{
			GUIPillageScene.LastSelectRecipe = this.mData.Info.ID;
		}
		GUIPillageScene.TryOpen(false);
		GameUIManager.mInstance.uiState.SelectItemID = this.mData.GetID();
	}

	protected override void OnIconClick(GameObject go)
	{
		GUIEquipInfoPopUp.ShowThis(this.mData, GUIEquipInfoPopUp.EIPT.EIPT_Bag, -1, false, true);
	}
}
