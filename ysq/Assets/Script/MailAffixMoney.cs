using Proto;
using System;

public class MailAffixMoney : MailContentElementBase
{
	private UISprite mMoneyIcon;

	private UILabel mMoneyNum;

	private void CreateObjects()
	{
		this.mMoneyIcon = base.transform.GetComponent<UISprite>();
		this.mMoneyNum = base.transform.Find("affixTxt").GetComponent<UILabel>();
	}

	public void InitMoneyInfo(EAffixType moneyType, int moneyNum)
	{
		if (this.mMoneyIcon == null)
		{
			this.CreateObjects();
		}
		switch (moneyType)
		{
		case EAffixType.EAffix_Money:
			this.mMoneyIcon.spriteName = "Gold_1";
			goto IL_1A7;
		case EAffixType.EAffix_Diamond:
			this.mMoneyIcon.spriteName = "redGem_1";
			goto IL_1A7;
		case EAffixType.EAffix_Honor:
			this.mMoneyIcon.spriteName = "Honor_1";
			goto IL_1A7;
		case EAffixType.EAffix_Reputation:
			this.mMoneyIcon.spriteName = "Guild_1";
			goto IL_1A7;
		case EAffixType.EAffix_Energy:
			this.mMoneyIcon.spriteName = "key_1";
			goto IL_1A7;
		case EAffixType.EAffix_Exp:
			this.mMoneyIcon.spriteName = "exp_1";
			goto IL_1A7;
		case EAffixType.EAffix_MagicCrystal:
			this.mMoneyIcon.spriteName = "magicCrystal";
			goto IL_1A7;
		case EAffixType.EAffix_MagicSoul:
			this.mMoneyIcon.spriteName = "magicSoul";
			goto IL_1A7;
		case EAffixType.EAffix_FireDragonScale:
			this.mMoneyIcon.spriteName = "FireDragon";
			goto IL_1A7;
		case EAffixType.EAffix_KingMedal:
			this.mMoneyIcon.spriteName = "KingMedal_1";
			goto IL_1A7;
		case EAffixType.EAffix_StarSoul:
			this.mMoneyIcon.spriteName = "starSoul";
			goto IL_1A7;
		case EAffixType.EAffix_Emblem:
			this.mMoneyIcon.spriteName = "emblem";
			goto IL_1A7;
		case EAffixType.EAffix_LopetSoul:
			this.mMoneyIcon.spriteName = "lopetSoul";
			goto IL_1A7;
		case EAffixType.EAffix_FestivalVoucher:
			this.mMoneyIcon.spriteName = "festivalVoucher";
			goto IL_1A7;
		}
		this.mMoneyIcon.spriteName = string.Empty;
		IL_1A7:
		this.mMoneyNum.text = Tools.FormatCurrency(moneyNum);
	}
}
