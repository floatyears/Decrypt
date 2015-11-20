using Att;
using Proto;
using System;
using UnityEngine;

public class PartnerFightBagItem : UICustomGridItem
{
	private GUIPartnerFightScene mBaseScene;

	public PetDataEx mPetData;

	private UISprite mParBg;

	private UISprite mPar;

	private UISprite mSkillBg;

	private UISprite mMarkBg;

	private UILabel mMarkTxt;

	public UILabel mFightTxt;

	private UILabel mLvText;

	private UILabel mLvTextNum;

	private UILabel mLvNum;

	private UILabel mZiZhi;

	private UILabel mZiZhiNum;

	private UILabel mPartnerName;

	private UILabel mdef;

	private UILabel mAct;

	private UILabel mFurther;

	private PetInfo mInfo;

	private string tempStr;

	private int tempInt;

	private PetDataEx tempPetDataEx;

	public void InitItemData(GUIPartnerFightScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mParBg = base.transform.Find("parBg").GetComponent<UISprite>();
		this.mPar = this.mParBg.transform.Find("par").GetComponent<UISprite>();
		this.mMarkBg = this.mParBg.transform.Find("markBg").GetComponent<UISprite>();
		this.mMarkTxt = this.mMarkBg.transform.Find("txt").GetComponent<UILabel>();
		this.mZiZhi = base.transform.Find("zizhi").GetComponent<UILabel>();
		this.mZiZhiNum = base.transform.Find("ziZhiNum").GetComponent<UILabel>();
		this.mPartnerName = base.transform.Find("name").GetComponent<UILabel>();
		this.mSkillBg = base.transform.Find("skillBg").GetComponent<UISprite>();
		this.mdef = this.mSkillBg.transform.Find("def").GetComponent<UILabel>();
		this.mLvText = this.mSkillBg.transform.Find("Lv").GetComponent<UILabel>();
		this.mLvTextNum = this.mSkillBg.transform.Find("LvNum").GetComponent<UILabel>();
		this.mLvNum = this.mParBg.transform.Find("Lv").GetComponent<UILabel>();
		this.mAct = this.mSkillBg.transform.Find("act").GetComponent<UILabel>();
		this.mFightTxt = base.transform.FindChild("fightBtn/fightTxt").GetComponent<UILabel>();
		GameUITools.RegisterClickEvent("fightBtn", new UIEventListener.VoidDelegate(this.OnFightBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("parBg", new UIEventListener.VoidDelegate(this.OnParBtnClick), base.gameObject);
	}

	private void OnParBtnClick(GameObject go)
	{
		GameUIManager.mInstance.ShowPetInfoSceneV2(this.mPetData, 0, null, 0);
	}

	public void OnFightBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mBaseScene.mSlot == -1)
		{
			MC2S_PetReborn mC2S_PetReborn = new MC2S_PetReborn();
			mC2S_PetReborn.PetID = this.mPetData.GetID();
			GameUIManager.mInstance.uiState.PetRebornData = mC2S_PetReborn;
			this.mBaseScene.OnBackClick(null);
		}
		else
		{
			MC2S_SetCombatPet mC2S_SetCombatPet = new MC2S_SetCombatPet();
			mC2S_SetCombatPet.Slot = this.mBaseScene.mSlot;
			mC2S_SetCombatPet.PetID = this.mPetData.GetID();
			Globals.Instance.CliSession.Send(195, mC2S_SetCombatPet);
		}
	}

	public override void Refresh(object data)
	{
		if (data == null || this.mPetData == data)
		{
			return;
		}
		this.mPetData = (PetDataEx)data;
		this.mPartnerName.text = Singleton<StringManager>.Instance.GetString("PetName", new object[]
		{
			(this.mPetData.GetSocketSlot() != 0) ? Tools.GetPetName(this.mPetData.Info) : Globals.Instance.Player.Data.Name,
			(this.mPetData.Data.Further <= 0u) ? null : Singleton<StringManager>.Instance.GetString("PetAdvance", new object[]
			{
				this.mPetData.Data.Further
			})
		});
		this.mPartnerName.color = Tools.GetItemQualityColor(this.mPetData.Info.Quality);
		this.mdef.text = Singleton<StringManager>.Instance.GetString(string.Format("petType{0}", this.mPetData.Info.Type));
		this.mPar.spriteName = this.mPetData.Info.Icon;
		this.mParBg.spriteName = Tools.GetItemQualityIcon(this.mPetData.Info.Quality);
		if (this.mPetData.Data.Level > 0u)
		{
			this.mLvText.gameObject.SetActive(true);
			this.mLvNum.gameObject.SetActive(true);
			this.mLvTextNum.gameObject.SetActive(true);
			this.mLvText.text = Singleton<StringManager>.Instance.GetString("PetLevelText");
			this.mLvTextNum.text = Singleton<StringManager>.Instance.GetString("PetLevelTextNum", new object[]
			{
				this.mPetData.Data.Level
			});
			this.mLvNum.text = Singleton<StringManager>.Instance.GetString("PetLevelNum", new object[]
			{
				this.mPetData.Data.Level
			});
		}
		else
		{
			this.mLvText.gameObject.SetActive(false);
			this.mLvTextNum.gameObject.SetActive(false);
			this.mLvNum.gameObject.SetActive(false);
		}
		this.tempInt = this.mPetData.Info.SubQuality;
		if (this.tempInt > 0)
		{
			this.mZiZhi.gameObject.SetActive(true);
			this.mZiZhiNum.gameObject.SetActive(true);
			this.mZiZhi.text = Singleton<StringManager>.Instance.GetString("PetSubQualityTxt");
			this.mZiZhiNum.text = Singleton<StringManager>.Instance.GetString("PetSubQualityNum", new object[]
			{
				this.mPetData.Info.SubQuality
			});
		}
		else
		{
			this.mZiZhi.gameObject.SetActive(false);
			this.mZiZhiNum.gameObject.SetActive(false);
		}
		if (this.mBaseScene.mSlot == -1)
		{
			this.mFightTxt.text = Singleton<StringManager>.Instance.GetString("recycle35");
			this.mAct.gameObject.SetActive(false);
		}
		else
		{
			this.mFightTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther2");
			this.tempInt = this.mPetData.Relation;
			if (this.tempInt > 0)
			{
				this.mAct.gameObject.SetActive(true);
				this.mAct.text = Singleton<StringManager>.Instance.GetString("PetActivateLuck", new object[]
				{
					this.mPetData.Relation
				});
			}
			else
			{
				this.mAct.gameObject.SetActive(false);
			}
		}
		if (this.mPetData.IsPetBattling())
		{
			this.mMarkBg.gameObject.SetActive(true);
			this.mMarkTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther6");
			this.mMarkBg.color = new Color(255f, 255f, 255f);
		}
		else if (this.mPetData.IsPetAssisting())
		{
			this.mMarkBg.gameObject.SetActive(true);
			this.mMarkTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther8");
			this.mMarkBg.color = new Color(0f, 0f, 0f);
		}
		else
		{
			this.mMarkBg.gameObject.SetActive(false);
		}
	}
}
