using Att;
using System;
using UnityEngine;

public class QuestRewardPet : MonoBehaviour
{
	private PetInfo petInfo;

	public static QuestRewardPet CreateReward(PetInfo info, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardPet");
		if (gameObject != null)
		{
			QuestRewardPet questRewardPet = gameObject.AddComponent<QuestRewardPet>();
			if (questRewardPet != null)
			{
				questRewardPet.Init(info, num);
				return questRewardPet;
			}
		}
		return null;
	}

	private void Init(PetInfo info, int num)
	{
		this.petInfo = info;
		if (this.petInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = this.petInfo.Icon;
		UISprite uISprite = GameUITools.FindUISprite("Quality", base.gameObject);
		uISprite.spriteName = Tools.GetItemQualityIcon(this.petInfo.Quality);
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		if (num > 1)
		{
			uILabel.text = string.Format("{0}{1}[-] [FFFFFF]x{2}", Tools.GetItemQualityColorHex(this.petInfo.Quality), this.petInfo.Name, num);
		}
		else
		{
			uILabel.text = Tools.GetPetName(this.petInfo);
			uILabel.color = Tools.GetItemQualityColor(this.petInfo.Quality);
		}
	}
}
