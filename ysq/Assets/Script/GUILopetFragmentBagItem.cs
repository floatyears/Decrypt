using System;
using UnityEngine;

public class GUILopetFragmentBagItem : GUICommonBagItem
{
	private GUILopetBagScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUILopetBagScene);
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
		if (this.mData.CanCreate())
		{
			this.mPoint0Value.color = Color.green;
			return Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				this.mData.GetCount(),
				this.mData.Info.Value1
			});
		}
		this.mPoint0Value.color = Color.red;
		return Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
		{
			this.mData.GetCount(),
			this.mData.Info.Value1
		});
	}

	protected override string GetTip()
	{
		if (this.mData.CanCreate())
		{
			return string.Empty;
		}
		return Singleton<StringManager>.Instance.GetString("equipImprove7");
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData.CanCreate())
		{
			this.mBaseScene.SendLopetCreateMsg(this.mData);
		}
		else
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mData.Info);
		}
	}

	protected override void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ShowLopetInfo(this.mData.Info);
	}

	protected override string GetClickableBtnTxt()
	{
		if (this.mData.CanCreate())
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove8");
		}
		return Singleton<StringManager>.Instance.GetString("equipImprove9");
	}

	protected override bool ShowClickableBtnEffect()
	{
		return this.mData.CanCreate();
	}
}
