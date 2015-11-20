using System;
using UnityEngine;

public class GUILopetBagItem : GUICommonBagItem
{
	protected override void InitObjects()
	{
	}

	protected override bool IsShowPanel()
	{
		return true;
	}

	protected override string GetLevel()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
		{
			this.mLopetData.Data.Level
		});
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

	protected override string GetEnhanceLb()
	{
		return Singleton<StringManager>.Instance.GetString("lvlup");
	}

	protected override string GetRefineLb()
	{
		return Singleton<StringManager>.Instance.GetString("jinJie");
	}

	protected override bool ShowIconLeftTopTag()
	{
		return this.mLopetData.IsBattling();
	}

	protected override string GetIconLeftTopTagLb()
	{
		return Singleton<StringManager>.Instance.GetString("battle");
	}

	protected override string GetEnhanceMax()
	{
		if (this.mLopetData.IsLevelMax())
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove26");
		}
		return string.Empty;
	}

	protected override string GetRefineMax()
	{
		if (this.mLopetData.IsAwakeMax())
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove26");
		}
		return string.Empty;
	}

	protected override void OnEnhanceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mLopetData.IsLevelMax())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("Lopet6", 0f, 0f);
			return;
		}
		GameUIManager.mInstance.uiState.SelectLopetID = this.mLopetData.GetID();
		GUIPetTrainSceneV2.Show(this.mLopetData, GUIPetTrainSceneV2.EUILopetTabs.E_UILvlUp);
	}

	public override void OnRefineClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mLopetData.IsAwakeMax())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("Lopet7", 0f, 0f);
			return;
		}
		GameUIManager.mInstance.uiState.SelectLopetID = this.mLopetData.GetID();
		GUIPetTrainSceneV2.Show(this.mLopetData, GUIPetTrainSceneV2.EUILopetTabs.E_UIAwake);
	}

	protected override void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.SelectLopetID = this.mLopetData.GetID();
		GUIPetTrainSceneV2.Show(this.mLopetData, GUIPetTrainSceneV2.EUILopetTabs.E_UIBaseInfo);
	}
}
