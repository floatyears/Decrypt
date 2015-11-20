using System;
using UnityEngine;

public class GUITrinketRebornBagItem : GUICommonBagItem
{
	protected override void InitObjects()
	{
	}

	protected override bool IsShowPanel()
	{
		return false;
	}

	protected override string GetName()
	{
		return this.mData.Info.Name;
	}

	protected override string GetLevel()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
		{
			this.mData.GetTrinketEnhanceLevel()
		});
	}

	protected override string GetPoint0()
	{
		return Tools.GetTrinketAEStr(this.mData, 0) + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint0Value()
	{
		return this.mData.GetTrinketEnhanceAttValue0().ToString();
	}

	protected override string GetPoint1()
	{
		return Tools.GetTrinketAEStr(this.mData, 1) + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint1Value()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
		{
			this.mData.GetTrinketEnhanceAttValue1().ToString("0.0")
		});
	}

	protected override int GetRefineLevel()
	{
		return this.mData.GetTrinketRefineLevel();
	}

	protected override string GetClickableBtnTxt()
	{
		return Singleton<StringManager>.Instance.GetString("recycle35");
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID = this.mData.GetID();
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn);
	}

	protected override void OnIconClick(GameObject go)
	{
	}
}
