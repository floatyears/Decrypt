using System;
using UnityEngine;

public class GUIMagicMirrorSelectPetItem : GUICommonBagItem
{
	private GUIMagicMirrorScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUIMagicMirrorScene);
		this.mBG.height = 106;
		this.mInfo.width = 222;
		this.mInfo.height = 66;
		this.mInfo.transform.localPosition = new Vector3(-17f, -4f, 0f);
		this.mName.transform.localPosition = new Vector3(119f, -20f, 0f);
		this.mPoint0.transform.localPosition = new Vector3(-106f, 14f, 0f);
		this.mPoint1.transform.localPosition = new Vector3(-106f, -13f, 0f);
		this.mStars.transform.localPosition = new Vector3(-87f, -12f, 0f);
		this.mClickableBtnSprite.height = 50;
		this.mClickableBtnSprite.width = 100;
		this.mClickableBtnSprite.transform.localPosition = new Vector3(385f, -50f);
	}

	protected override bool IsShowPanel()
	{
		return false;
	}

	protected override bool ShowIconLeftTopTag()
	{
		return this.mBaseScene.setIn && (this.mPetData.IsBattling() || this.mPetData.IsPetAssisting());
	}

	protected override string GetIconLeftTopTagLb()
	{
		if (this.mBaseScene.setIn)
		{
			if (this.mPetData.IsBattling())
			{
				return Singleton<StringManager>.Instance.GetString("PetFurther6");
			}
			if (this.mPetData.IsPetAssisting())
			{
				return Singleton<StringManager>.Instance.GetString("PetFurther8");
			}
		}
		return string.Empty;
	}

	protected override string GetName()
	{
		if (this.mBaseScene.setIn && this.mPetData.Data.Further > 0u)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetPetName(this.mPetData.Info),
				this.mPetData.Data.Further
			});
		}
		return Tools.GetPetName(this.mPetData.Info);
	}

	protected override string GetPoint0()
	{
		return Singleton<StringManager>.Instance.GetString("summonLvl") + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint0Value()
	{
		if (this.mBaseScene.setIn)
		{
			return this.mPetData.Data.Level.ToString();
		}
		return "1";
	}

	protected override string GetPoint1()
	{
		if (this.mBaseScene.setIn)
		{
			return base.GetPoint1();
		}
		return Singleton<StringManager>.Instance.GetString("subQuality") + Singleton<StringManager>.Instance.GetString("Colon0");
	}

	protected override string GetPoint1Value()
	{
		if (this.mBaseScene.setIn)
		{
			return base.GetPoint1();
		}
		return this.mPetData.Info.SubQuality.ToString();
	}

	protected override void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ShowPetInfoSceneV2(this.mPetData, 0, null, 2);
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mBaseScene.setIn)
		{
			this.mBaseScene.AddInPet(this.mPetData);
		}
		else
		{
			PetDataEx newPetByInfoID = Globals.Instance.Player.PetSystem.GetNewPetByInfoID(this.mPetData.Info.ID);
			if (newPetByInfoID == null)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("Mirror2", 0f, 0f);
				return;
			}
			this.mBaseScene.AddOutPet(newPetByInfoID);
		}
	}

	protected override string GetClickableBtnTxt()
	{
		return Singleton<StringManager>.Instance.GetString("recycle35");
	}

	protected override bool EnableClickableBtn()
	{
		if (this.mBaseScene.setIn)
		{
			return !this.mPetData.IsBattling() && !this.mPetData.IsPetAssisting();
		}
		return Tools.HasNewPet(this.mPetData.Info.ID);
	}

	protected override bool ShowIconGray()
	{
		if (this.mBaseScene.setIn)
		{
			return this.mPetData.IsBattling() || this.mPetData.IsPetAssisting();
		}
		return !Tools.HasNewPet(this.mPetData.Info.ID);
	}

	protected override bool ShowStars()
	{
		return this.mBaseScene.setIn;
	}

	protected override int GetStarsNum()
	{
		uint num = 0u;
		return (int)Tools.GetPetStarAndLvl(this.mPetData.Data.Awake, out num);
	}
}
