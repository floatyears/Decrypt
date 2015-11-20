using Att;
using System;
using UnityEngine;

public class GUISelectEquipBagItem : GUICommonBagItem
{
	private GUISelectEquipBagScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUISelectEquipBagScene);
		this.mRefineLevel.transform.localPosition = new Vector3(67.25f, 20f, 0f);
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

	protected override PetDataEx GetPetDataEx()
	{
		return this.mData.GetEquipPet();
	}

	protected override string GetPoint0()
	{
		if (this.mData.Info.Type == 0)
		{
			return Tools.GetEquipAEStr((ESubTypeEquip)this.mData.Info.SubType) + Singleton<StringManager>.Instance.GetString("Colon0");
		}
		if (this.mData.Info.Type == 1)
		{
			return Tools.GetTrinketAEStr(this.mData, 0) + Singleton<StringManager>.Instance.GetString("Colon0");
		}
		return string.Empty;
	}

	protected override string GetPoint0Value()
	{
		if (this.mData.Info.Type == 0)
		{
			return this.mData.GetEquipEnhanceAttValue().ToString();
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

	protected override int GetRefineLevel()
	{
		if (this.mData.Info.Type == 0)
		{
			return this.mData.GetEquipRefineLevel();
		}
		if (this.mData.Info.Type == 1)
		{
			return this.mData.GetTrinketRefineLevel();
		}
		return 0;
	}

	protected override string GetRelation()
	{
		if (this.mData.RelationCount > 0)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove20", new object[]
			{
				this.mData.RelationCount
			});
		}
		return string.Empty;
	}

	protected override void OnIconClick(GameObject go)
	{
		GUIEquipInfoPopUp.ShowThis(this.mData, GUIEquipInfoPopUp.EIPT.EIPT_View, -1, false, true);
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData.Info.Type == 1 && !Tools.CanPlay(GameConst.GetInt32(25), true))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove15", new object[]
			{
				GameConst.GetInt32(25)
			}), 0f, 0f);
			return;
		}
		this.mBaseScene.SendEquipItemMsg(this.mData);
	}

	protected override string GetClickableBtnTxt()
	{
		if (!this.mData.IsEquiped())
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove21");
		}
		return string.Empty;
	}
}
