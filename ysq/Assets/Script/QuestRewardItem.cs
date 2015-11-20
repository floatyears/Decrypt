using Att;
using System;
using UnityEngine;

public class QuestRewardItem : MonoBehaviour
{
	private ItemInfo itemInfo;

	public static QuestRewardItem CreateReward(ItemInfo info, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardItem");
		if (gameObject != null)
		{
			QuestRewardItem questRewardItem = gameObject.AddComponent<QuestRewardItem>();
			if (questRewardItem != null)
			{
				questRewardItem.Init(info, num);
				return questRewardItem;
			}
		}
		return null;
	}

	private void Init(ItemInfo info, int num)
	{
		if (info.Type == 3 && info.SubType == 0)
		{
			global::Debug.LogErrorFormat("Use reward type error {0}", new object[]
			{
				(EItemType)info.Type
			});
			base.gameObject.SetActive(false);
			return;
		}
		this.itemInfo = info;
		UISprite component = base.transform.Find("Quality").GetComponent<UISprite>();
		component.spriteName = Tools.GetItemQualityIcon(this.itemInfo.Quality);
		UISprite component2 = base.GetComponent<UISprite>();
		component2.spriteName = this.itemInfo.Icon;
		Transform transform = base.transform.Find("Flag");
		transform.gameObject.SetActive(info.Type == 3);
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		uILabel.text = string.Format("{0}{1}[-] [FFFFFF]x{2}", Tools.GetItemQualityColorHex(this.itemInfo.Quality), this.itemInfo.Name, num);
	}
}
