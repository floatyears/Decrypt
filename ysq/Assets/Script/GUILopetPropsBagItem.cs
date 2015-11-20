using System;
using UnityEngine;

public class GUILopetPropsBagItem : GUICommonBagItem
{
	protected override void InitObjects()
	{
		this.mBG.height = 106;
		this.mInfo.width = 222;
		this.mInfo.transform.localPosition = new Vector3(-17f, 10f, 0f);
		this.mName.transform.localPosition = new Vector3(119f, -38f, 0f);
		this.mPoint0.transform.localPosition = new Vector3(-106f, -16f, 0f);
		this.mRefineLevel.transform.localPosition = new Vector3(64f, -17f, 0f);
		this.mClickableBtnSprite.height = 50;
		this.mClickableBtnSprite.width = 100;
		this.mClickableBtnSprite.transform.localPosition = new Vector3(385f, -50f);
	}

	protected override bool IsShowPanel()
	{
		return false;
	}

	protected override string GetName()
	{
		return this.mData.Info.Name;
	}

	protected override string GetPoint0()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove5");
	}

	protected override string GetPoint0Value()
	{
		return this.mData.GetCount().ToString();
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIHowGetPetItemPopUp.ShowThis(this.mData.Info);
	}

	protected override void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPropsInfoPopUp.Show(this.mData);
	}

	protected override string GetClickableBtnTxt()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove9");
	}
}
