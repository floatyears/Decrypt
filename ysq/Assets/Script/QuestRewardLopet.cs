using Att;
using System;
using UnityEngine;

public class QuestRewardLopet : MonoBehaviour
{
	private LopetInfo petInfo;

	public static QuestRewardLopet CreateReward(LopetInfo info, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardLopet");
		if (gameObject != null)
		{
			QuestRewardLopet questRewardLopet = gameObject.AddComponent<QuestRewardLopet>();
			if (questRewardLopet != null)
			{
				questRewardLopet.Init(info, num);
				return questRewardLopet;
			}
		}
		return null;
	}

	private void Init(LopetInfo info, int num)
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
			uILabel.text = this.petInfo.Name;
			uILabel.color = Tools.GetItemQualityColor(this.petInfo.Quality);
		}
	}
}
