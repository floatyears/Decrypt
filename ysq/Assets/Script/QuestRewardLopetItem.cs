using Att;
using System;
using UnityEngine;

public class QuestRewardLopetItem : MonoBehaviour
{
	private ItemInfo itemInfo;

	private LopetInfo lopetInfo;

	public static QuestRewardLopetItem CreateReward(ItemInfo info, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardLopetItem");
		if (gameObject != null)
		{
			QuestRewardLopetItem questRewardLopetItem = gameObject.AddComponent<QuestRewardLopetItem>();
			if (questRewardLopetItem != null)
			{
				questRewardLopetItem.Init(info, num);
				return questRewardLopetItem;
			}
		}
		return null;
	}

	private void Init(ItemInfo info, int num)
	{
		if (info.Type != 3 || info.SubType != 3)
		{
			global::Debug.LogErrorFormat("Use reward type error {0}", new object[]
			{
				(EItemType)info.Type
			});
			base.gameObject.SetActive(false);
			return;
		}
		this.itemInfo = info;
		this.lopetInfo = Globals.Instance.AttDB.LopetDict.GetInfo(this.itemInfo.Value2);
		if (this.lopetInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("LopetDict.GetInfo, ID = {0}", this.itemInfo.Value2)
			});
			base.gameObject.SetActive(false);
			return;
		}
		UISprite uISprite = GameUITools.FindUISprite("Quality", base.gameObject);
		uISprite.spriteName = Tools.GetItemQualityIcon(this.lopetInfo.Quality);
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = this.lopetInfo.Icon;
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		uILabel.text = string.Format("{0}{1}[-] [FFFFFF]x{2}", Tools.GetItemQualityColorHex(this.lopetInfo.Quality), this.itemInfo.Name, num);
	}
}
