using System;
using UnityEngine;

public class GUISelectLopetBagItem : GUICommonBagItem
{
	private GUISelectLopetBagScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUISelectLopetBagScene);
	}

	protected override bool IsShowPanel()
	{
		return false;
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

	protected override bool ShowIconLeftTopTag()
	{
		return this.mLopetData.IsBattling();
	}

	protected override string GetIconLeftTopTagLb()
	{
		return Singleton<StringManager>.Instance.GetString("battle");
	}

	protected override void OnIconClick(GameObject go)
	{
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!Tools.CanPlay(GameConst.GetInt32(201), true))
		{
			return;
		}
		this.mBaseScene.SendEquipItemMsg(this.mLopetData);
	}

	protected override string GetClickableBtnTxt()
	{
		if (this.mLopetData.IsBattling())
		{
			return string.Empty;
		}
		return Singleton<StringManager>.Instance.GetString("battle");
	}
}
