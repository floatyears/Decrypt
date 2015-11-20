using System;
using UnityEngine;

public class GUIAwakeItemsBagItem : GUICommonBagItem
{
	private GUIPropsBagScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUIPropsBagScene);
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
		return Singleton<StringManager>.Instance.GetString("equipImprove69", new object[]
		{
			this.mData.GetCount()
		});
	}

	protected override string GetDesc()
	{
		return this.mData.Info.Desc;
	}

	protected override void OnIconClick(GameObject go)
	{
		GUIAwakeItemInfoPopUp.Show(this.mData.Info, GUIAwakeItemInfoPopUp.EOpenType.EOT_View, null);
	}

	protected override string GetClickableBtnTxt()
	{
		return Singleton<StringManager>.Instance.GetString("PetFurther3");
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.UseItem(this.mData);
	}
}
