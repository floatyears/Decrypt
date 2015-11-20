using Att;
using System;
using UnityEngine;

public class QuestRewardFashion : MonoBehaviour
{
	private FashionInfo fashionInfo;

	public static QuestRewardFashion CreateReward(FashionInfo info, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardFashion");
		if (gameObject != null)
		{
			QuestRewardFashion questRewardFashion = gameObject.AddComponent<QuestRewardFashion>();
			if (questRewardFashion != null)
			{
				questRewardFashion.Init(info, num);
				return questRewardFashion;
			}
		}
		return null;
	}

	private void Init(FashionInfo info, int num)
	{
		this.fashionInfo = info;
		if (this.fashionInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = this.fashionInfo.Icon;
		UISprite uISprite = GameUITools.FindUISprite("Quality", base.gameObject);
		uISprite.spriteName = Tools.GetItemQualityIcon(this.fashionInfo.Quality);
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		if (num > 1)
		{
			uILabel.text = string.Format("{0}{1}[-] [FFFFFF]x{2}", Tools.GetItemQualityColorHex(this.fashionInfo.Quality), this.fashionInfo.Name, num);
		}
		else
		{
			uILabel.text = this.fashionInfo.Name;
			uILabel.color = Tools.GetItemQualityColor(this.fashionInfo.Quality);
		}
	}
}
