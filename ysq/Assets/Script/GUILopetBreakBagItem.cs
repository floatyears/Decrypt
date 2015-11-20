using System;
using UnityEngine;

public class GUILopetBreakBagItem : GUICommonBagItem
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
		if (this.mLopetData.Data.Awake > 0u)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				this.mLopetData.Info.Name,
				this.mLopetData.Data.Awake
			});
		}
		return this.mLopetData.Info.Name;
	}

	protected override string GetLevel()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
		{
			this.mLopetData.Data.Level
		});
	}

	protected override string GetPoint0()
	{
		return Singleton<StringManager>.Instance.GetString("pvp4Txt12");
	}

	protected override string GetPoint0Value()
	{
		return this.mLopetData.GetCombatValue().ToString();
	}

	protected override bool ShowStars()
	{
		return false;
	}

	protected override string GetClickableBtnTxt()
	{
		return Singleton<StringManager>.Instance.GetString("recycle35");
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Clear();
		GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Add(this.mLopetData.GetID());
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak);
	}

	protected override void OnIconClick(GameObject go)
	{
	}
}
